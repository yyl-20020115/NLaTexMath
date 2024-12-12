/* predefMacros.java
 * =========================================================================
 * This file is part of the JLaTeXMath Library - http://forge.scilab.org/jlatexmath
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

using System.Drawing;
using System.Text;

namespace NLaTexMath;

/**
 * This class Contains the most of basic commands of LaTeX, they're activated in
 * byt the class PredefinedCommands.java.
 **/
public class PredefMacros
{

    static PredefMacros()
    {
        NewEnvironmentMacro.AddNewEnvironment("array", "\\array@@env{#1}{", "}", 1);
        NewEnvironmentMacro.AddNewEnvironment("tabular", "\\array@@env{#1}{", "}", 1);
        NewEnvironmentMacro.AddNewEnvironment("matrix", "\\matrix@@env{", "}", 0);
        NewEnvironmentMacro.AddNewEnvironment("smallmatrix", "\\smallmatrix@@env{", "}", 0);
        NewEnvironmentMacro.AddNewEnvironment("pmatrix", "\\left(\\begin{matrix}", "\\end{matrix}\\right)", 0);
        NewEnvironmentMacro.AddNewEnvironment("bmatrix", "\\left[\\begin{matrix}", "\\end{matrix}\\right]", 0);
        NewEnvironmentMacro.AddNewEnvironment("Bmatrix", "\\left\\{\\begin{matrix}", "\\end{matrix}\\right\\}", 0);
        NewEnvironmentMacro.AddNewEnvironment("vmatrix", "\\left|\\begin{matrix}", "\\end{matrix}\\right|", 0);
        NewEnvironmentMacro.AddNewEnvironment("Vmatrix", "\\left\\|\\begin{matrix}", "\\end{matrix}\\right\\|", 0);
        NewEnvironmentMacro.AddNewEnvironment("eqnarray", "\\begin{array}{rcl}", "\\end{array}", 0);
        NewEnvironmentMacro.AddNewEnvironment("align", "\\align@@env{", "}", 0);
        NewEnvironmentMacro.AddNewEnvironment("flalign", "\\flalign@@env{", "}", 0);
        NewEnvironmentMacro.AddNewEnvironment("alignat", "\\alignat@@env{#1}{", "}", 1);
        NewEnvironmentMacro.AddNewEnvironment("aligned", "\\aligned@@env{", "}", 0);
        NewEnvironmentMacro.AddNewEnvironment("alignedat", "\\alignedat@@env{#1}{", "}", 1);
        NewEnvironmentMacro.AddNewEnvironment("multline", "\\multline@@env{", "}", 0);
        NewEnvironmentMacro.AddNewEnvironment("cases", "\\left\\{\\begin{array}{l@{\\!}l}", "\\end{array}\\right.", 0);
        NewEnvironmentMacro.AddNewEnvironment("split", "\\begin{array}{rl}", "\\end{array}", 0);
        NewEnvironmentMacro.AddNewEnvironment("gather", "\\gather@@env{", "}", 0);
        NewEnvironmentMacro.AddNewEnvironment("gathered", "\\gathered@@env{", "}", 0);
        NewEnvironmentMacro.AddNewEnvironment("math", "\\(", "\\)", 0);
        NewEnvironmentMacro.AddNewEnvironment("displaymath", "\\[", "\\]", 0);
        NewCommandMacro.AddNewCommand("operatorname", "\\mathop{\\mathrm{#1}}\\nolimits ", 1);
        NewCommandMacro.AddNewCommand("DeclareMathOperator", "\\newcommand{#1}{\\mathop{\\mathrm{#2}}\\nolimits}", 2);
        NewCommandMacro.AddNewCommand("substack", "{\\scriptstyle\\begin{array}{c}#1\\end{array}}", 1);
        NewCommandMacro.AddNewCommand("dfrac", "\\genfrac{}{}{}{}{#1}{#2}", 2);
        NewCommandMacro.AddNewCommand("tfrac", "\\genfrac{}{}{}{1}{#1}{#2}", 2);
        NewCommandMacro.AddNewCommand("dbinom", "\\genfrac{(}{)}{0pt}{}{#1}{#2}", 2);
        NewCommandMacro.AddNewCommand("tbinom", "\\genfrac{(}{)}{0pt}{1}{#1}{#2}", 2);
        NewCommandMacro.AddNewCommand("pmod", "\\qquad\\mathbin{(\\mathrm{mod}\\ #1)}", 1);
        NewCommandMacro.AddNewCommand("mod", "\\qquad\\mathbin{\\mathrm{mod}\\ #1}", 1);
        NewCommandMacro.AddNewCommand("pod", "\\qquad\\mathbin{(#1)}", 1);
        NewCommandMacro.AddNewCommand("dddot", "\\mathop{#1}\\limits^{...}", 1);
        NewCommandMacro.AddNewCommand("ddddot", "\\mathop{#1}\\limits^{....}", 1);
        NewCommandMacro.AddNewCommand("spdddot", "^{\\mathrm{...}}", 0);
        NewCommandMacro.AddNewCommand("spbreve", "^{\\makeatletter\\sp@breve\\makeatother}", 0);
        NewCommandMacro.AddNewCommand("sphat", "^{\\makeatletter\\sp@hat\\makeatother}", 0);
        NewCommandMacro.AddNewCommand("spddot", "^{\\displaystyle..}", 0);
        NewCommandMacro.AddNewCommand("spcheck", "^{\\vee}", 0);
        NewCommandMacro.AddNewCommand("sptilde", "^{\\sim}", 0);
        NewCommandMacro.AddNewCommand("spdot", "^{\\displaystyle.}", 0);
        NewCommandMacro.AddNewCommand("d", "\\underaccent{\\dot}{#1}", 1);
        NewCommandMacro.AddNewCommand("b", "\\underaccent{\\bar}{#1}", 1);
        NewCommandMacro.AddNewCommand("Bra", "\\left\\langle{#1}\\right\\vert", 1);
        NewCommandMacro.AddNewCommand("Ket", "\\left\\vert{#1}\\right\\rangle", 1);
        NewCommandMacro.AddNewCommand("textsuperscript", "{}^{\\text{#1}}", 1);
        NewCommandMacro.AddNewCommand("textsubscript", "{}_{\\text{#1}}", 1);
        NewCommandMacro.AddNewCommand("textit", "\\mathit{\\text{#1}}", 1);
        NewCommandMacro.AddNewCommand("textbf", "\\mathbf{\\text{#1}}", 1);
        NewCommandMacro.AddNewCommand("textsf", "\\mathsf{\\text{#1}}", 1);
        NewCommandMacro.AddNewCommand("texttt", "\\mathtt{\\text{#1}}", 1);
        NewCommandMacro.AddNewCommand("textrm", "\\text{#1}", 1);
        NewCommandMacro.AddNewCommand("degree", "^\\circ", 0);
        NewCommandMacro.AddNewCommand("with", "\\mathbin{\\&}", 0);
        NewCommandMacro.AddNewCommand("parr", "\\mathbin{\\rotatebox[origin=c]{180}{\\&}}", 0);
        NewCommandMacro.AddNewCommand("copyright", "\\textcircled{\\raisebox{0.2ex}{c}}", 0);
        NewCommandMacro.AddNewCommand("L", "\\mathrm{\\polishlcross L}", 0);
        NewCommandMacro.AddNewCommand("l", "\\mathrm{\\polishlcross l}", 0);
        NewCommandMacro.AddNewCommand("Join", "\\mathop{\\rlap{\\ltimes}\\rtimes}", 0);
    }

    public static Atom fcscore_macro(TeXParser tp, string[] args)
    {
        int n = int.parseInt(args[1]);
        if (n > 5)
        {
            int q = n / 5;
            int r = n % 5;
            RowAtom rat = new RowAtom();
            for (int i = 0; i < q; i++)
            {
                rat.Add(new FcscoreAtom(5));
            }
            rat.Add(new FcscoreAtom(r));

            return rat;
        }
        else
        {
            return new FcscoreAtom(n);
        }
    }

    public static Atom longdiv_macro(TeXParser tp, string[] args)
    {
        try
        {
            long dividend = long.TryParse(args[1], out var v1) ? v1 : 0;
            long divisor = long.TryParse(args[2], out var v2) ? v2 : 0;
            return new LongdivAtom(divisor, dividend);
        }
        catch (Exception e)
        {
            throw new ParseException("Divisor and dividend must be integer numbers");
        }
    }

    public static Atom st_macro(TeXParser tp, string[] args)
    {
        return new StrikeThroughAtom(new TeXFormula(tp, args[1], false).root);
    }

    public static Atom Braket_macro(TeXParser tp, string[] args)
    {
        string str = args[1].replaceAll("\\|", "\\\\middle\\\\vert ");
        return new TeXFormula(tp, "\\left\\langle " + str + "\\right\\rangle").root;
    }

    public static Atom Set_macro(TeXParser tp, string[] args)
    {
        string str = args[1].replaceFirst("\\|", "\\\\middle\\\\vert ");
        return new TeXFormula(tp, "\\left\\{" + str + "\\right\\}").root;
    }

    public static Atom spATbreve_macro(TeXParser tp, string[] args)
    {
        VRowAtom vra = new VRowAtom(new TeXFormula("\\displaystyle\\!\\breve{}").root);
        vra.setRaise(TeXConstants.UNIT_EX, 0.6f);

        return new SmashedAtom(vra, null);
    }

    public static Atom spAThat_macro(TeXParser tp, string[] args)
    {
        VRowAtom vra = new VRowAtom(new TeXFormula("\\displaystyle\\widehat{}").root);
        vra.setRaise(TeXConstants.UNIT_EX, 0.6f);

        return new SmashedAtom(vra, null);
    }

    public static Atom hvspace_macro(TeXParser tp, string[] args)
    {
        int i;
        for (i = 0; i < args[1].Length && !char.IsLetter(args[1][i]); i++) ;
        float f = 0;
        try
        {
            f = float.parseFloat(args[1].Substring(0, i));
        }
        catch (Exception e)
        {
            throw new ParseException(e.ToString());
        }

        int unit;
        if (i != args[1].Length)
        {
            unit = SpaceAtom.getUnit(args[1].Substring(i).ToLower());
        }
        else
        {
            unit = TeXConstants.UNIT_POINT;
        }

        if (unit == -1)
        {
            throw new ParseException("Unknown unit \"" + args[1].Substring(i) + "\" !");
        }

        return args[0][0] == 'h' ? new SpaceAtom(unit, f, 0, 0) : new SpaceAtom(unit, 0, f, 0);
    }

    public static Atom clrlap_macro(TeXParser tp, string[] args)
    {
        return new LapedAtom(new TeXFormula(tp, args[1]).root, args[0][0]);
    }

    public static Atom mathclrlap_macro(TeXParser tp, string[] args)
    {
        return new LapedAtom(new TeXFormula(tp, args[1]).root, args[0].charAt(4));
    }

    public static Atom includegraphics_macro(TeXParser tp, string[] args)
    {
        return new GraphicsAtom(args[1], args[2]);
    }

    public static Atom rule_macro(TeXParser tp, string[] args)
    {
        float[] winfo = SpaceAtom.getLength(args[1]);
        if (winfo.Length == 1)
        {
            throw new ParseException("Error in getting width in \\rule command !");
        }
        float[] hinfo = SpaceAtom.getLength(args[2]);
        if (hinfo.Length == 1)
        {
            throw new ParseException("Error in getting height in \\rule command !");
        }

        float[] rinfo = SpaceAtom.getLength(args[3]);
        if (rinfo.Length == 1)
        {
            throw new ParseException("Error in getting raise in \\rule command !");
        }

        return new RuleAtom((int)winfo[0], winfo[1], (int)hinfo[0], hinfo[1], (int)rinfo[0], -rinfo[1]);
    }

    /* Thanks to Juan Enrique Escobar Robles for this macro */
    public static Atom cfrac_macro(TeXParser tp, string[] args)
    {
        int alig = TeXConstants.ALIGN_CENTER;
        if ("r" == (args[3]))
        {
            alig = TeXConstants.ALIGN_RIGHT;
        }
        else if ("l" == (args[3]))
        {
            alig = TeXConstants.ALIGN_LEFT;
        }
        TeXFormula num = new TeXFormula(tp, args[1], false);
        TeXFormula denom = new TeXFormula(tp, args[2], false);
        if (num.root == null || denom.root == null)
        {
            throw new ParseException("Both numerator and denominator of a fraction can't be empty!");
        }
        Atom f = new FractionAtom(num.root, denom.root, true, alig, TeXConstants.ALIGN_CENTER);
        RowAtom rat = new RowAtom();
        rat.Add(new StyleAtom(TeXConstants.STYLE_DISPLAY, f));
        return rat;
    }

    public static Atom frac_macro(TeXParser tp, string[] args)
    {
        TeXFormula num = new TeXFormula(tp, args[1], false);
        TeXFormula denom = new TeXFormula(tp, args[2], false);
        if (num.root == null || denom.root == null)
            throw new ParseException("Both numerator and denominator of a fraction can't be empty!");
        return new FractionAtom(num.root, denom.root, true);
    }

    public static Atom sfrac_macro(TeXParser tp, string[] args)
    {
        TeXFormula num = new TeXFormula(tp, args[1], false);
        TeXFormula denom = new TeXFormula(tp, args[2], false);
        if (num.root == null || denom.root == null)
            throw new ParseException("Both numerator and denominator of a fraction can't be empty!");

        double scaleX = 0.75;
        double scaleY = 0.75;
        float raise1 = 0.45f;
        float shiftL = -0.13f;
        float shiftR = -0.065f;
        Atom slash = SymbolAtom.Get("slash");

        if (!tp.isMathMode())
        {
            scaleX = 0.6;
            scaleY = 0.5;
            raise1 = 0.75f;
            shiftL = -0.24f;
            shiftR = -0.24f;
            slash = new VRowAtom(new ScaleAtom(SymbolAtom.Get("textfractionsolidus"), 1.25, 0.65));
            ((VRowAtom)slash).setRaise(TeXConstants.UNIT_EX, 0.4f);
        }

        VRowAtom snum = new VRowAtom(new ScaleAtom(num.root, scaleX, scaleY));
        snum.setRaise(TeXConstants.UNIT_EX, raise1);
        RowAtom at = new RowAtom(snum);
        at.Add(new SpaceAtom(TeXConstants.UNIT_EM, shiftL, 0f, 0f));
        at.Add(slash);
        at.Add(new SpaceAtom(TeXConstants.UNIT_EM, shiftR, 0f, 0f));
        at.Add(new ScaleAtom(denom.root, scaleX, scaleY));

        return at;
    }

    public static Atom genfrac_macro(TeXParser tp, string[] args)
    {
        TeXFormula left = new TeXFormula(tp, args[1], false);
        SymbolAtom L = null, R = null;
        if (left != null && left.root is SymbolAtom)
        {
            L = (SymbolAtom)left.root;
        }

        TeXFormula right = new TeXFormula(tp, args[2], false);
        if (right != null && right.root is SymbolAtom)
        {
            R = (SymbolAtom)right.root;
        }

        bool rule = true;
        float[] ths = SpaceAtom.getLength(args[3]);
        if (args[3] == null || args[3].Length == 0 || ths.Length == 1)
        {
            ths = new float[] { 0.0f, 0.0f };
            rule = false;
        }

        int style = 0;
        if (args[4].Length != 0)
        {
            style = int.parseInt(args[4]);
        }
        TeXFormula num = new TeXFormula(tp, args[5], false);
        TeXFormula denom = new TeXFormula(tp, args[6], false);
        if (num.root == null || denom.root == null)
            throw new ParseException("Both numerator and denominator of a fraction can't be empty!");
        Atom at = new FractionAtom(num.root, denom.root, rule, (int)ths[0], ths[1]);
        RowAtom rat = new RowAtom();
        rat.Add(new StyleAtom(style * 2, new FencedAtom(at, L, R)));

        return rat;
    }

    public static Atom over_macro(TeXParser tp, string[] args)
    {
        Atom num = tp.getFormulaAtom();
        Atom denom = new TeXFormula(tp, tp.getOverArgument(), false).root;
        if (num == null || denom == null)
            throw new ParseException("Both numerator and denominator of a fraction can't be empty!");
        return new FractionAtom(num, denom, true);
    }

    public static Atom overwithdelims_macro(TeXParser tp, string[] args)
    {
        Atom num = tp.getFormulaAtom();
        Atom denom = new TeXFormula(tp, tp.getOverArgument(), false).root;

        if (num == null || denom == null)
            throw new ParseException("Both numerator and denominator of a fraction can't be empty!");

        Atom left = new TeXFormula(tp, args[1], false).root;
        if (left is BigDelimiterAtom)
            left = ((BigDelimiterAtom)left).delim;
        Atom right = new TeXFormula(tp, args[2], false).root;
        if (right is BigDelimiterAtom)
            right = ((BigDelimiterAtom)right).delim;
        if (left is SymbolAtom && right is SymbolAtom)
        {
            return new FencedAtom(new FractionAtom(num, denom, true), (SymbolAtom)left, (SymbolAtom)right);
        }

        RowAtom ra = new RowAtom();
        ra.Add(left);
        ra.Add(new FractionAtom(num, denom, true));
        ra.Add(right);
        return ra;
    }

    public static Atom atop_macro(TeXParser tp, string[] args)
    {
        Atom num = tp.getFormulaAtom();
        Atom denom = new TeXFormula(tp, tp.getOverArgument(), false).root;
        if (num == null || denom == null)
            throw new ParseException("Both numerator and denominator of a fraction can't be empty!");
        return new FractionAtom(num, denom, false);
    }

    public static Atom atopwithdelims_macro(TeXParser tp, string[] args)
    {
        Atom num = tp.getFormulaAtom();
        Atom denom = new TeXFormula(tp, tp.getOverArgument(), false).root;

        if (num == null || denom == null)
            throw new ParseException("Both numerator and denominator of a fraction can't be empty!");

        Atom left = new TeXFormula(tp, args[1], false).root;
        if (left is BigDelimiterAtom)
            left = ((BigDelimiterAtom)left).delim;
        Atom right = new TeXFormula(tp, args[2], false).root;
        if (right is BigDelimiterAtom)
            right = ((BigDelimiterAtom)right).delim;
        if (left is SymbolAtom && right is SymbolAtom)
        {
            return new FencedAtom(new FractionAtom(num, denom, false), (SymbolAtom)left, (SymbolAtom)right);
        }

        RowAtom ra = new RowAtom();
        ra.Add(left);
        ra.Add(new FractionAtom(num, denom, false));
        ra.Add(right);
        return ra;
    }

    public static Atom choose_macro(TeXParser tp, string[] args)
    {
        return choose_brackets("lbrack", "rbrack", tp, args);
    }

    public static Atom brack_macro(TeXParser tp, string[] args)
    {
        return choose_brackets("lsqbrack", "rsqbrack", tp, args);
    }

    public static Atom bangle_macro(TeXParser tp, string[] args)
    {
        return choose_brackets("langle", "rangle", tp, args);
    }

    public static Atom brace_macro(TeXParser tp, string[] args)
    {
        return choose_brackets("lbrace", "rbrace", tp, args);
    }

    public static Atom choose_brackets(string left, string right, TeXParser tp, string[] args)
    {
        Atom num = tp.getFormulaAtom();
        Atom denom = new TeXFormula(tp, tp.getOverArgument(), false).root;
        if (num == null || denom == null)
            throw new ParseException("Both numerator and denominator of choose can't be empty!");
        return new FencedAtom(new FractionAtom(num, denom, false), new SymbolAtom(left, TeXConstants.TYPE_OPENING, true), new SymbolAtom(right, TeXConstants.TYPE_CLOSING, true));
    }

    public static Atom binom_macro(TeXParser tp, string[] args)
    {
        TeXFormula num = new TeXFormula(tp, args[1], false);
        TeXFormula denom = new TeXFormula(tp, args[2], false);
        if (num.root == null || denom.root == null)
            throw new ParseException("Both binomial coefficients must be not empty !!");
        return new FencedAtom(new FractionAtom(num.root, denom.root, false), new SymbolAtom("lbrack", TeXConstants.TYPE_OPENING, true), new SymbolAtom("rbrack", TeXConstants.TYPE_CLOSING, true));
    }

    public static Atom above_macro(TeXParser tp, string[] args)
    {
        Atom num = tp.getFormulaAtom();
        float[] dim = tp.Count;
        Atom denom = new TeXFormula(tp, tp.getOverArgument(), false).root;
        if (dim == null || dim.Length != 2)
        {
            throw new ParseException("Invalid length in above macro");
        }
        if (num == null || denom == null)
            throw new ParseException("Both numerator and denominator of a fraction can't be empty!");

        return new FractionAtom(num, denom, (int)dim[0], dim[1]);
    }

    public static Atom abovewithdelims_macro(TeXParser tp, string[] args)
    {
        Atom num = tp.getFormulaAtom();
        float[] dim = tp.Count;
        Atom denom = new TeXFormula(tp, tp.getOverArgument(), false).root;
        if (dim == null || dim.Length != 2)
        {
            throw new ParseException("Invalid length in above macro");
        }
        if (num == null || denom == null)
            throw new ParseException("Both numerator and denominator of a fraction can't be empty!");

        Atom left = new TeXFormula(tp, args[1], false).root;
        if (left is BigDelimiterAtom)
            left = ((BigDelimiterAtom)left).delim;
        Atom right = new TeXFormula(tp, args[2], false).root;
        if (right is BigDelimiterAtom)
            right = ((BigDelimiterAtom)right).delim;
        if (left is SymbolAtom && right is SymbolAtom)
        {
            return new FencedAtom(new FractionAtom(num, denom, (int)dim[0], dim[1]), (SymbolAtom)left, (SymbolAtom)right);
        }

        RowAtom ra = new RowAtom();
        ra.Add(left);
        ra.Add(new FractionAtom(num, denom, true));
        ra.Add(right);
        return ra;
    }

    public static Atom textstyle_macros(TeXParser tp, string[] args)
    {
        string style = args[0];
        if ("frak" == (args[0]))
            style = "mathfrak";
        else if ("Bbb" == (args[0]))
            style = "mathbb";
        else if ("bold" == (args[0]))
            return new BoldAtom(new TeXFormula(tp, args[1], false).root);
        else if ("cal" == (args[0]))
            style = "mathcal";

        TeXFormula.FontInfos fontInfos = TeXFormula.externalFontMap[(UnicodeBlock.BASIC_LATIN)];
        if (fontInfos != null)
        {
            TeXFormula.externalFontMap.Add(UnicodeBlock.BASIC_LATIN, null);
        }
        Atom at = new TeXFormula(tp, args[1], false).root;
        if (fontInfos != null)
        {
            TeXFormula.externalFontMap.Add(UnicodeBlock.BASIC_LATIN, fontInfos);
        }

        return new TextStyleAtom(at, style);
    }

    public static Atom mbox_macro(TeXParser tp, string[] args)
    {
        Atom group = new RomanAtom(new TeXFormula(tp, args[1], "mathnormal", false, false).root);
        return new StyleAtom(TeXConstants.STYLE_TEXT, group);
    }

    public static Atom text_macro(TeXParser tp, string[] args)
    {
        return new RomanAtom(new TeXFormula(tp, args[1], "mathnormal", false, false).root);
    }

    public static Atom underscore_macro(TeXParser tp, string[] args)
    {
        return new UnderscoreAtom();
    }

    public static Atom accent_macros(TeXParser tp, string[] args)
    {
        return new AccentedAtom(new TeXFormula(tp, args[1], false).root, args[0]);
    }

    public static Atom grkaccent_macro(TeXParser tp, string[] args)
    {
        return new AccentedAtom(new TeXFormula(tp, args[2], false).root, new TeXFormula(tp, args[1], false).root, false);
    }

    public static Atom accent_macro(TeXParser tp, string[] args)
    {
        return new AccentedAtom(new TeXFormula(tp, args[2], false).root, new TeXFormula(tp, args[1], false).root);
    }

    public static Atom accentbis_macros(TeXParser tp, string[] args)
    {
        string acc = "";
        switch (args[0][0])
        {
            case '~':
                acc = "tilde";
                break;
            case '\'':
                acc = "acute";
                break;
            case '^':
                acc = "hat";
                break;
            case '\"':
                acc = "ddot";
                break;
            case '`':
                acc = "grave";
                break;
            case '=':
                acc = "bar";
                break;
            case '.':
                acc = "dot";
                break;
            case 'u':
                acc = "breve";
                break;
            case 'v':
                acc = "check";
                break;
            case 'H':
                acc = "doubleacute";
                break;
            case 't':
                acc = "tie";
                break;
            case 'r':
                acc = "mathring";
                break;
            case 'U':
                acc = "cyrbreve";
        }

        return new AccentedAtom(new TeXFormula(tp, args[1], false).root, acc);
    }

    public static Atom cedilla_macro(TeXParser tp, string[] args)
    {
        return new CedillaAtom(new TeXFormula(tp, args[1]).root);
    }

    public static Atom IJ_macro(TeXParser tp, string[] args)
    {
        return new IJAtom(args[0][0] == 'I');
    }

    public static Atom TStroke_macro(TeXParser tp, string[] args)
    {
        return new TStrokeAtom(args[0][0] == 'T');
    }

    public static Atom LCaron_macro(TeXParser tp, string[] args)
    {
        return new LCaronAtom(args[0][0] == 'L');
    }

    public static Atom tcaron_macro(TeXParser tp, string[] args)
    {
        return new tcaronAtom();
    }

    public static Atom ogonek_macro(TeXParser tp, string[] args)
    {
        return new OgonekAtom(new TeXFormula(tp, args[1]).root);
    }

    public static Atom nbsp_macro(TeXParser tp, string[] args)
    {
        return new SpaceAtom();
    }

    public static Atom sqrt_macro(TeXParser tp, string[] args)
    {
        if (args[2] == null)
            return new NthRoot(new TeXFormula(tp, args[1], false).root, null);
        return new NthRoot(new TeXFormula(tp, args[1], false).root, new TeXFormula(tp, args[2], false).root);
    }

    public static Atom overrightarrow_macro(TeXParser tp, string[] args)
    {
        return new UnderOverArrowAtom(new TeXFormula(tp, args[1], false).root, false, true);
    }

    public static Atom overleftarrow_macro(TeXParser tp, string[] args)
    {
        return new UnderOverArrowAtom(new TeXFormula(tp, args[1], false).root, true, true);
    }

    public static Atom overleftrightarrow_macro(TeXParser tp, string[] args)
    {
        return new UnderOverArrowAtom(new TeXFormula(tp, args[1], false).root, true);
    }

    public static Atom underrightarrow_macro(TeXParser tp, string[] args)
    {
        return new UnderOverArrowAtom(new TeXFormula(tp, args[1], false).root, false, false);
    }

    public static Atom underleftarrow_macro(TeXParser tp, string[] args)
    {
        return new UnderOverArrowAtom(new TeXFormula(tp, args[1], false).root, true, false);
    }

    public static Atom underleftrightarrow_macro(TeXParser tp, string[] args)
    {
        return new UnderOverArrowAtom(new TeXFormula(tp, args[1], false).root, false);
    }

    public static Atom xleftarrow_macro(TeXParser tp, string[] args)
    {
        return new XArrowAtom(new TeXFormula(tp, args[1], false).root, new TeXFormula(tp, args[2]).root, true);
    }

    public static Atom xrightarrow_macro(TeXParser tp, string[] args)
    {
        return new XArrowAtom(new TeXFormula(tp, args[1], false).root, new TeXFormula(tp, args[2]).root, false);
    }

    public static Atom sideset_macro(TeXParser tp, string[] args)
    {
        TeXFormula tf = new TeXFormula();
        tf.Add(new PhantomAtom(new TeXFormula(tp, args[3]).root, false, true, true));
        tf.Append(tp.getIsPartial(), args[1]);
        tf.Add(new SpaceAtom(TeXConstants.UNIT_MU, -0.3f, 0f, 0f));
        tf.Append(tp.getIsPartial(), args[3] + "\\nolimits" + args[2]);
        return new TypedAtom(TeXConstants.TYPE_ORDINARY, TeXConstants.TYPE_ORDINARY, tf.root);
    }

    public static Atom prescript_macro(TeXParser tp, string[] args)
    {
        Atom _base = new TeXFormula(tp, args[3]).root;
        tp.addAtom(new ScriptsAtom(new PhantomAtom(_base, false, true, true), new TeXFormula(tp, args[2]).root, new TeXFormula(tp, args[1]).root, false));
        tp.addAtom(new SpaceAtom(TeXConstants.UNIT_MU, -0.3f, 0f, 0f));
        return new TypedAtom(TeXConstants.TYPE_ORDINARY, TeXConstants.TYPE_ORDINARY, _base);
    }

    public static Atom underbrace_macro(TeXParser tp, string[] args)
    {
        return new OverUnderDelimiter(new TeXFormula(tp, args[1], false).root, null, SymbolAtom.Get("rbrace"), TeXConstants.UNIT_EX, 0, false);
    }

    public static Atom overbrace_macro(TeXParser tp, string[] args)
    {
        return new OverUnderDelimiter(new TeXFormula(tp, args[1], false).root, null, SymbolAtom.Get("lbrace"), TeXConstants.UNIT_EX, 0, true);
    }

    public static Atom underbrack_macro(TeXParser tp, string[] args)
    {
        return new OverUnderDelimiter(new TeXFormula(tp, args[1], false).root, null, SymbolAtom.Get("rsqbrack"), TeXConstants.UNIT_EX, 0, false);
    }

    public static Atom overbrack_macro(TeXParser tp, string[] args)
    {
        return new OverUnderDelimiter(new TeXFormula(tp, args[1], false).root, null, SymbolAtom.Get("lsqbrack"), TeXConstants.UNIT_EX, 0, true);
    }

    public static Atom underparen_macro(TeXParser tp, string[] args)
    {
        return new OverUnderDelimiter(new TeXFormula(tp, args[1], false).root, null, SymbolAtom.Get("rbrack"), TeXConstants.UNIT_EX, 0, false);
    }

    public static Atom overparen_macro(TeXParser tp, string[] args)
    {
        return new OverUnderDelimiter(new TeXFormula(tp, args[1], false).root, null, SymbolAtom.Get("lbrack"), TeXConstants.UNIT_EX, 0, true);
    }

    public static Atom overline_macro(TeXParser tp, string[] args)
    {
        return new OverlinedAtom(new TeXFormula(tp, args[1], false).root);
    }

    public static Atom underline_macro(TeXParser tp, string[] args)
    {
        return new UnderlinedAtom(new TeXFormula(tp, args[1], false).root);
    }

    public static Atom mathop_macro(TeXParser tp, string[] args)
    {
        TypedAtom at = new TypedAtom(TeXConstants.TYPE_BIG_OPERATOR, TeXConstants.TYPE_BIG_OPERATOR, new TeXFormula(tp, args[1], false).root);
        at.TypeLimits = TeXConstants.SCRIPT_NORMAL;
        return at;
    }

    public static Atom mathpunct_macro(TeXParser tp, string[] args)
    {
        return new TypedAtom(TeXConstants.TYPE_PUNCTUATION, TeXConstants.TYPE_PUNCTUATION, new TeXFormula(tp, args[1], false).root);
    }

    public static Atom mathord_macro(TeXParser tp, string[] args)
    {
        return new TypedAtom(TeXConstants.TYPE_ORDINARY, TeXConstants.TYPE_ORDINARY, new TeXFormula(tp, args[1], false).root);
    }

    public static Atom mathrel_macro(TeXParser tp, string[] args)
    {
        return new TypedAtom(TeXConstants.TYPE_RELATION, TeXConstants.TYPE_RELATION, new TeXFormula(tp, args[1], false).root);
    }

    public static Atom mathinner_macro(TeXParser tp, string[] args)
    {
        return new TypedAtom(TeXConstants.TYPE_INNER, TeXConstants.TYPE_INNER, new TeXFormula(tp, args[1], false).root);
    }

    public static Atom mathbin_macro(TeXParser tp, string[] args)
    {
        return new TypedAtom(TeXConstants.TYPE_BINARY_OPERATOR, TeXConstants.TYPE_BINARY_OPERATOR, new TeXFormula(tp, args[1], false).root);
    }

    public static Atom mathopen_macro(TeXParser tp, string[] args)
    {
        return new TypedAtom(TeXConstants.TYPE_OPENING, TeXConstants.TYPE_OPENING, new TeXFormula(tp, args[1], false).root);
    }

    public static Atom mathclose_macro(TeXParser tp, string[] args)
    {
        return new TypedAtom(TeXConstants.TYPE_CLOSING, TeXConstants.TYPE_CLOSING, new TeXFormula(tp, args[1], false).root);
    }

    public static Atom joinrel_macro(TeXParser tp, string[] args)
    {
        return new TypedAtom(TeXConstants.TYPE_RELATION, TeXConstants.TYPE_RELATION, new SpaceAtom(TeXConstants.UNIT_MU, -2.6f, 0, 0));
    }

    public static Atom smash_macro(TeXParser tp, string[] args)
    {
        return new SmashedAtom(new TeXFormula(tp, args[1], false).root, args[2]);
    }

    public static Atom vdots_macro(TeXParser tp, string[] args)
    {
        return new VdotsAtom();
    }

    public static Atom ddots_macro(TeXParser tp, string[] args)
    {
        return new TypedAtom(TeXConstants.TYPE_INNER, TeXConstants.TYPE_INNER, new DdotsAtom());
    }

    public static Atom iddots_macro(TeXParser tp, string[] args)
    {
        return new TypedAtom(TeXConstants.TYPE_INNER, TeXConstants.TYPE_INNER, new IddotsAtom());
    }

    public static Atom nolimits_macro(TeXParser tp, string[] args)
    {
        Atom at = tp.getLastAtom();
        at.TypeLimits = TeXConstants.SCRIPT_NOLIMITS;
        return at.Clone();
    }

    public static Atom limits_macro(TeXParser tp, string[] args)
    {
        Atom at = tp.getLastAtom();
        at.TypeLimits = TeXConstants.SCRIPT_LIMITS;
        return at.Clone();
    }

    public static Atom normal_macro(TeXParser tp, string[] args)
    {
        Atom at = tp.getLastAtom();
        at.TypeLimits = TeXConstants.SCRIPT_NORMAL;
        return at.Clone();
    }

    public static Atom left_macro(TeXParser tp, string[] args)
    {
        string grp = tp.getGroup("\\left", "\\right");
        Atom left = new TeXFormula(tp, args[1], false).root;
        if (left is BigDelimiterAtom)
            left = ((BigDelimiterAtom)left).delim;
        Atom right = tp.getArgument();
        if (right is BigDelimiterAtom)
            right = ((BigDelimiterAtom)right).delim;
        if (left is SymbolAtom && right is SymbolAtom)
        {
            TeXFormula tf = new TeXFormula(tp, grp, false);
            return new FencedAtom(tf.root, (SymbolAtom)left, tf.middle, (SymbolAtom)right);
        }

        RowAtom ra = new RowAtom();
        ra.Add(left);
        ra.Add(new TeXFormula(tp, grp, false).root);
        ra.Add(right);
        return ra;
    }

    public static Atom leftparenthesis_macro(TeXParser tp, string[] args)
    {
        string grp = tp.getGroup("\\(", "\\)");
        return new MathAtom(new TeXFormula(tp, grp, false).root, TeXConstants.STYLE_TEXT);
    }

    public static Atom leftbracket_macro(TeXParser tp, string[] args)
    {
        string grp = tp.getGroup("\\[", "\\]");
        return new MathAtom(new TeXFormula(tp, grp, false).root, TeXConstants.STYLE_DISPLAY);
    }

    public static Atom middle_macro(TeXParser tp, string[] args)
    {
        return new MiddleAtom(new TeXFormula(tp, args[1]).root);
    }

    public static Atom cr_macro(TeXParser tp, string[] args)
    {
        if (tp.isArrayMode())
        {
            tp.addRow();
        }
        else
        {
            ArrayOfAtoms array = new ArrayOfAtoms();
            array.Add(tp.formula.root);
            array.AddRow();
            TeXParser parser = new TeXParser(tp.getIsPartial(), tp.getStringFromCurrentPos(), array, false, tp.isIgnoreWhiteSpace());
            parser.parse();
            array.CheckDimensions();
            tp.finish();
            tp.formula.root = array.GetAsVRow();//new MatrixAtom(tp.getIsPartial(), array, MatrixAtom.ARRAY, TeXConstants.ALIGN_LEFT, false);
        }

        return null;
    }

    public static Atom backslashcr_macro(TeXParser tp, string[] args)
    {
        return cr_macro(tp, args);
    }

    public static Atom intertext_macro(TeXParser tp, string[] args)
    {
        if (!tp.isArrayMode())
        {
            throw new ParseException("Bad environment for \\intertext command !");
        }

        string str = args[1].replaceAll("\\^\\{\\\\prime\\}", "\'");
        str = str.replaceAll("\\^\\{\\\\prime\\\\prime\\}", "\'\'");
        Atom at = new RomanAtom(new TeXFormula(tp, str, "mathnormal", false, false).root);
        at.Type = TeXConstants.TYPE_INTERTEXT;
        tp.addAtom(at);
        tp.addRow();
        return null;
    }

    public static Atom smallmatrixATATenv_macro(TeXParser tp, string[] args)
    {
        ArrayOfAtoms array = new ArrayOfAtoms();
        TeXParser parser = new TeXParser(tp.getIsPartial(), args[1], array, false);
        parser.parse();
        array.CheckDimensions();
        return new MatrixAtom(tp.getIsPartial(), array, MatrixAtom.SMALLMATRIX);
    }

    public static Atom matrixATATenv_macro(TeXParser tp, string[] args)
    {
        ArrayOfAtoms array = new ArrayOfAtoms();
        TeXParser parser = new TeXParser(tp.getIsPartial(), args[1], array, false);
        parser.parse();
        array.CheckDimensions();
        return new MatrixAtom(tp.getIsPartial(), array, MatrixAtom.MATRIX);
    }

    public static Atom multicolumn_macro(TeXParser tp, string[] args)
    {
        int n = int.parseInt(args[1]);
        tp.addAtom(new MulticolumnAtom(n, args[2], new TeXFormula(tp, args[3]).root));
        ((ArrayOfAtoms)tp.formula).AddCol(n);
        return null;
    }

    public static Atom hdotsfor_macro(TeXParser tp, string[] args)
    {
        int n = int.TryParse(args[1], out var v) ? v : 0;
        float f = 1;
        if (args[2] != null)
        {
            f = float.parseFloat(args[2]);
        }
        tp.addAtom(new HdotsforAtom(n, f));
        ((ArrayOfAtoms)tp.formula).AddCol(n);
        return null;
    }

    public static Atom arrayATATenv_macro(TeXParser tp, string[] args)
    {
        ArrayOfAtoms array = new ArrayOfAtoms();
        TeXParser parser = new TeXParser(tp.getIsPartial(), args[2], array, false);
        parser.parse();
        array.CheckDimensions();
        return new MatrixAtom(tp.getIsPartial(), array, args[1], true);
    }

    public static Atom alignATATenv_macro(TeXParser tp, string[] args)
    {
        ArrayOfAtoms array = new ArrayOfAtoms();
        TeXParser parser = new TeXParser(tp.getIsPartial(), args[1], array, false);
        parser.parse();
        array.CheckDimensions();
        return new MatrixAtom(tp.getIsPartial(), array, MatrixAtom.ALIGN);
    }

    public static Atom flalignATATenv_macro(TeXParser tp, string[] args)
    {
        ArrayOfAtoms array = new ArrayOfAtoms();
        TeXParser parser = new TeXParser(tp.getIsPartial(), args[1], array, false);
        parser.parse();
        array.CheckDimensions();
        return new MatrixAtom(tp.getIsPartial(), array, MatrixAtom.FLALIGN);
    }

    public static Atom alignatATATenv_macro(TeXParser tp, string[] args)
    {
        ArrayOfAtoms array = new ArrayOfAtoms();
        TeXParser parser = new TeXParser(tp.getIsPartial(), args[2], array, false);
        parser.parse();
        array.CheckDimensions();
        int n = int.parseInt(args[1]);
        if (array.col != 2 * n)
        {
            throw new ParseException("Bad number of equations in alignat environment !");
        }

        return new MatrixAtom(tp.getIsPartial(), array, MatrixAtom.ALIGNAT);
    }

    public static Atom alignedATATenv_macro(TeXParser tp, string[] args)
    {
        ArrayOfAtoms array = new ArrayOfAtoms();
        TeXParser parser = new TeXParser(tp.getIsPartial(), args[1], array, false);
        parser.parse();
        array.CheckDimensions();
        return new MatrixAtom(tp.getIsPartial(), array, MatrixAtom.ALIGNED);
    }

    public static Atom alignedatATATenv_macro(TeXParser tp, string[] args)
    {
        ArrayOfAtoms array = new ArrayOfAtoms();
        TeXParser parser = new TeXParser(tp.getIsPartial(), args[2], array, false);
        parser.parse();
        array.CheckDimensions();
        int n = int.TryParse(args[1],out var v)?v:0;
        if (array.col != 2 * n)
        {
            throw new ParseException("Bad number of equations in alignedat environment !");
        }

        return new MatrixAtom(tp.getIsPartial(), array, MatrixAtom.ALIGNEDAT);
    }

    public static Atom multlineATATenv_macro(TeXParser tp, string[] args)
    {
        ArrayOfAtoms array = new ArrayOfAtoms();
        TeXParser parser = new TeXParser(tp.getIsPartial(), args[1], array, false);
        parser.parse();
        array.CheckDimensions();
        if (array.col > 1)
        {
            throw new ParseException("char '&' is only available in array mode !");
        }
        if (array.col == 0)
        {
            return null;
        }

        return new MultlineAtom(tp.getIsPartial(), array, MultlineAtom.MULTLINE);
    }

    public static Atom gatherATATenv_macro(TeXParser tp, string[] args)
    {
        ArrayOfAtoms array = new ArrayOfAtoms();
        TeXParser parser = new TeXParser(tp.getIsPartial(), args[1], array, false);
        parser.parse();
        array.CheckDimensions();
        if (array.col > 1)
        {
            throw new ParseException("char '&' is only available in array mode !");
        }
        if (array.col == 0)
        {
            return null;
        }

        return new MultlineAtom(tp.getIsPartial(), array, MultlineAtom.GATHER);
    }

    public static Atom gatheredATATenv_macro(TeXParser tp, string[] args)
    {
        ArrayOfAtoms array = new ArrayOfAtoms();
        TeXParser parser = new TeXParser(tp.getIsPartial(), args[1], array, false);
        parser.parse();
        array.CheckDimensions();
        if (array.col > 1)
        {
            throw new ParseException("char '&' is only available in array mode !");
        }
        if (array.col == 0)
        {
            return null;
        }

        return new MultlineAtom(tp.getIsPartial(), array, MultlineAtom.GATHERED);
    }

    public static Atom shoveright_macro(TeXParser tp, string[] args)
    {
        Atom at = new TeXFormula(tp, args[1]).root;
        at.Alignment = TeXConstants.ALIGN_RIGHT;
        return at;
    }

    public static Atom shoveleft_macro(TeXParser tp, string[] args)
    {
        Atom at = new TeXFormula(tp, args[1]).root;
        at.Alignment = TeXConstants.ALIGN_LEFT;
        return at;
    }

    public static Atom newcommand_macro(TeXParser tp, string[] args)
    {
        string newcom = args[1];
        int nbArgs;
        if (!tp.isValidName(newcom))
        {
            throw new ParseException("Invalid name for the command :" + newcom);
        }

        if (args[3] == null)
            nbArgs = new int(0);
        else
            nbArgs = int.TryParse(args[3], out var v) ? v : 0;

        if (nbArgs == null)
        {
            throw new ParseException("The optional argument should be an integer !");
        }

        if (args[4] == null)
            NewCommandMacro.addNewCommand(newcom.Substring(1), args[2], nbArgs.intValue());
        else
            NewCommandMacro.addNewCommand(newcom.Substring(1), args[2], nbArgs.intValue(), args[4]);

        return null;
    }

    public static Atom renewcommand_macro(TeXParser tp, string[] args)
    {
        string newcom = args[1];
        int nbArgs;
        if (!tp.isValidName(newcom))
        {
            throw new ParseException("Invalid name for the command :" + newcom);
        }

        if (args[3] == null)
            nbArgs = (0);
        else
            nbArgs = int.TryParse(args[3], out var v) ? v : 0;

        if (nbArgs == null)
            throw new ParseException("The optional argument should be an integer !");

        NewCommandMacro.addReNewCommand(newcom.Substring(1), args[2], nbArgs.intValue());

        return null;
    }

    public static Atom makeatletter_macro(TeXParser tp, string[] args)
    {
        tp.makeAtLetter();
        return null;
    }

    public static Atom makeatother_macro(TeXParser tp, string[] args)
    {
        tp.makeAtOther();
        return null;
    }

    public static Atom newenvironment_macro(TeXParser tp, string[] args)
    {
        int opt = args[4] == null ? 0 : int.TryParse(args[4], out var v) ? v : 0;
        if (opt == null)
            throw new ParseException("The optional argument should be an integer !");

        NewEnvironmentMacro.addNewEnvironment(args[1], args[2], args[3], opt.intValue());
        return null;
    }

    public static Atom renewenvironment_macro(TeXParser tp, string[] args)
    {
        int opt = args[4] == null ? 0 : int.parseInt(args[4]);
        if (opt == null)
            throw new ParseException("The optional argument should be an integer !");

        NewEnvironmentMacro.addReNewEnvironment(args[1], args[2], args[3], opt.intValue());
        return null;
    }

    public static Atom fbox_macro(TeXParser tp, string[] args)
    {
        return new FBoxAtom(new TeXFormula(tp, args[1], false).root);
    }

    public static Atom questeq_macro(TeXParser tp, string[] args)
    {
        Atom at = new UnderOverAtom(SymbolAtom.Get(TeXFormula.symbolMappings['=']), new ScaleAtom(SymbolAtom.Get(TeXFormula.symbolMappings['?']), 0.75f), TeXConstants.UNIT_MU, 2.5f, true, true);
        return new TypedAtom(TeXConstants.TYPE_RELATION, TeXConstants.TYPE_RELATION, at);
    }

    public static Atom stackrel_macro(TeXParser tp, string[] args)
    {
        Atom at = new UnderOverAtom(new TeXFormula(tp, args[2], false).root, new TeXFormula(tp, args[3], false).root, TeXConstants.UNIT_MU, 0.5f, true, new TeXFormula(tp, args[1], false).root, TeXConstants.UNIT_MU, 2.5f, true);
        return new TypedAtom(TeXConstants.TYPE_RELATION, TeXConstants.TYPE_RELATION, at);
    }

    public static Atom stackbin_macro(TeXParser tp, string[] args)
    {
        Atom at = new UnderOverAtom(new TeXFormula(tp, args[2], false).root, new TeXFormula(tp, args[3], false).root, TeXConstants.UNIT_MU, 0.5f, true, new TeXFormula(tp, args[1], false).root, TeXConstants.UNIT_MU, 2.5f, true);
        return new TypedAtom(TeXConstants.TYPE_BINARY_OPERATOR, TeXConstants.TYPE_BINARY_OPERATOR, at);
    }

    public static Atom overset_macro(TeXParser tp, string[] args)
    {
        Atom at = new UnderOverAtom(new TeXFormula(tp, args[2], false).root, new TeXFormula(tp, args[1], false).root, TeXConstants.UNIT_MU, 2.5f, true, true);
        return new TypedAtom(TeXConstants.TYPE_RELATION, TeXConstants.TYPE_RELATION, at);
    }

    public static Atom underset_macro(TeXParser tp, string[] args)
    {
        Atom at = new UnderOverAtom(new TeXFormula(tp, args[2], false).root, new TeXFormula(tp, args[1], false).root, TeXConstants.UNIT_MU, 0.5f, true, false);
        return new TypedAtom(TeXConstants.TYPE_RELATION, TeXConstants.TYPE_RELATION, at);
    }

    public static Atom accentset_macro(TeXParser tp, string[] args)
    {
        return new AccentedAtom(new TeXFormula(tp, args[2], false).root, new TeXFormula(tp, args[1], false).root);
    }

    public static Atom underaccent_macro(TeXParser tp, string[] args)
    {
        return new UnderOverAtom(new TeXFormula(tp, args[2], false).root, new TeXFormula(tp, args[1], false).root, TeXConstants.UNIT_MU, 0.3f, true, false);
    }

    public static Atom undertilde_macro(TeXParser tp, string[] args)
    {
        Atom at = new TeXFormula(tp, args[1], false).root;
        return new UnderOverAtom(at, new AccentedAtom(new PhantomAtom(at, true, false, false), "widetilde"), TeXConstants.UNIT_MU, 0.3f, true, false);
    }

    public static Atom boldsymbol_macro(TeXParser tp, string[] args)
    {
        return new BoldAtom(new TeXFormula(tp, args[1], false).root);
    }

    public static Atom mathrm_macro(TeXParser tp, string[] args)
    {
        return new RomanAtom(new TeXFormula(tp, args[1], false).root);
    }

    public static Atom rm_macro(TeXParser tp, string[] args)
    {
        return new RomanAtom(new TeXFormula(tp, tp.getOverArgument(), null, false, tp.isIgnoreWhiteSpace()).root);
    }

    public static Atom mathbf_macro(TeXParser tp, string[] args)
    {
        return new BoldAtom(new RomanAtom(new TeXFormula(tp, args[1], false).root));
    }

    public static Atom bf_macro(TeXParser tp, string[] args)
    {
        return new BoldAtom(new RomanAtom(new TeXFormula(tp, tp.getOverArgument(), null, false, tp.isIgnoreWhiteSpace()).root));
    }

    public static Atom mathtt_macro(TeXParser tp, string[] args)
    {
        return new TtAtom(new TeXFormula(tp, args[1], false).root);
    }

    public static Atom tt_macro(TeXParser tp, string[] args)
    {
        return new TtAtom(new TeXFormula(tp, tp.getOverArgument(), null, false, tp.isIgnoreWhiteSpace()).root);
    }

    public static Atom mathit_macro(TeXParser tp, string[] args)
    {
        return new ItAtom(new TeXFormula(tp, args[1], false).root);
    }

    public static Atom it_macro(TeXParser tp, string[] args)
    {
        return new ItAtom(new TeXFormula(tp, tp.getOverArgument(), null, false, tp.isIgnoreWhiteSpace()).root);
    }

    public static Atom mathsf_macro(TeXParser tp, string[] args)
    {
        return new SsAtom(new TeXFormula(tp, args[1], false).root);
    }

    public static Atom sf_macro(TeXParser tp, string[] args)
    {
        return new SsAtom(new TeXFormula(tp, tp.getOverArgument(), null, false, tp.isIgnoreWhiteSpace()).root);
    }

    public static Atom LaTeX_macro(TeXParser tp, string[] args)
    {
        return new LaTeXAtom();
    }

    public static Atom GeoGebra_macro(TeXParser tp, string[] args)
    {
        TeXFormula tf = new TeXFormula("\\mathbb{G}\\mathsf{e}");
        tf.Add(new GeoGebraLogoAtom());
        tf.Add("\\mathsf{Gebra}");
        return new ColorAtom(tf.root, null, Color.FromArgb(102, 102, 102));
    }

    public static Atom hphantom_macro(TeXParser tp, string[] args)
    {
        return new PhantomAtom(new TeXFormula(tp, args[1], false).root, true, false, false);
    }

    public static Atom vphantom_macro(TeXParser tp, string[] args)
    {
        return new PhantomAtom(new TeXFormula(tp, args[1], false).root, false, true, true);
    }

    public static Atom phantom_macro(TeXParser tp, string[] args)
    {
        return new PhantomAtom(new TeXFormula(tp, args[1], false).root, true, true, true);
    }

    public static Atom big_macro(TeXParser tp, string[] args)
    {
        Atom at = new TeXFormula(tp, args[1], false).root;
        if (!(at is SymbolAtom))
        {
            return at;
        }
        return new BigDelimiterAtom((SymbolAtom)at, 1);
    }

    public static Atom Big_macro(TeXParser tp, string[] args)
    {
        Atom at = new TeXFormula(tp, args[1], false).root;
        if (!(at is SymbolAtom))
        {
            return at;
        }
        return new BigDelimiterAtom((SymbolAtom)at, 2);
    }

    public static Atom bigg_macro(TeXParser tp, string[] args)
    {
        Atom at = new TeXFormula(tp, args[1], false).root;
        if (!(at is SymbolAtom))
        {
            return at;
        }
        return new BigDelimiterAtom((SymbolAtom)at, 3);
    }

    public static Atom Bigg_macro(TeXParser tp, string[] args)
    {
        Atom at = new TeXFormula(tp, args[1], false).root;
        if (!(at is SymbolAtom))
        {
            return at;
        }
        return new BigDelimiterAtom((SymbolAtom)at, 4);
    }

    public static Atom bigl_macro(TeXParser tp, string[] args)
    {
        Atom at = new TeXFormula(tp, args[1], false).root;
        if (!(at is SymbolAtom))
        {
            return at;
        }
        Atom att = new BigDelimiterAtom((SymbolAtom)at, 1);
        att.Type = TeXConstants.TYPE_OPENING;
        return att;
    }

    public static Atom Bigl_macro(TeXParser tp, string[] args)
    {
        Atom at = new TeXFormula(tp, args[1], false).root;
        if (!(at is SymbolAtom))
        {
            return at;
        }
        Atom att = new BigDelimiterAtom((SymbolAtom)at, 2);
        att.Type = TeXConstants.TYPE_OPENING;
        return att;
    }

    public static Atom biggl_macro(TeXParser tp, string[] args)
    {
        Atom at = new TeXFormula(tp, args[1], false).root;
        if (!(at is SymbolAtom))
        {
            return at;
        }
        Atom att = new BigDelimiterAtom((SymbolAtom)at, 3);
        att.Type = TeXConstants.TYPE_OPENING;
        return att;
    }

    public static Atom Biggl_macro(TeXParser tp, string[] args)
    {
        Atom at = new TeXFormula(tp, args[1], false).root;
        if (!(at is SymbolAtom))
        {
            return at;
        }
        Atom att = new BigDelimiterAtom((SymbolAtom)at, 4);
        att.Type = TeXConstants.TYPE_OPENING;
        return att;
    }

    public static Atom bigr_macro(TeXParser tp, string[] args)
    {
        Atom at = new TeXFormula(tp, args[1], false).root;
        if (!(at is SymbolAtom))
        {
            return at;
        }
        Atom att = new BigDelimiterAtom((SymbolAtom)at, 1);
        att.Type = TeXConstants.TYPE_CLOSING;
        return att;
    }

    public static Atom Bigr_macro(TeXParser tp, string[] args)
    {
        Atom at = new TeXFormula(tp, args[1], false).root;
        if (!(at is SymbolAtom))
        {
            return at;
        }
        Atom att = new BigDelimiterAtom((SymbolAtom)at, 2);
        att.Type = TeXConstants.TYPE_CLOSING;
        return att;
    }

    public static Atom biggr_macro(TeXParser tp, string[] args)
    {
        Atom at = new TeXFormula(tp, args[1], false).root;
        if (!(at is SymbolAtom))
        {
            return at;
        }
        Atom att = new BigDelimiterAtom((SymbolAtom)at, 3);
        att.Type = TeXConstants.TYPE_CLOSING;
        return att;
    }

    public static Atom Biggr_macro(TeXParser tp, string[] args)
    {
        Atom at = new TeXFormula(tp, args[1], false).root;
        if (!(at is SymbolAtom))
        {
            return at;
        }
        Atom att = new BigDelimiterAtom((SymbolAtom)at, 4);
        att.Type = TeXConstants.TYPE_CLOSING;
        return att;
    }

    public static Atom displaystyle_macro(TeXParser tp, string[] args)
    {
        Atom group = new TeXFormula(tp, tp.getOverArgument(), false).root;
        return new StyleAtom(TeXConstants.STYLE_DISPLAY, group);
    }

    public static Atom scriptstyle_macro(TeXParser tp, string[] args)
    {
        Atom group = new TeXFormula(tp, tp.getOverArgument(), false).root;
        return new StyleAtom(TeXConstants.STYLE_SCRIPT, group);
    }

    public static Atom textstyle_macro(TeXParser tp, string[] args)
    {
        Atom group = new TeXFormula(tp, tp.getOverArgument(), false).root;
        return new StyleAtom(TeXConstants.STYLE_TEXT, group);
    }

    public static Atom scriptscriptstyle_macro(TeXParser tp, string[] args)
    {
        Atom group = new TeXFormula(tp, tp.getOverArgument(), false).root;
        return new StyleAtom(TeXConstants.STYLE_SCRIPT_SCRIPT, group);
    }

    public static Atom rotatebox_macro(TeXParser tp, string[] args)
    {
        return new RotateAtom(new TeXFormula(tp, args[2]).root, args[1] == null ? 0 : Double.parseDouble(args[1]), args[3]);
    }

    public static Atom reflectbox_macro(TeXParser tp, string[] args)
    {
        return new ReflectAtom(new TeXFormula(tp, args[1]).root);
    }

    public static Atom scalebox_macro(TeXParser tp, string[] args)
    {
        return new ScaleAtom(new TeXFormula(tp, args[2]).root, Double.parseDouble(args[1]), args[3] == null ? Double.parseDouble(args[1]) : Double.parseDouble(args[3]));
    }

    public static Atom resizebox_macro(TeXParser tp, string[] args)
    {
        return new ResizeAtom(new TeXFormula(tp, args[3]).root, args[1], args[2], args[1] == ("!") || args[2] == ("!"));
    }

    public static Atom raisebox_macro(TeXParser tp, string[] args)
    {
        float[] raise = SpaceAtom.getLength(args[1]);
        if (raise.Length == 1)
        {
            throw new ParseException("Error in getting raise in \\raisebox command !");
        }
        float[] height = SpaceAtom.getLength(args[3]);
        float[] depth = SpaceAtom.getLength(args[4]);
        if (height.Length == 1 || height[1] == 0)
        {
            height = new float[] { -1, 0 };
        }
        if (depth.Length == 1 || depth[1] == 0)
        {
            depth = new float[] { -1, 0 };
        }

        return new RaiseAtom(new TeXFormula(tp, args[2]).root, (int)raise[0], raise[1], (int)height[0], height[1], (int)depth[0], depth[1]);
    }

    public static Atom shadowbox_macro(TeXParser tp, string[] args)
    {
        return new ShadowAtom(new TeXFormula(tp, args[1]).root);
    }

    public static Atom ovalbox_macro(TeXParser tp, string[] args)
    {
        return new OvalAtom(new TeXFormula(tp, args[1]).root);
    }

    public static Atom doublebox_macro(TeXParser tp, string[] args)
    {
        return new DoubleFramedAtom(new TeXFormula(tp, args[1]).root);
    }

    public static Atom definecolor_macro(TeXParser tp, string[] args)
    {
        Color color = null;
        if ("gray" == (args[2]))
        {
            float f = float.parseFloat(args[3]);
            color = Color.FromArgb(f, f, f);
        }
        else if ("rgb" == (args[2]))
        {
            StringTokenizer stok = new StringTokenizer(args[3], ";,");
            if (stok.countTokens() != 3)
                throw new ParseException("The color definition must have three components !");
            float r = float.parseFloat(stok.nextToken().Trim());
            float g = float.parseFloat(stok.nextToken().Trim());
            float b = float.parseFloat(stok.nextToken().Trim());
            color = Color.FromArgb(r, g, b);
        }
        else if ("cmyk" == (args[2]))
        {
            StringTokenizer stok = new StringTokenizer(args[3], ",;");
            if (stok.countTokens() != 4)
                throw new ParseException("The color definition must have four components !");
            float[] cmyk = new float[4];
            for (int i = 0; i < 4; i++)
                cmyk[i] = float.parseFloat(stok.nextToken().Trim());
            float k = 1 - cmyk[3];
            color = Color.FromArgb(k * (1 - cmyk[0]), k * (1 - cmyk[1]), k * (1 - cmyk[2]));
        }
        else
            throw new ParseException("The color model is incorrect !");

        ColorAtom.Colors.Add(args[1], color);
        return null;
    }

    public static Atom fgcolor_macro(TeXParser tp, string[] args)
    {
        try
        {
            return new ColorAtom(new TeXFormula(tp, args[2]).root, null, ColorAtom.getColor(args[1]));
        }
        catch (Exception e)
        {
            throw new ParseException(e.ToString());
        }
    }

    public static Atom bgcolor_macro(TeXParser tp, string[] args)
    {
        try
        {
            return new ColorAtom(new TeXFormula(tp, args[2]).root, ColorAtom.getColor(args[1]), null);
        }
        catch (Exception e)
        {
            throw new ParseException(e.ToString());
        }
    }

    public static Atom textcolor_macro(TeXParser tp, string[] args)
    {
        return new ColorAtom(new TeXFormula(tp, args[2]).root, null, ColorAtom.getColor(args[1]));
    }

    public static Atom colorbox_macro(TeXParser tp, string[] args)
    {
        Color c = ColorAtom.getColor(args[1]);
        return new FBoxAtom(new TeXFormula(tp, args[2]).root, c, c);
    }

    public static Atom fcolorbox_macro(TeXParser tp, string[] args)
    {
        return new FBoxAtom(new TeXFormula(tp, args[3]).root, ColorAtom.getColor(args[2]), ColorAtom.getColor(args[1]));
    }

    public static Atom cong_macro(TeXParser tp, string[] args)
    {
        VRowAtom vra = new VRowAtom(SymbolAtom.Get("equals"));
        vra.Add(new SpaceAtom(TeXConstants.UNIT_MU, 0f, 1.5f, 0f));
        vra.Add(SymbolAtom.Get("sim"));
        vra.setRaise(TeXConstants.UNIT_MU, -1f);
        return new TypedAtom(TeXConstants.TYPE_RELATION, TeXConstants.TYPE_RELATION, vra);
    }

    public static Atom doteq_macro(TeXParser tp, string[] args)
    {
        Atom at = new UnderOverAtom(SymbolAtom.Get("equals"), SymbolAtom.Get("ldotp"), TeXConstants.UNIT_MU, 3.7f, false, true);
        return new TypedAtom(TeXConstants.TYPE_RELATION, TeXConstants.TYPE_RELATION, at);
    }

    public static Atom jlmDynamic_macro(TeXParser tp, string[] args)
    {
        if (DynamicAtom.hasAnExternalConverterFactory())
        {
            return new DynamicAtom(args[1], args[2]);
        }
        else
        {
            throw new ParseException("No ExternalConverterFactory set !");
        }
    }

    public static Atom jlmExternalFont_macro(TeXParser tp, string[] args)
    {
        JavaFontRenderingBox.setFont(args[1]);
        return null;
    }

    public static Atom jlmText_macro(TeXParser tp, string[] args)
    {
        return new JavaFontRenderingAtom(args[1], Font.PLAIN);
    }

    public static Atom jlmTextit_macro(TeXParser tp, string[] args)
    {
        return new JavaFontRenderingAtom(args[1], Font.ITALIC);
    }

    public static Atom jlmTextbf_macro(TeXParser tp, string[] args)
    {
        return new JavaFontRenderingAtom(args[1], Font.BOLD);
    }

    public static Atom jlmTextitbf_macro(TeXParser tp, string[] args)
    {
        return new JavaFontRenderingAtom(args[1], Font.BOLD | Font.ITALIC);
    }

    public static Atom DeclareMathSizes_macro(TeXParser tp, string[] args)
    {
        DefaultTeXFont.setMathSizes(float.parseFloat(args[1]), float.parseFloat(args[2]), float.parseFloat(args[3]), float.parseFloat(args[4]));
        return null;
    }

    public static Atom magnification_macro(TeXParser tp, string[] args)
    {
        DefaultTeXFont.setMagnification(float.parseFloat(args[1]));
        return null;
    }

    public static Atom hline_macro(TeXParser tp, string[] args)
    {
        if (!tp.isArrayMode())
            throw new ParseException("The macro \\hline is only available in array mode !");
        return new HlineAtom();
    }

    public static Atom size_macros(TeXParser tp, string[] args)
    {
        float f = 1f;
        if ("tiny" == (args[0]))
        {
            f = 0.5f;
        }
        else if ("scriptsize" == (args[0]))
        {
            f = 0.7f;
        }
        else if ("footnotesize" == (args[0]))
        {
            f = 0.8f;
        }
        else if ("small" == (args[0]))
        {
            f = 0.9f;
        }
        else if ("normalsize" == (args[0]))
        {
            f = 1f;
        }
        else if ("large" == (args[0]))
        {
            f = 1.2f;
        }
        else if ("Large" == (args[0]))
        {
            f = 1.4f;
        }
        else if ("LARGE" == (args[0]))
        {
            f = 1.8f;
        }
        else if ("huge" == (args[0]))
        {
            f = 2f;
        }
        else if ("Huge" == (args[0]))
        {
            f = 2.5f;
        }

        return new MonoScaleAtom(new TeXFormula(tp, tp.getOverArgument(), null, false, tp.isIgnoreWhiteSpace()).root, f);
    }

    public static Atom jlatexmathcumsup_macro(TeXParser tp, string[] args)
    {
        return new CumulativeScriptsAtom(tp.getLastAtom(), null, new TeXFormula(tp, args[1]).root);
    }

    public static Atom jlatexmathcumsub_macro(TeXParser tp, string[] args)
    {
        return new CumulativeScriptsAtom(tp.getLastAtom(), new TeXFormula(tp, args[1]).root, null);
    }

    public static Atom dotminus_macro(TeXParser tp, string[] args)
    {
        Atom at = new UnderOverAtom(SymbolAtom.Get("minus"), SymbolAtom.Get("normaldot"), TeXConstants.UNIT_MU, -3.3f, false, true);
        return new TypedAtom(TeXConstants.TYPE_BINARY_OPERATOR, TeXConstants.TYPE_BINARY_OPERATOR, at);
    }

    public static Atom ratio_macro(TeXParser tp, string[] args)
    {
        Atom at = new UnderOverAtom(SymbolAtom.Get("normaldot"), SymbolAtom.Get("normaldot"), TeXConstants.UNIT_MU, 5.2f, false, true);
        return new TypedAtom(TeXConstants.TYPE_RELATION, TeXConstants.TYPE_RELATION, at);
    }

    public static Atom geoprop_macro(TeXParser tp, string[] args)
    {
        RowAtom ddot = new RowAtom(SymbolAtom.Get("normaldot"));
        ddot.Add(new SpaceAtom(TeXConstants.UNIT_MU, 4f, 0f, 0f));
        ddot.Add(SymbolAtom.Get("normaldot"));
        Atom at = new UnderOverAtom(SymbolAtom.Get("minus"), ddot, TeXConstants.UNIT_MU, -3.4f, false, ddot, TeXConstants.UNIT_MU, -3.4f, false);
        return new TypedAtom(TeXConstants.TYPE_RELATION, TeXConstants.TYPE_RELATION, at);
    }

    public static Atom minuscolon_macro(TeXParser tp, string[] args)
    {
        RowAtom at = new RowAtom(SymbolAtom.Get("minus"));
        at.Add(new SpaceAtom(TeXConstants.UNIT_EM, -0.095f, 0f, 0f));
        at.Add(new UnderOverAtom(SymbolAtom.Get("normaldot"), SymbolAtom.Get("normaldot"), TeXConstants.UNIT_MU, 5.2f, false, true));
        return new TypedAtom(TeXConstants.TYPE_RELATION, TeXConstants.TYPE_RELATION, at);
    }

    public static Atom minuscoloncolon_macro(TeXParser tp, string[] args)
    {
        RowAtom at = new RowAtom(SymbolAtom.Get("minus"));
        at.Add(new SpaceAtom(TeXConstants.UNIT_EM, -0.095f, 0f, 0f));
        Atom colon = new UnderOverAtom(SymbolAtom.Get("normaldot"), SymbolAtom.Get("normaldot"), TeXConstants.UNIT_MU, 5.2f, false, true);
        at.Add(colon);
        at.Add(colon);
        return new TypedAtom(TeXConstants.TYPE_RELATION, TeXConstants.TYPE_RELATION, at);
    }

    public static Atom simcolon_macro(TeXParser tp, string[] args)
    {
        RowAtom at = new RowAtom(SymbolAtom.Get("sim"));
        at.Add(new SpaceAtom(TeXConstants.UNIT_EM, -0.095f, 0f, 0f));
        at.Add(new UnderOverAtom(SymbolAtom.Get("normaldot"), SymbolAtom.Get("normaldot"), TeXConstants.UNIT_MU, 5.2f, false, true));
        return new TypedAtom(TeXConstants.TYPE_RELATION, TeXConstants.TYPE_RELATION, at);
    }

    public static Atom simcoloncolon_macro(TeXParser tp, string[] args)
    {
        RowAtom at = new RowAtom(SymbolAtom.Get("sim"));
        at.Add(new SpaceAtom(TeXConstants.UNIT_EM, -0.095f, 0f, 0f));
        Atom colon = new UnderOverAtom(SymbolAtom.Get("normaldot"), SymbolAtom.Get("normaldot"), TeXConstants.UNIT_MU, 5.2f, false, true);
        at.Add(colon);
        at.Add(colon);
        return new TypedAtom(TeXConstants.TYPE_RELATION, TeXConstants.TYPE_RELATION, at);
    }

    public static Atom approxcolon_macro(TeXParser tp, string[] args)
    {
        RowAtom at = new RowAtom(SymbolAtom.Get("approx"));
        at.Add(new SpaceAtom(TeXConstants.UNIT_EM, -0.095f, 0f, 0f));
        at.Add(new UnderOverAtom(SymbolAtom.Get("normaldot"), SymbolAtom.Get("normaldot"), TeXConstants.UNIT_MU, 5.2f, false, true));
        return new TypedAtom(TeXConstants.TYPE_RELATION, TeXConstants.TYPE_RELATION, at);
    }

    public static Atom approxcoloncolon_macro(TeXParser tp, string[] args)
    {
        RowAtom at = new RowAtom(SymbolAtom.Get("approx"));
        at.Add(new SpaceAtom(TeXConstants.UNIT_EM, -0.095f, 0f, 0f));
        Atom colon = new UnderOverAtom(SymbolAtom.Get("normaldot"), SymbolAtom.Get("normaldot"), TeXConstants.UNIT_MU, 5.2f, false, true);
        at.Add(colon);
        at.Add(colon);
        return new TypedAtom(TeXConstants.TYPE_RELATION, TeXConstants.TYPE_RELATION, at);
    }

    public static Atom equalscolon_macro(TeXParser tp, string[] args)
    {
        RowAtom at = new RowAtom(SymbolAtom.Get("equals"));
        at.Add(new SpaceAtom(TeXConstants.UNIT_EM, -0.095f, 0f, 0f));
        at.Add(new UnderOverAtom(SymbolAtom.Get("normaldot"), SymbolAtom.Get("normaldot"), TeXConstants.UNIT_MU, 5.2f, false, true));
        return new TypedAtom(TeXConstants.TYPE_RELATION, TeXConstants.TYPE_RELATION, at);
    }

    public static Atom equalscoloncolon_macro(TeXParser tp, string[] args)
    {
        RowAtom at = new RowAtom(SymbolAtom.Get("equals"));
        at.Add(new SpaceAtom(TeXConstants.UNIT_EM, -0.095f, 0f, 0f));
        Atom colon = new UnderOverAtom(SymbolAtom.Get("normaldot"), SymbolAtom.Get("normaldot"), TeXConstants.UNIT_MU, 5.2f, false, true);
        at.Add(colon);
        at.Add(colon);
        return new TypedAtom(TeXConstants.TYPE_RELATION, TeXConstants.TYPE_RELATION, at);
    }

    public static Atom colonminus_macro(TeXParser tp, string[] args)
    {
        RowAtom at = new RowAtom(new UnderOverAtom(SymbolAtom.Get("normaldot"), SymbolAtom.Get("normaldot"), TeXConstants.UNIT_MU, 5.2f, false, true));
        at.Add(new SpaceAtom(TeXConstants.UNIT_EM, -0.32f, 0f, 0f));
        at.Add(SymbolAtom.Get("minus"));
        return new TypedAtom(TeXConstants.TYPE_RELATION, TeXConstants.TYPE_RELATION, at);
    }

    public static Atom coloncolonminus_macro(TeXParser tp, string[] args)
    {
        Atom colon = new UnderOverAtom(SymbolAtom.Get("normaldot"), SymbolAtom.Get("normaldot"), TeXConstants.UNIT_MU, 5.2f, false, true);
        RowAtom at = new RowAtom(colon);
        at.Add(colon);
        at.Add(new SpaceAtom(TeXConstants.UNIT_EM, -0.32f, 0f, 0f));
        at.Add(SymbolAtom.Get("minus"));
        return new TypedAtom(TeXConstants.TYPE_RELATION, TeXConstants.TYPE_RELATION, at);
    }

    public static Atom colonequals_macro(TeXParser tp, string[] args)
    {
        RowAtom at = new RowAtom(new UnderOverAtom(SymbolAtom.Get("normaldot"), SymbolAtom.Get("normaldot"), TeXConstants.UNIT_MU, 5.2f, false, true));
        at.Add(new SpaceAtom(TeXConstants.UNIT_EM, -0.32f, 0f, 0f));
        at.Add(SymbolAtom.Get("equals"));
        return new TypedAtom(TeXConstants.TYPE_RELATION, TeXConstants.TYPE_RELATION, at);
    }

    public static Atom coloncolonequals_macro(TeXParser tp, string[] args)
    {
        Atom colon = new UnderOverAtom(SymbolAtom.Get("normaldot"), SymbolAtom.Get("normaldot"), TeXConstants.UNIT_MU, 5.2f, false, true);
        RowAtom at = new RowAtom(colon);
        at.Add(colon);
        at.Add(new SpaceAtom(TeXConstants.UNIT_EM, -0.32f, 0f, 0f));
        at.Add(SymbolAtom.Get("equals"));
        return new TypedAtom(TeXConstants.TYPE_RELATION, TeXConstants.TYPE_RELATION, at);
    }

    public static Atom coloncolon_macro(TeXParser tp, string[] args)
    {
        Atom colon = new UnderOverAtom(SymbolAtom.Get("normaldot"), SymbolAtom.Get("normaldot"), TeXConstants.UNIT_MU, 5.2f, false, true);
        RowAtom at = new RowAtom(colon);
        at.Add(colon);
        return new TypedAtom(TeXConstants.TYPE_RELATION, TeXConstants.TYPE_RELATION, at);
    }

    public static Atom colonsim_macro(TeXParser tp, string[] args)
    {
        RowAtom at = new RowAtom(new UnderOverAtom(SymbolAtom.Get("normaldot"), SymbolAtom.Get("normaldot"), TeXConstants.UNIT_MU, 5.2f, false, true));
        at.Add(new SpaceAtom(TeXConstants.UNIT_EM, -0.32f, 0f, 0f));
        at.Add(SymbolAtom.Get("sim"));
        return new TypedAtom(TeXConstants.TYPE_RELATION, TeXConstants.TYPE_RELATION, at);
    }

    public static Atom coloncolonsim_macro(TeXParser tp, string[] args)
    {
        Atom colon = new UnderOverAtom(SymbolAtom.Get("normaldot"), SymbolAtom.Get("normaldot"), TeXConstants.UNIT_MU, 5.2f, false, true);
        RowAtom at = new RowAtom(colon);
        at.Add(colon);
        at.Add(new SpaceAtom(TeXConstants.UNIT_EM, -0.32f, 0f, 0f));
        at.Add(SymbolAtom.Get("sim"));
        return new TypedAtom(TeXConstants.TYPE_RELATION, TeXConstants.TYPE_RELATION, at);
    }

    public static Atom colonapprox_macro(TeXParser tp, string[] args)
    {
        RowAtom at = new RowAtom(new UnderOverAtom(SymbolAtom.Get("normaldot"), SymbolAtom.Get("normaldot"), TeXConstants.UNIT_MU, 5.2f, false, true));
        at.Add(new SpaceAtom(TeXConstants.UNIT_EM, -0.32f, 0f, 0f));
        at.Add(SymbolAtom.Get("approx"));
        return new TypedAtom(TeXConstants.TYPE_RELATION, TeXConstants.TYPE_RELATION, at);
    }

    public static Atom coloncolonapprox_macro(TeXParser tp, string[] args)
    {
        Atom colon = new UnderOverAtom(SymbolAtom.Get("normaldot"), SymbolAtom.Get("normaldot"), TeXConstants.UNIT_MU, 5.2f, false, true);
        RowAtom at = new RowAtom(colon);
        at.Add(colon);
        at.Add(new SpaceAtom(TeXConstants.UNIT_EM, -0.32f, 0f, 0f));
        at.Add(SymbolAtom.Get("approx"));
        return new TypedAtom(TeXConstants.TYPE_RELATION, TeXConstants.TYPE_RELATION, at);
    }

    public static Atom smallfrowneq_macro(TeXParser tp, string[] args)
    {
        Atom at = new UnderOverAtom(SymbolAtom.Get("equals"), SymbolAtom.Get("smallfrown"), TeXConstants.UNIT_MU, -2f, true, true);
        return new TypedAtom(TeXConstants.TYPE_RELATION, TeXConstants.TYPE_RELATION, at);
    }

    public static Atom hstrok_macro(TeXParser tp, string[] args)
    {
        RowAtom ra = new RowAtom(new SpaceAtom(TeXConstants.UNIT_EX, -0.1f, 0f, 0f));
        ra.Add(SymbolAtom.Get("bar"));
        VRowAtom vra = new VRowAtom(new LapedAtom(ra, 'r'));
        vra.setRaise(TeXConstants.UNIT_EX, -0.1f);
        RowAtom at = new RowAtom(vra);
        at.Add(new RomanAtom(new CharAtom('h', tp.formula.textStyle)));
        return at;
    }

    public static Atom Hstrok_macro(TeXParser tp, string[] args)
    {
        RowAtom ra = new RowAtom(new SpaceAtom(TeXConstants.UNIT_EX, 0.28f, 0f, 0f));
        ra.Add(SymbolAtom.Get("textendash"));
        VRowAtom vra = new VRowAtom(new LapedAtom(ra, 'r'));
        vra.setRaise(TeXConstants.UNIT_EX, 0.55f);
        RowAtom at = new RowAtom(vra);
        at.Add(new RomanAtom(new CharAtom('H', tp.formula.textStyle)));
        return at;
    }

    public static Atom dstrok_macro(TeXParser tp, string[] args)
    {
        RowAtom ra = new RowAtom(new SpaceAtom(TeXConstants.UNIT_EX, 0.25f, 0f, 0f));
        ra.Add(SymbolAtom.Get("bar"));
        VRowAtom vra = new VRowAtom(new LapedAtom(ra, 'r'));
        vra.setRaise(TeXConstants.UNIT_EX, -0.1f);
        RowAtom at = new RowAtom(vra);
        at.Add(new RomanAtom(new CharAtom('d', tp.formula.textStyle)));
        return at;
    }

    public static Atom Dstrok_macro(TeXParser tp, string[] args)
    {
        RowAtom ra = new RowAtom(new SpaceAtom(TeXConstants.UNIT_EX, -0.1f, 0f, 0f));
        ra.Add(SymbolAtom.Get("bar"));
        VRowAtom vra = new VRowAtom(new LapedAtom(ra, 'r'));
        vra.setRaise(TeXConstants.UNIT_EX, -0.55f);
        RowAtom at = new RowAtom(vra);
        at.Add(new RomanAtom(new CharAtom('D', tp.formula.textStyle)));
        return at;
    }

    public static Atom kern_macro(TeXParser tp, string[] args)
    {
        float[] info = SpaceAtom.getLength(args[1]);
        if (info.Length == 1)
        {
            throw new ParseException("Error in getting kern in \\kern command !");
        }

        return new SpaceAtom((int)info[0], info[1], 0f, 0f);
    }

    public static Atom char_macro(TeXParser tp, string[] args)
    {
        string number = args[1];
        int radix = 10;
        if (number.StartsWith("0x") || number.StartsWith("0X"))
        {
            number = number.Substring(2);
            radix = 16;
        }
        else if (number.StartsWith("x") || number.StartsWith("X"))
        {
            number = number.Substring(1);
            radix = 16;
        }
        else if (number.StartsWith("0"))
        {
            number = number.Substring(1);
            radix = 8;
        }
        int n = int.parseInt(number, radix);
        return tp.convertCharacter((char)n, true);
    }

    public static Atom T_macro(TeXParser tp, string[] args)
    {
        return new RotateAtom(new TeXFormula(tp, args[1]).root, 180, "origin=cc");
    }

    public static Atom romannumeral_macro(TeXParser tp, string[] args)
    {
        int[] numbers = { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
        string[] letters = { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };
        string roman = "";
        int num = int.parseInt(args[1].Trim());
        for (int i = 0; i < numbers.Length; i++)
        {
            while (num >= numbers[i])
            {
                roman += letters[i];
                num -= numbers[i];
            }
        }

        if (args[0][0] == 'r')
        {
            roman = roman.ToLower();
        }

        return new TeXFormula(roman, false).root;
    }

    public static Atom textcircled_macro(TeXParser tp, string[] args)
    {
        return new TextCircledAtom(new RomanAtom(new TeXFormula(tp, args[1]).root));
    }

    public static Atom textsc_macro(TeXParser tp, string[] args)
    {
        return new SmallCapAtom(new TeXFormula(tp, args[1], false).root);
    }

    public static Atom sc_macro(TeXParser tp, string[] args)
    {
        return new SmallCapAtom(new TeXFormula(tp, tp.getOverArgument(), null, false, tp.isIgnoreWhiteSpace()).root);
    }

    public static Atom quad_macro(TeXParser tp, string[] args)
    {
        return new SpaceAtom(TeXConstants.UNIT_EM, 1f, 0f, 0f);
    }

    public static Atom qquad_macro(TeXParser tp, string[] args)
    {
        return new SpaceAtom(TeXConstants.UNIT_EM, 2f, 0f, 0f);
    }

    public static Atom muskip_macros(TeXParser tp, string[] args)
    {
        int type = 0;
        if (args[0] == (","))
        {
            type = TeXConstants.THINMUSKIP;
        }
        else if (args[0] == (":"))
        {
            type = TeXConstants.MEDMUSKIP;
        }
        else if (args[0] == (";"))
        {
            type = TeXConstants.THICKMUSKIP;
        }
        else if (args[0] == ("thinspace"))
        {
            type = TeXConstants.THINMUSKIP;
        }
        else if (args[0] == ("medspace"))
        {
            type = TeXConstants.MEDMUSKIP;
        }
        else if (args[0] == ("thickspace"))
        {
            type = TeXConstants.THICKMUSKIP;
        }
        else if (args[0] == ("!"))
        {
            type = TeXConstants.NEGTHINMUSKIP;
        }
        else if (args[0] == ("negthinspace"))
        {
            type = TeXConstants.NEGTHINMUSKIP;
        }
        else if (args[0] == ("negmedspace"))
        {
            type = TeXConstants.NEGMEDMUSKIP;
        }
        else if (args[0] == ("negthickspace"))
        {
            type = TeXConstants.NEGTHICKMUSKIP;
        }

        return new SpaceAtom(type);
    }

    public static Atom surd_macro(TeXParser tp, string[] args)
    {
        return new VCenteredAtom(SymbolAtom.Get("surdsign"));
    }

    public static Atom int_macro(TeXParser tp, string[] args)
    {
        Atom integral = SymbolAtom.Get("int").Clone();
        integral.TypeLimits = TeXConstants.SCRIPT_NOLIMITS;
        return integral;
    }

    public static Atom oint_macro(TeXParser tp, string[] args)
    {
        Atom integral = SymbolAtom.Get("oint").Clone();
        integral.TypeLimits = TeXConstants.SCRIPT_NOLIMITS;
        return integral;
    }

    public static Atom iint_macro(TeXParser tp, string[] args)
    {
        Atom integral = SymbolAtom.Get("int").Clone();
        integral.TypeLimits = TeXConstants.SCRIPT_NOLIMITS;
        RowAtom ra = new RowAtom(integral);
        ra.Add(new SpaceAtom(TeXConstants.UNIT_MU, -6f, 0f, 0f));
        ra.Add(integral);
        ra.lookAtLastAtom = true;
        return new TypedAtom(TeXConstants.TYPE_BIG_OPERATOR, TeXConstants.TYPE_BIG_OPERATOR, ra);
    }

    public static Atom iiint_macro(TeXParser tp, string[] args)
    {
        Atom integral = SymbolAtom.Get("int").Clone();
        integral.TypeLimits = TeXConstants.SCRIPT_NOLIMITS;
        RowAtom ra = new RowAtom(integral);
        ra.Add(new SpaceAtom(TeXConstants.UNIT_MU, -6f, 0f, 0f));
        ra.Add(integral);
        ra.Add(new SpaceAtom(TeXConstants.UNIT_MU, -6f, 0f, 0f));
        ra.Add(integral);
        ra.lookAtLastAtom = true;
        return new TypedAtom(TeXConstants.TYPE_BIG_OPERATOR, TeXConstants.TYPE_BIG_OPERATOR, ra);
    }

    public static Atom iiiint_macro(TeXParser tp, string[] args)
    {
        Atom integral = SymbolAtom.Get("int").Clone();
        integral.TypeLimits = TeXConstants.SCRIPT_NOLIMITS;
        RowAtom ra = new RowAtom(integral);
        ra.Add(new SpaceAtom(TeXConstants.UNIT_MU, -6f, 0f, 0f));
        ra.Add(integral);
        ra.Add(new SpaceAtom(TeXConstants.UNIT_MU, -6f, 0f, 0f));
        ra.Add(integral);
        ra.Add(new SpaceAtom(TeXConstants.UNIT_MU, -6f, 0f, 0f));
        ra.Add(integral);
        ra.lookAtLastAtom = true;
        return new TypedAtom(TeXConstants.TYPE_BIG_OPERATOR, TeXConstants.TYPE_BIG_OPERATOR, ra);
    }

    public static Atom idotsint_macro(TeXParser tp, string[] args)
    {
        Atom integral = SymbolAtom.Get("int").Clone();
        integral.TypeLimits = TeXConstants.SCRIPT_NOLIMITS;
        RowAtom ra = new RowAtom(integral);
        ra.Add(new SpaceAtom(TeXConstants.UNIT_MU, -1f, 0f, 0f));
        Atom cdotp = SymbolAtom.Get("cdotp");
        RowAtom cdots = new RowAtom(cdotp);
        cdots.Add(cdotp);
        cdots.Add(cdotp);
        ra.Add(new TypedAtom(TeXConstants.TYPE_INNER, TeXConstants.TYPE_INNER, cdots));
        ra.Add(new SpaceAtom(TeXConstants.UNIT_MU, -1f, 0f, 0f));
        ra.Add(integral);
        ra.lookAtLastAtom = true;
        return new TypedAtom(TeXConstants.TYPE_BIG_OPERATOR, TeXConstants.TYPE_BIG_OPERATOR, ra);
    }

    public static Atom lmoustache_macro(TeXParser tp, string[] args)
    {
        Atom at = new BigDelimiterAtom((SymbolAtom)SymbolAtom.Get("lmoustache").Clone(), 1);
        at.Type = TeXConstants.TYPE_OPENING;
        return at;
    }

    public static Atom rmoustache_macro(TeXParser tp, string[] args)
    {
        Atom at = new BigDelimiterAtom((SymbolAtom)SymbolAtom.Get("rmoustache").Clone(), 1);
        at.Type = TeXConstants.TYPE_CLOSING;
        return at;
    }

    public static Atom insertBreakMark_macro(TeXParser tp, string[] args)
    {
        return new BreakMarkAtom();
    }

    public static Atom jlmXML_macro(TeXParser tp, string[] args)
    {
        Dictionary<string, string> map = tp.formula.jlmXMLMap;
        string str = args[1];
        StringBuilder buffer = new StringBuilder();
        int start = 0;
        int pos;
        while ((pos = str.IndexOf("$")) != -1)
        {
            if (pos < str.Length - 1)
            {
                start = pos;
                while (++start < str.Length && char.IsLetter(str[start])) ;
                string key = str.substring(pos + 1, start);
                string value = map.Get(key);
                if (value != null)
                {
                    buffer.Append(str.substring(0, pos));
                    buffer.Append(value);
                }
                else
                {
                    buffer.Append(str.substring(0, start));
                }
                str = str.Substring(start);
            }
            else
            {
                buffer.Append(str);
                str = "";
            }
        }
        buffer.Append(str);
        str = buffer.ToString();

        return new TeXFormula(tp, str).root;
    }
}
