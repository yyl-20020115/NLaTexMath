/* RowAtom.java
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

/* Modified by Calixte Denizet to handle the case where several ligatures occure*/

using NLaTexMath.dynamic;

namespace NLaTexMath;

/**
 * An atom representing a horizontal row of other atoms, to be seperated by glue.
 * It's also responsible for inserting kerns and ligatures.
 */
public class RowAtom : Atom, Row
{

    // atoms to be displayed horizontally next to eachother
    protected List<Atom> elements = [];

    public bool lookAtLastAtom = false;

    // previous atom (for nested Row atoms)
    private Dummy previousAtom = null;

    // set of atom types that make a previous bin atom change to ord
    private static BitSet binSet;

    // set of atom types that can possibly need a kern or, together with the
    // previous atom, be replaced by a ligature
    private static BitSet ligKernSet;

    static RowAtom()
    {
        // fill binSet
        binSet = new BitSet(16);
        binSet.Set(TeXConstants.TYPE_BINARY_OPERATOR);
        binSet.Set(TeXConstants.TYPE_BIG_OPERATOR);
        binSet.Set(TeXConstants.TYPE_RELATION);
        binSet.Set(TeXConstants.TYPE_OPENING);
        binSet.Set(TeXConstants.TYPE_PUNCTUATION);

        // fill ligKernSet
        ligKernSet = new BitSet(16);
        ligKernSet.Set(TeXConstants.TYPE_ORDINARY);
        ligKernSet.Set(TeXConstants.TYPE_BIG_OPERATOR);
        ligKernSet.Set(TeXConstants.TYPE_BINARY_OPERATOR);
        ligKernSet.Set(TeXConstants.TYPE_RELATION);
        ligKernSet.Set(TeXConstants.TYPE_OPENING);
        ligKernSet.Set(TeXConstants.TYPE_CLOSING);
        ligKernSet.Set(TeXConstants.TYPE_PUNCTUATION);
    }

    public RowAtom()
    {
        // empty
    }

    public RowAtom(Atom el)
    {
        if (el != null)
        {
            if (el is RowAtom atom)
                // no need to make an mrow the only element of an mrow
                elements.AddRange(atom.elements);
            else
                elements.Add(el);
        }
    }

    public Atom GetLastAtom()
    {
        if (elements.Count != 0)
        {
            var last = elements.Last();
            elements.Remove(last);
            return last;
        }

        return new SpaceAtom(TeXConstants.UNIT_POINT, 0.0f, 0.0f, 0.0f);
    }

    public void Add(Atom el)
    {
        if (el != null)
        {
            elements.Add(el);
        }
    }

    /**
     *
     * @param cur
     *           current atom being processed
     * @param prev
     *           previous atom
     */
    private void ChangeToOrd(Dummy cur, Dummy prev, Atom next)
    {
        int type = cur.LeftType;
        if (type == TeXConstants.TYPE_BINARY_OPERATOR && ((prev == null || binSet.Get(prev.RightType)) || next == null))
        {
            cur.Type = TeXConstants.TYPE_ORDINARY;
        }
        else if (next != null && cur.RightType == TeXConstants.TYPE_BINARY_OPERATOR)
        {
            int nextType = next.LeftType;
            if (nextType == TeXConstants.TYPE_RELATION || nextType == TeXConstants.TYPE_CLOSING || nextType == TeXConstants.TYPE_PUNCTUATION)
            {
                cur.Type = TeXConstants.TYPE_ORDINARY;
            }
        }
    }

    public override Box CreateBox(TeXEnvironment env)
    {
        TeXFont tf = env.TeXFont;
        HorizontalBox hBox = new HorizontalBox(env.Color, env.Background);
        int position = 0;
        env.Reset();

        // convert atoms to boxes and Add to the horizontal box
        for (ListIterator<Atom> it = elements.listIterator(); it.hasNext();)
        {
            Atom at = it.next();
            position++;

            bool markAdded = false;
            while (at is BreakMarkAtom)
            {
                if (!markAdded)
                {
                    markAdded = true;
                }
                if (it.hasNext())
                {
                    at = it.next();
                    position++;
                }
                else
                {
                    break;
                }
            }

            if (at is DynamicAtom && ((DynamicAtom)at).getInsertMode())
            {
                Atom a = ((DynamicAtom)at).getAtom();
                if (a is RowAtom atom1)
                {
                    elements.RemoveAt(position - 1);
                    elements.Add(position - 1, atom1.elements);
                    it = elements.listIterator(position - 1);
                    at = it.next();
                }
                else
                {
                    at = a;
                }
            }

            Dummy atom = new Dummy(at);

            // if necessary, change BIN type to ORD
            Atom nextAtom = null;
            if (it.hasNext())
            {
                nextAtom = it.next();
                it.previous();
            }
            ChangeToOrd(atom, previousAtom, nextAtom);

            // check for ligatures or kerning
            float kern = 0;
            // Calixte : I put a while to handle the case where there are
            // several ligatures as in ffi or ffl
            while (it.hasNext() && atom.RightType == TeXConstants.TYPE_ORDINARY && atom.IsCharSymbol)
            {
                Atom next = it.next();
                position++;
                if (next is CharSymbol && ligKernSet.Get(next.LeftType))
                {
                    atom.MarkAsTextSymbol();
                    CharFont l = atom.GetCharFont(tf), r = ((CharSymbol)next).GetCharFont(tf);
                    CharFont lig = tf.GetLigature(l, r);
                    if (lig == null)
                    {
                        kern = tf.GetKern(l, r, env.Style);
                        it.previous();
                        position--;
                        break; // iterator remains unchanged (no ligature!)
                    }
                    else
                    { // ligature
                        atom.ChangeAtom(new FixedCharAtom(lig)); // go on with the
                        // ligature
                    }
                }
                else
                {
                    it.previous();
                    position--;
                    break;
                }// iterator remains unchanged
            }

            // insert glue, unless it's the first element of the row
            // OR this element or the next is a Kern.
            if (it.previousIndex() != 0 && previousAtom != null && !previousAtom.IsKern && !atom.IsKern)
            {
                hBox.Add(Glue.Get(previousAtom.RightType, atom.LeftType, env));
            }

            // insert atom's box
            atom.SetPreviousAtom(previousAtom);
            Box b = atom.CreateBox(env);
            if (atom.IsCharInMathMode && b is CharBox)
            {
                // When we've a single char, we need to Add italic correction
                // As an example: (TVY) looks crappy...
                CharBox cb = (CharBox)b;
                cb.AddItalicCorrectionToWidth();
            }
            if (markAdded || (at is CharAtom && char.IsDigit(((CharAtom)at).Character)))
            {
                hBox.AddBreakPosition(hBox.Children.Count);
            }
            hBox.Add(b);

            // set last used fontId (for next atom)
            env.
            // set last used fontId (for next atom)
            LastFontId = b.LastFontId;

            // insert kern
            if (Math.Abs(kern) > TeXFormula.PREC)
            {
                hBox.Add(new StrutBox(kern, 0, 0, 0));
            }

            // kerns do not interfere with the normal glue-rules without kerns
            if (!atom.IsKern)
            {
                previousAtom = atom;
            }
        }
        // reset previousAtom
        previousAtom = null;

        return hBox;
    }

    public void SetPreviousAtom(Dummy prev)
    {
        previousAtom = prev;
    }

    public override int LeftType => elements.Count == 0 ? TeXConstants.TYPE_ORDINARY : elements[0].LeftType;

    public override int RightType => elements.Count == 0 ? TeXConstants.TYPE_ORDINARY : elements[^1].RightType;
}
