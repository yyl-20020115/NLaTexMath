/* TeXParser.java
 * =========================================================================
 * This file is part of the JLaTeXMath Library - http://forge.scilab.org/p/jlatexmath
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

using NLaTexMath.Internal.util;
using System.Drawing;
using System.Text;

namespace NLaTexMath;

/**
 * This class : a parser for LaTeX' formulas.
 */
public class TeXParser
{

    public TeXFormula formula;

    private StringBuilder parseString;
    private int pos;
    private int spos;
    private int line;
    private int col;
    private int len;
    private int group;
    private bool insertion;
    private int atIsLetter;
    private bool arrayMode;
    private bool ignoreWhiteSpace = true;
    private bool isPartial;

    // the escape character
    private const char ESCAPE = '\\';

    // grouping characters (for parsing)
    private const char L_GROUP = '{';
    private const char R_GROUP = '}';
    private const char L_BRACK = '[';
    private const char R_BRACK = ']';
    private const char DOLLAR = '$';
    private const char DQUOTE = '\"';

    // Percent char for comments
    private const char PERCENT = '%';

    // script characters (for parsing)
    private const char SUB_SCRIPT = '_';
    private const char SUPER_SCRIPT = '^';
    private const char PRIME = '\'';
    private const char BACKPRIME = '\u2035';
    private const char DEGRE = '\u00B0';
    private const char SUPZERO = '\u2070';
    private const char SUPONE = '\u00B9';
    private const char SUPTWO = '\u00B2';
    private const char SUPTHREE = '\u00B3';
    private const char SUPFOUR = '\u2074';
    private const char SUPFIVE = '\u2075';
    private const char SUPSIX = '\u2076';
    private const char SUPSEVEN = '\u2077';
    private const char SUPEIGHT = '\u2078';
    private const char SUPNINE = '\u2079';
    private const char SUPPLUS = '\u207A';
    private const char SUPMINUS = '\u207B';
    private const char SUPEQUAL = '\u207C';
    private const char SUPLPAR = '\u207D';
    private const char SUPRPAR = '\u207E';
    private const char SUPN = '\u207F';
    private const char SUBZERO = '\u2080';
    private const char SUBONE = '\u2081';
    private const char SUBTWO = '\u2082';
    private const char SUBTHREE = '\u2083';
    private const char SUBFOUR = '\u2084';
    private const char SUBFIVE = '\u2085';
    private const char SUBSIX = '\u2086';
    private const char SUBSEVEN = '\u2087';
    private const char SUBEIGHT = '\u2088';
    private const char SUBNINE = '\u2089';
    private const char SUBPLUS = '\u208A';
    private const char SUBMINUS = '\u208B';
    private const char SUBEQUAL = '\u208C';
    private const char SUBLPAR = '\u208D';
    private const char SUBRPAR = '\u208E';

    public static bool isLoading = false;

    private static readonly HashSet<string> unparsedContents = new(6);
    static TeXParser()
    {
        unparsedContents.Add("jlmDynamic");
        unparsedContents.Add("jlmText");
        unparsedContents.Add("jlmTextit");
        unparsedContents.Add("jlmTextbf");
        unparsedContents.Add("jlmTextitbf");
        unparsedContents.Add("jlmExternalFont");
    }

    /**
     * Create a new TeXParser
     *
     * @param parseString the string to be parsed
     * @param formula the formula where to put the atoms
     * @ if the string could not be parsed correctly
     */
    public TeXParser(string parseString, TeXFormula formula) : this(parseString, formula, true)
    {
    }

    /**
     * Create a new TeXParser
     *
     * @param isPartial if true certains exceptions are not thrown
     * @param parseString the string to be parsed
     * @param formula the formula where to put the atoms
     * @ if the string could not be parsed correctly
     */
    public TeXParser(bool isPartial, string parseString, TeXFormula formula)
    : this(parseString, formula, false)
    {
        this.isPartial = isPartial;
        Firstpass();
    }

    /**
     * Create a new TeXParser with or without a first pass
     *
     * @param isPartial if true certains exceptions are not thrown
     * @param parseString the string to be parsed
     * @param firstpass a bool to indicate if the parser must Replace the user-defined macros by their content
     * @ if the string could not be parsed correctly
     */
    public TeXParser(bool isPartial, string parseString, TeXFormula formula, bool _firstpass)
    {
        this.formula = formula;
        this.isPartial = isPartial;
        if (parseString != null)
        {
            this.parseString = new StringBuilder(parseString);
            this.len = parseString.Length;
            this.pos = 0;
            if (_firstpass)
            {
                Firstpass();
            }
        }
        else
        {
            this.parseString = null;
            this.pos = 0;
            this.len = 0;
        }
    }

    /**
     * Create a new TeXParser with or without a first pass
     *
     * @param parseString the string to be parsed
     * @param firstpass a bool to indicate if the parser must Replace the user-defined macros by their content
     * @ if the string could not be parsed correctly
     */
    public TeXParser(string parseString, TeXFormula formula, bool firstpass)
    : this(false, parseString, formula, firstpass)
    {
    }

    /**
     * Create a new TeXParser in the context of an array. When the parser meets a &amp; a new atom is added in the current line and when a \\ is met, a new line is created.
     *
     * @param isPartial if true certains exceptions are not thrown
     * @param parseString the string to be parsed
     * @param aoa an ArrayOfAtoms where to put the elements
     * @param firstpass a bool to indicate if the parser must Replace the user-defined macros by their content
     * @ if the string could not be parsed correctly
     */
    public TeXParser(bool isPartial, string parseString, ArrayOfAtoms aoa, bool firstpass)
    : this(isPartial, parseString, (TeXFormula)aoa, firstpass)
    {
        arrayMode = true;
    }

    /**
     * Create a new TeXParser in the context of an array. When the parser meets a &amp; a new atom is added in the current line and when a \\ is met, a new line is created.
     *
     * @param isPartial if true certains exceptions are not thrown
     * @param parseString the string to be parsed
     * @param aoa an ArrayOfAtoms where to put the elements
     * @param firstpass a bool to indicate if the parser must Replace the user-defined macros by their content
     * @ if the string could not be parsed correctly
     */
    public TeXParser(bool isPartial, string parseString, ArrayOfAtoms aoa, bool firstpass, bool space)
    : this(isPartial, parseString, (TeXFormula)aoa, firstpass, space)
    {
        arrayMode = true;
    }

    /**
     * Create a new TeXParser in the context of an array. When the parser meets a &amp; a new atom is added in the current line and when a \\ is met, a new line is created.
     *
     * @param parseString the string to be parsed
     * @param aoa an ArrayOfAtoms where to put the elements
     * @param firstpass a bool to indicate if the parser must Replace the user-defined macros by their content
     * @ if the string could not be parsed correctly
     */
    public TeXParser(string parseString, ArrayOfAtoms aoa, bool firstpass)
    : this(false, parseString, (TeXFormula)aoa, firstpass)
    {
    }

    /**
     * Create a new TeXParser which ignores or not the white spaces, it's useful for mbox command
     *
     * @param isPartial if true certains exceptions are not thrown
     * @param parseString the string to be parsed
     * @param firstpass a bool to indicate if the parser must Replace the user-defined macros by their content
     * @param space a bool to indicate if the parser must ignore or not the white space
     * @ if the string could not be parsed correctly
     */
    public TeXParser(bool isPartial, string parseString, TeXFormula formula, bool firstpass, bool space)
    : this(isPartial, parseString, formula, firstpass)
    {
        this.ignoreWhiteSpace = space;
    }

    /**
     * Create a new TeXParser which ignores or not the white spaces, it's useful for mbox command
     *
     * @param parseString the string to be parsed
     * @param firstpass a bool to indicate if the parser must Replace the user-defined macros by their content
     * @param space a bool to indicate if the parser must ignore or not the white space
     * @ if the string could not be parsed correctly
     */
    public TeXParser(string parseString, TeXFormula formula, bool firstpass, bool space)
    : this(false, parseString, formula, firstpass)
    {
        this.ignoreWhiteSpace = space;
    }

    /**
     * Reset the parser with a new latex expression
     */
    public void Reset(string latex)
    {
        parseString = new(latex);
        len = parseString.Length;
        formula.root = null;
        pos = 0;
        spos = 0;
        line = 0;
        col = 0;
        group = 0;
        insertion = false;
        atIsLetter = 0;
        arrayMode = false;
        ignoreWhiteSpace = true;
        Firstpass();
    }

    /** Return true if we get a partial formula
     */
    public bool IsPartial => isPartial;

    /** Get the number of the current line
     */
    public int Line => line;

    /** Get the number of the current column
     */
    public int Col => pos - col - 1;

    /** Get the last atom of the current formula
     */
    public Atom GetLastAtom()
    {
        Atom at = formula.root;
        if (at is RowAtom atom)
            return atom.GetLastAtom();
        formula.root = null;
        return at;
    }

    /** Get the atom represented by the current formula
     */
    public Atom GetFormulaAtom()
    {
        Atom at = formula.root;
        formula.root = null;
        return at;
    }

    /** Put an atom in the current formula
     */
    public void AddAtom(Atom at)
    {
        formula.Add(at);
    }

    /** Indicate if the character @ can be used in the command's name
     */
    public void MakeAtLetter()
    {
        atIsLetter++;
    }

    /** Indicate if the character @ can be used in the command's name
     */
    public void MakeAtOther()
    {
        atIsLetter--;
    }

    /** Return a bool indicating if the character @ is considered as a letter or not
     */
    public bool IsAtLetter()
    {
        return (atIsLetter != 0);
    }

    /** Return a bool indicating if the parser is used to parse an array or not
     */
    public bool IsArrayMode()
    {
        return arrayMode;
    }

    public void SetArrayMode(bool arrayMode)
    {
        this.arrayMode = arrayMode;
    }

    /** Return a bool indicating if the parser must ignore white spaces
     */
    public bool IsIgnoreWhiteSpace()
    {
        return ignoreWhiteSpace;
    }

    /** Return a bool indicating if the parser is in math mode
     */
    public bool IsMathMode()
    {
        return ignoreWhiteSpace;
    }

    /** Return the current position in the parsed string
     */
    public int GetPos()
    {
        return pos;
    }

    /** Rewind the current parsed string
     * @param n the number of character to be rewinded
     * @return the new position in the parsed string
     */
    public int Rewind(int n)
    {
        pos -= n;
        return pos;
    }

    public string GetStringFromCurrentPos()
    {
        return parseString.ToString()[pos..];
    }

    public void Finish()
    {
        pos = parseString.Length;
    }

    /** Add a new row when the parser is in array mode
     * @ if the parser is not in array mode
     */
    public void AddRow()
    {
        if (!arrayMode)
            throw new ParseException("You can Add a row only in array mode !");
        ((ArrayOfAtoms)formula).AddRow();
    }

    private void Firstpass()
    {
        if (len != 0)
        {
            char ch;
            string com;
            int spos;
            string[] args;
            MacroInfo mac;
            while (pos < len)
            {
                ch = parseString[pos];
                switch (ch)
                {
                    case ESCAPE:
                        spos = pos;
                        com = getCommand();
                        if ("newcommand" == (com) || "renewcommand" == (com))
                        {
                            args = getOptsArgs(2, 2);
                            mac = MacroInfo.Commands[(com)];
                            try
                            {
                                mac.Invoke(this, args);
                            }
                            catch (ParseException e)
                            {
                                if (!isPartial)
                                {
                                    throw e;
                                }
                            }
                            parseString.Remove(spos, pos - spos);
                            len = parseString.Length;
                            pos = spos;
                        }
                        else if (NewCommandMacro.IsMacro(com))
                        {
                            mac = MacroInfo.Commands[(com)];
                            args = getOptsArgs(mac.nbArgs, mac.hasOptions ? 1 : 0);
                            args[0] = com;
                            try
                            {
                                parseString.Replace(spos, pos, (string)mac.Invoke(this, args));
                            }
                            catch (ParseException e)
                            {
                                if (!isPartial)
                                {
                                    throw e;
                                }
                                else
                                {
                                    spos += com.Length + 1;
                                }
                            }
                            len = parseString.Length;
                            pos = spos;
                        }
                        else if ("begin" == (com))
                        {
                            args = getOptsArgs(1, 0);
                            mac = MacroInfo.Commands[(args[1] + "@env")];
                            if (mac == null)
                            {
                                if (!isPartial)
                                {
                                    throw new ParseException("Unknown environment: " + args[1] + " at position " + Line + ":" + Col);
                                }
                            }
                            else
                            {
                                try
                                {
                                    string[] optarg = getOptsArgs(mac.nbArgs - 1, 0);
                                    string grp = GetGroup("\\begin{" + args[1] + "}", "\\end{" + args[1] + "}");
                                    string expr = "{\\makeatletter \\" + args[1] + "@env";
                                    for (int i = 1; i <= mac.nbArgs - 1; i++)
                                        expr += "{" + optarg[i] + "}";
                                    expr += "{" + grp + "}\\makeatother}";
                                    parseString.Replace(spos, pos, expr);
                                    len = parseString.Length;
                                    pos = spos;
                                }
                                catch (ParseException e)
                                {
                                    if (!isPartial)
                                    {
                                        throw e;
                                    }
                                }
                            }
                        }
                        else if ("makeatletter" == (com))
                            atIsLetter++;
                        else if ("makeatother" == (com))
                            atIsLetter--;
                        else if (unparsedContents.Contains(com))
                        {
                            getOptsArgs(1, 0);
                        }
                        break;
                    case PERCENT:
                        spos = pos++;
                        char chr;
                        while (pos < len)
                        {
                            chr = parseString[(pos++)];
                            if (chr == '\r' || chr == '\n')
                            {
                                break;
                            }
                        }
                        if (pos < len)
                        {
                            pos--;
                        }
                        parseString.Replace(spos, pos, "");
                        len = parseString.Length;
                        pos = spos;
                        break;
                    case DEGRE:
                        parseString.Replace(pos, pos + 1, "^{\\circ}");
                        len = parseString.Length;
                        pos++;
                        break;
                    case SUPTWO:
                        parseString.Replace(pos, pos + 1, "\\jlatexmathcumsup{2}");
                        len = parseString.Length;
                        pos++;
                        break;
                    case SUPTHREE:
                        parseString.Replace(pos, pos + 1, "\\jlatexmathcumsup{3}");
                        len = parseString.Length;
                        pos++;
                        break;
                    case SUPONE:
                        parseString.Replace(pos, pos + 1, "\\jlatexmathcumsup{1}");
                        len = parseString.Length;
                        pos++;
                        break;
                    case SUPZERO:
                        parseString.Replace(pos, pos + 1, "\\jlatexmathcumsup{0}");
                        len = parseString.Length;
                        pos++;
                        break;
                    case SUPFOUR:
                        parseString.Replace(pos, pos + 1, "\\jlatexmathcumsup{4}");
                        len = parseString.Length;
                        pos++;
                        break;
                    case SUPFIVE:
                        parseString.Replace(pos, pos + 1, "\\jlatexmathcumsup{5}");
                        len = parseString.Length;
                        pos++;
                        break;
                    case SUPSIX:
                        parseString.Replace(pos, pos + 1, "\\jlatexmathcumsup{6}");
                        len = parseString.Length;
                        pos++;
                        break;
                    case SUPSEVEN:
                        parseString.Replace(pos, pos + 1, "\\jlatexmathcumsup{7}");
                        len = parseString.Length;
                        pos++;
                        break;
                    case SUPEIGHT:
                        parseString.Replace(pos, pos + 1, "\\jlatexmathcumsup{8}");
                        len = parseString.Length;
                        pos++;
                        break;
                    case SUPNINE:
                        parseString.Replace(pos, pos + 1, "\\jlatexmathcumsup{9}");
                        len = parseString.Length;
                        pos++;
                        break;
                    case SUPPLUS:
                        parseString.Replace(pos, pos + 1, "\\jlatexmathcumsup{+}");
                        len = parseString.Length;
                        pos++;
                        break;
                    case SUPMINUS:
                        parseString.Replace(pos, pos + 1, "\\jlatexmathcumsup{-}");
                        len = parseString.Length;
                        pos++;
                        break;
                    case SUPEQUAL:
                        parseString.Replace(pos, pos + 1, "\\jlatexmathcumsup{=}");
                        len = parseString.Length;
                        pos++;
                        break;
                    case SUPLPAR:
                        parseString.Replace(pos, pos + 1, "\\jlatexmathcumsup{(}");
                        len = parseString.Length;
                        pos++;
                        break;
                    case SUPRPAR:
                        parseString.Replace(pos, pos + 1, "\\jlatexmathcumsup{)}");
                        len = parseString.Length;
                        pos++;
                        break;
                    case SUPN:
                        parseString.Replace(pos, pos + 1, "\\jlatexmathcumsup{n}");
                        len = parseString.Length;
                        pos++;
                        break;
                    case SUBTWO:
                        parseString.Replace(pos, pos + 1, "\\jlatexmathcumsub{2}");
                        len = parseString.Length;
                        pos++;
                        break;
                    case SUBTHREE:
                        parseString.Replace(pos, pos + 1, "\\jlatexmathcumsub{3}");
                        len = parseString.Length;
                        pos++;
                        break;
                    case SUBONE:
                        parseString.Replace(pos, pos + 1, "\\jlatexmathcumsub{1}");
                        len = parseString.Length;
                        pos++;
                        break;
                    case SUBZERO:
                        parseString.Replace(pos, pos + 1, "\\jlatexmathcumsub{0}");
                        len = parseString.Length;
                        pos++;
                        break;
                    case SUBFOUR:
                        parseString.Replace(pos, pos + 1, "\\jlatexmathcumsub{4}");
                        len = parseString.Length;
                        pos++;
                        break;
                    case SUBFIVE:
                        parseString.Replace(pos, pos + 1, "\\jlatexmathcumsub{5}");
                        len = parseString.Length;
                        pos++;
                        break;
                    case SUBSIX:
                        parseString.Replace(pos, pos + 1, "\\jlatexmathcumsub{6}");
                        len = parseString.Length;
                        pos++;
                        break;
                    case SUBSEVEN:
                        parseString.Replace(pos, pos + 1, "\\jlatexmathcumsub{7}");
                        len = parseString.Length;
                        pos++;
                        break;
                    case SUBEIGHT:
                        parseString.Replace(pos, pos + 1, "\\jlatexmathcumsub{8}");
                        len = parseString.Length;
                        pos++;
                        break;
                    case SUBNINE:
                        parseString.Replace(pos, pos + 1, "\\jlatexmathcumsub{9}");
                        len = parseString.Length;
                        pos++;
                        break;
                    case SUBPLUS:
                        parseString.Replace(pos, pos + 1, "\\jlatexmathcumsub{+}");
                        len = parseString.Length;
                        pos++;
                        break;
                    case SUBMINUS:
                        parseString.Replace(pos, pos + 1, "\\jlatexmathcumsub{-}");
                        len = parseString.Length;
                        pos++;
                        break;
                    case SUBEQUAL:
                        parseString.Replace(pos, pos + 1, "\\jlatexmathcumsub{=}");
                        len = parseString.Length;
                        pos++;
                        break;
                    case SUBLPAR:
                        parseString.Replace(pos, pos + 1, "\\jlatexmathcumsub{(}");
                        len = parseString.Length;
                        pos++;
                        break;
                    case SUBRPAR:
                        parseString.Replace(pos, pos + 1, "\\jlatexmathcumsub{)}");
                        len = parseString.Length;
                        pos++;
                        break;
                    default:
                        pos++;
                        break;
                }
            }
            pos = 0;
            len = parseString.Length;
        }
    }

    /** Parse the input string
     * @ if an error is encountered during parsing
     */
    public void Parse()
    {
        if (len != 0)
        {
            char ch;
            while (pos < len)
            {
                ch = parseString[pos];

                switch (ch)
                {
                    case '\n':
                        line++;
                        col = pos;
                        pos++;
                        break;
                    case '\t':
                    case '\r':
                        pos++;
                        break;
                    case ' ':
                        pos++;
                        if (!ignoreWhiteSpace)
                        {// We are in a mbox
                            formula.Add(new SpaceAtom());
                            formula.Add(new BreakMarkAtom());
                            while (pos < len)
                            {
                                ch = parseString[pos];
                                if (ch != ' ' || ch != '\t' || ch != '\r')
                                    break;
                                pos++;
                            }
                        }
                        break;
                    case DOLLAR:
                        pos++;
                        if (!ignoreWhiteSpace)
                        {// We are in a mbox
                            int style = TeXConstants.STYLE_TEXT;
                            bool doubleDollar = false;
                            if (parseString[pos] == DOLLAR)
                            {
                                style = TeXConstants.STYLE_DISPLAY;
                                doubleDollar = true;
                                pos++;
                            }

                            formula.Add(new MathAtom(new TeXFormula(this, GetDollarGroup(DOLLAR), false).root, style));
                            if (doubleDollar)
                            {
                                if (parseString[pos] == DOLLAR)
                                {
                                    pos++;
                                }
                            }
                        }
                        break;
                    case ESCAPE:
                        Atom at = ProcessEscape();
                        formula.Add(at);
                        if (arrayMode && at is HlineAtom)
                        {
                            ((ArrayOfAtoms)formula).AddRow();
                        }
                        if (insertion)
                        {
                            insertion = false;
                        }
                        break;
                    case L_GROUP:
                        Atom atom = GetArgument();
                        if (atom != null)
                        {
                            atom.Type = TeXConstants.TYPE_ORDINARY;
                        }
                        formula.Add(atom);
                        break;
                    case R_GROUP:
                        group--;
                        pos++;
                        if (group == -1)
                            throw new ParseException("Found a closing '" + R_GROUP + "' without an opening '" + L_GROUP + "'!");
                        return;
                    case SUPER_SCRIPT:
                        formula.Add(GetScripts(ch));
                        break;
                    case SUB_SCRIPT:
                        if (ignoreWhiteSpace)
                        {
                            formula.Add(GetScripts(ch));
                        }
                        else
                        {
                            formula.Add(new UnderscoreAtom());
                            pos++;
                        }
                        break;
                    case '&':
                        if (!arrayMode)
                            throw new ParseException("char '&' is only available in array mode !");
                        ((ArrayOfAtoms)formula).AddCol();
                        pos++;
                        break;
                    case '~':
                        formula.Add(new SpaceAtom());
                        pos++;
                        break;
                    case PRIME:
                        if (ignoreWhiteSpace)
                        {
                            formula.Add(new CumulativeScriptsAtom(GetLastAtom(), null, SymbolAtom.Get("prime")));
                        }
                        else
                        {
                            formula.Add(ConvertCharacter(PRIME, true));
                        }
                        pos++;
                        break;
                    case BACKPRIME:
                        if (ignoreWhiteSpace)
                        {
                            formula.Add(new CumulativeScriptsAtom(GetLastAtom(), null, SymbolAtom.Get("backprime")));
                        }
                        else
                        {
                            formula.Add(ConvertCharacter(BACKPRIME, true));
                        }
                        pos++;
                        break;
                    case DQUOTE:
                        if (ignoreWhiteSpace)
                        {
                            formula.Add(new CumulativeScriptsAtom(GetLastAtom(), null, SymbolAtom.Get("prime")));
                            formula.Add(new CumulativeScriptsAtom(GetLastAtom(), null, SymbolAtom.Get("prime")));
                        }
                        else
                        {
                            formula.Add(ConvertCharacter(PRIME, true));
                            formula.Add(ConvertCharacter(PRIME, true));
                        }
                        pos++;
                        break;
                    default:
                        formula.Add(ConvertCharacter(ch, false));
                        pos++;
                        break;
                }
            }
        }

        if (formula.root == null && !arrayMode)
        {
            formula.Add(new EmptyAtom());
        }
    }

    private Atom GetScripts(char f)
    {
        pos++;
        Atom first = GetArgument();
        Atom second = null;
        char s = '\0';

        if (pos < len)
            s = parseString[pos];

        if (f == SUPER_SCRIPT && s == SUPER_SCRIPT)
        {
            second = first;
            first = null;
        }
        else if (f == SUB_SCRIPT && s == SUPER_SCRIPT)
        {
            pos++;
            second = GetArgument();
        }
        else if (f == SUPER_SCRIPT && s == SUB_SCRIPT)
        {
            pos++;
            second = first;
            first = GetArgument();
        }
        else if (f == SUPER_SCRIPT && s != SUB_SCRIPT)
        {
            second = first;
            first = null;
        }

        Atom at;
        if (formula.root is RowAtom)
        {
            at = ((RowAtom)formula.root).GetLastAtom();
        }
        else if (formula.root == null)
        {
            at = new PhantomAtom(new CharAtom('M', "mathnormal"), false, true, true);
        }
        else
        {
            at = formula.root;
            formula.root = null;
        }

        if (at.RightType == TeXConstants.TYPE_BIG_OPERATOR)
            return new BigOperatorAtom(at, first, second);
        else if (at is OverUnderDelimiter)
        {
            if (((OverUnderDelimiter)at).IsOver)
            {
                if (second != null)
                {
                    ((OverUnderDelimiter)at).AddScript(second);
                    return new ScriptsAtom(at, first, null);
                }
            }
            else if (first != null)
            {
                ((OverUnderDelimiter)at).AddScript(first);
                return new ScriptsAtom(at, null, second);
            }
        }

        return new ScriptsAtom(at, first, second);
    }

    /** Get the contents between two delimiters
     * @param openclose the opening and closing character (such $)
     * @return the enclosed contents
     * @ if the contents are badly enclosed
     */
    public string GetDollarGroup(char openclose)
    {
        int spos = pos;
        char ch;

        do
        {
            ch = parseString[pos++];
            if (ch == ESCAPE)
            {
                pos++;
            }
        } while (pos < len && ch != openclose);

        if (ch == openclose)
        {
            return parseString.ToString().Substring(spos, pos - spos - 1);
        }
        else
        {
            return parseString.ToString().Substring(spos, pos - spos - 1);
        }
    }

    /** Get the contents between two delimiters
     * @param open the opening character
     * @param close the closing character
     * @return the enclosed contents
     * @ if the contents are badly enclosed
     */
    public string getGroup(char open, char close)
    {
        if (pos == len)
            return null;

        int group, spos;
        char ch = parseString[(pos)];

        if (pos < len && ch == open)
        {
            group = 1;
            spos = pos;
            while (pos < len - 1 && group != 0)
            {
                pos++;
                ch = parseString[(pos)];
                if (ch == open)
                    group++;
                else if (ch == close)
                    group--;
                else if (ch == ESCAPE && pos != len - 1)
                    pos++;
            }

            pos++;

            if (group != 0)
            {
                return parseString.ToString().Substring(spos + 1, pos - (spos + 1));
            }

            return parseString.ToString().Substring(spos + 1, pos - 1 - (spos + 1));
        }
        else
        {
            throw new ParseException("missing '" + open + "'!");
        }
    }

    /** Get the contents between two strings as in \begin{foo}...\end{foo}
     * @param open the opening string
     * @param close the closing string
     * @return the enclosed contents
     * @ if the contents are badly enclosed
     */
    public string GetGroup(string open, string close)
    {
        int group = 1;
        int ol = open.Length, cl = close.Length;
        bool lastO = IsValidCharacterInCommand(open[ol - 1]);
        bool lastC = IsValidCharacterInCommand(close[cl - 1]);
        int oc = 0, cc = 0;
        int startC = 0;
        char prev = '\0';
        var buf = new StringBuilder();

        while (pos < len && group != 0)
        {
            char c = parseString[pos];
            char c1;

            if (prev != ESCAPE && c == ' ')
            {//Trick to handle case where close == "\end   {foo}"
                while (pos < len && parseString[pos++] == ' ')
                {
                    buf.Append(' ');
                }
                c = parseString[(--pos)];
                if (IsValidCharacterInCommand(prev) && IsValidCharacterInCommand(c))
                {
                    oc = cc = 0;
                }
            }

            if (c == open[(oc)])
                oc++;
            else
                oc = 0;

            if (c == close[cc])
            {
                if (cc == 0)
                {
                    startC = pos;
                }
                cc++;
            }
            else
                cc = 0;

            if (pos + 1 < len)
            {
                c1 = parseString[pos + 1];

                if (oc == ol)
                {
                    if (!lastO || !IsValidCharacterInCommand(c1))
                    {
                        group++;
                    }
                    oc = 0;
                }

                if (cc == cl)
                {
                    if (!lastC || !IsValidCharacterInCommand(c1))
                    {
                        group--;
                    }
                    cc = 0;
                }
            }
            else
            {
                if (oc == ol)
                {
                    group++;
                    oc = 0;
                }
                if (cc == cl)
                {
                    group--;
                    cc = 0;
                }
            }

            prev = c;
            buf.Append(c);
            pos++;
        }

        if (group != 0)
        {
            if (isPartial)
            {
                return buf.ToString();
            }
            throw new ParseException("The token " + open + " must be closed by " + close);
        }

        return buf.ToString().Substring(0, buf.Length - pos + startC);
    }

    /** Get the argument of a command in his atomic format
     * @return the corresponding atom
     * @ if the argument is incorrect
     */
    public Atom GetArgument()
    {
        SkipWhiteSpace();
        char ch;
        if (pos < len)
        {
            ch = parseString[pos];
        }
        else
        {
            return new EmptyAtom();
        }
        if (ch == L_GROUP)
        {
            TeXFormula tf = new TeXFormula();
            TeXFormula sformula = this.formula;
            this.formula = tf;
            pos++;
            group++;
            Parse();
            this.formula = sformula;
            if (this.formula.root == null)
            {
                RowAtom _at = new RowAtom();
                _at.Add(tf.root);
                return _at;
            }
            return tf.root;
        }

        if (ch == ESCAPE)
        {
            Atom _at = ProcessEscape();
            if (insertion)
            {
                insertion = false;
                return GetArgument();
            }
            return _at;
        }

        Atom at = ConvertCharacter(ch, true);
        pos++;
        return at;
    }

    public string GetOverArgument()
    {
        if (pos == len)
            return null;

        int ogroup = 1, spos;
        char ch = '\0';

        spos = pos;
        while (pos < len && ogroup != 0)
        {
            ch = parseString[pos];
            switch (ch)
            {
                case L_GROUP:
                    ogroup++;
                    break;
                case '&':
                    /* if a & is encountered at the same level as \over
                       we must break the argument */
                    if (ogroup == 1)
                    {
                        ogroup--;
                    }
                    break;
                case R_GROUP:
                    ogroup--;
                    break;
                case ESCAPE:
                    pos++;
                    /* if a \\ or a \cr is encountered at the same level as \over
                       we must break the argument */
                    if (pos < len && parseString[pos] == '\\' && ogroup == 1)
                    {
                        ogroup--;
                        pos--;
                    }
                    else if (pos < len - 1 && parseString[pos] == 'c' && parseString[pos + 1] == 'r' && ogroup == 1)
                    {
                        ogroup--;
                        pos--;
                    }
                    break;
            }
            pos++;
        }

        if (ogroup >= 2)
            // end of string reached, but not processed properly
            throw new ParseException("Illegal end,  missing '}' !");

        string str;
        if (ogroup == 0)
        {
            str = parseString.ToString().Substring(spos, pos - 1 - spos);
        }
        else
        {
            str = parseString.ToString().Substring(spos, pos - spos);
            ch = '\0';
        }

        if (ch == '&' || ch == '\\' || ch == R_GROUP)
            pos--;

        return str;
    }

    public float[] GetLength()
    {
        if (pos == len)
            return null;

        int spos;
        char ch = '\0';

        SkipWhiteSpace();
        spos = pos;
        while (pos < len && ch != ' ')
        {
            ch = parseString[pos++];
        }
        SkipWhiteSpace();

        return SpaceAtom.GetLength(parseString.ToString().Substring(spos, pos - 1 - spos));
    }

    /** Convert a character in the corresponding atom in using the file TeXFormulaSettings.xml for non-alphanumeric characters
     * @param c the character to be converted
     * @return the corresponding atom
     * @ if the character is unknown
     */
    public Atom ConvertCharacter(char c, bool oneChar)
    {
        if (ignoreWhiteSpace)
        {// The Unicode Greek letters in math mode are not drawn with the Greek font
            if (c >= 945 && c <= 969)
            {
                return SymbolAtom.Get(TeXFormula.symbolMappings[c]);
            }
            else if (c >= 913 && c <= 937)
            {
                return new TeXFormula(TeXFormula.symbolFormulaMappings[c]).root;
            }
        }

        c = ConvertToRomanNumber(c);
        if (((c < '0' || c > '9') && (c < 'a' || c > 'z') && (c < 'A' || c > 'Z')))
        {
            UnicodeBlock block = UnicodeBlock.Of(c);
            if (!isLoading && !DefaultTeXFont.loadedAlphabets.Contains(block))
            {
                DefaultTeXFont.AddAlphabet(DefaultTeXFont.registeredAlphabets[block]);
            }

            string symbolName = TeXFormula.symbolMappings[c];
            if (symbolName == null && (TeXFormula.symbolFormulaMappings == null || TeXFormula.symbolFormulaMappings[c] == null))
            {
                TeXFormula.FontInfos fontInfos = null;
                bool isLatin = UnicodeBlock.BASIC_LATIN == (block);
                if ((isLatin && TeXFormula.IsRegisteredBlock(UnicodeBlock.BASIC_LATIN)) || !isLatin)
                {
                    fontInfos = TeXFormula.GetExternalFont(block);
                }
                if (fontInfos != null)
                {
                    if (oneChar)
                    {
                        return new JavaFontRenderingAtom(char.ToString(c), fontInfos);
                    }
                    int start = pos++;
                    int end = len - 1;
                    while (pos < len)
                    {
                        c = parseString[(pos)];
                        if (UnicodeBlock.Of(c) != (block))
                        {
                            end = --pos;
                            break;
                        }
                        pos++;
                    }
                    return new JavaFontRenderingAtom(parseString.ToString().Substring(start, end + 1 - start), fontInfos);
                }

                if (!isPartial)
                {
                    throw new ParseException("Unknown character : '"
                                             + char.ToString(c) + "' (or " + ((int)c) + ")");
                }
                else
                {
                    return new ColorAtom(new RomanAtom(new TeXFormula("\\text{(Unknown char " + ((int)c) + ")}").root), null, Color.Red);
                }
            }
            else
            {
                if (!ignoreWhiteSpace)
                {// we are in text mode
                    if (TeXFormula.symbolTextMappings[c] != null)
                    {
                        return SymbolAtom.Get(TeXFormula.symbolTextMappings[c]).SetUnicode(c);
                    }
                }
                if (TeXFormula.symbolFormulaMappings != null && TeXFormula.symbolFormulaMappings[c] != null)
                {
                    return new TeXFormula(TeXFormula.symbolFormulaMappings[c]).root;
                }

                try
                {
                    return SymbolAtom.Get(symbolName);
                }
                catch (SymbolNotFoundException e)
                {
                    throw new ParseException("The character '"
                                             + char.ToString(c)
                                             + "' was mapped to an unknown symbol with the name '"
                                             + symbolName + "'!", e);
                }
            }
        }
        else
        {
            // alphanumeric character
            if (TeXFormula.externalFontMap.TryGetValue(UnicodeBlock.BASIC_LATIN, out var fontInfos))
            {
                if (oneChar)
                {
                    return new JavaFontRenderingAtom(char.ToString(c), fontInfos);
                }
                int start = pos++;
                int end = len - 1;
                while (pos < len)
                {
                    c = parseString[pos];
                    if (((c < '0' || c > '9') && (c < 'a' || c > 'z') && (c < 'A' || c > 'Z')))
                    {
                        end = --pos;
                        break;
                    }
                    pos++;
                }
                return new JavaFontRenderingAtom(parseString.ToString().Substring(start, end + 1 - start), fontInfos);
            }

            return new CharAtom(c, formula.textStyle, ignoreWhiteSpace);
        }
    }

    private string getCommand()
    {
        int spos = ++pos;
        char ch = '\0';

        while (pos < len)
        {
            ch = parseString[(pos)];
            if ((ch < 'a' || ch > 'z') && (ch < 'A' || ch > 'Z') && (atIsLetter == 0 || ch != '@'))
                break;

            pos++;
        }

        if (ch == '\0')
            return "";

        if (pos == spos)
        {
            pos++;
        }

        string com = parseString.ToString().Substring(spos, pos - spos);
        if ("cr" == (com) && pos < len && parseString[pos] == ' ')
        {
            pos++;
        }

        return com;
    }

    private Atom ProcessEscape()
    {
        spos = pos;
        string command = getCommand();

        if (command.Length == 0)
        {
            return new EmptyAtom();
        }

        if (MacroInfo.Commands[(command)] != null)
            return processCommands(command);

        try
        {
            return TeXFormula.Get(command).root;
        }
        catch (FormulaNotFoundException e)
        {
            try
            {
                return SymbolAtom.Get(command);
            }
            catch (SymbolNotFoundException e1) { }
        }

        // not a valid command or symbol or predefined TeXFormula found
        if (!isPartial)
        {
            throw new ParseException("Unknown symbol or command or predefined TeXFormula: '" + command + "'");
        }
        else
        {
            return new ColorAtom(new RomanAtom(new TeXFormula("\\backslash " + command).root), null, Color.Red);
        }
    }

    private void Insert(int beg, int end, string formula)
    {
        parseString.Replace(beg, end, formula);
        len = parseString.Length;
        pos = beg;
        insertion = true;
    }

    /** Get the arguments ant the options of a command
     * @param nbArgs the number of arguments of the command
     * @param opts must be 1 if the options are found before the first argument and must be 2 if they must be found before the second argument
     * @return an array containing arguments and at the end the options are put
     */
    /* Should be improved */
    public string[] getOptsArgs(int nbArgs, int opts)
    {
        //A maximum of 10 options can be passed to a command
        string[] args = new string[nbArgs + 10 + 1];
        if (nbArgs != 0)
        {

            //We get the options just after the command name
            if (opts == 1)
            {
                int j = nbArgs + 1;
                try
                {
                    for (; j < nbArgs + 11; j++)
                    {
                        SkipWhiteSpace();
                        args[j] = getGroup(L_BRACK, R_BRACK);
                    }
                }
                catch (ParseException e)
                {
                    args[j] = null;
                }
            }

            //We get the first argument
            SkipWhiteSpace();
            try
            {
                args[1] = getGroup(L_GROUP, R_GROUP);
            }
            catch (ParseException e)
            {
                if (parseString[pos] != '\\')
                {
                    args[1] = "" + parseString[pos];
                    pos++;
                }
                else
                    args[1] = getCommandWithArgs(getCommand());
            }

            //We get the options after the first argument
            if (opts == 2)
            {
                int j = nbArgs + 1;
                try
                {
                    for (; j < nbArgs + 11; j++)
                    {
                        SkipWhiteSpace();
                        args[j] = getGroup(L_BRACK, R_BRACK);
                    }
                }
                catch (ParseException e)
                {
                    args[j] = null;
                }
            }

            //We get the next arguments
            for (int i = 2; i <= nbArgs; i++)
            {
                SkipWhiteSpace();
                try
                {
                    args[i] = getGroup(L_GROUP, R_GROUP);
                }
                catch (ParseException e)
                {
                    if (parseString[pos] != '\\')
                    {
                        args[i] = "" + parseString[(pos)];
                        pos++;
                    }
                    else
                    {
                        args[i] = getCommandWithArgs(getCommand());
                    }
                }
            }

            if (ignoreWhiteSpace)
            {
                SkipWhiteSpace();
            }
        }
        return args;
    }

    /**
     * return a string with command and options and args
     * @param command name of command
     * @return
     * @author Juan Enrique Escobar Robles
     */
    private string getCommandWithArgs(string command)
    {
        if (command == "left")
        {
            return GetGroup("\\left", "\\right");
        }

        MacroInfo mac = MacroInfo.Commands[(command)];
        if (mac != null)
        {
            int mac_opts = 0;
            if (mac.hasOptions)
            {
                mac_opts = mac.posOpts;
            }

            string[] mac_args = getOptsArgs(mac.nbArgs, mac_opts);
            StringBuilder mac_arg = new StringBuilder("\\");
            mac_arg.Append(command);
            for (int j = 0; j < mac.posOpts; j++)
            {
                string arg_t = mac_args[mac.nbArgs + j + 1];
                if (arg_t != null)
                {
                    mac_arg.Append("[").Append(arg_t).Append("]");
                }
            }

            for (int j = 0; j < mac.nbArgs; j++)
            {
                string arg_t = mac_args[j + 1];
                if (arg_t != null)
                {
                    mac_arg.Append("{").Append(arg_t).Append("}");
                }
            }

            return mac_arg.ToString();
        }

        return "\\" + command;
    }

    /**
     * Processes the given TeX command (by parsing following command arguments
     * in the parse string).
     */
    private Atom processCommands(string command)
    {
        MacroInfo mac = MacroInfo.Commands[(command)];
        int opts = 0;
        if (mac.hasOptions)
            opts = mac.posOpts;

        string[] args = getOptsArgs(mac.nbArgs, opts);
        args[0] = command;

        if (NewCommandMacro.IsMacro(command))
        {
            string ret = (string)mac.Invoke(this, args);
            Insert(spos, pos, ret);
            return null;
        }

        return (Atom)mac.Invoke(this, args);
    }

    /** Test the validity of the name of a command. It must Contains only alpha characters and eventually a @ if makeAtletter activated
     * @param com the command's name
     * @return the validity of the name
     */
    public bool isValidName(string com)
    {
        if (com == null || "" == (com))
        {
            return false;
        }

        char c = '\0';
        if (com[0] == '\\')
        {
            int pos = 1;
            int len = com.Length;
            while (pos < len)
            {
                c = com[(pos)];
                if (!char.IsLetter(c) && (atIsLetter == 0 || c != '@'))
                    break;
                pos++;
            }
        }
        else
        {
            return false;
        }

        return char.IsLetter(c);
    }

    /** Test the validity of a character in a command. It must Contains only alpha characters and eventually a @ if makeAtletter activated.
     * @param ch character to test
     * @return the validity of the character
     */
    public bool IsValidCharacterInCommand(char ch)
    {
        return char.IsLetter(ch) || (atIsLetter != 0 && ch == '@');
    }

    private void SkipWhiteSpace()
    {
        char c;
        while (pos < len)
        {
            c = parseString[pos];
            if (c != ' ' && c != '\t' && c != '\n' && c != '\r')
                break;
            if (c == '\n')
            {
                line++;
                col = pos;
            }
            pos++;
        }
    }

    /**
     * The aim of this method is to convert foreign number into roman ones !
     */
    private static char ConvertToRomanNumber(char c)
    {
        if (c == 0x66b)
        {//Arabic dot
            return '.';
        }
        else if (0x660 <= c && c <= 0x669)
        {//Arabic
            return (char)(c - (char)0x630);
        }
        else if (0x6f0 <= c && c <= 0x6f9)
        {//Arabic
            return (char)(c - (char)0x6c0);
        }
        else if (0x966 <= c && c <= 0x96f)
        {//Devanagari
            return (char)(c - (char)0x936);
        }
        else if (0x9e6 <= c && c <= 0x9ef)
        {//Bengali
            return (char)(c - (char)0x9b6);
        }
        else if (0xa66 <= c && c <= 0xa6f)
        {//Gurmukhi
            return (char)(c - (char)0xa36);
        }
        else if (0xae6 <= c && c <= 0xaef)
        {//Gujarati
            return (char)(c - (char)0xab6);
        }
        else if (0xb66 <= c && c <= 0xb6f)
        {//Oriya
            return (char)(c - (char)0xb36);
        }
        else if (0xc66 <= c && c <= 0xc6f)
        {//Telugu
            return (char)(c - (char)0xc36);
        }
        else if (0xd66 <= c && c <= 0xd6f)
        {//Malayalam
            return (char)(c - (char)0xd36);
        }
        else if (0xe50 <= c && c <= 0xe59)
        {//Thai
            return (char)(c - (char)0xe20);
        }
        else if (0xed0 <= c && c <= 0xed9)
        {//Lao
            return (char)(c - (char)0xea0);
        }
        else if (0xf20 <= c && c <= 0xf29)
        {//Tibetan
            return (char)(c - (char)0xe90);
        }
        else if (0x1040 <= c && c <= 0x1049)
        {//Myanmar
            return (char)(c - (char)0x1010);
        }
        else if (0x17e0 <= c && c <= 0x17e9)
        {//Khmer
            return (char)(c - (char)0x17b0);
        }
        else if (0x1810 <= c && c <= 0x1819)
        {//Mongolian
            return (char)(c - (char)0x17e0);
        }
        else if (0x1b50 <= c && c <= 0x1b59)
        {//Balinese
            return (char)(c - (char)0x1b20);
        }
        else if (0x1bb0 <= c && c <= 0x1bb9)
        {//Sundanese
            return (char)(c - (char)0x1b80);
        }
        else if (0x1c40 <= c && c <= 0x1c49)
        {//Lepcha
            return (char)(c - (char)0x1c10);
        }
        else if (0x1c50 <= c && c <= 0x1c59)
        {//Ol Chiki
            return (char)(c - (char)0x1c20);
        }
        else if (0xa8d0 <= c && c <= 0xa8d9)
        {//Saurashtra
            return (char)(c - (char)0xa8a0);
        }

        return c;
    }
}
