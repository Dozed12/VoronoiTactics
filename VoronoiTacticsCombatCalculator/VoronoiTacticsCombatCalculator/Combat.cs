using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VoronoiTacticsCombatCalculator
{
    public class Combat
    {

        public enum Phase
        {
            //Ranged fire being exchansed
            RANGED,
            //Melee combat
            MELEE,
            //Attacker chasing defender retreat
            CHASE
        }

        public Unit attacker;
        public Unit defender;

        public Terrain terrainAttacker;
        public Terrain terrainDefender;

        public Connection connection;

        public Phase phase;

        public Combat(Unit A, Unit B, Terrain a, Terrain b, Connection C, Phase Phase)
        {
            attacker = A;
            defender = B;
            terrainAttacker = a;
            terrainDefender = b;
            connection = C;
            phase = Phase;
        }

        public void Process()
        {

        }

    }
}
