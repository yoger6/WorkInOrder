using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Data.Sqlite;


namespace WorkInOrder
{
    public class TaskStorage : ITaskStorage
    {
        private readonly string _connectionString;

        public TaskStorage(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Create(DateTime createdOn, string content, Status status = Status.Pending)
        {
            try
            {
                RunNonQuery("INSERT INTO Tasks (Content, CreatedOn, Status) VALUES (@Content, @CreatedOn, @Status)", 
                    new SqliteParameter("@Content", content), 
                    new SqliteParameter("@CreatedOn", createdOn), 
                    new SqliteParameter("@Status", status));
            }
            catch (SqliteException e) when (e.SqliteErrorCode == 19)
            {
                throw new TaskAlreadyExistsException();
            }
        }
        
        public ITask[] GetAll()
        {
            const string commandText = @"SELECT Content, Status, CreatedOn, CompletedOn FROM Tasks;";
            return RunReader(commandText, Read).ToArray();
        }

        private ITask Read(SqliteDataReader reader)
        {
            return new Task(
                DateTime.Parse((string) reader[2]),
                (string) reader[0],
                (Status) Enum.Parse(typeof(Status), 
                (string) reader[1]),
                reader.IsDBNull(3) ? (DateTime?) null : DateTime.Parse((string) reader[3]),
                this);
        }

        public void UpdateStatus(string name, Status status)
        {
            if (!DoesItExist())
            {
                throw new TaskNotFoundException(name);
            }

            RunNonQuery("UPDATE Tasks SET Status = @Status WHERE Content = @Content", new SqliteParameter("@Status", status), new SqliteParameter("@Content", name));

            bool DoesItExist()
            {
                return RunScalar("SELECT 1 FROM Tasks WHERE Content = @Content", new SqliteParameter("@Content", name)) != null;
            }
        }

        public ITask Find(Status status)
        {
            var results = RunReader(
                "SELECT Content, Status, CreatedOn, CompletedOn FROM Tasks WHERE Status = @Status;",
                Read,
                new SqliteParameter("@Status", status)
                ).ToArray();

            return results.SingleOrDefault();
        }

        public ITask FindFirstAvailableSince(DateTime since)
        {
            var results = RunReader(
                "SELECT Content, Status, CreatedOn, CompletedOn FROM Tasks WHERE Status IN (@PendingStatus, @SkippedStatus) AND CreatedOn >= @Since ORDER BY CreatedOn LIMIT 1;",
                Read,
                new SqliteParameter("@PendingStatus", Status.Pending),
                new SqliteParameter("@SkippedStatus", Status.Skipped),
                new SqliteParameter("@Since", since)
                ).ToArray();

            return results.SingleOrDefault();
        }

        public void UpdateCompletionDate(string content, DateTime date)
        {
            RunNonQuery(
                "UPDATE Tasks SET CompletedOn = @Date WHERE Content = @Content;", 
                new SqliteParameter("@Content", content), 
                new SqliteParameter("@Date", date));
        }

        /// <summary>
        /// Finds task with given name. Can be a partial match.
        /// </summary>
        /// <exception cref="NonUniqueNameException">When more than one task is found.</exception>
        public ITask Find(string phrase)
        {
            var results = RunReader(
                "SELECT Content, Status, CreatedOn, CompletedOn FROM Tasks WHERE Content LIKE @Phrase;", 
                Read,
                new SqliteParameter("@Phrase", "%" + phrase + "%")
                ).ToArray();

            if (results.Length > 1)
            {
                throw new NonUniqueNameException(results.Select(x => x.Name));
            }

            return results.SingleOrDefault();
        }

        public void Clear()
        {
            RunNonQuery("DELETE FROM Tasks;");
        }

        private IEnumerable<T> RunReader<T>(string commandText, Func<SqliteDataReader, T> transformer, params SqliteParameter[] parameters)
        {
            using (var connection = Connect())
            using (var command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = commandText;
                    command.Parameters.AddRange(parameters);
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            yield break;
                        }

                        while (reader.Read())
                        {
                            yield return transformer(reader);
                        }
                    }
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        private object RunScalar(string commandText, params SqliteParameter[] parameters)
        {
            using (var connection = Connect())
            using (var command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = commandText;
                    command.Parameters.AddRange(parameters);
                    return command.ExecuteScalar();
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        private void RunNonQuery(string commandText, params SqliteParameter[] parameters)
        {
            using (var connection = Connect())
            using (var command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = commandText;
                    command.Parameters.AddRange(parameters);
                    command.ExecuteNonQuery();
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        private SqliteConnection Connect()
        {
            var dbFilePath = _connectionString.Substring(11);
            var directoryPath = Path.GetDirectoryName(dbFilePath);

            if (!string.IsNullOrWhiteSpace(directoryPath) && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }


            if (!File.Exists(dbFilePath))
            {
                File.Create(dbFilePath).Dispose();
                RunNonQuery(@"
                            CREATE TABLE Tasks(
                               Content TEXT PRIMARY KEY NOT NULL,
                               CreatedOn DateTime NOT NULL,
                               Status TEXT NOT NULL,
                               CompletedOn DateTime NULL
                            );");
            }

            var connection = new SqliteConnection(_connectionString);
            connection.Open();
            
            return connection;
        }
    }
}