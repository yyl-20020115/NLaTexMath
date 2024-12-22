namespace NLaTexMath.Internal.Util;

using System.Drawing;

public static class Images
{
    public const double DISTANCE_THRESHOLD = 40;

    public static double Distance(Bitmap? imgA, Bitmap? imgB)
    {
        // The images must be the same size.
        if (imgA!=null && imgB!=null && imgA.Width == imgB.Width && imgA.Height == imgB.Height)
        {
            var width = imgA.Width;
            var height = imgA.Height;

            var mse = 0.0;
            // Loop over every pixel.
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var ca = imgA.GetPixel(x, y);
                    var cb = imgB.GetPixel(x, y);
                    var variance = Square(ca.R - cb.R) 
                        + Square(ca.B - cb.B) 
                        + Square(ca.G - cb.G) 
                        + Square(ca.A - cb.A)
                        ;
                    mse += variance;
                }
            }
            return Math.Sqrt(mse / height / width);
        }
        return -1;
    }

    private static double Square(double x) => x * x;
}
