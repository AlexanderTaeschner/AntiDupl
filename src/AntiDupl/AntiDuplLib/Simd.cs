using System;
using System.IO;
using System.Runtime.InteropServices;

namespace AntiDuplLib
{
    internal static class Simd
    {
        private const string LibraryFileName = "Simd.dll";

        static Simd()
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

            [DllImport(LibraryFileName, EntryPoint = "SimdVersion")]
            public static extern IntPtr Version();
        }
    }
}
