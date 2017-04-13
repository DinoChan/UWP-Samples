using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        static Form1()
        {
            Application.ThreadException += Application_ThreadException;
            //Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        public Form1()
        {
            InitializeComponent();
            Test t = new Test();
            t.CloseSopMessageDialog();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(GetExceptionMessage(e.ExceptionObject as Exception));
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            MessageBox.Show(GetExceptionMessage(e.Exception));
        }

        private static string GetExceptionMessage(Exception ex)
        {
            string result = "";
            var exception = ex;
            while (ex != null)
            {
                result += ex.Message;
                result += Environment.NewLine;
                result += ex.StackTrace;
                result += Environment.NewLine;
                ex = ex.InnerException;
            }
            return result;
        }
    }

    public class Test
    {
        [DllImport("user32.dll")]
        public static extern int FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern int SendMessage(int hWnd, uint Msg, int wParam, int lParam);

        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_CLOSE = 0xF060;

        public void CloseSopMessageDialog()
        {
            Thread thread = new Thread(new ThreadStart(CloseSopMessageDialogCore));
            thread.IsBackground = true;
            thread.Start();


        }

        private void CloseSopMessageDialogCore()
        {
            int count = 0;
            while (count < 10)
            {
                int iHandle = FindWindow("Notepad", "Topsizer_XMLFile.TXT - 记事本");
                if (iHandle > 0)
                {
                    // close the window using API        
                    SendMessage(iHandle, WM_SYSCOMMAND, SC_CLOSE, 0);
                    break;
                }
                count++;
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }
    }
}
