using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VoronoiTacticsCombatCalculator
{
    class Unit
    {

        public int men;
        public int guns;
        public int menPerGun;
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

        public bool charge;
        public float chargeAttack;
        public float chargeMoraleImpact;

        public float rangedDefense;
        public float meleeDefense;
        public float chargeDefense;

    }
}
