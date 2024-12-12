using System.Drawing;
namespace NLaTexMath;

public static class LaTeXGenerator
{
    /**
     * Generate a PNG with the given path and LaTeX formula
     * @param formula the formula to compile
     * @param path the image path
     */
    public static void Generate(String formula, String path)
    {
        var tf = new TeXFormula(formula);
        var ti = tf.CreateTeXIcon(TeXConstants.STYLE_DISPLAY, 40);
        var bitmap = new Bitmap(ti.GetIconWidth(), ti.GetIconHeight());

        using var g2d = Graphics.FromImage(bitmap);

        //g2d.setColor(Color.White);
        //g2d.fillRect(0, 0, ti.getIconWidth(), ti.getIconHeight());
        
        //jl.setForeground(new Color());
        //ti.paintIcon(jl, g2d, 0, 0);

        bitmap.Save(path);
    }
}