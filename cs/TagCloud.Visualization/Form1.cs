namespace TagCloud.Visualization
{
    public partial class Form1 : Form
    {
        TagCloudForDebug layouter;
        Random rd=new Random();

        public Form1()
        {
            InitializeComponent();
            var center = new Point(pictureBox1.Width / 2, pictureBox1.Height / 2);
            layouter = new TagCloudForDebug(center);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //var inp = textBox1.Text.Split().Select(int.Parse).ToArray();
            //var size = new Size(inp[0], inp[1]);
            var size = new Size(rd.Next(20,40), rd.Next(20,40));
            foreach (var el in layouter.PutNextRectangle(size))
            {
                pictureBox1.Image = el;
                pictureBox1.Update();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}