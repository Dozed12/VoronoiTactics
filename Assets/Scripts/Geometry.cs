using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//https://github.com/Zalgo2462/VoronoiLib
using VoronoiLib;
using VoronoiLib.Structures;

public static class Geometry
{

    //Useful point struct
    private struct Point
    {
        public short x;
        public short y;
        public Point(short aX, short aY) { x = aX; y = aY; }
        public Point(int aX, int aY) : this((short)aX, (short)aY) { }
    }

    //Point inside convex polygon
    public static bool InsideConvex(List<VPoint> vertices, VPoint point)
    {
        VPoint[] poly = vertices.ToArray();
        var coef = poly.Skip(1).Select((p, i) =>
                                           (point.Y - poly[i].Y) * (p.X - poly[i].X)
                                         - (point.X - poly[i].X) * (p.Y - poly[i].Y))
                                   .ToList();

        if (coef.Any(p => p == 0))
            return true;

        for (int i = 1; i < coef.Count(); i++)
        {
            if (coef[i] * coef[i - 1] < 0)
                return false;
        }
        return true;
    }

}
