using WinFormsTest.Core.Interfaces.IPresenters;

namespace WinFormsTest.Core.Interfaces
{
    public interface IAppFactory
    {
        void Run<TPresenter>()
            where TPresenter : class, IPresenter;

        void Run<TPresenter, TArgumnent>(TArgumnent argumnent)
            where TPresenter : class, IPresenter<TArgumnent>;
    }
}
