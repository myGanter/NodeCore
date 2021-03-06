﻿using System;
using System.Collections.Generic;
using System.Linq;
using NodeCore.Base;

namespace NodeCore.Realization.Services
{
    static class Helper
    {
        public static double CalculateDistance(Point3D P1, Point3D P2) => Math.Sqrt(Math.Pow(P1.X - P2.X, 2) + Math.Pow(P1.Y - P2.Y, 2) + Math.Pow(P1.Z - P2.Z, 2));

        public static double CalculateManhattanDistance(Point3D P1, Point3D P2) => Math.Abs(P1.X - P2.X) + Math.Abs(P1.Y - P2.Y) + Math.Abs(P1.Z - P2.Z);

        public static INode<T> NodeFlightBirdSearch<T>(IEnumerable<INode<T>> Collection, Point3D Finish)
        {
            var res = Collection.First();
            var minL = CalculateDistance(res.Point, Finish);

            foreach (var n in Collection.Skip(1))
            {
                var newL = CalculateDistance(n.Point, Finish);
                if (minL > newL)
                {
                    minL = newL;
                    res = n;
                }
            }

            return res;
        }
    }
}
