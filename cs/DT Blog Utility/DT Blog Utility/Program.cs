using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DT_Blog_Utility
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //var proc = Process.GetProcesses()
            //if(Form1.Instance != null && args != null && args.Length > 0)
            //{
            //    Form1.Instance.OpenImage(args[0]);
                
            //    // make a singleton app by closing this
            //    proc.Kill();
            //    return;
            //}

            if(true)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1(args));
            }
            else
            {
                //Application.
                //Form1.Instance.OpenImage(args[0]);
            }

            
        }
    }
}
