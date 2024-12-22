using NLaTexMath;
using System.Drawing;

namespace NLaTexMathTests.Examples.Basic;

[TestClass]
public class ExamplesFile
{
    [TestMethod]
    public void Test()
    {
        var latex = "\\text{hello world}";
        var formula = new TeXFormula(latex);
        var icon = new TeXFormula.TeXIconBuilder().SetStyle(TeXConstants.STYLE_DISPLAY)
                       .SetSize(16)
                       .SetWidth(TeXConstants.UNIT_PIXEL, 256f, TeXConstants.ALIGN_CENTER)
                       .SetIsMaxWidth(true).SetInterLineSpacing(TeXConstants.UNIT_PIXEL, 20f)
                       .Build();
        using var image = new Bitmap(120, 120);
        using var g = Graphics.FromImage(image);
        icon.PaintIcon(g, 0, 0);
        image.Save("test.png");
    }

}
