using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using WinFormsTest.Models;
using WinFormsTest.Core.Attributes;
using WinFormsTest.Core.Interfaces.IPresenters;
using WinFormsTest.Core.Interfaces;
using WinFormsTest.Core.Services;
using WinFormsTest.UI;
using System.Windows.Forms;
using NodeCore.Base;
using NodeCore.Realization.Serialization;
using System.Linq;
using System.Threading;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WinFormsTest.Presenters
{
    [FrameElement("Random graph")]
    public class RandomGraphTestPresenters : IPresenter<FrameElementArg>
    {      
        public RandomGraphTestPresenters(IAppFactory Controller, ProcTimer ProcTimer, GraphService<string> GraphService) 
        {
            Rnd = new Random();
            this.ProcTimer = ProcTimer;
            this.GraphService = GraphService;
            PresenterType = GetType();
        }

        private event Action<Action> Invoke;

        private event Action<string> ExecLog;

        private readonly Random Rnd;

        private readonly ProcTimer ProcTimer;

        private readonly GraphService<string> GraphService;

        private readonly Type PresenterType;

        private const int NodeRadiusPx = 30;
        private const int Offset = NodeRadiusPx / 2;

        public void Run(FrameElementArg Arg)
        {
            ExecLog += (str) => Arg.LogInvoke(PresenterType, str);
            Invoke += Arg.InvokeInvoke;
            ProcTimer.Log += ExecLog;
            ExecLog("Start process");

            Task.Run(() =>
            {
                try 
                { 
                    ProcTimer.ProcTest("End process", StartProcess);                    
                } 
                catch (Exception e) 
                {
                    ExecLog($"[ERR] {e.Message}");
                }
            });
        }

        private void StartProcess() 
        {
            var inputDialog = InputForm.CreateForm("Settings", new InitModel(GraphService.GetNames()));
            if (inputDialog.ShowDialog() == DialogResult.OK) 
            {
                var settings = inputDialog.Model;

                if ((settings.T_Rnd_Or_F_Deseri && (
                    settings.NodeCount < 1 || 
                    settings.MaxEdjesInNode < 1 || 
                    settings.PicSize < 100 || 
                    settings.PicSize > 22000 || 
                    settings.MaxEdjesInNode > settings.NodeCount)) || !GraphService.ContainsMethod(settings.GraphType)) 
                {
                    ExecLog("[ERR] Invalid settings!");
                    return;
                }

                var graph = GraphService.CreateGraph(settings.GraphType, settings.GraphName);
                string fileName;

                if (settings.T_Rnd_Or_F_Deseri)
                {
                    ExecLog($"[+] Start random {settings.NodeCount} nodes");
                    ProcTimer.ProcTest(() => "Node length " + graph.NodeLength.ToString(), () => RandomNodes(graph, settings.NodeCount, settings.PicSize));

                    if (settings.SmartRandom)
                        ProcTimer.ProcTest("Edges smart random end", () => SmartRandomEdjes(graph, settings.MaxEdjesInNode));
                    else
                        ProcTimer.ProcTest("Edges random end", () => RandomEdjes(graph, settings.MaxEdjesInNode));

                    var sfd = new SaveFileDialog();
                    DialogResult sfdResult = DialogResult.Cancel;
                    Invoke(() => sfdResult = sfd.ShowDialog());
                    if (sfdResult != DialogResult.OK)
                        return;

                    fileName = sfd.FileName ?? "TestGraph";
                    using (var fs = File.Open(fileName + ".bin", FileMode.Create))
                        ProcTimer.ProcTest("Bin serialize to " + fileName, () => graph.SerializeToBinary(fs));
                    using (var fs = File.Open(fileName + ".xml", FileMode.Create))
                        ProcTimer.ProcTest("Xml serialize to " + fileName, () => graph.SerializeToXml(fs));                    
                }
                else 
                {
                    var ofd = new OpenFileDialog
                    {
                        Filter = "(*.bin;*.xml)|*.bin;*.xml"
                    };
                    DialogResult sfdResult = DialogResult.Cancel;
                    Invoke(() => sfdResult = ofd.ShowDialog());
                    if (sfdResult != DialogResult.OK)
                        return;

                    fileName = ofd.FileName ?? "TestGraph";
                    using (var fs = File.Open(fileName, FileMode.Open))
                    {
                        if (fileName.ToLower().EndsWith(".bin"))
                        {
                            ProcTimer.ProcTest("Binary deserialize", () => graph.BinaryDeserialize(fs));
                        }
                        else if (fileName.ToLower().EndsWith(".xml"))
                        {
                            ProcTimer.ProcTest("Xml deserialize", () => graph.XmlDeserialize(fs));
                        }
                        else
                        {
                            ExecLog("[ERR] File type is not a valid!");
                            return;
                        }
                    }

                    settings.NodeCount = graph.NodeLength;
                    settings.GraphName = graph.Name;
                }

                ExecLog("[+] Start create pic");
                Bitmap img = null; 
                ProcTimer.ProcTest(fileName + ".jpeg ", () => img = DrawGraph(graph, settings.NodeCount, fileName, settings.PicSize));

                var inputDialog2 = InputForm.CreateForm("Search", new SearchModel());
                do
                {
                    if (inputDialog2.ShowDialog() == DialogResult.OK)
                    {
                        var model = inputDialog2.Model;
                        if (!graph.NodeExist(model.Start) || !graph.NodeExist(model.Finish))
                        {
                            ExecLog("[ERR] Invalid settings!");
                            return;
                        }

                        var nodeProc = graph.CreateNodeProcessor();
                        List<INode<string>> puth = null;

                        ProcTimer.ProcTest($"Serch {model.Start} -> {model.Finish}", () => puth = nodeProc.SearchPath(graph[model.Start], graph[model.Finish]));
                        ProcTimer.ProcTest($"Draw puth {model.Start} -> {model.Finish}", () => DrawPuth((Bitmap)img.Clone(), puth, fileName + $"{model.Start} - {model.Finish}"));
                    }
                    else
                        return;
                }
                while (!inputDialog2.Model.Last);
            }
        }

        private void DrawPuth(Bitmap Img, List<INode<string>> Puth, string FileName) 
        {
            var nodeDiametrPx = NodeRadiusPx << 1;
            var g = Graphics.FromImage(Img);
            var bG = new SolidBrush(Color.Green);
            var pG = new Pen(bG, 2);
            var bG05 = new SolidBrush(Color.FromArgb(80, Color.Green));

            if (Puth.Count > 1) 
            {
                for (var i = 0; i < Puth.Count - 1; ++i) 
                {
                    var p1 = Puth[i].Point;
                    var x1 = p1.X;
                    var y1 = p1.Y;

                    var p2 = Puth[i + 1].Point;
                    var x2 = p2.X;
                    var y2 = p2.Y;

                    g.DrawLine(pG, x1, y1, x2, y2);
                }
            }

            for (var i = 0; i < Puth.Count; ++i) 
            {
                var p = Puth[i].Point;
                var x = p.X - NodeRadiusPx;
                var y = p.Y - NodeRadiusPx;

                g.FillEllipse(bG05, new Rectangle(x, y, nodeDiametrPx, nodeDiametrPx));
            }

            g.Dispose();
            Img.Save(FileName + ".jpeg", ImageFormat.Jpeg);
        }

        private Bitmap DrawGraph(IGraph<string> Graph, int Len, string FileName, int PicSize) 
        {
            var nodeDiametrPx = NodeRadiusPx << 1;

            var maxIndex = ((int)Math.Ceiling(Math.Sqrt(Len))) * 3;
            var maxCapacity = PicSize / nodeDiametrPx;

            if (maxCapacity < maxIndex)
                throw new Exception("Invalid settings (PicSize or NodeCount)!");
            

            var img = new Bitmap(PicSize, PicSize);

            var g = Graphics.FromImage(img);
            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.Black);

            var bW = new SolidBrush(Color.Aqua); 
            var bW05 = new SolidBrush(Color.FromArgb(200, Color.White));
            var bO05 = new SolidBrush(Color.FromArgb(200, Color.Red));
            var pW05 = new Pen(bW05, 1);
            var pO05 = new Pen(bO05, 2);
            var bO = Brushes.Black;
            var font = new Font("Consolas", 12);

            var circles = new List<Point>();

            foreach (var n in Graph) 
            {
                var p = n.Point;
                var x = p.X;
                var y = p.Y;

                foreach (var c in n)
                {
                    var chp = c.ChildNode.Point;
                    var chx = chp.X;
                    var chy = chp.Y;

                    if (c.Dependence == Dependence.Doubly) 
                    {
                        g.DrawLine(pW05, x, y, chx, chy);
                    }
                    else
                    { 
                        double nX = chx - x;
                        double nY = chy - y;
                        var vL = Math.Sqrt(nX * nX + nY * nY);
                        nX /= vL;
                        nY /= vL;
                        nX *= -40;
                        nY *= -40;
                        nX = chx + nX - 5;
                        nY = chy + nY - 5;
                        circles.Add(new Point((int)nX, (int)nY));

                        g.DrawLine(pO05, x, y, chx, chy);                        
                    }                    
                }
            }

            for (var i = 0; i < circles.Count; ++i) 
            {
                var p = circles[i];
                g.FillEllipse(bW, p.X, p.Y, 10, 10);
            }

            foreach (var i in Graph) 
            {
                var p = i.Point;
                var x = p.X - NodeRadiusPx;
                var y = p.Y - NodeRadiusPx;                
            
                g.FillEllipse(bW05, new Rectangle(x, y, nodeDiametrPx, nodeDiametrPx));
                g.DrawString(i.Name, font, bO, x, y + NodeRadiusPx - 6);
            }

            img.Save(FileName + ".jpeg", ImageFormat.Jpeg);

            g.Dispose();

            return img;
        }

        private void SmartRandomEdjes(IGraph<string> Graph, int MaxEdjesInNode) 
        {
            var nodeLength = Graph.NodeLength;
            var oneValProcent = 50d / nodeLength;
            var procentCounter = 0d;
            var taskBarCounter = 0;

            var cache = Graph.ToArray();

            foreach (var n in Graph) 
            {
                if (Rnd.Next(100) >= 10)
                    continue;

                var targetNode = cache[Rnd.Next(nodeLength)];
                var puthL = Rnd.Next(MaxEdjesInNode + 1);

                if (targetNode == n || targetNode.NodeExist(n) || n.NodeExist(targetNode))
                    continue;

                INode<string> curN = n;
                for (var i = 0; i < puthL - 1; ++i) 
                {
                    var newN = cache[Rnd.Next(nodeLength)];

                    if (newN == curN || newN.NodeExist(curN) || curN.NodeExist(newN))
                        continue;

                    var P1 = curN.Point;
                    var P2 = newN.Point;
                    var distance = Math.Sqrt(Math.Pow(P1.X - P2.X, 2) + Math.Pow(P1.Y - P2.Y, 2) + Math.Pow(P1.Z - P2.Z, 2));

                    curN = Rnd.Next(2) == 0 ? curN.AddNodeDD((n, g) => newN, distance) : curN.AddNodeDS((n, g) => newN, distance);

                    curN = newN;
                }

                if (targetNode != curN && !targetNode.NodeExist(curN) && !curN.NodeExist(targetNode)) 
                {
                    var P1 = curN.Point;
                    var P2 = targetNode.Point;
                    var distance = Math.Sqrt(Math.Pow(P1.X - P2.X, 2) + Math.Pow(P1.Y - P2.Y, 2) + Math.Pow(P1.Z - P2.Z, 2));

                    curN = Rnd.Next(2) == 0 ? curN.AddNodeDD((n, g) => targetNode, distance) : curN.AddNodeDS((n, g) => targetNode, distance);
                }

                procentCounter += oneValProcent;

                if (procentCounter >= 5)
                {
                    taskBarCounter += 5;
                    procentCounter -= 5;
                    ExecLog($"[smart random] {taskBarCounter + 50}/100");
                }
            }

            if (taskBarCounter < 50 && Math.Round(procentCounter) >= 5)
                ExecLog($"[smart random] 100/100");
        }

        private void RandomEdjes(IGraph<string> Graph, int MaxEdjesInNode) 
        {
            var nodeLength = Graph.NodeLength;
            var oneValProcent = 50d / nodeLength;
            var procentCounter = 0d;
            var taskBarCounter = 0;

            var cache = Graph.ToArray();

            foreach (var n in Graph) 
            {
                var connCount = Rnd.Next(MaxEdjesInNode + 1);
                for (var i = 0; i < connCount; ++i) 
                {
                    var rndIndex = Rnd.Next(nodeLength);
                    var targetNode = cache[rndIndex]; 
                    if (targetNode == n || targetNode.NodeExist(n) || n.NodeExist(targetNode))
                        continue;

                    var dependence = (Dependence)Rnd.Next(2);
                    var P1 = n.Point;
                    var P2 = targetNode.Point;
                    var distance = Math.Sqrt(Math.Pow(P1.X - P2.X, 2) + Math.Pow(P1.Y - P2.Y, 2) + Math.Pow(P1.Z - P2.Z, 2));

                    n.AddNode(distance, dependence, (g, n) => targetNode);
                }

                procentCounter += oneValProcent;

                if (procentCounter >= 5)
                {
                    taskBarCounter += 5;
                    procentCounter -= 5;
                    ExecLog($"[random] {taskBarCounter + 50}/100");
                }
            }

            if (taskBarCounter < 50 && Math.Round(procentCounter) >= 5)
                ExecLog($"[random] 100/100");
        }

        private void RandomNodes(IGraph<string> Graph, int Len, int PicSize) 
        {
            var nodeDiametr = NodeRadiusPx << 1;

            var maxIndex = ((int)Math.Ceiling(Math.Sqrt(Len))) * 3;
            var maxCapacity = PicSize / nodeDiametr;

            if (maxCapacity < maxIndex)
                throw new Exception("Invalid settings (PicSize or NodeCount)!");

            var indexes = new List<int>();
            for (var i = 0; i < maxCapacity * maxCapacity; ++i) 
            {
                indexes.Add(i);
            }

            void remIndex(int X, int Y)
            {
                var remIndex1d = Y * maxCapacity + X;
                indexes.Remove(remIndex1d);
            }

            var oneValProcent = 50d / Len;
            var procentCounter = 0d;
            var taskBarCounter = 0;

            for (int i = 0; i < Len; ++i) 
            {
                var rndIndex = Rnd.Next(indexes.Count);
                var index1d = indexes[rndIndex];

                var y = index1d / maxCapacity;
                var x = index1d - y * maxCapacity;
                var xOffSet = Rnd.Next(-Offset, Offset);
                var yOffSet = Rnd.Next(-Offset, Offset);

                Graph.AddNode(i.ToString(), new Point3D((x + 1) * nodeDiametr - NodeRadiusPx + xOffSet, (y + 1) * nodeDiametr - NodeRadiusPx + yOffSet));

                indexes.RemoveAt(rndIndex);                              

                remIndex(x - 1, y - 1);
                remIndex(x, y - 1);
                remIndex(x + 1, y - 1);
                remIndex(x - 1, y);
                remIndex(x + 1, y);
                remIndex(x - 1, y + 1);
                remIndex(x, y + 1);
                remIndex(x + 1, y + 1);

                procentCounter += oneValProcent;

                if (procentCounter >= 5)
                {
                    taskBarCounter += 5;
                    procentCounter -= 5;
                    ExecLog($"[random] {taskBarCounter}/100");
                }
            }

            if (taskBarCounter < 50 && Math.Round(procentCounter) >= 5)
                ExecLog($"[random] 50/100");
        }

        class SearchModel 
        {
            public string Start { get; set; }

            public string Finish { get; set; }

            public bool Last { get; set; }
        }

        class InitModel 
        {
            public InitModel(List<string> GraphTypes) 
            {
                this.GraphTypes = GraphTypes;
            }

            public string GraphName { get; set; } = "Big boy";

            public int PicSize { get; set; } = 10000;

            public int NodeCount { get; set; } = 1000;

            public int MaxEdjesInNode { get; set; } = 30;

            public string GraphType { get; set; }
            public List<string> GraphTypes;

            public bool T_Rnd_Or_F_Deseri { get; set; }

            public bool SmartRandom { get; set; }
        }
    }    
}
