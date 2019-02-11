using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VoronoiTacticsCombatCalculator
{
    class Unit
    {

        //Unit data

        public int number;
        public float morale;
        public float moraleRecover;
        public float speed;
        public float fatigueModifier;

        public bool ranged;
        public float rangedAccuracy;
        public float rangedAttack;
        public int rangedTargets;
        public int rangedReload;

        public bool melee;
        public float meleeAttack;
        public int meleeTime;

        public float rangedDefense;
        public float meleeDefense;

        public bool charge;
        public float chargeAttack;

        //Terrain data (Just for test here)

    }
}
