/* ScaleBox.cs
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

namespace NLaTexMath;

using System.Drawing;

/**
 * A box representing a scaled box.
 */
public class JavaFontRenderingBox : Box
{

    //private static readonly Graphics TEMPGRAPHIC = new Bitmap(1, 1).createGraphics();

    public static Font DefaultFont { get; protected set; } = new Font("Serif",10.0f, Fonts.PLAIN);

    public static void SetFont(string name) => DefaultFont = new Font(name, 10, Fonts.PLAIN);

    //private TextLayout text;
    private readonly float size;
    //private static TextAttribute KERNING;
    //private static TextAttribute LIGATURES;
    private static int KERNING_ON;
    private static int LIGATURES_ON;

    static JavaFontRenderingBox()
    {
        //try
        //{ // to avoid problems with Java 1.5
        //    //KERNING = (TextAttribute) (TextAttribute..getField("KERNING").Get(TextAttribute.));
        //    //KERNING_ON = (int) (TextAttribute..getField("KERNING_ON").Get(TextAttribute.));
        //    //LIGATURES = (TextAttribute) (TextAttribute..getField("LIGATURES").Get(TextAttribute.));
        //    //LIGATURES_ON = (int) (TextAttribute..getField("LIGATURES_ON").Get(TextAttribute.));
        //}
        //catch (Exception e) { }
    }

    public JavaFontRenderingBox(string str, FontStyle type, float size, Font f, bool kerning)
    {
        this.size = size;

        //if (kerning && KERNING != null)
        //{
        //    Dictionary<TextAttribute, object> map = new()
        //    {
        //        { KERNING, KERNING_ON },
        //        { LIGATURES, LIGATURES_ON }
        //    };
        //    //f = f.deriveFont(map);
        //}
        //this.text = new TextLayout(str, f.deriveFont(type), TEMPGRAPHIC.getFontRenderContext());
        //RectangleF rect = text.getBounds();


        RectangleF rect = new();
        this.height = (float)(-rect.Y * size / 10.0f);
        this.depth = (float)(rect.Height * size / 10) - this.height;
        this.width = (float)((rect.Width + rect.X + 0.4f) * size / 10);
    }

    public JavaFontRenderingBox(string str, FontStyle type, float size)
    : this(str, type, size, DefaultFont, true)
    {
    }


    public override void Draw(Graphics g, float x, float y)
    {
        DrawDebug(g, x, y);
        var t = g.Transform.Clone();
        g.Transform.Translate(x, y);
        g.Transform.Scale(0.1f * size, 0.1f * size);
        //TODO:
        //text.draw(g, 0, 0);
        g.Transform.Scale(10 / size, 10 / size);
        g.Transform.Translate(-x, -y);
    }

    public override int LastFontId => 0;
}
