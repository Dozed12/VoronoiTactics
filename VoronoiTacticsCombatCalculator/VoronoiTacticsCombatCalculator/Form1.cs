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
            //TODO add other stats

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

            //Create combat
            Combat combat = new Combat(A, B, a, b, c);
        }
    }
}
