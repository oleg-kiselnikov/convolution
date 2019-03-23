using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Convolution
{
    public static class Convolution
    {
        public static Image Apply(this Image image, Filter filter)
        {
            var sourceBitmap = (Bitmap) image;
            var kernel = filter.Kernel;
            int kernelWidth = kernel.GetLength(1);
            int kernelHeight = kernel.GetLength(0);

            int bitmapWidth = sourceBitmap.Width;
            int bitmapHeight = sourceBitmap.Height;

            int rectangleWidth = bitmapWidth;// - bitmapWidth%kernelWidth;
            int rectangleHeight = bitmapHeight;// - bitmapHeight%kernelHeight;

            var rectangle = new Rectangle(0, 0, rectangleWidth, rectangleHeight);

            var bitmapData = sourceBitmap.LockBits(rectangle, 
                ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            var pixelBuffer = new byte[bitmapData.Stride*bitmapData.Height];
            var resultBuffer = new byte[bitmapData.Stride * bitmapData.Height];


            Marshal.Copy(bitmapData.Scan0, pixelBuffer, 0, pixelBuffer.Length);

            var stride = bitmapData.Stride;

            sourceBitmap.UnlockBits(bitmapData);
            
            int dx = (kernelHeight - 1)/2;
            int dy = (kernelWidth - 1)/2;

            for (int y = dy; y < rectangleHeight - dy; y++)
                for (int x = dx; x < rectangleWidth - dx; x++)
                {
                    double r = 0, g = 0, b = 0;

                    int offset;

                    for (int ky = 0; ky < kernelHeight; ky++)
                    for (int kx = 0; kx < kernelWidth; kx++)
                        {
                            offset = (y - dy + ky) * stride + 3 * (x - dx + kx );

                            var coefficient = kernel[ky, kx];

                            var pbr = pixelBuffer[offset + 2];
                            var pbg = pixelBuffer[offset + 1];
                            var pbb = pixelBuffer[offset + 0];


                            r += coefficient * pbr;
                            g += coefficient * pbg;
                            b += coefficient * pbb;
                        }

                    r *= filter.Factor;
                    g *= filter.Factor;
                    b *= filter.Factor;

                    r += filter.Bias;
                    g += filter.Bias;
                    b += filter.Bias;

                    r = r < 0 ? 0 : r > 255 ? 255 : r;
                    g = g < 0 ? 0 : g > 255 ? 255 : g;
                    b = b < 0 ? 0 : b > 255 ? 255 : b;

                    offset = y*stride + 3 * x;

                    resultBuffer[offset + 2] = (byte)(r);
                    resultBuffer[offset + 1] = (byte)(g);
                    resultBuffer[offset + 0] = (byte)(b);
                }

            var resultBitmap = new Bitmap(rectangleWidth, rectangleHeight);

            var resultData = resultBitmap.LockBits(rectangle, 
                ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);

            resultBitmap.UnlockBits(resultData);

            return resultBitmap;
        }


        public static Image Combine(this Image image1, Image image2)
        {
            var bitmap1 = (Bitmap)image1;
            var bitmap2 = (Bitmap)image2;

            var rectangle = new Rectangle(0, 0, bitmap1.Width, bitmap1.Height);

            var data1 = bitmap1.LockBits(rectangle, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            var data2 = bitmap2.LockBits(rectangle, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            var buffer1 = new byte[data1.Stride * data1.Height];
            var buffer2 = new byte[data2.Stride * data2.Height];
            var resultBuffer = new byte[data1.Stride * data1.Height];


            Marshal.Copy(data1.Scan0, buffer1, 0, buffer1.Length);
            Marshal.Copy(data2.Scan0, buffer2, 0, buffer2.Length);

            bitmap1.UnlockBits(data1);
            bitmap2.UnlockBits(data2);

            for (int offset = 0; offset < data1.Height*data1.Stride; offset++)
            {
                int r = (buffer1[offset] + buffer2[offset])/2;

                r = r < 0 ? 0 : r > 255 ? 255 : r;

                resultBuffer[offset] = (byte)(r);
            }
                
            var resultBitmap = new Bitmap(bitmap1.Width, bitmap1.Height);

            var resultData = resultBitmap.LockBits(rectangle,
                ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);

            resultBitmap.UnlockBits(resultData);

            return resultBitmap;
        }
    }
}
