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
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void Start_Click(object sender, EventArgs e)
        {
            Unit A = new Unit();
            A.men = Int32.Parse(this.menA.Text);
            A.guns = Int32.Parse(this.gunsA.Text);
            A.menPerGun = Int32.Parse(this.menPerGunA.Text);
            A.moraleRecover = float.Parse(this.recoverA.Text);
            A.minimumMorale = float.Parse(this.minimumMoraleA.Text);
            A.speed = Int32.Parse(this.speedA.Text);
            A.fatigueModifier = float.Parse(this.fatigueModA.Text);
            A.ranged = this.rangedA.Enabled;
            A.rangedAccuracy = float.Parse(this.rangedAccuracyA.Text);
            A.rangedAttack = float.Parse(this.rangedAttackA.Text);
            A.rangedTargets = Int32.Parse(this.rangedTargetsA.Text);
            A.rangedReload = Int32.Parse(this.reloadA.Text);
            A.melee = this.meleeA.Enabled;
            A.meleeAttack = float.Parse(this.meleeAttackA.Text);
            A.meleeTime = Int32.Parse(this.meleeTimeA.Text);
            A.rangedDefense = float.Parse(this.rangedDefenseA.Text);
            A.meleeDefense = float.Parse(this.meleeDefenseA.Text);
            A.chargeDefense = float.Parse(this.chargeDefenseA.Text);
            A.charge = this.chargeA.Enabled;
            A.chargeAttack = float.Parse(this.chargeAttackA.Text);
            A.chargeMoraleImpact = float.Parse(this.chargeMoraleImpactA.Text);

            Unit B = new Unit();
            B.men = Int32.Parse(this.menB.Text);
            B.guns = Int32.Parse(this.gunsB.Text);
            //TODO add other stats

            Terrain a = new Terrain();
            //TODO add other stats

            Terrain b = new Terrain();
            //TODO add other stats

            Connection c = new Connection();
            c.river = float.Parse(this.river.Text);
            c.distance = Int32.Parse(this.distance.Text);
            //TODO probably would have other stats(fortifications,etc)

            //Create combat
            Combat combat = new Combat(A, B, a, b, c);
        }
    }
}
