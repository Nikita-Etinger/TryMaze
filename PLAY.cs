using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tryMaze3
{
    
    public partial class PLAY : Form
    {
       
        private Timer timer = new Timer();//таймер для события
        private PictureBox spherePictureBox;

        private Stopwatch stopwatch = new Stopwatch();
        const int sizeBlock = 30;
        private List<Block> blocks;
        private Game game;
        private BlockManager blockManager;
        public PLAY(Game gameForm)
        {
            InitializeComponent();
            this.game = gameForm;
            blocks = new List<Block>();
            blockManager = new BlockManager(sizeBlock, ref blocks);
            this.MouseDown += PLAY_MouseDown;
            this.MouseMove += PLAY_MouseMove;
            this.MouseUp += PLAY_MouseUp;

            timer.Interval = 100; //  интервал в миллисекундах
            timer.Tick += Timer_Tick; // обработчик события Tick
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            
            if (spherePictureBox != null && stopwatch.IsRunning)
            {
                label1.Text = stopwatch.Elapsed.ToString(@"s\.fff");
            }
        }
        private void PLAY_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (var block in blocks)
            {
                // Удаляем PictureBox из контейнера управления
                if (block.PictureBox != null && block.PictureBox.Parent != null)
                {
                    block.PictureBox.Parent.Controls.Remove(block.PictureBox);
                }
            }
            blocks.Clear();
            
            game.Show();
            button2.Show();
            this.Hide();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            blockManager.LoadBlocksFromJson("mapTest.json");
            foreach (var block in blocks)
            {
                Controls.Add(block.PictureBox);
            }
            button2.Hide();

        }

        private void PLAY_KeyDown(object sender, KeyEventArgs e)
        {

        }
        private void CreateSphere(Point location)
        {
            // была ли уже создана сфера
            if (spherePictureBox != null)
            {
                return; 
            }


            spherePictureBox = new PictureBox();
            spherePictureBox.BackColor = Color.Red; 
            spherePictureBox.Size = new Size(10, 10); 
            spherePictureBox.Location = location;
            spherePictureBox.Enabled = false;

            Controls.Add(spherePictureBox);
        }
        private void PLAY_MouseMove(object sender, MouseEventArgs e)
        {
            if (spherePictureBox != null && e.Button == MouseButtons.Left)
            {
                spherePictureBox.Location = new Point(e.X - spherePictureBox.Width / 2, e.Y - spherePictureBox.Height / 2);
                foreach (var block in blocks)
                {
                    // Проверяем, если координаты сферы находятся в пределах блока не равного 3
                    if (spherePictureBox.Location.X >= block.PictureBox.Location.X &&
                        spherePictureBox.Location.X <= block.PictureBox.Location.X + sizeBlock &&
                        spherePictureBox.Location.Y >= block.PictureBox.Location.Y &&
                        spherePictureBox.Location.Y <= block.PictureBox.Location.Y + sizeBlock &&
                        block.Type != "3")
                    {
                        if(spherePictureBox.Location.X >= block.PictureBox.Location.X &&
                        spherePictureBox.Location.X <= block.PictureBox.Location.X + sizeBlock &&
                        spherePictureBox.Location.Y >= block.PictureBox.Location.Y &&
                        spherePictureBox.Location.Y <= block.PictureBox.Location.Y + sizeBlock &&
                        block.Type == "4")
                        {
                            MessageBox.Show($"Вы прошли уровень за {label1.Text} секунд!");
                        }
                        // Удаляем сферу и выходим из метода
                        stopwatch.Reset();
                        Controls.Remove(spherePictureBox);
                        spherePictureBox.Dispose();
                        spherePictureBox = null;
                        return;
                    }
                }

                
            }
        }
        private void PLAY_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                foreach (var block in blocks)
                {
                    if (e.X >= block.PictureBox.Location.X && e.X <= block.PictureBox.Location.X + sizeBlock &&
                        e.Y >= block.PictureBox.Location.Y && e.Y <= block.PictureBox.Location.Y + sizeBlock &&
                        block.Type == "3")
                    {
                        CreateSphere(block.PictureBox.Location);
                        if (spherePictureBox != null)
                        {
                            stopwatch.Start();
                            timer.Start(); 
                        }
                        break;
                    }
                }
            }
        }
        private void PLAY_MouseUp(object sender, MouseEventArgs e)
        {
            if (spherePictureBox != null)
            {
                Controls.Remove(spherePictureBox);
                spherePictureBox.Dispose();
                spherePictureBox = null;
                stopwatch.Stop();
                timer.Stop();
                stopwatch.Reset(); 
                label1.Text = "0.000"; 
                return;
            }
        }
    }
}
