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
        if (polygon.Count < 3)
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
        if (polygon.Count < 3)
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

    //Jitters a line segment of 2 VPoints
    public static List<VPoint> Jitter(VPoint start, VPoint end, int divisions, double magnitude)
    {

        //Create list and add start
        List<VPoint> jitter = new List<VPoint>();
        jitter.Add(start);

        //Distance of points
        double distance = System.Math.Sqrt((start.X - end.X) * (start.X - end.X) - (start.Y - end.Y) * (start.Y - end.Y));

        //Jitter distance
        double jitterDistance = distance / divisions;

        //Normal vector
        double dirX = (end.X - start.X) / distance;
        double dirY = (end.Y - start.Y) / distance;

        //Perpendicular
        double normalX = dirY;
        double normalY = -dirX;

        //For each division
        for (int i = 1; i < divisions; i++)
        {
            //Begin at first point
            double finalX = start.X;
            double finalY = start.Y;

            //Move N divisions
            finalX += i * dirX * jitterDistance;
            finalY += i * dirY * jitterDistance;

            //Move randomly in perpendicular
            finalX += UnityEngine.Random.Range(-(float)magnitude, (float)magnitude) * normalX;
            finalY += UnityEngine.Random.Range(-(float)magnitude, (float)magnitude) * normalY;

            //Add new point
            jitter.Add(new VPoint(finalX, finalY));

        }

        //Add last point
        jitter.Add(end);

        return jitter;

    }

}
