/* DefaultTeXFont.java
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
 * The default implementation of the TeXFont-interface. All font information is read
 * from an xml-file.
 */
public class DefaultTeXFont : TeXFont {

    private static string[] defaultTextStyleMappings;

    /**
     * No extension part for that kind (TOP,MID,REP or BOT)
     */
    public const int   NONE = -1;

    public readonly static int NUMBERS = 0;
    public readonly static int CAPITALS = 1;
    public readonly static int SMALL = 2;
    public readonly static int UNICODE = 3;

    private static Dictionary<string, CharFont[]> textStyleMappings;
    private static Dictionary<string, CharFont> symbolMappings;
    private static FontInfo[] fontInfo = new FontInfo[0];
    private static Dictionary<string, float> parameters;
    private static Dictionary<string, ValueType> generalSettings;

    private static bool magnificationEnable = true;

    public const int   TOP = 0, MID = 1, REP = 2, BOT = 3;

    public const int   WIDTH = 0, HEIGHT = 1, DEPTH = 2, IT = 3;

    public static List<UnicodeBlock> loadedAlphabets = [];
    public static Dictionary<UnicodeBlock, AlphabetRegistration> registeredAlphabets = [];

    protected float factor = 1f;

    public bool isBold = false;
    public bool isRoman = false;
    public bool isSs = false;
    public bool isTt = false;
    public bool isIt = false;

    static DefaultTeXFont() {
        DefaultTeXFontParser parser = new DefaultTeXFontParser();
        //load LATIN block
        loadedAlphabets.Add(UnicodeBlock.of('a'));
        // fonts + font descriptions
        fontInfo = parser.parseFontDescriptions(fontInfo);
        // general font parameters
        parameters = parser.parseParameters();
        // text style mappings
        textStyleMappings = parser.parseTextStyleMappings();
        // default text style : style mappings
        defaultTextStyleMappings = parser.parseDefaultTextStyleMappings();
        // symbol mappings
        symbolMappings = parser.parseSymbolMappings();
        // general settings
        generalSettings = parser.parseGeneralSettings();
        generalSettings.Add("textfactor", 1);

        // check if mufontid exists
        int muFontId = (int)generalSettings[(DefaultTeXFontParser.MUFONTID_ATTR)];
        if (muFontId < 0 || muFontId >= fontInfo.Length || fontInfo[muFontId] == null)
            throw new XMLResourceParseException(
                DefaultTeXFontParser.RESOURCE_NAME,
                DefaultTeXFontParser.GEN_SET_EL,
                DefaultTeXFontParser.MUFONTID_ATTR,
                "Contains an unknown font id!");
    }

    private  float size; // standard size

    public DefaultTeXFont(float pointSize) {
        size = pointSize;
    }

    public DefaultTeXFont(float pointSize, bool b, bool rm, bool ss, bool tt, bool it)
    : this(pointSize, 1, b, rm, ss, tt, it)
    {
        ;
    }

    public DefaultTeXFont(float pointSize, float f, bool b, bool rm, bool ss, bool tt, bool it) {
        size = pointSize;
        factor = f;
        isBold = b;
        isRoman = rm;
        isSs = ss;
        isTt = tt;
        isIt = it;
    }

    public static void addTeXFontDescription(string file){
        FileInputStream _in;
        try {
            _in = new FileInputStream(file);
        } catch (FileNotFoundException e) {
            throw new ResourceParseException(file, e);
        }
        addTeXFontDescription(_in, file);
    }

    public static void addTeXFontDescription(Stream _in, string name){
        DefaultTeXFontParser dtfp = new DefaultTeXFontParser(_in, name);
        fontInfo = dtfp.parseFontDescriptions(fontInfo);
        textStyleMappings.putAll(dtfp.parseTextStyleMappings());
        symbolMappings.putAll(dtfp.parseSymbolMappings());
    }

    public static void addTeXFontDescription(object _base, Stream _in, string name){
        DefaultTeXFontParser dtfp = new DefaultTeXFontParser(_base, _in, name);
        fontInfo = dtfp.parseFontDescriptions(fontInfo);
        dtfp.parseExtraPath();
        textStyleMappings.putAll(dtfp.parseTextStyleMappings());
        symbolMappings.putAll(dtfp.parseSymbolMappings());
    }

    public static void addAlphabet(UnicodeBlock alphabet, Stream inlanguage, string language, Stream insymbols, string symbols, Stream inmappings, string mappings){
        if (!loadedAlphabets.Contains(alphabet)) {
            addTeXFontDescription(inlanguage, language);
            SymbolAtom.AddSymbolAtom(insymbols, symbols);
            TeXFormula.addSymbolMappings(inmappings, mappings);
            loadedAlphabets.Add(alphabet);
        }
    }

    public static void addAlphabet(object _base, UnicodeBlock[] alphabet, string language){
        bool b = false;
        for (int i = 0; !b && i < alphabet.Length; i++) {
            b = loadedAlphabets.Contains(alphabet[i]) || b;
        }
        if (!b) {
            TeXParser.isLoading = true;
            addTeXFontDescription(_base, _base.GetType().getResourceAsStream(language), language);
            for (int i = 0; i < alphabet.Length; i++) {
                loadedAlphabets.Add(alphabet[i]);
            }
            TeXParser.isLoading = false;
        }
    }

    public static void addAlphabet(UnicodeBlock alphabet, string name) {
        string lg = "fonts/" + name + "/language_" + name+ ".xml";
        string sym = "fonts/" + name + "/symbols_" + name+ ".xml";
        string map = "fonts/" + name + "/mappings_" + name+ ".xml";

        try {
            DefaultTeXFont.addAlphabet(alphabet, TeXFormula.getResourceAsStream(lg), lg, TeXFormula..getResourceAsStream(sym), sym, TeXFormula..getResourceAsStream(map), map);
        } catch (FontAlreadyLoadedException e) { }
    }

    public static void addAlphabet(AlphabetRegistration reg) {
        try {
            if (reg != null) {
                DefaultTeXFont.addAlphabet(reg.Package, reg.UnicodeBlocks, reg.TeXFontFileName);
            }
        } catch (FontAlreadyLoadedException e) {
        } catch (AlphabetRegistrationException e) {
            Console.Error.WriteLine(e.ToString());
        }
    }

    public static void registerAlphabet(AlphabetRegistration reg) {
        UnicodeBlock[] blocks = reg.UnicodeBlocks;
        for (int i = 0; i < blocks.Length; i++) {
            registeredAlphabets.Add(blocks[i], reg);
        }
    }

    public TeXFont Copy() {
        return new DefaultTeXFont(size, factor, isBold, isRoman, isSs, isTt, isIt);
    }

    public TeXFont DeriveFont(float size) {
        return new DefaultTeXFont(size, factor, isBold, isRoman, isSs, isTt, isIt);
    }

    public TeXFont ScaleFont(float factor) {
        return new DefaultTeXFont(size, factor, isBold, isRoman, isSs, isTt, isIt);
    }

    public float GetScaleFactor() {
        return factor;
    }

    public float GetAxisHeight(int style) {
        return getParameter("axisheight") * getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT;
    }

    public float GetBigOpSpacing1(int style) {
        return getParameter("bigopspacing1") * getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT;
    }

    public float GetBigOpSpacing2(int style) {
        return getParameter("bigopspacing2") * getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT;
    }

    public float GetBigOpSpacing3(int style) {
        return getParameter("bigopspacing3") * getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT;
    }

    public float GetBigOpSpacing4(int style) {
        return getParameter("bigopspacing4") * getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT;
    }

    public float GetBigOpSpacing5(int style) {
        return getParameter("bigopspacing5") * getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT;
    }

    private Char getChar(char c, CharFont[] cf, int style) {
        int kind, offset;
        if (c >= '0' && c <= '9') {
            kind = NUMBERS;
            offset = c - '0';
        } else if (c >= 'a' && c <= 'z') {
            kind = SMALL;
            offset = c - 'a';
        } else if (c >= 'A' && c  <= 'Z') {
            kind = CAPITALS;
            offset = c - 'A';
        } else {
            kind = UNICODE;
            offset = c;
        }

        // if the mapping for the character's range, then use the default style
        if (cf[kind] == null)
            return GetDefaultChar(c, style);
        else
            return GetChar(new CharFont((char) (cf[kind].c + offset), cf[kind].fontId), style);
    }

    public Char GetChar(char c, string textStyle, int style)  {
        object mapping = textStyleMappings[(textStyle)];
        if (mapping == null) // text style mapping not found
            throw new TextStyleMappingNotFoundException(textStyle);
        else
            return getChar(c, (CharFont[]) mapping, style);
    }

    public Char GetChar(CharFont cf, int style) {
        float fsize = getSizeFactor(style);
        int id = isBold ? cf.boldFontId : cf.fontId;
        FontInfo info = fontInfo[id];
        if (isBold && cf.fontId == cf.boldFontId) {
            id = info.BoldId;
            info = fontInfo[id];
            cf = new CharFont(cf.c, id, style);
        }
        if (isRoman) {
            id = info.RomanId;
            info = fontInfo[id];
            cf = new CharFont(cf.c, id, style);
        }
        if (isSs) {
            id = info.SsId;
            info = fontInfo[id];
            cf = new CharFont(cf.c, id, style);
        }
        if (isTt) {
            id = info.TtId;
            info = fontInfo[id];
            cf = new CharFont(cf.c, id, style);
        }
        if (isIt) {
            id = info.ItId;
            info = fontInfo[id];
            cf = new CharFont(cf.c, id, style);
        }
        Font font = info.Font;
        return new Char(cf.c, font, id, getMetrics(cf, factor * fsize));
    }

    public Char GetChar(string symbolName, int style)  {
        object obj = symbolMappings[(symbolName)];
        if (obj == null) {// no symbol mapping found!
            throw new SymbolMappingNotFoundException(symbolName); 
        } else {
            return GetChar((CharFont) obj, style);
        }
    }

    public Char GetDefaultChar(char c, int style) {
        // these default text style mappings will allways exist,
        // because it's checked during parsing
        if (c >= '0' && c <= '9') {
            return GetChar(c, defaultTextStyleMappings[NUMBERS], style);
        } else if (c >= 'a' && c <= 'z') {
            return GetChar(c, defaultTextStyleMappings[SMALL], style);
        } else {
            return GetChar(c, defaultTextStyleMappings[CAPITALS], style);
        }
    }

    public float GetDefaultRuleThickness(int style) {
        return getParameter("defaultrulethickness") * getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT;
    }

    public float GetDenom1(int style) {
        return getParameter("denom1") * getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT;
    }

    public float GetDenom2(int style) {
        return getParameter("denom2") * getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT;
    }

    public Extension GetExtension(Char c, int style) {
        Font f = c.Font;
        int fc = c.FontCode;
        float s = getSizeFactor(style);

        // construct Char for every part
        FontInfo info = fontInfo[fc];
        int[] ext = info.GetExtension(c.Character);
        Char[] parts = new Char[ext.Length];
        for (int i = 0; i < ext.Length; i++) {
            if (ext[i] == NONE) {
                parts[i] = null;
            } else {
                parts[i] = new Char((char) ext[i], f, fc, getMetrics(new CharFont((char) ext[i], fc), s));
            }
        }

        return new Extension(parts[TOP], parts[MID], parts[REP], parts[BOT]);
    }

    public float GetKern(CharFont left, CharFont right, int style) {
        if (left.fontId == right.fontId) {
            FontInfo info = fontInfo[left.fontId];
            return info.GetKern(left.c, right.c, getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT);
        } else {
            return 0;
        }
    }

    public CharFont GetLigature(CharFont left, CharFont right) {
        if (left.fontId == right.fontId) {
            FontInfo info =  fontInfo[left.fontId];
            return info.GetLigature(left.c, right.c);
        } else {
            return null;
        }
    }

    private Metrics getMetrics(CharFont cf, float size) {
        FontInfo info = fontInfo[cf.fontId];
        float[] m = info.GetMetrics(cf.c);
        return new Metrics(m[WIDTH], m[HEIGHT], m[DEPTH], m[IT], size * TeXFormula.PIXELS_PER_POINT, size);
    }

    public int GetMuFontId() {
        return (int)generalSettings[(DefaultTeXFontParser.MUFONTID_ATTR)];
    }

    public Char GetNextLarger(Char c, int style) {
        FontInfo info = fontInfo[c.FontCode];
        CharFont ch = info.GetNextLarger(c.Character);
        FontInfo newInfo = fontInfo[ch.fontId];
        return new Char(ch.c, newInfo.Font, ch.fontId, getMetrics(ch, getSizeFactor(style)));
    }

    public float GetNum1(int style) {
        return getParameter("num1") * getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT;
    }

    public float GetNum2(int style) {
        return getParameter("num2") * getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT;
    }

    public float GetNum3(int style) {
        return getParameter("num3") * getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT;
    }

    public float GetQuad(int style, int fontCode) {
        FontInfo info = fontInfo[fontCode];
        return info.GetQuad(getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT);
    }

    public float GetSize() {
        return size;
    }

    public float GetSkew(CharFont cf, int style) {
        FontInfo info = fontInfo[cf.fontId];
        char skew = info.SkewChar;
        if (skew == -1)
            return 0;
        else
            return GetKern(cf, new CharFont(skew, cf.fontId), style);
    }

    public float GetSpace(int style) {
        int spaceFontId = (int)generalSettings[DefaultTeXFontParser.SPACEFONTID_ATTR];
        FontInfo info = fontInfo[spaceFontId];
        return info.GetSpace(getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT);
    }

    public float GetSub1(int style) {
        return getParameter("sub1") * getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT;
    }

    public float GetSub2(int style) {
        return getParameter("sub2") * getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT;
    }

    public float GetSubDrop(int style) {
        return getParameter("subdrop") * getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT;
    }

    public float GetSup1(int style) {
        return getParameter("sup1") * getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT;
    }

    public float GetSup2(int style) {
        return getParameter("sup2") * getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT;
    }

    public float GetSup3(int style) {
        return getParameter("sup3") * getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT;
    }

    public float GetSupDrop(int style) {
        return getParameter("supdrop") * getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT;
    }

    public float GetXHeight(int style, int fontCode) {
        FontInfo info = fontInfo[fontCode];
        return info.GetXHeight(getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT);
    }

    public float GetEM(int style) {
        return getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT;
    }

    public bool HasNextLarger(Char c) {
        FontInfo info = fontInfo[c.FontCode];
        return (info.GetNextLarger(c.Character) != null);
    }

    public void SetBold(bool bold) {
        isBold = bold;
    }

    public bool GetBold() {
        return isBold;
    }

    public void SetRoman(bool rm) {
        isRoman = rm;
    }

    public bool GetRoman() {
        return isRoman;
    }

    public void SetTt(bool tt) {
        isTt = tt;
    }

    public bool GetTt() {
        return isTt;
    }

    public void SetIt(bool it) {
        isIt = it;
    }

    public bool GetIt() {
        return isIt;
    }

    public void SetSs(bool ss) {
        isSs = ss;
    }

    public bool GetSs() {
        return isSs;
    }

    public bool HasSpace(int font) {
        FontInfo info = fontInfo[font];
        return info.HasSpace;
    }

    public bool IsExtensionChar(Char c) {
        FontInfo info = fontInfo[c.FontCode];
        return info.GetExtension(c.Character) != null;
    }

    public static void setMathSizes(float ds, float ts, float ss, float sss) {
        if (magnificationEnable) {
            generalSettings.Add("scriptfactor", Math.Abs(ss / ds));
            generalSettings.Add("scriptscriptfactor", Math.Abs(sss / ds));
            generalSettings.Add("textfactor", Math.Abs(ts / ds));
            TeXIcon.defaultSize = Math.Abs(ds);
        }
    }

    public static void setMagnification(float mag) {
        if (magnificationEnable) {
            TeXIcon.magFactor = mag / 1000f;
        }
    }

    public static void enableMagnification(bool b) {
        magnificationEnable = b;
    }

    private static float getParameter(string parameterName) {
        var param = parameters[(parameterName)];
        if (param == null)
            return 0;
        else
            return ((float) param);
    }

    public static float getSizeFactor(int style) {
        if (style < TeXConstants.STYLE_TEXT)
            return 1;
        else if (style < TeXConstants.STYLE_SCRIPT)
            return (float)generalSettings[("textfactor")];
        else if (style < TeXConstants.STYLE_SCRIPT_SCRIPT)
            return (float)generalSettings[("scriptfactor")];
        else
            return (float)generalSettings[("scriptscriptfactor")];
    }
}
