/* TeXFormula.java
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

using System.Drawing;

namespace NLaTexMath;


/**
 * Represents a logical mathematical formula that will be displayed (by creating a
 * {@link TeXIcon} from it and painting it) using algorithms that are based on the
 * TeX algorithms.
 * <p>
 * These formula's can be built using the built-in primitive TeX parser
 * (methods with string arguments) or using other TeXFormula objects. Most methods
 * have (an) equivalent(s) where one or more TeXFormula arguments are replaced with
 * string arguments. These are just shorter notations, because all they do is parse
 * the string(s) to TeXFormula's and call an equivalent method with (a) TeXFormula argument(s).
 * Most methods also come in 2 variants. One kind will use this TeXFormula to build
 * another mathematical construction and then change this object to represent the newly
 * build construction. The other kind will only use other
 * TeXFormula's (or parse strings), build a mathematical construction with them and
 * insert this newly build construction at the end of this TeXFormula.
 * Because all the provided methods return a pointer to this (modified) TeXFormula
 * (except for the createTeXIcon method that returns a TeXIcon pointer),
 * method chaining is also possible.
 * <p>
 * <b> Important: All the provided methods modify this TeXFormula object, but all the
 * TeXFormula arguments of these methods will remain unchanged and independent of
 * this TeXFormula object!</b>
 */
public class TeXFormula 
{
    public class FontInfos
    {

        string sansserif;
        string serif;

        FontInfos(string sansserif, string serif)
        {
            this.sansserif = sansserif;
            this.serif = serif;
        }
    }

    public static readonly string VERSION = "1.0.3";

    public const int   SERIF = 0;
    public const int   SANSSERIF = 1;
    public const int   BOLD = 2;
    public const int   ITALIC = 4;
    public const int   ROMAN = 8;
    public const int   TYPEWRITER = 16;

    // point-to-pixel conversion
    public static float PIXELS_PER_POINT = 1f;

    // font scale for deriving
    public static float FONT_SCALE_FACTOR = 100f;

    // for comparing floats with 0
    public static readonly float PREC = 0.0000001f;

    // predefined TeXFormula's
    public static Dictionary<string, TeXFormula> predefinedTeXFormulas = new (150);
    public static Dictionary<string, string> predefinedTeXFormulasAsString = new (150);

    // character-to-symbol and character-to-delimiter mappings
    public static string[] symbolMappings = new string[65536];
    public static string[] symbolTextMappings = new string[65536];
    public static string[] symbolFormulaMappings = new string[65536];
    public static Dictionary<UnicodeBlock, FontInfos> externalFontMap = [];

    public List<MiddleAtom> middle = [];

    public Dictionary<string, string> jlmXMLMap;
    private TeXParser parser;

    static TeXFormula(){
        // character-to-symbol and character-to-delimiter mappings
        TeXFormulaSettingsParser parser = new TeXFormulaSettingsParser();
        parser.parseSymbolMappings(symbolMappings, symbolTextMappings);

        new PredefinedCommands();
        new PredefinedTeXFormulas();
        new PredefMacros();

        parser.parseSymbolToFormulaMappings(symbolFormulaMappings, symbolTextMappings);

        try {
            DefaultTeXFont.registerAlphabet((AlphabetRegistration) Type.forName("NLaTexMath.cyrillic.CyrillicRegistration").newInstance());
            DefaultTeXFont.registerAlphabet((AlphabetRegistration) Type.forName("NLaTexMath.greek.GreekRegistration").newInstance());
        } catch (Exception e) { }

        //setDefaultDPI();
    }

    public static void addSymbolMappings(string file){
        FileInputStream _in;
        try {
            _in = new FileInputStream(file);
        } catch (FileNotFoundException e) {
            throw new ResourceParseException(file, e);
        }
        addSymbolMappings(_in, file);
    }

    public static void addSymbolMappings(Stream _in, string name){
        TeXFormulaSettingsParser tfsp = new TeXFormulaSettingsParser(_in, name);
        tfsp.parseSymbolMappings(symbolMappings, symbolTextMappings);
        tfsp.parseSymbolToFormulaMappings(symbolFormulaMappings, symbolTextMappings);
    }

    public static bool isRegisteredBlock(UnicodeBlock block) {
        return externalFontMap[(block)] != null;
    }

    public static FontInfos getExternalFont(UnicodeBlock block) {
        FontInfos infos = externalFontMap[(block)];
        if (infos == null) {
            infos = new FontInfos("SansSerif", "Serif");
            externalFontMap.Add(block, infos);
        }

        return infos;
    }

    public static void registerExternalFont(UnicodeBlock block, string sansserif, string serif) {
        if (sansserif == null && serif == null) {
            externalFontMap.Remove(block);
            return;
        }
        externalFontMap.Add(block, new FontInfos(sansserif, serif));
        if (block==(UnicodeBlock.BASIC_LATIN)) {
            predefinedTeXFormulas.Clear();
        }
    }

    public static void registerExternalFont(UnicodeBlock block, string fontName) {
        registerExternalFont(block, fontName, fontName);
    }

    /**
     * Set the DPI of target
     * @param dpi the target DPI
     */
    public static void setDPITarget(float dpi) {
        PIXELS_PER_POINT = dpi / 72f;
    }

    /**
     * Set the default target DPI to the screen dpi (only if we're in non-headless mode)
     */
    public static void setDefaultDPI() {
        if (!GraphicsEnvironment.isHeadless()) {
            setDPITarget((float) Toolkit.getDefaultToolkit().getScreenResolution());
        }
    }

    // the root atom of the "atom tree" that represents the formula
    public Atom root = null;

    // the current text style
    public string textStyle = null;

    public bool isColored = false;

    /**
     * Creates an empty TeXFormula.
     *
     */
    public TeXFormula() {
        parser = new TeXParser("", this, false);
    }

    /**
     * Creates a new TeXFormula by parsing the given string (using a primitive TeX parser).
     *
     * @param s the string to be parsed
     * @ if the string could not be parsed correctly
     */
    public TeXFormula(string s, Dictionary<string, string> map)  {
        this.jlmXMLMap = map;
        parser = new TeXParser(s, this);
        parser.parse();
    }

    /**
     * Creates a new TeXFormula by parsing the given string (using a primitive TeX parser).
     *
     * @param s the string to be parsed
     * @ if the string could not be parsed correctly
     */
    public TeXFormula(string s) : this(s, (string)null)
    {
        ;
    }

    public TeXFormula(string s, bool firstpass)  {
        this.textStyle = null;
        parser = new TeXParser(s, this, firstpass);
        parser.parse();
    }

    /*
     * Creates a TeXFormula by parsing the given string in the given text style.
     * Used when a text style command was found in the parse string.
     */
    public TeXFormula(string s, string textStyle)  {
        this.textStyle = textStyle;
        parser = new TeXParser(s, this);
        parser.parse();
    }

    public TeXFormula(string s, string textStyle, bool firstpass, bool space)  {
        this.textStyle = textStyle;
        parser = new TeXParser(s, this, firstpass, space);
        parser.parse();
    }

    /**
     * Creates a new TeXFormula that is a copy of the given TeXFormula.
     * <p>
     * <b>Both TeXFormula's are independent of one another!</b>
     *
     * @param f the formula to be copied
     */
    public TeXFormula(TeXFormula f) {
        if (f != null) {
            addImpl(f);
        }
    }

    /**
     * Creates an empty TeXFormula.
     *
     */
    public TeXFormula(TeXParser tp) {
        this.jlmXMLMap = tp.formula.jlmXMLMap;
        parser = new TeXParser(tp.getIsPartial(), "", this, false);
    }

    /**
     * Creates a new TeXFormula by parsing the given string (using a primitive TeX parser).
     *
     * @param s the string to be parsed
     * @ if the string could not be parsed correctly
     */
    public TeXFormula(TeXParser tp, string s)  : this(tp, s, null)
    {
        ;
    }

    public TeXFormula(TeXParser tp, string s, bool firstpass)  {
        this.textStyle = null;
        this.jlmXMLMap = tp.formula.jlmXMLMap;
        bool isPartial = tp.getIsPartial();
        parser = new TeXParser(isPartial, s, this, firstpass);
        if (isPartial) {
            try {
                parser.parse();
            } catch (Exception e) { }
        } else {
            parser.parse();
        }
    }

    /*
     * Creates a TeXFormula by parsing the given string in the given text style.
     * Used when a text style command was found in the parse string.
     */
    public TeXFormula(TeXParser tp, string s, string textStyle)  {
        this.textStyle = textStyle;
        this.jlmXMLMap = tp.formula.jlmXMLMap;
        bool isPartial = tp.getIsPartial();
        parser = new TeXParser(isPartial, s, this);
        if (isPartial) {
            try {
                parser.parse();
            } catch (Exception e) {
                if (root == null) {
                    root = new EmptyAtom();
                }
            }
        } else {
            parser.parse();
        }
    }

    public TeXFormula(TeXParser tp, string s, string textStyle, bool firstpass, bool space)  {
        this.textStyle = textStyle;
        this.jlmXMLMap = tp.formula.jlmXMLMap;
        bool isPartial = tp.getIsPartial();
        parser = new TeXParser(isPartial, s, this, firstpass, space);
        if (isPartial) {
            try {
                parser.parse();
            } catch (Exception e) {
                if (root == null) {
                    root = new EmptyAtom();
                }
            }
        } else {
            parser.parse();
        }
    }

    public static TeXFormula getAsText(string text, int alignment)  {
        TeXFormula formula = new TeXFormula();
        if (text == null || ""==(text)) {
            formula.Add(new EmptyAtom());
            return formula;
        }

        string[] arr = text.split("\n|\\\\\\\\|\\\\cr");
        ArrayOfAtoms atoms = new ArrayOfAtoms();
        foreach (string s in arr) {
            TeXFormula f = new TeXFormula(s, "mathnormal", true, false);
            atoms.Add(new RomanAtom(f.root));
            atoms.AddRow();
        }
        atoms.CheckDimensions();
        formula.Add(new MatrixAtom(false, atoms, MatrixAtom.ARRAY, alignment));

        return formula;
    }

    /**
     * @param formula a formula
     * @return a partial TeXFormula containing the valid part of formula
     */
    public static TeXFormula getPartialTeXFormula(string formula) {
        TeXFormula f = new TeXFormula();
        if (formula == null) {
            f.Add(new EmptyAtom());
            return f;
        }
        TeXParser parser = new TeXParser(true, formula, f);
        try {
            parser.parse();
        } catch (Exception e) {
            if (f.root == null) {
                f.root = new EmptyAtom();
            }
        }

        return f;
    }

    /**
     * @param b true if the fonts should be registered (Java 1.6 only) to be used
     * with FOP.
     */
    public static void registerFonts(bool b) {
        DefaultTeXFontParser.registerFonts(b);
    }

    /**
     * Change the text of the TeXFormula and regenerate the root
     *
     * @param ltx the latex formula
     */
    public void setLaTeX(string ltx)  {
        parser.reset(ltx);
        if (ltx != null && ltx.Length != 0)
            parser.parse();
    }

    /**
     * Inserts an atom at the end of the current formula
     */
    public TeXFormula Add(Atom el) {
        if (el != null) {
            if (el is MiddleAtom)
                middle.Add((MiddleAtom) el);
            if (root == null) {
                root = el;
            } else {
                if (!(root is RowAtom)) {
                    root = new RowAtom(root);
                }
                ((RowAtom) root).Add(el);
                if (el is TypedAtom) {
                    TypedAtom ta = (TypedAtom) el;
                    int rtype = ta.RightType;
                    if (rtype == TeXConstants.TYPE_BINARY_OPERATOR || rtype == TeXConstants.TYPE_RELATION) {
                        ((RowAtom) root).Add(new BreakMarkAtom());
                    }
                }
            }
        }
        return this;
    }

    /**
     * Parses the given string and inserts the resulting formula
     * at the end of the current TeXFormula.
     *
     * @param s the string to be parsed and inserted
     * @ if the string could not be parsed correctly
     * @return the modified TeXFormula
     */
    public TeXFormula Add(string s)  {
        if (s != null && s.Length != 0) {
            // reset parsing variables
            textStyle = null;
            // parse and Add the string
            Add(new TeXFormula(s));
        }
        return this;
    }

    public TeXFormula Append(string s)  {
        return Append(false, s);
    }

    public TeXFormula Append(bool isPartial, string s)  {
        if (s != null && s.Length != 0) {
            TeXParser tp = new TeXParser(isPartial, s, this);
            tp.parse();
        }
        return this;
    }

    /**
     * Inserts the given TeXFormula at the end of the current TeXFormula.
     *
     * @param f the TeXFormula to be inserted
     * @return the modified TeXFormula
     */
    public TeXFormula Add(TeXFormula f) {
        addImpl(f);
        return this;
    }

    private void addImpl(TeXFormula f) {
        if (f.root != null) {
            // special copy-treatment for Mrow as a root!!
            if (f.root is RowAtom)
                Add(new RowAtom(f.root));
            else
                Add(f.root);
        }
    }

    public void setLookAtLastAtom(bool b) {
        if (root is RowAtom)
            ((RowAtom)root).lookAtLastAtom = b;
    }

    public bool getLookAtLastAtom() {
        if (root is RowAtom)
            return ((RowAtom)root).lookAtLastAtom;
        return false;
    }

    /**
     * Centers the current TeXformula vertically on the axis (defined by the parameter
     * "axisheight" in the resource "DefaultTeXFont.xml".
     *
     * @return the modified TeXFormula
     */
    public TeXFormula centerOnAxis() {
        root = new VCenteredAtom(root);
        return this;
    }

    public static void addPredefinedTeXFormula(Stream xmlFile){
        new PredefinedTeXFormulaParser(xmlFile, "TeXFormula").Parse(predefinedTeXFormulas);
    }

    public static void addPredefinedCommands(Stream xmlFile){
        new PredefinedTeXFormulaParser(xmlFile, "Command").parse(MacroInfo.Commands);
    }

    /**
     * Inserts a strut box (whitespace) with the given width, height and depth (in
     * the given unit) at the end of the current TeXFormula.
     *
     * @param unit a unit constant (from {@link TeXConstants})
     * @param width the width of the strut box
     * @param height the height of the strut box
     * @param depth the depth of the strut box
     * @return the modified TeXFormula
     * @throws InvalidUnitException if the given integer value does not represent
     *                  a valid unit
     */
    public TeXFormula addStrut(int unit, float width, float height, float depth)
   {
        return Add(new SpaceAtom(unit, width, height, depth));
    }

    /**
     * Inserts a strut box (whitespace) with the given width, height and depth (in
     * the given unit) at the end of the current TeXFormula.
     *
     * @param type thinmuskip, medmuskip or thickmuskip (from {@link TeXConstants})
     * @return the modified TeXFormula
     * @throws InvalidUnitException if the given integer value does not represent
     *                  a valid unit
     */
    public TeXFormula addStrut(int type)
   {
        return Add(new SpaceAtom(type));
    }

    /**
     * Inserts a strut box (whitespace) with the given width (in widthUnits), height
     * (in heightUnits) and depth (in depthUnits) at the end of the current TeXFormula.
     *
     * @param widthUnit a unit constant used for the width (from {@link TeXConstants})
     * @param width the width of the strut box
     * @param heightUnit a unit constant used for the height (from TeXConstants)
     * @param height the height of the strut box
     * @param depthUnit a unit constant used for the depth (from TeXConstants)
     * @param depth the depth of the strut box
     * @return the modified TeXFormula
     * @throws InvalidUnitException if the given integer value does not represent
     *                  a valid unit
     */
    public TeXFormula addStrut(int widthUnit, float width, int heightUnit,
                               float height, int depthUnit, float depth){
        return Add(new SpaceAtom(widthUnit, width, heightUnit, height, depthUnit,
                                 depth));
    }

    /*
     * Convert this TeXFormula into a box, starting form the given style
     */
    private Box createBox(TeXEnvironment style) {
        if (root == null)
            return new StrutBox(0, 0, 0, 0);
        else
            return root.CreateBox(style);
    }

    private DefaultTeXFont createFont(float size, int type) {
        DefaultTeXFont dtf = new DefaultTeXFont(size);
        if (type == 0) {
            dtf.setSs(false);
        }
        if ((type & ROMAN) != 0) {
            dtf.setRoman(true);
        }
        if ((type & TYPEWRITER) != 0) {
            dtf.setTt(true);
        }
        if ((type & SANSSERIF) != 0) {
            dtf.setSs(true);
        }
        if ((type & ITALIC) != 0) {
            dtf.setIt(true);
        }
        if ((type & BOLD) != 0) {
            dtf.setBold(true);
        }

        return dtf;
    }

    /**
     * Apply the Builder pattern instead of using the createTeXIcon(...) factories
     * @author Felix Natter
     *
     */
    public class TeXIconBuilder {
        private int style;
        private float size;
        private int type;
        private Color fgcolor;
        private bool trueValues = false;
        private int widthUnit;
        private float textWidth;
        private int align;
        private bool isMaxWidth = false;
        private int interLineUnit;
        private float interLineSpacing;

        /**
         * Specify the style for rendering the given TeXFormula
         * @param style the style
         * @return the builder, used for chaining
         */
        public TeXIconBuilder setStyle(int style) {
            this.style = style;
            return this;
        }

        /**
         * Specify the font size for rendering the given TeXFormula
         * @param size the size
         * @return the builder, used for chaining
         */
        public TeXIconBuilder setSize( float size) {
            this.size = size;
            return this;
        }

        /**
         * Specify the font type for rendering the given TeXFormula
         * @param type the font type
         * @return the builder, used for chaining
         */
        public TeXIconBuilder setType( int type) {
            this.type = type;
            return this;
        }

        /**
         * Specify the background color for rendering the given TeXFormula
         * @param fgcolor the foreground color
         * @return the builder, used for chaining
         */
        public TeXIconBuilder setFGColor( Color fgcolor) {
            this.fgcolor = fgcolor;
            return this;
        }

        /**
         * Specify the "true values" parameter for rendering the given TeXFormula
         * @param trueValues the "true values" value
         * @return the builder, used for chaining
         */
        public TeXIconBuilder setTrueValues( bool trueValues) {
            this.trueValues = trueValues;
            return this;
        }

        /**
         * Specify the width of the formula (may be exact or maximum width, see {@link #setIsMaxWidth(boolean)})
         * @param widthUnit the width unit
         * @param textWidth the width
         * @param align the alignment
         * @return the builder, used for chaining
         */
        public TeXIconBuilder setWidth( int widthUnit,  float textWidth,  int align) {
            this.widthUnit = widthUnit;
            this.textWidth = textWidth;
            this.align = align;
            trueValues = true; // TODO: is this necessary?
            return this;
        }

        /**
         * Specifies whether the width is the exact or the maximum width
         * @param isMaxWidth whether the width is a maximum width
         * @return the builder, used for chaining
         */
        public TeXIconBuilder setIsMaxWidth( bool isMaxWidth) {
            if (widthUnit == null) {
                throw new Exception("Cannot set 'isMaxWidth' without having specified a width!");
            }
            if (isMaxWidth) {
                // NOTE: Currently isMaxWidth==true does not work with ALIGN_CENTER or ALIGN_RIGHT (see HorizontalBox ctor)
                // The case (1) we don't support by setting align := ALIGN_LEFT here is this:
                //  \text{hello world\\hello} with align=ALIGN_CENTER (but forced to ALIGN_LEFT) and isMaxWidth==true results in:
                // [hello world]
                // [hello      ]
                // and NOT:
                // [hello world]
                // [   hello   ]
                // However, this case (2) is currently not supported anyway (ALIGN_CENTER with isMaxWidth==false):
                // [  hello world  ]
                // [  hello        ]
                // and NOT:
                // [  hello world  ]
                // [     hello     ]
                // => until (2) is solved, we stick with the hack to set align := ALIGN_LEFT!
                this.align = TeXConstants.ALIGN_LEFT;
            }
            this.isMaxWidth = isMaxWidth;
            return this;
        }

        /**
         * Specify the inter line spacing unit and value. NOTE: this is required for automatic linebreaks to work!
         * @param interLineUnit the unit
         * @param interLineSpacing the value
         * @return the builder, used for chaining
         */
        public TeXIconBuilder setInterLineSpacing( int interLineUnit,  float interLineSpacing) {
            if (widthUnit == null) {
                throw new Exception("Cannot set inter line spacing without having specified a width!");
            }
            this.interLineUnit = interLineUnit;
            this.interLineSpacing = interLineSpacing;
            return this;
        }

        /**
         * Create a TeXIcon from the information gathered by the (chained) setXXX() methods.
         * (see Builder pattern)
         * @return the TeXIcon
         */
        public TeXIcon build() {
            if (style == null) {
                throw new Exception("A style is required. Use setStyle()");
            }
            if (size == null) {
                throw new Exception("A size is required. Use setStyle()");
            }
            DefaultTeXFont font = (type == null) ? new DefaultTeXFont(size) : createFont(size, type);
            TeXEnvironment te;
            if (widthUnit != null) {
                te = new TeXEnvironment(style, font, widthUnit, textWidth);
            } else {
                te = new TeXEnvironment(style, font);
            }

            if (interLineUnit != null) {
                te.SetInterline(interLineUnit, interLineSpacing);
            }

            Box box = createBox(te);
            TeXIcon ti;
            if (widthUnit != null) {
                HorizontalBox hb;
                if (interLineUnit != null) {
                    float il = interLineSpacing * SpaceAtom.getFactor(interLineUnit, te);
                    Box b = BreakFormula.split(box, te.GetTextwidth(), il);
                    hb = new HorizontalBox(b, isMaxWidth ? b.Width : te.GetTextwidth(), align);
                } else {
                    hb = new HorizontalBox(box, isMaxWidth ? box.Width : te.GetTextwidth(), align);
                }
                ti = new TeXIcon(hb, size, trueValues);
            } else {
                ti = new TeXIcon(box, size, trueValues);
            }
            if (fgcolor != null) {
                ti.setForeground(fgcolor);
            }
            ti.isColored = te.isColored;
            return ti;
        }
    }

    /**
     * Creates a TeXIcon from this TeXFormula using the default TeXFont in the given
     * point size and starting from the given TeX style. If the given integer value
     * does not represent a valid TeX style, the default style
     * TeXConstants.STYLE_DISPLAY will be used.
     *
     * @param style a TeX style constant (from {@link TeXConstants}) to start from
     * @param size the default TeXFont's point size
     * @return the created TeXIcon
     */
    public TeXIcon CreateTeXIcon(int style, float size) {
        return new TeXIconBuilder().setStyle(style).setSize(size).build();
    }

    public TeXIcon CreateTeXIcon(int style, float size, int type) {
        return new TeXIconBuilder().setStyle(style).setSize(size).setType(type).build();
    }

    public TeXIcon CreateTeXIcon(int style, float size, int type, Color fgcolor) {
        return new TeXIconBuilder().setStyle(style).setSize(size).setType(type).setFGColor(fgcolor).build();
    }

    public TeXIcon CreateTeXIcon(int style, float size, bool trueValues) {
        return new TeXIconBuilder().setStyle(style).setSize(size).setTrueValues(trueValues).build();
    }

    public TeXIcon CreateTeXIcon(int style, float size, int widthUnit, float textwidth, int align) {
        return createTeXIcon(style, size, 0, widthUnit, textwidth, align);
    }

    public TeXIcon createTeXIcon(int style, float size, int type, int widthUnit, float textwidth, int align) {
        return new TeXIconBuilder().setStyle(style).setSize(size).setType(type).setWidth(widthUnit, textwidth, align).build();
    }

    public TeXIcon CreateTeXIcon(int style, float size, int widthUnit, float textwidth, int align, int interlineUnit, float interline) {
        return CreateTeXIcon(style, size, 0, widthUnit, textwidth, align, interlineUnit, interline);
    }

    public TeXIcon CreateTeXIcon(int style, float size, int type, int widthUnit, float textwidth, int align, int interlineUnit, float interline) {
        return new TeXIconBuilder().setStyle(style).setSize(size).setType(type).setWidth(widthUnit, textwidth, align).setInterLineSpacing(interlineUnit, interline).build();
    }

    public void CreateImage(string format, int style, float size, string _out, Color bg, Color fg, bool transparency) {
        TeXIcon icon = CreateTeXIcon(style, size);
        icon.setInsets(new Insets(1, 1, 1, 1));
        int w = icon.getIconWidth(), h = icon.getIconHeight();

        Bitmap image = new Bitmap(w, h, transparency ? Bitmap.TYPE_INT_ARGB : Bitmap.TYPE_INT_RGB);
        Graphics g2 = image.createGraphics();
        if (bg != null && !transparency) {
            g2.setColor(bg);
            g2.fillRect(0, 0, w, h);
        }

        icon.setForeground(fg);
        icon.paintIcon(null, g2, 0, 0);
        try {
            FileImageOutputStream imout = new FileImageOutputStream(new File(_out));
            ImageIO.write(image, format, imout);
            imout.flush();
            imout.close();
        } catch (IOException ex) {
            Console.Error.WriteLine("I/O error : Cannot generate " + _out);
        }

        g2.dispose();
    }

    public void CreatePNG(int style, float size, string _out, Color bg, Color fg) {
        CreateImage("png", style, size, _out, bg, fg, bg == null);
    }

    public void CreateGIF(int style, float size, string _out, Color bg, Color fg) {
        CreateImage("gif", style, size, _out, bg, fg, bg == null);
    }

    public void CreateJPEG(int style, float size, string _out, Color bg, Color fg) {
        //There is a bug when a Bitmap has a component alpha so we disabel it
        CreateImage("jpeg", style, size, _out, bg, fg, false);
    }

    /**
     * @param formula the formula
     * @param style the style
     * @param size the size
     * @param fg the foreground color
     * @param bg the background color
     * @return the generated image
     */
    public static Image CreateBufferedImage(string formula, int style, float size, Color fg, Color bg)  {
        TeXFormula f = new TeXFormula(formula);
        TeXIcon icon = f.CreateTeXIcon(style, size);
        icon.setInsets(new Insets(2, 2, 2, 2));
        int w = icon.getIconWidth(), h = icon.getIconHeight();

        Bitmap image = new Bitmap(w, h, bg == null ? Bitmap.TYPE_INT_ARGB : Bitmap.TYPE_INT_RGB);
        Graphics g2 = image.createGraphics();
        if (bg != null) {
            g2.setColor(bg);
            g2.fillRect(0, 0, w, h);
        }

        icon.setForeground(fg == null ? Color.Black : fg);
        icon.paintIcon(null, g2, 0, 0);
        g2.dispose();

        return image;
    }

    /**
     * @param style the style
     * @param size the size
     * @param fg the foreground color
     * @param bg the background color
     * @return the generated image
     */
    public Image CreateBufferedImage(int style, float size, Color fg, Color bg)  {
        TeXIcon icon = CreateTeXIcon(style, size);
        icon.setInsets(new Insets(2, 2, 2, 2));
        int w = icon.getIconWidth(), h = icon.getIconHeight();

        Bitmap image = new Bitmap(w, h, bg == null ? Bitmap.TYPE_INT_ARGB : Bitmap.TYPE_INT_RGB);
        Graphics g2 = image.createGraphics();
        if (bg != null) {
            g2.setColor(bg);
            g2.fillRect(0, 0, w, h);
        }

        icon.setForeground(fg == null ? Color.Black : fg);
        icon.paintIcon(null, g2, 0, 0);
        g2.dispose();

        return image;
    }

    public void setDEBUG(bool b) {
        Box.DEBUG = b;
    }

    /**
     * Changes the background color of the <i>current</i> TeXFormula into the given color.
     * By default, a TeXFormula has no background color, it's transparent.
     * The backgrounds of subformula's will be painted on top of the background of
     * the whole formula! Any changes that will be made to this TeXFormula after this
     * background color was set, will have the default background color (unless it will
     * also be changed into another color afterwards)!
     *
     * @param c the desired background color for the <i>current</i> TeXFormula
     * @return the modified TeXFormula
     */
    public TeXFormula setBackground(Color c) {
        if (c != null) {
            if (root is ColorAtom)
                root = new ColorAtom(c, null, (ColorAtom) root);
            else
                root = new ColorAtom(root, c, null);
        }
        return this;
    }

    /**
     * Changes the (foreground) color of the <i>current</i> TeXFormula into the given color.
     * By default, the foreground color of a TeXFormula is the foreground color of the
     * component on which the TeXIcon (created from this TeXFormula) will be painted. The
     * color of subformula's overrides the color of the whole formula.
     * Any changes that will be made to this TeXFormula after this color was set, will be
     * painted in the default color (unless the color will also be changed afterwards into
     * another color)!
     *
     * @param c the desired foreground color for the <i>current</i> TeXFormula
     * @return the modified TeXFormula
     */
    public TeXFormula setColor(Color c) {
        if (c != null) {
            if (root is ColorAtom)
                root = new ColorAtom(null, c, (ColorAtom) root);
            else
                root = new ColorAtom(root, null, c);
        }
        return this;
    }

    /**
     * Sets a fixed left and right type of the current TeXFormula. This has an influence
     * on the glue that will be inserted before and after this TeXFormula.
     *
     * @param leftType atom type constant (from {@link TeXConstants})
     * @param rightType atom type constant (from TeXConstants)
     * @return the modified TeXFormula
     * @ if the given integer value does not represent
     *                  a valid atom type
     */
    public TeXFormula setFixedTypes(int leftType, int rightType)
     {
        root = new TypedAtom(leftType, rightType, root);
        return this;
    }

    /**
     * Get a predefined TeXFormula.
     *
     * @param name the name of the predefined TeXFormula
     * @return a copy of the predefined TeXFormula
     * @ if no predefined TeXFormula is found with the
     *                  given name
     */
    public static TeXFormula Get(string name)  {
        TeXFormula formula = predefinedTeXFormulas[name];
        if (formula == null) {
            string f = predefinedTeXFormulasAsString[name];
            if (f == null) {
                throw new FormulaNotFoundException(name);
            }
            TeXFormula tf = new TeXFormula(f);
            if (tf.root is not RowAtom) {
                // depending of the context a RowAtom can be modified
                // so we can't reuse it
                predefinedTeXFormulas.Add(name, tf);
            }
            return tf;
        } else {
            return new TeXFormula(formula);
        }
    }

}
