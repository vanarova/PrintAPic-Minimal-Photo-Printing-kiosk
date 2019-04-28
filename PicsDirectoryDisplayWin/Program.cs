using NLog;
using PicsDirectoryDisplayWin.lib_ImgIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicsDirectoryDisplayWin
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            //ConfigurationManager.AppSettings.Add("Bill Info", "Billing Info --");
            //Application.ApplicationExit += Application_ApplicationExit;
            // Application.ThreadExit += Application_ThreadExit;
            //Application.

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            var config = new NLog.Config.LoggingConfiguration();

            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "log.txt" };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            NLog.LogManager.Configuration = config;
            NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new Animation());
            }
            catch (Exception e)
            {
                logger.Log(NLog.LogLevel.Error, e.Message);
            }
            
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Log(NLog.LogLevel.Error, ((Exception)e.ExceptionObject).Message);
        }
    }
}
