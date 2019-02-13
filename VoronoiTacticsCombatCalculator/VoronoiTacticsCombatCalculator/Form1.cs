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

        int time;
        public Combat combat;

        public Form1()
        {
            InitializeComponent();
        }

        private void Start_Click(object sender, EventArgs e)
        {

            //Unit A setup
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

            //Unit B setup
            Unit B = new Unit();
            B.men = Int32.Parse(this.menB.Text);
            B.guns = Int32.Parse(this.gunsB.Text);
            B.menPerGun = Int32.Parse(this.menPerGunB.Text);
            B.moraleRecover = float.Parse(this.recoverB.Text);
            B.minimumMorale = float.Parse(this.minimumMoraleB.Text);
            B.speed = Int32.Parse(this.speedB.Text);
            B.fatigueModifier = float.Parse(this.fatigueModB.Text);
            B.ranged = this.rangedB.Enabled;
            B.rangedAccuracy = float.Parse(this.rangedAccuracyB.Text);
            B.rangedAttack = float.Parse(this.rangedAttackB.Text);
            B.rangedTargets = Int32.Parse(this.rangedTargetsB.Text);
            B.rangedReload = Int32.Parse(this.reloadB.Text);
            B.melee = this.meleeB.Enabled;
            B.meleeAttack = float.Parse(this.meleeAttackB.Text);
            B.meleeTime = Int32.Parse(this.meleeTimeB.Text);
            B.rangedDefense = float.Parse(this.rangedDefenseB.Text);
            B.meleeDefense = float.Parse(this.meleeDefenseB.Text);
            B.chargeDefense = float.Parse(this.chargeDefenseB.Text);
            B.charge = this.chargeB.Enabled;
            B.chargeAttack = float.Parse(this.chargeAttackB.Text);
            B.chargeMoraleImpact = float.Parse(this.chargeMoraleImpactB.Text);

            //Terrain A setup
            Terrain a = new Terrain();
            a.attack = float.Parse(this.attackA.Text);
            a.defense = float.Parse(this.defenseA.Text);
            a.movement = float.Parse(this.movementA.Text);
            a.heightImpact = Int32.Parse(this.heightA.Text);

            //Terrain B setup
            Terrain b = new Terrain();
            b.attack = float.Parse(this.attackB.Text);
            b.defense = float.Parse(this.defenseB.Text);
            b.movement = float.Parse(this.movementB.Text);
            b.heightImpact = Int32.Parse(this.heightB.Text);

            //Connection setup
            Connection c = new Connection();
            c.river = float.Parse(this.river.Text);
            c.distance = Int32.Parse(this.distance.Text);
            //TODO probably would have other stats(fortifications,etc)

            //Create combat
            combat = new Combat(A, B, a, b, c);

            //Enable timer
            this.timer1.Enabled = true;

            //Reset time
            time = 0;

            //First call (to display time right away)
            timer1_Tick(this, new EventArgs() );
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Time
            int seconds = this.time;
            int minutes = seconds / 60;
            int hours = minutes / 60;

            //Time string
            string sTime = hours.ToString() + "h " + minutes.ToString() + "m " + seconds.ToString() + "s ";

            //Display on UI
            this.Time.Text = sTime;

            //Increment time(seconds)
            time++;
        }
    }
}
