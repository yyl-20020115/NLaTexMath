/* TStrokeAtom.java
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
 * An atom with a stroked T
 */
public class TStrokeAtom(bool upper) : Atom
{

    private bool upper = upper;

    public override Box CreateBox(TeXEnvironment env)
    {
        Char ch = env.TeXFont.GetChar("bar", env.Style);
        float italic = ch.Italic;
        var T = new CharBox(env.TeXFont.GetChar(upper ? 'T' : 't', "mathnormal", env.Style));
        var B = new CharBox(ch);
        Box y;
        if (Math.Abs(italic) > TeXFormula.PREC)
        {
            y = new HorizontalBox(new StrutBox(-italic, 0, 0, 0));
            y.Add(B);
        }
        else
            y = B;
        var b = new HorizontalBox(y, T.Width, TeXConstants.ALIGN_CENTER);
        var vb = new VerticalBox();
        vb.Add(T);
        vb.Add(new StrutBox(0, -0.5f * T.Height, 0, 0));
        vb.Add(b);
        return vb;
    }
}
/*if (upper)
    hb.Add(new SpaceAtom(TeXConstants.UNIT_EM, -0.7f, 0, 0).createBox(env));
else
    hb.Add(new SpaceAtom(TeXConstants.UNIT_EM, -0.3f, 0, 0).createBox(env));
    hb.Add(A);
return hb;
}

public override Box createBox(TeXEnvironment env) {
Box b = base.createBox(env);
VerticalBox vb = new VerticalBox();
vb.Add(b);
Char ch = env.getTeXFont().getChar("ogonek", env.getStyle());
float italic = ch.getItalic();
float x = new SpaceAtom(TeXConstants.UNIT_MU, 1f, 0, 0).createBox(env).getWidth();
Box ogonek = new CharBox(ch);
Box y;
if (Math.Abs(italic) > TeXFormula.PREC) {
        y = new HorizontalBox(new StrutBox(-italic, 0, 0, 0));
        y.Add(ogonek);
    } else
        y = ogonek;

Box og = new HorizontalBox(y, b.getWidth(), TeXConstants.ALIGN_RIGHT);
vb.Add(new StrutBox(0, -ogonek.getHeight(), 0, 0));
vb.Add(og);
float f = vb.getHeight() + vb.getDepth();
vb.setHeight(b.getHeight());
vb.setDepth(f - b.getHeight());
return vb;
}
}*/
