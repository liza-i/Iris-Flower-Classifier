﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageProcessing
{
    public partial class Form1 : Form
    {
        public List<Point> Coordinates { get; set; }
        public Tuple<double, double> ScaleHW { get; set; }
        public Dictionary<string, List<double>> FlowerDimensions;
        public Form1()
        {
            Coordinates = new List<Point>();
            FlowerDimensions = new Dictionary<string, List<double>>();
            FlowerDimensions.Add("sepal", new List<double>());
            FlowerDimensions.Add("petal", new List<double>());
            InitializeComponent();
            message(); 
        }

        private void message()
        {
           if(FlowerDimensions["sepal"].Count == 0)
            {
                MessageBox.Show("Preia lungimea unei sepale.");
                return;
            } 

           if(FlowerDimensions["sepal"].Count == 1)
            {
                MessageBox.Show("Preia latimea unei sepale.");
                return;
            }
           
           if(FlowerDimensions["petal"].Count == 0)
            {
                MessageBox.Show("Preia lungimea unei sepale.");
                return;
            } 

           if(FlowerDimensions["petal"].Count == 1)
            {
                MessageBox.Show("Preia latimea unei sepale.");
                return;
            } 
           

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string imageLocation = "";

            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "jpg files(*.jpg)|*.jpg| PNG files(*.png)|*.png| All files(*.*)|*.*";

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    imageLocation = openFileDialog.FileName;

                    PictureBox.Image = Image.FromFile(imageLocation);
                    int imgHeight = PictureBox.Image.Height;
                    int imgWidth = PictureBox.Image.Width;
                    double ratio = 0;
                    if (imgWidth > imgHeight)
                    {
                        ratio = imgWidth / PictureBox.Width;
                        imgWidth = 650;
                        imgHeight = (int)(imgHeight / ratio);
                    }
                    else
                    {
                        ratio = imgHeight / PictureBox.Height;
                        imgHeight = 650;
                        imgWidth = (int)(imgWidth / ratio);
                    }
                    PictureBox.Width = imgWidth;
                    PictureBox.Height = imgHeight;
                    message();
                }

            } catch (Exception)
            {
                MessageBox.Show("An Error has occured!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PictureBox.MouseClick += PictureBox_Click;
        }

        private void PictureBox_Click(object sender, MouseEventArgs e)
        {
            var point = new Point(e.X, e.Y);
            Coordinates.Add(point);
            MessageBox.Show(string.Format("You've selected a pixel with coordinates: {0}:{1}", point.X, point.Y));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text == "" || textBox2.Text == "")
                {
                    MessageBox.Show("No image Height or Width given", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (Coordinates.Count != 2)
                {
                    MessageBox.Show("Wrong number of Coordinates!\n" +
                        "Reselect 2 Coordinates and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Coordinates.Clear();
                }
                else
                {
                    double realImageHeight = Convert.ToDouble(textBox1.Text);
                    double realImageWidth = Convert.ToDouble(textBox2.Text);
                    ScaleHW = new Tuple<double, double>(realImageHeight / PictureBox.Height, realImageWidth / PictureBox.Width);
                    double mHeight = Math.Abs(Coordinates[0].Y - Coordinates[1].Y) * ScaleHW.Item1;
                    double mWidth = Math.Abs(Coordinates[0].X - Coordinates[1].X) * ScaleHW.Item2;
                    double distance = Math.Sqrt(mWidth * mWidth + mHeight * mHeight);
                    MessageBox.Show(string.Format("The measured distance is: {0} cm.", distance));

                    if(this.FlowerDimensions["sepal"].Count !=2)
                    {
                        this.FlowerDimensions["sepal"].Add(distance);
                    }
                    else this.FlowerDimensions["petal"].Add(distance);

                    Coordinates.Clear();
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Invalid format for image Height or Width", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
