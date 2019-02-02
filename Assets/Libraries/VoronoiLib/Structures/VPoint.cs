using System;

using UnityEngine;

namespace VoronoiLib.Structures
{
    public class VPoint
    {
        public float X { get; }
        public float Y { get; }

        internal VPoint(float x, float y)
        {
            X = x;
            Y = y;
        }

    }
}
