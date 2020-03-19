using WinFormsTest.Core.Interfaces;
using WinFormsTest.Core.Interfaces.IViews;
using WinFormsTest.Core.Services;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;
using WinFormsTest.Models;

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

        private Task View_Start(Type PType) => Task.Run(() =>
        {
            if (!FrameService.ContainsType(PType))
                return;

            var attr = new FrameElementArg();
            attr.Log += Log;

            Controller.Run<FrameElementArg>(PType, attr);
        });

        private void Log(Type PType, string LogText) 
        {
            var attrInst = FrameService.GetFrameElementAttributeForType(PType);
            LogText = $"{DateTime.Now:dd.MM.yyyy_hh:mm:ss} \"{attrInst.FrameName}\" =>{Environment.NewLine}{LogText}{Environment.NewLine}{Environment.NewLine}";
            View.Log(LogText, attrInst.HeaderColor);
        }

        public override void Run() 
        {
            var values = FrameService.Select(x => Tuple.Create(x.Key, x.Value.FrameName)).ToList();
            View.BuildTests(values);

            base.Run();
        }
    }
}
