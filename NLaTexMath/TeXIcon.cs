/* TeXIcon.cs
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

using System.Drawing;

/**
 * An {@link javax.swing.Icon} implementation that will paint the TeXFormula
 * that created it.
 * <p>
 * This class cannot be instantiated directly. It can be constructed from a
 * TeXFormula using the {@link TeXFormula#createTeXIcon(int,float)} method.
 *
 * @author Kurt Vermeulen
 */
public class TeXIcon
{

    private static readonly Color DefaultColor = Color.Black;

    public static float DefaultSize = -1;
    public static float MagFactor = 0;

    private Box box;

    private float size;

    private Insets insets = new(0, 0, 0, 0);

    private Color? fg;

    public bool isColored = false;

    /**
     * Creates a new icon that will paint the given formula box in the given point size.
     *
     * @param b the formula box to be painted
     * @param size the point size
     */
    public TeXIcon(Box b, float size) : this(b, size, false) { }

    public TeXIcon(Box b, float size, bool trueValues)
    {
        box = b;

        if (DefaultSize != -1)
        {
            size = DefaultSize;
        }

        if (MagFactor != 0)
        {
            this.size = size * Math.Abs(MagFactor);
        }
        else
        {
            this.size = size;
        }

        /* I Add this little value because it seems that tftopl calculates badly
           the height and the depth of certains characters.
        */
        if (!trueValues)
        {
            insets.top += (int)(0.18f * size);
            insets.bottom += (int)(0.18f * size);
            insets.left += (int)(0.18f * size);
            insets.right += (int)(0.18f * size);
        }
    }

    public void SetForeground(Color? fg) => this.fg = fg;

    /**
     * Get the insets of the TeXIcon.
     *
     * @return the insets
     */
    /**
 * Set the insets of the TeXIcon.
 *
 * @param insets the insets
 */
    public Insets Insets { get => insets; set => SetInsets(value, false); }

    /**
     * Set the insets of the TeXIcon.
     *
     * @param insets the insets
     * @param trueValues true to force the true values
     */
    public void SetInsets(Insets insets, bool trueValues)
    {
        this.insets = insets;
        if (!trueValues)
        {
            this.insets.top += (int)(0.18f * size);
            this.insets.bottom += (int)(0.18f * size);
            this.insets.left += (int)(0.18f * size);
            this.insets.right += (int)(0.18f * size);
        }
    }

    /**
     * Change the width of the TeXIcon. The new width must be greater than the current
     * width, otherwise the icon will remain unchanged. The formula will be aligned to the
     * left ({@linkplain TeXConstants#ALIGN_LEFT}), to the right
     * ({@linkplain TeXConstants#ALIGN_RIGHT}) or will be centered
     * in the middle ({@linkplain TeXConstants#ALIGN_CENTER}).
     *
     * @param width the new width of the TeXIcon
     * @param alignment a horizontal alignment constant: LEFT, RIGHT or CENTER
     */
    public void SetIconWidth(int width, int alignment)
    {
        float diff = width - IconWidth;
        if (diff > 0)
            box = new HorizontalBox(box, box.Width + diff, alignment);
    }

    /**
     * Change the height of the TeXIcon. The new height must be greater than the current
     * height, otherwise the icon will remain unchanged. The formula will be aligned on top
     * (TeXConstants.TOP), at the bottom (TeXConstants.BOTTOM) or will be centered
     * in the middle (TeXConstants.CENTER).
     *
     * @param height the new height of the TeXIcon
     * @param alignment a vertical alignment constant: TOP, BOTTOM or CENTER
     */
    public void SetIconHeight(int height, int alignment)
    {
        float diff = height - IconHeight;
        if (diff > 0)
            box = new VerticalBox(box, diff, alignment);
    }

    /**
     * Get the total height of the TeXIcon. This also includes the insets.
     */
    public int IconHeight => ((int)((box.Height) * size + 0.99 + insets.top)) + ((int)((box.Depth) * size + 0.99 + insets.bottom));

    /**
     * Get the total height of the TeXIcon. This also includes the insets.
     */
    public int IconDepth => (int)(box.Depth * size + 0.99 + insets.bottom);

    /**
     * Get the total width of the TeXIcon. This also includes the insets.
     */

    public int IconWidth => (int)(box.Width * size + 0.99 + insets.left + insets.right);

    public float TrueIconHeight => (box.Height + box.Depth) * size;

    /**
     * Get the total height of the TeXIcon. This also includes the insets.
     */
    public float TrueIconDepth => box.Depth * size;

    /**
     * Get the total width of the TeXIcon. This also includes the insets.
     */

    public float TrueIconWidth => box.Width * size;

    public float BaseLine => (float)(((box.Height * size) + 0.99 + insets.top) /
                        ((box.Height + box.Depth) * size + 0.99 + insets.top + insets.bottom));

    public Box Box => box;

    /**
     * Paint the {@link TeXFormula} that created this icon.
     */
    public void PaintIcon(Graphics g, int x = 0, int y = 0)
    {
        var oldHints = g.TextRenderingHint;
        var oldAt = g.Transform;
        g.TextRenderingHint |= System.Drawing.Text.TextRenderingHint.AntiAlias;

        g.Transform.Scale(size, size);

        // draw formula box
        box.Draw(g, (x + insets.left) / size, (y + insets.top) / size + box.Height);

        g.TextRenderingHint = oldHints;
        g.Transform = oldAt;

    }
}
