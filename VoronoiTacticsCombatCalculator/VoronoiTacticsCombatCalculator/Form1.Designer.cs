using System.Drawing;

namespace VoronoiTacticsCombatCalculator
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Begin = new System.Windows.Forms.Button();
            this.CombatLog = new System.Windows.Forms.TextBox();
            this.A = new System.Windows.Forms.Label();
            this.B = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.menA = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.minimumMoraleA = new System.Windows.Forms.TextBox();
            this.recoverA = new System.Windows.Forms.TextBox();
            this.rangedAccuracyA = new System.Windows.Forms.TextBox();
            this.rangedAttackA = new System.Windows.Forms.TextBox();
            this.rangedTargetsA = new System.Windows.Forms.TextBox();
            this.reloadA = new System.Windows.Forms.TextBox();
            this.meleeAttackA = new System.Windows.Forms.TextBox();
            this.meleeTimeA = new System.Windows.Forms.TextBox();
            this.rangedDefenseA = new System.Windows.Forms.TextBox();
            this.meleeDefenseA = new System.Windows.Forms.TextBox();
            this.rangedA = new System.Windows.Forms.CheckBox();
            this.meleeA = new System.Windows.Forms.CheckBox();
            this.meleeB = new System.Windows.Forms.CheckBox();
            this.rangedB = new System.Windows.Forms.CheckBox();
            this.meleeDefenseB = new System.Windows.Forms.TextBox();
            this.rangedDefenseB = new System.Windows.Forms.TextBox();
            this.meleeTimeB = new System.Windows.Forms.TextBox();
            this.meleeAttackB = new System.Windows.Forms.TextBox();
            this.reloadB = new System.Windows.Forms.TextBox();
            this.rangedTargetsB = new System.Windows.Forms.TextBox();
            this.rangedAttackB = new System.Windows.Forms.TextBox();
            this.rangedAccuracyB = new System.Windows.Forms.TextBox();
            this.recoverB = new System.Windows.Forms.TextBox();
            this.minimumMoraleB = new System.Windows.Forms.TextBox();
            this.menB = new System.Windows.Forms.TextBox();
            this.Resume = new System.Windows.Forms.Button();
            this.Pause = new System.Windows.Forms.Button();
            this.Fast = new System.Windows.Forms.Button();
            this.chargeA = new System.Windows.Forms.CheckBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.chargeAttackA = new System.Windows.Forms.TextBox();
            this.chargeAttackB = new System.Windows.Forms.TextBox();
            this.chargeB = new System.Windows.Forms.CheckBox();
            this.Swap = new System.Windows.Forms.Button();
            this.Time = new System.Windows.Forms.TextBox();
            this.StartRangedA = new System.Windows.Forms.Button();
            this.StartMeleeA = new System.Windows.Forms.Button();
            this.StartMeleeB = new System.Windows.Forms.Button();
            this.StartRangedB = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.speedA = new System.Windows.Forms.TextBox();
            this.fatigueModA = new System.Windows.Forms.TextBox();
            this.fatigueModB = new System.Windows.Forms.TextBox();
            this.speedB = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.distance = new System.Windows.Forms.TextBox();
            this.defenseA = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.attackA = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.attackB = new System.Windows.Forms.TextBox();
            this.defenseB = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.movementA = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.movementB = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.river = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.heightA = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.heightB = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.chargeMoraleImpactB = new System.Windows.Forms.TextBox();
            this.chargeMoraleImpactA = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.gunsB = new System.Windows.Forms.TextBox();
            this.gunsA = new System.Windows.Forms.TextBox();
            this.label30 = new System.Windows.Forms.Label();
            this.menPerGunB = new System.Windows.Forms.TextBox();
            this.menPerGunA = new System.Windows.Forms.TextBox();
            this.label31 = new System.Windows.Forms.Label();
            this.chargeDefenseB = new System.Windows.Forms.TextBox();
            this.chargeDefenseA = new System.Windows.Forms.TextBox();
            this.label32 = new System.Windows.Forms.Label();
            this.tiles = new System.Windows.Forms.TextBox();
            this.label33 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.usableB = new System.Windows.Forms.TextBox();
            this.usableA = new System.Windows.Forms.TextBox();
            this.label34 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Begin
            // 
            this.Begin.Location = new System.Drawing.Point(46, 884);
            this.Begin.Name = "Begin";
            this.Begin.Size = new System.Drawing.Size(103, 38);
            this.Begin.TabIndex = 1;
            this.Begin.Text = "Begin";
            this.Begin.UseVisualStyleBackColor = true;
            this.Begin.Click += new System.EventHandler(this.Start_Click);
            // 
            // CombatLog
            // 
            this.CombatLog.Location = new System.Drawing.Point(572, 12);
            this.CombatLog.Multiline = true;
            this.CombatLog.Name = "CombatLog";
            this.CombatLog.ReadOnly = true;
            this.CombatLog.Size = new System.Drawing.Size(420, 932);
            this.CombatLog.TabIndex = 2;
            // 
            // A
            // 
            this.A.AutoSize = true;
            this.A.Font = new System.Drawing.Font("Arial", 15F);
            this.A.ForeColor = System.Drawing.Color.Red;
            this.A.Location = new System.Drawing.Point(198, 147);
            this.A.Name = "A";
            this.A.Size = new System.Drawing.Size(23, 23);
            this.A.TabIndex = 3;
            this.A.Text = "A";
            // 
            // B
            // 
            this.B.AutoSize = true;
            this.B.BackColor = System.Drawing.Color.White;
            this.B.Font = new System.Drawing.Font("Arial", 15F);
            this.B.ForeColor = System.Drawing.Color.Blue;
            this.B.Location = new System.Drawing.Point(415, 147);
            this.B.Name = "B";
            this.B.Size = new System.Drawing.Size(23, 23);
            this.B.TabIndex = 4;
            this.B.Text = "B";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 187);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Men";
            // 
            // menA
            // 
            this.menA.Location = new System.Drawing.Point(174, 184);
            this.menA.Name = "menA";
            this.menA.Size = new System.Drawing.Size(64, 20);
            this.menA.TabIndex = 6;
            this.menA.Text = "300";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 301);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Minimum Morale";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 331);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Morale Recover";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 418);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Ranged";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 447);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Ranged Accuracy";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(30, 478);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Ranged Attack";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(30, 509);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(84, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Ranged Targets";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(30, 542);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "Reload";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(30, 574);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(36, 13);
            this.label9.TabIndex = 14;
            this.label9.Text = "Melee";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(30, 601);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(70, 13);
            this.label10.TabIndex = 15;
            this.label10.Text = "Melee Attack";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(30, 628);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(62, 13);
            this.label11.TabIndex = 16;
            this.label11.Text = "Melee Time";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(30, 656);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(88, 13);
            this.label12.TabIndex = 17;
            this.label12.Text = "Ranged Defense";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(30, 686);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(79, 13);
            this.label13.TabIndex = 18;
            this.label13.Text = "Melee Defense";
            // 
            // minimumMoraleA
            // 
            this.minimumMoraleA.Location = new System.Drawing.Point(174, 298);
            this.minimumMoraleA.Name = "minimumMoraleA";
            this.minimumMoraleA.Size = new System.Drawing.Size(64, 20);
            this.minimumMoraleA.TabIndex = 19;
            this.minimumMoraleA.Text = "0.3";
            // 
            // recoverA
            // 
            this.recoverA.Location = new System.Drawing.Point(174, 328);
            this.recoverA.Name = "recoverA";
            this.recoverA.Size = new System.Drawing.Size(64, 20);
            this.recoverA.TabIndex = 20;
            this.recoverA.Text = "0.001";
            // 
            // rangedAccuracyA
            // 
            this.rangedAccuracyA.Location = new System.Drawing.Point(174, 444);
            this.rangedAccuracyA.Name = "rangedAccuracyA";
            this.rangedAccuracyA.Size = new System.Drawing.Size(64, 20);
            this.rangedAccuracyA.TabIndex = 21;
            this.rangedAccuracyA.Text = "0.4";
            // 
            // rangedAttackA
            // 
            this.rangedAttackA.Location = new System.Drawing.Point(174, 475);
            this.rangedAttackA.Name = "rangedAttackA";
            this.rangedAttackA.Size = new System.Drawing.Size(64, 20);
            this.rangedAttackA.TabIndex = 22;
            this.rangedAttackA.Text = "0.8";
            // 
            // rangedTargetsA
            // 
            this.rangedTargetsA.Location = new System.Drawing.Point(174, 506);
            this.rangedTargetsA.Name = "rangedTargetsA";
            this.rangedTargetsA.Size = new System.Drawing.Size(64, 20);
            this.rangedTargetsA.TabIndex = 23;
            this.rangedTargetsA.Text = "1";
            // 
            // reloadA
            // 
            this.reloadA.Location = new System.Drawing.Point(174, 539);
            this.reloadA.Name = "reloadA";
            this.reloadA.Size = new System.Drawing.Size(64, 20);
            this.reloadA.TabIndex = 24;
            this.reloadA.Text = "20";
            // 
            // meleeAttackA
            // 
            this.meleeAttackA.Location = new System.Drawing.Point(174, 598);
            this.meleeAttackA.Name = "meleeAttackA";
            this.meleeAttackA.Size = new System.Drawing.Size(64, 20);
            this.meleeAttackA.TabIndex = 25;
            this.meleeAttackA.Text = "0.3";
            // 
            // meleeTimeA
            // 
            this.meleeTimeA.Location = new System.Drawing.Point(174, 625);
            this.meleeTimeA.Name = "meleeTimeA";
            this.meleeTimeA.Size = new System.Drawing.Size(64, 20);
            this.meleeTimeA.TabIndex = 26;
            this.meleeTimeA.Text = "10";
            // 
            // rangedDefenseA
            // 
            this.rangedDefenseA.Location = new System.Drawing.Point(174, 653);
            this.rangedDefenseA.Name = "rangedDefenseA";
            this.rangedDefenseA.Size = new System.Drawing.Size(64, 20);
            this.rangedDefenseA.TabIndex = 27;
            this.rangedDefenseA.Text = "0.2";
            // 
            // meleeDefenseA
            // 
            this.meleeDefenseA.Location = new System.Drawing.Point(174, 683);
            this.meleeDefenseA.Name = "meleeDefenseA";
            this.meleeDefenseA.Size = new System.Drawing.Size(64, 20);
            this.meleeDefenseA.TabIndex = 28;
            this.meleeDefenseA.Text = "0.1";
            // 
            // rangedA
            // 
            this.rangedA.AutoSize = true;
            this.rangedA.Checked = true;
            this.rangedA.CheckState = System.Windows.Forms.CheckState.Checked;
            this.rangedA.Location = new System.Drawing.Point(223, 418);
            this.rangedA.Name = "rangedA";
            this.rangedA.Size = new System.Drawing.Size(15, 14);
            this.rangedA.TabIndex = 29;
            this.rangedA.UseVisualStyleBackColor = true;
            // 
            // meleeA
            // 
            this.meleeA.AutoSize = true;
            this.meleeA.Checked = true;
            this.meleeA.CheckState = System.Windows.Forms.CheckState.Checked;
            this.meleeA.Location = new System.Drawing.Point(223, 573);
            this.meleeA.Name = "meleeA";
            this.meleeA.Size = new System.Drawing.Size(15, 14);
            this.meleeA.TabIndex = 30;
            this.meleeA.UseVisualStyleBackColor = true;
            // 
            // meleeB
            // 
            this.meleeB.AutoSize = true;
            this.meleeB.Checked = true;
            this.meleeB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.meleeB.Location = new System.Drawing.Point(439, 573);
            this.meleeB.Name = "meleeB";
            this.meleeB.Size = new System.Drawing.Size(15, 14);
            this.meleeB.TabIndex = 43;
            this.meleeB.UseVisualStyleBackColor = true;
            // 
            // rangedB
            // 
            this.rangedB.AutoSize = true;
            this.rangedB.Checked = true;
            this.rangedB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.rangedB.Location = new System.Drawing.Point(439, 418);
            this.rangedB.Name = "rangedB";
            this.rangedB.Size = new System.Drawing.Size(15, 14);
            this.rangedB.TabIndex = 42;
            this.rangedB.UseVisualStyleBackColor = true;
            // 
            // meleeDefenseB
            // 
            this.meleeDefenseB.Location = new System.Drawing.Point(390, 683);
            this.meleeDefenseB.Name = "meleeDefenseB";
            this.meleeDefenseB.Size = new System.Drawing.Size(64, 20);
            this.meleeDefenseB.TabIndex = 41;
            this.meleeDefenseB.Text = "0.1";
            // 
            // rangedDefenseB
            // 
            this.rangedDefenseB.Location = new System.Drawing.Point(390, 653);
            this.rangedDefenseB.Name = "rangedDefenseB";
            this.rangedDefenseB.Size = new System.Drawing.Size(64, 20);
            this.rangedDefenseB.TabIndex = 40;
            this.rangedDefenseB.Text = "0.2";
            // 
            // meleeTimeB
            // 
            this.meleeTimeB.Location = new System.Drawing.Point(390, 625);
            this.meleeTimeB.Name = "meleeTimeB";
            this.meleeTimeB.Size = new System.Drawing.Size(64, 20);
            this.meleeTimeB.TabIndex = 39;
            this.meleeTimeB.Text = "10";
            // 
            // meleeAttackB
            // 
            this.meleeAttackB.Location = new System.Drawing.Point(390, 598);
            this.meleeAttackB.Name = "meleeAttackB";
            this.meleeAttackB.Size = new System.Drawing.Size(64, 20);
            this.meleeAttackB.TabIndex = 38;
            this.meleeAttackB.Text = "0.3";
            // 
            // reloadB
            // 
            this.reloadB.Location = new System.Drawing.Point(390, 539);
            this.reloadB.Name = "reloadB";
            this.reloadB.Size = new System.Drawing.Size(64, 20);
            this.reloadB.TabIndex = 37;
            this.reloadB.Text = "20";
            // 
            // rangedTargetsB
            // 
            this.rangedTargetsB.Location = new System.Drawing.Point(390, 506);
            this.rangedTargetsB.Name = "rangedTargetsB";
            this.rangedTargetsB.Size = new System.Drawing.Size(64, 20);
            this.rangedTargetsB.TabIndex = 36;
            this.rangedTargetsB.Text = "1";
            // 
            // rangedAttackB
            // 
            this.rangedAttackB.Location = new System.Drawing.Point(390, 475);
            this.rangedAttackB.Name = "rangedAttackB";
            this.rangedAttackB.Size = new System.Drawing.Size(64, 20);
            this.rangedAttackB.TabIndex = 35;
            this.rangedAttackB.Text = "0.8";
            // 
            // rangedAccuracyB
            // 
            this.rangedAccuracyB.Location = new System.Drawing.Point(390, 444);
            this.rangedAccuracyB.Name = "rangedAccuracyB";
            this.rangedAccuracyB.Size = new System.Drawing.Size(64, 20);
            this.rangedAccuracyB.TabIndex = 34;
            this.rangedAccuracyB.Text = "0.4";
            // 
            // recoverB
            // 
            this.recoverB.Location = new System.Drawing.Point(390, 328);
            this.recoverB.Name = "recoverB";
            this.recoverB.Size = new System.Drawing.Size(64, 20);
            this.recoverB.TabIndex = 33;
            this.recoverB.Text = "0.001";
            // 
            // minimumMoraleB
            // 
            this.minimumMoraleB.Location = new System.Drawing.Point(390, 298);
            this.minimumMoraleB.Name = "minimumMoraleB";
            this.minimumMoraleB.Size = new System.Drawing.Size(64, 20);
            this.minimumMoraleB.TabIndex = 32;
            this.minimumMoraleB.Text = "0.3";
            // 
            // menB
            // 
            this.menB.Location = new System.Drawing.Point(390, 183);
            this.menB.Name = "menB";
            this.menB.Size = new System.Drawing.Size(64, 20);
            this.menB.TabIndex = 31;
            this.menB.Text = "300";
            // 
            // Resume
            // 
            this.Resume.Location = new System.Drawing.Point(218, 884);
            this.Resume.Name = "Resume";
            this.Resume.Size = new System.Drawing.Size(30, 38);
            this.Resume.TabIndex = 44;
            this.Resume.Text = "x1";
            this.Resume.UseVisualStyleBackColor = true;
            this.Resume.Click += new System.EventHandler(this.Resume_Click);
            // 
            // Pause
            // 
            this.Pause.Location = new System.Drawing.Point(182, 884);
            this.Pause.Name = "Pause";
            this.Pause.Size = new System.Drawing.Size(30, 38);
            this.Pause.TabIndex = 45;
            this.Pause.Text = "| |";
            this.Pause.UseVisualStyleBackColor = true;
            // 
            // Fast
            // 
            this.Fast.Location = new System.Drawing.Point(254, 884);
            this.Fast.Name = "Fast";
            this.Fast.Size = new System.Drawing.Size(30, 38);
            this.Fast.TabIndex = 46;
            this.Fast.Text = "x4";
            this.Fast.UseVisualStyleBackColor = true;
            this.Fast.Click += new System.EventHandler(this.Fast_Click);
            // 
            // chargeA
            // 
            this.chargeA.AutoSize = true;
            this.chargeA.Location = new System.Drawing.Point(223, 747);
            this.chargeA.Name = "chargeA";
            this.chargeA.Size = new System.Drawing.Size(15, 14);
            this.chargeA.TabIndex = 48;
            this.chargeA.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(30, 748);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(41, 13);
            this.label14.TabIndex = 47;
            this.label14.Text = "Charge";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(30, 775);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(75, 13);
            this.label15.TabIndex = 49;
            this.label15.Text = "Charge Attack";
            // 
            // chargeAttackA
            // 
            this.chargeAttackA.Location = new System.Drawing.Point(174, 772);
            this.chargeAttackA.Name = "chargeAttackA";
            this.chargeAttackA.Size = new System.Drawing.Size(64, 20);
            this.chargeAttackA.TabIndex = 50;
            this.chargeAttackA.Text = "0";
            // 
            // chargeAttackB
            // 
            this.chargeAttackB.Location = new System.Drawing.Point(390, 772);
            this.chargeAttackB.Name = "chargeAttackB";
            this.chargeAttackB.Size = new System.Drawing.Size(64, 20);
            this.chargeAttackB.TabIndex = 52;
            this.chargeAttackB.Text = "0";
            // 
            // chargeB
            // 
            this.chargeB.AutoSize = true;
            this.chargeB.Location = new System.Drawing.Point(439, 747);
            this.chargeB.Name = "chargeB";
            this.chargeB.Size = new System.Drawing.Size(15, 14);
            this.chargeB.TabIndex = 51;
            this.chargeB.UseVisualStyleBackColor = true;
            // 
            // Swap
            // 
            this.Swap.Location = new System.Drawing.Point(294, 147);
            this.Swap.Name = "Swap";
            this.Swap.Size = new System.Drawing.Size(40, 38);
            this.Swap.TabIndex = 53;
            this.Swap.Text = "<=>";
            this.Swap.UseVisualStyleBackColor = true;
            // 
            // Time
            // 
            this.Time.Location = new System.Drawing.Point(402, 894);
            this.Time.Name = "Time";
            this.Time.ReadOnly = true;
            this.Time.Size = new System.Drawing.Size(112, 20);
            this.Time.TabIndex = 58;
            // 
            // StartRangedA
            // 
            this.StartRangedA.Enabled = false;
            this.StartRangedA.Location = new System.Drawing.Point(155, 838);
            this.StartRangedA.Name = "StartRangedA";
            this.StartRangedA.Size = new System.Drawing.Size(80, 31);
            this.StartRangedA.TabIndex = 60;
            this.StartRangedA.Text = "Start Ranged";
            this.StartRangedA.UseVisualStyleBackColor = true;
            this.StartRangedA.Click += new System.EventHandler(this.StartRangedA_Click);
            // 
            // StartMeleeA
            // 
            this.StartMeleeA.Enabled = false;
            this.StartMeleeA.Location = new System.Drawing.Point(254, 838);
            this.StartMeleeA.Name = "StartMeleeA";
            this.StartMeleeA.Size = new System.Drawing.Size(80, 31);
            this.StartMeleeA.TabIndex = 61;
            this.StartMeleeA.Text = "Start Melee";
            this.StartMeleeA.UseVisualStyleBackColor = true;
            // 
            // StartMeleeB
            // 
            this.StartMeleeB.Enabled = false;
            this.StartMeleeB.Location = new System.Drawing.Point(471, 838);
            this.StartMeleeB.Name = "StartMeleeB";
            this.StartMeleeB.Size = new System.Drawing.Size(80, 31);
            this.StartMeleeB.TabIndex = 63;
            this.StartMeleeB.Text = "Start Melee";
            this.StartMeleeB.UseVisualStyleBackColor = true;
            // 
            // StartRangedB
            // 
            this.StartRangedB.Enabled = false;
            this.StartRangedB.Location = new System.Drawing.Point(372, 838);
            this.StartRangedB.Name = "StartRangedB";
            this.StartRangedB.Size = new System.Drawing.Size(80, 31);
            this.StartRangedB.TabIndex = 62;
            this.StartRangedB.Text = "Start Ranged";
            this.StartRangedB.UseVisualStyleBackColor = true;
            this.StartRangedB.Click += new System.EventHandler(this.StartRangedB_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(31, 360);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(38, 13);
            this.label16.TabIndex = 64;
            this.label16.Text = "Speed";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(31, 388);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(82, 13);
            this.label17.TabIndex = 65;
            this.label17.Text = "Fatigue Modifier";
            // 
            // speedA
            // 
            this.speedA.Location = new System.Drawing.Point(174, 357);
            this.speedA.Name = "speedA";
            this.speedA.Size = new System.Drawing.Size(64, 20);
            this.speedA.TabIndex = 66;
            this.speedA.Text = "10";
            // 
            // fatigueModA
            // 
            this.fatigueModA.Location = new System.Drawing.Point(174, 385);
            this.fatigueModA.Name = "fatigueModA";
            this.fatigueModA.Size = new System.Drawing.Size(64, 20);
            this.fatigueModA.TabIndex = 67;
            this.fatigueModA.Text = "1.0";
            // 
            // fatigueModB
            // 
            this.fatigueModB.Location = new System.Drawing.Point(390, 381);
            this.fatigueModB.Name = "fatigueModB";
            this.fatigueModB.Size = new System.Drawing.Size(64, 20);
            this.fatigueModB.TabIndex = 69;
            this.fatigueModB.Text = "1.0";
            // 
            // speedB
            // 
            this.speedB.Location = new System.Drawing.Point(390, 353);
            this.speedB.Name = "speedB";
            this.speedB.Size = new System.Drawing.Size(64, 20);
            this.speedB.TabIndex = 68;
            this.speedB.Text = "10";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(28, 50);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(49, 13);
            this.label18.TabIndex = 70;
            this.label18.Text = "Distance";
            // 
            // distance
            // 
            this.distance.Location = new System.Drawing.Point(111, 47);
            this.distance.Name = "distance";
            this.distance.Size = new System.Drawing.Size(64, 20);
            this.distance.TabIndex = 71;
            this.distance.Text = "100";
            // 
            // defenseA
            // 
            this.defenseA.Location = new System.Drawing.Point(262, 42);
            this.defenseA.Name = "defenseA";
            this.defenseA.Size = new System.Drawing.Size(64, 20);
            this.defenseA.TabIndex = 73;
            this.defenseA.Text = "1";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(199, 45);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(47, 13);
            this.label19.TabIndex = 72;
            this.label19.Text = "Defense";
            // 
            // attackA
            // 
            this.attackA.Location = new System.Drawing.Point(262, 12);
            this.attackA.Name = "attackA";
            this.attackA.Size = new System.Drawing.Size(64, 20);
            this.attackA.TabIndex = 75;
            this.attackA.Text = "1";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(199, 15);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(38, 13);
            this.label20.TabIndex = 74;
            this.label20.Text = "Attack";
            // 
            // attackB
            // 
            this.attackB.Location = new System.Drawing.Point(479, 12);
            this.attackB.Name = "attackB";
            this.attackB.Size = new System.Drawing.Size(64, 20);
            this.attackB.TabIndex = 77;
            this.attackB.Text = "1";
            // 
            // defenseB
            // 
            this.defenseB.Location = new System.Drawing.Point(479, 42);
            this.defenseB.Name = "defenseB";
            this.defenseB.Size = new System.Drawing.Size(64, 20);
            this.defenseB.TabIndex = 76;
            this.defenseB.Text = "1";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(416, 15);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(38, 13);
            this.label21.TabIndex = 79;
            this.label21.Text = "Attack";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(416, 45);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(47, 13);
            this.label22.TabIndex = 78;
            this.label22.Text = "Defense";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Arial", 15F);
            this.label23.Location = new System.Drawing.Point(30, 12);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(117, 23);
            this.label23.TabIndex = 80;
            this.label23.Text = "Tile Settings";
            // 
            // movementA
            // 
            this.movementA.Location = new System.Drawing.Point(262, 73);
            this.movementA.Name = "movementA";
            this.movementA.Size = new System.Drawing.Size(64, 20);
            this.movementA.TabIndex = 82;
            this.movementA.Text = "1";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(199, 76);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(57, 13);
            this.label24.TabIndex = 81;
            this.label24.Text = "Movement";
            // 
            // movementB
            // 
            this.movementB.Location = new System.Drawing.Point(479, 73);
            this.movementB.Name = "movementB";
            this.movementB.Size = new System.Drawing.Size(64, 20);
            this.movementB.TabIndex = 84;
            this.movementB.Text = "1";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(416, 76);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(57, 13);
            this.label25.TabIndex = 83;
            this.label25.Text = "Movement";
            // 
            // river
            // 
            this.river.Location = new System.Drawing.Point(111, 78);
            this.river.Name = "river";
            this.river.Size = new System.Drawing.Size(64, 20);
            this.river.TabIndex = 86;
            this.river.Text = "0";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(28, 81);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(32, 13);
            this.label26.TabIndex = 85;
            this.label26.Text = "River";
            // 
            // heightA
            // 
            this.heightA.Location = new System.Drawing.Point(262, 104);
            this.heightA.Name = "heightA";
            this.heightA.Size = new System.Drawing.Size(64, 20);
            this.heightA.TabIndex = 88;
            this.heightA.Text = "1";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(199, 107);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(38, 13);
            this.label27.TabIndex = 87;
            this.label27.Text = "Height";
            // 
            // heightB
            // 
            this.heightB.Location = new System.Drawing.Point(479, 104);
            this.heightB.Name = "heightB";
            this.heightB.Size = new System.Drawing.Size(64, 20);
            this.heightB.TabIndex = 90;
            this.heightB.Text = "1";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(416, 107);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(38, 13);
            this.label28.TabIndex = 89;
            this.label28.Text = "Height";
            // 
            // chargeMoraleImpactB
            // 
            this.chargeMoraleImpactB.Location = new System.Drawing.Point(391, 800);
            this.chargeMoraleImpactB.Name = "chargeMoraleImpactB";
            this.chargeMoraleImpactB.Size = new System.Drawing.Size(64, 20);
            this.chargeMoraleImpactB.TabIndex = 93;
            this.chargeMoraleImpactB.Text = "0";
            // 
            // chargeMoraleImpactA
            // 
            this.chargeMoraleImpactA.Location = new System.Drawing.Point(175, 800);
            this.chargeMoraleImpactA.Name = "chargeMoraleImpactA";
            this.chargeMoraleImpactA.Size = new System.Drawing.Size(64, 20);
            this.chargeMoraleImpactA.TabIndex = 92;
            this.chargeMoraleImpactA.Text = "0";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(31, 803);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(111, 13);
            this.label29.TabIndex = 91;
            this.label29.Text = "Charge Morale Impact";
            // 
            // gunsB
            // 
            this.gunsB.Location = new System.Drawing.Point(390, 242);
            this.gunsB.Name = "gunsB";
            this.gunsB.Size = new System.Drawing.Size(64, 20);
            this.gunsB.TabIndex = 96;
            this.gunsB.Text = "0";
            // 
            // gunsA
            // 
            this.gunsA.Location = new System.Drawing.Point(174, 242);
            this.gunsA.Name = "gunsA";
            this.gunsA.Size = new System.Drawing.Size(64, 20);
            this.gunsA.TabIndex = 95;
            this.gunsA.Text = "0";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(30, 245);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(32, 13);
            this.label30.TabIndex = 94;
            this.label30.Text = "Guns";
            // 
            // menPerGunB
            // 
            this.menPerGunB.Location = new System.Drawing.Point(390, 270);
            this.menPerGunB.Name = "menPerGunB";
            this.menPerGunB.Size = new System.Drawing.Size(64, 20);
            this.menPerGunB.TabIndex = 99;
            this.menPerGunB.Text = "0";
            // 
            // menPerGunA
            // 
            this.menPerGunA.Location = new System.Drawing.Point(174, 270);
            this.menPerGunA.Name = "menPerGunA";
            this.menPerGunA.Size = new System.Drawing.Size(64, 20);
            this.menPerGunA.TabIndex = 98;
            this.menPerGunA.Text = "0";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(30, 273);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(70, 13);
            this.label31.TabIndex = 97;
            this.label31.Text = "Men Per Gun";
            // 
            // chargeDefenseB
            // 
            this.chargeDefenseB.Location = new System.Drawing.Point(391, 713);
            this.chargeDefenseB.Name = "chargeDefenseB";
            this.chargeDefenseB.Size = new System.Drawing.Size(64, 20);
            this.chargeDefenseB.TabIndex = 102;
            this.chargeDefenseB.Text = "0.2";
            // 
            // chargeDefenseA
            // 
            this.chargeDefenseA.Location = new System.Drawing.Point(175, 713);
            this.chargeDefenseA.Name = "chargeDefenseA";
            this.chargeDefenseA.Size = new System.Drawing.Size(64, 20);
            this.chargeDefenseA.TabIndex = 101;
            this.chargeDefenseA.Text = "0.2";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(31, 716);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(84, 13);
            this.label32.TabIndex = 100;
            this.label32.Text = "Charge Defense";
            // 
            // tiles
            // 
            this.tiles.Location = new System.Drawing.Point(111, 107);
            this.tiles.Name = "tiles";
            this.tiles.Size = new System.Drawing.Size(64, 20);
            this.tiles.TabIndex = 104;
            this.tiles.Text = "0";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(28, 108);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(74, 13);
            this.label33.TabIndex = 103;
            this.label33.Text = "Tiles Between";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // usableB
            // 
            this.usableB.Location = new System.Drawing.Point(390, 213);
            this.usableB.Name = "usableB";
            this.usableB.Size = new System.Drawing.Size(64, 20);
            this.usableB.TabIndex = 107;
            this.usableB.Text = "100";
            // 
            // usableA
            // 
            this.usableA.Location = new System.Drawing.Point(174, 213);
            this.usableA.Name = "usableA";
            this.usableA.Size = new System.Drawing.Size(64, 20);
            this.usableA.TabIndex = 106;
            this.usableA.Text = "100";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(30, 216);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(40, 13);
            this.label34.TabIndex = 105;
            this.label34.Text = "Usable";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(290, 884);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(42, 38);
            this.button1.TabIndex = 108;
            this.button1.Text = "x10";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(338, 884);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(42, 38);
            this.button2.TabIndex = 109;
            this.button2.Text = "x20";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1004, 956);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.usableB);
            this.Controls.Add(this.usableA);
            this.Controls.Add(this.label34);
            this.Controls.Add(this.tiles);
            this.Controls.Add(this.label33);
            this.Controls.Add(this.chargeDefenseB);
            this.Controls.Add(this.chargeDefenseA);
            this.Controls.Add(this.label32);
            this.Controls.Add(this.menPerGunB);
            this.Controls.Add(this.menPerGunA);
            this.Controls.Add(this.label31);
            this.Controls.Add(this.gunsB);
            this.Controls.Add(this.gunsA);
            this.Controls.Add(this.label30);
            this.Controls.Add(this.chargeMoraleImpactB);
            this.Controls.Add(this.chargeMoraleImpactA);
            this.Controls.Add(this.label29);
            this.Controls.Add(this.heightB);
            this.Controls.Add(this.label28);
            this.Controls.Add(this.heightA);
            this.Controls.Add(this.label27);
            this.Controls.Add(this.river);
            this.Controls.Add(this.label26);
            this.Controls.Add(this.movementB);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.movementA);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.attackB);
            this.Controls.Add(this.defenseB);
            this.Controls.Add(this.attackA);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.defenseA);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.distance);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.fatigueModB);
            this.Controls.Add(this.speedB);
            this.Controls.Add(this.fatigueModA);
            this.Controls.Add(this.speedA);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.StartMeleeB);
            this.Controls.Add(this.StartRangedB);
            this.Controls.Add(this.StartMeleeA);
            this.Controls.Add(this.StartRangedA);
            this.Controls.Add(this.Time);
            this.Controls.Add(this.Swap);
            this.Controls.Add(this.chargeAttackB);
            this.Controls.Add(this.chargeB);
            this.Controls.Add(this.chargeAttackA);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.chargeA);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.Fast);
            this.Controls.Add(this.Pause);
            this.Controls.Add(this.Resume);
            this.Controls.Add(this.meleeB);
            this.Controls.Add(this.rangedB);
            this.Controls.Add(this.meleeDefenseB);
            this.Controls.Add(this.rangedDefenseB);
            this.Controls.Add(this.meleeTimeB);
            this.Controls.Add(this.meleeAttackB);
            this.Controls.Add(this.reloadB);
            this.Controls.Add(this.rangedTargetsB);
            this.Controls.Add(this.rangedAttackB);
            this.Controls.Add(this.rangedAccuracyB);
            this.Controls.Add(this.recoverB);
            this.Controls.Add(this.minimumMoraleB);
            this.Controls.Add(this.menB);
            this.Controls.Add(this.meleeA);
            this.Controls.Add(this.rangedA);
            this.Controls.Add(this.meleeDefenseA);
            this.Controls.Add(this.rangedDefenseA);
            this.Controls.Add(this.meleeTimeA);
            this.Controls.Add(this.meleeAttackA);
            this.Controls.Add(this.reloadA);
            this.Controls.Add(this.rangedTargetsA);
            this.Controls.Add(this.rangedAttackA);
            this.Controls.Add(this.rangedAccuracyA);
            this.Controls.Add(this.recoverA);
            this.Controls.Add(this.minimumMoraleA);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.menA);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.B);
            this.Controls.Add(this.A);
            this.Controls.Add(this.CombatLog);
            this.Controls.Add(this.Begin);
            this.Name = "Form1";
            this.Text = "Combat Calculator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button Begin;
        private System.Windows.Forms.TextBox CombatLog;
        private System.Windows.Forms.Label A;
        private System.Windows.Forms.Label B;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox menA;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox minimumMoraleA;
        private System.Windows.Forms.TextBox recoverA;
        private System.Windows.Forms.TextBox rangedAccuracyA;
        private System.Windows.Forms.TextBox rangedAttackA;
        private System.Windows.Forms.TextBox rangedTargetsA;
        private System.Windows.Forms.TextBox reloadA;
        private System.Windows.Forms.TextBox meleeAttackA;
        private System.Windows.Forms.TextBox meleeTimeA;
        private System.Windows.Forms.TextBox rangedDefenseA;
        private System.Windows.Forms.TextBox meleeDefenseA;
        private System.Windows.Forms.CheckBox rangedA;
        private System.Windows.Forms.CheckBox meleeA;
        private System.Windows.Forms.CheckBox meleeB;
        private System.Windows.Forms.CheckBox rangedB;
        private System.Windows.Forms.TextBox meleeDefenseB;
        private System.Windows.Forms.TextBox rangedDefenseB;
        private System.Windows.Forms.TextBox meleeTimeB;
        private System.Windows.Forms.TextBox meleeAttackB;
        private System.Windows.Forms.TextBox reloadB;
        private System.Windows.Forms.TextBox rangedTargetsB;
        private System.Windows.Forms.TextBox rangedAttackB;
        private System.Windows.Forms.TextBox rangedAccuracyB;
        private System.Windows.Forms.TextBox recoverB;
        private System.Windows.Forms.TextBox minimumMoraleB;
        private System.Windows.Forms.TextBox menB;
        private System.Windows.Forms.Button Resume;
        private System.Windows.Forms.Button Pause;
        private System.Windows.Forms.Button Fast;
        private System.Windows.Forms.CheckBox chargeA;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox chargeAttackA;
        private System.Windows.Forms.TextBox chargeAttackB;
        private System.Windows.Forms.CheckBox chargeB;
        private System.Windows.Forms.Button Swap;
        private System.Windows.Forms.TextBox Time;
        private System.Windows.Forms.Button StartRangedA;
        private System.Windows.Forms.Button StartMeleeA;
        private System.Windows.Forms.Button StartMeleeB;
        private System.Windows.Forms.Button StartRangedB;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox speedA;
        private System.Windows.Forms.TextBox fatigueModA;
        private System.Windows.Forms.TextBox fatigueModB;
        private System.Windows.Forms.TextBox speedB;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox distance;
        private System.Windows.Forms.TextBox defenseA;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox attackA;
        private System.Windows.Forms.TextBox attackB;
        private System.Windows.Forms.TextBox defenseB;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox movementA;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox movementB;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TextBox river;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox heightA;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.TextBox heightB;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.TextBox chargeMoraleImpactB;
        private System.Windows.Forms.TextBox chargeMoraleImpactA;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox gunsB;
        private System.Windows.Forms.TextBox gunsA;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.TextBox menPerGunB;
        private System.Windows.Forms.TextBox menPerGunA;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.TextBox chargeDefenseB;
        private System.Windows.Forms.TextBox chargeDefenseA;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.TextBox tiles;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox usableB;
        private System.Windows.Forms.TextBox usableA;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

