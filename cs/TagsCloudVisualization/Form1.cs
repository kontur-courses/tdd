using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TagsCloudVisualization
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Width = 800;
            Height = 500;
            var center = new Point(Width / 2, Height / 2);
            
            var rectangles = RandomRectanglesCreator.GetRectangles(60, center);

            var bitmap = TagCloudDrawer.Draw(rectangles, Width, Height, center,
                Color.White, Color.DarkOrange);
            
            AddPictureBox(bitmap);
            AddFileNameField();
            AddSaveButton();
        }

        private void AddFileNameField()
        {
            var field = new TextBox();
            field.Name = "fileName";
            field.Height = 10;
            field.Width = 200;
            field.Top = 10;
            field.Left = 10;
            field.Text = "tag cloud visualization.png";
            Controls.Add(field);
            field.BringToFront();
        }
        
        private void AddSaveButton()
        {
            var fileNameField = Controls.Find("fileName", false).First();
            var button = new Button();
            button.Text = "Сохранить";
            button.Name = "saveButton";
            button.Click += SaveImage;
            button.Left = fileNameField.Right + 10;
            button.Top = fileNameField.Top;
            Controls.Add(button);
            button.BringToFront();
        }

        private void SaveImage(object sender, EventArgs eventArgs)
        {
            var field = Controls.Find("fileName", false).First();
            var fileName = field.Text.EndsWith(".png") ? field.Text : field.Text + ".png";
            var pictureBox = Controls.Find("image", false).First();
            var path = AppContext.BaseDirectory;
            ((PictureBox)pictureBox).Image.Save(fileName);
            MessageBox.Show($"Изображение было сохранено в {path + fileName}");
        }

        private void AddPictureBox(Image image)
        {
            var pictureBox = new PictureBox();
            pictureBox.Image = image;
            pictureBox.Height = image.Height;
            pictureBox.Width = image.Width;
            pictureBox.Name = "image";
            Controls.Add(pictureBox);
            Width = image.Width;
            Height = image.Height;
        }
    }
}