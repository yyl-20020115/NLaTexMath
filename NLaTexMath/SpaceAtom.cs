/* SpaceAtom.java
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

using NLaTexMath;
using static NLaTexMath.SpaceAtom;

namespace NLaTexMath;

/**
 * An atom representing whitespace. The dimension values can be set using different
 * unit types.
 */
public class SpaceAtom : Atom
{

    private static Dictionary<string, int> units = [];
    static SpaceAtom()
    {
        units.Add("em", TeXConstants.UNIT_EM);
        units.Add("ex", TeXConstants.UNIT_EX);
        units.Add("px", TeXConstants.UNIT_PIXEL);
        units.Add("pix", TeXConstants.UNIT_PIXEL);
        units.Add("pixel", TeXConstants.UNIT_PIXEL);
        units.Add("pt", TeXConstants.UNIT_PT);
        units.Add("bp", TeXConstants.UNIT_POINT);
        units.Add("pica", TeXConstants.UNIT_PICA);
        units.Add("pc", TeXConstants.UNIT_PICA);
        units.Add("mu", TeXConstants.UNIT_MU);
        units.Add("cm", TeXConstants.UNIT_CM);
        units.Add("mm", TeXConstants.UNIT_MM);
        units.Add("in", TeXConstants.UNIT_IN);
        units.Add("sp", TeXConstants.UNIT_SP);
        units.Add("dd", TeXConstants.UNIT_DD);
        units.Add("cc", TeXConstants.UNIT_CC);
    }
    // whether a hard space should be represented
    private bool blankSpace;

    // thinmuskip, medmuskip, thickmuskip
    private int blankType;

    // dimensions
    private float width;
    private float height;
    private float depth;

    // units for the dimensions
    private int wUnit;
    private int hUnit;
    private int dUnit;

    public SpaceAtom()
    {
        blankSpace = true;
    }

    public SpaceAtom(int type)
    {
        blankSpace = true;
        blankType = type;
    }

    public SpaceAtom(int unit, float width, float height, float depth)
    {
        // check if unit is valid
        checkUnit(unit);

        // unit valid
        this.wUnit = unit;
        this.hUnit = unit;
        this.dUnit = unit;
        this.width = width;
        this.height = height;
        this.depth = depth;
    }

    /**
     * Check if the given unit is valid
     *
     * @param unit the unit's integer representation (a constant)
     * @throws InvalidUnitException if the given integer value does not represent
     *                  a valid unit
     */
    public static void checkUnit(int unit)
    {
        if (unit < 0 || unit >= unitConversions.Length)
            throw new InvalidUnitException();
    }

    public SpaceAtom(int widthUnit, float width, int heightUnit, float height,
                     int depthUnit, float depth)
    {
        // check if units are valid
        checkUnit(widthUnit);
        checkUnit(heightUnit);
        checkUnit(depthUnit);

        // all units valid
        wUnit = widthUnit;
        hUnit = heightUnit;
        dUnit = depthUnit;
        this.width = width;
        this.height = height;
        this.depth = depth;
    }

    public static int getUnit(string unit)
    {
        int u = (int)units.Get(unit);
        return u == null ? TeXConstants.UNIT_PIXEL : u.intValue();
    }

    public static float[] getLength(string lgth)
    {
        if (lgth == null)
        {
            return new float[] { TeXConstants.UNIT_PIXEL, 0f };
        }

        int i = 0;
        for (; i < lgth.Length && !char.IsLetter(lgth[i]); i++) ;
        float f = 0;
        try
        {
            f = float.parseFloat(lgth.substring(0, i));
        }
        catch (Exception e)
        {
            return new float[] { float.NaN };
        }

        int unit;
        if (i != lgth.Length)
        {
            unit = getUnit(lgth.Substring(i).ToLower());
        }
        else
        {
            unit = TeXConstants.UNIT_PIXEL;
        }

        return new float[] { (float)unit, f };
    }

    public override Box CreateBox(TeXEnvironment env)
    {
        if (blankSpace)
        {
            if (blankType == 0)
                return new StrutBox(env.Space, 0, 0, 0);
            else
            {
                int bl = blankType < 0 ? -blankType : blankType;
                Box b;
                if (bl == TeXConstants.THINMUSKIP)
                {
                    b = Glue.Get(TeXConstants.TYPE_INNER, TeXConstants.TYPE_BIG_OPERATOR, env);
                }
                else if (bl == TeXConstants.MEDMUSKIP)
                    b = Glue.Get(TeXConstants.TYPE_BINARY_OPERATOR, TeXConstants.TYPE_BIG_OPERATOR, env);
                else
                    b = Glue.Get(TeXConstants.TYPE_RELATION, TeXConstants.TYPE_BIG_OPERATOR, env);
                if (blankType < 0)
                    b.NegWidth();
                return b;
            }
        }
        else
        {
            return new StrutBox(width * getFactor(wUnit, env), height * getFactor(hUnit, env), depth * getFactor(dUnit, env), 0);
        }
    }

    public static float getFactor(int unit, TeXEnvironment env)
    {
        return unitConversions[unit].getPixelConversion(env);
    }

    public interface UnitConversion
    { // NOPMD
        public float getPixelConversion(TeXEnvironment env);
    }


    class EM : UnitConversion
    {//EM
        public float getPixelConversion(TeXEnvironment env) => env.TeXFont.getEM(env.Style);
    }
    class EX : UnitConversion
    {//EX
        public float getPixelConversion(TeXEnvironment env) => env.TeXFont.getXHeight(env.Style, env.LastFontId);
    }
    class PIXEL : UnitConversion
    {//PIXEL
        public float getPixelConversion(TeXEnvironment env) => 1 / env.Size;
    }
    class BP : UnitConversion
    {//BP (or PostScript point)
        public float getPixelConversion(TeXEnvironment env) => TeXFormula.PIXELS_PER_POINT / env.Size;
    }
    class PICA : UnitConversion
    {//PICA
        public float getPixelConversion(TeXEnvironment env) => (12 * TeXFormula.PIXELS_PER_POINT) / env.Size;
    }
    class MU : UnitConversion
    {//MU
        public float getPixelConversion(TeXEnvironment env)
        {
            TeXFont tf = env.TeXFont;
            return tf.getQuad(env.Style, tf.getMuFontId()) / 18;
        }
    }
    class CM : UnitConversion
    {//CM
        public float getPixelConversion(TeXEnvironment env) => 28.346456693f * TeXFormula.PIXELS_PER_POINT / env.Size;
    }
    class MM : UnitConversion
    {//MM
        public float getPixelConversion(TeXEnvironment env) => 2.8346456693f * TeXFormula.PIXELS_PER_POINT / env.Size;
    }
    class IN : UnitConversion
    {//IN
        public float getPixelConversion(TeXEnvironment env) => 72 * TeXFormula.PIXELS_PER_POINT / env.Size;
    }
    class SP : UnitConversion
    {//SP
        public float getPixelConversion(TeXEnvironment env) => 65536 * TeXFormula.PIXELS_PER_POINT / env.Size;
    }
    class PT : UnitConversion
    {//PT (or Standard Anglo-American point)
        public float getPixelConversion(TeXEnvironment env) => .9962640099f * TeXFormula.PIXELS_PER_POINT / env.Size;
    }
    class DD : UnitConversion
    {//DD
        public float getPixelConversion(TeXEnvironment env) => 1.0660349422f * TeXFormula.PIXELS_PER_POINT / env.Size;
    }
    class CC : UnitConversion
    {//CC
        public float getPixelConversion(TeXEnvironment env) => 12.7924193070f * TeXFormula.PIXELS_PER_POINT / env.Size;
    }
    class X8 : UnitConversion
    {//X8
        public float getPixelConversion(TeXEnvironment env) => env.TeXFont.getDefaultRuleThickness(env.Style);
    }
    private static readonly UnitConversion[] unitConversions = [
        new EM(),
        new EX(),
        new PIXEL(),
        new BP(),
        new PICA(),
        new MU(),
        new CM(),
        new MM(),
        new IN(),
        new SP(),
        new PT(),
        new DD(),
        new CC(),
        new X8(),
        ];
}
