using System;
using System.Windows.Forms;

namespace TicketApplicationSystem
{
    static class Program
    {
        /// <summary>
        ///   This is v 1.2
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}