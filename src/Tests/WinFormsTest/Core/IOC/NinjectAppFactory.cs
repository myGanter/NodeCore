using WinFormsTest.Core.Interfaces.IPresenters;
using WinFormsTest.Core.Interfaces;
using System;
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

        public void Run(Type PType) 
        {
            var opresenter = Kernel.Get(PType);
            var tpresenter = opresenter as IPresenter;

            if (tpresenter == null)
                throw new Exception(PType.ToString() + " != IPresenter");

            tpresenter.Run();
        }

        public void Run<TArgumnent>(Type PType, TArgumnent Argumnent)
        {
            var opresenter = Kernel.Get(PType);
            var tpresenter = opresenter as IPresenter<TArgumnent>;

            if (tpresenter == null)
                throw new Exception(PType.ToString() + $" != IPresenter<{typeof(TArgumnent).ToString()}>");

            tpresenter.Run(Argumnent);
        }

        public void Run<TPresenter>() where TPresenter : class, IPresenter
        {
            var presenter = Kernel.Get<TPresenter>();
            presenter.Run();
        }

        public void Run<TPresenter, TArgumnent>(TArgumnent Argumnent) where TPresenter : class, IPresenter<TArgumnent>
        {
            var presenter = Kernel.Get<TPresenter>();
            presenter.Run(Argumnent);
        }
    }
}
