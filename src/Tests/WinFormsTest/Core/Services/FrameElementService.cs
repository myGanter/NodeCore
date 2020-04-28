using System;
using System.Reflection;
using System.Collections.Generic;
using WinFormsTest.Core.Interfaces.IPresenters;
using WinFormsTest.Models;
using WinFormsTest.Core.Attributes;
using System.Linq;
using System.Text;
using System.Collections;

namespace WinFormsTest.Core.Services
{
    public class FrameElementService : IEnumerable<KeyValuePair<Type, FrameElementAttribute>>
    {
        private readonly Type AttrType;

        private Lazy<Dictionary<Type, FrameElementAttribute>> Chache { get; set; }

        public FrameElementService() 
        {
            AttrType = typeof(FrameElementAttribute);

            UpdateData();
        }

        public void UpdateData() 
        {
            Chache = new Lazy<Dictionary<Type, FrameElementAttribute>>(() => GetEnumerableFrameElementTypes()
                .ToDictionary(k => k, x => (FrameElementAttribute)x.GetCustomAttribute(AttrType)));
        }

        public Dictionary<Type, FrameElementAttribute> GetAttrInstanceForPresenter() 
        {
            var vals = Chache.Value;
            var res = new Dictionary<Type, FrameElementAttribute>();
            foreach (var i in vals)
                res.Add(i.Key, i.Value);

            return res;
        }

        public bool ContainsType(Type PType)
        {
            var vals = Chache.Value;

            return vals.ContainsKey(PType);
        }

        public FrameElementAttribute GetFrameElementAttributeForType(Type PType) 
        {
            var vals = Chache.Value;

            return vals[PType];
        }

        public IEnumerator<KeyValuePair<Type, FrameElementAttribute>> GetEnumerator() 
        {
            var vals = Chache.Value;

            return vals.Select(x => x).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private IEnumerable<Type> GetEnumerableFrameElementTypes() 
        {
            var presenteType = typeof(IPresenter<FrameElementArg>);
            var res = Assembly
                .GetCallingAssembly()
                .GetTypes()
                .Where(x => x.Namespace != null
                            && x.Namespace.EndsWith("Presenters")
                            && x.GetInterfaces().Any(x => x == presenteType)
                            && x.GetCustomAttribute(AttrType) != null);

            return res;
        }
    }
}
