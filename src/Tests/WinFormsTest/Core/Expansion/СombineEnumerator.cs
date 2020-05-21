using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace WinFormsTest.Core.Expansion
{
    #region Сombine
    /// <summary>
    /// [ [ "val1", "val2" ], [ "val3", "val4" ], [ "val5", "val5" ] ] => [ "val1", "val2", "val3", "val4", "val5", "val5" ]
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class СombineEnumerator<T> : IEnumerator<T>
    {
        private IEnumerator<IEnumerable<T>> elemsE;
        private IEnumerator<T> eEE;

        public СombineEnumerator(IEnumerable<IEnumerable<T>> h)
        {
            elemsE = h.GetEnumerator();
            elemsE.MoveNext();

            eEE = elemsE.Current?.GetEnumerator();
        }

        public T Current { get; private set; }

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            var res = true;
            if (eEE == null || !eEE.MoveNext())
            {
                if (eEE != null)
                    Current = eEE.Current;

                res = elemsE.MoveNext();
                eEE = elemsE.Current?.GetEnumerator();

                if (res)
                    MoveNext();
            }
            else
            {
                Current = eEE.Current;
            }

            return res;
        }

        public void Dispose()
        { }

        public void Reset()
        { }
    }

    class СombineEnumerable<T> : IEnumerable<T>
    {
        private IEnumerable<IEnumerable<T>> Elems;

        public СombineEnumerable(IEnumerable<IEnumerable<T>> h)
        {
            Elems = h;
        }

        public IEnumerator<T> GetEnumerator() => new СombineEnumerator<T>(Elems);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    #endregion

    public static class ExpansionLinq
    {
        public static IEnumerable<T> Сombine<T>(this IEnumerable<IEnumerable<T>> h)
        {
            return new СombineEnumerable<T>(h);
        }
    }
}
