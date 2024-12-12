/* BigOperatorAtom.java
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

/* Modified by Calixte Denizet */

namespace NLaTexMath;

/**
 * An atom representing a "big operator" (or an atom that acts as one) together
 * with its limits.
 */
public class BigOperatorAtom : Atom
{

    // limits
    private Atom under, over;

    // atom representing a big operator
    protected Atom _base;

    // whether the "limits"-value should be taken into account
    // (otherwise the default rules will be applied)
    private bool limitsSet = false;

    // whether limits should be drawn over and under the _base (<-> as scripts)
    private bool limits = false;

    /**
     * Creates a new BigOperatorAtom from the given atoms.
     * The default rules the positioning of the limits will be applied.
     *
     * @param _base atom representing the big operator
     * @param under atom representing the under limit
     * @param over atom representing the over limit
     */
    public BigOperatorAtom(Atom _base, Atom under, Atom over)
    {
        this._base = _base;
        this.under = under;
        this.over = over;
        Type = TeXConstants.TYPE_BIG_OPERATOR;
    }

    /**
     * Creates a new BigOperatorAtom from the given atoms.
     * Limits will be drawn according to the "limits"-value
     *
     * @param _base atom representing the big operator
     * @param under atom representing the under limit
     * @param over atom representing the over limit
     * @param limits whether limits should be drawn over and under the _base (&lt;-&gt; as scripts)
     */
    public BigOperatorAtom(Atom _base, Atom under, Atom over, bool limits) : this(_base, under, over)
    {
        this.limits = limits;
        this.limitsSet = true;
    }

    public override Box CreateBox(TeXEnvironment env)
    {
        TeXFont tf = env.TeXFont;
        int style = env.Style;

        Box y;
        float delta;
        RowAtom bbase = null;
        Atom Base = _base;
        if (_base is TypedAtom)
        {
            Atom at = ((TypedAtom)_base).GetBase();
            if (at is RowAtom atom && atom.lookAtLastAtom && _base.TypeLimits != TeXConstants.SCRIPT_LIMITS)
            {
                _base = atom.GetLastAtom();
                bbase = atom;
            }
            else
                _base = at;
        }

        if ((limitsSet && !limits)
                || (!limitsSet && style >= TeXConstants.STYLE_TEXT)
                || (_base.TypeLimits == TeXConstants.SCRIPT_NOLIMITS)
                || (_base.TypeLimits == TeXConstants.SCRIPT_NORMAL && style >= TeXConstants.STYLE_TEXT))
        {
            // if explicitly set to not display as limits or if not set and style
            // is not display, then attach over and under as regular sub- en
            // superscript
            if (bbase != null)
            {
                bbase.Add(new ScriptsAtom(_base, under, over));
                Box b = bbase.CreateBox(env);
                bbase.GetLastAtom();
                bbase.Add(_base);
                _base = Base;
                return b;
            }
            return new ScriptsAtom(_base, under, over).CreateBox(env);
        }
        else
        {
            if (_base is SymbolAtom atom
                    && _base.Type == TeXConstants.TYPE_BIG_OPERATOR)
            { // single
                // bigop
                // symbol
                Char c = tf.GetChar(atom.Name, style);
                y = _base.CreateBox(env);

                // include delta in width
                delta = c.Italic;
            }
            else
            { // formula
                delta = 0;
                y = new HorizontalBox(_base == null ? new StrutBox(0, 0, 0, 0)
                                      : _base.CreateBox(env));
            }

            // limits
            Box x = null, z = null;
            if (over != null)
                x = over.CreateBox(env.SupStyle);
            if (under != null)
                z = under.CreateBox(env.SubStyle);

            // make boxes equally wide
            float maxWidth = Math.Max(Math.Max(x == null ? 0 : x.Width, y
                                               .Width), z == null ? 0 : z.Width);
            x = ChangeWidth(x, maxWidth);
            y = ChangeWidth(y, maxWidth);
            z = ChangeWidth(z, maxWidth);

            // build vertical box
            VerticalBox vBox = new VerticalBox();

            float bigop5 = tf.GetBigOpSpacing5(style), kern = 0;
            float xh = 0; // TODO: check why this is not used // NOPMD

            // over
            if (over != null)
            {
                vBox.Add(new StrutBox(0, bigop5, 0, 0));
                x.Shift = delta / 2;
                vBox.Add(x);
                kern = Math.Max(tf.GetBigOpSpacing1(style), tf
                                .GetBigOpSpacing3(style)
                                - x.Depth);
                vBox.Add(new StrutBox(0, kern, 0, 0));
                xh = vBox.Height + vBox.Depth;
            }

            // _base
            vBox.Add(y);

            // under
            if (under != null)
            {
                float k = Math.Max(tf.GetBigOpSpacing2(style), tf
                                   .GetBigOpSpacing4(style)
                                   - z.Height);
                vBox.Add(new StrutBox(0, k, 0, 0));
                z.Shift = -delta / 2;
                vBox.Add(z);
                vBox.Add(new StrutBox(0, bigop5, 0, 0));
            }

            // set height and depth vertical box and return it
            float h = y.Height, total = vBox.Height + vBox.Depth;
            if (x != null)
                h += bigop5 + kern + x.Height + x.Depth;
            vBox.Height = h;
            vBox.Depth = total - h;

            if (bbase != null)
            {
                var hb = new HorizontalBox(bbase.CreateBox(env));
                bbase.Add(_base);
                hb.Add(vBox);
                _base = Base;
                return hb;
            }

            return vBox;
        }
    }

    /*
     * Centers the given box in a new box that has the given width
     */
    private static Box ChangeWidth(Box b, float maxWidth)
        => b != null && Math.Abs(maxWidth - b.Width) > TeXFormula.PREC
            ? new HorizontalBox(b, maxWidth, TeXConstants.ALIGN_CENTER)
            : b;
}
