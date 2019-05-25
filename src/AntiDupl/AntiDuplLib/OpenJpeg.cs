using System;
using System.IO;
using System.Runtime.InteropServices;

namespace AntiDuplLib
{
    internal static class OpenJpeg
    {
        private const string LibraryFileName = "OpenJpeg.dll";

        static OpenJpeg()
        {
            var path = new Uri(typeof(OpenJpeg).Assembly.CodeBase).LocalPath;
            var folder = Path.GetDirectoryName(path);

            var subFolder = Environment.Is64BitProcess ? "x64" : "x86";

            NativeMethods.LoadLibrary(Path.Combine(folder, subFolder, LibraryFileName));
        }

        public static string Version()
        {
            var ptr = NativeMethods.Version();
            var convertedString = Marshal.PtrToStringAnsi(ptr);
            return convertedString;
        }

        private static class NativeMethods
        {
            [DllImport("kernel32.dll")]
            public static extern IntPtr LoadLibrary(string dllToLoad);

            [DllImport(LibraryFileName, EntryPoint = "opj_version")]
            public static extern IntPtr Version();
        }
    }
}
