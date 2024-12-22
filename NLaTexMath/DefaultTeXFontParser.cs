/* DefaultTeXFontParser.cs
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

/* Modified by Calixte Denizet */
using NLaTexMath.Internal.Util;
using System.Drawing;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.XPath;

namespace NLaTexMath;

/**
 * Parses the font information from an XML-file.
 */
public class DefaultTeXFontParser
{

    /**
     * if the register font cannot be found, we display an error message
     * but we do it only once
     */
    private static bool registerFontExceptionDisplayed = false;
    private static bool shouldRegisterFonts = true;
    //private static DocumentBuilderFactory factory = DocumentBuilderFactory.newInstance();
    public interface CharChildParser
    { // NOPMD
        public void Parse(XElement el, char ch, FontInfo info);
    }

    public class ExtensionParser : CharChildParser
    {

        public ExtensionParser()
        {
            // avoid generation of access class
        }

        public void Parse(XElement el, char ch, FontInfo info)
        {
            int[] extensionChars = new int[4];
            // get required integer attributes
            extensionChars[DefaultTeXFont.REP] = DefaultTeXFontParser
                                                 .GetIntAndCheck("rep", el);
            // get optional integer attributes
            extensionChars[DefaultTeXFont.TOP] = DefaultTeXFontParser
                                                 .GetOptionalInt("top", el, DefaultTeXFont.NONE);
            extensionChars[DefaultTeXFont.MID] = DefaultTeXFontParser
                                                 .GetOptionalInt("mid", el, DefaultTeXFont.NONE);
            extensionChars[DefaultTeXFont.BOT] = DefaultTeXFontParser
                                                 .GetOptionalInt("bot", el, DefaultTeXFont.NONE);

            // parsing OK, Add extension info
            info.SetExtension(ch, extensionChars);
        }
    }

    public class KernParser : CharChildParser
    {
        public KernParser()
        {
            // avoid generation of access class
        }

        public void Parse(XElement el, char ch, FontInfo info)
        {
            // get required integer attribute
            int code = DefaultTeXFontParser.GetIntAndCheck("code", el);
            // get required float attribute
            float kernAmount = DefaultTeXFontParser.GetFloatAndCheck("val", el);

            // parsing OK, Add kern info
            info.AddKern(ch, (char)code, kernAmount);
        }
    }

    public class LigParser : CharChildParser
    {

        public LigParser()
        {
            // avoid generation of access class
        }

        public void Parse(XElement el, char ch, FontInfo info)
        {
            // get required integer attributes
            int code = DefaultTeXFontParser.GetIntAndCheck("code", el);
            int ligCode = DefaultTeXFontParser.GetIntAndCheck("ligCode", el);

            // parsing OK, Add ligature info
            info.AddLigature(ch, (char)code, (char)ligCode);
        }
    }

    public class NextLargerParser : CharChildParser
    {

        public NextLargerParser()
        {
            // avoid generation of access class
        }

        public void Parse(XElement el, char ch, FontInfo info)
        {
            // get required integer attributes
            string fontId = DefaultTeXFontParser.GetAttrValueAndCheckIfNotNull("fontId", el);
            int code = DefaultTeXFontParser.GetIntAndCheck("code", el);

            // parsing OK, Add "next larger" info
            info.SetNextLarger(ch, (char)code, Font_ID.IndexOf(fontId));
        }
    }

    public static readonly string RESOURCE_NAME = "DefaultTeXFont.xml";

    public static readonly string STYLE_MAPPING_EL = "TextStyleMapping";
    public static readonly string SYMBOL_MAPPING_EL = "SymbolMapping";
    public static readonly string GEN_SET_EL = "GeneralSettings";
    public static readonly string MUFONTID_ATTR = "mufontid";
    public static readonly string SPACEFONTID_ATTR = "spacefontid";

    protected static List<string> Font_ID = new();
    private static Dictionary<string, int> rangeTypeMappings = new();
    private static Dictionary<string, CharChildParser> charChildParsers = new();

    private Dictionary<string, CharFont[]> parsedTextStyles;

    private XElement root;
    private object _base = null;

    static DefaultTeXFontParser()
    {
        // string-to-constant mappings
        SetRangeTypeMappings();
        // parsers for the child elements of a "Char"-element
        setCharChildParsers();
    }

    public DefaultTeXFontParser()
        : this(typeof(DefaultTeXFontParser).GetResourceAsStream(RESOURCE_NAME), RESOURCE_NAME)
    {
        ;
    }

    public DefaultTeXFontParser(Stream file, string name)
    {
        try
        {
            root = XDocument.Load(file).Root;
        }
        catch (Exception e)
        { // JDOMException or IOException
            throw new XMLResourceParseException(name, e);
        }
    }

    public DefaultTeXFontParser(object _base, Stream file, string name)
    {
        this._base = _base;
        try
        {
            root = XDocument.Load(file).Root;
        }
        catch (Exception e)
        { // JDOMException or IOException
            throw new XMLResourceParseException(name, e);
        }
    }

    private static void setCharChildParsers()
    {
        charChildParsers.Add("Kern", new KernParser());
        charChildParsers.Add("Lig", new LigParser());
        charChildParsers.Add("NextLarger", new NextLargerParser());
        charChildParsers.Add("Extension", new ExtensionParser());
    }

    public FontInfo[] parseFontDescriptions(FontInfo[] fi, Stream file, string name)
    {
        if (file == null)
        {
            return fi;
        }
        List<FontInfo> res = new List<FontInfo>(fi);
        XElement font;
        try
        {
            font = XDocument.Load(file).Root;
        }
        catch (Exception e)
        {
            throw new XMLResourceParseException("Cannot find the file " + name + "!" + e.ToString());
        }

        string fontName = GetAttrValueAndCheckIfNotNull("name", font);
        // get required integer attribute
        string fontId = GetAttrValueAndCheckIfNotNull("id", font);
        if (Font_ID.IndexOf(fontId) < 0)
            Font_ID.Add(fontId);
        else throw new FontAlreadyLoadedException("Font " + fontId + " is already loaded !");
        // get required real attributes
        float space = GetFloatAndCheck("space", font);
        float xHeight = GetFloatAndCheck("xHeight", font);
        float quad = GetFloatAndCheck("quad", font);

        // get optional integer attribute
        int skewChar = GetOptionalInt("skewChar", font, -1);

        // get optional bool for unicode
        int unicode = GetOptionalInt("unicode", font, 0);

        // get different versions of a font
        string bold = null;
        try
        {
            bold = GetAttrValueAndCheckIfNotNull("boldVersion", font);
        }
        catch (ResourceParseException e) { }
        string roman = null;
        try
        {
            roman = GetAttrValueAndCheckIfNotNull("romanVersion", font);
        }
        catch (ResourceParseException e) { }
        string ss = null;
        try
        {
            ss = GetAttrValueAndCheckIfNotNull("ssVersion", font);
        }
        catch (ResourceParseException e) { }
        string tt = null;
        try
        {
            tt = GetAttrValueAndCheckIfNotNull("ttVersion", font);
        }
        catch (ResourceParseException e) { }
        string it = null;
        try
        {
            it = GetAttrValueAndCheckIfNotNull("itVersion", font);
        }
        catch (ResourceParseException e) { }

        string path = name.Substring(0, name.LastIndexOf("/") + 1) + fontName;

        // create FontInfo-object
        FontInfo info = new FontInfo(Font_ID.IndexOf(fontId), _base, path, fontName, unicode, xHeight, space, quad, bold, roman, ss, tt, it);

        if (skewChar != -1) // attribute set
            info.SkewChar = (char)skewChar;

        // process all "Char"-elements
        List<XElement> listF = font.Elements("Char").ToList();
        for (int j = 0; j < listF.Count; j++)
            ProcessCharElement((XElement)listF[(j)], info);

        // parsing OK, Add to table
        res.Add(info);

        for (int i = 0; i < res.Count; i++)
        {
            FontInfo fin = res[i];
            fin.BoldId = Font_ID.IndexOf(fin.boldVersion);
            fin.RomanId = Font_ID.IndexOf(fin.romanVersion);
            fin.SsId = Font_ID.IndexOf(fin.ssVersion);
            fin.TtId = Font_ID.IndexOf(fin.ttVersion);
            fin.ItId = Font_ID.IndexOf(fin.itVersion);
        }

        parsedTextStyles = ParseStyleMappings();
        return res.ToArray();
    }

    public FontInfo[] ParseFontDescriptions(FontInfo[] fi)
    {
        XElement fontDescriptions = root.XPathSelectElement("FontDescriptions");
        if (fontDescriptions != null)
        { // element present
            List<XElement> list = fontDescriptions.XPathSelectElements("Metrics").ToList();
            for (int i = 0; i < list.Count; i++)
            {
                // get required string attribute
                string include = GetAttrValueAndCheckIfNotNull("include", (XElement)list[i]);
                if (_base == null)
                {
                    fi = parseFontDescriptions(fi, typeof(DefaultTeXFontParser).GetResourceAsStream(include), include);
                }
                else
                {
                    fi = parseFontDescriptions(fi, _base.GetType().GetResourceAsStream(include), include);
                }
            }
        }
        return fi;
    }

    public void ParseExtraPath()
    {
        XElement syms = root.XPathSelectElement("TeXSymbols");
        if (syms != null)
        { // element present
          // get required string attribute
            string include = GetAttrValueAndCheckIfNotNull("include", syms);
            SymbolAtom.AddSymbolAtom(_base.GetType().GetResourceAsStream(include), include);
        }
        XElement settings = (XElement)root.XPathSelectElement("FormulaSettings");
        if (settings != null)
        { // element present
          // get required string attribute
            string include = GetAttrValueAndCheckIfNotNull("include", settings);
            TeXFormula.AddSymbolMappings(_base.GetType().GetResourceAsStream(include), include);
        }
    }

    private static void ProcessCharElement(XElement charElement, FontInfo info)
    {
        // retrieve required integer attribute
        char ch = (char)GetIntAndCheck("code", charElement);
        // retrieve optional float attributes
        float[] metrics = new float[4];
        metrics[DefaultTeXFont.WIDTH] = GetOptionalFloat("width", charElement, 0);
        metrics[DefaultTeXFont.HEIGHT] = GetOptionalFloat("height", charElement, 0);
        metrics[DefaultTeXFont.DEPTH] = GetOptionalFloat("depth", charElement, 0);
        metrics[DefaultTeXFont.IT] = GetOptionalFloat("italic", charElement, 0);
        // set metrics
        info.SetMetrics(ch, metrics);

        // process children
        List<XNode> list = charElement.Nodes().ToList();
        for (int i = 0; i < list.Count; i++)
        {
            XNode node = list[i];
            if (node.NodeType != System.Xml.XmlNodeType.Text)
            {
                XElement el = (XElement)node;
                if (!charChildParsers.TryGetValue(el.Name.LocalName, out var parser)) // unknown element
                    throw new XMLResourceParseException(RESOURCE_NAME
                                                        + ": a <Char>-element has an unknown child element '"
                                                        + el.Name.LocalName + "'!");
                else
                    // process the child element
                    ((CharChildParser)parser).Parse(el, ch, info);
            }
        }
    }

    public static void RegisterFonts(bool b)
    {
        shouldRegisterFonts = b;
    }

    public static Font CreateFont(string name)
    {
        return CreateFont(typeof(DefaultTeXFontParser).GetResourceAsStream(name), name);
    }

    public static Font CreateFont(Stream fontIn, string name)
    {
        try
        {
            Font f = null;
            //Font f = Font.createFont(Font.TRUETYPE_FONT, fontIn)
            //         .deriveFont(TeXFormula.PIXELS_PER_POINT * TeXFormula.FONT_SCALE_FACTOR);
            //GraphicsEnvironment graphicEnv = GraphicsEnvironment.getLocalGraphicsEnvironment();
            /**
             * The following fails under java 1.5
             * graphicEnv.registerFont(f);
             * dynamic load then
             */
            if (shouldRegisterFonts)
            {
                try
                {
                    //MethodInfo registerFontMethod = graphicEnv.GetType().getMethod("registerFont", [typeof(Font)]);
                    //if ((Boolean)registerFontMethod.Invoke(graphicEnv, new object[] { f }) == false)
                    //{
                    //    Console.Error.WriteLine("Cannot register the font " + f.Name);
                    //}
                }
                catch (Exception ex)
                {
                    if (!registerFontExceptionDisplayed)
                    {
                        Console.Error.WriteLine("Warning: Jlatexmath: Could not access to registerFont. Please update to java 6");
                        registerFontExceptionDisplayed = true;
                    }
                }
            }
            return f;
        }
        catch (Exception e)
        {
            throw new XMLResourceParseException(RESOURCE_NAME
                                                + ": error reading font '" + name + "'. Error message: "
                                                + e.Message);
        }
        finally
        {
            try
            {
                if (fontIn != null)
                    fontIn.Close();
            }
            catch (IOException ioex)
            {
                throw new Exception("Close threw exception", ioex);
            }
        }
    }

    public Dictionary<string, CharFont> ParseSymbolMappings()
    {
        Dictionary<string, CharFont> res = new();
        XElement symbolMappings = (XElement)root.Element("SymbolMappings");
        if (symbolMappings == null)
            // "SymbolMappings" is required!
            throw new XMLResourceParseException(RESOURCE_NAME, "SymbolMappings");
        else
        { // element present
          // iterate all mappings
            List<XElement> list = symbolMappings.Elements("Mapping").ToList();
            for (int i = 0; i < list.Count; i++)
            {
                string include = GetAttrValueAndCheckIfNotNull("include", (XElement)list[i]);
                XElement map;
                try
                {
                    if (_base == null)
                    {
                        map = XDocument.Load(typeof(DefaultTeXFontParser).GetResourceAsStream(include)).Root;
                    }
                    else
                    {
                        map = XDocument.Load(_base.GetType().GetResourceAsStream(include)).Root;
                    }
                }
                catch (Exception e)
                {
                    throw new XMLResourceParseException("Cannot find the file " + include + "!");
                }
                List<XElement> listM = map.Elements(SYMBOL_MAPPING_EL).ToList();
                for (int j = 0; j < listM.Count; j++)
                {
                    XElement mapping = (XElement)listM[(j)];
                    // get string attribute
                    string symbolName = GetAttrValueAndCheckIfNotNull("name", mapping);
                    // get integer attributes
                    int ch = GetIntAndCheck("ch", mapping);
                    string fontId = GetAttrValueAndCheckIfNotNull("fontId", mapping);
                    // put mapping in table
                    string boldFontId = null;
                    try
                    {
                        boldFontId = GetAttrValueAndCheckIfNotNull("boldId", mapping);
                    }
                    catch (ResourceParseException e) { }

                    if (boldFontId == null)
                    {
                        res.Add(symbolName, new CharFont((char)ch, Font_ID.IndexOf(fontId)));
                    }
                    else
                    {
                        res.Add(symbolName, new CharFont((char)ch, Font_ID.IndexOf(fontId), Font_ID.IndexOf(boldFontId)));
                    }
                }
            }

            return res;
        }
    }

    public string[] ParseDefaultTextStyleMappings()
    {
        string[] res = new string[4];
        XElement defaultTextStyleMappings = (XElement)root.Element("DefaultTextStyleMapping");
        if (defaultTextStyleMappings == null)
            return res;
        else
        { // element present
          // iterate all mappings
            List<XElement> list = defaultTextStyleMappings.Elements("MapStyle").ToList();
            for (int i = 0; i < list.Count; i++)
            {
                XElement mapping = (XElement)list[i];
                // get range name and check if it's valid
                string code = GetAttrValueAndCheckIfNotNull("code", mapping);
                object codeMapping = rangeTypeMappings[(code)];
                if (codeMapping == null) // unknown range name
                    throw new XMLResourceParseException(RESOURCE_NAME, "MapStyle",
                                                        "code", "Contains an unknown \"range name\" '" + code
                                                        + "'!");
                // get mapped style and check if it exists
                string textStyleName = GetAttrValueAndCheckIfNotNull("textStyle",
                                       mapping);
                object styleMapping = parsedTextStyles[(textStyleName)];
                if (styleMapping == null) // unknown text style
                    throw new XMLResourceParseException(RESOURCE_NAME, "MapStyle",
                                                        "textStyle", "Contains an unknown text style '"
                                                        + textStyleName + "'!");
                // now check if the range is defined within the mapped text style
                CharFont[] charFonts = parsedTextStyles[(textStyleName)];
                int index = ((int)codeMapping);
                if (charFonts[index] == null) // range not defined
                    throw new XMLResourceParseException(RESOURCE_NAME
                                                        + ": the default text style mapping '" + textStyleName
                                                        + "' for the range '" + code
                                                        + "' Contains no mapping for that range!");
                else
                    // everything OK, put mapping in table
                    res[index] = textStyleName;
            }
        }
        return res;
    }

    public Dictionary<string, float> ParseParameters()
    {
        Dictionary<string, float> res = new();
        XElement parameters = root.Element("Parameters");
        if (parameters == null)
            // "Parameters" is required!
            throw new XMLResourceParseException(RESOURCE_NAME, "Parameters");
        else
        { // element present
          // iterate all attributes
            var list = parameters.Attributes().ToList();
            for (int i = 0; i < list.Count; i++)
            {
                string name = list[i].Name.ToString();
                // set float value (if valid)
                res.Add(name, (GetFloatAndCheck(name, parameters)));
            }
            return res;
        }
    }

    public Dictionary<string, ValueType> ParseGeneralSettings()
    {
        Dictionary<string, ValueType> res = new();
        // TODO: must this be 'Number' ?
        XElement generalSettings = (XElement)root.XPathSelectElements("//GeneralSettings").First();
        if (generalSettings == null)
            // "GeneralSettings" is required!
            throw new XMLResourceParseException(RESOURCE_NAME, "GeneralSettings");
        else
        { // element present
          // set required int values (if valid)
            res.Add(MUFONTID_ATTR, Font_ID.IndexOf(GetAttrValueAndCheckIfNotNull(MUFONTID_ATTR, generalSettings))); // autoboxing
            res.Add(SPACEFONTID_ATTR, Font_ID.IndexOf(GetAttrValueAndCheckIfNotNull(SPACEFONTID_ATTR, generalSettings))); // autoboxing
                                                                                                                          // set required float values (if valid)
            res.Add("scriptfactor", GetFloatAndCheck("scriptfactor", generalSettings)); // autoboxing
            res.Add("scriptscriptfactor", GetFloatAndCheck("scriptscriptfactor", generalSettings)); // autoboxing

        }
        return res;
    }

    public Dictionary<string, CharFont[]> ParseTextStyleMappings()
    {
        return parsedTextStyles;
    }

    private Dictionary<string, CharFont[]> ParseStyleMappings()
    {
        Dictionary<string, CharFont[]> res = [];
        XElement textStyleMappings = (XElement)root.Element("TextStyleMappings");
        if (textStyleMappings == null)
            return res;
        else
        { // element present
          // iterate all mappings
            List<XElement> list = textStyleMappings.Elements(STYLE_MAPPING_EL).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                XElement mapping = (XElement)list[i];
                // get required string attribute
                string textStyleName = GetAttrValueAndCheckIfNotNull("name",
                                       mapping);
                string boldFontId = null;
                try
                {
                    boldFontId = GetAttrValueAndCheckIfNotNull("bold", mapping);
                }
                catch (ResourceParseException e) { }

                List<XElement> mapRangeList = mapping.Elements("MapRange").ToList();
                // iterate all mapping ranges
                CharFont[] charFonts = new CharFont[4];
                for (int j = 0; j < mapRangeList.Count; j++)
                {
                    XElement mapRange = (XElement)mapRangeList[(j)];
                    // get required integer attributes
                    string fontId = GetAttrValueAndCheckIfNotNull("fontId", mapRange);
                    int ch = GetIntAndCheck("start", mapRange);
                    // get required string attribute and check if it's a known range
                    string code = GetAttrValueAndCheckIfNotNull("code", mapRange);
                    object codeMapping = rangeTypeMappings[(code)];
                    if (codeMapping == null)
                        throw new XMLResourceParseException(RESOURCE_NAME,
                                                            "MapRange", "code",
                                                            "Contains an unknown \"range name\" '" + code + "'!");
                    else if (boldFontId == null)
                        charFonts[((int)codeMapping)] = new CharFont((char)ch, Font_ID.IndexOf(fontId));
                    else charFonts[((int)codeMapping)] = new CharFont((char)ch, Font_ID.IndexOf(fontId), Font_ID.IndexOf(boldFontId));
                }
                res.Add(textStyleName, charFonts);
            }
        }
        return res;
    }

    private static void SetRangeTypeMappings()
    {
        rangeTypeMappings.Add("numbers", DefaultTeXFont.NUMBERS); // autoboxing
        rangeTypeMappings.Add("capitals", DefaultTeXFont.CAPITALS); // autoboxing
        rangeTypeMappings.Add("small", DefaultTeXFont.SMALL); // autoboxing
        rangeTypeMappings.Add("unicode", DefaultTeXFont.UNICODE); // autoboxing
    }

    private static string GetAttrValueAndCheckIfNotNull(string attrName,
            XElement element)
    {
        string attrValue = element.Attribute(attrName)?.Value ?? "";
        if (attrValue == (""))
            throw new XMLResourceParseException(RESOURCE_NAME, element.Name.LocalName,
                                                attrName, null);
        return attrValue;
    }

    public static float GetFloatAndCheck(string attrName, XElement element)
    {
        string attrValue = GetAttrValueAndCheckIfNotNull(attrName, element);

        // try parsing string to float value
        float res = 0;
        try
        {
            res = (float)(double.TryParse(attrValue, out var r) ? r : 0);
        }
        catch (Exception e)
        {
            throw new XMLResourceParseException(RESOURCE_NAME, element.Name.LocalName,
                                                attrName, "has an invalid real value!");
        }
        // parsing OK
        return res;
    }

    public static int GetIntAndCheck(string attrName, XElement element)
    {
        string attrValue = GetAttrValueAndCheckIfNotNull(attrName, element);

        // try parsing string to integer value
        int res = 0;
        try
        {
            res = int.TryParse(attrValue, out var i) ? i : 0;
        }
        catch (Exception e)
        {
            throw new XMLResourceParseException(RESOURCE_NAME, element.Name.LocalName,
                                                attrName, "has an invalid integer value!");
        }
        // parsing OK
        return res;
    }

    public static int GetOptionalInt(string attrName, XElement element,
                                     int defaultValue)
    {
        string attrValue = element.Attribute(attrName)?.Value ?? "";
        if (attrValue == ("")) // attribute not present
            return defaultValue;
        else
        {
            // try parsing string to integer value
            int res = 0;
            try
            {
                res = int.TryParse(attrValue, out var i) ? i : 0;
            }
            catch (Exception e)
            {
                throw new XMLResourceParseException(RESOURCE_NAME, element
                                                    .Name.LocalName, attrName, "has an invalid integer value!");
            }
            // parsing OK
            return res;
        }
    }

    public static float GetOptionalFloat(string attrName, XElement element,
                                         float defaultValue)
    {
        string attrValue = element.Attribute(attrName)?.Value ?? "";
        if (attrValue == ("")) // attribute not present
            return defaultValue;
        else
        {
            // try parsing string to float value
            float res = 0;
            try
            {
                res = (float)(Double.TryParse(attrValue, out var d) ? d : 0);
            }
            catch (Exception e)
            {
                throw new XMLResourceParseException(RESOURCE_NAME, element.Name.LocalName, attrName, "has an invalid float value!");
            }
            // parsing OK
            return res;
        }
    }
}
