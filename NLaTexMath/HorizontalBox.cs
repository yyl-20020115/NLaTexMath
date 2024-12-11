/* HorizontalBox.java
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

using System.Drawing;

namespace NLaTexMath;

/**
 * A box composed of a horizontal row of child boxes.
 */
public class HorizontalBox : Box {

    private float curPos = 0; // NOPMD
    public List<int> breakPositions;

    public HorizontalBox(Box b, float w, int alignment) {
        if (w != float.PositiveInfinity) {
            float rest = w - b.getWidth();
            if (rest > 0) {
                if (alignment == TeXConstants.ALIGN_CENTER || alignment == TeXConstants.ALIGN_NONE) {
                    StrutBox s = new StrutBox(rest / 2, 0, 0, 0);
                    Add(s);
                    Add(b);
                    Add(s);
                } else if (alignment == TeXConstants.ALIGN_LEFT) {
                    Add(b);
                    Add(new StrutBox(rest, 0, 0, 0));
                } else if (alignment == TeXConstants.ALIGN_RIGHT) {
                    Add(new StrutBox(rest, 0, 0, 0));
                    Add(b);
                } else {
                    Add(b);
                }
            } else {
                Add(b);
            }
        } else {
            Add(b);
        }
    }

    public HorizontalBox(Box b) {
        Add(b);
    }

    public HorizontalBox() {
        // basic horizontal box
    }

    public HorizontalBox(Color fg, Color bg): base(fg, bg)
    {
        ;
    }

    public HorizontalBox cloneBox() {
        HorizontalBox b = new HorizontalBox(foreground, background);
        b.shift = shift;

        return b;
    }

    public override void draw(Graphics g2, float x, float y) {
        startDraw(g2, x, y);
        float xPos = x;
        foreach (Box box in children) {
            /*int i = children.IndexOf(box);
              if (breakPositions != null && breakPositions.IndexOf(i) != -1) {
              box.markForDEBUG = java.awt.Color.BLUE;
              }*/

            box.draw(g2, xPos, y + box.shift);
            xPos += box.getWidth();
        }
        endDraw(g2);
    }

    public  void Add(Box b) {
        recalculate(b);
        base.Add(b);
    }

    public  void Add(int pos, Box b) {
        recalculate(b);
        base.Add(pos, b);
    }

    private void recalculate(Box b) {
        // Commented for ticket 764
        // \left(\!\!\!\begin{array}{c}n\\\\r\end{array}\!\!\!\right)+123
        //curPos += b.getWidth();
        //width = Math.Max(width, curPos);
        width += b.getWidth();
        height = Math.Max((children.Count == 0 ? float.NegativeInfinity : height), b.height - b.shift);
        depth = Math.Max((children.Count == 0 ? float.NegativeInfinity : depth), b.depth + b.shift);
    }

    public override int getLastFontId() {
        // iterate from the last child box to the first untill a font id is found
        // that's not equal to NO_FONT
        int fontId = TeXFont.NO_FONT;
        //for (ListIterator<Box> it = children.listIterator(children.Count); fontId == TeXFont.NO_FONT && it.hasPrevious();)
        //    fontId = ((Box) it.previous()).getLastFontId();
        for(int i = children.Count - 1; i >= 0; i--)
        {
            fontId = children[i].getLastFontId();
            if (fontId == TeXFont.NO_FONT)
                break;
        }
        return fontId;
    }

    public void addBreakPosition(int pos) {
        breakPositions ??= [];
        breakPositions.Add(pos);
    }

    protected HorizontalBox[] split(int position) {
        return split(position, 1);
    }

    protected HorizontalBox[] splitRemove(int position) {
        return split(position, 2);
    }

    private HorizontalBox[] split(int position, int shift) {
        HorizontalBox hb1 = cloneBox();
        HorizontalBox hb2 = cloneBox();
        for (int i = 0; i <= position; i++) {
            hb1.Add(children[(i)]);
        }

        for (int i = position + shift; i < children.Count; i++) {
            hb2.Add(children[(i)]);
        }

        if (breakPositions != null) {
            for (int i = 0; i < breakPositions.Count; i++) {
                if (breakPositions[(i)] > position + 1) {
                    hb2.addBreakPosition(breakPositions[(i)] - position - 1);
                }
            }
        }

        return [hb1, hb2];
    }
}
