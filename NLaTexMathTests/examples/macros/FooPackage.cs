/* FooPackage.cs
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

using NLaTexMath;
using System.Drawing;

namespace NLaTexMathTests.Examples.Macros;

public class FooPackage
{

    /*
     * The macro fooA is equivalent to \newcommand{\fooA}[2]{\frac{\textcolor{red}{#2}}{#1}}
     */
    public Atom? FooA_macro(TeXParser tp, string[] args) 
        => new TeXFormula("\\frac{\\textcolor{red}{" + args[2] + "}}{" + args[1] + "}").root;

    public Atom? FooB_macro(TeXParser tp, string[] args)
    {
        float f = float.TryParse(args[1],out var v)?v:0;
        return new MyAtom(f);
    }

    public Atom? FooC_macro(TeXParser tp, string[] args)
    {
        float f = float.TryParse(args[1], out var v) ? v : 0;
        return new MyAtom(f, args[2].Length != 0);
    }

    public Atom? FooD_macro(TeXParser tp, string[] args)
    {
        float f = float.TryParse(args[1], out var v) ? v : 0;
        return new MyAtom(f, args[2].Length == 0);
    }

    public class MyAtom : Atom
    {

        public float f;
        public bool filled = false;

        public MyAtom(float f) => this.f = f;

        public MyAtom(float f, bool filled)
        {
            this.f = f;
            this.filled = filled;
        }

        public override Box CreateBox(TeXEnvironment env) 
            => new MyBox((int)f, new SpaceAtom(TeXConstants.UNIT_POINT, f, 0, 0).CreateBox(env).Width, filled);
    }

    public class MyBox : Box
    {

        public bool filled;
        public int r;

        public MyBox(int r, float f, bool filled)
        {
            this.r = r;
            this.filled = filled;
            this.width = f;
            this.height = f / 2;
            this.depth = f / 2;
        }

        public override void Draw(Graphics g, float x, float y)
        {
            var t = g.Transform.Clone();
            g.Transform.Translate(x, y - height);
            g.Transform.Scale(Math.Abs(1.0f / this.scaleX), Math.Abs(1.0f / this.scaleY));

            using var brush = new SolidBrush(this.foreground);
            if (filled)
            {
                g.FillEllipse(brush, new RectangleF(0, 0, r, r));
            }
            else
            {
                using var pen = new Pen(brush);
                g.DrawEllipse(pen, new RectangleF(0, 0, r, r));
            }
            g.Transform = t;
        }

        public override int LastFontId => 0;
    }
}
