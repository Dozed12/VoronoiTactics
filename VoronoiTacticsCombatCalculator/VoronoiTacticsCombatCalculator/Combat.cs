using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VoronoiTacticsCombatCalculator
{
    public class Combat
    {
        public Unit unitA;
        public Unit unitB;

        public Terrain terrainA;
        public Terrain terrainB;

        public Connection connection;

        public Combat(Unit A, Unit B, Terrain a, Terrain b, Connection C)
        {
            unitA = A;
            unitB = B;
            terrainA = a;
            terrainB = b;
            connection = C;
        }
    }
}
