/* FcscoreBox.cs
 * =========================================================================
 * This file is part of the JLaTeXMath Library - http://forge.scilab.org/jlatexmath
 *
 * Copyright (C) 2013 DENIZET Calixte
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

namespace NLaTexMath;

using System.Drawing;
/**
 * A box representing glue.
 */
public class FcscoreBox : Box
{

    private readonly int N;
    private readonly bool strike;
    private readonly float space;
    private readonly float thickness;

    public FcscoreBox(int N, float h, float thickness, float space, bool strike)
    {
        this.N = N;
        this.width = N * (thickness + space) + 2 * space;
        this.height = h;
        this.depth = 0;
        this.strike = strike;
        this.space = space;
        this.thickness = thickness;
    }

    public override void Draw(Graphics g2, float x, float y)
    {
        var transf = g2.Transform.Clone();

        float sx = this.scaleX;
        float sy = this.scaleY;
        double s = 1;
        if (sx == sy)
        {
            // There are rounding problems due to scale factor: lines could have different
            // spacing...
            // So the increment (space+thickness) is done in using integer.
            s = sx;
            g2.Transform.Scale(1.0f / sx, 1.0f / sy);
        }
        
        //g2.setStroke(new BasicStroke((float)(s * thickness), BasicStroke.CAP_BUTT, BasicStroke.JOIN_MITER));
        //float th = thickness / 2.0f;

        //float xx = x + space;
        //xx = (float)(xx * s + (space / 2.0f) * s);
        //int inc = (int)Math.Round((space + thickness) * s);

        //for (int i = 0; i < N; i++)
        //{
        //    line.setLine(xx + th * s, (y - height) * s, xx + th * s, y * s);
        //    g2.draw(line);
        //    xx += inc;
        //}

        //if (strike)
        //{
        //    line.setLine((x + space) * s, (y - height / 2.0f) * s, xx - s * space / 2, (y - height / 2.0f) * s);
        //    g2.draw(line);
        //}
        //g2.Transform = transf;
    }

    public override int LastFontId => TeXFont.NO_FONT;
}
