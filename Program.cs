using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tryMaze3
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetCompatibleTextRenderingDefault(false);

            // Инициализация переменных


            // Создание экземпляра Game с передачей playForm и mapEditor в конструктор
            Game gameForm = new Game();
            PLAY playForm = new PLAY(gameForm);

            Form1 mapEditor = new Form1(gameForm);
            // Создание экземпляров PLAY и Form1 с передачей gameForm в конструкторы

            gameForm.SetGameReference( playForm, mapEditor);

            
            // Запуск приложения с формой Game через метод Show()
            gameForm.Show();

            // Запуск обработки событий приложения
            Application.EnableVisualStyles();
            Application.Run();
        }
    }
}