﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WorkInOrder.Commands {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Messages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Messages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WorkInOrder.Commands.Messages", typeof(Messages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot activate {0} as there&apos;s another active task: {1}. To switch active task use the switch command..
        /// </summary>
        internal static string AnotherTaskAlreadyActive {
            get {
                return ResourceManager.GetString("AnotherTaskAlreadyActive", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Missing task description.
        /// </summary>
        internal static string MissingDescription {
            get {
                return ResourceManager.GetString("MissingDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to More than one task found when looking for {0}, you can pick one from list below using its number or more specific name:.
        /// </summary>
        internal static string MoreThanOneTaskFound {
            get {
                return ResourceManager.GetString("MoreThanOneTaskFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Nothing to display.
        /// </summary>
        internal static string NothingToDisplay {
            get {
                return ResourceManager.GetString("NothingToDisplay", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} is now active.
        /// </summary>
        internal static string TaskActivated {
            get {
                return ResourceManager.GetString("TaskActivated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} has been added to current todo list.
        /// </summary>
        internal static string TaskAdded {
            get {
                return ResourceManager.GetString("TaskAdded", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} is already active.
        /// </summary>
        internal static string TaskAlreadyActive {
            get {
                return ResourceManager.GetString("TaskAlreadyActive", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Task {0} already exists.
        /// </summary>
        internal static string TaskAlreadyExists {
            get {
                return ResourceManager.GetString("TaskAlreadyExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} not found.
        /// </summary>
        internal static string TaskNotFound {
            get {
                return ResourceManager.GetString("TaskNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unknown command: {0}.
        /// </summary>
        internal static string UnknownCommand {
            get {
                return ResourceManager.GetString("UnknownCommand", resourceCulture);
            }
        }
    }
}
