/* FramedBox.cs
 * =========================================================================
 * This file is part of the JLaTeXMath Library - http://forge.scilab.org/jlatexmath
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

using System.Drawing;

namespace NLaTexMath;


/**
 * A box representing a rotated box.
 */
public class FramedBox : Box
{
    public Box box;
    public float thickness;
    public float space;
    private readonly Color line;
    private readonly Color bg;

    public FramedBox(Box box, float thickness, float space)
    {
        this.box = box;
        this.width = box.Width + 2 * thickness + 2 * space;
        this.height = box.Height + thickness + space;
        this.depth = box.Depth + thickness + space;
        this.shift = box.Shift;
        this.thickness = thickness;
        this.space = space;
    }

    public FramedBox(Box box, float thickness, float space, Color line, Color bg) : this(box, thickness, space)
    {
        this.line = line;
        this.bg = bg;
    }

    public override void Draw(Graphics g, float x, float y)
    {
        //thickness
        using var brush = new SolidBrush(this.foreground);
        float th = thickness / 2;
        if (bg != Color.Empty)
        {
            g.FillRectangle(brush, new RectangleF(x + th, y - height + th, width - thickness, height + depth - thickness));
        }
        using var pen = new Pen(brush, th);
        if (line != Color.Empty)
        {
            g.DrawRectangle(pen, new RectangleF(x + th, y - height + th, width - thickness, height + depth - thickness));
        }
        else
        {
            g.DrawRectangle(pen, new RectangleF(x + th, y - height + th, width - thickness, height + depth - thickness));
        }
        //drawDebug(g2, x, y);
        box.Draw(g, x + space + thickness, y);
    }

    public override int LastFontId => box.LastFontId;
}
