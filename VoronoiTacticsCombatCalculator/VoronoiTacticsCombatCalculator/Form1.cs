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
    public partial class Form1 : Form
    {

        int time;

        public Combat combat;
        public Unit unitA;
        public Unit unitB;
        public Terrain terrainA;
        public Terrain terrainB;
        public Connection connection;

        public Form1()
        {
            InitializeComponent();
        }

        private void Start_Click(object sender, EventArgs e)
        {

            //Unit A setup
            unitA = new Unit(this.CombatLog);
            unitA.maxMen = Int32.Parse(this.menA.Text);
            //TEMPORARY starting = max
            unitA.currentMen = unitA.maxMen;
            //TEMPORARY morale starting at 100%
            unitA.currentMorale = 1;
            unitA.currentMaxMorale = 1;
            unitA.usable = Int32.Parse(this.usableA.Text);
            unitA.guns = Int32.Parse(this.gunsA.Text);
            unitA.menPerGun = Int32.Parse(this.menPerGunA.Text);
            unitA.moraleRecover = float.Parse(this.recoverA.Text);
            unitA.minimumMorale = float.Parse(this.minimumMoraleA.Text);
            unitA.speed = Int32.Parse(this.speedA.Text);
            unitA.weight = float.Parse(this.weightA.Text);
            unitA.ranged = this.rangedA.Checked;
            unitA.rangedAccuracy = float.Parse(this.rangedAccuracyA.Text);
            unitA.rangedAttack = float.Parse(this.rangedAttackA.Text);
            unitA.rangedTargets = Int32.Parse(this.rangedTargetsA.Text);
            unitA.rangedReload = Int32.Parse(this.reloadA.Text);
            unitA.reloadTimer = unitA.rangedReload;
            unitA.melee = this.meleeA.Checked;
            unitA.meleeAttack = float.Parse(this.meleeAttackA.Text);
            unitA.meleeTime = Int32.Parse(this.meleeTimeA.Text);
            unitA.meleeTimer = unitA.meleeTime;
            unitA.rangedDefense = float.Parse(this.rangedDefenseA.Text);
            unitA.meleeDefense = float.Parse(this.meleeDefenseA.Text);
            unitA.chargeDefense = float.Parse(this.chargeDefenseA.Text);
            unitA.charge = this.chargeA.Checked;
            unitA.chargeAttack = float.Parse(this.chargeAttackA.Text);
            unitA.chargeMoraleImpact = float.Parse(this.chargeMoraleImpactA.Text);

            //Unit B setup
            unitB = new Unit(this.CombatLog);
            unitB.maxMen = Int32.Parse(this.menB.Text);
            //TEMPORARY starting = max
            unitB.currentMen = unitB.maxMen;
            //TEMPORARY morale starting at 100%
            unitB.currentMorale = 1;
            unitB.currentMaxMorale = 1;
            unitB.usable = Int32.Parse(this.usableB.Text);
            unitB.guns = Int32.Parse(this.gunsB.Text);
            unitB.menPerGun = Int32.Parse(this.menPerGunB.Text);
            unitB.moraleRecover = float.Parse(this.recoverB.Text);
            unitB.minimumMorale = float.Parse(this.minimumMoraleB.Text);
            unitB.speed = Int32.Parse(this.speedB.Text);
            unitB.weight = float.Parse(this.weightB.Text);
            unitB.ranged = this.rangedB.Checked;
            unitB.rangedAccuracy = float.Parse(this.rangedAccuracyB.Text);
            unitB.rangedAttack = float.Parse(this.rangedAttackB.Text);
            unitB.rangedTargets = Int32.Parse(this.rangedTargetsB.Text);
            unitB.rangedReload = Int32.Parse(this.reloadB.Text);
            unitB.reloadTimer = unitB.rangedReload;
            unitB.melee = this.meleeB.Checked;
            unitB.meleeAttack = float.Parse(this.meleeAttackB.Text);
            unitB.meleeTime = Int32.Parse(this.meleeTimeB.Text);
            unitB.meleeTimer = unitB.meleeTime;
            unitB.rangedDefense = float.Parse(this.rangedDefenseB.Text);
            unitB.meleeDefense = float.Parse(this.meleeDefenseB.Text);
            unitB.chargeDefense = float.Parse(this.chargeDefenseB.Text);
            unitB.charge = this.chargeB.Checked;
            unitB.chargeAttack = float.Parse(this.chargeAttackB.Text);
            unitB.chargeMoraleImpact = float.Parse(this.chargeMoraleImpactB.Text);

            //Terrain A setup
            terrainA = new Terrain();
            terrainA.attack = float.Parse(this.attackA.Text);
            terrainA.defense = float.Parse(this.defenseA.Text);
            terrainA.movement = float.Parse(this.movementA.Text);
            terrainA.heightImpact = Int32.Parse(this.heightA.Text);

            //Terrain B setup
            terrainB = new Terrain();
            terrainB.attack = float.Parse(this.attackB.Text);
            terrainB.defense = float.Parse(this.defenseB.Text);
            terrainB.movement = float.Parse(this.movementB.Text);
            terrainB.heightImpact = Int32.Parse(this.heightB.Text);

            //Connection setup
            connection = new Connection();
            connection.river = float.Parse(this.river.Text);
            connection.distance = Int32.Parse(this.distance.Text);
            //TODO probably would have other stats(fortifications,etc)

            //Enable timer
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;

            //Reset time
            time = 0;

            //Reset UI things
            this.StartRangedA.Enabled = true;
            this.StartRangedB.Enabled = true;
            this.StartMeleeA.Enabled = true;
            this.StartMeleeB.Enabled = true;

            //Dispose of previous combat
            combat = null;

            //First call (to display time right away)
            timer1_Tick(this, new EventArgs());
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Time
            int seconds = this.time;
            int minutes = seconds / 60;
            int hours = minutes / 60;

            //0-59 seconds
            seconds %= 60;

            //Time string
            string sTime = hours.ToString() + "h " + minutes.ToString() + "m " + seconds.ToString() + "s ";

            //Display on UI
            this.Time.Text = sTime;

            //Increment time(seconds)
            time++;

            //Process Combat
            if (combat != null)
                combat.Process();
        }

        private void Resume_Click(object sender, EventArgs e)
        {
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
        }

        private void Fast_Click(object sender, EventArgs e)
        {
            this.timer1.Enabled = true;
            this.timer1.Interval = 250;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.timer1.Enabled = true;
            this.timer1.Interval = 100;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.timer1.Enabled = true;
            this.timer1.Interval = 50;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.timer1.Enabled = true;
            this.timer1.Interval = 10;
        }

        private void Pause_Click(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
        }

        private void StartRangedA_Click(object sender, EventArgs e)
        {

            //Create combat
            combat = new Combat(this.CombatLog, unitA, unitB, terrainA, terrainB, connection, Combat.Phase.RANGED);

            //Disable ranged attack buttons
            this.StartRangedA.Enabled = false;
            this.StartRangedB.Enabled = false;

        }

        private void StartRangedB_Click(object sender, EventArgs e)
        {

            //TODO Same as StartRangedA_Click

        }

        private void StartMeleeA_Click(object sender, EventArgs e)
        {

            //Create combat
            combat = new Combat(this.CombatLog, unitA, unitB, terrainA, terrainB, connection, Combat.Phase.MELEE);

            //Disable ranged attack buttons
            this.StartRangedA.Enabled = false;
            this.StartRangedB.Enabled = false;

            //Disable melee attack buttons
            this.StartMeleeA.Enabled = false;
            this.StartMeleeB.Enabled = false;

        }

        private void StartMeleeB_Click(object sender, EventArgs e)
        {

            //TODO Same as StartMeleeA_Click

        }

    }
}
