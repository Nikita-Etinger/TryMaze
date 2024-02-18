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
    public class BlockManager
    {
        private List<Block> blocks;
        private int sizeBlock;
        private Dictionary<string, Image> textureDictionary;

        public BlockManager(int sizeBlock, ref List<Block> blocks)
        {
            this.blocks = blocks;
            this.sizeBlock = sizeBlock;
            this.textureDictionary = LoadTextures();
        }
        public void AddBlock(string type, Point location)
        {
            // Проверяем, есть ли текстура для указанного типа
            if (textureDictionary.ContainsKey(type))
            {
                // Создаем блок, передавая ему текстуру из словаря
                Block block = new Block(type, textureDictionary[type], location.X, location.Y, sizeBlock);
                block.PictureBox.Enabled = false;
                blocks.Add(block);
            }
            else
            {
                Console.WriteLine($"Ошибка: нет текстуры для типа {type}");
            }
        }
        private Dictionary<string, Image> LoadTextures()
        {
            Dictionary<string, Image> textures = new Dictionary<string, Image>();
            try
            {

                for (int i = 1; i <= 4; i++)
                {
                    string type = i.ToString();
                    string fileName = $"{type}.png";
                    textures.Add(type, Image.FromFile(fileName));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки текстур: {ex.Message}");
            }
            return textures;
        }

        public void AddBlock(string type, int x, int y)
        {
            // Проверяем, есть ли текстура для указанного типа
            if (textureDictionary.ContainsKey(type))
            {
                // Создаем блок, передавая ему текстуру из словаря
                Block block = new Block(type, textureDictionary[type], x, y, sizeBlock);
                block.PictureBox.Enabled = false;
                blocks.Add(block);
            }
            else
            {
                Console.WriteLine($"Ошибка: нет текстуры для типа {type}");
            }
        }

        public void SaveBlocksToJson(string filename)
        {
            List<BlockData> blockDataList = new List<BlockData>();
            foreach (var block in blocks)
            {
                blockDataList.Add(new BlockData(block.Type, block.PictureBox.Location.X, block.PictureBox.Location.Y));
            }
            string json = JsonConvert.SerializeObject(blockDataList);
            File.WriteAllText(filename, json);
        }

        public void LoadBlocksFromJson(string filename)
        {
            try
            {
                string json = File.ReadAllText(filename);
                List<BlockData> blockDataList = JsonConvert.DeserializeObject<List<BlockData>>(json);
                foreach (var blockData in blockDataList)
                {
                    AddBlock(blockData.Type, blockData.X, blockData.Y);
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Файл {filename} не найден: {ex.Message}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Ошибка при десериализации JSON: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        }

    }
    public class Block
    {
        public string Type { get; set; }
        public PictureBox PictureBox { get; set; }
        public Image Texture { get; set; }

        public Block(string type, Image texture, int x, int y, int size)
        {
            Type = type;
            Texture = texture;
            PictureBox = new PictureBox();
            PictureBox.Location = new Point(x, y);
            PictureBox.Size = new Size(size, size);
            PictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            PictureBox.BackgroundImageLayout = ImageLayout.None;
            SetTexture();
        }

        private void SetTexture()
        {
            PictureBox.BackColor = Color.Transparent;
            PictureBox.Image = Texture;
        }
    }
    public class BlockData
    {
        public string Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public BlockData(string type, int x, int y)
        {
            Type = type;
            X = x;
            Y = y;
        }
    }
}
