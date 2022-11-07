using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TagCloud
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AllocConsole();
            //new DataParser("data.txt");
            //Console.ReadKey();
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            Application.Run(new Form1());
        }
        
        [DllImport("kernel32.dll",   
            EntryPoint = "AllocConsole",   
            SetLastError = true,   
            CharSet = CharSet.Auto,   
            CallingConvention = CallingConvention.StdCall)]   
        private static extern int AllocConsole();   
    }
}