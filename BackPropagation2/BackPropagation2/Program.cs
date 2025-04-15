using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace BackPropagation2
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new BackProgagationForm());
        }

        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();

        public static List<Pattern> TrainingSet = new List<Pattern>();
        public static int MaxIteration;
    }
}
