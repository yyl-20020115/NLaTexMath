/* PredefinedCommands.java
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

 public class PredefinedCommands {

    //public PredefinedCommands() { }

    static PredefinedCommands() {
        MacroInfo.Commands.Add("newcommand", new PredefMacroInfo(0, 2, 2));
        MacroInfo.Commands.Add("renewcommand", new PredefMacroInfo(1, 2, 2));
        MacroInfo.Commands.Add("rule", new PredefMacroInfo(2, 2, 1));
        MacroInfo.Commands.Add("hspace", new PredefMacroInfo(3, 1));
        MacroInfo.Commands.Add("vspace", new PredefMacroInfo(4, 1));
        MacroInfo.Commands.Add("llap", new PredefMacroInfo(5, 1));
        MacroInfo.Commands.Add("rlap", new PredefMacroInfo(6, 1));
        MacroInfo.Commands.Add("clap", new PredefMacroInfo(7, 1));
        MacroInfo.Commands.Add("mathllap", new PredefMacroInfo(8, 1));
        MacroInfo.Commands.Add("mathrlap", new PredefMacroInfo(9, 1));
        MacroInfo.Commands.Add("mathclap", new PredefMacroInfo(10, 1));
        MacroInfo.Commands.Add("includegraphics", new PredefMacroInfo(11, 1, 1));
        MacroInfo.Commands.Add("cfrac", new PredefMacroInfo(12, 2, 1));
        MacroInfo.Commands.Add("frac", new PredefMacroInfo(13, 2));
        MacroInfo.Commands.Add("sfrac", new PredefMacroInfo(14, 2));
        MacroInfo.Commands.Add("genfrac", new PredefMacroInfo(15, 6));
        MacroInfo.Commands.Add("over", new PredefMacroInfo(16, 0));
        MacroInfo.Commands.Add("overwithdelims", new PredefMacroInfo(17, 2));
        MacroInfo.Commands.Add("atop", new PredefMacroInfo(18, 0));
        MacroInfo.Commands.Add("atopwithdelims", new PredefMacroInfo(19, 2));
        MacroInfo.Commands.Add("choose", new PredefMacroInfo(20, 0));
        MacroInfo.Commands.Add("underscore", new PredefMacroInfo(21, 0));
        MacroInfo.Commands.Add("mbox", new PredefMacroInfo(22, 1));
        MacroInfo.Commands.Add("text", new PredefMacroInfo(23, 1));
        MacroInfo.Commands.Add("intertext", new PredefMacroInfo(24, 1));
        MacroInfo.Commands.Add("binom", new PredefMacroInfo(25, 2));
        MacroInfo.Commands.Add("mathbf", new PredefMacroInfo(26, 1));
        MacroInfo.Commands.Add("bf", new PredefMacroInfo(27, 0));
        MacroInfo.Commands.Add("mathbb", new PredefMacroInfo(28, 1));
        MacroInfo.Commands.Add("mathcal", new PredefMacroInfo(29, 1));
        MacroInfo.Commands.Add("cal", new PredefMacroInfo(30, 1));
        MacroInfo.Commands.Add("mathit", new PredefMacroInfo(31, 1));
        MacroInfo.Commands.Add("it", new PredefMacroInfo(32, 0));
        MacroInfo.Commands.Add("mathrm", new PredefMacroInfo(33, 1));
        MacroInfo.Commands.Add("rm", new PredefMacroInfo(34, 0));
        MacroInfo.Commands.Add("mathscr", new PredefMacroInfo(35, 1));
        MacroInfo.Commands.Add("mathsf", new PredefMacroInfo(36, 1));
        MacroInfo.Commands.Add("sf", new PredefMacroInfo(37, 0));
        MacroInfo.Commands.Add("mathtt", new PredefMacroInfo(38, 1));
        MacroInfo.Commands.Add("tt", new PredefMacroInfo(39, 0));
        MacroInfo.Commands.Add("mathfrak", new PredefMacroInfo(40, 1));
        MacroInfo.Commands.Add("mathds", new PredefMacroInfo(41, 1));
        MacroInfo.Commands.Add("frak", new PredefMacroInfo(42, 1));
        MacroInfo.Commands.Add("Bbb", new PredefMacroInfo(43, 1));
        MacroInfo.Commands.Add("oldstylenums", new PredefMacroInfo(44, 1));
        MacroInfo.Commands.Add("bold", new PredefMacroInfo(45, 1));
        MacroInfo.Commands.Add("^", new PredefMacroInfo(46, 1));
        MacroInfo.Commands.Add("\'", new PredefMacroInfo(47, 1));
        MacroInfo.Commands.Add("\"", new PredefMacroInfo(48, 1));
        MacroInfo.Commands.Add("`", new PredefMacroInfo(49, 1));
        MacroInfo.Commands.Add("=", new PredefMacroInfo(50, 1));
        MacroInfo.Commands.Add(".", new PredefMacroInfo(51, 1));
        MacroInfo.Commands.Add("~", new PredefMacroInfo(52, 1));
        MacroInfo.Commands.Add("u", new PredefMacroInfo(53, 1));
        MacroInfo.Commands.Add("v", new PredefMacroInfo(54, 1));
        MacroInfo.Commands.Add("H", new PredefMacroInfo(55, 1));
        MacroInfo.Commands.Add("r", new PredefMacroInfo(56, 1));
        MacroInfo.Commands.Add("U", new PredefMacroInfo(57, 1));
        MacroInfo.Commands.Add("T", new PredefMacroInfo(58, 1));
        MacroInfo.Commands.Add("t", new PredefMacroInfo(59, 1));
        MacroInfo.Commands.Add("accent", new PredefMacroInfo(60, 2));
        MacroInfo.Commands.Add("grkaccent", new PredefMacroInfo(61, 2));
        MacroInfo.Commands.Add("hat", new PredefMacroInfo(62, 1));
        MacroInfo.Commands.Add("widehat", new PredefMacroInfo(63, 1));
        MacroInfo.Commands.Add("tilde", new PredefMacroInfo(64, 1));
        MacroInfo.Commands.Add("acute", new PredefMacroInfo(65, 1));
        MacroInfo.Commands.Add("grave", new PredefMacroInfo(66, 1));
        MacroInfo.Commands.Add("ddot", new PredefMacroInfo(67, 1));
        MacroInfo.Commands.Add("cyrddot", new PredefMacroInfo(68, 1));
        MacroInfo.Commands.Add("mathring", new PredefMacroInfo(69, 1));
        MacroInfo.Commands.Add("bar", new PredefMacroInfo(70, 1));
        MacroInfo.Commands.Add("breve", new PredefMacroInfo(71, 1));
        MacroInfo.Commands.Add("check", new PredefMacroInfo(72, 1));
        MacroInfo.Commands.Add("vec", new PredefMacroInfo(73, 1));
        MacroInfo.Commands.Add("dot", new PredefMacroInfo(74, 1));
        MacroInfo.Commands.Add("widetilde", new PredefMacroInfo(75, 1));
        MacroInfo.Commands.Add("nbsp", new PredefMacroInfo(76, 0));
        MacroInfo.Commands.Add("smallmatrix@@env", new PredefMacroInfo(77, 1));
        MacroInfo.Commands.Add("matrix@@env", new PredefMacroInfo(78, 1));
        MacroInfo.Commands.Add("overrightarrow", new PredefMacroInfo(79, 1));
        MacroInfo.Commands.Add("overleftarrow", new PredefMacroInfo(80, 1));
        MacroInfo.Commands.Add("overleftrightarrow", new PredefMacroInfo(81, 1));
        MacroInfo.Commands.Add("underrightarrow", new PredefMacroInfo(82, 1));
        MacroInfo.Commands.Add("underleftarrow", new PredefMacroInfo(83, 1));
        MacroInfo.Commands.Add("underleftrightarrow", new PredefMacroInfo(84, 1));
        MacroInfo.Commands.Add("xleftarrow", new PredefMacroInfo(85, 1, 1));
        MacroInfo.Commands.Add("xrightarrow", new PredefMacroInfo(86, 1, 1));
        MacroInfo.Commands.Add("underbrace", new PredefMacroInfo(87, 1));
        MacroInfo.Commands.Add("overbrace", new PredefMacroInfo(88, 1));
        MacroInfo.Commands.Add("underbrack", new PredefMacroInfo(89, 1));
        MacroInfo.Commands.Add("overbrack", new PredefMacroInfo(90, 1));
        MacroInfo.Commands.Add("underparen", new PredefMacroInfo(91, 1));
        MacroInfo.Commands.Add("overparen", new PredefMacroInfo(92, 1));
        MacroInfo.Commands.Add("sqrt", new PredefMacroInfo(93, 1, 1));
        MacroInfo.Commands.Add("sqrtsign", new PredefMacroInfo(94, 1));
        MacroInfo.Commands.Add("overline", new PredefMacroInfo(95, 1));
        MacroInfo.Commands.Add("underline", new PredefMacroInfo(96, 1));
        MacroInfo.Commands.Add("mathop", new PredefMacroInfo(97, 1));
        MacroInfo.Commands.Add("mathpunct", new PredefMacroInfo(98, 1));
        MacroInfo.Commands.Add("mathord", new PredefMacroInfo(99, 1));
        MacroInfo.Commands.Add("mathrel", new PredefMacroInfo(100, 1));
        MacroInfo.Commands.Add("mathinner", new PredefMacroInfo(101, 1));
        MacroInfo.Commands.Add("mathbin", new PredefMacroInfo(102, 1));
        MacroInfo.Commands.Add("mathopen", new PredefMacroInfo(103, 1));
        MacroInfo.Commands.Add("mathclose", new PredefMacroInfo(104, 1));
        MacroInfo.Commands.Add("joinrel", new PredefMacroInfo(105, 0));
        MacroInfo.Commands.Add("smash", new PredefMacroInfo(106, 1, 1));
        MacroInfo.Commands.Add("vdots", new PredefMacroInfo(107, 0));
        MacroInfo.Commands.Add("ddots", new PredefMacroInfo(108, 0));
        MacroInfo.Commands.Add("iddots", new PredefMacroInfo(109, 0));
        MacroInfo.Commands.Add("nolimits", new PredefMacroInfo(110, 0));
        MacroInfo.Commands.Add("limits", new PredefMacroInfo(111, 0));
        MacroInfo.Commands.Add("normal", new PredefMacroInfo(112, 0));
        MacroInfo.Commands.Add("(", new PredefMacroInfo(113, 0));
        MacroInfo.Commands.Add("[", new PredefMacroInfo(114, 0));
        MacroInfo.Commands.Add("left", new PredefMacroInfo(115, 1));
        MacroInfo.Commands.Add("middle", new PredefMacroInfo(116, 1));
        MacroInfo.Commands.Add("cr", new PredefMacroInfo(117, 0));
        MacroInfo.Commands.Add("multicolumn", new PredefMacroInfo(118, 3));
        MacroInfo.Commands.Add("hdotsfor", new PredefMacroInfo(119, 1, 1));
        MacroInfo.Commands.Add("array@@env", new PredefMacroInfo(120, 2));
        MacroInfo.Commands.Add("align@@env", new PredefMacroInfo(121, 2));
        MacroInfo.Commands.Add("aligned@@env", new PredefMacroInfo(122, 2));
        MacroInfo.Commands.Add("flalign@@env", new PredefMacroInfo(123, 2));
        MacroInfo.Commands.Add("alignat@@env", new PredefMacroInfo(124, 2));
        MacroInfo.Commands.Add("alignedat@@env", new PredefMacroInfo(125, 2));
        MacroInfo.Commands.Add("multline@@env", new PredefMacroInfo(126, 2));
        MacroInfo.Commands.Add("gather@@env", new PredefMacroInfo(127, 2));
        MacroInfo.Commands.Add("gathered@@env", new PredefMacroInfo(128, 2));
        MacroInfo.Commands.Add("shoveright", new PredefMacroInfo(129, 1));
        MacroInfo.Commands.Add("shoveleft", new PredefMacroInfo(130, 1));
        MacroInfo.Commands.Add("\\", new PredefMacroInfo(131, 0));
        MacroInfo.Commands.Add("newenvironment", new PredefMacroInfo(132, 3));
        MacroInfo.Commands.Add("renewenvironment", new PredefMacroInfo(133, 3));
        MacroInfo.Commands.Add("makeatletter", new PredefMacroInfo(134, 0));
        MacroInfo.Commands.Add("makeatother", new PredefMacroInfo(135, 0));
        MacroInfo.Commands.Add("fbox", new PredefMacroInfo(136, 1));
        MacroInfo.Commands.Add("boxed", new PredefMacroInfo(137, 1));
        MacroInfo.Commands.Add("stackrel", new PredefMacroInfo(138, 2, 1));
        MacroInfo.Commands.Add("stackbin", new PredefMacroInfo(139, 2, 1));
        MacroInfo.Commands.Add("accentset", new PredefMacroInfo(140, 2));
        MacroInfo.Commands.Add("underaccent", new PredefMacroInfo(141, 2));
        MacroInfo.Commands.Add("undertilde", new PredefMacroInfo(142, 1));
        MacroInfo.Commands.Add("overset", new PredefMacroInfo(143, 2));
        MacroInfo.Commands.Add("Braket", new PredefMacroInfo(144, 1));
        MacroInfo.Commands.Add("Set", new PredefMacroInfo(145, 1));
        MacroInfo.Commands.Add("underset", new PredefMacroInfo(146, 2));
        MacroInfo.Commands.Add("boldsymbol", new PredefMacroInfo(147, 1));
        MacroInfo.Commands.Add("LaTeX", new PredefMacroInfo(148, 0));
        MacroInfo.Commands.Add("GeoGebra", new PredefMacroInfo(149, 0));
        MacroInfo.Commands.Add("big", new PredefMacroInfo(150, 1));
        MacroInfo.Commands.Add("Big", new PredefMacroInfo(151, 1));
        MacroInfo.Commands.Add("bigg", new PredefMacroInfo(152, 1));
        MacroInfo.Commands.Add("Bigg", new PredefMacroInfo(153, 1));
        MacroInfo.Commands.Add("bigl", new PredefMacroInfo(154, 1));
        MacroInfo.Commands.Add("Bigl", new PredefMacroInfo(155, 1));
        MacroInfo.Commands.Add("biggl", new PredefMacroInfo(156, 1));
        MacroInfo.Commands.Add("Biggl", new PredefMacroInfo(157, 1));
        MacroInfo.Commands.Add("bigr", new PredefMacroInfo(158, 1));
        MacroInfo.Commands.Add("Bigr", new PredefMacroInfo(159, 1));
        MacroInfo.Commands.Add("biggr", new PredefMacroInfo(160, 1));
        MacroInfo.Commands.Add("Biggr", new PredefMacroInfo(161, 1));
        MacroInfo.Commands.Add("displaystyle", new PredefMacroInfo(162, 0));
        MacroInfo.Commands.Add("textstyle", new PredefMacroInfo(163, 0));
        MacroInfo.Commands.Add("scriptstyle", new PredefMacroInfo(164, 0));
        MacroInfo.Commands.Add("scriptscriptstyle", new PredefMacroInfo(165, 0));
        MacroInfo.Commands.Add("sideset", new PredefMacroInfo(166, 3));
        MacroInfo.Commands.Add("prescript", new PredefMacroInfo(167, 3));
        MacroInfo.Commands.Add("rotatebox", new PredefMacroInfo(168, 2, 1));
        MacroInfo.Commands.Add("reflectbox", new PredefMacroInfo(169, 1));
        MacroInfo.Commands.Add("scalebox", new PredefMacroInfo(170, 2, 2));
        MacroInfo.Commands.Add("resizebox", new PredefMacroInfo(171, 3));
        MacroInfo.Commands.Add("raisebox", new PredefMacroInfo(172, 2, 2));
        MacroInfo.Commands.Add("shadowbox", new PredefMacroInfo(173, 1));
        MacroInfo.Commands.Add("ovalbox", new PredefMacroInfo(174, 1));
        MacroInfo.Commands.Add("doublebox", new PredefMacroInfo(175, 1));
        MacroInfo.Commands.Add("phantom", new PredefMacroInfo(176, 1));
        MacroInfo.Commands.Add("hphantom", new PredefMacroInfo(177, 1));
        MacroInfo.Commands.Add("vphantom", new PredefMacroInfo(178, 1));
        MacroInfo.Commands.Add("sp@breve", new PredefMacroInfo(179, 0));
        MacroInfo.Commands.Add("sp@hat", new PredefMacroInfo(180, 0));
        MacroInfo.Commands.Add("definecolor", new PredefMacroInfo(181, 3));
        MacroInfo.Commands.Add("textcolor", new PredefMacroInfo(182, 2));
        MacroInfo.Commands.Add("fgcolor", new PredefMacroInfo(183, 2));
        MacroInfo.Commands.Add("bgcolor", new PredefMacroInfo(184, 2));
        MacroInfo.Commands.Add("colorbox", new PredefMacroInfo(185, 2));
        MacroInfo.Commands.Add("fcolorbox", new PredefMacroInfo(186, 3));
        MacroInfo.Commands.Add("c", new PredefMacroInfo(187, 1));
        MacroInfo.Commands.Add("IJ", new PredefMacroInfo(188, 0));
        MacroInfo.Commands.Add("ij", new PredefMacroInfo(189, 0));
        MacroInfo.Commands.Add("TStroke", new PredefMacroInfo(190, 0));
        MacroInfo.Commands.Add("tStroke", new PredefMacroInfo(191, 0));
        MacroInfo.Commands.Add("Lcaron", new PredefMacroInfo(192, 0));
        MacroInfo.Commands.Add("tcaron", new PredefMacroInfo(193, 0));
        MacroInfo.Commands.Add("lcaron", new PredefMacroInfo(194, 0));
        MacroInfo.Commands.Add("k", new PredefMacroInfo(195, 1));
        MacroInfo.Commands.Add("cong", new PredefMacroInfo(196, 0));
        MacroInfo.Commands.Add("doteq", new PredefMacroInfo(197, 0));
        MacroInfo.Commands.Add("jlmDynamic", new PredefMacroInfo(198, 1, 1));
        MacroInfo.Commands.Add("jlmExternalFont", new PredefMacroInfo(199, 1));
        MacroInfo.Commands.Add("jlmText", new PredefMacroInfo(200, 1));
        MacroInfo.Commands.Add("jlmTextit", new PredefMacroInfo(201, 1));
        MacroInfo.Commands.Add("jlmTextbf", new PredefMacroInfo(202, 1));
        MacroInfo.Commands.Add("jlmTextitbf", new PredefMacroInfo(203, 1));
        MacroInfo.Commands.Add("DeclareMathSizes", new PredefMacroInfo(204, 4));
        MacroInfo.Commands.Add("magnification", new PredefMacroInfo(205, 1));
        MacroInfo.Commands.Add("hline", new PredefMacroInfo(206, 0));
        MacroInfo.Commands.Add("tiny", new PredefMacroInfo(207, 0));
        MacroInfo.Commands.Add("scriptsize", new PredefMacroInfo(208, 0));
        MacroInfo.Commands.Add("footnotesize", new PredefMacroInfo(209, 0));
        MacroInfo.Commands.Add("small", new PredefMacroInfo(210, 0));
        MacroInfo.Commands.Add("normalsize", new PredefMacroInfo(211, 0));
        MacroInfo.Commands.Add("large", new PredefMacroInfo(212, 0));
        MacroInfo.Commands.Add("Large", new PredefMacroInfo(213, 0));
        MacroInfo.Commands.Add("LARGE", new PredefMacroInfo(214, 0));
        MacroInfo.Commands.Add("huge", new PredefMacroInfo(215, 0));
        MacroInfo.Commands.Add("Huge", new PredefMacroInfo(216, 0));
        MacroInfo.Commands.Add("jlatexmathcumsup", new PredefMacroInfo(217, 1));
        MacroInfo.Commands.Add("jlatexmathcumsub", new PredefMacroInfo(218, 1));
        MacroInfo.Commands.Add("hstrok", new PredefMacroInfo(219, 0));
        MacroInfo.Commands.Add("Hstrok", new PredefMacroInfo(220, 0));
        MacroInfo.Commands.Add("dstrok", new PredefMacroInfo(221, 0));
        MacroInfo.Commands.Add("Dstrok", new PredefMacroInfo(222, 0));
        MacroInfo.Commands.Add("dotminus", new PredefMacroInfo(223, 0));
        MacroInfo.Commands.Add("ratio", new PredefMacroInfo(224, 0));
        MacroInfo.Commands.Add("smallfrowneq", new PredefMacroInfo(225, 0));
        MacroInfo.Commands.Add("geoprop", new PredefMacroInfo(226, 0));
        MacroInfo.Commands.Add("minuscolon", new PredefMacroInfo(227, 0));
        MacroInfo.Commands.Add("minuscoloncolon", new PredefMacroInfo(228, 0));
        MacroInfo.Commands.Add("simcolon", new PredefMacroInfo(229, 0));
        MacroInfo.Commands.Add("simcoloncolon", new PredefMacroInfo(230, 0));
        MacroInfo.Commands.Add("approxcolon", new PredefMacroInfo(231, 0));
        MacroInfo.Commands.Add("approxcoloncolon", new PredefMacroInfo(232, 0));
        MacroInfo.Commands.Add("coloncolon", new PredefMacroInfo(233, 0));
        MacroInfo.Commands.Add("equalscolon", new PredefMacroInfo(234, 0));
        MacroInfo.Commands.Add("equalscoloncolon", new PredefMacroInfo(235, 0));
        MacroInfo.Commands.Add("colonminus", new PredefMacroInfo(236, 0));
        MacroInfo.Commands.Add("coloncolonminus", new PredefMacroInfo(237, 0));
        MacroInfo.Commands.Add("colonequals", new PredefMacroInfo(238, 0));
        MacroInfo.Commands.Add("coloncolonequals", new PredefMacroInfo(239, 0));
        MacroInfo.Commands.Add("colonsim", new PredefMacroInfo(240, 0));
        MacroInfo.Commands.Add("coloncolonsim", new PredefMacroInfo(241, 0));
        MacroInfo.Commands.Add("colonapprox", new PredefMacroInfo(242, 0));
        MacroInfo.Commands.Add("coloncolonapprox", new PredefMacroInfo(243, 0));
        MacroInfo.Commands.Add("kern", new PredefMacroInfo(244, 1));
        MacroInfo.Commands.Add("char", new PredefMacroInfo(245, 1));
        MacroInfo.Commands.Add("roman", new PredefMacroInfo(246, 1));
        MacroInfo.Commands.Add("Roman", new PredefMacroInfo(247, 1));
        MacroInfo.Commands.Add("textcircled", new PredefMacroInfo(248, 1));
        MacroInfo.Commands.Add("textsc", new PredefMacroInfo(249, 1));
        MacroInfo.Commands.Add("sc", new PredefMacroInfo(250, 0));
        MacroInfo.Commands.Add(",", new PredefMacroInfo(251, 0));
        MacroInfo.Commands.Add(":", new PredefMacroInfo(252, 0));
        MacroInfo.Commands.Add(";", new PredefMacroInfo(253, 0));
        MacroInfo.Commands.Add("thinspace", new PredefMacroInfo(254, 0));
        MacroInfo.Commands.Add("medspace", new PredefMacroInfo(255, 0));
        MacroInfo.Commands.Add("thickspace", new PredefMacroInfo(256, 0));
        MacroInfo.Commands.Add("!", new PredefMacroInfo(257, 0));
        MacroInfo.Commands.Add("negthinspace", new PredefMacroInfo(258, 0));
        MacroInfo.Commands.Add("negmedspace", new PredefMacroInfo(259, 0));
        MacroInfo.Commands.Add("negthickspace", new PredefMacroInfo(260, 0));
        MacroInfo.Commands.Add("quad", new PredefMacroInfo(261, 0));
        MacroInfo.Commands.Add("surd", new PredefMacroInfo(262, 0));
        MacroInfo.Commands.Add("iint", new PredefMacroInfo(263, 0));
        MacroInfo.Commands.Add("iiint", new PredefMacroInfo(264, 0));
        MacroInfo.Commands.Add("iiiint", new PredefMacroInfo(265, 0));
        MacroInfo.Commands.Add("idotsint", new PredefMacroInfo(266, 0));
        MacroInfo.Commands.Add("int", new PredefMacroInfo(267, 0));
        MacroInfo.Commands.Add("oint", new PredefMacroInfo(268, 0));
        MacroInfo.Commands.Add("lmoustache", new PredefMacroInfo(269, 0));
        MacroInfo.Commands.Add("rmoustache", new PredefMacroInfo(270, 0));
        MacroInfo.Commands.Add("-", new PredefMacroInfo(271, 0));
        MacroInfo.Commands.Add("jlmXML", new PredefMacroInfo(272, 1));
        MacroInfo.Commands.Add("above", new PredefMacroInfo(273, 0));
        MacroInfo.Commands.Add("abovewithdelims", new PredefMacroInfo(274, 2));
        MacroInfo.Commands.Add("st", new PredefMacroInfo(275, 1));
        MacroInfo.Commands.Add("fcscore", new PredefMacroInfo(276, 1));
        MacroInfo.Commands.Add("mathnormal", new PredefMacroInfo(277, 1));
        MacroInfo.Commands.Add("qquad", new PredefMacroInfo(278, 0));
        MacroInfo.Commands.Add("longdiv", new PredefMacroInfo(279, 2));
        MacroInfo.Commands.Add("questeq", new PredefMacroInfo(280, 0));
        MacroInfo.Commands.Add("bangle", new PredefMacroInfo(281, 0));
        MacroInfo.Commands.Add("brace", new PredefMacroInfo(282, 0));
        MacroInfo.Commands.Add("brack", new PredefMacroInfo(283, 0));
    }
}
