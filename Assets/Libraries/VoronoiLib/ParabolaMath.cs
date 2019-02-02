using System;
using System.Runtime.CompilerServices;

namespace VoronoiLib
{
    public static class ParabolaMath
    {
        public const double EPSILON = double.Epsilon*1E100;

        public static float EvalParabola(float focusX, float focusY, float directrix, float x)
        {
            return .5f*( (x - focusX) * (x - focusX) /(focusY - directrix) + focusY + directrix);
        }

        //gives the intersect point such that parabola 1 will be on top of parabola 2 slightly before the intersect
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float IntersectParabolaX(float focus1X, float focus1Y, float focus2X, float focus2Y,
            float directrix)
        {
            //admittedly this is pure voodoo.
            //there is attached documentation for this function
            return focus1Y.ApproxEqual(focus2Y)
                ? (focus1X + focus2X)/2
                : (focus1X*(directrix - focus2Y) + focus2X*(focus1Y - directrix) +
                   (float)Math.Sqrt((directrix - focus1Y)*(directrix - focus2Y)*
                             ((focus1X - focus2X)*(focus1X - focus2X) +
                              (focus1Y - focus2Y)*(focus1Y - focus2Y))
                   )
                  )/(focus1Y - focus2Y);
        }

        public static bool ApproxEqual(this float value1, float value2)
        {
            return Math.Abs(value1 - value2) <= EPSILON;
        }

        public static bool ApproxGreaterThanOrEqualTo(this float value1, float value2)
        {
            return value1 > value2 || value1.ApproxEqual(value2);
        }

        public static bool ApproxLessThanOrEqualTo(this float value1, float value2)
        {
            return value1 < value2 || value1.ApproxEqual(value2);
        }
    }
}
