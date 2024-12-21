/* AccentedAtom.cs
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
 * An atom representing another atom with an accent symbol above it.
 */
public class AccentedAtom : Atom
{
    // accent symbol
    private readonly SymbolAtom accent;
    private readonly bool acc = false;
    private readonly bool changeSize = true;

    // _base atom
    public readonly Atom Base;
    public readonly Atom Underbase;

    public AccentedAtom(Atom _base, Atom accent)
    {
        this.Base = _base;
        this.Underbase = _base is AccentedAtom atom ? atom.Underbase : _base;

        if (accent is not SymbolAtom s)
            throw new InvalidSymbolTypeException("Invalid accent");

        this.accent = s;
        this.acc = true;
    }

    public AccentedAtom(Atom _base, Atom accent, bool changeSize) 
        : this(_base, accent) => this.changeSize = changeSize;

    /**
     * Creates an AccentedAtom from a _base atom and an accent symbol defined by its name
     *
     * @param _base _base atom
     * @param accentName name of the accent symbol to be put over the _base atom
     * @throws InvalidSymbolTypeException if the symbol is not defined as an accent ('acc')
     * @throws SymbolNotFoundException if there's no symbol defined with the given name
     */
    public AccentedAtom(Atom _base, string accentName)
    {
        accent = SymbolAtom.Get(accentName);
        if (accent.Type == TeXConstants.TYPE_ACCENT)
        {
            this.Base = _base;
            Underbase = _base is AccentedAtom atom ? atom.Underbase : _base;
        }
        else
            throw new InvalidSymbolTypeException($"The symbol with the name '{accentName}' is not defined as an accent ({TeXSymbolParser.TYPE_ATTR}='acc') in '{TeXSymbolParser.RESOURCE_NAME}'!");
    }

    /**
     * Creates an AccentedAtom from a base atom and an accent symbol defined as a TeXFormula.
     * This is used for parsing MathML.
     *
     * @param base base atom
     * @param acc TeXFormula representing an accent (SymbolAtom)
     * @throws InvalidTeXFormulaException if the given TeXFormula does not represent a
     * 			single SymbolAtom (type "TeXConstants.TYPE_ACCENT")
     * @throws InvalidSymbolTypeException if the symbol is not defined as an accent ('acc')
     */
    public AccentedAtom(Atom _base, TeXFormula acc)
    {
        if (acc == null)
            throw new InvalidTeXFormulaException(
                "The accent TeXFormula can't be null!");
        else
        {
            Atom root = acc.root;
            if (root is SymbolAtom atom)
            {
                accent = atom;
                if (accent.Type == TeXConstants.TYPE_ACCENT)
                    this.Base = _base;
                else
                    throw new InvalidSymbolTypeException(
                        "The accent TeXFormula represents a single symbol with the name '"
                        + accent.Name
                        + "', but this symbol is not defined as an accent ("
                        + TeXSymbolParser.TYPE_ATTR + "='acc') in '"
                        + TeXSymbolParser.RESOURCE_NAME + "'!");
            }
            else
                throw new InvalidTeXFormulaException(
                    "The accent TeXFormula does not represent a single symbol!");
        }
    }

    public override Box CreateBox(TeXEnvironment env)
    {
        TeXFont tf = env.TeXFont;
        int style = env.Style;

        // set _base in cramped style
        Box b = Base == null ? new StrutBox(0, 0, 0, 0) : Base.CreateBox(env.CrampStyle());

        float u = b.Width;
        float s = 0;
        if (Underbase is CharSymbol symbol)
            s = tf.GetSkew(symbol.GetCharFont(tf), style);

        // retrieve best Char from the accent symbol
        Char ch = tf.GetChar(accent.Name, style);
        while (tf.HasNextLarger(ch))
        {
            Char larger = tf.GetNextLarger(ch, style);
            if (larger.Width <= u)
                ch = larger;
            else
                break;
        }

        // calculate delta
        float ec = -SpaceAtom.GetFactor(TeXConstants.UNIT_MU, env);
        float delta = acc ? ec : Math.Min(b.Height, tf.GetXHeight(style, ch.FontCode));

        // create vertical box
        VerticalBox vBox = new();

        // accent
        Box y;
        float italic = ch.Italic;
        Box cb = new CharBox(ch);
        if (acc)
            cb = accent.CreateBox(changeSize ? env.SubStyle : env);

        if (Math.Abs(italic) > TeXFormula.PREC)
        {
            y = new HorizontalBox(new StrutBox(-italic, 0, 0, 0));
            y.Add(cb);
        }
        else
            y = cb;

        // if diff > 0, center accent, otherwise center _base
        float diff = (u - y.Width) / 2;
        y.Shift = s + (diff > 0 ? diff : 0);
        if (diff < 0)
            b = new HorizontalBox(b, y.Width, TeXConstants.ALIGN_CENTER);
        vBox.Add(y);

        // kern
        vBox.Add(new StrutBox(0, changeSize ? -delta : -b.Height, 0, 0));
        // _base
        vBox.Add(b);

        // set height and depth vertical box
        float total = vBox.Height + vBox.Depth, d = b.Depth;
        vBox.Depth = d;
        vBox.Height = total - d;

        if (diff < 0)
        {
            var hb = new HorizontalBox(new StrutBox(diff, 0, 0, 0));
            hb.Add(vBox);
            hb.Width = u;
            return hb;
        }

        return vBox;
    }
}
