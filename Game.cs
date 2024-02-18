using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tryMaze3
{
    public partial class Game : Form
    {
        private PLAY playForm;
        private Form1 form1;

        public Game()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            playForm.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            form1.Show ();
            this.Hide();
        }
        public void SetGameReference(PLAY playForm,Form1 form1)
        {
            this.playForm = playForm;
            this.form1 = form1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            playForm.Close();
            form1.Close();
            this.Close();
            Application.Exit();
        }

        private void Game_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
