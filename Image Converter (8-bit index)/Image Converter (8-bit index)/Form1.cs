//Upload image to debug folder and write the name in text box

//Program automatically resizes image to 106x49
//Program goes through each pixel, and stores it in pixelColor array
//All the unique colors are put into the colorIndex list
//Then each pixel is assigned a number corrosponding to the location of the
//color in the colorIndex list
//Both colorIndex list and pixelColorRef array are printed out

//Make sure to put image through an 8-bit color indexer to 
//ensure that there are no more than 256 unique colors in image
//and to resize to 106x49 first:
//https://online-converting.com/image/convert2png/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Image_Converter__8_bit_index_
{
    public partial class Form1 : Form
    {
        const int picXSize = 41;   //X-size of image
        const int picYSize = 23;   //Y-size of image (I have 46 LEDS)
        bool error = false;
        Bitmap pictureMap;
        Image pic;
        Bitmap zoomMap;
        string textBoxText = "";
        bool checkBoxState = false;

        //Output strings (used to speed up program)
        string indexOutput;     
        string arrayOutput;

        //Arrays and lists
        IList<Color> colorIndex = new List<Color>();            //Stores all unique colors
        Color[,] pixelColor = new Color[picXSize, picYSize];    //Stores all pixel colors
        int[,] pixelColorRef = new int[picXSize, picYSize];     //Stores all pixel colors by refrencing colors in colorIndex

        public Form1()
        {
            InitializeComponent();
        }

        //Calculate Button
        private void calculateButton_Click(object sender, EventArgs e)
        {
            CreatePictureBitmap();

            if (!error)
            {
                richTextBox1.Clear();

                //Goes through each pixel on image
                for (int x = 0; x < pictureMap.Width; x++)
                {

                    for (int y = 0; y < pictureMap.Height; y++)
                    {
                        pixelColor[x, y] = pictureMap.GetPixel(x, y);                   //Stores pixel color

                        if (!colorIndex.Contains(pixelColor[x, y]))                     //Checks if the pixel color are exists in colorIndex
                            colorIndex.Add(pixelColor[x, y]);                           //If not, adds it to colorIndex

                        pixelColorRef[x, y] = colorIndex.IndexOf(pixelColor[x, y]);     //Adds a reference to the pixel color in the pixelColorRef array
                    }
                }

                indexLabel.Text = "Defined Colors: " + colorIndex.Count();
                sizeLabel.Text = "Picture Size: " + pictureMap.Width.ToString() + " x " + pictureMap.Height.ToString();
                PrintColorIndex();
                PrintPixelColorRef();
            }
        }

        //Load button
        private void loadButton_Click(object sender, EventArgs e)
        {
            CreatePictureBitmap();
        }

        //Creates bitmap of image, resizes it and displays it on picturebox
        private void CreatePictureBitmap()
        {
            try
            {
                if (textBox1.Text != textBoxText || checkBoxState != checkBox1.Checked)                     //runs only if a change was made to pic name or checkbox state
                {
                    pic = Image.FromFile(Application.StartupPath + @"\Images\" + textBox1.Text, true);      //Stores image in pic variable
                    pictureMap = new Bitmap(pic, new Size(picXSize, picYSize));                             //Resizes the image to the set size   

                    if (checkBox1.Checked)
                        pictureMap = pictureMap.Clone(new Rectangle(0, 0, pictureMap.Width, pictureMap.Height), PixelFormat.Format8bppIndexed);                   //This indexes the picture but it's not as good as the website

                    zoomMap = new Bitmap(pictureBox1.Width, pictureBox1.Height);      //Makes a new bitmap zooming in image (for picturebox)

                    //This whole thing zooms the map properly and makes sure to keep the resolution the same
                    using (var gr = Graphics.FromImage(zoomMap))
                    {
                        gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;;
                        gr.DrawImage(pictureMap, new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height));
                    }

                    pictureBox1.Image = zoomMap;

                    //This resets all the labels, textboxes, and variables
                    indexLabel.Text = "Defined Colors: ";
                    sizeLabel.Text = "Picture Size: ";
                    richTextBox1.Clear();
                    richTextBox2.Clear();
                    colorIndex.Clear();
                    textBoxText = textBox1.Text;
                    checkBoxState = checkBox1.Checked;

                    error = false;
                }
            }
            catch (Exception)
            {
                error = true;
                MessageBox.Show("There was an error. Check the path to the image file.");
            }
        }

        //Goes through the colorIndex list and prints it to textbox2 with the appropriate format to work with c++
        private void PrintColorIndex()
        {
            richTextBox2.Clear();

            if (checkBox2.Checked)
            {
                indexOutput = $"const long int clr[][{colorIndex.Count()}] = ";
                indexOutput += "\n{";
            }
            else
                indexOutput = "{";

            for (int i = 0; i < colorIndex.Count; i++)
            {
                if (i == colorIndex.Count - 1)
                {
                    indexOutput += "0x" + ColorTranslator.ToHtml(colorIndex[i]).Substring(1, 6) + "}";
                    if (checkBox2.Checked)
                        indexOutput += ";";
                }
                else
                    indexOutput += "0x" + ColorTranslator.ToHtml(colorIndex[i]).Substring(1, 6) + ", ";
            }

            richTextBox2.Text = indexOutput;
        }

        //Goes through pixelColorRef and prints it to textbox1 with the appropriate format to work with c++
        private void PrintPixelColorRef()
        {
            richTextBox1.Clear();
            if (checkBox2.Checked)
            {
                arrayOutput = $"const byte pixel[][{picXSize}][{picYSize}] PROGMEM = ";
                arrayOutput += "{{";
            }
            else
                arrayOutput = "{";

            for (int x = 0; x < pixelColorRef.GetLength(0); x++)
            {
                arrayOutput += "{";

                for (int y = 0; y < pixelColorRef.GetLength(1); y++)
                {
                    if (y == pixelColorRef.GetLength(1) - 1)
                        arrayOutput += pixelColorRef[x, y];                //Prints out the same color reference twice to get the proportions correct
                    else
                        arrayOutput += pixelColorRef[x, y] + ", ";
                }

                if (x == pixelColorRef.GetLength(0) - 1)
                {
                    arrayOutput += "}}";
                    if (checkBox2.Checked)
                        arrayOutput += "};";
                }
                else
                    arrayOutput += "}, ";
            }

            richTextBox1.Text = arrayOutput;
        }

        //Copy button
        private void button1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(indexOutput + "\n" + arrayOutput); //Copies the outputs to clipboard in correct format to paste in c++
        }

        //This is for getting color by hovering over a pixel on map
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                MouseEventArgs me = (MouseEventArgs)e;
                Point coordinates = me.Location;
                int x = map(coordinates.X, 0, 400, 0, 41);
                int y = map(coordinates.Y, 0, 225, 0, 23);
                label3.Text = "Pixel Color: " + ColorTranslator.ToHtml(pictureMap.GetPixel(x, y));
            }
        }

        //Mapping number function
        private static int map(int value, int fromLow, int fromHigh, int toLow, int toHigh)
        {
            return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
        }
    }
}
