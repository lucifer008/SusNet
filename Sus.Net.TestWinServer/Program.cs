using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sus.Net.TestWinServer
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                var log4netFileInfo = new FileInfo("Config/log4net.cfg.xml");
                XmlConfigurator.Configure(log4netFileInfo);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmServer());
            }
            catch (Exception ex)
            {
                log4net.LogManager.GetLogger(typeof(Program)).Error(ex); ;
                MessageBox.Show(ex.Message);
            }
        }
    }
}
