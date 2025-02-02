/* UnderOverAtom.cs
 * =========================================================================
 * This file is originally part of the JMathTeX Library - http://jmathtex.sourceforge.net
 *
 * Copyright (C) 2004-2007 Universiteit Gent
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
 * An atom representing another atom with an atom above it (if not null) seperated
 * by a kern and in a smaller size depending on "overScriptSize" and/or an atom under
 * it (if not null) seperated by a kern and in a smaller size depending on "underScriptSize"
 */
public class UnderOverAtom : Atom
{
    // base, underscript and overscript
    private readonly Atom Base;
    private readonly Atom under;
    private readonly Atom over;

    // kern between base and under- and overscript
    private readonly float underSpace;
    private readonly float overSpace;

    // units for the kerns
    private readonly int underUnit; // NOPMD
    // TODO: seems never to be used?
    private readonly int overUnit;

    // whether the under- and overscript should be drawn in a smaller size
    private readonly bool underScriptSize;
    private readonly bool overScriptSize;

    public UnderOverAtom(Atom _base, Atom underOver, int underOverUnit,
                         float underOverSpace, bool underOverScriptSize, bool over)
    {
        // check if unit is valid
        SpaceAtom.CheckUnit(underOverUnit);
        // units valid
        this.Base = _base;

        if (over)
        {
            this.under = null;
            this.underSpace = 0.0f;
            this.underUnit = 0;
            this.underScriptSize = false;
            this.over = underOver;
            this.overUnit = underOverUnit;
            this.overSpace = underOverSpace;
            this.overScriptSize = underOverScriptSize;
        }
        else
        {
            this.under = underOver;
            this.underUnit = underOverUnit;
            this.underSpace = underOverSpace;
            this.underScriptSize = underOverScriptSize;
            this.overSpace = 0.0f;
            this.over = null;
            this.overUnit = 0;
            this.overScriptSize = false;
        }
    }

    public UnderOverAtom(Atom _base, Atom under, int underUnit, float underSpace,
                         bool underScriptSize, Atom over, int overUnit, float overSpace,
                         bool overScriptSize)
    {
        // check if units are valid
        SpaceAtom.CheckUnit(underUnit);
        SpaceAtom.CheckUnit(overUnit);

        // units valid
        this.Base = _base;
        this.under = under;
        this.underUnit = underUnit;
        this.underSpace = underSpace;
        this.underScriptSize = underScriptSize;
        this.over = over;
        this.overUnit = overUnit;
        this.overSpace = overSpace;
        this.overScriptSize = overScriptSize;
    }

    public override Box CreateBox(TeXEnvironment env)
    {
        // create boxes in right style and calculate maximum width
        Box b = (Base == null ? new StrutBox(0, 0, 0, 0) : Base.CreateBox(env));
        Box? o = null, u = null;
        float max = b.Width;
        if (over != null)
        {
            o = over.CreateBox(overScriptSize ? env.SubStyle : env);
            max = Math.Max(max, o.Width);
        }
        if (under != null)
        {
            u = under.CreateBox(underScriptSize ? env.SubStyle : env);
            max = Math.Max(max, u.Width);
        }

        // create vertical box
        var vBox = new VerticalBox();

        // last font used by the base (for Mspace atoms following)
        // last font used by the base (for Mspace atoms following)
        env.LastFontId = b.LastFontId;

        // overscript + space
        if (over != null)
        {
            vBox.Add(ChangeWidth(o, max));
            // unit will be valid (checked in constructor)
            vBox.Add(new SpaceAtom(overUnit, 0, overSpace, 0).CreateBox(env));
        }

        // base
        Box c = ChangeWidth(b, max);
        vBox.Add(c);

        // calculate future height of the vertical box (to make sure that the base
        // stays on the baseline!)
        float h = vBox.Height + vBox.Depth - c.Depth;

        // underscript + space
        if (under != null)
        {
            // unit will be valid (checked in constructor)
            vBox.Add(new SpaceAtom(overUnit, 0, underSpace, 0).CreateBox(env));
            vBox.Add(ChangeWidth(u, max));
        }

        // set height and depth
        // set height and depth
        vBox.Depth = vBox.Height + vBox.Depth - h;
        vBox.Height = h;
        return vBox;
    }

    private static Box? ChangeWidth(Box? b, float maxWidth) 
        => b != null && Math.Abs(maxWidth - b.Width) > TeXFormula.PREC
            ? new HorizontalBox(b, maxWidth, TeXConstants.ALIGN_CENTER)
            : b
        ;
}
