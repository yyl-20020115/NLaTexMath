/* MulticolumnAtom.cs
 * =========================================================================
 * This file is part of the JLaTeXMath Library - http://forge.scilab.org/jlatexmath
 *
 * Copyright (C) 2010 DENIZET Calixte
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
 * An atom used in array mode to write on several columns.
 */
public class MulticolumnAtom : Atom
{
    protected int n;
    protected int align;
    protected float w = 0;
    protected Atom cols;
    protected int beforeVlines;
    protected int afterVlines;
    protected int row, col;

    public MulticolumnAtom(int n, string align, Atom cols)
    {
        this.n = n >= 1 ? n : 1;
        this.cols = cols;
        this.align = ParseAlign(align);
    }

    public void SetWidth(float w) => this.w = w;

    public int Skipped => n;

    public bool HasRightVline => afterVlines != 0;

    public void SetRowColumn(int i, int j)
    {
        this.row = i;
        this.col = j;
    }

    public int Row => row;

    public int Col => col;

    private int ParseAlign(string str)
    {
        int pos = 0;
        int len = str.Length;
        int align = TeXConstants.ALIGN_CENTER;
        bool first = true;
        while (pos < len)
        {
            char c = str[(pos)];
            switch (c)
            {
                case 'l':
                    align = TeXConstants.ALIGN_LEFT;
                    first = false;
                    break;
                case 'r':
                    align = TeXConstants.ALIGN_RIGHT;
                    first = false;
                    break;
                case 'c':
                    align = TeXConstants.ALIGN_CENTER;
                    first = false;
                    break;
                case '|':
                    if (first)
                    {
                        beforeVlines = 1;
                    }
                    else
                    {
                        afterVlines = 1;
                    }
                    while (++pos < len)
                    {
                        c = str[(pos)];
                        if (c != '|')
                        {
                            pos--;
                            break;
                        }
                        else
                        {
                            if (first)
                            {
                                beforeVlines++;
                            }
                            else
                            {
                                afterVlines++;
                            }
                        }
                    }
                    break;
            }
            pos++;
        }
        return align;
    }

    public override Box CreateBox(TeXEnvironment env)
    {
        var b = w == 0 ? cols.CreateBox(env) : new HorizontalBox(cols.CreateBox(env), w, align);
        b.Type = TeXConstants.TYPE_MULTICOLUMN;
        return b;
    }
}
