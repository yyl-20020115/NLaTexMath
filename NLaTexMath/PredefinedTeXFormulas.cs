/* PredefinedTeXFormulas.cs
 * =========================================================================
 * This file is part of the JLaTeXMath Library - http://forge.scilab.org/jlatexmath
 *
 * Copyright (C) 2011 DENIZET Calixte
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

public class PredefinedTeXFormulas
{
    static PredefinedTeXFormulas()
    {
        TeXFormula.predefinedTeXFormulasAsString.Add("qquad", "\\quad\\quad");
        TeXFormula.predefinedTeXFormulasAsString.Add(" ", "\\nbsp");
        TeXFormula.predefinedTeXFormulasAsString.Add("ne", "\\not\\equals");
        TeXFormula.predefinedTeXFormulasAsString.Add("neq", "\\not\\equals");
        TeXFormula.predefinedTeXFormulasAsString.Add("ldots", "\\mathinner{\\ldotp\\ldotp\\ldotp}");
        TeXFormula.predefinedTeXFormulasAsString.Add("dotsc", "\\ldots");
        TeXFormula.predefinedTeXFormulasAsString.Add("dots", "\\ldots");
        TeXFormula.predefinedTeXFormulasAsString.Add("cdots", "\\mathinner{\\cdotp\\cdotp\\cdotp}");
        TeXFormula.predefinedTeXFormulasAsString.Add("dotsb", "\\cdots");
        TeXFormula.predefinedTeXFormulasAsString.Add("dotso", "\\ldots");
        TeXFormula.predefinedTeXFormulasAsString.Add("dotsi", "\\!\\cdots");
        TeXFormula.predefinedTeXFormulasAsString.Add("bowtie", "\\mathrel\\triangleright\\joinrel\\mathrel\\triangleleft");
        TeXFormula.predefinedTeXFormulasAsString.Add("models", "\\mathrel|\\joinrel\\equals");
        TeXFormula.predefinedTeXFormulasAsString.Add("Doteq", "\\doteqdot");
        TeXFormula.predefinedTeXFormulasAsString.Add("{", "\\lbrace");
        TeXFormula.predefinedTeXFormulasAsString.Add("}", "\\rbrace");
        TeXFormula.predefinedTeXFormulasAsString.Add("|", "\\Vert");
        TeXFormula.predefinedTeXFormulasAsString.Add("&", "\\textampersand");
        TeXFormula.predefinedTeXFormulasAsString.Add("%", "\\textpercent");
        TeXFormula.predefinedTeXFormulasAsString.Add("_", "\\underscore");
        TeXFormula.predefinedTeXFormulasAsString.Add("$", "\\textdollar");
        TeXFormula.predefinedTeXFormulasAsString.Add("@", "\\jlatexmatharobase");
        TeXFormula.predefinedTeXFormulasAsString.Add("#", "\\jlatexmathsharp");
        TeXFormula.predefinedTeXFormulasAsString.Add("relbar", "\\mathrel{\\smash-}");
        TeXFormula.predefinedTeXFormulasAsString.Add("hookrightarrow", "\\lhook\\joinrel\\joinrel\\joinrel\\rightarrow");
        TeXFormula.predefinedTeXFormulasAsString.Add("hookleftarrow", "\\leftarrow\\joinrel\\joinrel\\joinrel\\rhook");
        TeXFormula.predefinedTeXFormulasAsString.Add("Longrightarrow", "\\Relbar\\joinrel\\Rightarrow");
        TeXFormula.predefinedTeXFormulasAsString.Add("longrightarrow", "\\relbar\\joinrel\\rightarrow");
        TeXFormula.predefinedTeXFormulasAsString.Add("Longleftarrow", "\\Leftarrow\\joinrel\\Relbar");
        TeXFormula.predefinedTeXFormulasAsString.Add("longleftarrow", "\\leftarrow\\joinrel\\relbar");
        TeXFormula.predefinedTeXFormulasAsString.Add("Longleftrightarrow", "\\Leftarrow\\joinrel\\Rightarrow");
        TeXFormula.predefinedTeXFormulasAsString.Add("longleftrightarrow", "\\leftarrow\\joinrel\\rightarrow");
        TeXFormula.predefinedTeXFormulasAsString.Add("iff", "\\;\\Longleftrightarrow\\;");
        TeXFormula.predefinedTeXFormulasAsString.Add("implies", "\\;\\Longrightarrow\\;");
        TeXFormula.predefinedTeXFormulasAsString.Add("impliedby", "\\;\\Longleftarrow\\;");
        TeXFormula.predefinedTeXFormulasAsString.Add("mapsto", "\\mapstochar\\rightarrow");
        TeXFormula.predefinedTeXFormulasAsString.Add("longmapsto", "\\mapstochar\\longrightarrow");
        TeXFormula.predefinedTeXFormulasAsString.Add("log", "\\mathop{\\mathrm{log}}\\nolimits");
        TeXFormula.predefinedTeXFormulasAsString.Add("lg", "\\mathop{\\mathrm{lg}}\\nolimits");
        TeXFormula.predefinedTeXFormulasAsString.Add("ln", "\\mathop{\\mathrm{ln}}\\nolimits");
        TeXFormula.predefinedTeXFormulasAsString.Add("ln", "\\mathop{\\mathrm{ln}}\\nolimits");
        TeXFormula.predefinedTeXFormulasAsString.Add("lim", "\\mathop{\\mathrm{lim}}");
        TeXFormula.predefinedTeXFormulasAsString.Add("limsup", "\\mathop{\\mathrm{lim\\,sup}}");
        TeXFormula.predefinedTeXFormulasAsString.Add("liminf", "\\mathop{\\mathrm{lim\\,inf}}");
        TeXFormula.predefinedTeXFormulasAsString.Add("injlim", "\\mathop{\\mathrm{inj\\,lim}}");
        TeXFormula.predefinedTeXFormulasAsString.Add("projlim", "\\mathop{\\mathrm{proj\\,lim}}");
        TeXFormula.predefinedTeXFormulasAsString.Add("varinjlim", "\\mathop{\\underrightarrow{\\mathrm{lim}}}");
        TeXFormula.predefinedTeXFormulasAsString.Add("varprojlim", "\\mathop{\\underleftarrow{\\mathrm{lim}}}");
        TeXFormula.predefinedTeXFormulasAsString.Add("varliminf", "\\mathop{\\underline{\\mathrm{lim}}}");
        TeXFormula.predefinedTeXFormulasAsString.Add("varlimsup", "\\mathop{\\overline{\\mathrm{lim}}}");
        TeXFormula.predefinedTeXFormulasAsString.Add("sin", "\\mathop{\\mathrm{sin}}\\nolimits");
        TeXFormula.predefinedTeXFormulasAsString.Add("arcsin", "\\mathop{\\mathrm{arcsin}}\\nolimits");
        TeXFormula.predefinedTeXFormulasAsString.Add("sinh", "\\mathop{\\mathrm{sinh}}\\nolimits");
        TeXFormula.predefinedTeXFormulasAsString.Add("cos", "\\mathop{\\mathrm{cos}}\\nolimits");
        TeXFormula.predefinedTeXFormulasAsString.Add("arccos", "\\mathop{\\mathrm{arccos}}\\nolimits");
        TeXFormula.predefinedTeXFormulasAsString.Add("cot", "\\mathop{\\mathrm{cot}}\\nolimits");
        TeXFormula.predefinedTeXFormulasAsString.Add("arccot", "\\mathop{\\mathrm{arccot}}\\nolimits");
        TeXFormula.predefinedTeXFormulasAsString.Add("cosh", "\\mathop{\\mathrm{cosh}}\\nolimits");
        TeXFormula.predefinedTeXFormulasAsString.Add("tan", "\\mathop{\\mathrm{tan}}\\nolimits");
        TeXFormula.predefinedTeXFormulasAsString.Add("arctan", "\\mathop{\\mathrm{arctan}}\\nolimits");
        TeXFormula.predefinedTeXFormulasAsString.Add("tanh", "\\mathop{\\mathrm{tanh}}\\nolimits");
        TeXFormula.predefinedTeXFormulasAsString.Add("coth", "\\mathop{\\mathrm{coth}}\\nolimits");
        TeXFormula.predefinedTeXFormulasAsString.Add("sec", "\\mathop{\\mathrm{sec}}\\nolimits");
        TeXFormula.predefinedTeXFormulasAsString.Add("arcsec", "\\mathop{\\mathrm{arcsec}}\\nolimits");
        TeXFormula.predefinedTeXFormulasAsString.Add("arccsc", "\\mathop{\\mathrm{arccsc}}\\nolimits");
        TeXFormula.predefinedTeXFormulasAsString.Add("sech", "\\mathop{\\mathrm{sech}}\\nolimits");
        TeXFormula.predefinedTeXFormulasAsString.Add("csc", "\\mathop{\\mathrm{csc}}\\nolimits");
        TeXFormula.predefinedTeXFormulasAsString.Add("csch", "\\mathop{\\mathrm{csch}}\\nolimits");
        TeXFormula.predefinedTeXFormulasAsString.Add("max", "\\mathop{\\mathrm{max}}");
        TeXFormula.predefinedTeXFormulasAsString.Add("min", "\\mathop{\\mathrm{min}}");
        TeXFormula.predefinedTeXFormulasAsString.Add("sup", "\\mathop{\\mathrm{sup}}");
        TeXFormula.predefinedTeXFormulasAsString.Add("inf", "\\mathop{\\mathrm{inf}}");
        TeXFormula.predefinedTeXFormulasAsString.Add("arg", "\\mathop{\\mathrm{arg}}\\nolimits");
        TeXFormula.predefinedTeXFormulasAsString.Add("ker", "\\mathop{\\mathrm{ker}}\\nolimits");
        TeXFormula.predefinedTeXFormulasAsString.Add("dim", "\\mathop{\\mathrm{dim}}\\nolimits");
        TeXFormula.predefinedTeXFormulasAsString.Add("hom", "\\mathop{\\mathrm{hom}}\\nolimits");
        TeXFormula.predefinedTeXFormulasAsString.Add("det", "\\mathop{\\mathrm{det}}");
        TeXFormula.predefinedTeXFormulasAsString.Add("exp", "\\mathop{\\mathrm{exp}}\\nolimits");
        TeXFormula.predefinedTeXFormulasAsString.Add("Pr", "\\mathop{\\mathrm{Pr}}");
        TeXFormula.predefinedTeXFormulasAsString.Add("gcd", "\\mathop{\\mathrm{gcd}}");
        TeXFormula.predefinedTeXFormulasAsString.Add("deg", "\\mathop{\\mathrm{deg}}\\nolimits");
        TeXFormula.predefinedTeXFormulasAsString.Add("bmod", "\\:\\mathbin{\\mathrm{mod}}\\:");
        TeXFormula.predefinedTeXFormulasAsString.Add("JLaTeXMath", "\\mathbb{J}\\LaTeX Math");
        TeXFormula.predefinedTeXFormulasAsString.Add("Mapsto", "\\Mapstochar\\Rightarrow");
        TeXFormula.predefinedTeXFormulasAsString.Add("mapsfrom", "\\leftarrow\\mapsfromchar");
        TeXFormula.predefinedTeXFormulasAsString.Add("Mapsfrom", "\\Leftarrow\\Mapsfromchar");
        TeXFormula.predefinedTeXFormulasAsString.Add("Longmapsto", "\\Mapstochar\\Longrightarrow");
        TeXFormula.predefinedTeXFormulasAsString.Add("longmapsfrom", "\\longleftarrow\\mapsfromchar");
        TeXFormula.predefinedTeXFormulasAsString.Add("Longmapsfrom", "\\Longleftarrow\\Mapsfromchar");
        TeXFormula.predefinedTeXFormulasAsString.Add("arrowvert", "\\vert");
        TeXFormula.predefinedTeXFormulasAsString.Add("Arrowvert", "\\Vert");
        TeXFormula.predefinedTeXFormulasAsString.Add("aa", "\\mathring{a}");
        TeXFormula.predefinedTeXFormulasAsString.Add("AA", "\\mathring{A}");
        TeXFormula.predefinedTeXFormulasAsString.Add("ddag", "\\ddagger");
        TeXFormula.predefinedTeXFormulasAsString.Add("dag", "\\dagger");
        TeXFormula.predefinedTeXFormulasAsString.Add("Doteq", "\\doteqdot");
        TeXFormula.predefinedTeXFormulasAsString.Add("doublecup", "\\Cup");
        TeXFormula.predefinedTeXFormulasAsString.Add("doublecap", "\\Cap");
        TeXFormula.predefinedTeXFormulasAsString.Add("llless", "\\lll");
        TeXFormula.predefinedTeXFormulasAsString.Add("gggtr", "\\ggg");
        TeXFormula.predefinedTeXFormulasAsString.Add("Alpha", "\\mathord{\\mathrm{A}}");
        TeXFormula.predefinedTeXFormulasAsString.Add("Beta", "\\mathord{\\mathrm{B}}");
        TeXFormula.predefinedTeXFormulasAsString.Add("Epsilon", "\\mathord{\\mathrm{E}}");
        TeXFormula.predefinedTeXFormulasAsString.Add("Zeta", "\\mathord{\\mathrm{Z}}");
        TeXFormula.predefinedTeXFormulasAsString.Add("Eta", "\\mathord{\\mathrm{H}}");
        TeXFormula.predefinedTeXFormulasAsString.Add("Iota", "\\mathord{\\mathrm{I}}");
        TeXFormula.predefinedTeXFormulasAsString.Add("Kappa", "\\mathord{\\mathrm{K}}");
        TeXFormula.predefinedTeXFormulasAsString.Add("Mu", "\\mathord{\\mathrm{M}}");
        TeXFormula.predefinedTeXFormulasAsString.Add("Nu", "\\mathord{\\mathrm{N}}");
        TeXFormula.predefinedTeXFormulasAsString.Add("Omicron", "\\mathord{\\mathrm{O}}");
        TeXFormula.predefinedTeXFormulasAsString.Add("Rho", "\\mathord{\\mathrm{P}}");
        TeXFormula.predefinedTeXFormulasAsString.Add("Tau", "\\mathord{\\mathrm{T}}");
        TeXFormula.predefinedTeXFormulasAsString.Add("Chi", "\\mathord{\\mathrm{X}}");
        TeXFormula.predefinedTeXFormulasAsString.Add("hdots", "\\ldots");
        TeXFormula.predefinedTeXFormulasAsString.Add("restriction", "\\upharpoonright");
        TeXFormula.predefinedTeXFormulasAsString.Add("celsius", "\\mathord{{}^\\circ\\mathrm{C}}");
        TeXFormula.predefinedTeXFormulasAsString.Add("micro", "\\textmu");
        TeXFormula.predefinedTeXFormulasAsString.Add("marker", "\\kern{0.25ex}\\rule{0.5ex}{1.2ex}\\kern{0.25ex}");
        TeXFormula.predefinedTeXFormulasAsString.Add("hybull", "\\rule[0.6ex]{1ex}{0.2ex}");
        TeXFormula.predefinedTeXFormulasAsString.Add("block", "\\rule{1ex}{1.2ex}");
        TeXFormula.predefinedTeXFormulasAsString.Add("uhblk", "\\rule[0.6ex]{1ex}{0.6ex}");
        TeXFormula.predefinedTeXFormulasAsString.Add("lhblk", "\\rule{1ex}{0.6ex}");
        TeXFormula.predefinedTeXFormulasAsString.Add("notin", "\\not\\in");
        TeXFormula.predefinedTeXFormulasAsString.Add("rVert", "\\Vert");
        TeXFormula.predefinedTeXFormulasAsString.Add("lVert", "\\Vert");
    }
}
