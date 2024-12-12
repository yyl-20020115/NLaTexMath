/* JLaTeXMathCache.java
 * =========================================================================
 * This file is part of the JLaTeXMath Library - http://forge.scilab.org/p/jlatexmath
 *
 * Copyright (C) 2010 DENIZET Calixte
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

using System.Collections.Concurrent;
using System.Drawing;

namespace NLaTexMath.cache;

/**
 * Class to cache generated image from formulas
 * @author Calixte DENIZET
 */
public  class JLaTeXMathCache {

    private static readonly AffineTransform identity = new AffineTransform();
    private static ConcurrentDictionary<CachedTeXFormula, WeakReference<CachedImage>> cache = new ();
    private static int max = int.MaxValue;
    private static Queue<CachedImage> queue = new Queue<CachedImage>();

    private JLaTeXMathCache() { }

    /**
     * Set max size. Take care the cache will be reinitialized
     * @param max the max size
     */
    public static void setMaxCachedObjects(int max) {
        JLaTeXMathCache.max = Math.Max(max, 1);
        cache.Clear();
        cache = new (JLaTeXMathCache.max);
    }

    /**
     * @param f a formula
     * @param style a style like TeXConstants.STYLE_DISPLAY
     * @param size the size of font
     * @param inset the inset to Add on the top, bottom, left and right
     * @return an array of length 3 containing width, height and depth
     */
    public static int[] getCachedTeXFormulaDimensions(string f, int style, int type, int size, int inset, Color fgcolor)   {
        return getCachedTeXFormulaDimensions(new CachedTeXFormula(f, style, type, size, inset, fgcolor));
    }

    public static int[] getCachedTeXFormulaDimensions(string f, int style, int size, int inset)   {
        return getCachedTeXFormulaDimensions(f, style, 0, size, inset, null);
    }

    /**
     * @param o an object to identify the image in the cache
     * @return an array of length 3 containing width, height and depth
     */
    public static int[] getCachedTeXFormulaDimensions(object o)   {
        if (o == null || !(o is CachedTeXFormula)) {
            return new int[] {0, 0, 0};
        }
        CachedTeXFormula cached = (CachedTeXFormula) o;
        WeakReference<CachedImage> img = cache.Get(cached);
        if (img == null || img.Get() == null) {
            img = makeImage(cached);
        }

        return new int[] {cached.width, cached.height, cached.depth};
    }

    /**
     * Get a cached formula
     * @param f a formula
     * @param style a style like TeXConstants.STYLE_DISPLAY
     * @param size the size of font
     * @param inset the inset to Add on the top, bottom, left and right
     * @return the key in the map
     */
    public static object getCachedTeXFormula(string f, int style, int type, int size, int inset, Color fgcolor)   {
        CachedTeXFormula cached = new CachedTeXFormula(f, style, type, size, inset, fgcolor);
        WeakReference<CachedImage> img = cache.Get(cached);
        if (img == null || img.Get() == null) {
            img = makeImage(cached);
        }

        return cached;
    }

    public static object getCachedTeXFormula(string f, int style, int size, int inset)   {
        return getCachedTeXFormula(f, style, 0, size, inset, null);
    }

    /**
     * Clear the cache
     */
    public static void clearCache() {
        cache.clear();
    }

    /**
     * Remove a formula from the cache
     * @param f a formula
     * @param style a style like TeXConstants.STYLE_DISPLAY
     * @param size the size of font
     * @param inset the inset to Add on the top, bottom, left and right
     */
    public static void removeCachedTeXFormula(string f, int style, int type, int size, int inset, Color fgcolor)   {
        cache.remove(new CachedTeXFormula(f, style, type, size, inset, fgcolor));
    }

    public static void removeCachedTeXFormula(string f, int style, int size, int inset)   {
        removeCachedTeXFormula(f, style, 0, size, inset, null);
    }

    /**
     * Remove a formula from the cache. Take care, remove the object o, invalidate it !
     * @param o an object to identify the image in the cache
     */
    public static void removeCachedTeXFormula(object o)   {
        if (o != null && o is CachedTeXFormula) {
            cache.remove((CachedTeXFormula) o);
        }
    }

    /**
     * Paint a cached formula
     * @param f a formula
     * @param style a style like TeXConstants.STYLE_DISPLAY
     * @param size the size of font
     * @param inset the inset to Add on the top, bottom, left and right
     * @return the key in the map
     */
    public static object paintCachedTeXFormula(string f, int style, int type, int size, int inset, Color fgcolor, Graphics g)   {
        return paintCachedTeXFormula(new CachedTeXFormula(f, style, type, size, inset, fgcolor), g);
    }

    public static object paintCachedTeXFormula(string f, int style, int size, int inset, Graphics g)   {
        return paintCachedTeXFormula(f, style, 0, size, inset, null, g);
    }

    /**
     * Paint a cached formula
     * @param o an object to identify the image in the cache
     * @param g the graphics where to paint the image
     * @return the key in the map
     */
    public static object paintCachedTeXFormula(object o, Graphics g)   {
        if (o == null || !(o is CachedTeXFormula)) {
            return null;
        }
        CachedTeXFormula cached = (CachedTeXFormula) o;
        WeakReference<CachedImage> img = cache.Get(cached);
        if (img == null || img.Get() == null) {
            img = makeImage(cached);
        }
        g.drawImage(img.Get().image, identity, null);

        return cached;
    }

    /**
     * Get a cached formula
     * @param f a formula
     * @param style a style like TeXConstants.STYLE_DISPLAY
     * @param size the size of font
     * @param inset the inset to Add on the top, bottom, left and right
     * @return the cached image
     */
    public static Image getCachedTeXFormulaImage(string f, int style, int type, int size, int inset, Color fgcolor)  {
        return getCachedTeXFormulaImage(new CachedTeXFormula(f, style, type, size, inset, fgcolor));
    }

    public static Image getCachedTeXFormulaImage(string f, int style, int size, int inset)  {
        return getCachedTeXFormulaImage(f, style, 0, size, inset, null);
    }

    /**
     * Get a cached formula
     * @param o an object to identify the image in the cache
     * @return the cached image
     */
    public static Image getCachedTeXFormulaImage(object o)  {
        if (o == null || !(o is CachedTeXFormula)) {
            return null;
        }
        CachedTeXFormula cached = (CachedTeXFormula) o;
        WeakReference<CachedImage> img = cache.Get(cached);
        if (img == null || img.Get() == null) {
            img = makeImage(cached);
        }

        return img.Get().image;
    }

    private static WeakReference<CachedImage> makeImage(CachedTeXFormula cached)  {
        TeXFormula formula = new TeXFormula(cached.f);
        TeXIcon icon = formula.CreateTeXIcon(cached.style, cached.size, cached.type, cached.fgcolor);
        icon.setInsets(new Insets(cached.inset, cached.inset, cached.inset, cached.inset));
        Bitmap image = new Bitmap(icon.getIconWidth(), icon.getIconHeight(), Bitmap.TYPE_INT_ARGB);
        Graphics g2 = image.createGraphics();
        icon.paintIcon(null, g2, 0, 0);
        g2.dispose();
        cached.setDimensions(icon.getIconWidth(), icon.getIconHeight(), icon.getIconDepth());
        WeakReference<CachedImage> img = new WeakReference<CachedImage>(new CachedImage(image, cached), queue);

        if (cache.Length >= max) {
            WeakReference<CachedImage> soft;
            while ((soft = queue.poll()) != null) {
                CachedImage ci = (CachedImage) soft.Get();
                if (ci != null) {
                    cache.remove(ci.cachedTf);
                }
            }
            Iterator<CachedTeXFormula> iter = cache.keySet().iterator();
            if (iter.hasNext()) {
                CachedTeXFormula c = iter.next();
                WeakReference<CachedImage> cachedImage = cache.Get(c);
                if (cachedImage != null) {
                    cachedImage.clear();
                }
                cache.remove(c);
            }
        }
        cache.TryAdd(cached, img);

        return img;
    }

    private class CachedImage {

        Image image;
        CachedTeXFormula cachedTf;

        CachedImage(Image image, CachedTeXFormula cachedTf) {
            this.image = image;
            this.cachedTf = cachedTf;
        }
    }
    public class CachedTeXFormula {

        public string f;
        public int style;
        public int type;
        public int size;
        public int inset;
        public int width = -1;
        public int height;
        public int depth;
        public Color fgcolor;

        public CachedTeXFormula(string f, int style, int type, int size, int inset, Color fgcolor) {
            this.f = f;
            this.style = style;
            this.type = type;
            this.size = size;
            this.inset = inset;
            this.fgcolor = fgcolor;
        }

        void setDimensions(int width, int height, int depth) {
            this.width = width;
            this.height = height;
            this.depth = depth;
        }

        /**
         * {@inheritDoc}
         */
        public bool equals(object o) {
            if (o != null && o is CachedTeXFormula) {
                CachedTeXFormula c = (CachedTeXFormula) o;
                bool b = (c.f==(f) && c.style == style && c.type == type && c.size == size && c.inset == inset && c.fgcolor==(fgcolor));
                if (b) {
                    if (c.width == -1) {
                        c.width = width;
                        c.height = height;
                        c.depth = depth;
                    } else if (width == -1) {
                        width = c.width;
                        height = c.height;
                        depth = c.depth;
                    }
                }

                return b;
            }

            return false;
        }

        /**
         * {@inheritDoc}
         */
        public int hashCode() {
            return f.hashCode();
        }
    }
}
