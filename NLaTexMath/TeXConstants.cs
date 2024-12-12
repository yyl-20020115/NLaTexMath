/* TeXConstants.java
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

namespace NLaTexMath;

/**
 * The collection of constants that can be used in the methods of the classes of
 * this package.
 */
public class TeXConstants
{

    // *******************
    // ALIGNMENT CONSTANTS
    // *******************

    /**
     * Alignment constant: extra space will be added to the right of the formula
     */
    public const int ALIGN_LEFT = 0;

    /**
     * Alignment constant: extra space will be added to the left of the formula
     */
    public const int ALIGN_RIGHT = 1;

    /**
     * Alignment constant: the formula will be centered in the middle. This constant
     * can be used for both horizontal and vertical alignment.
     */
    public const int ALIGN_CENTER = 2;

    /**
     * Alignment constant: extra space will be added under the formula
     */
    public const int ALIGN_TOP = 3;

    /**
     * Alignment constant: extra space will be added above the formula
     */
    public const int ALIGN_BOTTOM = 4;

    /**
     * Alignment constant: none
     */
    public const int ALIGN_NONE = 5;

    public const int THINMUSKIP = 1;
    public const int MEDMUSKIP = 2;
    public const int THICKMUSKIP = 3;
    public const int NEGTHINMUSKIP = -1;
    public const int NEGMEDMUSKIP = -2;
    public const int NEGTHICKMUSKIP = -3;

    public const int QUAD = 3;

    public const int SCRIPT_NORMAL = 0;
    public const int SCRIPT_NOLIMITS = 1;
    public const int SCRIPT_LIMITS = 2;

    // *********************
    // SYMBOL TYPE CONSTANTS
    // *********************

    /**
     * Symbol/Atom type: ordinary symbol, e.g. "slash"
     */
    public const int TYPE_ORDINARY = 0;

    /**
     * Symbol/Atom type: big operator (= large operator), e.g. "sum"
     */
    public const int TYPE_BIG_OPERATOR = 1;

    /**
     * Symbol/Atom type: binary operator, e.g. "plus"
     */
    public const int TYPE_BINARY_OPERATOR = 2;

    /**
     * Symbol/Atom type: relation, e.g. "equals"
     */
    public const int TYPE_RELATION = 3;

    /**
     * Symbol/Atom type: opening symbol, e.g. "lbrace"
     */
    public const int TYPE_OPENING = 4;

    /**
     * Symbol/Atom type: closing symbol, e.g. "rbrace"
     */
    public const int TYPE_CLOSING = 5;

    /**
     * Symbol/Atom type: punctuation symbol, e.g. "comma"
     */
    public const int TYPE_PUNCTUATION = 6;

    /**
     * Atom type: inner atom (NOT FOR SYMBOLS!!!)
     */
    public const int TYPE_INNER = 7;

    /**
     * Symbol type: accent, e.g. "hat"
     */
    public const int TYPE_ACCENT = 10;

    public const int TYPE_INTERTEXT = 11;

    public const int TYPE_MULTICOLUMN = 12;

    public const int TYPE_HLINE = 13;

    // ***************************************
    // OVER AND UNDER DELIMITER TYPE CONSTANTS
    // ***************************************

    /**
     * Delimiter type constant for putting delimiters over and under formula's: brace
     */
    public const int DELIM_BRACE = 0;

    /**
     * Delimiter type constant for putting delimiters over and under formula's: square bracket
     */
    public const int DELIM_SQUARE_BRACKET = 1;

    /**
     * Delimiter type constant for putting delimiters over and under formula's: parenthesis
     */
    public const int DELIM_BRACKET = 2;

    /**
     * Delimiter type constant for putting delimiters over and under formula's:
     * arrow with single line pointing to the left
     */
    public const int DELIM_LEFT_ARROW = 3;

    /**
     * Delimiter type constant for putting delimiters over and under formula's:
     * arrow with single line pointing to the right
     */
    public const int DELIM_RIGHT_ARROW = 4;

    /**
     * Delimiter type constant for putting delimiters over and under formula's:
     * arrow with single line pointing to the left and to the right
     */
    public const int DELIM_LEFT_RIGHT_ARROW = 5;

    /**
     * Delimiter type constant for putting delimiters over and under formula's:
     * arrow with two lines pointing to the left
     */
    public const int DELIM_DOUBLE_LEFT_ARROW = 6;

    /**
     * Delimiter type constant for putting delimiters over and under formula's:
     * arrow with two lines pointing to the right
     */
    public const int DELIM_DOUBLE_RIGHT_ARROW = 7;

    /**
     * Delimiter type constant for putting delimiters over and under formula's:
     * arrow with two lines pointing to the left and to the right
     */
    public const int DELIM_DOUBLE_LEFT_RIGHT_ARROW = 8;

    /**
     * Delimiter type constant for putting delimiters over and under formula's:
     * underline once
     */
    public const int DELIM_SINGLE_LINE = 9;

    /**
     * Delimiter type constant for putting delimiters over and under formula's:
     * underline twice
     */
    public const int DELIM_DOUBLE_LINE = 10;

    // *******************
    // TEX STYLE CONSTANTS
    // *******************

    /**
     * TeX style: display style.
     * <p>
     * The large versions of big operators are used and limits are placed under and over
     * these operators (default). Symbols are rendered in the largest size.
     */
    public const int STYLE_DISPLAY = 0;

    /**
     * TeX style: text style.
     * <p>
     * The small versions of big operators are used and limits are attached to
     * these operators as scripts (default). The same size as in the display style
     * is used to render symbols.
     */
    public const int STYLE_TEXT = 2;

    /**
     * TeX style: script style.
     * <p>
     * The same as the text style, but symbols are rendered in a smaller size.
     */
    public const int STYLE_SCRIPT = 4;

    /**
     * TeX style: script_script style.
     * <p>
     * The same as the script style, but symbols are rendered in a smaller size.
     */
    public const int STYLE_SCRIPT_SCRIPT = 6;

    // **************
    // UNIT CONSTANTS
    // **************

    /**
     * Unit constant: em
     * <p>
     * 1 em = the width of the capital 'M' in the current font
     */
    public const int UNIT_EM = 0;

    /**
     * Unit constant: ex
     * <p>
     * 1 ex = the height of the character 'x' in the current font
     */
    public const int UNIT_EX = 1;

    /**
     * Unit constant: pixel
     */
    public const int UNIT_PIXEL = 2;

    /**
     * Unit constant: postscript point
     */
    public const int UNIT_POINT = 3;

    /**
     * Unit constant: pica
     * <p>
     * 1 pica = 12 point
     */
    public const int UNIT_PICA = 4;

    /**
     * Unit constant: math unit (mu)
     * <p>
     * 1 mu = 1/18 em (em taken from the "mufont")
     */
    public const int UNIT_MU = 5;

    /**
     * Unit constant: cm
     * <p>
     * 1 cm = 28.346456693 point
     */
    public const int UNIT_CM = 6;

    /**
     * Unit constant: mm
     * <p>
     * 1 mm = 2.8346456693 point
     */
    public const int UNIT_MM = 7;

    /**
     * Unit constant: in
     * <p>
     * 1 in = 72 point
     */
    public const int UNIT_IN = 8;

    /**
     * Unit constant: sp
     * <p>
     * 1 sp = 65536 point
     */
    public const int UNIT_SP = 9;

    /**
     * Unit constant: in
     * <p>
     * 1 in = 72.27 pt
     */
    public const int UNIT_PT = 10;

    /**
     * Unit constant: in
     * <p>
     * 1 in = 72 point
     */
    public const int UNIT_DD = 11;

    /**
     * Unit constant: in
     * <p>
     * 1 in = 72 point
     */
    public const int UNIT_CC = 12;

    /**
     * Unit constant: x8
     * <p>
     * 1 s8 = 1 default rule thickness
     */
    public const int UNIT_X8 = 13;
}
