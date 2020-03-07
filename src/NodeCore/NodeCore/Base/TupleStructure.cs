namespace NodeCore.Base
{
    //GetHashCode TODO
    public struct TupleStructure
    {
        public static TupleStructure<T1> Create<T1>(T1 Item1) => new TupleStructure<T1>(Item1);

        public static TupleStructure<T1, T2> Create<T1, T2>(T1 Item1, T2 Item2) => new TupleStructure<T1, T2>(Item1, Item2);

        public static TupleStructure<T1, T2, T3> Create<T1, T2, T3>(T1 Item1, T2 Item2, T3 Item3) => new TupleStructure<T1, T2, T3>(Item1, Item2, Item3);
    }
    public struct TupleStructure<T1>
    {
        public T1 Item1 { get; set; }

        public TupleStructure(T1 Item1)
        {
            this.Item1 = Item1;
        }
    }

    public struct TupleStructure<T1, T2>
    {
        public T1 Item1 { get; set; }

        public T2 Item2 { get; set; }

        public TupleStructure(T1 Item1, T2 Item2) 
        {
            this.Item1 = Item1;
            this.Item2 = Item2;
        }
    }

    public struct TupleStructure<T1, T2, T3>
    {
        public T1 Item1 { get; set; }

        public T2 Item2 { get; set; }

        public T3 Item3 { get; set; }

        public TupleStructure(T1 Item1, T2 Item2, T3 Item3)
        {
            this.Item1 = Item1;
            this.Item2 = Item2;
            this.Item3 = Item3;
        }
    }
}
