/* RotateBox.java
 * =========================================================================
 * This file is part of the JLaTeXMath Library - http://forge.scilab.org/jlatexmath
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

namespace NLaTexMath;

using System.Drawing;

/**
 * A box representing a rotated box.
 */
public class RotateBox : Box
{

    public static readonly int BL = 0;
    public static readonly int BC = 1;
    public static readonly int BR = 2;
    public static readonly int TL = 3;
    public static readonly int TC = 4;
    public static readonly int TR = 5;
    public static readonly int BBL = 6;
    public static readonly int BBR = 7;
    public static readonly int BBC = 8;
    public static readonly int CL = 9;
    public static readonly int CC = 10;
    public static readonly int CR = 11;

    protected double angle = 0;
    private Box box;
    private float xmax, xmin, ymax, ymin;

    private float shiftX;
    private float shiftY;

    public RotateBox(Box b, double angle, float x, float y)
    {
        this.box = b;
        this.angle = angle * Math.PI / 180;
        height = b.height;
        depth = b.depth;
        width = b.width;
        double s = Math.Sin(this.angle);
        double c = Math.Cos(this.angle);
        shiftX = (float)(x * (1 - c) + y * s);
        shiftY = (float)(y * (1 - c) - x * s);
        xmax = (float)Math.Max(-height * s, Math.Max(depth * s, Math.Max(width * c + depth * s, width * c - height * s))) + shiftX;
        xmin = (float)Math.Min(-height * s, Math.Min(depth * s, Math.Min(width * c + depth * s, width * c - height * s))) + shiftX;
        ymax = (float)Math.Max(height * c, Math.Max(-depth * c, Math.Max(width * s - depth * c, width * s + height * c)));
        ymin = (float)Math.Min(height * c, Math.Min(-depth * c, Math.Min(width * s - depth * c, width * s + height * c)));
        width = xmax - xmin;
        height = ymax + shiftY;
        depth = -ymin - shiftY;
    }

    public RotateBox(Box b, double angle, Point origin)
    {
        this(b, angle, origin.X, origin.Y);
    }

    public RotateBox(Box b, double angle, int option)
    {
        this(b, angle, calculateShift(b, option));
    }

    public static int getOrigin(string option)
    {
        if (option == null || option.Length == 0)
        {
            return BBL;
        }

        if (option.Length == 1)
        {
            option += "c";
        }
        if (option == ("bl") || option == ("lb"))
        {
            return BL;
        }
        else if (option == ("bc") || option == ("cb"))
        {
            return BC;
        }
        else if (option == ("br") || option == ("rb"))
        {
            return BR;
        }
        else if (option == ("cl") || option == ("lc"))
        {
            return CL;
        }
        else if (option == ("cc"))
        {
            return CC;
        }
        else if (option == ("cr") || option == ("cr"))
        {
            return CR;
        }
        else if (option == ("tl") || option == ("lt"))
        {
            return TL;
        }
        else if (option == ("tc") || option == ("ct"))
        {
            return TC;
        }
        else if (option == ("tr") || option == ("rt"))
        {
            return TR;
        }
        else if (option == ("Bl") || option == ("lB"))
        {
            return BBL;
        }
        else if (option == ("Bc") || option == ("cB"))
        {
            return BBC;
        }
        else if (option == ("Br") || option == ("rB"))
        {
            return BBR;
        }
        else

            return BBL;
    }

    private static PointF calculateShift(Box b, int option)
    {
        PointF p = new PointF(0, -b.depth);
        switch (option)
        {
            case BL:
                p.X = 0;
                p.Y = -b.depth;
                break;
            case BR:
                p.X = b.width;
                p.Y = -b.depth;
                break;
            case BC:
                p.X = b.width / 2;
                p.Y = -b.depth;
                break;
            case TL:
                p.X = 0;
                p.Y = b.height;
                break;
            case TR:
                p.X = b.width;
                p.Y = b.height;
                break;
            case TC:
                p.X = b.width / 2;
                p.Y = b.height;
                break;
            case BBL:
                p.X = 0;
                p.Y = 0;
                break;
            case BBR:
                p.X = b.width;
                p.Y = 0;
                break;
            case BBC:
                p.X = b.width / 2;
                p.Y = 0;
                break;
            case CL:
                p.X = 0;
                p.Y = (b.height - b.depth) / 2;
                break;
            case CR:
                p.X = b.width;
                p.Y = (b.height - b.depth) / 2;
                break;
            case CC:
                p.X = b.width / 2;
                p.Y = (b.height - b.depth) / 2;
                break;
            default:
                break;
        }

        return p;
    }

    public override void draw(Graphics g2, float x, float y)
    {
        drawDebug(g2, x, y);
        box.drawDebug(g2, x, y, true);
        y -= shiftY;
        x += shiftX - xmin;
        g2.rotate(-angle, x, y);
        box.draw(g2, x, y);
        box.drawDebug(g2, x, y, true);
        g2.rotate(angle, x, y);
    }

    public override int getLastFontId()
    {
        return box.getLastFontId();
    }
}
