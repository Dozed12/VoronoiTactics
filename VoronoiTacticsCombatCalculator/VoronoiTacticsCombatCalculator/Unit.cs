﻿using System;
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
        public bool acclimatized;
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
        public float currentMinimumMorale;
        public float currentMaxMorale;
        public int reloadTimer;
        public int meleeTimer;

        //Constants
        public float conditionRecovery;
        public float conditionCostModifier;

        //Log
        public TextBox log;

        public Unit(TextBox Log)
        {
            log = Log;
        }

        //Setup specific unit constants
        public void Setup()
        {

            currentCondition = 1;

            //TODO can replace (1 - (weight - 1)) by (-weight + 2)
            conditionRecovery = Combat.conditionRecovery * (1 - (weight - 1));

            //TODO can replace (1 - (weight - 1)) by (-weight + 2)
            conditionCostModifier = (1 - (weight - 1));
        }

        //Calculate max morale given men lost
        public void CalculateMaxMorale()
        {
            int diff = maxMen - currentMen;
            currentMaxMorale = -(float)diff * (float)diff / (float)maxMen / (float)maxMen + 1;
        }

        //Fire, calculate and log casualities
        public int Fire(Random random, string who, Unit target)
        {

            //Not ranged
            if (!ranged)
                return 0;

            int kills = 0;

            //Check Reload
            if (reloadTimer == rangedReload)
            {

                //Number of fires
                int fires = rangedTargets;

                //No guns, every man has a gun
                if (guns == 0)
                {
                    if (currentMen < usable)
                        fires *= currentMen;
                    else
                        fires *= usable;
                }
                //Gun pieces
                else
                {
                    int possibleGuns = (int)(currentMen / menPerGun);
                    if (guns < possibleGuns)
                        fires *= guns;
                    else
                        fires *= possibleGuns;
                }

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
                for (int i = 0; i < mortal; i++)
                {
                    if (random.Next(0, 100) > target.rangedDefense * 100)
                        kills++;
                }

                //TODO Condition Cost

                //Message for log
                //TODO Log defended shots
                //TODO Look at RichTextBox for multi color text box(Color for each side)
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

                //Condition Cost
                currentCondition -= Combat.conditionReload * conditionCostModifier;
                if (currentCondition < 0)
                    currentCondition = 0;
            }

            return kills;
        }

        //Charge, calculate and log casualities
        public int Melee(Random random, string who, Unit target, bool charge)
        {

            //If no melee, use Ranged
            if (!melee)
            {
                return Fire(random, who, target);
            }

            int kills = 0;

            //Charge
            if (charge && this.charge)
            {

                //Number of hits
                int hits = 0;
                for (int i = 0; i < usable; i++)
                {
                    if (random.Next(0, 100) < chargeAttack * 100)
                        hits++;
                }

                //Number of kills
                for (int i = 0; i < hits; i++)
                {
                    if (random.Next(0, 100) > target.chargeDefense * 100)
                        kills++;
                }

                //Condition Cost
                currentCondition -= Combat.conditionMelee * conditionCostModifier;

                //Message for log
                //TODO Log defended hits
                //TODO Look at RichTextBox for multi color text box(Color for each side)
                log.AppendText(who);
                string message = " charged " + usable + " times, " + kills + " mortally\n";
                log.AppendText(message);

            }
            //Normal Melee
            else
            {

                //Number of hits
                int hits = 0;
                for (int i = 0; i < usable; i++)
                {
                    if (random.Next(0, 100) < meleeAttack * 100)
                        hits++;
                }

                //Number of kills
                for (int i = 0; i < hits; i++)
                {
                    if (random.Next(0, 100) > target.meleeDefense * 100)
                        kills++;
                }

                //Condition Cost
                currentCondition -= Combat.conditionMelee * conditionCostModifier;

                //Message for log
                //TODO Log defended hits
                //TODO Look at RichTextBox for multi color text box(Color for each side)
                log.AppendText(who);
                string message = " melee'd " + usable + " times, " + kills + " mortally\n";
                log.AppendText(message);

            }

            return kills;
        }

        //Apply casualities and morale damage
        public void Casualties(int number, string who, float moraleModifier)
        {

            //No Casualities
            if (number == 0)
                return;

            //Real casualities(Can't lose more than current amount)
            int trueCasualities = Math.Min(number, currentMen);

            //Remove casualities
            currentMen -= trueCasualities;

            //Base Morale impact as percentage of total men
            float baseMoraleLost = trueCasualities / (float)maxMen;

            //Apply final morale
            currentMorale -= baseMoraleLost * moraleModifier;

            //Recalculate max morale
            CalculateMaxMorale();

            //Recalculate min morale
            CalculateMinMorale();

            //Display in log
            log.AppendText(who + " morale: " + currentMorale + " minimum: " + currentMinimumMorale);
            log.AppendText(Environment.NewLine);

        }

        //Recovery of stats(morale, condition)
        public void Recovery()
        {

            //Recover Condition
            if (currentCondition < 1)
            {
                currentCondition += conditionRecovery * conditionCostModifier;
                if (currentCondition > 1)
                    currentCondition = 1;
            }

            //Recover Morale
            if (currentMorale < currentMaxMorale)
            {
                currentMorale += moraleRecover;
                if (currentMorale > currentMaxMorale)
                    currentMorale = currentMaxMorale;
            }

        }

        //Calculate situation minimum morale
        public void CalculateMinMorale()
        {
            //Base value
            currentMinimumMorale = minimumMorale;

            //Add condition impact on minimum morale
            currentMinimumMorale += (1 - currentCondition) / Combat.conditionMinMoraleDivider;

            //TODO Add weather impact to minimum morale(negated by Acclimatization)

        }

        //Morale check
        public bool MoraleCheck()
        {
            return (currentMorale < currentMinimumMorale);
        }

    }
}
