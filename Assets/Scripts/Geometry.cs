using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//https://github.com/Zalgo2462/VoronoiLib
using VoronoiLib;
using VoronoiLib.Structures;

public static class Geometry
{

    //Extended version of Unity Vector2 prepared for circular sorting
    public class Vector2X : IComparable, IEquatable<Vector2X>
    {
        public Vector2 value;

        private Vector2 center;
        private float angle;
        public void findAngle(Vector2 center)
        {
            this.center = center;
            angle = Mathf.Rad2Deg * Mathf.Atan2(this.value.y - center.y, this.value.x - center.x);
            if (angle < 0)
                angle += 360;
        }

        public Vector2X(int X, int Y)
        {
            value = new Vector2(X, Y);
        }

        public Vector2X(float X, float Y)
        {
            value = new Vector2(X, Y);
        }

        //Comparators
        int IComparable.CompareTo(object obj)
        {
            Vector2X b = (Vector2X)obj;
            return (int)(this.angle - b.angle);
        }
        public bool Equals(Vector2X obj)
        {
            if (this.value.x == obj.value.x && this.value.y == obj.value.y)
                return true;
            return false;
        }
        public override int GetHashCode()
        {
            int hashFirstName = value.x.GetHashCode();
            int hashLastName = value.y.GetHashCode();

            return hashFirstName ^ hashLastName;
        }

    }

    //Vector2 based Edge
    public class Vector2Edge
    {
        public Vector2X start;
        public Vector2X end;

        public FortuneSite left;
        public FortuneSite right;

        public Vector2Edge(Vector2X Start, Vector2X End)
        {
            start = Start;
            end = End;
        }

        public Vector2Edge(Vector2X Start, Vector2X End, FortuneSite Left, FortuneSite Right)
        {
            start = Start;
            end = End;
            left = Left;
            right = Right;
        }

    }

    //Point inside convex polygon
    public static bool InsideConvex(List<Vector2X> polygon, Vector2X point)
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
            var x1 = polygon[i].value.x;
            var y1 = polygon[i].value.y;

            //And the i+1'th, or if i is the last, with the first point
            var i2 = i < polygon.Count - 1 ? i + 1 : 0;

            var x2 = polygon[i2].value.x;
            var y2 = polygon[i2].value.y;

            var x = point.value.x;
            var y = point.value.y;

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

    //Point inside polygon (convex or concave)
    public static bool PointInPolygon(List<Vector2X> poly, Vector2X point)
    {
        bool c = false;
        int i, j;
        for (i = 0, j = poly.Count - 1; i < poly.Count; j = i++)
        {
            if (((poly[i].value.y > point.value.y) != (poly[j].value.y > point.value.y)) && (point.value.x < (poly[j].value.x - poly[i].value.x) * (point.value.y - poly[i].value.y) / (poly[j].value.y - poly[i].value.y) + poly[i].value.x))
                c = !c;
        }
        return c;
    }

    //Jitters a line segment of 2 Vector2
    public static List<Vector2X> Jitter(Vector2X start, Vector2X end, int divisions, float magnitude)
    {

        //Create list and add start
        List<Vector2X> jitter = new List<Vector2X>();
        jitter.Add(start);

        //Vector
        float vecX = end.value.x - start.value.x;
        float vecY = end.value.y - start.value.y;

        //Distance of points
        float distance = Mathf.Sqrt(vecX * vecX + vecY * vecY);

        //Jitter distance
        float jitterDistance = distance / divisions;

        //Normalized Vector
        float dirX = vecX / distance;
        float dirY = vecY / distance;

        //Perpendicular
        float normalX = dirY;
        float normalY = -dirX;

        //Division step
        float divisionX = dirX * jitterDistance;
        float divisionY = dirY * jitterDistance;

        //For each division
        for (int i = 1; i < divisions; i++)
        {
            //Begin at first point
            float finalX = start.value.x;
            float finalY = start.value.y;

            //Move N divisions
            finalX += i * divisionX;
            finalY += i * divisionY;

            //Move randomly in perpendicular
            finalX += UnityEngine.Random.Range(-magnitude, magnitude) * normalX;
            finalY += UnityEngine.Random.Range(-magnitude, magnitude) * normalY;

            //Add new point
            jitter.Add(new Vector2X(finalX, finalY));

        }

        //Add last point
        jitter.Add(end);

        return jitter;

    }

    //Returns a list of points from Bresenham
    public static List<VPoint> BresenhamLine(int x0, int y0, int x1, int y1)
    {

        List<VPoint> list = new List<VPoint>();

        int dx = Mathf.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
        int dy = Mathf.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
        int err = (dx > dy ? dx : -dy) / 2, e2;

        do
        {

            list.Add(new VPoint(x0, y0));

            if (x0 == x1 && y0 == y1) break;
            e2 = err;
            if (e2 > -dx) { err -= dy; x0 += sx; }
            if (e2 < dy) { err += dx; y0 += sy; }
        } while (true);

        return list;
    }

    //Generate random polygon
    public static List<VPoint> RandomPolygon(int centerX, int centerY, int minRadius, int maxRadius, int sides)
    {
        List<VPoint> list = new List<VPoint>();

        float angleInc = Utilities.PI2 / sides;

        for (float i = 0; i < Utilities.PI2; i += angleInc)
        {
            float cos = Mathf.Cos(i);
            float sin = Mathf.Sin(i);
            float radius = UnityEngine.Random.Range(minRadius, maxRadius);
            list.Add(new VPoint(centerX + cos * radius, centerY + sin * radius));
        }

        return list;
    }

    //Polygon pixels
    public static List<VPoint> PolygonPixels(List<VPoint> polygon)
    {

        List<VPoint> list = new List<VPoint>();

        int pixelX, pixelY, swap;
        int i, j, nodes;
        int[] nodeX = new int[polygon.Count];

        //Limits of polygon
        int IMAGE_TOP = int.MaxValue, IMAGE_BOT = -1, IMAGE_RIGHT = -1, IMAGE_LEFT = int.MaxValue;

        for (int p = 0; p < polygon.Count; p++)
        {
            if (polygon[p].X > IMAGE_RIGHT)
                IMAGE_RIGHT = (int)polygon[p].X;
            if (polygon[p].X < IMAGE_LEFT)
                IMAGE_LEFT = (int)polygon[p].X;
            if (polygon[p].Y < IMAGE_TOP)
                IMAGE_TOP = (int)polygon[p].Y;
            if (polygon[p].Y > IMAGE_BOT)
                IMAGE_BOT = (int)polygon[p].Y;
        }

        //  Loop through the rows of the image.
        for (pixelY = IMAGE_TOP; pixelY < IMAGE_BOT; pixelY++)
        {

            //  Build a list of nodes.
            nodes = 0; j = polygon.Count - 1;
            for (i = 0; i < polygon.Count; i++)
            {
                if (polygon[i].Y < (float)pixelY && polygon[j].Y >= (float)pixelY || polygon[j].Y < (float)pixelY && polygon[i].Y >= (float)pixelY)
                {
                    nodeX[nodes++] = (int)(polygon[i].X + (pixelY - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) * (polygon[j].X - polygon[i].X));
                }
                j = i;
            }

            //  Sort the nodes, via a simple “Bubble” sort.
            i = 0;
            while (i < nodes - 1)
            {
                if (nodeX[i] > nodeX[i + 1])
                {
                    swap = nodeX[i]; nodeX[i] = nodeX[i + 1]; nodeX[i + 1] = swap; if (i != 0) i--;
                }
                else
                {
                    i++;
                }
            }

            //  Fill the pixels between node pairs.
            for (i = 0; i < nodes; i += 2)
            {
                if (nodeX[i] >= IMAGE_RIGHT)
                    break;
                if (nodeX[i + 1] > IMAGE_LEFT)
                {
                    if (nodeX[i] < IMAGE_LEFT)
                        nodeX[i] = IMAGE_LEFT;
                    if (nodeX[i + 1] > IMAGE_RIGHT)
                        nodeX[i + 1] = IMAGE_RIGHT;
                    for (pixelX = nodeX[i]; pixelX < nodeX[i + 1]; pixelX++)
                        list.Add(new VPoint(pixelX, pixelY));
                }
            }
        }

        return list;
    }

}
