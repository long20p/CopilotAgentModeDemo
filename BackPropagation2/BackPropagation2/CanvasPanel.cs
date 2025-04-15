using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace BackPropagation2
{
    class CanvasPanel : Panel
    {

        private bool mousePress = false;
        private Brush canvasBrush = Brushes.Black;
        private Graphics gCanvas;
        private Graphics gMask;
        private Bitmap bufferMask;
        private Bitmap activeSection;

        public Bitmap BufferMask
        {
            get { return bufferMask; }
            set { bufferMask = value; }
        }

        public CanvasPanel()
        {
            this.BorderStyle = BorderStyle.FixedSingle;
            this.MouseMove += new MouseEventHandler(this.CanvasPanel_MouseMove);
            this.MouseDown += new MouseEventHandler(this.CanvasPanel_MouseDown);
            this.MouseUp += new MouseEventHandler(this.CanvasPanel_MouseUp);
            this.Paint += new PaintEventHandler(CanvasPanel_Paint);
            refreshMask();
        }

        private void InitializeComponent()
        {
        }

        void CanvasPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(bufferMask, 0, 0);
            g.Dispose();
        }

        private void CanvasPanel_MouseDown(object sender, MouseEventArgs e)
        {
            mousePress = true;
            gMask = Graphics.FromImage(bufferMask);
            gMask.FillEllipse(canvasBrush, e.X, e.Y, BPGlobalAccess.BRUSH_RADIUS * 2, BPGlobalAccess.BRUSH_RADIUS * 2);
            gCanvas = CreateGraphics();
            gCanvas.DrawImage(bufferMask, 0, 0);
            gMask.Dispose();
            gCanvas.Dispose();
        }

        private void CanvasPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (mousePress)
            {
                //int upperLeftX = e.X > BPGlobalAccess.BRUSH_RADIUS ? e.X - BPGlobalAccess.BRUSH_RADIUS : 0;
                //int upperLeftY = e.Y > BPGlobalAccess.BRUSH_RADIUS ? e.Y - BPGlobalAccess.BRUSH_RADIUS : 0;
                //int width = upperLeftX == 0 ? e.X + BPGlobalAccess.BRUSH_RADIUS : BPGlobalAccess.BRUSH_RADIUS * 2;
                //int height = upperLeftY == 0 ? e.Y + BPGlobalAccess.BRUSH_RADIUS : BPGlobalAccess.BRUSH_RADIUS * 2;
                gMask = Graphics.FromImage(bufferMask);
                //gMask.FillEllipse(canvasBrush, upperLeftX, upperLeftY, width, height);
                gMask.FillEllipse(canvasBrush, e.X, e.Y, BPGlobalAccess.BRUSH_RADIUS * 2, BPGlobalAccess.BRUSH_RADIUS * 2);
                gCanvas = CreateGraphics();
                gCanvas.DrawImage(bufferMask, 0, 0);
                gMask.Dispose();
                gCanvas.Dispose();
            }
        }

        private void CanvasPanel_MouseUp(object sender, MouseEventArgs e)
        {
            mousePress = false;
        }

        /// <summary>
        /// Set all pixels in bitmap to white color
        /// </summary>
        private void refreshMask()
        {
            bufferMask = new Bitmap(200, 200);
            for (int i = 0; i < bufferMask.Width; i++)
            {
                for (int j = 0; j < bufferMask.Height; j++)
                {
                    bufferMask.SetPixel(i, j, Color.White);
                }
            }
        }

        public void ResetCanvas()
        {
            refreshMask();
            Invalidate();
            Update();
        }

        /// <summary>
        /// Capture the area containing actual drawing
        /// </summary>
        public void SetActiveSection()
        {
            int leftmost = Int32.MaxValue, rightmost = -1, topmost = Int32.MaxValue, bottommost = -1;
            for (int i = 0; i < bufferMask.Height; i++)
            {
                for (int j = 0; j < bufferMask.Width; j++)
                {
                    Color pixelColor = bufferMask.GetPixel(j, i);
                    if (pixelColor.Name == "ff000000") // Black
                    {
                        if (i < topmost)
                            topmost = i;
                        if (i > bottommost)
                            bottommost = i;
                        if (j < leftmost)
                            leftmost = j;
                        if (j > rightmost)
                            rightmost = j;
                    }
                }
            }
            int activeWidth = rightmost - leftmost;
            int activeHeight = bottommost - topmost;
            int activeEdgeLength = activeHeight > activeWidth ? activeHeight : activeWidth;
            activeSection = new Bitmap(activeEdgeLength, activeEdgeLength);
            Graphics g = Graphics.FromImage(activeSection);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            int startPointX = leftmost + activeWidth / 2 - activeEdgeLength / 2;
            int startPointY = topmost + activeHeight / 2 - activeEdgeLength / 2;
            int bufferLength = (int)(activeEdgeLength * 1.1);
            g.DrawImage(bufferMask, new Rectangle(0, 0, activeEdgeLength, activeEdgeLength),
                new Rectangle(startPointX, startPointY, bufferLength, bufferLength),
                GraphicsUnit.Pixel);
            g.Dispose();
        }

        /// <summary>
        /// Draw to 30x30 bitmap
        /// </summary>
        /// <returns></returns>
        public Bitmap GetSmallImage()
        {
            Bitmap small = new Bitmap(30, 30);
            //small.SetResolution(bufferMask.HorizontalResolution, bufferMask.VerticalResolution);
            Graphics g = Graphics.FromImage(small);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(activeSection, new Rectangle(0, 0, 30, 30),
                new Rectangle(0, 0, activeSection.Width, activeSection.Height), 
                GraphicsUnit.Pixel);
            g.Dispose();
            return small;
        }
    }
}
