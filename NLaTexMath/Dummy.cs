/* Dummy.java
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
 * Used by RowAtom. The "textSymbol"-property and the type of an atom can change
 * (according to the TeX-algorithms used). Or this atom can be replaced by a ligature,
 * (if it was a CharAtom). But atoms cannot be changed, otherwise
 * different boxes could be made from the same TeXFormula, and that is not desired!
 * This "dummy atom" makes sure that changes to an atom (during the createBox-method of
 * a RowAtom) will be reset.
 */
public class Dummy(Atom a) : Atom
{
    private Atom el = a;
    private bool textSymbol = false;
    private int type = -1;

    /**
     * Returns the type of the atom
     * 
     * @return the type of the atom
     */
    /**
 * Changes the type of the atom
 *
 * @param t the new type
 */
    public override int Type { get => type; set => type = value; }

    /**
     * @return the changed type, or the old left type if it hasn't been changed
     */
    public override int LeftType => (type >= 0 ? type : el.LeftType);

    /**
     *
     * @return the changed type, or the old right type if it hasn't been changed
     */
    public override int RightType => (type >= 0 ? type : el.RightType);

    public bool IsCharSymbol => el is CharSymbol;

    public bool IsCharInMathMode => el is CharAtom atom && atom.IsMathMode;

    /**
     * This method will only be called if isCharSymbol returns true.
     */
    public CharFont GetCharFont(TeXFont tf) => ((CharSymbol)el).GetCharFont(tf);

    /**
     * Changes this atom into the given "ligature atom".
     *
     * @param a the ligature atom
     */
    public void ChangeAtom(FixedCharAtom a)
    {
        textSymbol = false;
        type = -1;
        el = a;
    }

    public override Box CreateBox(TeXEnvironment rs)
    {
        if (textSymbol)
            ((CharSymbol)el).MarkAsTextSymbol();
        var b = el.CreateBox(rs);
        if (textSymbol)
            ((CharSymbol)el).RemoveMark(); // atom remains unchanged!
        return b;
    }

    public void MarkAsTextSymbol()
    {
        textSymbol = true;
    }

    public bool IsKern => el is SpaceAtom;

    // only for Row-elements
    public void SetPreviousAtom(Dummy prev)
    {
        if (el is Row row)
        {
            row.SetPreviousAtom(prev);
        }
    }
}
