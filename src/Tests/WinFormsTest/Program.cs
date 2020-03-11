using System;
using WinFormsTest.Core.IOC;
using System.Windows.Forms;
using Ninject;
using WinFormsTest.Presenters;

namespace WinFormsTest
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var iocConf = new NinjectConfigure();
            var kernel = new StandardKernel(iocConf);
            var appFactory = new NinjectAppFactory(kernel);

            appFactory.Run<MainPresenter>();
        }
    }
}
