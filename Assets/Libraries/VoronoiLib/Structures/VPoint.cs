using System;

using UnityEngine;

namespace VoronoiLib.Structures
{
    public class VPoint : IComparable, IEquatable<VPoint>
    {
        public double X { get; }
        public double Y { get; }

        //Added by Dozed
        private VPoint center;
        private double angle;
        public void findAngle(VPoint center)
        {
            this.center = center;
            angle = Mathf.Rad2Deg * Mathf.Atan2((float)this.Y - (float)center.Y, (float)this.X - (float)center.X);
            if (angle < 0)
                angle += 360;
        }

        internal VPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        //Added by Dozed
        int IComparable.CompareTo(object obj)
        {
            VPoint b = (VPoint)obj;
            return (int)(this.angle - b.angle);
        }

        //Added by Dozed
        public bool Equals(VPoint obj){
            if(this.X == obj.X && this.Y == obj.Y)
                return true;
            return false;
        }
        public override int GetHashCode()
        {
            int hashFirstName = X.GetHashCode();
            int hashLastName = Y.GetHashCode();

            return hashFirstName ^ hashLastName;
        }

    }
}
