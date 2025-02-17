﻿namespace VoronoiLib.Structures
{
    public class VEdge
    {
        public VPoint Start { get; internal set; }
        public VPoint End { get; internal set; }
        public FortuneSite Left { get; }
        public FortuneSite Right { get; }
        internal float SlopeRise { get; }
        internal float SlopeRun { get; }
        internal float? Slope { get; }
        internal float? Intercept { get; }

        public VEdge Neighbor { get; internal set; }

        //Added by Dozed
        internal VEdge(VPoint start, VPoint end, FortuneSite left, FortuneSite right)
        {
            Start = start;
            End = end;
            Left = left;
            Right = right;
        }

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

    }
}
