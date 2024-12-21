namespace NLaTexMathTests.Examples.Basic;

public class TestBase
{
    public static void DoTest(string latex, string testname)
    {
        //TeXFormula formula = new TeXFormula(latex);

        //TODO:
        // Note: Old interface for creating icons:
        // TeXIcon icon = formula.createTeXIcon(TeXConstants.STYLE_DISPLAY, 20);
        // Note: New interface using builder pattern (inner class):
        //TeXIcon icon = new TeXFormula.TeXIconBuilder().SetStyle(TeXConstants.STYLE_DISPLAY).SetSize(20)
        //               .build();

        //icon.
        //Insets = new Insets(5, 5, 5, 5);

        //Bitmap image = new Bitmap(icon.GetIconWidth(), icon.GetIconHeight(), BufferedImage.TYPE_INT_ARGB);
        //Graphics g2 = image.CreateGraphics();
        //g2.setColor(Color.white);
        //g2.fillRect(0, 0, icon.GetIconWidth(), icon.GetIconHeight());
        //JLabel jl = new JLabel();
        //jl.setForeground(new Color(0, 0, 0));
        //icon.paintIcon(jl, g2, 0, 0);
        //File file = new File("target/Example1.png");
        //ImageIO.write(image, "png", file.getAbsoluteFile());

    }
}
