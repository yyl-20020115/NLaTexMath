/* XArrowAtom.java
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

namespace NLaTexMath;

/**
 * An atom representing an extensible left or right arrow to handle xleftarrow and xrightarrow commands in LaTeX.
 */
public class XArrowAtom(Atom over, Atom under, bool left) : Atom
{
    private readonly Atom over = over;
    private readonly Atom under = under;
    private readonly bool left = left;

    public override Box CreateBox(TeXEnvironment env)
    {
        Box O = over != null ? over.CreateBox(env.SupStyle) : new StrutBox(0, 0, 0, 0);
        Box U = under != null ? under.CreateBox(env.SubStyle) : new StrutBox(0, 0, 0, 0);
        Box oside = new SpaceAtom(TeXConstants.UNIT_EM, 1.5f, 0, 0).CreateBox(env.SupStyle);
        Box uside = new SpaceAtom(TeXConstants.UNIT_EM, 1.5f, 0, 0).CreateBox(env.SubStyle);
        Box sep = new SpaceAtom(TeXConstants.UNIT_MU, 0, 2f, 0).CreateBox(env);
        float width = Math.Max(O.Width + 2 * oside.Width, U.Width + 2 * uside.Width);
        Box arrow = XLeftRightArrowFactory.Create(left, env, width);

        Box ohb = new HorizontalBox(O, width, TeXConstants.ALIGN_CENTER);
        Box uhb = new HorizontalBox(U, width, TeXConstants.ALIGN_CENTER);

        var vb = new VerticalBox();
        vb.Add(ohb);
        vb.Add(sep);
        vb.Add(arrow);
        vb.Add(sep);
        vb.Add(uhb);

        float h = vb.Height + vb.Depth;
        float d = sep.Height + sep.Depth + uhb.Height + uhb.Depth;
        vb.Depth = d;
        vb.Height = h - d;

        var hb = new HorizontalBox(vb, vb.Width + 2 * sep.Height, TeXConstants.ALIGN_CENTER);
        return hb;
    }
}
