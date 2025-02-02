﻿/* TeXFormula.cs
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

public class UnicodeBlock(char start, char end)
{
    public static readonly UnicodeBlock GREEK = new('\u0370', '\u03ff');
    public static readonly UnicodeBlock GREEK_EXTENDED = new('\u1f00', '\u1fff');
    public static readonly UnicodeBlock CYRILLIC = new('\u0400', '\u04ff');
    public static readonly UnicodeBlock BASIC_LATIN = new('\u0020', '\u007f');

    public readonly char Start = start;
    public readonly char End = end;

    public static UnicodeBlock Of(char v)
        => new(v, v);

    public override string ToString() => $"{this.Start}-{this.End}";
    public override bool Equals(object? obj)
        => obj is UnicodeBlock u ? this.Start == u.Start && this.End == u.End : base.Equals(obj);
    public override int GetHashCode() => this.Start ^ this.End;
}