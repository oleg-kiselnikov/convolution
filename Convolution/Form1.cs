using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Convolution
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var image = GetImage();

            if (image != null)
            {
                var furierFilter = new FurierFilter(3, 16);

                furierFilter.Factor *= 1;

                image = pictureBox1.Image == null
                    ? image.Apply(furierFilter)
                    : pictureBox1.Image.Combine(image.Apply(furierFilter));

                pictureBox1.Image = image;
            }
                
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var image = GetImage();

            if (image != null)
            {
                var furierFilter = new FurierFilter2(11, 24);
                
                //highPassFilter.Factor *= 0.5;
                
                //furierFilter.Factor *= -1;

                //furierFilter.Bias = 255;

                if (pictureBox1.Image == null)
                {
                    pictureBox1.Image = image.Apply(furierFilter);
                }

                image = pictureBox1.Image == null
                    ? image.Apply(furierFilter)
                    : pictureBox1.Image.Combine(image.Apply(furierFilter));

                pictureBox1.Image = image;
            }
        }

        private Image GetImage()
        {
            var openFileDialog = new OpenFileDialog()
            {
                Title = "Select an image file.",
                Filter = "Png Images(*.png)|*.png|Jpeg Images(*.jpg)|*.jpg|Bitmap Images(*.bmp)|*.bmp"
            };
            Image image = null;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                using (var streamReader = new StreamReader(openFileDialog.FileName))
                    image = Image.FromStream(streamReader.BaseStream);

            return image;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog()
            {
                Title = "Specify a file name and file path",
                Filter = "Png Images(*.png)|*.png|Jpeg Images(*.jpg)|*.jpg|Bitmap Images(*.bmp)|*.bmp"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                var fileExtension = Path.GetExtension(saveFileDialog.FileName) ?? "";

                ImageFormat imageFormat;

                switch (fileExtension.ToUpper())
                {
                    case "BMP":
                        imageFormat = ImageFormat.Bmp;
                        break;
                    case "JPG":
                        imageFormat = ImageFormat.Jpeg;
                        break;
                    default:
                        imageFormat = ImageFormat.Png;
                        break;
                }

                using (var streamWriter = new StreamWriter(saveFileDialog.FileName, false))
                {
                    pictureBox1.Image.Save(streamWriter.BaseStream, imageFormat);
                }

                pictureBox1.Image = null;
            }
        }
        
    }
}
