using System;
using TEditor.Abstractions;

namespace TEditor
{
    public class CrossTEditor
    {
        private static ITEditor Implementation;

        /// <summary>
        /// Current settings to use
        /// </summary>
        public static ITEditor Current
        {
            get
            {
                var ret = Implementation;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
        }

        public static void CreateTEditor(ITEditor implementation)
        {
            Implementation = implementation;
        }

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        }


        /// <summary>
        /// Dispose of everything 
        /// </summary>
        public static void Dispose()
        {
            Implementation?.Dispose();
        }

        public static string PageTitle { get; set; } = "HTML Editor";
        public static string SaveText { get; set; } = "Save";
        public static string CancelText { get; set; } = "Cancel";
    }
}