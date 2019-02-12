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

        Connection connection;

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
