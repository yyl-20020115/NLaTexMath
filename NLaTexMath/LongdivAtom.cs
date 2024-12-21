/* GraphicsAtom.cs
 * =========================================================================
 * This file is part of the JLaTeXMath Library - http://forge.scilab.org/jlatexmath
 *
 * Copyright (C) 2017 DENIZET Calixte
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
 * An atom representing a long division.
 */
public class LongdivAtom : VRowAtom
{
    public LongdivAtom(long divisor, long dividend)
    {
        Halign = TeXConstants.ALIGN_RIGHT;
        Vtop = true;
        var res = MakeResults(divisor, dividend);
        var rule = new RuleAtom(TeXConstants.UNIT_EX, 0f,
                                 TeXConstants.UNIT_EX, 2.6f,
                                 TeXConstants.UNIT_EX, 0.5f);
        for (int i = 0; i < res.Length; ++i)
        {
            var num = new TeXFormula(res[i]).root;
            if (i % 2 == 0)
            {
                var ra = new RowAtom(num);
                ra.Add(rule);
                if (i == 0)
                {
                    Append(ra);
                }
                else
                {
                    Append(new UnderlinedAtom(ra));
                }
            }
            else if (i == 1)
            {
                var div = (divisor.ToString());
                var rparen = SymbolAtom.Get(TeXFormula.symbolMappings[')']);
                var big = new BigDelimiterAtom(rparen, 1);
                var ph = new PhantomAtom(big, false, true, true);
                var ra = new RowAtom(ph);
                Atom raised = new RaiseAtom(big,
                                            TeXConstants.UNIT_X8, 3.5f,
                                            TeXConstants.UNIT_X8, 0f,
                                            TeXConstants.UNIT_X8, 0f);
                ra.Add(new SmashedAtom(raised));
                ra.Add(num);
                Atom a = new OverlinedAtom(ra);
                var ra1 = new RowAtom(new TeXFormula(div).root);
                ra1.Add(new SpaceAtom(TeXConstants.THINMUSKIP));
                ra1.Add(a);
                Append(ra1);
            }
            else
            {
                var ra = new RowAtom(num);
                ra.Add(rule);
                Append(ra);
            }
        }
    }

    private string[] MakeResults(long divisor, long dividend)
    {
        List<string> vec = [];
        long q = dividend / divisor;
        vec.Add(q.ToString());
        vec.Add(dividend.ToString());

        while (q != 0)
        {
            double p = (double)Math.Floor(Math.Log10((double)q));
            double p10 = Math.Pow(10.0, p);
            long d = (long)(Math.Floor(((double)q) / p10) * p10);
            long dd = d * divisor;
            vec.Add(dd.ToString());
            dividend -= dd;
            vec.Add((dividend.ToString()));
            q -= d;
        }

        return [.. vec];
    }
}
