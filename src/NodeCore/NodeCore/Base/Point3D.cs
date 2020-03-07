using System;
using System.Drawing;

namespace NodeCore.Base
{
    /// <summary>
    /// Точка в 3d пространстве
    /// </summary>
    public struct Point3D : IEquatable<Point3D>
    {
        public int X { get; set; }

        public int Y { get; set; }

        public int Z { get; set; }

        public Point3D(int X, int Y, int Z) 
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        public Point3D(int X) : this(X, 0, 0) { }

        public Point3D(int X, int Y) : this(X, Y, 0) { }

        public Point3D(Point P) : this(P.X, P.Y, 0) { }

        public Point3D(Size S) : this(S.Width, S.Height, 0) { }

        public void Offset(Point3D P) => Offset(P.X, P.Y, P.Z);

        public void Offset(int Dx, int Dy, int Dz) 
        {
            X += Dx;
            Y += Dy;
            Z += Dz;
        }

        public bool Equals(Point3D Other) => this == Other;

        public override bool Equals(object obj) => obj is Point3D p && Equals(p); 

        public override int GetHashCode()//переопределить срочно, куча говна(коллизий)!!!!
        {
            var hash = 17;
            hash = hash * 23 + X;
            hash = hash * 23 + Y;
            hash = hash * 23 + Z;
            return hash;

            //return ToString().GetHashCode();
        }

        public override string ToString() => $"{{X = {X} Y = {Y} Z = {Z}}}";

        public static Point3D operator +(Point3D P1, Point3D P2) => new Point3D(P1.X + P2.X, P1.Y + P2.Y, P1.Z + P2.Z);

        public static Point3D operator -(Point3D P1, Point3D P2) => new Point3D(P1.X - P2.X, P1.Y - P2.Y, P1.Z - P2.Z);

        public static bool operator ==(Point3D L, Point3D R) => L.X == R.X && L.Y == R.Y && L.Z == R.Z;

        public static bool operator !=(Point3D L, Point3D R) => L.X != R.X || L.Y != R.Y || L.Z != R.Z;
    }
}
