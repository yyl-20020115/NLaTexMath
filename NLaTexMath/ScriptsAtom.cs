/* ScriptsAtom.java
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
 * An atom representing scripts to be attached to another atom.
 */
public class ScriptsAtom : Atom {

    // TeX constant: what's the use???
    private  static SpaceAtom SCRIPT_SPACE = new SpaceAtom(TeXConstants.UNIT_POINT, 0.5f, 0, 0);

    // base atom
    private  Atom _base;

    // subscript and superscript to be attached to the base (if not null)
    private  Atom subscript;
    private  Atom superscript;
    private int align = TeXConstants.ALIGN_LEFT;

    public ScriptsAtom(Atom _base, Atom sub, Atom sup) {
        this._base = _base;
        subscript = sub;
        superscript = sup;
    }

    public ScriptsAtom(Atom _base, Atom sub, Atom sup, bool left)
    : this(_base, sub, sup)
    {
        ;
        if (!left)
            align = TeXConstants.ALIGN_RIGHT;
    }

    public override Box CreateBox(TeXEnvironment env) {
        Box b = (_base == null ? new StrutBox(0, 0, 0, 0) : _base.CreateBox(env));
        Box deltaSymbol = new StrutBox(0, 0, 0, 0);
        if (subscript == null && superscript == null)
            return b;
        else {
            TeXFont tf = env.TeXFont;
            int style = env.Style;

            if (base.TypeLimits == TeXConstants.SCRIPT_LIMITS || (_base.TypeLimits == TeXConstants.SCRIPT_NORMAL && style == TeXConstants.STYLE_DISPLAY))
                return new UnderOverAtom(new UnderOverAtom(_base, subscript, TeXConstants.UNIT_POINT, 0.3f, true, false),
                                         superscript, TeXConstants.UNIT_POINT, 3.0f, true, true).CreateBox(env);

            HorizontalBox hor = new HorizontalBox(b);

            int lastFontId = b.LastFontId;
            // if no last font found (whitespace box), use default "mu font"
            if (lastFontId == TeXFont.NO_FONT)
                lastFontId = tf.getMuFontId();

            TeXEnvironment subStyle = env.SubStyle, supStyle = env.SupStyle;

            // set delta and preliminary shift-up and shift-down values
            float delta = 0, shiftUp, shiftDown;

            // TODO: use polymorphism?
            if (_base is AccentedAtom) { // special case :
                // accent. This positions superscripts better next to the accent!
                Box box = ((AccentedAtom)_base)._base.CreateBox(env.CrampStyle());
                shiftUp = box.Height - tf.getSupDrop(supStyle.Style);
                shiftDown = box.Depth + tf.getSubDrop(subStyle.Style);
            } else if (_base is SymbolAtom
                       && base.Type == TeXConstants.TYPE_BIG_OPERATOR) { // single big operator symbol
                Char c = tf.getChar(((SymbolAtom)_base).getName(), style);
                if (style < TeXConstants.STYLE_TEXT && tf.hasNextLarger(c)) // display
                    // style
                    c = tf.getNextLarger(c, style);
                Box x = new CharBox(c);

                x.
                Shift = -(x.Height + x.Depth) / 2
                           - env.TeXFont.getAxisHeight(env.Style);
                hor = new HorizontalBox(x);

                // include delta in width or not?
                delta = c.getItalic();
                deltaSymbol = new SpaceAtom(TeXConstants.MEDMUSKIP).CreateBox(env);
                if (delta > TeXFormula.PREC && subscript == null)
                    hor.Add(new StrutBox(delta, 0, 0, 0));

                shiftUp = hor.Height - tf.getSupDrop(supStyle.Style);
                shiftDown = hor.Depth + tf.getSubDrop(subStyle.Style);
            } else if (_base is CharSymbol) {
                shiftUp = shiftDown = 0;
                CharFont cf = ((CharSymbol)_base).GetCharFont(tf);
                if (!((CharSymbol)_base).IsMarkedAsTextSymbol() || !tf.hasSpace(cf.fontId)) {
                    delta = tf.getChar(cf, style).getItalic();
                }
                if (delta > TeXFormula.PREC && subscript == null) {
                    hor.Add(new StrutBox(delta, 0, 0, 0));
                    delta = 0;
                }
            } else {
                shiftUp = b.Height - tf.getSupDrop(supStyle.Style);
                shiftDown = b.Depth + tf.getSubDrop(subStyle.Style);
            }

            if (superscript == null) { // only subscript
                Box x = subscript.CreateBox(subStyle);
                // calculate and set shift amount
                x.                // calculate and set shift amount
                Shift = Math.Max(Math.Max(shiftDown, tf.getSub1(style)), x.Height - 4 * Math.Abs(tf.getXHeight(style, lastFontId)) / 5);
                hor.Add(x);
                hor.Add(deltaSymbol);

                return hor;
            } else {
                Box x = superscript.CreateBox(supStyle);
                float msiz = x.Width;
                if (subscript != null && align == TeXConstants.ALIGN_RIGHT) {
                    msiz = Math.Max(msiz, subscript.CreateBox(subStyle).Width);
                }

                HorizontalBox sup = new HorizontalBox(x, msiz, align);
                // Add scriptspace (constant value!)
                sup.Add(SCRIPT_SPACE.CreateBox(env));
                // adjust shift-up
                float p;
                if (style == TeXConstants.STYLE_DISPLAY)
                    p = tf.getSup1(style);
                else if (env.CrampStyle().Style == style)
                    p = tf.getSup3(style);
                else
                    p = tf.getSup2(style);
                shiftUp = Math.Max(Math.Max(shiftUp, p), x.Depth
                                   + Math.Abs(tf.getXHeight(style, lastFontId)) / 4);

                if (subscript == null) { // only superscript
                    sup.                    Shift = -shiftUp;
                    hor.Add(sup);
                } else { // both superscript and subscript
                    Box y = subscript.CreateBox(subStyle);
                    HorizontalBox sub = new HorizontalBox(y, msiz, align);
                    // Add scriptspace (constant value!)
                    sub.Add(SCRIPT_SPACE.CreateBox(env));
                    // adjust shift-down
                    shiftDown = Math.Max(shiftDown, tf.getSub2(style));
                    // position both sub- and superscript
                    float drt = tf.getDefaultRuleThickness(style);
                    float interSpace = shiftUp - x.Depth + shiftDown
                                       - y.Height; // space between sub- en
                    // superscript
                    if (interSpace < 4 * drt) { // too small
                        shiftUp += 4 * drt - interSpace;
                        // set bottom superscript at least 4/5 of X-height
                        // above
                        // baseline
                        float psi = 4 * Math.Abs(tf.getXHeight(style, lastFontId))
                                    / 5 - (shiftUp - x.Depth);

                        if (psi > 0) {
                            shiftUp += psi;
                            shiftDown -= psi;
                        }
                    }
                    // create total box

                    VerticalBox vBox = new VerticalBox();
                    sup.                    Shift = delta;
                    vBox.Add(sup);
                    // recalculate interspace
                    interSpace = shiftUp - x.Depth + shiftDown - y.Height;
                    vBox.Add(new StrutBox(0, interSpace, 0, 0));
                    vBox.Add(sub);
                    vBox.                    Height = shiftUp + x.Height;
                    vBox.                    Depth = shiftDown + y.Depth;
                    hor.Add(vBox);
                }
                hor.Add(deltaSymbol);

                return hor;
            }
        }
    }

    public int getLeftType() {
        return base.LeftType;
    }

    public int getRightType() {
        return base.RightType;
    }
}
