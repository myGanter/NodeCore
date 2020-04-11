using System;
using NodeCore.Base;

namespace NodeCore.Realization.Services
{
    static class Helper
    {
        public static double CalculateDistance(Point3D P1, Point3D P2) => Math.Sqrt(Math.Pow(P1.X - P2.X, 2) + Math.Pow(P1.Y - P2.Y, 2) + Math.Pow(P1.Z - P2.Z, 2));      
    }
}
