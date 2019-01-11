using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//https://github.com/Zalgo2462/VoronoiLib
using VoronoiLib;
using VoronoiLib.Structures;

public static class Geometry
{

    public static bool InsideConvex(List<VPoint> polygon, VPoint point)
    {
        //Check if a triangle or higher n-gon
        if(polygon.Count < 3)
            return false;

        //n>2 Keep track of cross product sign changes
        var pos = 0;
        var neg = 0;

        for (var i = 0; i < polygon.Count; i++)
        {
            //If point is in the polygon
            if (polygon[i] == point)
                return true;

            //Form a segment between the i'th point
            var x1 = polygon[i].X;
            var y1 = polygon[i].Y;

            //And the i+1'th, or if i is the last, with the first point
            var i2 = i < polygon.Count - 1 ? i + 1 : 0;

            var x2 = polygon[i2].X;
            var y2 = polygon[i2].Y;

            var x = point.X;
            var y = point.Y;

            //Compute the cross product
            var d = (x - x1) * (y2 - y1) - (y - y1) * (x2 - x1);

            if (d > 0) pos++;
            if (d < 0) neg++;

            //If the sign changes, then point is outside
            if (pos > 0 && neg > 0)
                return false;
        }

        //If no change in direction, then on same side of all segments, and thus inside
        return true;
    }

    //Point inside concave polygon
    public static bool InsideConcave(List<VPoint> polygon, VPoint point)
    {

        //Check if a triangle or higher n-gon
        if(polygon.Count < 3)
            return false;

        bool result = false;
        int j = polygon.Count() - 1;
        for (int i = 0; i < polygon.Count(); i++)
        {
            if (polygon[i].Y < point.Y && polygon[j].Y >= point.Y || polygon[j].Y < point.Y && polygon[i].Y >= point.Y)
            {
                if (polygon[i].X + (point.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) * (polygon[j].X - polygon[i].X) < point.X)
                {
                    result = !result;
                }
            }
            j = i;
        }
        return result;
    }

}
