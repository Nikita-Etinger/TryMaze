using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json; // Для работы с JSON
using System.IO;
namespace tryMaze3
{


    public partial class Form1 : Form
    {
        const int sizeBlock = 30;
        private int menuChek = 0;
        private bool menuOpen = false;
        private List<Block> blocks;
        private PictureBox temporaryPictureBox;
        private BlockManager blockManager;
        private Game game;
        public Form1(Game game)
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            blocks = new List<Block>();
            blockManager = new BlockManager(sizeBlock, ref blocks);
            blockManager.LoadBlocksFromJson("mapTest.json");
            Console.WriteLine(blocks.Count().ToString());
            InitializeComponent();
            this.MouseClick += Form1_MouseClick;
            this.MouseMove += Form1_MouseMove;
            foreach (var block in blocks)
            {
                Controls.Add(block.PictureBox);
            }
            panel1.Visible = false;
            this.game = game;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            menuChek = 1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            menuChek = 2;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            menuChek = 3;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            menuChek = 4;
        }

        private void ButtonMenu_Click(object sender, EventArgs e)
        {
            if (!menuOpen)
            {
                panel1.Visible = true;
            }
            else
            {
                panel1.Visible = false;
            }
            menuOpen = !menuOpen;
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && menuChek!=0)
            {
                if (menuChek == 3)
                {
                    bool blockExists = CheckBlockExists(3);
                    if (blockExists)
                        return;
                }
                else if (menuChek == 4)
                {
                    bool blockExists = CheckBlockExists(4);
                    if (blockExists)
                        return;
                }


                // Создаем блок на месте временного блока
                blockManager.AddBlock(menuChek.ToString(), temporaryPictureBox.Location);
                foreach (var block in blocks)
                {
                    if (!Controls.Contains(block.PictureBox))
                    {
                        Controls.Add(block.PictureBox);
                    }
                }
                // Удаляем временный блок
                this.Controls.Remove(temporaryPictureBox);
                temporaryPictureBox.Dispose();
                temporaryPictureBox = null;
            }
            else if (e.Button == MouseButtons.Right)
            {
                Block blockToRemove = blocks.Find(b => b.PictureBox.Bounds.Contains(e.Location)); //блок по координате мыши
                if (blockToRemove != null)
                {
                    Controls.Remove(blockToRemove.PictureBox);
                    blocks.Remove(blockToRemove);
                }
            }

        }
        private bool CheckBlockExists(int type)
        {
            foreach (var block in blocks)
            {
                if (block.Type == type.ToString())
                    return true;
            }
            return false;
        }

        private void BlockPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                PictureBox pictureBox = (PictureBox)sender; // ПPictureBox на котором был щелчок
                Block blockToRemove = blocks.Find(b => b.PictureBox == pictureBox); // соответствующий блок
                if (blockToRemove != null)
                {
                    Controls.Remove(pictureBox);
                    blocks.Remove(blockToRemove);
                }
            }
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {

            if (temporaryPictureBox == null)
            {
                temporaryPictureBox = new PictureBox();
                temporaryPictureBox.Size = new Size(sizeBlock, sizeBlock); 
                temporaryPictureBox.BackColor = Color.FromArgb(100, Color.Gray); 
                this.Controls.Add(temporaryPictureBox);
                temporaryPictureBox.Enabled = false;
            }
            // обновляем позицию временного блока в соответствии с текущими координатами курсора мыши
            temporaryPictureBox.Location = new Point(e.X - temporaryPictureBox.Width / 2, e.Y - temporaryPictureBox.Height / 2);
            this.Cursor = Cursors.Default;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            blockManager.SaveBlocksToJson("mapTest.json");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Очистить?", "Подтверждение очистки", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

           
            if (result == DialogResult.Yes)
            {
                foreach (var block in blocks)
                {
                    // Удаляем PictureBox из контейнера управления
                    if (block.PictureBox != null && block.PictureBox.Parent != null)
                    {
                        block.PictureBox.Parent.Controls.Remove(block.PictureBox);
                    }
                }
                blocks.Clear(); // Очищаем список blocks
                blockManager.SaveBlocksToJson("mapTest.json");
            }
            else
            {
               
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            game.Show();
            this.Hide();
        }
    }

    

}
