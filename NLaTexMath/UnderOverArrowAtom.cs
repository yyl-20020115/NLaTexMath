/* UnderOverArrowAtom.java
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
 * An atom representing an other atom with an extensible arrow or doublearrow over or under it.
 */
public class UnderOverArrowAtom : Atom {

    private Atom _base;
    private bool over, left = false, dble = false;

    public UnderOverArrowAtom(Atom _base, bool left, bool over) {
        this._base = _base;
        this.left = left;
        this.over = over;
    }

    public UnderOverArrowAtom(Atom _base, bool over) {
        this._base = _base;
        this.over = over;
        this.dble = true;
    }

    public override Box CreateBox(TeXEnvironment env) {
        Box b = _base != null ? _base.CreateBox(env) : new StrutBox(0, 0, 0, 0);
        float sep = new SpaceAtom(TeXConstants.UNIT_POINT, 1f, 0, 0).CreateBox(env).getWidth();
        Box arrow;

        if (dble) {
            arrow = XLeftRightArrowFactory.create(env, b.getWidth());
            sep = 4 * sep;
        } else {
            arrow = XLeftRightArrowFactory.create(left, env, b.getWidth());
            sep = -sep;
        }

        VerticalBox vb = new VerticalBox();
        if (over) {
            vb.Add(arrow);
            vb.Add(new HorizontalBox(b, arrow.getWidth(), TeXConstants.ALIGN_CENTER));
            float h = vb.getDepth() + vb.getHeight();
            vb.setDepth(b.getDepth());
            vb.setHeight(h - b.getDepth());
        } else {
            vb.Add(new HorizontalBox(b, arrow.getWidth(), TeXConstants.ALIGN_CENTER));
            vb.Add(new StrutBox(0, sep, 0, 0));
            vb.Add(arrow);
            float h = vb.getDepth() + vb.getHeight();
            vb.setDepth(h - b.getHeight());
            vb.setHeight(b.getHeight());
        }

        return vb;

    }
}
