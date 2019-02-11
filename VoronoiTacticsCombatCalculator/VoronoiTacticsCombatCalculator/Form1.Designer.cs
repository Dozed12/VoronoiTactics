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
            this.Start = new System.Windows.Forms.Button();
            this.CombatLog = new System.Windows.Forms.TextBox();
            this.A = new System.Windows.Forms.Label();
            this.B = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numberA = new System.Windows.Forms.TextBox();
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
            this.moraleA = new System.Windows.Forms.TextBox();
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
            this.moraleB = new System.Windows.Forms.TextBox();
            this.numberB = new System.Windows.Forms.TextBox();
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
            this.currentMoraleA = new System.Windows.Forms.TextBox();
            this.currentNumberA = new System.Windows.Forms.TextBox();
            this.currentMoraleB = new System.Windows.Forms.TextBox();
            this.currentNumberB = new System.Windows.Forms.TextBox();
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
            this.Distance = new System.Windows.Forms.TextBox();
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
            this.SuspendLayout();
            // 
            // Start
            // 
            this.Start.Location = new System.Drawing.Point(103, 735);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(103, 38);
            this.Start.TabIndex = 1;
            this.Start.Text = "Begin";
            this.Start.UseVisualStyleBackColor = true;
            // 
            // CombatLog
            // 
            this.CombatLog.Location = new System.Drawing.Point(572, 12);
            this.CombatLog.Multiline = true;
            this.CombatLog.Name = "CombatLog";
            this.CombatLog.ReadOnly = true;
            this.CombatLog.Size = new System.Drawing.Size(420, 762);
            this.CombatLog.TabIndex = 2;
            // 
            // A
            // 
            this.A.AutoSize = true;
            this.A.Font = new System.Drawing.Font("Arial", 15F);
            this.A.Location = new System.Drawing.Point(235, 147);
            this.A.Name = "A";
            this.A.Size = new System.Drawing.Size(23, 23);
            this.A.TabIndex = 3;
            this.A.Text = "A";
            // 
            // B
            // 
            this.B.AutoSize = true;
            this.B.Font = new System.Drawing.Font("Arial", 15F);
            this.B.Location = new System.Drawing.Point(453, 147);
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
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Number";
            // 
            // numberA
            // 
            this.numberA.Location = new System.Drawing.Point(174, 184);
            this.numberA.Name = "numberA";
            this.numberA.Size = new System.Drawing.Size(64, 20);
            this.numberA.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 219);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Morale";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 249);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Morale Recover";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 336);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Ranged";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 365);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Ranged Accuracy";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(30, 396);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Ranged Attack";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(30, 427);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(84, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Ranged Targets";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(30, 460);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "Reload";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(30, 492);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(36, 13);
            this.label9.TabIndex = 14;
            this.label9.Text = "Melee";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(30, 519);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(70, 13);
            this.label10.TabIndex = 15;
            this.label10.Text = "Melee Attack";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(30, 546);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(62, 13);
            this.label11.TabIndex = 16;
            this.label11.Text = "Melee Time";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(30, 574);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(88, 13);
            this.label12.TabIndex = 17;
            this.label12.Text = "Ranged Defense";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(30, 604);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(79, 13);
            this.label13.TabIndex = 18;
            this.label13.Text = "Melee Defense";
            // 
            // moraleA
            // 
            this.moraleA.Location = new System.Drawing.Point(174, 216);
            this.moraleA.Name = "moraleA";
            this.moraleA.Size = new System.Drawing.Size(64, 20);
            this.moraleA.TabIndex = 19;
            // 
            // recoverA
            // 
            this.recoverA.Location = new System.Drawing.Point(174, 246);
            this.recoverA.Name = "recoverA";
            this.recoverA.Size = new System.Drawing.Size(64, 20);
            this.recoverA.TabIndex = 20;
            // 
            // rangedAccuracyA
            // 
            this.rangedAccuracyA.Location = new System.Drawing.Point(174, 362);
            this.rangedAccuracyA.Name = "rangedAccuracyA";
            this.rangedAccuracyA.Size = new System.Drawing.Size(64, 20);
            this.rangedAccuracyA.TabIndex = 21;
            // 
            // rangedAttackA
            // 
            this.rangedAttackA.Location = new System.Drawing.Point(174, 393);
            this.rangedAttackA.Name = "rangedAttackA";
            this.rangedAttackA.Size = new System.Drawing.Size(64, 20);
            this.rangedAttackA.TabIndex = 22;
            // 
            // rangedTargetsA
            // 
            this.rangedTargetsA.Location = new System.Drawing.Point(174, 424);
            this.rangedTargetsA.Name = "rangedTargetsA";
            this.rangedTargetsA.Size = new System.Drawing.Size(64, 20);
            this.rangedTargetsA.TabIndex = 23;
            // 
            // reloadA
            // 
            this.reloadA.Location = new System.Drawing.Point(174, 457);
            this.reloadA.Name = "reloadA";
            this.reloadA.Size = new System.Drawing.Size(64, 20);
            this.reloadA.TabIndex = 24;
            // 
            // meleeAttackA
            // 
            this.meleeAttackA.Location = new System.Drawing.Point(174, 516);
            this.meleeAttackA.Name = "meleeAttackA";
            this.meleeAttackA.Size = new System.Drawing.Size(64, 20);
            this.meleeAttackA.TabIndex = 25;
            // 
            // meleeTimeA
            // 
            this.meleeTimeA.Location = new System.Drawing.Point(174, 543);
            this.meleeTimeA.Name = "meleeTimeA";
            this.meleeTimeA.Size = new System.Drawing.Size(64, 20);
            this.meleeTimeA.TabIndex = 26;
            // 
            // rangedDefenseA
            // 
            this.rangedDefenseA.Location = new System.Drawing.Point(174, 571);
            this.rangedDefenseA.Name = "rangedDefenseA";
            this.rangedDefenseA.Size = new System.Drawing.Size(64, 20);
            this.rangedDefenseA.TabIndex = 27;
            // 
            // meleeDefenseA
            // 
            this.meleeDefenseA.Location = new System.Drawing.Point(174, 601);
            this.meleeDefenseA.Name = "meleeDefenseA";
            this.meleeDefenseA.Size = new System.Drawing.Size(64, 20);
            this.meleeDefenseA.TabIndex = 28;
            // 
            // rangedA
            // 
            this.rangedA.AutoSize = true;
            this.rangedA.Location = new System.Drawing.Point(223, 336);
            this.rangedA.Name = "rangedA";
            this.rangedA.Size = new System.Drawing.Size(15, 14);
            this.rangedA.TabIndex = 29;
            this.rangedA.UseVisualStyleBackColor = true;
            // 
            // meleeA
            // 
            this.meleeA.AutoSize = true;
            this.meleeA.Location = new System.Drawing.Point(223, 491);
            this.meleeA.Name = "meleeA";
            this.meleeA.Size = new System.Drawing.Size(15, 14);
            this.meleeA.TabIndex = 30;
            this.meleeA.UseVisualStyleBackColor = true;
            // 
            // meleeB
            // 
            this.meleeB.AutoSize = true;
            this.meleeB.Location = new System.Drawing.Point(439, 491);
            this.meleeB.Name = "meleeB";
            this.meleeB.Size = new System.Drawing.Size(15, 14);
            this.meleeB.TabIndex = 43;
            this.meleeB.UseVisualStyleBackColor = true;
            // 
            // rangedB
            // 
            this.rangedB.AutoSize = true;
            this.rangedB.Location = new System.Drawing.Point(439, 336);
            this.rangedB.Name = "rangedB";
            this.rangedB.Size = new System.Drawing.Size(15, 14);
            this.rangedB.TabIndex = 42;
            this.rangedB.UseVisualStyleBackColor = true;
            // 
            // meleeDefenseB
            // 
            this.meleeDefenseB.Location = new System.Drawing.Point(390, 601);
            this.meleeDefenseB.Name = "meleeDefenseB";
            this.meleeDefenseB.Size = new System.Drawing.Size(64, 20);
            this.meleeDefenseB.TabIndex = 41;
            // 
            // rangedDefenseB
            // 
            this.rangedDefenseB.Location = new System.Drawing.Point(390, 571);
            this.rangedDefenseB.Name = "rangedDefenseB";
            this.rangedDefenseB.Size = new System.Drawing.Size(64, 20);
            this.rangedDefenseB.TabIndex = 40;
            // 
            // meleeTimeB
            // 
            this.meleeTimeB.Location = new System.Drawing.Point(390, 543);
            this.meleeTimeB.Name = "meleeTimeB";
            this.meleeTimeB.Size = new System.Drawing.Size(64, 20);
            this.meleeTimeB.TabIndex = 39;
            // 
            // meleeAttackB
            // 
            this.meleeAttackB.Location = new System.Drawing.Point(390, 516);
            this.meleeAttackB.Name = "meleeAttackB";
            this.meleeAttackB.Size = new System.Drawing.Size(64, 20);
            this.meleeAttackB.TabIndex = 38;
            // 
            // reloadB
            // 
            this.reloadB.Location = new System.Drawing.Point(390, 457);
            this.reloadB.Name = "reloadB";
            this.reloadB.Size = new System.Drawing.Size(64, 20);
            this.reloadB.TabIndex = 37;
            // 
            // rangedTargetsB
            // 
            this.rangedTargetsB.Location = new System.Drawing.Point(390, 424);
            this.rangedTargetsB.Name = "rangedTargetsB";
            this.rangedTargetsB.Size = new System.Drawing.Size(64, 20);
            this.rangedTargetsB.TabIndex = 36;
            // 
            // rangedAttackB
            // 
            this.rangedAttackB.Location = new System.Drawing.Point(390, 393);
            this.rangedAttackB.Name = "rangedAttackB";
            this.rangedAttackB.Size = new System.Drawing.Size(64, 20);
            this.rangedAttackB.TabIndex = 35;
            // 
            // rangedAccuracyB
            // 
            this.rangedAccuracyB.Location = new System.Drawing.Point(390, 362);
            this.rangedAccuracyB.Name = "rangedAccuracyB";
            this.rangedAccuracyB.Size = new System.Drawing.Size(64, 20);
            this.rangedAccuracyB.TabIndex = 34;
            // 
            // recoverB
            // 
            this.recoverB.Location = new System.Drawing.Point(390, 246);
            this.recoverB.Name = "recoverB";
            this.recoverB.Size = new System.Drawing.Size(64, 20);
            this.recoverB.TabIndex = 33;
            // 
            // moraleB
            // 
            this.moraleB.Location = new System.Drawing.Point(390, 216);
            this.moraleB.Name = "moraleB";
            this.moraleB.Size = new System.Drawing.Size(64, 20);
            this.moraleB.TabIndex = 32;
            // 
            // numberB
            // 
            this.numberB.Location = new System.Drawing.Point(390, 184);
            this.numberB.Name = "numberB";
            this.numberB.Size = new System.Drawing.Size(64, 20);
            this.numberB.TabIndex = 31;
            // 
            // Resume
            // 
            this.Resume.Location = new System.Drawing.Point(306, 735);
            this.Resume.Name = "Resume";
            this.Resume.Size = new System.Drawing.Size(30, 38);
            this.Resume.TabIndex = 44;
            this.Resume.Text = "x1";
            this.Resume.UseVisualStyleBackColor = true;
            // 
            // Pause
            // 
            this.Pause.Location = new System.Drawing.Point(270, 735);
            this.Pause.Name = "Pause";
            this.Pause.Size = new System.Drawing.Size(30, 38);
            this.Pause.TabIndex = 45;
            this.Pause.Text = "| |";
            this.Pause.UseVisualStyleBackColor = true;
            // 
            // Fast
            // 
            this.Fast.Location = new System.Drawing.Point(342, 735);
            this.Fast.Name = "Fast";
            this.Fast.Size = new System.Drawing.Size(30, 38);
            this.Fast.TabIndex = 46;
            this.Fast.Text = "x4";
            this.Fast.UseVisualStyleBackColor = true;
            // 
            // chargeA
            // 
            this.chargeA.AutoSize = true;
            this.chargeA.Location = new System.Drawing.Point(223, 633);
            this.chargeA.Name = "chargeA";
            this.chargeA.Size = new System.Drawing.Size(15, 14);
            this.chargeA.TabIndex = 48;
            this.chargeA.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(30, 634);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(41, 13);
            this.label14.TabIndex = 47;
            this.label14.Text = "Charge";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(30, 661);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(75, 13);
            this.label15.TabIndex = 49;
            this.label15.Text = "Charge Attack";
            // 
            // chargeAttackA
            // 
            this.chargeAttackA.Location = new System.Drawing.Point(174, 658);
            this.chargeAttackA.Name = "chargeAttackA";
            this.chargeAttackA.Size = new System.Drawing.Size(64, 20);
            this.chargeAttackA.TabIndex = 50;
            // 
            // chargeAttackB
            // 
            this.chargeAttackB.Location = new System.Drawing.Point(390, 658);
            this.chargeAttackB.Name = "chargeAttackB";
            this.chargeAttackB.Size = new System.Drawing.Size(64, 20);
            this.chargeAttackB.TabIndex = 52;
            // 
            // chargeB
            // 
            this.chargeB.AutoSize = true;
            this.chargeB.Location = new System.Drawing.Point(439, 633);
            this.chargeB.Name = "chargeB";
            this.chargeB.Size = new System.Drawing.Size(15, 14);
            this.chargeB.TabIndex = 51;
            this.chargeB.UseVisualStyleBackColor = true;
            // 
            // Swap
            // 
            this.Swap.Location = new System.Drawing.Point(339, 142);
            this.Swap.Name = "Swap";
            this.Swap.Size = new System.Drawing.Size(40, 38);
            this.Swap.TabIndex = 53;
            this.Swap.Text = "<=>";
            this.Swap.UseVisualStyleBackColor = true;
            // 
            // currentMoraleA
            // 
            this.currentMoraleA.Location = new System.Drawing.Point(254, 216);
            this.currentMoraleA.Name = "currentMoraleA";
            this.currentMoraleA.ReadOnly = true;
            this.currentMoraleA.Size = new System.Drawing.Size(64, 20);
            this.currentMoraleA.TabIndex = 55;
            // 
            // currentNumberA
            // 
            this.currentNumberA.Location = new System.Drawing.Point(254, 184);
            this.currentNumberA.Name = "currentNumberA";
            this.currentNumberA.ReadOnly = true;
            this.currentNumberA.Size = new System.Drawing.Size(64, 20);
            this.currentNumberA.TabIndex = 54;
            // 
            // currentMoraleB
            // 
            this.currentMoraleB.Location = new System.Drawing.Point(471, 216);
            this.currentMoraleB.Name = "currentMoraleB";
            this.currentMoraleB.ReadOnly = true;
            this.currentMoraleB.Size = new System.Drawing.Size(64, 20);
            this.currentMoraleB.TabIndex = 57;
            // 
            // currentNumberB
            // 
            this.currentNumberB.Location = new System.Drawing.Point(471, 184);
            this.currentNumberB.Name = "currentNumberB";
            this.currentNumberB.ReadOnly = true;
            this.currentNumberB.Size = new System.Drawing.Size(64, 20);
            this.currentNumberB.TabIndex = 56;
            // 
            // Time
            // 
            this.Time.Location = new System.Drawing.Point(395, 745);
            this.Time.Name = "Time";
            this.Time.Size = new System.Drawing.Size(112, 20);
            this.Time.TabIndex = 58;
            // 
            // StartRangedA
            // 
            this.StartRangedA.Location = new System.Drawing.Point(158, 689);
            this.StartRangedA.Name = "StartRangedA";
            this.StartRangedA.Size = new System.Drawing.Size(80, 31);
            this.StartRangedA.TabIndex = 60;
            this.StartRangedA.Text = "Start Ranged";
            this.StartRangedA.UseVisualStyleBackColor = true;
            // 
            // StartMeleeA
            // 
            this.StartMeleeA.Location = new System.Drawing.Point(257, 689);
            this.StartMeleeA.Name = "StartMeleeA";
            this.StartMeleeA.Size = new System.Drawing.Size(80, 31);
            this.StartMeleeA.TabIndex = 61;
            this.StartMeleeA.Text = "Start Melee";
            this.StartMeleeA.UseVisualStyleBackColor = true;
            // 
            // StartMeleeB
            // 
            this.StartMeleeB.Location = new System.Drawing.Point(474, 689);
            this.StartMeleeB.Name = "StartMeleeB";
            this.StartMeleeB.Size = new System.Drawing.Size(80, 31);
            this.StartMeleeB.TabIndex = 63;
            this.StartMeleeB.Text = "Start Melee";
            this.StartMeleeB.UseVisualStyleBackColor = true;
            // 
            // StartRangedB
            // 
            this.StartRangedB.Location = new System.Drawing.Point(375, 689);
            this.StartRangedB.Name = "StartRangedB";
            this.StartRangedB.Size = new System.Drawing.Size(80, 31);
            this.StartRangedB.TabIndex = 62;
            this.StartRangedB.Text = "Start Ranged";
            this.StartRangedB.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(31, 278);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(38, 13);
            this.label16.TabIndex = 64;
            this.label16.Text = "Speed";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(31, 306);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(82, 13);
            this.label17.TabIndex = 65;
            this.label17.Text = "Fatigue Modifier";
            // 
            // speedA
            // 
            this.speedA.Location = new System.Drawing.Point(174, 275);
            this.speedA.Name = "speedA";
            this.speedA.Size = new System.Drawing.Size(64, 20);
            this.speedA.TabIndex = 66;
            // 
            // fatigueModA
            // 
            this.fatigueModA.Location = new System.Drawing.Point(174, 303);
            this.fatigueModA.Name = "fatigueModA";
            this.fatigueModA.Size = new System.Drawing.Size(64, 20);
            this.fatigueModA.TabIndex = 67;
            // 
            // fatigueModB
            // 
            this.fatigueModB.Location = new System.Drawing.Point(390, 299);
            this.fatigueModB.Name = "fatigueModB";
            this.fatigueModB.Size = new System.Drawing.Size(64, 20);
            this.fatigueModB.TabIndex = 69;
            // 
            // speedB
            // 
            this.speedB.Location = new System.Drawing.Point(390, 271);
            this.speedB.Name = "speedB";
            this.speedB.Size = new System.Drawing.Size(64, 20);
            this.speedB.TabIndex = 68;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(320, 110);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(49, 13);
            this.label18.TabIndex = 70;
            this.label18.Text = "Distance";
            // 
            // Distance
            // 
            this.Distance.Location = new System.Drawing.Point(375, 107);
            this.Distance.Name = "Distance";
            this.Distance.Size = new System.Drawing.Size(64, 20);
            this.Distance.TabIndex = 71;
            // 
            // defenseA
            // 
            this.defenseA.Location = new System.Drawing.Point(262, 42);
            this.defenseA.Name = "defenseA";
            this.defenseA.Size = new System.Drawing.Size(64, 20);
            this.defenseA.TabIndex = 73;
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
            // 
            // defenseB
            // 
            this.defenseB.Location = new System.Drawing.Point(479, 42);
            this.defenseB.Name = "defenseB";
            this.defenseB.Size = new System.Drawing.Size(64, 20);
            this.defenseB.TabIndex = 76;
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
            this.label23.Location = new System.Drawing.Point(30, 42);
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1004, 786);
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
            this.Controls.Add(this.Distance);
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
            this.Controls.Add(this.currentMoraleB);
            this.Controls.Add(this.currentNumberB);
            this.Controls.Add(this.currentMoraleA);
            this.Controls.Add(this.currentNumberA);
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
            this.Controls.Add(this.moraleB);
            this.Controls.Add(this.numberB);
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
            this.Controls.Add(this.moraleA);
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
            this.Controls.Add(this.numberA);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.B);
            this.Controls.Add(this.A);
            this.Controls.Add(this.CombatLog);
            this.Controls.Add(this.Start);
            this.Name = "Form1";
            this.Text = "Combat Calculator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button Start;
        private System.Windows.Forms.TextBox CombatLog;
        private System.Windows.Forms.Label A;
        private System.Windows.Forms.Label B;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox numberA;
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
        private System.Windows.Forms.TextBox moraleA;
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
        private System.Windows.Forms.TextBox moraleB;
        private System.Windows.Forms.TextBox numberB;
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
        private System.Windows.Forms.TextBox currentMoraleA;
        private System.Windows.Forms.TextBox currentNumberA;
        private System.Windows.Forms.TextBox currentMoraleB;
        private System.Windows.Forms.TextBox currentNumberB;
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
        private System.Windows.Forms.TextBox Distance;
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
    }
}

