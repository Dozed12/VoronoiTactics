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
    public class Unit
    {
        //Stats
        public int maxMen;
        public int guns;
        public int usable;
        public int menPerGun;
        public float minimumMorale;
        public float moraleRecover;
        public float speed;
        public float weight;

        public bool ranged;
        public float rangedAccuracy;
        public float rangedAttack;
        public int rangedTargets;
        public int rangedReload;

        public bool melee;
        public float meleeAttack;
        public int meleeTime;

        public bool charge;
        public float chargeAttack;
        public float chargeMoraleImpact;

        public float rangedDefense;
        public float meleeDefense;
        public float chargeDefense;

        //Situation
        public int currentMen;
        public float currentCondition;
        public float currentMorale;
        public float currentMaxMorale;
        public int reloadTimer;
        public int meleeTimer;

        //Log
        public TextBox log;

        public Unit(TextBox Log)
        {
            log = Log;
        }

        public void CalculateMaxMorale()
        {
            int diff = maxMen - currentMen;
            currentMaxMorale = -(float)diff * (float)diff / (float)maxMen / (float)maxMen + 1;
        }

        public int Fire(Random random, string who)
        {

            int kills = 0;

            //Check Reload
            if (reloadTimer == rangedReload)
            {

                //Number of fires
                int fires = usable;
                if (currentMen < usable)
                    fires = currentMen;

                //Number of hits
                int hits = 0;
                for (int i = 0; i < fires; i++)
                {
                    if (random.Next(0, 100) < rangedAccuracy * 100)
                        hits++;
                }

                //Number of potential kills
                int mortal = 0;
                for (int i = 0; i < hits; i++)
                {
                    if (random.Next(0, 100) < rangedAttack * 100)
                        mortal++;
                }

                //Number of kills
                kills = 0;
                for (int i = 0; i < mortal; i++)
                {
                    if (random.Next(0, 100) < rangedAttack * 100)
                        kills++;
                }

                //Message for log
                //TODO Log defended shots
                //TODO Look at RichTextBox for multi color text box
                log.AppendText(who);
                string message = " fired " + fires + " shots, hit " + hits + " men, " + kills + " mortally\n";
                log.AppendText(message);

                //Start Reload
                reloadTimer = 0;

            }
            else
            {
                //Progress reload
                reloadTimer++;

                //Reload cost on Condition
                currentCondition -= Combat.conditionReload * (1 - (weight - 1));
                if (currentCondition < 0)
                    currentCondition = 0;
                currentCondition -= Combat.conditionReload * (1 - (weight - 1));
                if (currentCondition < 0)
                    currentCondition = 0;
            }

            return kills;
        }

    }
}
