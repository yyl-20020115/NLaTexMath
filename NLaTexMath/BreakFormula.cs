/* BreakFormula.cs
 * =========================================================================
 * This file is part of the JLaTeXMath Library - http://forge.scilab.org/jlatexmath
 *
 * Copyright (C) 2011 DENIZET Calixte
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


public class BreakFormula
{
    public static Box Split(Box box, float width, float interline)
        => box is HorizontalBox hbox ? Split(hbox, width, interline) : box is VerticalBox vbox ? Split(vbox, width, interline) : box;

    public static Box Split(HorizontalBox hbox, float width, float interline)
    {
        var vbox = new VerticalBox();
        HorizontalBox first;
        HorizontalBox second = null;
        var positions = new Stack<Position>();
        float w = -1;
        while (hbox.Width > width && (w = CanBreak(positions, hbox, width)) != hbox.Width)
        {
            Position pos = positions.Pop();
            HorizontalBox[] hboxes = pos.hbox.Split(pos.index - 1);
            first = hboxes[0];
            second = hboxes[1];
            while (positions.Count != 0)
            {
                pos = positions.Pop();
                hboxes = pos.hbox.SplitRemove(pos.index);
                hboxes[0].Add(first);
                hboxes[1].Add(0, second);
                first = hboxes[0];
                second = hboxes[1];
            }
            vbox.Add(first, interline);
            hbox = second;
        }

        if (second != null)
        {
            vbox.Add(second, interline);
            return vbox;
        }

        return hbox;
    }

    private static Box Split(VerticalBox vbox, float width, float interline)
    {
        var newBox = new VerticalBox();
        foreach (var box in vbox.Children)
        {
            newBox.Add(Split(box, width, interline));
        }

        return newBox;
    }

    private static float CanBreak(Stack<Position> stack, HorizontalBox hbox, float width)
    {
        List<Box> children = hbox.Children;
        float[] cumWidth = new float[children.Count + 1];
        cumWidth[0] = 0;
        for (int i = 0; i < children.Count; i++)
        {
            Box box = children[i];
            cumWidth[i + 1] = cumWidth[i] + box.Width;
            if (cumWidth[i + 1] > width)
            {
                int pos = GetBreakPosition(hbox, i);
                if (box is HorizontalBox _hbox)
                {
                    Stack<Position> newStack = new();
                    float w = CanBreak(newStack, _hbox, width - cumWidth[i]);
                    if (w != box.Width && (cumWidth[i] + w <= width || pos == -1))
                    {
                        stack.Push(new Position(i - 1, hbox));
                        //stack.addAll(newStack);
                        foreach (var s in newStack)
                        {
                            stack.Push(s);
                        }
                        return cumWidth[i] + w;
                    }
                }

                if (pos != -1)
                {
                    stack.Push(new Position(pos, hbox));
                    return cumWidth[pos];
                }
            }
        }

        return hbox.Width;
    }

    private static int GetBreakPosition(HorizontalBox hb, int i)
    {
        if (hb.breakPositions == null)
        {
            return -1;
        }

        if (hb.breakPositions.Count == 1 && hb.breakPositions[0] <= i)
        {
            return hb.breakPositions[0];
        }

        int pos = 0;
        for (; pos < hb.breakPositions.Count; pos++)
        {
            if (hb.breakPositions[(pos)] > i)
            {
                if (pos == 0)
                {
                    return -1;
                }
                return hb.breakPositions[(pos - 1)];
            }
        }

        return hb.breakPositions[(pos - 1)];
    }

    public class Position(int index, HorizontalBox hbox)
    {
        public int index = index;
        public HorizontalBox hbox = hbox;
    }
}
