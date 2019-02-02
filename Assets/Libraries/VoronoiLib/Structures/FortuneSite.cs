using System.Collections.Generic;

namespace VoronoiLib.Structures
{
    public class FortuneSite
    {
        public float X { get; }
        public float Y { get; }
        public int ID { get; }

        public List<Geometry.Vector2Edge> Cell { get; private set; }

        public List<FortuneSite> Neighbors { get; private set; }

        public FortuneSite(float x, float y, int id)
        {
            X = x;
            Y = y;
            ID = id;
            Cell = new List<Geometry.Vector2Edge>();
            Neighbors = new List<FortuneSite>();
        }
    }
}
