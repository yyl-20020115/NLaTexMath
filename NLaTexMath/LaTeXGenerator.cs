using System.Drawing;
namespace NLaTexMath;

public static class LaTeXGenerator
{
    /**
     * Generate a PNG with the given path and LaTeX formula
     * @param formula the formula to compile
     * @param path the image path
     */
    public static void Generate(string formula, string path)
    {
        var tf = new TeXFormula(formula);
        var ti = tf.CreateTeXIcon(TeXConstants.STYLE_DISPLAY, 40);
        using var bitmap = new Bitmap(ti.IconWidth, ti.IconHeight);

        using var g = Graphics.FromImage(bitmap);
        using var b = new SolidBrush(Color.White);
        g.FillRectangle(b, new RectangleF(0, 0, ti.IconWidth, ti.IconHeight));
        ti.PaintIcon(g, 0, 0);
        bitmap.Save(path);
    }
}