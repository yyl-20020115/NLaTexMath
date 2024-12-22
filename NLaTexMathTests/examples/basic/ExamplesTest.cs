/* Main.cs
 * =========================================================================
 * This file is part of the JLaTeXMath Library - http://jlatexmath.sourceforge.net
 *
 * Copyright (C) 2009-2011 DENIZET Calixte
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
using NLaTexMath.Internal.Util;
using System.Drawing;

namespace NLaTexMathTests.Examples.Basic;

[TestClass]
public class ExamplesTest
{

    [TestMethod]
    public void TestExample1()
    {
        Example1._Main([]);
        Check("Example1.png");
    }

    [TestMethod]
    public void TestExample2()
    {
        Example2._Main([]);
        Check("Example2.png");
    }

    [TestMethod]
    public void TestExample3()
    {
        Example3._Main([]);
        Check("Example3.png");
    }

    [TestMethod]
    public void TestExample4()
    {
        Example4._Main([]);
        Check("Example4.png");
    }

    [TestMethod]
    public void TestExample5()
    {
        Example5._Main([]);
        Check("Example5.png");
    }

    [TestMethod]
    public void TestExample6()
    {
        Example6._Main([]);
        Check("Example6.png");
    }

    [TestMethod]
    public void TestExample8()
    {
        Example8._Main([]);
        Check("Example8.png");
    }

    [TestMethod]
    public void TestExample9()
    {
        Example9._Main([]);
        Check("Example9.png");
    }

    private static void Check(string filename)
    {
        try
        {
            Console.WriteLine("checking image " + filename);
            var a = Image.FromFile("src/test/resources/expected/" + filename) as Bitmap;
            var b = Image.FromFile("target/" + filename) as Bitmap;
            var distance = Images.Distance(a, b);
            Console.WriteLine("distance=" + distance);
            var THRESHOLD = Images.DISTANCE_THRESHOLD;
            Assert.IsTrue(distance >= 0, $"actual and expected images for {filename} are different sizes!");
            Assert.IsTrue(distance <= THRESHOLD,
                $"distance={distance} is above threshold={THRESHOLD}, images are probably significantly different, distance={distance}"
                );
        }
        catch (IOException e)
        {
            throw e;
        }
    }

}
