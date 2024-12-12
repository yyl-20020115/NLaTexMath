/* ColorAtom.java
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

using System.Drawing;

namespace NLaTexMath;

/**
 * An atom representing the foreground and background color of an other atom.
 */
public class ColorAtom : Atom, Row
{

    public static Dictionary<string, Color> Colors = [];

    // background color
    private readonly Color? background;

    // foreground color
    private readonly Color? color;

    // RowAtom for which the colorsettings apply
    private readonly RowAtom elements;

    static ColorAtom()
    {
        InitColors();
    }

    /**
     * Creates a new ColorAtom that sets the given colors for the given atom.
     * Null for a color means: no specific color set for this atom.
     *
     * @param atom the atom for which the given colors have to be set
     * @param bg the background color
     * @param c the foreground color
     */
    public ColorAtom(Atom atom, Color? bg, Color? c)
    {
        elements = new RowAtom(atom);
        background = bg;
        color = c;
    }

    /**
     * Creates a ColorAtom that overrides the colors of the given ColorAtom if the given
     * colors are not null. If they're null, the old values are used.
     *
     * @param bg the background color
     * @param c the foreground color
     * @param old the ColorAtom for which the colorsettings should be overriden with the
     *                  given colors.
     */
    public ColorAtom(Color? bg, Color? c, ColorAtom old)
    {
        elements = new RowAtom(old.elements);
        background = (bg == null ? old.background : bg);
        color = (c == null ? old.color : c);
    }

    public override Box CreateBox(TeXEnvironment env)
    {
        env.isColored = true;
        TeXEnvironment copy = env.Copy();
        if (background != null)
            copy.Background = background;
        if (color != null)
            copy.Color = color;
        return elements.CreateBox(copy);
    }

    public override int LeftType => elements.LeftType;

    public override int RightType => elements.RightType;

    public void SetPreviousAtom(Dummy prev)
    {
        elements.SetPreviousAtom(prev);
    }

    public static Color GetColor(string s)
    {
        if (s != null)
        {
            s = s.Trim();
            if (s.Length >= 1)
            {
                if (s[0] == '#')
                {
                    return Color.decode(s);
                }
                else if (s.IndexOf(',') != -1 || s.IndexOf(';') != -1)
                {
                    StringTokenizer toks = new StringTokenizer(s, ";,");
                    int n = toks.countTokens();
                    if (n == 3)
                    {
                        // RGB model
                        try
                        {
                            string R = toks.nextToken().Trim();
                            string G = toks.nextToken().Trim();
                            string B = toks.nextToken().Trim();

                            float r = float.parseFloat(R);
                            float g = float.parseFloat(G);
                            float b = float.parseFloat(B);

                            if (r == (int)r && g == (int)g && b == (int)b && R.IndexOf('.') == -1 && G.IndexOf('.') == -1 && B.IndexOf('.') == -1)
                            {
                                int ir = (int)Math.Min(255, Math.Max(0, r));
                                int ig = (int)Math.Min(255, Math.Max(0, g));
                                int ib = (int)Math.Min(255, Math.Max(0, b));
                                return Color.FromArgb(ir, ig, ib);
                            }
                            else
                            {
                                r = (float)Math.Min(1, Math.Max(0, r));
                                g = (float)Math.Min(1, Math.Max(0, g));
                                b = (float)Math.Min(1, Math.Max(0, b));
                                return Color.FromArgb(r, g, b);
                            }
                        }
                        catch (Exception e)
                        {
                            return Color.Black;
                        }
                    }
                    else if (n == 4)
                    {
                        // CMYK model
                        try
                        {
                            float c = float.parseFloat(toks.nextToken().Trim());
                            float m = float.parseFloat(toks.nextToken().Trim());
                            float y = float.parseFloat(toks.nextToken().Trim());
                            float k = float.parseFloat(toks.nextToken().Trim());

                            c = (float)Math.Min(1, Math.Max(0, c));
                            m = (float)Math.Min(1, Math.Max(0, m));
                            y = (float)Math.Min(1, Math.Max(0, y));
                            k = (float)Math.Min(1, Math.Max(0, k));

                            return ConvertColor(c, m, y, k);
                        }
                        catch (Exception e)
                        {
                            return Color.Black;
                        }
                    }
                }

                Color c = Colors[(s.ToLower())];
                if (c != null)
                {
                    return c;
                }
                else
                {
                    if (s.IndexOf('.') != -1)
                    {
                        try
                        {
                            float g = (float)Math.Min(1, Math.Max(float.parseFloat(s), 0));

                            return Color.FromArgb(g, g, g);
                        }
                        catch (Exception e) { }
                    }

                    return Color.decode("#" + s);
                }
            }
        }

        return Color.Black;
    }

    private static void InitColors()
    {
        Colors.Add("black", Color.Black);
        Colors.Add("white", Color.White);
        Colors.Add("red", Color.Red);
        Colors.Add("green", Color.Green);
        Colors.Add("blue", Color.Blue);
        Colors.Add("cyan", Color.Cyan);
        Colors.Add("magenta", Color.Magenta);
        Colors.Add("yellow", Color.Yellow);
        Colors.Add("greenyellow", ConvertColor(0.15f, 0f, 0.69f, 0f));
        Colors.Add("goldenrod", ConvertColor(0f, 0.10f, 0.84f, 0f));
        Colors.Add("dandelion", ConvertColor(0f, 0.29f, 0.84f, 0f));
        Colors.Add("apricot", ConvertColor(0f, 0.32f, 0.52f, 0f));
        Colors.Add("peach", ConvertColor(0f, 0.50f, 0.70f, 0f));
        Colors.Add("melon", ConvertColor(0f, 0.46f, 0.50f, 0f));
        Colors.Add("yelloworange", ConvertColor(0f, 0.42f, 1f, 0f));
        Colors.Add("orange", ConvertColor(0f, 0.61f, 0.87f, 0f));
        Colors.Add("burntorange", ConvertColor(0f, 0.51f, 1f, 0f));
        Colors.Add("bittersweet", ConvertColor(0f, 0.75f, 1f, 0.24f));
        Colors.Add("redorange", ConvertColor(0f, 0.77f, 0.87f, 0f));
        Colors.Add("mahogany", ConvertColor(0f, 0.85f, 0.87f, 0.35f));
        Colors.Add("maroon", ConvertColor(0f, 0.87f, 0.68f, 0.32f));
        Colors.Add("brickred", ConvertColor(0f, 0.89f, 0.94f, 0.28f));
        Colors.Add("orangered", ConvertColor(0f, 1f, 0.50f, 0f));
        Colors.Add("rubinered", ConvertColor(0f, 1f, 0.13f, 0f));
        Colors.Add("wildstrawberry", ConvertColor(0f, 0.96f, 0.39f, 0f));
        Colors.Add("salmon", ConvertColor(0f, 0.53f, 0.38f, 0f));
        Colors.Add("carnationpink", ConvertColor(0f, 0.63f, 0f, 0f));
        Colors.Add("magenta", ConvertColor(0f, 1f, 0f, 0f));
        Colors.Add("violetred", ConvertColor(0f, 0.81f, 0f, 0f));
        Colors.Add("rhodamine", ConvertColor(0f, 0.82f, 0f, 0f));
        Colors.Add("mulberry", ConvertColor(0.34f, 0.90f, 0f, 0.02f));
        Colors.Add("redviolet", ConvertColor(0.07f, 0.90f, 0f, 0.34f));
        Colors.Add("fuchsia", ConvertColor(0.47f, 0.91f, 0f, 0.08f));
        Colors.Add("lavender", ConvertColor(0f, 0.48f, 0f, 0f));
        Colors.Add("thistle", ConvertColor(0.12f, 0.59f, 0f, 0f));
        Colors.Add("orchid", ConvertColor(0.32f, 0.64f, 0f, 0f));
        Colors.Add("darkorchid", ConvertColor(0.40f, 0.80f, 0.20f, 0f));
        Colors.Add("purple", ConvertColor(0.45f, 0.86f, 0f, 0f));
        Colors.Add("plum", ConvertColor(0.50f, 1f, 0f, 0f));
        Colors.Add("violet", ConvertColor(0.79f, 0.88f, 0f, 0f));
        Colors.Add("royalpurple", ConvertColor(0.75f, 0.90f, 0f, 0f));
        Colors.Add("blueviolet", ConvertColor(0.86f, 0.91f, 0f, 0.04f));
        Colors.Add("periwinkle", ConvertColor(0.57f, 0.55f, 0f, 0f));
        Colors.Add("cadetblue", ConvertColor(0.62f, 0.57f, 0.23f, 0f));
        Colors.Add("cornflowerblue", ConvertColor(0.65f, 0.13f, 0f, 0f));
        Colors.Add("midnightblue", ConvertColor(0.98f, 0.13f, 0f, 0.43f));
        Colors.Add("navyblue", ConvertColor(0.94f, 0.54f, 0f, 0f));
        Colors.Add("royalblue", ConvertColor(1f, 0.50f, 0f, 0f));
        Colors.Add("cerulean", ConvertColor(0.94f, 0.11f, 0f, 0f));
        Colors.Add("processblue", ConvertColor(0.96f, 0f, 0f, 0f));
        Colors.Add("skyblue", ConvertColor(0.62f, 0f, 0.12f, 0f));
        Colors.Add("turquoise", ConvertColor(0.85f, 0f, 0.20f, 0f));
        Colors.Add("tealblue", ConvertColor(0.86f, 0f, 0.34f, 0.02f));
        Colors.Add("aquamarine", ConvertColor(0.82f, 0f, 0.30f, 0f));
        Colors.Add("bluegreen", ConvertColor(0.85f, 0f, 0.33f, 0f));
        Colors.Add("emerald", ConvertColor(1f, 0f, 0.50f, 0f));
        Colors.Add("junglegreen", ConvertColor(0.99f, 0f, 0.52f, 0f));
        Colors.Add("seagreen", ConvertColor(0.69f, 0f, 0.50f, 0f));
        Colors.Add("forestgreen", ConvertColor(0.91f, 0f, 0.88f, 0.12f));
        Colors.Add("pinegreen", ConvertColor(0.92f, 0f, 0.59f, 0.25f));
        Colors.Add("limegreen", ConvertColor(0.50f, 0f, 1f, 0f));
        Colors.Add("yellowgreen", ConvertColor(0.44f, 0f, 0.74f, 0f));
        Colors.Add("springgreen", ConvertColor(0.26f, 0f, 0.76f, 0f));
        Colors.Add("olivegreen", ConvertColor(0.64f, 0f, 0.95f, 0.40f));
        Colors.Add("rawsienna", ConvertColor(0f, 0.72f, 1f, 0.45f));
        Colors.Add("sepia", ConvertColor(0f, 0.83f, 1f, 0.70f));
        Colors.Add("brown", ConvertColor(0f, 0.81f, 1f, 0.60f));
        Colors.Add("tan", ConvertColor(0.14f, 0.42f, 0.56f, 0f));
        Colors.Add("gray", ConvertColor(0f, 0f, 0f, 0.50f));
    }

    private static Color ConvertColor(float c, float m, float y, float k)
    {
        float kk = 1f - k;
        return Color.FromArgb((int)(kk * (1f - c)), (int)(kk * (1f - m)), (int)(kk * (1f - y)));
    }
}
