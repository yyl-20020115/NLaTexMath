/* TeXFont.java
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

namespace NLaTexMath;

/**
 * An interface representing a "TeXFont", which is responsible for all the necessary
 * fonts and font information.
 *
 * @author Kurt Vermeulen
 */
public interface TeXFont
{

    public const int NO_FONT = -1;

    /**
     * Derives a new {@link TeXFont} object with the given point size
     *
     * @param pointSize the new size (in points) of the derived {@link TeXFont}
     * @return a <b>copy</b> of this {@link TeXFont} with the new size
     */
    public TeXFont DeriveFont(float pointSize);

    public TeXFont ScaleFont(float factor);

    public float GetScaleFactor();

    public float GetAxisHeight(int style);

    public float GetBigOpSpacing1(int style);

    public float GetBigOpSpacing2(int style);

    public float GetBigOpSpacing3(int style);

    public float GetBigOpSpacing4(int style);

    public float GetBigOpSpacing5(int style);

    /**
     * Get a Char-object specifying the given character in the given text style with
     * metric information depending on the given "style".
     *
     * @param c alphanumeric character
     * @param textStyle the text style in which the character should be drawn
     * @param style the style in which the atom should be drawn
     * @return the Char-object specifying the given character in the given text style
     * @ if there's no text style defined with
     * 		the given name
     */
    public Char GetChar(char c, string textStyle, int style);

    /**
     * Get a Char-object for this specific character containing the metric information
     *
     * @param cf CharFont-object determining a specific character of a specific font
     * @param style the style in which the atom should be drawn
     * @return the Char-object for this character containing metric information
     */
    public Char GetChar(CharFont cf, int style);

    /**
     * Get a Char-object for the given symbol with metric information depending on
     * "style".
     *
     * @param name the symbol name
     * @param style the style in which the atom should be drawn
     * @return a Char-object for this symbol with metric information
     * @ if there's no symbol defined with the given
     * 			name
     */
    public Char GetChar(string name, int style);

    /**
     * Get a Char-object specifying the given character in the default text style with
     * metric information depending on the given "style".
     *
     * @param c alphanumeric character
     * @param style the style in which the atom should be drawn
     * @return the Char-object specifying the given character in the default text style
     */
    public Char GetDefaultChar(char c, int style);

    public float GetDefaultRuleThickness(int style);

    public float GetDenom1(int style);

    public float GetDenom2(int style);

    /**
     * Get an Extension-object for the given Char containing the 4 possible parts to
     * build an arbitrary large variant. This will only be called if isExtensionChar(Char)
     * returns true.
     *
     * @param c a Char-object for a specific character
     * @param style the style in which the atom should be drawn
     * @return an Extension object containing the 4 possible parts
     */
    public Extension GetExtension(Char c, int style);

    /**
     * Get the kern value to be inserted between the given characters in the given style.
     *
     * @param left left character
     * @param right right character
     * @param style the style in which the atom should be drawn
     * @return the kern value between both characters (default 0)
     */
    public float GetKern(CharFont left, CharFont right, int style);

    /**
     * Get the ligature that replaces both characters (if any).
     *
     * @param left left character
     * @param right right character
     * @return a ligature replacing both characters (or null: no ligature)
     */
    public CharFont GetLigature(CharFont left, CharFont right);

    public int GetMuFontId();

    /**
     * Get the next larger version of the given character. This is only called if
     * hasNextLarger(Char) returns true.
     *
     * @param c character
     * @param style the style in which the atom should be drawn
     * @return the next larger version of this character
     */
    public Char GetNextLarger(Char c, int style);

    public float GetNum1(int style);

    public float GetNum2(int style);

    public float GetNum3(int style);

    public float GetQuad(int style, int fontCode);

    /**
     *
     * @return the point size of this TeXFont
     */
    public float GetSize();

    /**
     * Get the kern amount of the character defined by the given CharFont followed by the
     * "skewchar" of it's font. This is used in the algorithm for placing an accent above
     * a single character.
     *
     * @param cf the character and it's font above which an accent has to be placed
     * @param style the render style
     * @return the kern amount of the character defined by cf followed by the
     * "skewchar" of it's font.
     */
    public float GetSkew(CharFont cf, int style);

    public float GetSpace(int style);

    public float GetSub1(int style);

    public float GetSub2(int style);

    public float GetSubDrop(int style);

    public float GetSup1(int style);

    public float GetSup2(int style);

    public float GetSup3(int style);

    public float GetSupDrop(int style);

    public float GetXHeight(int style, int fontCode);

    public float GetEM(int style);

    /**
     *
     * @param c a character
     * @return true if the given character has a larger version, false otherwise
     */
    public bool HasNextLarger(Char c);

    public bool HasSpace(int font);

    public bool Bold { get; set; }
    public bool Roman { get; set; }
    public bool Tt { get; set; }
    public bool It { get; set; }
    public bool Ss { get; set; }

    /**
     *
     * @param c a character
     * @return true if the given character Contains extension information to buid
     * 			an arbitrary large version of this character.
     */
    public bool IsExtensionChar(Char c);

    public TeXFont Copy();
}
