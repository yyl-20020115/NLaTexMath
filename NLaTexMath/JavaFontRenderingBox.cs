/* ScaleBox.java
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
public class JavaFontRenderingBox : Box {

    private static readonly Graphics TEMPGRAPHIC = new Bitmap(1, 1, Bitmap.TYPE_INT_ARGB).createGraphics();

    //TODO:
    private static Font font = null;// new Font("Serif", Font.PLAIN, 10);

    //private TextLayout text;
    private float size;
    private static TextAttribute KERNING;
    private static TextAttribute LIGATURES;
    private static int KERNING_ON;
    private static int LIGATURES_ON;

    static JavaFontRenderingBox(){
        try { // to avoid problems with Java 1.5
            //KERNING = (TextAttribute) (TextAttribute..getField("KERNING").Get(TextAttribute.));
            //KERNING_ON = (int) (TextAttribute..getField("KERNING_ON").Get(TextAttribute.));
            //LIGATURES = (TextAttribute) (TextAttribute..getField("LIGATURES").Get(TextAttribute.));
            //LIGATURES_ON = (int) (TextAttribute..getField("LIGATURES_ON").Get(TextAttribute.));
        } catch (Exception e) { }
    }

    public JavaFontRenderingBox(string str, int type, float size, Font f, bool kerning) {
        this.size = size;

        if (kerning && KERNING != null) {
            Dictionary<TextAttribute, object> map = new ();
            map.Add(KERNING, KERNING_ON);
            map.Add(LIGATURES, LIGATURES_ON);
            f = f.deriveFont(map);
        }

        this.text = new TextLayout(str, f.deriveFont(type), TEMPGRAPHIC.getFontRenderContext());
        RectangleF rect = text.getBounds();
        this.height = (float) (-rect.getY() * size / 10);
        this.depth = (float) (rect.getHeight() * size / 10) - this.height;
        this.width = (float) ((rect.getWidth() + rect.getX() + 0.4f) * size / 10);
    }

    public JavaFontRenderingBox(string str, int type, float size) 
    : this(str, type, size, font, true)
    {
        ;
    }

    public static void setFont(string name) {
        font = new Font(name, Font.PLAIN, 10);
    }

    public override void Draw(Graphics g2, float x, float y) {
        DrawDebug(g2, x, y);
        g2.translate(x, y);
        g2.scale(0.1 * size, 0.1 * size);
        text.draw(g2, 0, 0);
        g2.scale(10 / size, 10 / size);
        g2.translate(-x, -y);
    }

    public override int LastFontId => 0;
}
