using System;
using System.Windows.Forms;

namespace VoronoiTacticsCombatCalculator
{
    public class Combat
    {

        //Condition base values
        public static float conditionRecovery = 0.001f;
        public static float conditionReload = 0.0015f;
        public static float conditionMarch = 0.002f;
        public static float conditionRun = 0.005f;
        public static float conditionMelee = 0.003f;

        public enum Phase
        {
            //Ranged fire being exchansed
            RANGED,
            //Melee combat
            MELEE,
            //Attacker chasing defender retreat
            CHASE
        }

        public TextBox log;

        public Unit attacker;
        public Unit defender;

        public Terrain terrainAttacker;
        public Terrain terrainDefender;

        public Connection connection;

        public Phase phase;

        public int round;

        public bool firstMelee;

        public Combat(TextBox log, Unit A, Unit B, Terrain a, Terrain b, Connection C, Phase Phase)
        {
            this.log = log;
            this.attacker = A;
            this.defender = B;
            this.terrainAttacker = a;
            this.terrainDefender = b;
            this.connection = C;
            this.phase = Phase;
            this.round = 1;
            this.firstMelee = true;
        }

        public void Process()
        {
            //TODO Melee Combat
            //TODO Charge
            //TODO Condition impact on combat
            //TODO Terrain and Connection impact on combat
            //TODO Most things should be functions inside Unit.cs

            //TODO Increment can be precalculated(Always the same)
            attacker.currentCondition += conditionRecovery * (1 - (attacker.weight - 1));
            if (attacker.currentCondition > 1)
                attacker.currentCondition = 1;
            defender.currentCondition += conditionRecovery * (1 - (defender.weight - 1));
            if (defender.currentCondition > 1)
                defender.currentCondition = 1;

            //Recover morale based on currentMaxMorale
            attacker.currentMorale += attacker.moraleRecover;
            defender.currentMorale += defender.moraleRecover;
            if (attacker.currentMorale > attacker.currentMaxMorale)
                attacker.currentMorale = attacker.currentMaxMorale;
            if (defender.currentMorale > defender.currentMaxMorale)
                defender.currentMorale = defender.currentMaxMorale;

            //Casualities
            int casualtiesAttacker = 0;
            int casualtiesDefender = 0;

            //Random server
            System.Random random = new System.Random();

            switch (phase)
            {
                case Phase.RANGED:
                    casualtiesDefender = attacker.Fire(random, "Attacker", defender);
                    casualtiesAttacker = defender.Fire(random, "Defender", attacker);
                    break;
                case Phase.MELEE:

                    //Charge round
                    if (firstMelee)
                    {
                        casualtiesDefender = attacker.Melee(random, "Attacker", defender, true);
                        casualtiesAttacker = defender.Melee(random, "Defender", attacker, true);
                        firstMelee = false;
                    }
                    //Normal melee
                    else
                    {
                        casualtiesDefender = attacker.Melee(random, "Attacker", defender, false);
                        casualtiesAttacker = defender.Melee(random, "Defender", attacker, false);
                    }

                    break;
                case Phase.CHASE:
                    break;
                default:
                    break;
            }

            //Apply casualties/morale after combat
            if (casualtiesAttacker != 0 || casualtiesDefender != 0)
            {
                attacker.Casualities(casualtiesAttacker, "Attacker", 0);
                defender.Casualities(casualtiesDefender, "Defender", 0);
                log.AppendText(Environment.NewLine);
            }

            //Check Retreat(morale)
            //TODO If both fall morale at same time who retreats?
            //  - The one with lowest morale?
            //  - The one with lowest morale in percentage?
            if (attacker.currentMorale < attacker.minimumMorale && phase != Phase.CHASE)
            {
                log.AppendText("Attacker retreats");
                log.AppendText(Environment.NewLine);

                phase = Phase.CHASE;
            }
            if (defender.currentMorale < defender.minimumMorale && phase != Phase.CHASE)
            {
                log.AppendText("Defender retreats");
                log.AppendText(Environment.NewLine);

                phase = Phase.CHASE;
            }

            //TODO Apply Condition change

            //Increment round
            round++;

        }

    }
}
