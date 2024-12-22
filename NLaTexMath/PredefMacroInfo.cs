/* PredefMacroInfo.cs
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

/**
 * Class to load the predefined commands. Mainly wrote to avoid the use of the Java reflection.
 */
public class PredefMacroInfo : MacroInfo
{

    private readonly int id;

    public PredefMacroInfo(int id, int nbArgs, int posOpts) : base(nbArgs, posOpts) => this.id = id;

    public PredefMacroInfo(int id, int nbArgs) : base(nbArgs) => this.id = id;

    public override object? Invoke(TeXParser tp, string[] args) => InvokeID(id, tp, args);

    private static object? InvokeID(int id, TeXParser tp, string[] args)
    {
        try
        {
            return id switch
            {
                0 => PredefMacros.newcommand_macro(tp, args),
                1 => PredefMacros.renewcommand_macro(tp, args),
                2 => PredefMacros.Rule_macro(tp, args),
                3 or 4 => PredefMacros.Hvspace_macro(tp, args),
                5 or 6 or 7 => PredefMacros.Clrlap_macro(tp, args),
                8 or 9 or 10 => PredefMacros.Mathclrlap_macro(tp, args),
                11 => PredefMacros.Includegraphics_macro(tp, args),
                12 => PredefMacros.Cfrac_macro(tp, args),
                13 => PredefMacros.Frac_macro(tp, args),
                14 => PredefMacros.sfrac_macro(tp, args),
                15 => PredefMacros.Genfrac_macro(tp, args),
                16 => PredefMacros.Over_macro(tp, args),
                17 => PredefMacros.Overwithdelims_macro(tp, args),
                18 => PredefMacros.Atop_macro(tp, args),
                19 => PredefMacros.Atopwithdelims_macro(tp, args),
                20 => PredefMacros.Choose_macro(tp, args),
                21 => PredefMacros.Underscore_macro(tp, args),
                22 => PredefMacros.Mbox_macro(tp, args),
                23 => PredefMacros.Text_macro(tp, args),
                24 => PredefMacros.intertext_macro(tp, args),
                25 => PredefMacros.Binom_macro(tp, args),
                26 => PredefMacros.mathbf_macro(tp, args),
                27 => PredefMacros.bf_macro(tp, args),
                28 => PredefMacros.textstyle_macros(tp, args),
                29 => PredefMacros.textstyle_macros(tp, args),
                30 => PredefMacros.textstyle_macros(tp, args),
                31 => PredefMacros.mathit_macro(tp, args),
                32 => PredefMacros.it_macro(tp, args),
                33 => PredefMacros.mathrm_macro(tp, args),
                34 => PredefMacros.rm_macro(tp, args),
                35 => PredefMacros.textstyle_macros(tp, args),
                36 => PredefMacros.mathsf_macro(tp, args),
                37 => PredefMacros.sf_macro(tp, args),
                38 => PredefMacros.mathtt_macro(tp, args),
                39 => PredefMacros.tt_macro(tp, args),
                40 or 41 or 42 or 43 or 44 or 45 => PredefMacros.textstyle_macros(tp, args),
                46 or 47 or 48 or 49 or 50 or 51 or 52 or 53 or 54 or 55 or 56 or 57 => PredefMacros.Accentbis_macros(tp, args),
                58 => PredefMacros.T_macro(tp, args),
                59 => PredefMacros.Accentbis_macros(tp, args),
                60 => PredefMacros.Accent_macro(tp, args),
                61 => PredefMacros.Grkaccent_macro(tp, args),
                62 or 63 or 64 or 65 or 66 or 67 or 68 or 69 or 70 or 71 or 72 or 73 or 74 or 75 => PredefMacros.Accent_macros(tp, args),
                76 => PredefMacros.nbsp_macro(tp, args),
                77 => PredefMacros.smallmatrixATATenv_macro(tp, args),
                78 => PredefMacros.matrixATATenv_macro(tp, args),
                79 => PredefMacros.overrightarrow_macro(tp, args),
                80 => PredefMacros.overleftarrow_macro(tp, args),
                81 => PredefMacros.overleftrightarrow_macro(tp, args),
                82 => PredefMacros.underrightarrow_macro(tp, args),
                83 => PredefMacros.underleftarrow_macro(tp, args),
                84 => PredefMacros.underleftrightarrow_macro(tp, args),
                85 => PredefMacros.xleftarrow_macro(tp, args),
                86 => PredefMacros.xrightarrow_macro(tp, args),
                87 => PredefMacros.underbrace_macro(tp, args),
                88 => PredefMacros.overbrace_macro(tp, args),
                89 => PredefMacros.underbrack_macro(tp, args),
                90 => PredefMacros.overbrack_macro(tp, args),
                91 => PredefMacros.underparen_macro(tp, args),
                92 => PredefMacros.overparen_macro(tp, args),
                93 or 94 => PredefMacros.sqrt_macro(tp, args),
                95 => PredefMacros.overline_macro(tp, args),
                96 => PredefMacros.underline_macro(tp, args),
                97 => PredefMacros.mathop_macro(tp, args),
                98 => PredefMacros.mathpunct_macro(tp, args),
                99 => PredefMacros.mathord_macro(tp, args),
                100 => PredefMacros.mathrel_macro(tp, args),
                101 => PredefMacros.mathinner_macro(tp, args),
                102 => PredefMacros.mathbin_macro(tp, args),
                103 => PredefMacros.mathopen_macro(tp, args),
                104 => PredefMacros.mathclose_macro(tp, args),
                105 => PredefMacros.joinrel_macro(tp, args),
                106 => PredefMacros.smash_macro(tp, args),
                107 => PredefMacros.vdots_macro(tp, args),
                108 => PredefMacros.ddots_macro(tp, args),
                109 => PredefMacros.iddots_macro(tp, args),
                110 => PredefMacros.nolimits_macro(tp, args),
                111 => PredefMacros.limits_macro(tp, args),
                112 => PredefMacros.normal_macro(tp, args),
                113 => PredefMacros.leftparenthesis_macro(tp, args),
                114 => PredefMacros.leftbracket_macro(tp, args),
                115 => PredefMacros.left_macro(tp, args),
                116 => PredefMacros.middle_macro(tp, args),
                117 => PredefMacros.cr_macro(tp, args),
                118 => PredefMacros.multicolumn_macro(tp, args),
                119 => PredefMacros.hdotsfor_macro(tp, args),
                120 => PredefMacros.arrayATATenv_macro(tp, args),
                121 => PredefMacros.alignATATenv_macro(tp, args),
                122 => PredefMacros.alignedATATenv_macro(tp, args),
                123 => PredefMacros.FlalignATATenv_macro(tp, args),
                124 => PredefMacros.alignatATATenv_macro(tp, args),
                125 => PredefMacros.alignedatATATenv_macro(tp, args),
                126 => PredefMacros.multlineATATenv_macro(tp, args),
                127 => PredefMacros.gatherATATenv_macro(tp, args),
                128 => PredefMacros.gatheredATATenv_macro(tp, args),
                129 => PredefMacros.shoveright_macro(tp, args),
                130 => PredefMacros.shoveleft_macro(tp, args),
                131 => PredefMacros.backslashcr_macro(tp, args),
                132 => PredefMacros.newenvironment_macro(tp, args),
                133 => PredefMacros.renewenvironment_macro(tp, args),
                134 => PredefMacros.makeatletter_macro(tp, args),
                135 => PredefMacros.makeatother_macro(tp, args),
                136 or 137 => PredefMacros.fbox_macro(tp, args),
                138 => PredefMacros.stackrel_macro(tp, args),
                139 => PredefMacros.stackbin_macro(tp, args),
                140 => PredefMacros.accentset_macro(tp, args),
                141 => PredefMacros.underaccent_macro(tp, args),
                142 => PredefMacros.undertilde_macro(tp, args),
                143 => PredefMacros.overset_macro(tp, args),
                144 => PredefMacros.Braket_macro(tp, args),
                145 => PredefMacros.Set_macro(tp, args),
                146 => PredefMacros.underset_macro(tp, args),
                147 => PredefMacros.boldsymbol_macro(tp, args),
                148 => PredefMacros.LaTeX_macro(tp, args),
                149 => PredefMacros.GeoGebra_macro(tp, args),
                150 => PredefMacros.big_macro(tp, args),
                151 => PredefMacros.Big_macro(tp, args),
                152 => PredefMacros.bigg_macro(tp, args),
                153 => PredefMacros.Bigg_macro(tp, args),
                154 => PredefMacros.bigl_macro(tp, args),
                155 => PredefMacros.Bigl_macro(tp, args),
                156 => PredefMacros.biggl_macro(tp, args),
                157 => PredefMacros.Biggl_macro(tp, args),
                158 => PredefMacros.bigr_macro(tp, args),
                159 => PredefMacros.Bigr_macro(tp, args),
                160 => PredefMacros.biggr_macro(tp, args),
                161 => PredefMacros.Biggr_macro(tp, args),
                162 => PredefMacros.displaystyle_macro(tp, args),
                163 => PredefMacros.textstyle_macro(tp, args),
                164 => PredefMacros.scriptstyle_macro(tp, args),
                165 => PredefMacros.scriptscriptstyle_macro(tp, args),
                166 => PredefMacros.sideset_macro(tp, args),
                167 => PredefMacros.prescript_macro(tp, args),
                168 => PredefMacros.rotatebox_macro(tp, args),
                169 => PredefMacros.reflectbox_macro(tp, args),
                170 => PredefMacros.scalebox_macro(tp, args),
                171 => PredefMacros.resizebox_macro(tp, args),
                172 => PredefMacros.raisebox_macro(tp, args),
                173 => PredefMacros.shadowbox_macro(tp, args),
                174 => PredefMacros.ovalbox_macro(tp, args),
                175 => PredefMacros.doublebox_macro(tp, args),
                176 => PredefMacros.phantom_macro(tp, args),
                177 => PredefMacros.hphantom_macro(tp, args),
                178 => PredefMacros.vphantom_macro(tp, args),
                179 => PredefMacros.SpATbreve_macro(tp, args),
                180 => PredefMacros.SpAThat_macro(tp, args),
                181 => PredefMacros.definecolor_macro(tp, args),
                182 => PredefMacros.textcolor_macro(tp, args),
                183 => PredefMacros.fgcolor_macro(tp, args),
                184 => PredefMacros.bgcolor_macro(tp, args),
                185 => PredefMacros.colorbox_macro(tp, args),
                186 => PredefMacros.fcolorbox_macro(tp, args),
                187 => PredefMacros.cedilla_macro(tp, args),
                188 => PredefMacros.IJ_macro(tp, args),
                189 => PredefMacros.IJ_macro(tp, args),
                190 => PredefMacros.TStroke_macro(tp, args),
                191 => PredefMacros.TStroke_macro(tp, args),
                192 => PredefMacros.LCaron_macro(tp, args),
                193 => PredefMacros.tcaron_macro(tp, args),
                194 => PredefMacros.LCaron_macro(tp, args),
                195 => PredefMacros.ogonek_macro(tp, args),
                196 => PredefMacros.cong_macro(tp, args),
                197 => PredefMacros.doteq_macro(tp, args),
                198 => PredefMacros.jlmDynamic_macro(tp, args),
                199 => PredefMacros.jlmExternalFont_macro(tp, args),
                200 => PredefMacros.jlmText_macro(tp, args),
                201 => PredefMacros.jlmTextit_macro(tp, args),
                202 => PredefMacros.jlmTextbf_macro(tp, args),
                203 => PredefMacros.jlmTextitbf_macro(tp, args),
                204 => PredefMacros.DeclareMathSizes_macro(tp, args),
                205 => PredefMacros.magnification_macro(tp, args),
                206 => PredefMacros.hline_macro(tp, args),
                207 or 208 or 209 or 210 or 211 or 212 or 213 or 214 or 215 or 216 => PredefMacros.size_macros(tp, args),
                217 => PredefMacros.jlatexmathcumsup_macro(tp, args),
                218 => PredefMacros.jlatexmathcumsub_macro(tp, args),
                219 => PredefMacros.hstrok_macro(tp, args),
                220 => PredefMacros.Hstrok_macro(tp, args),
                221 => PredefMacros.dstrok_macro(tp, args),
                222 => PredefMacros.Dstrok_macro(tp, args),
                223 => PredefMacros.dotminus_macro(tp, args),
                224 => PredefMacros.ratio_macro(tp, args),
                225 => PredefMacros.smallfrowneq_macro(tp, args),
                226 => PredefMacros.geoprop_macro(tp, args),
                227 => PredefMacros.minuscolon_macro(tp, args),
                228 => PredefMacros.minuscoloncolon_macro(tp, args),
                229 => PredefMacros.simcolon_macro(tp, args),
                230 => PredefMacros.simcoloncolon_macro(tp, args),
                231 => PredefMacros.approxcolon_macro(tp, args),
                232 => PredefMacros.approxcoloncolon_macro(tp, args),
                233 => PredefMacros.coloncolon_macro(tp, args),
                234 => PredefMacros.equalscolon_macro(tp, args),
                235 => PredefMacros.equalscoloncolon_macro(tp, args),
                236 => PredefMacros.colonminus_macro(tp, args),
                237 => PredefMacros.coloncolonminus_macro(tp, args),
                238 => PredefMacros.colonequals_macro(tp, args),
                239 => PredefMacros.coloncolonequals_macro(tp, args),
                240 => PredefMacros.colonsim_macro(tp, args),
                241 => PredefMacros.coloncolonsim_macro(tp, args),
                242 => PredefMacros.colonapprox_macro(tp, args),
                243 => PredefMacros.coloncolonapprox_macro(tp, args),
                244 => PredefMacros.Kern_macro(tp, args),
                245 => PredefMacros.Char_macro(tp, args),
                246 or 247 => PredefMacros.Romannumeral_macro(tp, args),
                248 => PredefMacros.Textcircled_macro(tp, args),
                249 => PredefMacros.Textsc_macro(tp, args),
                250 => PredefMacros.Sc_macro(tp, args),
                251 or 252 or 253 or 254 or 255 or 256 or 257 or 258 or 259 or 260 => PredefMacros.Muskip_macros(tp, args),
                261 => PredefMacros.Quad_macro(tp, args),
                262 => PredefMacros.Surd_macro(tp, args),
                263 => PredefMacros.Iint_macro(tp, args),
                264 => PredefMacros.Iiint_macro(tp, args),
                265 => PredefMacros.Iiiint_macro(tp, args),
                266 => PredefMacros.Idotsint_macro(tp, args),
                267 => PredefMacros.Int_macro(tp, args),
                268 => PredefMacros.Oint_macro(tp, args),
                269 => PredefMacros.Lmoustache_macro(tp, args),
                270 => PredefMacros.Rmoustache_macro(tp, args),
                271 => PredefMacros.InsertBreakMark_macro(tp, args),
                272 => PredefMacros.JlmXML_macro(tp, args),
                273 => PredefMacros.above_macro(tp, args),
                274 => PredefMacros.abovewithdelims_macro(tp, args),
                275 => PredefMacros.St_macro(tp, args),
                276 => PredefMacros.fcscore_macro(tp, args),
                277 => PredefMacros.textstyle_macros(tp, args),
                278 => PredefMacros.Qquad_macro(tp, args),
                279 => PredefMacros.Longdiv_macro(tp, args),
                280 => PredefMacros.questeq_macro(tp, args),
                281 => PredefMacros.Bangle_macro(tp, args),
                282 => PredefMacros.Brace_macro(tp, args),
                283 => PredefMacros.Brack_macro(tp, args),
                _ => null,
            };
        }
        catch (Exception e)
        {
            throw new ParseException($"Problem with command {args[0]} at position {tp.Line}:{tp.Col}\n{e.Message}");
        }
    }
}
