using WinFormsTest.Core.Interfaces.IPresenters;
using WinFormsTest.Core.Interfaces;
using Ninject;

namespace WinFormsTest.Core.IOC
{
    public class NinjectAppFactory : IAppFactory
    {
        private readonly IKernel Kernel;

        public NinjectAppFactory(IKernel Kernel) 
        {
            this.Kernel = Kernel;
            this.Kernel.Bind<IAppFactory>().ToMethod(c => this);
        }

        public void Run<TPresenter>() where TPresenter : class, IPresenter
        {
            var presenter = Kernel.Get<TPresenter>();
            presenter.Run();
        }

        public void Run<TPresenter, TArgumnent>(TArgumnent Argumnent) where TPresenter : class, IPresenter<TArgumnent>
        {
            var presenter = Kernel.Get<TPresenter>();
        }
    }
}
