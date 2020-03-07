using System;
using System.Diagnostics;
using NodeCore.Base;
using System.Threading;
using System.Threading.Tasks;
using NodeCore.Realization;
using System.Linq;
using System.Collections.Generic;

namespace ConsoleTest
{
    class Program
    {
        static IGraph<string> StaticGraph;

        static void Main(string[] args)
        {
            var s = Zavod.Get2<string>("1");
            PerformanceTest("first jit compilation", () => { FirstJitCompilation(s); });
            PerformanceTest("test1", Test1);

            int n = 90;
            PerformanceTest("TestCreateRectGraph", () => { TestCreateRectGraph(n); });
            Console.ReadKey();
            PerformanceTest("TestSearchPathRectGraph", (sw) => { TestSearchPathRectGraph(sw, n); }, false);
            Console.ReadKey();
            PerformanceTest("TestSearchPathRectGraph", (sw) => { TestSearchPathRectGraph(sw, n); }, false);

            Console.ReadKey();
        }

        static void TestSearchPathRectGraph(Stopwatch sw, int n) 
        {
            var sss = StaticGraph.CreateNodeProcessor();
            sw.Start();
            var puth2 = sss.SearchPath(StaticGraph["0:0"], StaticGraph[$"{n - 1}:{n - 1}"]);
            sw.Stop();
        }

        static void TestCreateRectGraph(int n) 
        {
            StaticGraph = Zavod.Get2<string>("rect");
            var rn = StaticGraph.AddNode("root");
            //StaticGraph.AddNode("test", new Point3D(n + 30, n + 30));

            for (var i = 0; i < n; i++)
            {
                var p = i - 1;
                if (p > -1)
                {
                    rn = rn.Graph[$"{p}:{0}"];
                }

                for (var j = 0; j < n; j++)
                {
                    var name = $"{i}:{j}";

                    rn.AddNodeDD(name, 1, new Point3D((j + 1), (i)));

                    rn = rn.Graph[name];
                }

                if (p > -1)
                {
                    for (var j = 1; j < n; j++)
                    {
                        rn = rn.Graph[$"{p}:{j}"];
                        rn.AddNodeDD((x, y) => x[$"{i}:{j}"], 1);
                    }
                }
            }
            //var sw = new Stopwatch(); 
            //var sss = StaticGraph.CreateNodeProcessor();
            //GC.Collect();
            //sw.Start();
            //var puth2 = sss.SearchPath(StaticGraph["0:0"], StaticGraph[$"{n - 1}:{n - 1}"]);
            //sw.Stop(); Console.ForegroundColor = ConsoleColor.Blue;
            //Console.WriteLine(sw.ElapsedMilliseconds); Console.ReadKey();
        }

        static void Test1() 
        {
            var s = Zavod.Get3<string>("2");
            var tn = s.AddNode(null);
            tn.AddNodeDD(null, 1, new Point3D(1));
            tn.AddNodeDD(null, 1, new Point3D(0, 1));
            tn = s[new Point3D(1)];
            tn.AddNodeDD(null, 1, new Point3D(1, 1));
            tn = s[new Point3D(0, 1)];
            tn.AddNodeDD((g, n) => g[new Point3D(1, 1)], 1);

            var np = s.CreateNodeProcessor();
            var res = np.SearchPath(s[new Point3D()], s[new Point3D(1, 1)]);
        }

        static void FirstJitCompilation<T>(IGraph<T> Graph) 
        {
            Graph.Clear();
            Graph.AddNode("test");
            Graph.NodeExist("test");
            Graph.GetEnumerator();
            var gl = Graph.NodeLength;
            var n = Graph["test"];
            n = Graph[new Point3D()];

            n = n.AddNodeDD("test2", 12, new Point3D(12));
            n = n.AddNodeDS("test3", 13, new Point3D(13));

            Graph.AddNode("test4", new Point3D(14));
            n.AddNodeDD((g, no) => g["test4"], 14);
            Graph["test3"].AddNodeDS((g, no) => g["test4"], 14);

            var nl = n.ConnectionLength;

            var np = Graph.CreateNodeProcessor();
            var puth = np.SearchPath(Graph["test4"], Graph["test2"]);
            var puthAsync = np.SearchPathAsync(Graph["test4"], Graph["test2"]).Result;

            Graph.DeleteNode("test");
            Graph.DeleteNode("test4");
            Graph.DeleteNode("test2");
            Graph.DeleteNode("test3");
        }

        static void PerformanceTest(string PrevText, Action Clbk, bool Clear = true) 
        {
            if (Clear)
                Console.Clear();

            GC.Collect();
            Thread.Sleep(300);

            var sw = new Stopwatch();
            sw.Start();

            Clbk();

            sw.Stop();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(PrevText + " : " + sw.ElapsedMilliseconds + "ms.");
        }

        static void PerformanceTest(string PrevText, Action<Stopwatch> Clbk, bool Clear = true)
        {
            if (Clear)
                Console.Clear();

            GC.Collect();
            Thread.Sleep(300);

            var sw = new Stopwatch();

            Clbk(sw);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(PrevText + " : " + sw.ElapsedMilliseconds + "ms.");
        }
    }
}
