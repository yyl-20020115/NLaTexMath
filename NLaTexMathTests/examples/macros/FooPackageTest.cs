/* Main.cs
 * =========================================================================
 * This file is part of the JLaTeXMath Library - http://jlatexmath.sourceforge.net
 *
 * Copyright (C) 2009 DENIZET Calixte
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or (at
 * your option) any later version.
 *
 * This program is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * General Public License for more details.
 *
 * A copy of the GNU General Public License can be found in the file
 * LICENSE.txt provided with the source distribution of this program (see
 * the META-INF directory in the source jar). This license can also be
 * found on the GNU website at http://www.gnu.org/licenses/gpl.html.
 *
 * If you did not receive a copy of the GNU General Public License along
 * with this program, contact the lead developer, or write to the Free
 * Software Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA
 * 02110-1301, USA.
 *
 * Linking this library statically or dynamically with other modules
 * is making a combined work based on this library. Thus, the terms
 * and conditions of the GNU General Public License cover the whole
 * combination.
 *
 * As a special exception, the copyright holders of this library give you
 * permission to link this library with independent modules to produce
 * an executable, regardless of the license terms of these independent
 * modules, and to copy and distribute the resulting executable under terms
 * of your choice, provided that you also meet, for each linked independent
 * module, the terms and conditions of the license of that module.
 * An independent module is a module which is not derived from or based
 * on this library. If you modify this library, you may extend this exception
 * to your version of the library, but you are not obliged to do so.
 * If you do not wish to do so, delete this exception statement from your
 * version.
 *
 */
using NLaTexMath;
using NLaTexMath.Internal.Util;
using System.Drawing;
using System.Drawing.Imaging;

namespace NLaTexMathTests.Examples.Macros;


/**
 * A class to test LaTeX rendering.
 **/
[TestClass]
public class FooPackageTest
{
    [TestMethod]
    public void TestUseCustomPackage()
    {
        var stream = typeof(FooPackageTest).GetResourceAsStream("Package_Foo.xml");
        Assert.IsNotNull(stream);
        TeXFormula.AddPredefinedCommands(stream);
        var latex = "\\begin{array}{l}";
        latex += "\\fooA{\\pi}{C}\\\\";
        latex += "\\mbox{A red circle }\\fooB{75.3}\\\\";
        latex += "\\mbox{A red disk }\\fooC[abc]{126.7}\\\\";
        latex += "\\mbox{An other red circle }\\fooD{159.81}[ab]";
        latex += "\\end{array}";

        var formula = new TeXFormula(latex);
        var icon = formula.CreateTeXIcon(TeXConstants.STYLE_DISPLAY, 20);
        icon.Insets = new Insets(5, 5, 5, 5);

        var image = new Bitmap(icon.IconWidth, icon.IconHeight);
        using var g = Graphics.FromImage(image);
        using var brush = new SolidBrush(Color.White);
        g.FillRectangle(brush, 0, 0, icon.IconWidth, icon.IconHeight);
        icon.PaintIcon(g);
        var file = "target/ExampleMacros.png";
        image.Save(file, ImageFormat.Png);
    }
}
