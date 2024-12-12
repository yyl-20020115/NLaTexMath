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
    private readonly Color background;

    // foreground color
    private readonly Color color;

    // RowAtom for which the colorsettings apply
    private readonly RowAtom elements;

    static ColorAtom()
    {
        initColors();
    }

    /**
     * Creates a new ColorAtom that sets the given colors for the given atom.
     * Null for a color means: no specific color set for this atom.
     *
     * @param atom the atom for which the given colors have to be set
     * @param bg the background color
     * @param c the foreground color
     */
    public ColorAtom(Atom atom, Color bg, Color c)
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
    public ColorAtom(Color bg, Color c, ColorAtom old)
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
            copy.            Background = background;
        if (color != null)
            copy.            Color = color;
        return elements.CreateBox(copy);
    }

    public int getLeftType()
    {
        return elements.getLeftType();
    }

    public int getRightType()
    {
        return elements.RightType;
    }

    public void SetPreviousAtom(Dummy prev)
    {
        elements.SetPreviousAtom(prev);
    }

    public static Color getColor(string s)
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

                            return convColor(c, m, y, k);
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

    private static void initColors()
    {
        Colors.Add("black", Color.Black);
        Colors.Add("white", Color.White);
        Colors.Add("red", Color.Red);
        Colors.Add("green", Color.Green);
        Colors.Add("blue", Color.Blue);
        Colors.Add("cyan", Color.Cyan);
        Colors.Add("magenta", Color.Magenta); 
        Colors.Add("yellow", Color.Yellow);
        Colors.Add("greenyellow", convColor(0.15f, 0f, 0.69f, 0f));
        Colors.Add("goldenrod", convColor(0f, 0.10f, 0.84f, 0f));
        Colors.Add("dandelion", convColor(0f, 0.29f, 0.84f, 0f));
        Colors.Add("apricot", convColor(0f, 0.32f, 0.52f, 0f));
        Colors.Add("peach", convColor(0f, 0.50f, 0.70f, 0f));
        Colors.Add("melon", convColor(0f, 0.46f, 0.50f, 0f));
        Colors.Add("yelloworange", convColor(0f, 0.42f, 1f, 0f));
        Colors.Add("orange", convColor(0f, 0.61f, 0.87f, 0f));
        Colors.Add("burntorange", convColor(0f, 0.51f, 1f, 0f));
        Colors.Add("bittersweet", convColor(0f, 0.75f, 1f, 0.24f));
        Colors.Add("redorange", convColor(0f, 0.77f, 0.87f, 0f));
        Colors.Add("mahogany", convColor(0f, 0.85f, 0.87f, 0.35f));
        Colors.Add("maroon", convColor(0f, 0.87f, 0.68f, 0.32f));
        Colors.Add("brickred", convColor(0f, 0.89f, 0.94f, 0.28f));
        Colors.Add("orangered", convColor(0f, 1f, 0.50f, 0f));
        Colors.Add("rubinered", convColor(0f, 1f, 0.13f, 0f));
        Colors.Add("wildstrawberry", convColor(0f, 0.96f, 0.39f, 0f));
        Colors.Add("salmon", convColor(0f, 0.53f, 0.38f, 0f));
        Colors.Add("carnationpink", convColor(0f, 0.63f, 0f, 0f));
        Colors.Add("magenta", convColor(0f, 1f, 0f, 0f));
        Colors.Add("violetred", convColor(0f, 0.81f, 0f, 0f));
        Colors.Add("rhodamine", convColor(0f, 0.82f, 0f, 0f));
        Colors.Add("mulberry", convColor(0.34f, 0.90f, 0f, 0.02f));
        Colors.Add("redviolet", convColor(0.07f, 0.90f, 0f, 0.34f));
        Colors.Add("fuchsia", convColor(0.47f, 0.91f, 0f, 0.08f));
        Colors.Add("lavender", convColor(0f, 0.48f, 0f, 0f));
        Colors.Add("thistle", convColor(0.12f, 0.59f, 0f, 0f));
        Colors.Add("orchid", convColor(0.32f, 0.64f, 0f, 0f));
        Colors.Add("darkorchid", convColor(0.40f, 0.80f, 0.20f, 0f));
        Colors.Add("purple", convColor(0.45f, 0.86f, 0f, 0f));
        Colors.Add("plum", convColor(0.50f, 1f, 0f, 0f));
        Colors.Add("violet", convColor(0.79f, 0.88f, 0f, 0f));
        Colors.Add("royalpurple", convColor(0.75f, 0.90f, 0f, 0f));
        Colors.Add("blueviolet", convColor(0.86f, 0.91f, 0f, 0.04f));
        Colors.Add("periwinkle", convColor(0.57f, 0.55f, 0f, 0f));
        Colors.Add("cadetblue", convColor(0.62f, 0.57f, 0.23f, 0f));
        Colors.Add("cornflowerblue", convColor(0.65f, 0.13f, 0f, 0f));
        Colors.Add("midnightblue", convColor(0.98f, 0.13f, 0f, 0.43f));
        Colors.Add("navyblue", convColor(0.94f, 0.54f, 0f, 0f));
        Colors.Add("royalblue", convColor(1f, 0.50f, 0f, 0f));
        Colors.Add("cerulean", convColor(0.94f, 0.11f, 0f, 0f));
        Colors.Add("processblue", convColor(0.96f, 0f, 0f, 0f));
        Colors.Add("skyblue", convColor(0.62f, 0f, 0.12f, 0f));
        Colors.Add("turquoise", convColor(0.85f, 0f, 0.20f, 0f));
        Colors.Add("tealblue", convColor(0.86f, 0f, 0.34f, 0.02f));
        Colors.Add("aquamarine", convColor(0.82f, 0f, 0.30f, 0f));
        Colors.Add("bluegreen", convColor(0.85f, 0f, 0.33f, 0f));
        Colors.Add("emerald", convColor(1f, 0f, 0.50f, 0f));
        Colors.Add("junglegreen", convColor(0.99f, 0f, 0.52f, 0f));
        Colors.Add("seagreen", convColor(0.69f, 0f, 0.50f, 0f));
        Colors.Add("forestgreen", convColor(0.91f, 0f, 0.88f, 0.12f));
        Colors.Add("pinegreen", convColor(0.92f, 0f, 0.59f, 0.25f));
        Colors.Add("limegreen", convColor(0.50f, 0f, 1f, 0f));
        Colors.Add("yellowgreen", convColor(0.44f, 0f, 0.74f, 0f));
        Colors.Add("springgreen", convColor(0.26f, 0f, 0.76f, 0f));
        Colors.Add("olivegreen", convColor(0.64f, 0f, 0.95f, 0.40f));
        Colors.Add("rawsienna", convColor(0f, 0.72f, 1f, 0.45f));
        Colors.Add("sepia", convColor(0f, 0.83f, 1f, 0.70f));
        Colors.Add("brown", convColor(0f, 0.81f, 1f, 0.60f));
        Colors.Add("tan", convColor(0.14f, 0.42f, 0.56f, 0f));
        Colors.Add("gray", convColor(0f, 0f, 0f, 0.50f));
    }

    private static Color convColor(float c, float m, float y, float k)
    {
        float kk = 1f - k;
        return Color.FromArgb((int)(kk * (1f - c)), (int)(kk * (1f - m)), (int)(kk * (1f - y)));
    }
}
