/* OverUnderDelimiter.java
 * =========================================================================
 * This file is originally part of the JMathTeX Library - http://jmathtex.sourceforge.net
 *
 * Copyright (C) 2004-2007 Universiteit Gent
 * Copyright (C) 2009-2010 DENIZET Calixte
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
 * A box representing another atom with a delimiter and a script above or under it,
 * with script and delimiter seperated by a kern.
 */
public class OverUnderDelimiter : Atom
{

    // base and script atom
    private readonly Atom Base;
    private Atom script;

    // delimiter symbol
    private readonly SymbolAtom symbol;

    // kern between delimiter and script
    private readonly SpaceAtom kern;

    // whether the delimiter should be positioned above or under the base
    private readonly bool over;

    public OverUnderDelimiter(Atom _base, Atom script, SymbolAtom s, int kernUnit,
                              float kern, bool over)
    {
        Type = TeXConstants.TYPE_INNER;
        this.Base = _base;
        this.script = script;
        symbol = s;
        this.kern = new SpaceAtom(kernUnit, 0, kern, 0);
        this.over = over;
    }

    public void AddScript(Atom script)
    {
        this.script = script;
    }

    public bool IsOver => over;

    public override Box CreateBox(TeXEnvironment env)
    {
        Box b = (Base == null ? new StrutBox(0, 0, 0, 0) : Base.CreateBox(env));
        Box del = DelimiterFactory.Create(symbol.Name, env, b.Width);

        Box scriptBox = null;
        if (script != null)
        {
            scriptBox = script.CreateBox((over ? env.SupStyle : env.SubStyle));
        }

        // create centered horizontal box if smaller than maximum width
        float max = GetMaxWidth(b, del, scriptBox);
        if (max - b.Width > TeXFormula.PREC)
        {
            b = new HorizontalBox(b, max, TeXConstants.ALIGN_CENTER);
        }

        del = new VerticalBox(del, max, TeXConstants.ALIGN_CENTER);
        if (scriptBox != null && max - scriptBox.Width > TeXFormula.PREC)
        {
            scriptBox = new HorizontalBox(scriptBox, max, TeXConstants.ALIGN_CENTER);
        }

        return new OverUnderBox(b, del, scriptBox, kern.CreateBox(env).Height, over);
    }

    private static float GetMaxWidth(Box b, Box del, Box script)
    {
        float max = Math.Max(b.Width, del.Height + del.Depth);
        if (script != null)
        {
            max = Math.Max(max, script.Width);
        }

        return max;
    }
}
