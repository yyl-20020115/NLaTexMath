/* NthRoot.java
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
 * An atom representing an nth-root construction.
 */
public class NthRoot : Atom {

    private static readonly string sqrtSymbol = "sqrt";

    private static readonly float FACTOR = 0.55f;

    // base atom to be put under the root sign
    private  Atom _base;

    // root atom to be put in the upper left corner above the root sign
    private  Atom root;

    public NthRoot(Atom _base, Atom root) {
        this._base = _base == null ? new EmptyAtom() : _base;
        this.root = root == null ? new EmptyAtom() : root;
    }

    public override Box CreateBox(TeXEnvironment env) {
        // first create a simple square root construction

        TeXFont tf = env.TeXFont;
        int style = env.Style;
        // calculate minimum clearance clr
        float clr, drt = tf.getDefaultRuleThickness(style);
        if (style < TeXConstants.STYLE_TEXT)
            clr = tf.getXHeight(style, tf.getChar(sqrtSymbol, style).getFontCode());
        else
            clr = drt;
        clr = drt + Math.Abs(clr) / 4 ;

        // cramped style for the formula under the root sign
        Box bs = _base.CreateBox(env.CrampStyle());
        HorizontalBox b = new HorizontalBox(bs);
        b.Add(new SpaceAtom(TeXConstants.UNIT_MU, 1, 0, 0).CreateBox(env.CrampStyle()));
        // create root sign
        float totalH = b.Height + b.Depth;
        Box rootSign = DelimiterFactory.create(sqrtSymbol, env, totalH + clr + drt);

        // Add half the excess to clr
        float delta = rootSign.Depth - (totalH + clr);
        clr += delta / 2;

        // create total box
        rootSign.
        // create total box
        Shift = -(b.Height + clr);
        OverBar ob = new OverBar(b, clr, rootSign.Height);
        ob.        Shift = -(b.Height + clr + drt);
        HorizontalBox squareRoot = new HorizontalBox(rootSign);
        squareRoot.Add(ob);

        if (root == null)
            // simple square root
            return squareRoot;
        else { // nthRoot, not a simple square root

            // create box from root
            Box r = root.CreateBox(env.RootStyle);

            // shift root up
            float bottomShift = FACTOR * (squareRoot.Height + squareRoot.Depth);
            r.            Shift = squareRoot.Depth - r.Depth - bottomShift;

            // negative kern
            Box negativeKern = new SpaceAtom(TeXConstants.UNIT_MU, -10f, 0, 0).CreateBox(env);

            // arrange both boxes together with the negative kern
            Box res = new HorizontalBox();
            float pos = r.Width + negativeKern.Width;
            if (pos < 0)
                res.Add(new StrutBox(-pos, 0, 0, 0));

            res.Add(r);
            res.Add(negativeKern);
            res.Add(squareRoot);
            return res;
        }
    }
}
