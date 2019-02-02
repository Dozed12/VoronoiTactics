namespace VoronoiLib.Structures
{
    internal class FortuneCircleEvent : FortuneEvent
    {
        internal VPoint Lowest { get; }
        internal float YCenter { get; }
        internal RBTreeNode<BeachSection> ToDelete { get; }

        internal FortuneCircleEvent(VPoint lowest, float yCenter, RBTreeNode<BeachSection> toDelete)
        {
            Lowest = lowest;
            YCenter = yCenter;
            ToDelete = toDelete;
        }

        public int CompareTo(FortuneEvent other)
        {
            var c = Y.CompareTo(other.Y);
            return c == 0 ? X.CompareTo(other.X) : c;
        }

        public float X => Lowest.X;
        public float Y => Lowest.Y;
    }
}
