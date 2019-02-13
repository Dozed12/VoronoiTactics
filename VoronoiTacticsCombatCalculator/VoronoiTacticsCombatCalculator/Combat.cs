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

        public int round;

        public Combat(Unit A, Unit B, Terrain a, Terrain b, Connection C, Phase Phase)
        {
            attacker = A;
            defender = B;
            terrainAttacker = a;
            terrainDefender = b;
            connection = C;
            phase = Phase;
            round = 1;
        }

        public void Process()
        {

            int casualtiesAttacker = 0;
            int casualtiesDefender = 0;

            switch (phase)
            {
                case Phase.RANGED:
                    //Reload complete
                    if(attacker.reloadTimer == attacker.rangedReload)
                    {

                        //Fire


                        //Start Reload
                        attacker.reloadTimer = 0;

                    }
                    else
                    {
                        //Continue reloading
                        attacker.reloadTimer++;
                    }
                    break;
                case Phase.MELEE:
                    if(round == 1)
                    {
                        //TODO do charge on first round of melee
                        //if melee comes after ranged and we have a single combat object for all combat then round != 0, what will we do?
                        //keep track if there was a melee round before(simple flag)
                    }
                    break;
                case Phase.CHASE:
                    break;
                default:
                    break;
            }

            //TODO Apply casualties/morale after combat
            attacker.currentMen -= casualtiesAttacker;
            defender.currentMen -= casualtiesDefender;            

            //TODO Check Retreat(morale)

            //TODO Apply fatigue

            //Increment round
            round++;

        }

    }
}
