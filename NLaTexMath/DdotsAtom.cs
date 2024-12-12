/* DdotsAtom.java
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
 * An atom representing ddots.
 */
public class DdotsAtom : Atom
{
    public DdotsAtom() { }

    public override Box CreateBox(TeXEnvironment env)
    {
        Box ldots = TeXFormula.Get("ldots").root.CreateBox(env);
        float w = ldots.Width;
        Box dot = SymbolAtom.Get("ldotp").CreateBox(env);
        var hb1 = new HorizontalBox(dot, w, TeXConstants.ALIGN_LEFT);
        var hb2 = new HorizontalBox(dot, w, TeXConstants.ALIGN_CENTER);
        var hb3 = new HorizontalBox(dot, w, TeXConstants.ALIGN_RIGHT);
        Box pt4 = new SpaceAtom(TeXConstants.UNIT_MU, 0, 4, 0).CreateBox(env);
        var vb = new VerticalBox();
        vb.Add(hb1);
        vb.Add(pt4);
        vb.Add(hb2);
        vb.Add(pt4);
        vb.Add(hb3);

        float h = vb.Height + vb.Depth;
        vb.Height = h;
        vb.Depth = 0;

        return vb;
    }
}
