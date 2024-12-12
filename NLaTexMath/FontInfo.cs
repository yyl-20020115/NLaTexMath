/* FontInfo.java
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
 * Contains all the font information for 1 font.
 */
public class FontInfo
{

    /**
     * Maximum number of character codes in a TeX font.
     */
    public const int NUMBER_OF_CHAR_CODES = 256;

    private static readonly Dictionary<int, FontInfo> fonts = [];

    public class CharCouple(char l, char r)
    {

        private readonly char left = l, right = r;

        public override bool Equals(object? o) => o is CharCouple lig && left == lig.left && right == lig.right;

        public override int GetHashCode() => (left + right) % 128;
    }

    // ID
    private readonly int fontId;

    // font
    private Font font;
    private readonly object _base;
    private readonly string path;
    private readonly string fontName;
    private readonly Dictionary<CharCouple, char> lig = [];
    private readonly Dictionary<CharCouple, float> kern = [];
    private float[][] metrics;
    private CharFont[] nextLarger;
    private int[][] extensions;
    private Dictionary<char, char> unicode = null;

    // skew character of the font (used for positioning accents)
    private char skewChar = '\uffff';

    // general parameters for this font
    private readonly float xHeight;
    private readonly float space;
    private readonly float quad;
    private int boldId;
    private int romanId;
    private int ssId;
    private int ttId;
    private int itId;
    public readonly string boldVersion;
    public readonly string romanVersion;
    public readonly string ssVersion;
    public readonly string ttVersion;
    public readonly string itVersion;

    public FontInfo(int fontId, object _base, string path, string fontName, int unicode, float xHeight, float space, float quad, string boldVersion, string romanVersion, string ssVersion, string ttVersion, string itVersion)
    {
        this.fontId = fontId;
        this._base = _base;
        this.path = path;
        this.fontName = fontName;
        this.xHeight = xHeight;
        this.space = space;
        this.quad = quad;
        this.boldVersion = boldVersion;
        this.romanVersion = romanVersion;
        this.ssVersion = ssVersion;
        this.ttVersion = ttVersion;
        this.itVersion = itVersion;
        int num = NUMBER_OF_CHAR_CODES;
        if (unicode != 0)
        {
            this.unicode = new(unicode);
            num = unicode;
        }
        metrics = new float[num][];
        nextLarger = new CharFont[num];
        extensions = new int[num][];
        fonts.Add(fontId, this);
    }

    /**
     *
     * @param left
     *           left character
     * @param right
     *           right character
     * @param k
     *           kern value
     */
    public void AddKern(char left, char right, float k)
    {
        kern.Add(new CharCouple(left, right), (k));
    }

    /**
     * @param left
     *           left character
     * @param right
     *           right character
     * @param ligChar
     *           ligature to Replace left and right character
     */
    public void AddLigature(char left, char right, char ligChar)
    {
        lig.Add(new CharCouple(left, right), (ligChar));
    }

    public int[] GetExtension(char ch) => unicode == null ? extensions[ch] : extensions[unicode[(ch)]];

    public float GetKern(char left, char right, float factor) => kern.TryGetValue(new CharCouple(left, right), out var f) ? f : 0;

    public CharFont GetLigature(char left, char right) =>
        lig.TryGetValue(new(left, right), out var c) ? new CharFont(c, fontId) : null;

    public float[] GetMetrics(char c) => unicode == null ? metrics[c] : metrics[unicode[c]];

    public CharFont GetNextLarger(char ch)
    {
        if (unicode == null)
            return nextLarger[ch];
        return nextLarger[unicode[(ch)]];
    }

    public float GetQuad(float factor) => quad * factor;

    /**
     * @return the skew character of the font (for the correct positioning of
     *         accents)
     */
    public char SkewChar { get => skewChar; set => skewChar = value; }

    public float GetSpace(float factor) => space * factor;

    public float GetXHeight(float factor) => xHeight * factor;

    public bool HasSpace => space > TeXFormula.PREC;

    public void SetExtension(char ch, int[] ext)
    {
        if (unicode == null)
            extensions[ch] = ext;
        else if (!unicode.TryGetValue(ch, out char value))
        {
            char s = (char)unicode.Count;
            unicode.Add(ch, s);
            extensions[s] = ext;
        }
        else
            extensions[value] = ext;
    }

    public void SetMetrics(char c, float[] arr)
    {
        if (unicode == null)
            metrics[c] = arr;
        else if (!unicode.TryGetValue(c, out char value))
        {
            char s = (char)unicode.Count;
            unicode.Add(c, s);
            metrics[s] = arr;
        }
        else
            metrics[value] = arr;
    }

    public void SetNextLarger(char ch, char larger, int fontLarger)
    {
        if (unicode == null)
            nextLarger[ch] = new CharFont(larger, fontLarger);
        else if (!unicode.TryGetValue(ch, out char value))
        {
            char s = (char)unicode.Count;
            unicode.Add(ch, s);
            nextLarger[s] = new CharFont(larger, fontLarger);
        }
        else
            nextLarger[value] = new CharFont(larger, fontLarger);
    }


    public int Id => fontId;

    public int BoldId { get => boldId; set => boldId = value == -1 ? fontId : value; }

    public int RomanId { get => romanId; set => romanId = value == -1 ? fontId : value; }
    public int TtId { get => ttId; set => ttId = value == -1 ? fontId : value; }

    public int ItId { get => itId; set => itId = value == -1 ? fontId : value; }

    public int SsId { get => ssId; set => ssId = value == -1 ? fontId : value; }

    public Font Font => font ??= _base == null
                    ? DefaultTeXFontParser.CreateFont(path)
                    : DefaultTeXFontParser.CreateFont(_base.GetType().GetResourceAsStream(path), fontName);

    public static Font GetFont(int id) => fonts[(id)].Font;
}

