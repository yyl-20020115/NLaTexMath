/* MatrixAtom.java
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

using System.Text;

/**
 * A box representing a matrix.
 */
public class MatrixAtom : Atom
{

    public static SpaceAtom hsep = new (TeXConstants.UNIT_EM, 1f, 0.0f, 0.0f);
    public static SpaceAtom semihsep = new (TeXConstants.UNIT_EM, 0.5f, 0.0f, 0.0f);
    public static SpaceAtom vsep_in = new (TeXConstants.UNIT_EX, 0.0f, 1f, 0.0f);
    public static SpaceAtom vsep_ext_top = new (TeXConstants.UNIT_EX, 0.0f, 0.4f, 0.0f);
    public static SpaceAtom vsep_ext_bot = new (TeXConstants.UNIT_EX, 0.0f, 0.4f, 0.0f);

    public const int ARRAY = 0;
    public const int MATRIX = 1;
    public const int ALIGN = 2;
    public const int ALIGNAT = 3;
    public const int FLALIGN = 4;
    public const int SMALLMATRIX = 5;
    public const int ALIGNED = 6;
    public const int ALIGNEDAT = 7;

    private static readonly Box nullBox = new StrutBox(0, 0, 0, 0);

    private ArrayOfAtoms matrix;
    private int[] position;
    private Dictionary<int, VlineAtom> vlines = [];
    private int type;
    private bool isPartial;
    private bool spaceAround;

    private static SpaceAtom align = new (TeXConstants.MEDMUSKIP);

    /**
     * Creates an empty matrix
     *
     */
    public MatrixAtom(bool isPartial, ArrayOfAtoms array, string options, bool spaceAround)
    {
        this.isPartial = isPartial;
        this.matrix = array;
        this.type = ARRAY;
        this.spaceAround = spaceAround;
        ParsePositions(new StringBuilder(options));
    }

    /**
     * Creates an empty matrix
     *
     */
    public MatrixAtom(bool isPartial, ArrayOfAtoms array, string options) : this(isPartial, array, options, false)
    {
    }

    /**
     * Creates an empty matrix
     *
     */
    public MatrixAtom(ArrayOfAtoms array, string options) : this(false, array, options)
    {
    }

    public MatrixAtom(bool isPartial, ArrayOfAtoms array, int type) : this(isPartial, array, type, false)
    {
    }

    public MatrixAtom(bool isPartial, ArrayOfAtoms array, int type, bool spaceAround)
    {
        this.isPartial = isPartial;
        this.matrix = array;
        this.type = type;
        this.spaceAround = spaceAround;

        if (type != MATRIX && type != SMALLMATRIX)
        {
            position = new int[matrix.col];
            for (int i = 0; i < matrix.col; i += 2)
            {
                position[i] = TeXConstants.ALIGN_RIGHT;
                if (i + 1 < matrix.col)
                {
                    position[i + 1] = TeXConstants.ALIGN_LEFT;
                }
            }
        }
        else
        {
            position = new int[matrix.col];
            for (int i = 0; i < matrix.col; i++)
            {
                position[i] = TeXConstants.ALIGN_CENTER;
            }
        }
    }

    public MatrixAtom(bool isPartial, ArrayOfAtoms array, int type, int alignment)
        : this(isPartial, array, type, alignment, true)
    {
    }

    public MatrixAtom(bool isPartial, ArrayOfAtoms array, int type, int alignment, bool spaceAround)
    {
        this.isPartial = isPartial;
        this.matrix = array;
        this.type = type;
        this.spaceAround = spaceAround;

        position = new int[matrix.col];
        for (int i = 0; i < matrix.col; i++)
        {
            position[i] = alignment;
        }
    }

    public MatrixAtom(ArrayOfAtoms array, int type)
        : this(false, array, type)
    {
    }

    private void ParsePositions(StringBuilder opt)
    {
        int len = opt.Length;
        int pos = 0;
        char ch;
        TeXFormula tf;
        TeXParser tp;
        List<int> lposition = new();
        while (pos < len)
        {
            ch = opt[pos];
            switch (ch)
            {
                case 'l':
                    lposition.Add(TeXConstants.ALIGN_LEFT);
                    break;
                case 'r':
                    lposition.Add(TeXConstants.ALIGN_RIGHT);
                    break;
                case 'c':
                    lposition.Add(TeXConstants.ALIGN_CENTER);
                    break;
                case '|':
                    int nb = 1;
                    while (++pos < len)
                    {
                        ch = opt[pos];
                        if (ch != '|')
                        {
                            pos--;
                            break;
                        }
                        else
                        {
                            nb++;
                        }
                    }
                    vlines.Add(lposition.Count, new VlineAtom(nb));
                    break;
                case '@':
                    pos++;
                    tf = new TeXFormula();
                    tp = new TeXParser(isPartial, opt.ToString()[pos..], tf, false);
                    Atom at = tp.GetArgument();
                    matrix.col++;
                    for (int j = 0; j < matrix.row; j++)
                    {
                        matrix.array[j].Insert(lposition.Count, at);
                    }

                    lposition.Add(TeXConstants.ALIGN_NONE);
                    pos += tp.GetPos();
                    pos--;
                    break;
                case '*':
                    pos++;
                    tf = new TeXFormula();
                    tp = new TeXParser(isPartial, opt.ToString()[pos..], tf, false);
                    string[] args = tp.getOptsArgs(2, 0);
                    pos += tp.GetPos();
                    int nrep = int.TryParse(args[1], out var v) ? v : 0;
                    string str = "";
                    for (int j = 0; j < nrep; j++)
                    {
                        str += args[2];
                    }
                    opt.Insert(pos, str);
                    len = opt.Length;
                    pos--;
                    break;
                case ' ':
                case '\t':
                    break;
                default:
                    lposition.Add(TeXConstants.ALIGN_CENTER);
                    break;
            }
            pos++;
        }

        for (int j = lposition.Count; j < matrix.col; j++)
        {
            lposition.Add(TeXConstants.ALIGN_CENTER);
        }

        if (lposition.Count != 0)
        {
            int[] tab = lposition.ToArray();
            position = new int[tab.Length];
            for (int i = 0; i < tab.Length; i++)
            {
                position[i] = tab[i];
            }
        }
        else
        {
            position = [TeXConstants.ALIGN_CENTER];
        }
    }

    public Box[] GetColumnSep(TeXEnvironment env, float width)
    {
        int col = matrix.col;
        Box[] arr = new Box[col + 1];
        Box Align, AlignSep, Hsep;
        float h, w = env.Textwidth;
        int i;

        if (type == ALIGNED || type == ALIGNEDAT)
        {
            w = float.PositiveInfinity;
        }

        switch (type)
        {
            case ARRAY:
                //Array : hsep_col/2 elem hsep_col elem hsep_col ... hsep_col elem hsep_col/2
                i = 1;
                if (position[0] == TeXConstants.ALIGN_NONE)
                {
                    arr[1] = new StrutBox(0.0f, 0.0f, 0.0f, 0.0f);
                    i = 2;
                }
                if (spaceAround)
                {
                    arr[0] = semihsep.CreateBox(env);
                }
                else
                {
                    arr[0] = new StrutBox(0.0f, 0.0f, 0.0f, 0.0f);
                }
                arr[col] = arr[0];
                Hsep = hsep.CreateBox(env);
                for (; i < col; i++)
                {
                    if (position[i] == TeXConstants.ALIGN_NONE)
                    {
                        arr[i] = new StrutBox(0.0f, 0.0f, 0.0f, 0.0f);
                        arr[i + 1] = arr[i];
                        i++;
                    }
                    else
                    {
                        arr[i] = Hsep;
                    }
                }

                return arr;
            case MATRIX:
            case SMALLMATRIX:
                //Simple matrix : (hsep_col/2 or 0) elem hsep_col elem hsep_col ... hsep_col elem (hsep_col/2 or 0)
                arr[0] = nullBox;
                arr[col] = arr[0];
                Hsep = hsep.CreateBox(env);
                for (i = 1; i < col; i++)
                {
                    arr[i] = Hsep;
                }

                return arr;
            case ALIGNED:
            case ALIGN:
                //Align env. : hsep=(textwidth-matWidth)/(2n+1) and hsep eq_lft \medskip el_rgt hsep ... hsep elem hsep
                Align = align.CreateBox(env);
                if (w != float.PositiveInfinity)
                {
                    h = Math.Max((w - width - (col / 2) * Align.Width) / (float)Math.Floor((col + 3.0) / 2), 0);
                    AlignSep = new StrutBox(h, 0.0f, 0.0f, 0.0f);
                }
                else
                {
                    AlignSep = hsep.CreateBox(env);
                }

                arr[col] = AlignSep;
                for (i = 0; i < col; i++)
                {
                    if (i % 2 == 0)
                    {
                        arr[i] = AlignSep;
                    }
                    else
                    {
                        arr[i] = Align;
                    }
                }

                break;
            case ALIGNEDAT:
            case ALIGNAT:
                //Alignat env. : hsep=(textwidth-matWidth)/2 and hsep elem ... elem hsep
                if (w != float.PositiveInfinity)
                {
                    h = Math.Max((w - width) / 2, 0);
                }
                else
                {
                    h = 0;
                }

                Align = align.CreateBox(env);
                Box empty = nullBox;
                arr[0] = new StrutBox(h, 0.0f, 0.0f, 0.0f);
                arr[col] = arr[0];
                for (i = 1; i < col; i++)
                {
                    if (i % 2 == 0)
                    {
                        arr[i] = empty;
                    }
                    else
                    {
                        arr[i] = Align;
                    }
                }

                break;
            case FLALIGN:
                //flalign env. : hsep=(textwidth-matWidth)/(2n+1) and hsep eq_lft \medskip el_rgt hsep ... hsep elem hsep
                Align = align.CreateBox(env);
                if (w != float.PositiveInfinity)
                {
                    h = Math.Max((w - width - (col / 2) * Align.Width) / (float)Math.Floor((col - 1) / 2.0), 0);
                    AlignSep = new StrutBox(h, 0.0f, 0.0f, 0.0f);
                }
                else
                {
                    AlignSep = hsep.CreateBox(env);
                }

                arr[0] = nullBox;
                arr[col] = arr[0];
                for (i = 1; i < col; i++)
                {
                    if (i % 2 == 0)
                    {
                        arr[i] = AlignSep;
                    }
                    else
                    {
                        arr[i] = Align;
                    }
                }

                break;
        }

        if (w == float.PositiveInfinity)
        {
            arr[0] = nullBox;
            arr[col] = arr[0];
        }

        return arr;

    }

    public override Box CreateBox(TeXEnvironment env)
    {
        int row = matrix.row;
        int col = matrix.col;
        Box[,] boxarr = new Box[row, col];
        float[] lineDepth = new float[row];
        float[] lineHeight = new float[row];
        float[] rowWidth = new float[col];
        float matW = 0;
        float drt = env.TeXFont.GetDefaultRuleThickness(env.Style);

        if (type == SMALLMATRIX)
        {
            env = env.Copy();
            env.Style = TeXConstants.STYLE_SCRIPT;
        }

        List<MulticolumnAtom> listMulti = [];

        for (int i = 0; i < row; i++)
        {
            lineDepth[i] = 0;
            lineHeight[i] = 0;
            for (int j = 0; j < col; j++)
            {
                Atom at = null;
                try
                {
                    at = matrix.array[i][j];
                }
                catch (Exception e)
                {
                    //The previous atom was an intertext atom
                    //position[j - 1] = -1;
                    boxarr[i, j - 1].Type = TeXConstants.TYPE_INTERTEXT;
                    j = col - 1;
                }

                boxarr[i, j] = (at == null) ? nullBox : at.CreateBox(env);

                lineDepth[i] = Math.Max(boxarr[i, j].Depth, lineDepth[i]);
                lineHeight[i] = Math.Max(boxarr[i, j].Height, lineHeight[i]);

                if (boxarr[i, j].Type != TeXConstants.TYPE_MULTICOLUMN)
                {
                    rowWidth[j] = Math.Max(boxarr[i, j].Width, rowWidth[j]);
                }
                else
                {
                    ((MulticolumnAtom)at).SetRowColumn(i, j);
                    listMulti.Add((MulticolumnAtom)at);
                }
            }
        }

        for (int i = 0; i < listMulti.Count; i++)
        {
            MulticolumnAtom multi = listMulti[i];
            int c = multi.Col;
            int r = multi.Row;
            int n = multi.Skipped;
            float w = 0;
            for (int j = c; j < c + n; j++)
            {
                w += rowWidth[j];
            }
            if (boxarr[r, c].Width > w)
            {
                float extraW = (boxarr[r, c].Width - w) / n;
                for (int j = c; j < c + n; j++)
                {
                    rowWidth[j] += extraW;
                }
            }
        }

        for (int j = 0; j < col; j++)
        {
            matW += rowWidth[j];
        }
        Box[] Hsep = GetColumnSep(env, matW);

        for (int j = 0; j < col + 1; j++)
        {
            matW += Hsep[j].Width;
            if (vlines[j] != null)
            {
                matW += vlines[j].GetWidth(env);
            }
        }

        VerticalBox vb = new VerticalBox();
        Box Vsep = vsep_in.CreateBox(env);
        vb.Add(vsep_ext_top.CreateBox(env));
        float totalHeight = 0;

        for (int i = 0; i < row; i++)
        {
            var hb = new HorizontalBox();
            for (int j = 0; j < col; j++)
            {
                switch (boxarr[i, j].Type)
                {
                    case -1:
                    case TeXConstants.TYPE_MULTICOLUMN:
                        if (j == 0)
                        {
                            if (vlines[0] != null)
                            {
                                VlineAtom vat = vlines[0];
                                vat.SetHeight(lineHeight[i] + lineDepth[i] + Vsep.Height);
                                vat.SetShift(lineDepth[i] + Vsep.Height / 2);
                                Box vatBox = vat.CreateBox(env);
                                hb.Add(new HorizontalBox(vatBox, Hsep[0].Width + vatBox.Width, TeXConstants.ALIGN_LEFT));
                            }
                            else
                            {
                                hb.Add(Hsep[0]);
                            }
                        }

                        bool lastVline = true;

                        if (boxarr[i, j].Type == -1)
                        {
                            hb.Add(new HorizontalBox(boxarr[i, j], rowWidth[j], position[j]));
                        }
                        else
                        {
                            Box b = GenerateMulticolumn(env, Hsep, rowWidth, i, j);
                            MulticolumnAtom matom = (MulticolumnAtom)matrix.array[i][j];
                            j += matom.Skipped - 1;
                            hb.Add(b);
                            lastVline = matom.HasRightVline;
                        }

                        if (lastVline && vlines[(j + 1)] != null)
                        {
                            VlineAtom vat = vlines[(j + 1)];
                            vat.SetHeight(lineHeight[i] + lineDepth[i] + Vsep.Height);
                            vat.SetShift(lineDepth[i] + Vsep.Height / 2);
                            Box vatBox = vat.CreateBox(env);
                            if (j < col - 1)
                            {
                                hb.Add(new HorizontalBox(vatBox, Hsep[j + 1].Width + vatBox.Width, TeXConstants.ALIGN_CENTER));
                            }
                            else
                            {
                                hb.Add(new HorizontalBox(vatBox, Hsep[j + 1].Width + vatBox.Width, TeXConstants.ALIGN_RIGHT));
                            }
                        }
                        else
                        {
                            hb.Add(Hsep[j + 1]);
                        }
                        break;
                    case TeXConstants.TYPE_INTERTEXT:
                        float f = env.Textwidth;
                        f = f == float.PositiveInfinity ? rowWidth[j] : f;
                        hb = new HorizontalBox(boxarr[i, j], f, TeXConstants.ALIGN_LEFT);
                        j = col - 1;
                        break;
                    case TeXConstants.TYPE_HLINE:
                        HlineAtom at = (HlineAtom)matrix.array[i][j];
                        at.SetWidth(matW);
                        if (i >= 1 && matrix.array[i - 1][j] is HlineAtom)
                        {
                            hb.Add(new StrutBox(0, 2 * drt, 0, 0));
                            at.SetShift(-Vsep.Height / 2 + drt);
                        }
                        else
                        {
                            at.SetShift(-Vsep.Height / 2);
                        }

                        hb.Add(at.CreateBox(env));
                        j = col;
                        break;
                }
            }

            if (boxarr[i, 0].Type != TeXConstants.TYPE_HLINE)
            {
                hb.Height = lineHeight[i];
                hb.Depth = lineDepth[i];
                vb.Add(hb);

                if (i < row - 1)
                    vb.Add(Vsep);
            }
            else
            {
                vb.Add(hb);
            }
        }

        vb.Add(vsep_ext_bot.CreateBox(env));
        totalHeight = vb.Height + vb.Depth;

        float axis = env.TeXFont.GetAxisHeight(env.Style);
        vb.Height = totalHeight / 2 + axis;
        vb.Depth = totalHeight / 2 - axis;

        return vb;
    }

    private Box GenerateMulticolumn(TeXEnvironment env, Box[] Hsep, float[] rowWidth, int i, int j)
    {
        float w = 0;
        MulticolumnAtom mca = (MulticolumnAtom)matrix.array[i][j];
        int k, n = mca.Skipped;
        for (k = j; k < j + n - 1; k++)
        {
            w += rowWidth[k] + Hsep[k + 1].Width;
            if (vlines[(k + 1)] != null)
            {
                w += vlines[(k + 1)].GetWidth(env);
            }
        }
        w += rowWidth[k];

        Box b = mca.CreateBox(env);
        float bw = b.Width;
        if (bw > w)
        {
            // It isn't a good idea but for the moment I have no other solution !
            w = 0;
        }

        mca.SetWidth(w);
        b = mca.CreateBox(env);
        return b;
    }
}
