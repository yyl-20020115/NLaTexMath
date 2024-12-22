using NLaTexMath;
using System.Drawing;
using System.Drawing.Imaging;

namespace NLaTexMathTests.Examples.Basic;

public class TestBase
{
    public static void DoTest(string latex, string testname)
    {
        var formula = new TeXFormula(latex);

        var icon = new TeXFormula.TeXIconBuilder().SetStyle(TeXConstants.STYLE_DISPLAY).SetSize(20).Build();

        var Insets = new Insets(5, 5, 5, 5);

        using var image = new Bitmap(icon.IconWidth, icon.IconHeight);
        using var g = Graphics.FromImage(image);
        using var brush = new SolidBrush(Color.White);
        g.FillRectangle(brush, new RectangleF(0, 0, icon.IconWidth, icon.IconHeight));
        icon.PaintIcon(g, 0, 0);
        var file = $"target/{testname}.png";
        image.Save(file,ImageFormat.Png);

    }
}
