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
    public static readonly int NONE = -1;

    public readonly static int NUMBERS = 0;
    public readonly static int CAPITALS = 1;
    public readonly static int SMALL = 2;
    public readonly static int UNICODE = 3;

    private static Dictionary<string, CharFont[]> textStyleMappings;
    private static Dictionary<string, CharFont> symbolMappings;
    private static FontInfo[] fontInfo = new FontInfo[0];
    private static Dictionary<string, float> parameters;
    private static Dictionary<string, double> generalSettings;

    private static bool magnificationEnable = true;

    public static readonly int TOP = 0, MID = 1, REP = 2, BOT = 3;

    public static readonly int WIDTH = 0, HEIGHT = 1, DEPTH = 2, IT = 3;

    public static List<UnicodeBlock> loadedAlphabets = new List<UnicodeBlock>();
    public static Dictionary<UnicodeBlock, AlphabetRegistration> registeredAlphabets = new HashMap<Character.UnicodeBlock, AlphabetRegistration>();

    protected float factor = 1f;

    public bool isBold = false;
    public bool isRoman = false;
    public bool isSs = false;
    public bool isTt = false;
    public bool isIt = false;

    static DefaultTeXFont() {
        DefaultTeXFontParser parser = new DefaultTeXFontParser();
        //load LATIN block
        loadedAlphabets.Add(Character.UnicodeBlock.of('a'));
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
        int muFontId = generalSettings.Get(DefaultTeXFontParser.MUFONTID_ATTR).intValue();
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
            SymbolAtom.addSymbolAtom(insymbols, symbols);
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
        Character.UnicodeBlock[] blocks = reg.UnicodeBlocks;
        for (int i = 0; i < blocks.Length; i++) {
            registeredAlphabets.Add(blocks[i], reg);
        }
    }

    public TeXFont copy() {
        return new DefaultTeXFont(size, factor, isBold, isRoman, isSs, isTt, isIt);
    }

    public TeXFont deriveFont(float size) {
        return new DefaultTeXFont(size, factor, isBold, isRoman, isSs, isTt, isIt);
    }

    public TeXFont scaleFont(float factor) {
        return new DefaultTeXFont(size, factor, isBold, isRoman, isSs, isTt, isIt);
    }

    public float getScaleFactor() {
        return factor;
    }

    public float getAxisHeight(int style) {
        return getParameter("axisheight") * getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT;
    }

    public float getBigOpSpacing1(int style) {
        return getParameter("bigopspacing1") * getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT;
    }

    public float getBigOpSpacing2(int style) {
        return getParameter("bigopspacing2") * getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT;
    }

    public float getBigOpSpacing3(int style) {
        return getParameter("bigopspacing3") * getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT;
    }

    public float getBigOpSpacing4(int style) {
        return getParameter("bigopspacing4") * getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT;
    }

    public float getBigOpSpacing5(int style) {
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
            return getDefaultChar(c, style);
        else
            return getChar(new CharFont((char) (cf[kind].c + offset), cf[kind].fontId), style);
    }

    public Char getChar(char c, string textStyle, int style)  {
        object mapping = textStyleMappings.Get(textStyle);
        if (mapping == null) // text style mapping not found
            throw new TextStyleMappingNotFoundException(textStyle);
        else
            return getChar(c, (CharFont[]) mapping, style);
    }

    public Char getChar(CharFont cf, int style) {
        float fsize = getSizeFactor(style);
        int id = isBold ? cf.boldFontId : cf.fontId;
        FontInfo info = fontInfo[id];
        if (isBold && cf.fontId == cf.boldFontId) {
            id = info.getBoldId();
            info = fontInfo[id];
            cf = new CharFont(cf.c, id, style);
        }
        if (isRoman) {
            id = info.getRomanId();
            info = fontInfo[id];
            cf = new CharFont(cf.c, id, style);
        }
        if (isSs) {
            id = info.getSsId();
            info = fontInfo[id];
            cf = new CharFont(cf.c, id, style);
        }
        if (isTt) {
            id = info.getTtId();
            info = fontInfo[id];
            cf = new CharFont(cf.c, id, style);
        }
        if (isIt) {
            id = info.getItId();
            info = fontInfo[id];
            cf = new CharFont(cf.c, id, style);
        }
        Font font = info.getFont();
        return new Char(cf.c, font, id, getMetrics(cf, factor * fsize));
    }

    public Char getChar(string symbolName, int style)  {
        object obj = symbolMappings.Get(symbolName);
        if (obj == null) {// no symbol mapping found!
            throw new SymbolMappingNotFoundException(symbolName);
        } else {
            return getChar((CharFont) obj, style);
        }
    }

    public Char getDefaultChar(char c, int style) {
        // these default text style mappings will allways exist,
        // because it's checked during parsing
        if (c >= '0' && c <= '9') {
            return getChar(c, defaultTextStyleMappings[NUMBERS], style);
        } else if (c >= 'a' && c <= 'z') {
            return getChar(c, defaultTextStyleMappings[SMALL], style);
        } else {
            return getChar(c, defaultTextStyleMappings[CAPITALS], style);
        }
    }

    public float getDefaultRuleThickness(int style) {
        return getParameter("defaultrulethickness") * getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT;
    }

    public float getDenom1(int style) {
        return getParameter("denom1") * getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT;
    }

    public float getDenom2(int style) {
        return getParameter("denom2") * getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT;
    }

    public Extension getExtension(Char c, int style) {
        Font f = c.getFont();
        int fc = c.getFontCode();
        float s = getSizeFactor(style);

        // construct Char for every part
        FontInfo info = fontInfo[fc];
        int[] ext = info.getExtension(c.getChar());
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

    public float getKern(CharFont left, CharFont right, int style) {
        if (left.fontId == right.fontId) {
            FontInfo info = fontInfo[left.fontId];
            return info.getKern(left.c, right.c, getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT);
        } else {
            return 0;
        }
    }

    public CharFont getLigature(CharFont left, CharFont right) {
        if (left.fontId == right.fontId) {
            FontInfo info =  fontInfo[left.fontId];
            return info.getLigature(left.c, right.c);
        } else {
            return null;
        }
    }

    private Metrics getMetrics(CharFont cf, float size) {
        FontInfo info = fontInfo[cf.fontId];
        float[] m = info.getMetrics(cf.c);
        return new Metrics(m[WIDTH], m[HEIGHT], m[DEPTH], m[IT], size * TeXFormula.PIXELS_PER_POINT, size);
    }

    public int getMuFontId() {
        return generalSettings.Get(DefaultTeXFontParser.MUFONTID_ATTR).intValue();
    }

    public Char getNextLarger(Char c, int style) {
        FontInfo info = fontInfo[c.getFontCode()];
        CharFont ch = info.getNextLarger(c.getChar());
        FontInfo newInfo = fontInfo[ch.fontId];
        return new Char(ch.c, newInfo.getFont(), ch.fontId, getMetrics(ch, getSizeFactor(style)));
    }

    public float getNum1(int style) {
        return getParameter("num1") * getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT;
    }

    public float getNum2(int style) {
        return getParameter("num2") * getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT;
    }

    public float getNum3(int style) {
        return getParameter("num3") * getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT;
    }

    public float getQuad(int style, int fontCode) {
        FontInfo info = fontInfo[fontCode];
        return info.getQuad(getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT);
    }

    public float getSize() {
        return size;
    }

    public float getSkew(CharFont cf, int style) {
        FontInfo info = fontInfo[cf.fontId];
        char skew = info.getSkewChar();
        if (skew == -1)
            return 0;
        else
            return getKern(cf, new CharFont(skew, cf.fontId), style);
    }

    public float getSpace(int style) {
        int spaceFontId = generalSettings.Get(DefaultTeXFontParser.SPACEFONTID_ATTR).intValue();
        FontInfo info = fontInfo[spaceFontId];
        return info.getSpace(getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT);
    }

    public float getSub1(int style) {
        return getParameter("sub1") * getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT;
    }

    public float getSub2(int style) {
        return getParameter("sub2") * getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT;
    }

    public float getSubDrop(int style) {
        return getParameter("subdrop") * getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT;
    }

    public float getSup1(int style) {
        return getParameter("sup1") * getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT;
    }

    public float getSup2(int style) {
        return getParameter("sup2") * getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT;
    }

    public float getSup3(int style) {
        return getParameter("sup3") * getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT;
    }

    public float getSupDrop(int style) {
        return getParameter("supdrop") * getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT;
    }

    public float getXHeight(int style, int fontCode) {
        FontInfo info = fontInfo[fontCode];
        return info.getXHeight(getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT);
    }

    public float getEM(int style) {
        return getSizeFactor(style) * TeXFormula.PIXELS_PER_POINT;
    }

    public bool hasNextLarger(Char c) {
        FontInfo info = fontInfo[c.getFontCode()];
        return (info.getNextLarger(c.getChar()) != null);
    }

    public void setBold(bool bold) {
        isBold = bold;
    }

    public bool getBold() {
        return isBold;
    }

    public void setRoman(bool rm) {
        isRoman = rm;
    }

    public bool getRoman() {
        return isRoman;
    }

    public void setTt(bool tt) {
        isTt = tt;
    }

    public bool getTt() {
        return isTt;
    }

    public void setIt(bool it) {
        isIt = it;
    }

    public bool getIt() {
        return isIt;
    }

    public void setSs(bool ss) {
        isSs = ss;
    }

    public bool getSs() {
        return isSs;
    }

    public bool hasSpace(int font) {
        FontInfo info = fontInfo[font];
        return info.hasSpace();
    }

    public bool isExtensionChar(Char c) {
        FontInfo info = fontInfo[c.getFontCode()];
        return info.getExtension(c.getChar()) != null;
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
        object param = parameters.Get(parameterName);
        if (param == null)
            return 0;
        else
            return ((float) param).floatValue();
    }

    public static float getSizeFactor(int style) {
        if (style < TeXConstants.STYLE_TEXT)
            return 1;
        else if (style < TeXConstants.STYLE_SCRIPT)
            return generalSettings.get("textfactor").floatValue();
        else if (style < TeXConstants.STYLE_SCRIPT_SCRIPT)
            return generalSettings.get("scriptfactor").floatValue();
        else
            return generalSettings.get("scriptscriptfactor").floatValue();
    }
}
