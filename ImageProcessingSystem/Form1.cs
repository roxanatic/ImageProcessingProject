using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Emgu;
using Emgu.CV;
using Emgu.CV.Structure;


namespace ImageProcessingSystem
{
    public partial class Form1 : Form
    {
        int maskSize = 3;
        Image<Gray, Byte> imgGray;

        public Form1()
        {
            InitializeComponent();
            clearForm();
        }

        private void clearForm()
        {
            label1.Visible = false; label2.Visible = false; label3.Visible = false; label4.Visible = false; label5.Visible = false; label6.Visible = false;
            textBox1.Visible = false; textBox2.Visible = false; textBox3.Visible = false;
            button1.Visible = false; button2.Visible = false;
            RadioButton1.Visible = false; RadioButton2.Visible = false; RadioButton3.Visible = false;
            pictureBox2.Visible = false;
          
        }

        private void openImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // deschidere imagine 
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog1.FileName.ToString();

                Image<Bgr, Byte> imgRGB = new Image<Bgr, Byte>(fileName);
                imgGray = imgRGB.Convert<Gray, Byte>();

                pictureBox1.Size = imgGray.Size;
                pictureBox1.Image = imgGray.ToBitmap();

                CvInvoke.Imshow("Image", imgRGB);
                // CvInvoke.WaitKey(0);
            }

        }

        private void invertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Matrix<float> lutInvert = new Matrix<float>(256,1);

            for (int i = 0; i < lutInvert.Height; i++)
                lutInvert.Data[i,0] = 255 - i;

            Image<Gray, Byte> imgGrayInvert = new Image<Gray, Byte>(imgGray.Size);

            for (int i = 0; i < imgGray.Height; i++)
                for (int j = 0; j < imgGray.Width; j++)
                    imgGrayInvert.Data[i, j, 0] = (Byte)(lutInvert.Data[imgGray.Data[i,j,0], 0]);

            pictureBox2.Left = pictureBox1.Left + pictureBox1.Width + 10;
            pictureBox2.Size = imgGrayInvert.Size;
            pictureBox2.Image = imgGrayInvert.ToBitmap();
            pictureBox2.Visible = true;

        }

        private void binarizationthresholdingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label1.Visible = true; 
            textBox1.Visible = true;
            button1.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // binarizare
            int pragBinTextBox = Convert.ToByte(textBox1.Text);
            pictureBox2.Left = pictureBox1.Left + pictureBox1.Width + 10;
            pictureBox2.Size = imgGray.Size;
            Image<Gray, Byte> imgBinPrag = BinarizareImagine(imgGray, pragBinTextBox);
            pictureBox2.Image = imgBinPrag.ToBitmap();
            pictureBox2.Visible = true;
        }

        private Image<Gray, Byte> BinarizareImagine(Image<Gray, Byte> ImgOrig, int pragBin)
        {
            Matrix<float> lutInvert = new Matrix<float>(256, 1);

            for (int i = 0; i < lutInvert.Height; i++)
                if (i< pragBin)
                    lutInvert.Data[i, 0] = 0;
                else
                    lutInvert.Data[i, 0] = 255;

            Image<Gray, Byte> imgBinFct = new Image<Gray, Byte>(ImgOrig.Size);

            for (int i = 0; i < ImgOrig.Height; i++)
                for (int j = 0; j < ImgOrig.Width; j++)
                    imgBinFct.Data[i, j, 0] = (Byte)(lutInvert.Data[ImgOrig.Data[i, j, 0], 0]);

            return imgBinFct;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            int x = e.X;
            int y = e.Y;

            label2.Text = Convert.ToString(x);
            label3.Text = Convert.ToString(y);
            label2.Visible = true; label3.Visible = true;
        }

        private void grayImageProcessingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clearForm();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //mask 3x3
        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            maskSize = 3;
        }
        //mask 5x5
        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            maskSize = 5;
        }
        //mask 7x7
        private void RadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            maskSize = 7;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            double sigma = Convert.ToDouble(textBox2.Text);
            int scaleFactor = Convert.ToInt32(textBox3.Text);
            pictureBox2.Size = imgGray.Size;
            pictureBox2.Visible = true;

            double[,] GX= new double[0,0];

            if (maskSize == 3)
            {

                GX = new double[3,3]
                {
                   { 0, -1, 0},
                   { -1, 4, -1 },
                   { 0, -1, 0 }
                    
                };


            }
            else if (maskSize == 5)
            {
                GX = new double[5,5]
                {
                    { 0, 0, 1, 0, 0 },
                    { 0, 1, 2, 1, 0 },
                    { 1, 2, -16, 2, 1},
                    { 0, 1, 2, 1, 0 },
                    { 0, 0, 1, 0, 0 }
                };

            }
            else if (maskSize == 7)
            {

                GX = new double[7,7]
                {
                    { 0, 0, 1, 1, 1, 0, 0 },
                    { 0, 1, 3, 3, 3, 1, 0 },
                    { 1, 3, 0, -7, 0, 3, 1 },
                    { 1, 3, -7, -24, -7, 3, 1},
                    { 1, 3, 0, -7, 0, 3, 1 },
                    { 0, 1, 3, 3, 3, 1, 0 },
                    { 0, 0, 1, 1, 1, 0, 0 }
                };

            }
            // LoG
            Matrix<double> log = new Matrix<double>(maskSize, maskSize);
            int vOS = (maskSize - 1) / 2;
            for (int i = -vOS; i < vOS; i++)
                for (int j = -vOS; j < vOS; j++)
                    log.Data[i + vOS, j + vOS] = scaleFactor*-(1 / (3.14 * Math.Pow(sigma, 4)) * (1 - ((i * i + j * j) / 2 * sigma * sigma)) * Math.Exp(-((i * i + j * j) / (2 * sigma * sigma))));
            //(double)scaleFactor * (1/(2*3.14*sigma*sigma))*Math.Exp(-(i*i+j*j)/(2*sigma*sigma)); 

            Image<Gray, double> imgL = new Image<Gray, double>(imgGray.Size);


            // convolutia
            for (int i = vOS; i < imgGray.Height - vOS; i++)
                for (int j = vOS; j < imgGray.Width - vOS; j++)
                {
                    imgL.Data[i, j,0] = 0;

                    for (int ii = -vOS; ii <= vOS; ii++)
                        for (int jj = -vOS; jj <= vOS; jj++)
                        {
                            imgL.Data[i, j, 0] += GX[ii + vOS, jj + vOS] * imgGray.Data[i + ii, j + jj, 0];
                        }

                }
            pictureBox2.Image = imgL.ToBitmap();
            pictureBox2.Left = pictureBox1.Left + pictureBox1.Width + 10;



        }

        private void laplaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button2.Visible = true;
            RadioButton1.Visible = true;
            RadioButton2.Visible = true;
            RadioButton3.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
            label6.Visible = true;
            textBox2.Visible = true;
            textBox3.Visible = true;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }


    
}
