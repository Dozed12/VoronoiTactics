using System;
using UnityEngine;

namespace VoronoiLib.Structures
{
    public class VEdge : IComparable<VEdge>
    {

        //ADDED BY DOZED
        public VPoint center;

        public VPoint Start { get; internal set; }
        public VPoint End { get; internal set; }
        public FortuneSite Left { get; }
        public FortuneSite Right { get; }
        internal double SlopeRise { get; }
        internal double SlopeRun { get; }
        internal double? Slope { get; }
        internal double? Intercept { get; }

        public VEdge Neighbor { get; internal set; }

        internal VEdge(VPoint start, FortuneSite left, FortuneSite right)
        {
            Start = start;
            Left = left;
            Right = right;

            //for bounding box edges
            if (left == null || right == null)
                return;

            //from negative reciprocal of slope of line from left to right
            //ala m = (left.y -right.y / left.x - right.x)
            SlopeRise = left.X - right.X;
            SlopeRun = -(left.Y - right.Y);
            Intercept = null;

            if (SlopeRise.ApproxEqual(0) || SlopeRun.ApproxEqual(0)) return;
            Slope = SlopeRise / SlopeRun;
            Intercept = start.Y - Slope * start.X;
        }

        //ADDED BY DOZED
        //https://stackoverflow.com/questions/7586063/how-to-calculate-the-angle-between-a-line-and-the-horizontal-axis
        public int CompareTo(VEdge p)
        {
            float deltaAY = (float)(center.Y - this.Start.Y);
            float deltaAX = (float)(center.X - this.Start.X);
            float angleA = Mathf.Atan(deltaAY/ deltaAX) * 180 / Mathf.PI;

            float deltaPY = (float)(center.Y - p.Start.Y);
            float deltaPX = (float)(center.X - p.Start.X);
            float angleP = Mathf.Atan(deltaPY/ deltaPX) * 180 / Mathf.PI;

            return (int)(angleP - angleA);
        }

    }
}
