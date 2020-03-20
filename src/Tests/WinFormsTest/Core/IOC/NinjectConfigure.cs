using Ninject.Modules;
using WinFormsTest.Core.Interfaces.IViews;
using WinFormsTest.Core.Services;
using WinFormsTest.Presenters;
using WinFormsTest.Views;

namespace WinFormsTest.Core.IOC
{
    public class NinjectConfigure : NinjectModule
    {
        public override void Load()
        {
            Bind<ImainForm>().To<MainForm>();
            Bind<IMatrixForm>().To<MatrixTestForm>();

            Bind<MainPresenter>().To<MainPresenter>();
            Bind<MatrixTestPresenter>().To<MatrixTestPresenter>();

            Bind<FrameElementService>().To<FrameElementService>();
        }
    }
}
