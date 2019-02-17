using System;
using System.Windows.Forms;

namespace VoronoiTacticsCombatCalculator
{
    public class Combat
    {

        //Fatigue base values
        public static float fatigueRecovery = 0.001f;
        public static float fatigueReload = 0.0015f;
        public static float fatigueMarch = 0.002f;
        public static float fatigueRun = 0.005f;
        public static float fatigueMelee = 0.003f;

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

        public Combat(TextBox log, Unit A, Unit B, Terrain a, Terrain b, Connection C, Phase Phase)
        {
            this.log = log;
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

            //TODO Condition and Weight not applied
            //TODO Terrain and Connection not used

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
                    casualtiesDefender = attacker.Fire(random, "Attacker");
                    casualtiesAttacker = defender.Fire(random, "Defender");
                    break;
                case Phase.MELEE:
                    if (round == 1)
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

            //Apply casualties/morale after combat
            if (casualtiesAttacker != 0 || casualtiesDefender != 0)
            {
                //Real casualities(Can't lose more than current amount)
                int casualtiesAttackerTrue = Math.Min(casualtiesAttacker, attacker.currentMen);
                int casualtiesDefenderTrue = Math.Min(casualtiesDefender, defender.currentMen);

                //Remove casualities
                attacker.currentMen -= casualtiesAttackerTrue;
                defender.currentMen -= casualtiesDefenderTrue;

                //Morale impact as percentage of total men
                attacker.currentMorale -= casualtiesAttackerTrue / (float)attacker.maxMen;
                defender.currentMorale -= casualtiesDefenderTrue / (float)defender.maxMen;

                //Recalculate max morale
                attacker.CalculateMaxMorale();
                defender.CalculateMaxMorale();

                //Display in log
                log.AppendText("Attacker morale: " + attacker.currentMorale);
                log.AppendText(Environment.NewLine);
                log.AppendText("Defender morale: " + defender.currentMorale);
                log.AppendText(Environment.NewLine);
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
