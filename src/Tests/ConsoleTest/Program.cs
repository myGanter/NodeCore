using System;
using System.Diagnostics;
using NodeCore.Base;
using System.Threading;
using System.Threading.Tasks;
using NodeCore.Realization;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using NodeCore.Realization.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace ConsoleTest
{
    public class TestObj
    {
        public string A1 = "test1";

        public int A2 = 23;

        public double A3 { get; set; } = 43.22;
    }

    class Program
    {
        static IGraph<TestObj> StaticGraph;

        static void Main(string[] args)
        {
            var s = GraphFactory.CreateDijkstraGraph<string>("1");
            PerformanceTest("first jit compilation", () => { FirstJitCompilation(s); });
            PerformanceTest("test1", Test1);

            PerformanceTest("hash", () => { for (var i = 0; i < 100000; ++i) { var s = i.ToString().GetHashCode(); } });
            PerformanceTest("hash2", () => { for (var i = 0; i < 100000; ++i) { var s = new Point3D(i, 3).GetHashCode(); } }, false);
            Console.ReadKey();

            int n = 100;
            Console.Clear();
            CheckGetHashCodePoint3D(n);
            Console.ReadKey();

            PerformanceTest("TestCreateRectGraph", () => { TestCreateRectGraph(n); });           
            PerformanceTest("TestSearchPathRectGraph", (sw) => { TestSearchPathRectGraph(sw, n); }, false);
            PerformanceTest("TestSearchPathRectGraph", (sw) => { TestSearchPathRectGraph(sw, n); }, false);
            Console.ReadKey();

            GraphBinarySerializer.ConfigureBaseTypes();
            PerformanceTest("CheckBinSerialize", CheckSerialize);
            PerformanceTest("CheckBinDeserialize", CheckDeserialize, false);
            Console.ReadKey();

            //проверка асинковых методов
            PerformanceTest("CheckBinSerializeAsync", () => CheckSerializeAsync().Wait());
            PerformanceTest("CheckBinDeserializeAsync", () => CheckDeserializeAsync().Wait(), false);
            Console.ReadKey();

            foreach (var i in StaticGraph)
            {
                i.Object = new TestObj();
            }

            PerformanceTest("CheckXMLSerialize", CheckSerializeXML);
            PerformanceTest("CheckXMLDeserialize", CheckDeserializeXML, false);
            Console.ReadKey();

            //проверка асинковых методов
            PerformanceTest("CheckXMLSerializeAsync", () => CheckSerializeXMLAsync().Wait());
            PerformanceTest("CheckXMLDeserializeAsync", () => CheckDeserializeXMLAsync().Wait(), false);
            Console.ReadKey();
        }

        static void CheckSerializeXML()
        {
            if (File.Exists("test.xml"))
                File.Delete("test.xml");
            using (var fs = File.Open("test.xml", FileMode.Create))
            {
                StaticGraph.SerializeToXml(fs, true);
            }
        }

        static void CheckDeserializeXML()
        {
            using (var fs = File.Open("test.xml", FileMode.Open))
            {
                StaticGraph.XmlDeserialize(fs, true);
            }
        }

        static async Task CheckSerializeXMLAsync()
        {
            if (File.Exists("test.xml"))
                File.Delete("test.xml");
            using (var fs = File.Open("test.xml", FileMode.Create))
            {
                await StaticGraph.SerializeToXmlAsync(fs, true);
            }
        }

        static async Task CheckDeserializeXMLAsync()
        {
            using (var fs = File.Open("test.xml", FileMode.Open))
            {
                await StaticGraph.XmlDeserializeAsync(fs, true);
            }
        }

        static void CheckSerialize() 
        {
            if (File.Exists("test.bin"))
                File.Delete("test.bin");
            using (var fs = File.Open("test.bin", FileMode.Create))
            {
                StaticGraph.SerializeToBinary(fs, true);
            }
        }

        static void CheckDeserialize()
        {
            using (var fs = File.Open("test.bin", FileMode.Open))
            {
                StaticGraph.BinaryDeserialize(fs, true);
            }
        }

        static async Task CheckSerializeAsync()
        {
            if (File.Exists("test.bin"))
                File.Delete("test.bin");
            using (var fs = File.Open("test.bin", FileMode.Create))
            {
                await StaticGraph.SerializeToBinaryAsync(fs, true);
            }
        }

        static async Task CheckDeserializeAsync()
        {
            using (var fs = File.Open("test.bin", FileMode.Open))
            {
                await StaticGraph.BinaryDeserializeAsync(fs, true);
            }
        }

        static void CheckGetHashCodePoint3D(int n) 
        {
            var hs = new HashSet<int>();
            var res = 0;

            for (var i = 0; i < n; ++i) 
            {
                for (var j = 0; j < n; ++j) 
                {
                    var p = new Point3D(j, i);
                    var ph = p.GetHashCode();

                    if (hs.Contains(ph))
                        res++;
                    else
                        hs.Add(ph);
                }
            }

            Console.WriteLine($"{res} | {n * n}");
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
            StaticGraph = GraphFactory.CreateDijkstraGraph<TestObj>("rect");
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
            var s = GraphFactory.CreateAStarGraph<string>("2");
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

            var sw = new Stopwatch();

            Clbk(sw);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(PrevText + " : " + sw.ElapsedMilliseconds + "ms.");
        }
    }
}
