using WinFormsTest.Core.Interfaces;
using WinFormsTest.Core.Interfaces.IViews;

namespace WinFormsTest.Presenters
{
    public class MainPresenter : BasePresenter<ImainForm>
    {
        public MainPresenter(IAppFactory Controller, ImainForm View) : base(Controller, View)
        {

        }
    }
}
