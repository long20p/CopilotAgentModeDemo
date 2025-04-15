using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BackPropagationWPF.Extensions
{
    public static class WriteableBitmapExtensions
    {
        /// <summary>
        /// Clears the bitmap with the specified color
        /// </summary>
        public static void Clear(this WriteableBitmap writeableBitmap, Color color)
        {
            try
            {
                writeableBitmap.Lock();

                // Fill the bitmap with the specified color
                var pixels = new int[writeableBitmap.PixelWidth * writeableBitmap.PixelHeight];
                int colorValue = (color.A << 24) | (color.R << 16) | (color.G << 8) | color.B;

                for (int i = 0; i < pixels.Length; i++)
                {
                    pixels[i] = colorValue;
                }

                System.Runtime.InteropServices.Marshal.Copy(pixels, 0,
                    writeableBitmap.BackBuffer, pixels.Length);

                writeableBitmap.AddDirtyRect(
                    new Int32Rect(0, 0, writeableBitmap.PixelWidth, writeableBitmap.PixelHeight));
            }
            finally
            {
                writeableBitmap.Unlock();
            }
        }

        /// <summary>
        /// Gets a pixel color from the bitmap
        /// </summary>
        public static Color GetPixel(this WriteableBitmap writeableBitmap, int x, int y)
        {
            IntPtr pBackBuffer = writeableBitmap.BackBuffer;
            int stride = writeableBitmap.BackBufferStride;

            // Get the pixel color
            unsafe
            {
                byte* pPixel = (byte*)pBackBuffer.ToPointer() + y * stride + x * 4;
                return Color.FromArgb(
                    pPixel[3], // Alpha
                    pPixel[2], // Red
                    pPixel[1], // Green
                    pPixel[0]  // Blue
                );
            }
        }

        /// <summary>
        /// Sets a pixel color in the bitmap
        /// </summary>
        public static void SetPixel(this WriteableBitmap writeableBitmap, int x, int y, Color color)
        {
            IntPtr pBackBuffer = writeableBitmap.BackBuffer;
            int stride = writeableBitmap.BackBufferStride;

            // Set the pixel color
            unsafe
            {
                byte* pPixel = (byte*)pBackBuffer.ToPointer() + y * stride + x * 4;
                pPixel[0] = color.B; // Blue
                pPixel[1] = color.G; // Green
                pPixel[2] = color.R; // Red
                pPixel[3] = color.A; // Alpha
            }

            // Mark as dirty
            writeableBitmap.AddDirtyRect(new Int32Rect(x, y, 1, 1));
        }

        /// <summary>
        /// Creates a render target for further composition
        /// </summary>
        public static DrawingContext GetRenderTargetForComposition(this WriteableBitmap writeableBitmap)
        {
            var drawingVisual = new DrawingVisual();
            return drawingVisual.RenderOpen();
        }

        /// <summary>
        /// Draws a visual onto the drawing context
        /// </summary>
        public static void DrawDrawingVisual(this DrawingContext drawingContext, DrawingVisual visual)
        {
            drawingContext.DrawDrawing(visual.Drawing);
        }
    }
}
