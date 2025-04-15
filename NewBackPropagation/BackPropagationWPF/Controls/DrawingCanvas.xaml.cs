using BackPropagationWPF.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BackPropagationWPF.Controls
{
    /// <summary>
    /// Interaction logic for DrawingCanvas.xaml
    /// </summary>
    public partial class DrawingCanvas : UserControl
    {
        private WriteableBitmap? drawingSurface;
        private bool isDrawing = false;
        private Point lastPoint;
        private int brushRadius = BackPropagationWPF.Models.BackPropagationConfig.BrushRadius;

        public DrawingCanvas()
        {
            InitializeComponent();
            Loaded += DrawingCanvas_Loaded;
        }

        private void DrawingCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            ResetCanvas();
        }

        /// <summary>
        /// Initializes or resets the canvas to a blank state
        /// </summary>
        public void ResetCanvas()
        {
            // Create a new bitmap with the size of our canvas
            drawingSurface = new WriteableBitmap(
                (int)canvasImage.Width,
                (int)canvasImage.Height,
                96, 96,
                PixelFormats.Bgra32,
                null);

            // Fill with white
            drawingSurface.Clear(Colors.White);

            // Set as the source of our image control
            canvasImage.Source = drawingSurface;
        }

        /// <summary>
        /// Converts the drawing to a 30x30 pixel bitmap for neural network processing
        /// </summary>
        public WriteableBitmap GetProcessedImage()
        {
            if (drawingSurface == null)
                throw new InvalidOperationException("Drawing surface has not been initialized");

            // Create a smaller bitmap (30x30) for processing
            WriteableBitmap smallBitmap = new WriteableBitmap(
                30, 30,
                96, 96,
                PixelFormats.Bgra32,
                null);

            // Scale down the image to 30x30
            using (var drawingContext = smallBitmap.GetRenderTargetForComposition())
            {
                //drawingContext.Clear(Colors.White);

                var visual = new DrawingVisual();
                using (var dc = visual.RenderOpen())
                {
                    dc.DrawImage(
                        drawingSurface,
                        new Rect(0, 0, 30, 30));
                }

                drawingContext.DrawDrawingVisual(visual);
            }

            return smallBitmap;
        }

        /// <summary>
        /// Converts the current drawing to an array of doubles (0-1) for the neural network
        /// </summary>
        public double[] ConvertToInputArray()
        {
            var smallBitmap = GetProcessedImage();
            double[] result = new double[30 * 30];
            int index = 0;

            // Lock the bitmap for reading
            smallBitmap.Lock();

            try
            {
                unsafe
                {
                    for (int y = 0; y < 30; y++)
                    {
                        for (int x = 0; x < 30; x++)
                        {
                            // Get pixel color
                            var color = smallBitmap.GetPixel(x, y);

                            // Convert to grayscale and normalize to 0-1 range
                            // For black pixels value will be close to 1, for white - close to 0
                            double value = 1.0 - ((color.R + color.G + color.B) / (3.0 * 255.0));

                            // Threshold to make binary
                            result[index++] = value > 0.5 ? 1.0 : 0.0;
                        }
                    }
                }
            }
            finally
            {
                // Unlock the bitmap
                smallBitmap.Unlock();
            }

            return result;
        }

        #region Event Handlers

        private void CanvasImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                isDrawing = true;
                lastPoint = e.GetPosition(canvasImage);
                DrawPoint(lastPoint);
            }
        }

        private void CanvasImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing && e.LeftButton == MouseButtonState.Pressed)
            {
                Point currentPoint = e.GetPosition(canvasImage);
                DrawLine(lastPoint, currentPoint);
                lastPoint = currentPoint;
            }
        }

        private void CanvasImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isDrawing = false;
        }

        #endregion

        #region Drawing Methods

        private void DrawPoint(Point point)
        {
            if (drawingSurface == null)
                return;

            drawingSurface.Lock();

            try
            {
                // Draw a circle at the point
                for (int x = -brushRadius; x <= brushRadius; x++)
                {
                    for (int y = -brushRadius; y <= brushRadius; y++)
                    {
                        if (x * x + y * y <= brushRadius * brushRadius)
                        {
                            int posX = (int)point.X + x;
                            int posY = (int)point.Y + y;

                            if (posX >= 0 && posX < drawingSurface.PixelWidth &&
                                posY >= 0 && posY < drawingSurface.PixelHeight)
                            {
                                drawingSurface.SetPixel(posX, posY, Colors.Black);
                            }
                        }
                    }
                }
            }
            finally
            {
                drawingSurface.Unlock();
            }
        }

        private void DrawLine(Point from, Point to)
        {
            if (drawingSurface == null)
                return;

            // Bresenham's line algorithm
            int x0 = (int)from.X;
            int y0 = (int)from.Y;
            int x1 = (int)to.X;
            int y1 = (int)to.Y;

            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            int err = dx - dy;

            drawingSurface.Lock();

            try
            {
                while (true)
                {
                    // Draw a point at each step of the line
                    for (int x = -brushRadius; x <= brushRadius; x++)
                    {
                        for (int y = -brushRadius; y <= brushRadius; y++)
                        {
                            if (x * x + y * y <= brushRadius * brushRadius)
                            {
                                int posX = x0 + x;
                                int posY = y0 + y;

                                if (posX >= 0 && posX < drawingSurface.PixelWidth &&
                                    posY >= 0 && posY < drawingSurface.PixelHeight)
                                {
                                    drawingSurface.SetPixel(posX, posY, Colors.Black);
                                }
                            }
                        }
                    }

                    if (x0 == x1 && y0 == y1) break;

                    int e2 = 2 * err;
                    if (e2 > -dy)
                    {
                        err -= dy;
                        x0 += sx;
                    }
                    if (e2 < dx)
                    {
                        err += dx;
                        y0 += sy;
                    }
                }
            }
            finally
            {
                drawingSurface.Unlock();
            }
        }

        #endregion
    }
}
