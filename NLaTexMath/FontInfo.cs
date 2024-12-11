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
public class FontInfo {

    /**
     * Maximum number of character codes in a TeX font.
     */
    public static readonly int NUMBER_OF_CHAR_CODES = 256;

    private static Dictionary<int, FontInfo> fonts = [];

    public class CharCouple {

        private readonly char left, right;

        public CharCouple(char l, char r) {
            left = l;
            right = r;
        }

        public bool equals(object o) {
            CharCouple lig = (CharCouple) o;
            return left == lig.left && right == lig.right;
        }

        public int hashCode() {
            return (left + right) % 128;
        }
    }

    // ID
    private readonly int fontId;

    // font
    private Font font;
    private readonly object _base;
    private readonly string path;
    private readonly string fontName;
    private readonly Dictionary<CharCouple,char> lig = [];
    private readonly Dictionary<CharCouple,float> kern = [];
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
    protected readonly string boldVersion;
    protected readonly string romanVersion;
    protected readonly string ssVersion;
    protected readonly string ttVersion;
    protected readonly string itVersion;

    public FontInfo(int fontId, object _base, string path, string fontName, int unicode, float xHeight, float space, float quad, string boldVersion, string romanVersion, string ssVersion, string ttVersion, string itVersion) {
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
        if (unicode != 0) {
            this.unicode = new (unicode);
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
    public void addKern(char left, char right, float k) {
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
    public void addLigature(char left, char right, char ligChar) {
        lig.Add(new CharCouple(left, right), (ligChar));
    }

    public int[] getExtension(char ch) {
        if (unicode == null)
            return extensions[ch];
        return extensions[unicode[(ch)]];
    }

    public float getKern(char left, char right, float factor) {
        object obj = kern.Get(new CharCouple(left, right));
        if (obj == null)
            return 0;
        else
            return ((float) obj).floatValue() * factor;
    }

    public CharFont getLigature(char left, char right) {
        object obj = lig.Get(new CharCouple(left, right));
        if (obj == null)
            return null;
        else
            return new CharFont(((Character) obj).charValue(), fontId);
    }

    public float[] getMetrics(char c) {
        if (unicode == null)
            return metrics[c];
        return metrics[unicode.Get(c)];
    }

    public CharFont getNextLarger(char ch) {
        if (unicode == null)
            return nextLarger[ch];
        return nextLarger[unicode.Get(ch)];
    }

    public float getQuad(float factor) {
        return quad * factor;
    }

    /**
     * @return the skew character of the font (for the correct positioning of
     *         accents)
     */
    public char getSkewChar() {
        return skewChar;
    }

    public float getSpace(float factor) {
        return space * factor;
    }

    public float getXHeight(float factor) {
        return xHeight * factor;
    }

    public bool hasSpace() {
        return space > TeXFormula.PREC;
    }

    public void setExtension(char ch, int[] ext) {
        if (unicode == null)
            extensions[ch] = ext;
        else if (!unicode.ContainsKey(ch)) {
            char s = (char)unicode.Length;
            unicode.Add(ch, s);
            extensions[s] = ext;
        } else
            extensions[unicode.Get(ch)] = ext;
    }

    public void setMetrics(char c, float[] arr) {
        if (unicode == null)
            metrics[c] = arr;
        else if (!unicode.ContainsKey(c)) {
            char s = (char)unicode.Length;
            unicode.Add(c, s);
            metrics[s] = arr;
        } else
            metrics[unicode.Get(c)] = arr;
    }

    public void setNextLarger(char ch, char larger, int fontLarger) {
        if (unicode == null)
            nextLarger[ch] = new CharFont(larger, fontLarger);
        else if (!unicode.ContainsKey(ch)) {
            char s = (char)unicode.Count;
            unicode.Add(ch, s);
            nextLarger[s] = new CharFont(larger, fontLarger);
        } else
            nextLarger[unicode.Get(ch)] = new CharFont(larger, fontLarger);
    }

    public void setSkewChar(char c) {
        skewChar = c;
    }

    public int getId() {
        return fontId;
    }

    public int getBoldId() {
        return boldId;
    }

    public int getRomanId() {
        return romanId;
    }

    public int getTtId() {
        return ttId;
    }

    public int getItId() {
        return itId;
    }

    public int getSsId() {
        return ssId;
    }

    public void setSsId(int id) {
        ssId = id == -1 ? fontId : id;
    }

    public void setTtId(int id) {
        ttId = id == -1 ? fontId : id;
    }

    public void setItId(int id) {
        itId = id == -1 ? fontId : id;
    }

    public void setRomanId(int id) {
        romanId = id == -1 ? fontId : id;
    }

    public void setBoldId(int id) {
        boldId = id == -1 ? fontId : id;
    }

    public Font getFont() {
        if (font == null) {
            if (_base == null) {
                font = DefaultTeXFontParser.createFont(path);
            } else {
                font = DefaultTeXFontParser.createFont(_base.GetType().getResourceAsStream(path), fontName);
            }
        }
        return font;
    }

    public static Font getFont(int id) {
        return fonts.Get(id).getFont();
    }
}

