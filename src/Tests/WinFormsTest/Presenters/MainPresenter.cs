using WinFormsTest.Core.Interfaces;
using WinFormsTest.Core.Interfaces.IViews;
using WinFormsTest.Core.Services;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace WinFormsTest.Presenters
{
    public class MainPresenter : BasePresenter<ImainForm>
    {
        private readonly FrameElementService FrameService;

        public MainPresenter(FrameElementService FrameService, IAppFactory Controller, ImainForm View) : base(Controller, View)
        {
            this.FrameService = FrameService;

            View.Start += View_Start;
        }

        private Task View_Start(Type obj) => Task.Run(() =>
        {
            Thread.Sleep(5000);
        });

        public override void Run() 
        {
            var values = FrameService.Select(x => Tuple.Create(x.Key, x.Value.FrameName)).ToList();
            View.BuildTests(values);

            base.Run();
        }
    }
}
