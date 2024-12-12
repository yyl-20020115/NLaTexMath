/* Main.java
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
namespace org.scilab.forge.jlatexmath.examples.basic;

[TestClass]
public class ExamplesTest
{

    [TestMethod]
    public void TestExample1()
    {
        Example1.main([]);
        Check("Example1.png");
    }

    [TestMethod]
    public void TestExample2()
    {
        Example2.main([]);
        Check("Example2.png");
    }

    [TestMethod]
    public void TestExample3()
    {
        Example3.main([]);
        Check("Example3.png");
    }

    [TestMethod]
    public void TestExample4()
    {
        Example4.main([]);
        Check("Example4.png");
    }

    [TestMethod]
    public void TestExample5()
    {
        Example5.main([]);
        Check("Example5.png");
    }

    [TestMethod]
    public void TestExample6()
    {
        Example6.main([]);
        Check("Example6.png");
    }

    [TestMethod]
    public void TestExample8()
    {
        Example8.main([]);
        Check("Example8.png");
    }

    [TestMethod]
    public void TestExample9()
    {
        Example9.main([]);
        Check("Example9.png");
    }

    private static void Check(String filename)
    {
        try
        {
            System._out.println("checking image " + filename);
            BufferedImage a = ImageIO.read(new File("src/test/resources/expected/" + filename));
            BufferedImage b = ImageIO.read(new File("target/" + filename));
            double distance = Images.distance(a, b);
            System._out.println("distance=" + distance);
            // TODO establish a reasonable threshold after running the tests on
            // different platforms (windows, osx, linux, others?) and different
            // jdks
            double THRESHOLD = Images.DISTANCE_THRESHOLD;
            assertTrue("actual and expected images for " + filename + " are different sizes!", distance >= 0);
            assertTrue(
                "distance=" + distance + " is above threshold=" + THRESHOLD
                + ", images are probably significantly different, distance=" + distance,
                distance <= THRESHOLD);
        }
        catch (IOException e)
        {
            throw (e);
        }
    }

}
