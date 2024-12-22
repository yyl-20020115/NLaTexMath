using NLaTexMath;
using System.Windows.Forms;

namespace NLaTexMathTests.Examples.Basic;

[TestClass]
public class ExampleSwing2
{
    [TestMethod]
    public void Test()
    {
        //TODO:
        var latex = "\\text{hello world}";
        var formula = new TeXFormula(latex);
        var icon = new TeXFormula.TeXIconBuilder().SetStyle(TeXConstants.STYLE_DISPLAY)
                       .SetSize(16)
                       .SetWidth(TeXConstants.UNIT_PIXEL, 256f, TeXConstants.ALIGN_CENTER)
                       .SetIsMaxWidth(true).SetInterLineSpacing(TeXConstants.UNIT_PIXEL, 20f)
                       .Build();

        //var form = new Form();

        //JFrame frame = new JFrame();
        //final JLabel label = new JLabel(icon);
        //label.setMaximumSize(new Dimension(100, 300));
        //label.setBorder(BorderFactory.createEmptyBorder(10, 10, 10, 10));
        //frame.getContentPane().add(label);

        //frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        //frame.pack();
        //frame.setVisible(true);
    }

}
