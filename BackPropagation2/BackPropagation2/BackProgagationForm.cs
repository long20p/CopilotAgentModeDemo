using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;

namespace BackPropagation2
{
    public partial class BackProgagationForm : Form
    {

        private Network network;
        private Bitmap smallImage;

        public BackProgagationForm()
        {
            InitializeComponent();
            BPGlobalAccess.BPForm = this;
            patternPathTxt.Text = Directory.GetCurrentDirectory() + "\\Letters";
        }

        private void testResetBtn_Click(object sender, EventArgs e)
        {
            testingPanel.ResetCanvas();
            clearSmallImage();
            updateSmallImage();
            ClearResultLabel();

        }

        private void trainBtn_Click(object sender, EventArgs e)
        {
            int hiddenLayerCount = getHiddenLayerCount();
            int[] hiddenLayerElemCount = getHiddenLayerElementCount(hiddenLayerCount);
            int maxIteration = getMaxIteration();
            double allowedError = getAllowedError();
            if (hiddenLayerCount == -1 || hiddenLayerElemCount == null ||
                maxIteration == -1 || allowedError == -1)
                return;
            try
            {
                ClearProcessLabel();
                UpdateProcessLabel("Generating training set...");
                generateTrainingSet();
                UpdateProcessLabel("Training set generated successfully");
                UpdateProcessLabel("Creating network...");
                network = new Network(hiddenLayerCount, hiddenLayerElemCount,
                    BPGlobalAccess.OUTPUT_COUNT);
                network.AllowedError = allowedError;
                Program.MaxIteration = maxIteration;
                network.Train();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClearProcessLabel();
            }
        }

        private void patternBrowseBtn_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = patternPathTxt.Text;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                patternPathTxt.Text = fbd.SelectedPath;
            }
        }

        private void recognizeBtn_Click(object sender, EventArgs e)
        {
            if (network == null)
            {
                MessageBox.Show("Network needs to be trained first", "Untrained network", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    testingPanel.SetActiveSection();
                    smallImage = testingPanel.GetSmallImage();
                    updateSmallImage();
                    ClearResultLabel();
                    UpdateResultLabel("Fetching data from image...");
                    double[] inputs = convertBitmapToArray(smallImage);
                    network.Recognize(inputs);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Get letter images, processing pixels and create patterns
        /// </summary>
        private void generateTrainingSet()
        {
            Program.TrainingSet.Clear();
            String[] letters = Directory.GetFiles(patternPathTxt.Text, "*.bmp");
            for (int i = 0; i < letters.Length; i++)
            {
                Bitmap letterImage = new Bitmap(letters[i]);
                double[] inputs = convertBitmapToArray(letterImage);
                double[] outputs = new double[letters.Length];
                outputs[i] = 1;
                Pattern pat = new Pattern(inputs, outputs);
                Program.TrainingSet.Add(pat);
            }
            BPGlobalAccess.INPUT_COUNT = Program.TrainingSet.ElementAt(0).InputsX.Length;
            BPGlobalAccess.OUTPUT_COUNT = Program.TrainingSet.ElementAt(0).ExpectedOutput.Length;
        }

        /// <summary>
        /// Get all the pixels in image and convert them to array of doubles
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        private double[] convertBitmapToArray(Bitmap bmp)
        {
            double[] pixels = new double[bmp.Height * bmp.Width];
            for (int i = 0; i < bmp.Height; i++)
            {
                for (int j = 0; j < bmp.Width; j++)
                {
                    Color pixelColor = bmp.GetPixel(j, i);
                    if (pixelColor.Name == "ffffffff") // White
                        pixels[i * bmp.Width + j] = 0;
                    else
                        pixels[i * bmp.Width + j] = 1;
                }
            }
            return pixels;
        }

        public void UpdateProcessLabel(String text)
        {
            this.processLbl.Text += text + "\n";
            this.processLbl.Update();
        }

        public void UpdateResultLabel(String text)
        {
            this.resultLbl.Text += text + "\n";
            this.resultLbl.Update();
        }

        public void ClearProcessLabel()
        {
            this.processLbl.Text = "";
            this.processLbl.Update();
        }

        public void ClearResultLabel()
        {
            this.resultLbl.Text = "";
            this.resultLbl.Update();
        }

        private void updateSmallImage()
        {
            smallImagePanel.Invalidate();
            smallImagePanel.Update();
        }

        private void clearSmallImage()
        {
            smallImage = new Bitmap(30, 30);
            for (int i = 0; i < smallImage.Width; i++)
            {
                for (int j = 0; j < smallImage.Height; j++)
                {
                    smallImage.SetPixel(i, j, Color.White);
                }
            }
        }

        private void smallImagePanel_Paint(object sender, PaintEventArgs e)
        {
            if (smallImage != null)
            {
                Graphics g = e.Graphics;
                g.DrawImage(smallImage, 0, 0);
                g.Dispose();
            }
        }

        private int getHiddenLayerCount()
        {
            try
            {
                int count = Int32.Parse(hidLayerCountTxt.Text);
                return count;
            }
            catch (Exception e)
            {
                MessageBox.Show("Hidden layer count value is invalid", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }

        private int[] getHiddenLayerElementCount(int layerCount)
        {
            try
            {
                string[] counts = Regex.Split(hidLayerElemCountTxt.Text, ",");
                if (counts.Length != layerCount)
                {
                    throw new Exception();
                }
                int[] elemCount = new int[counts.Length];
                for (int i = 0; i < counts.Length; i++)
                {
                    elemCount[i] = Int32.Parse(counts[i]);
                }
                return elemCount;
            }
            catch (Exception e)
            {
                MessageBox.Show("Hidden layer element count value should be in format 'c1,c2,c3,...' and total count must match layer count",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private int getMaxIteration()
        {
            try
            {
                int max = Int32.Parse(maxIterationTxt.Text);
                return max;
            }
            catch (Exception e)
            {
                MessageBox.Show("Max iteration value is invalid", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }

        private double getAllowedError()
        {
            try
            {
                double allow = Double.Parse(allowedErrorTxt.Text);
                return allow;
            }
            catch (Exception e)
            {
                MessageBox.Show("Allowed error value is invalid", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }

    }
}
