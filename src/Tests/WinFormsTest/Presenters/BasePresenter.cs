using WinFormsTest.Core.Interfaces;
using WinFormsTest.Core.Interfaces.IPresenters;
using WinFormsTest.Core.Interfaces.IViews;

namespace WinFormsTest.Presenters
{
    public abstract class BasePresenter<TView> : IPresenter where TView : IView
    {
        protected TView View { get; private set; }

        protected IAppFactory Controller { get; private set; }

        protected BasePresenter(IAppFactory Controller, TView View)
        {
            this.Controller = Controller;
            this.View = View;
        }

        public virtual void Run()
        {
            View.Show();
        }
    }

    public abstract class BasePresenter<TView, TArg> : IPresenter<TArg> where TView : IView
    {
        protected TView View { get; private set; }

        protected IAppFactory Controller { get; private set; }

        protected BasePresenter(IAppFactory Controller, TView View)
        {
            this.Controller = Controller;
            this.View = View;
        }

        public abstract void Run(TArg Argument);
    }
}
