using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using TagsCloudVisualization;
using System.IO;
using System.Drawing.Imaging;

namespace TagsCloudForm
{
    class Program
    {

        public static void Main()
        {
            var form = new CloudForm(10, 10, 30)
            {
                Size = new Size(600, 600)
            };

            Application.Run(form);
        }
    }
}
