﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WMBusinessTools.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WMBusinessTools.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to Error.
        /// </summary>
        internal static string InitializationForm_mBackgroundWorker_RunWorkerCompleted_Error {
            get {
                return ResourceManager.GetString("InitializationForm_mBackgroundWorker_RunWorkerCompleted_Error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &amp;Finish.
        /// </summary>
        internal static string InitializationForm_precompileRadioButton_CheckedChanged__Finish {
            get {
                return ResourceManager.GetString("InitializationForm_precompileRadioButton_CheckedChanged__Finish", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &amp;Next &gt;.
        /// </summary>
        internal static string InitializationForm_precompileRadioButton_CheckedChanged__Next__ {
            get {
                return ResourceManager.GetString("InitializationForm_precompileRadioButton_CheckedChanged__Next__", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Precompilation error!.
        /// </summary>
        internal static string PrecompilationException_PrecompilationException_Precompilation_error_ {
            get {
                return ResourceManager.GetString("PrecompilationException_PrecompilationException_Precompilation_error_", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SET NETFX=%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319
        ///IF NOT EXIST %NETFX% SET NETFX=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319
        ///SET NGEN=%NETFX%\ngen.exe
        ///&quot;%NGEN%&quot; install &quot;{0}&quot;.
        /// </summary>
        internal static string PrecompileBatchScriptTemplate {
            get {
                return ResourceManager.GetString("PrecompileBatchScriptTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error occurred while attempting to create the new session!.
        /// </summary>
        internal static string Program_Main_An_error_occurred_while_attempting_to_create_the_new_session_ {
            get {
                return ResourceManager.GetString("Program_Main_An_error_occurred_while_attempting_to_create_the_new_session_", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error occurred while trying to load the extension manager!.
        /// </summary>
        internal static string Program_Main_An_error_occurred_while_trying_to_load_the_extension_manager_ {
            get {
                return ResourceManager.GetString("Program_Main_An_error_occurred_while_trying_to_load_the_extension_manager_", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error.
        /// </summary>
        internal static string Program_Main_Error {
            get {
                return ResourceManager.GetString("Program_Main_Error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap RunAsAdministrator {
            get {
                object obj = ResourceManager.GetObject("RunAsAdministrator", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to https://www.marketkernel.com/registration/{0}.
        /// </summary>
        internal static string SuccessUrlTemplate {
            get {
                return ResourceManager.GetString("SuccessUrlTemplate", resourceCulture);
            }
        }
    }
}
