/* FencedAtom.cs
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
 * An atom representing a _base atom surrounded with delimiters that change their size
 * according to the height of the _base.
 */
public class FencedAtom : Atom
{

    // parameters used in the TeX algorithm
    private static readonly int DELIMITER_FACTOR = 901;

    private static readonly float DELIMITER_SHORTFALL = 5f;

    // _base atom
    private readonly Atom Base;

    // delimiters
    private readonly SymbolAtom? left;
    private readonly SymbolAtom? right;
    private readonly List<MiddleAtom> middle;

    /**
     * Creates a new FencedAtom from the given _base and delimiters
     *
     * @param _base the _base to be surrounded with delimiters
     * @param l the left delimiter
     * @param r the right delimiter
     */
    public FencedAtom(Atom Base, SymbolAtom l, SymbolAtom r)
        : this(Base, l, null, r)
    {
    }

    public FencedAtom(Atom Base, SymbolAtom l, List<MiddleAtom> m, SymbolAtom r)
    {
        if (Base == null)
            this.Base = new RowAtom(); // empty _base
        else
            this.Base = Base;
        if (l == null || l.Name != "normaldot")
        {
            left = l;
        }
        if (r == null || r.Name != "normaldot")
        {
            right = r;
        }
        middle = m;
    }

    public override int LeftType => TeXConstants.TYPE_INNER;

    public override int RightType => TeXConstants.TYPE_INNER;

    /**
     * Centers the given box with resprect to the given axis, by setting an appropriate
     * shift value.
     *
     * @param box
     *           box to be vertically centered with respect to the axis
     */
    private static void Center(Box box, float axis)
    {
        float h = box.Height, total = h + box.Depth;
        box.Shift = -(total / 2 - h) - axis;
    }

    public override Box CreateBox(TeXEnvironment env)
    {
        TeXFont tf = env.TeXFont;
        var content = Base.CreateBox(env);
        float shortfall = DELIMITER_SHORTFALL * SpaceAtom.GetFactor(TeXConstants.UNIT_POINT, env);
        float axis = tf.GetAxisHeight(env.Style);
        float delta = Math.Max(content.Height - axis, content.Depth + axis);
        float minHeight = Math.Max((delta / 500) * DELIMITER_FACTOR, 2 * delta - shortfall);

        // construct box
        var hBox = new HorizontalBox();

        if (middle != null)
        {
            for (int i = 0; i < middle.Count; i++)
            {
                MiddleAtom at = middle[(i)];
                if (at.Base is SymbolAtom atom)
                {
                    Box b = DelimiterFactory.Create(atom.Name, env, minHeight);
                    Center(b, axis);
                    at.Box = b;
                }
            }
            if (middle.Count != 0)
            {
                content = Base.CreateBox(env);
            }
        }

        // left delimiter
        if (left != null)
        {
            Box b = DelimiterFactory.Create(left.Name, env, minHeight);
            Center(b, axis);
            hBox.Add(b);
        }

        // glue between left delimiter and content (if not whitespace)
        if (Base is not SpaceAtom)
        {
            hBox.Add(Glue.Get(TeXConstants.TYPE_OPENING, Base.LeftType, env));
        }

        // Add content
        hBox.Add(content);

        // glue between right delimiter and content (if not whitespace)
        if (Base is not SpaceAtom)
        {
            hBox.Add(Glue.Get(Base.RightType, TeXConstants.TYPE_CLOSING, env));
        }

        // right delimiter
        if (right != null)
        {
            Box b = DelimiterFactory.Create(right.Name, env, minHeight);
            Center(b, axis);
            hBox.Add(b);
        }

        return hBox;
    }
}
