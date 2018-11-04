using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TagsCloudVisualization
{
	public class CloudWordsForm : Form
	{
		[STAThread]
		static void Main(string[] args)
		{
			Application.Run(new CloudWordsForm());
		}

		private Container components;
		public CloudWordsForm()
		{
			InitializeComponent();
			CenterToScreen();
			SetStyle(ControlStyles.ResizeRedraw, true);
		}
		private void InitializeComponent()
		{
			ClientSize = new Size(1000, 800);
			Resize += Form1_Resize;
			Paint += MainForm_Paint;
		}

		private void MainForm_Paint(object sender, PaintEventArgs e)
		{
			var graphics = CreateGraphics();
			var pen = new Pen(Color.Red, 1);
			var cloud = new CircularCloudLayouter(new Point(500, 400));
			var data = GetData(cloud, 150);

			graphics.DrawRectangles(pen, data.ToArray());
		}

		public static IEnumerable<Rectangle> GetData(CircularCloudLayouter layouter, int number)
		{
			var rnd = new Random();
			for (var i = 0; i < number; i++)
				layouter.PutNextRectangle(new Size(rnd.Next(10, 30), rnd.Next(4, 12)));

			return layouter.GetExistRectangles();
		}

		private void Form1_Resize(object sender, EventArgs e)
		{
		}

	}
}
