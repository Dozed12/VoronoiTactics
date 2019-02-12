using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VoronoiTacticsCombatCalculator
{
    class Combat
    {
        Unit unitA;
        Unit unitB;

        Terrain terrainA;
        Terrain terrainB;

        public float river;
        public int distance;

        public Combat(Unit A, Unit B, Terrain a, Terrain b, float River, int Distance)
        {
            unitA = A;
            unitB = B;
            terrainA = a;
            terrainB = b;
            river = River;
            distance = Distance;
        }
    }
}
