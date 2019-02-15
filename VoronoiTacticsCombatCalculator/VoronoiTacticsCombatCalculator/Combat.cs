using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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

            //TODO Fatigue and Fatigue Modifier not applied
            //TODO Terrain and Connection not used

            //Recover morale
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

                int casualtiesAttackerTrue = Math.Min(casualtiesAttacker, attacker.currentMen);
                int casualtiesDefenderTrue = Math.Min(casualtiesDefender, defender.currentMen);

                attacker.currentMen -= casualtiesAttackerTrue;
                defender.currentMen -= casualtiesDefenderTrue;

                attacker.currentMorale -= casualtiesAttackerTrue / (float)attacker.maxMen;
                defender.currentMorale -= casualtiesDefenderTrue / (float)defender.maxMen;

                attacker.CalculateMaxMorale();
                defender.CalculateMaxMorale();

                log.AppendText("Attacker morale: " + attacker.currentMorale);
                log.AppendText(Environment.NewLine);
                log.AppendText("Defender morale: " + defender.currentMorale);
                log.AppendText(Environment.NewLine);
                log.AppendText(Environment.NewLine);
            }

            //TODO Check Retreat(morale)
            if (attacker.currentMorale < attacker.minimumMorale)
            {

            }
            if (defender.currentMorale < defender.minimumMorale)
            {

            }

            //TODO Apply fatigue

            //Increment round
            round++;

        }

    }
}
