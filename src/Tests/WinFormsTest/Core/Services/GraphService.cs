using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NodeCore.Base;
using NodeCore.Realization;
using System.Linq.Expressions;

namespace WinFormsTest.Core.Services
{
    public class GraphService<T>
    {
        private Lazy<Dictionary<string, Func<string, IGraph<T>>>> Chache;

        public GraphService() 
        {
            Chache = new Lazy<Dictionary<string, Func<string, IGraph<T>>>>(CreateDict);
        }

        public List<string> GetNames() 
        {
            var ch = Chache.Value;
            var res = ch.Select(x => x.Key).ToList();

            return res;
        }

        public IGraph<T> CreateGraph(string MethodName, string GraphName = null) 
        {
            var ch = Chache.Value;

            return ch[MethodName](GraphName);
        }

        private Dictionary<string, Func<string, IGraph<T>>> CreateDict() 
        {
            var factoryType = typeof(GraphFactory);
            var iGraphType = typeof(IGraph<>);
            var strType = typeof(string);
            var TType = typeof(T);

            var methods = factoryType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(x => 
                { 
                    var par = x.GetParameters(); 
                    return x.ReturnType.Name == iGraphType.Name && par.Length == 1 && par[0].ParameterType == strType; 
                })
                .ToList();

            var res = new Dictionary<string, Func<string, IGraph<T>>>();

            foreach (var i in methods) 
            {
                var metInfo = i.MakeGenericMethod(TType);
                var method = BuildFunc(metInfo);
                res.Add(i.Name, method);
            }

            return res;
        }

        private Func<string, IGraph<T>> BuildFunc(MethodInfo Method)         
        {
            var parameter = Expression.Parameter(typeof(string), "X");
            var methodCall = Expression.Call(null, Method, parameter);
            var expressionlambda = Expression.Lambda<Func<string, IGraph<T>>>(methodCall, parameter);
            var res = expressionlambda.Compile();

            return res;
        }
    }
}
