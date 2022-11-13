using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TagCloud2
{
    public static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            //ApplicationConfiguration.Initialize();
            //AllocConsole();
            Application.Run(new Form1());
        }
        
        // [DllImport("kernel32.dll",   
        //     EntryPoint = "AllocConsole",   
        //     SetLastError = true,   
        //     CharSet = CharSet.Auto,   
        //     CallingConvention = CallingConvention.StdCall)]   
        // private static extern int AllocConsole(); 
    }
}