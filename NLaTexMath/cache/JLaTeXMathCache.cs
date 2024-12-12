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
public class JLaTeXMathCache
{

    private static readonly AffineTransform identity = new AffineTransform();
    private static ConcurrentDictionary<CachedTeXFormula, WeakReference<CachedImage>> cache = new();
    private static int max = int.MaxValue;
    private static readonly Queue<CachedImage> queue = [];

    private JLaTeXMathCache() { }

    /**
     * Set max size. Take care the cache will be reinitialized
     * @param max the max size
     */
    public static void SetMaxCachedObjects(int max)
    {
        JLaTeXMathCache.max = Math.Max(max, 1);
        cache.Clear();
        cache = [];
    }

    /**
     * @param f a formula
     * @param style a style like TeXConstants.STYLE_DISPLAY
     * @param size the size of font
     * @param inset the inset to Add on the top, bottom, left and right
     * @return an array of length 3 containing width, height and depth
     */
    public static int[] GetCachedTeXFormulaDimensions(string f, int style, int type, int size, int inset, Color? fgcolor)
    {
        return GetCachedTeXFormulaDimensions(new CachedTeXFormula(f, style, type, size, inset, fgcolor), style);
    }

    public static int[] GetCachedTeXFormulaDimensions(string f, int style, int size, int inset)
    {
        return GetCachedTeXFormulaDimensions(f, style, 0, size, inset, null);
    }

    /**
     * @param o an object to identify the image in the cache
     * @return an array of length 3 containing width, height and depth
     */
    public static int[] GetCachedTeXFormulaDimensions(object o, int style)
    {
        if (o == null || o is not CachedTeXFormula)
        {
            return [0, 0, 0];
        }
        CachedTeXFormula cached = (CachedTeXFormula)o;
        if (!cache.TryGetValue(cached, out var img) || !img.TryGetTarget(out var target))
        {
            MakeImage(cached);
        }

        return [cached.width, cached.height, cached.depth];
    }

    /**
     * Get a cached formula
     * @param f a formula
     * @param style a style like TeXConstants.STYLE_DISPLAY
     * @param size the size of font
     * @param inset the inset to Add on the top, bottom, left and right
     * @return the key in the map
     */
    public static object GetCachedTeXFormula(string f, int style, int type, int size, int inset, Color? fgcolor)
    {
        CachedTeXFormula cached = new CachedTeXFormula(f, style, type, size, inset, fgcolor);
        if (!cache.TryGetValue(cached, out var img) || !img.TryGetTarget(out var target))
        {
            MakeImage(cached);
        }

        return cached;
    }

    public static object GetCachedTeXFormula(string f, int style, int size, int inset)
    {
        return GetCachedTeXFormula(f, style, 0, size, inset, null);
    }

    /**
     * Clear the cache
     */
    public static void ClearCache()
    {
        cache.Clear();
    }

    /**
     * Remove a formula from the cache
     * @param f a formula
     * @param style a style like TeXConstants.STYLE_DISPLAY
     * @param size the size of font
     * @param inset the inset to Add on the top, bottom, left and right
     */
    public static void RemoveCachedTeXFormula(string f, int style, int type, int size, int inset, Color? fgcolor)
    {
        cache.TryRemove(new CachedTeXFormula(f, style, type, size, inset, fgcolor),out var _);
    }

    public static void RemoveCachedTeXFormula(string f, int style, int size, int inset)
    {
        RemoveCachedTeXFormula(f, style, 0, size, inset, null);
    }

    /**
     * Remove a formula from the cache. Take care, remove the object o, invalidate it !
     * @param o an object to identify the image in the cache
     */
    public static void RemoveCachedTeXFormula(object o, int style)
    {
        if (o != null && o is CachedTeXFormula)
        {
            cache.TryRemove((CachedTeXFormula)o,out var _);
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
    public static object PaintCachedTeXFormula(string f, int style, int type, int size, int inset, Color? fgcolor, Graphics g)
    {
        return PaintCachedTeXFormula(new CachedTeXFormula(f, style, type, size, inset, fgcolor), g);
    }

    public static object PaintCachedTeXFormula(string f, int style, int size, int inset, Graphics g)
    {
        return PaintCachedTeXFormula(f, style, 0, size, inset, null, g);
    }

    /**
     * Paint a cached formula
     * @param o an object to identify the image in the cache
     * @param g the graphics where to paint the image
     * @return the key in the map
     */
    public static object PaintCachedTeXFormula(object o, Graphics g)
    {
        if (o == null || o is not CachedTeXFormula)
        {
            return null;
        }
        CachedTeXFormula cached = (CachedTeXFormula)o;
        if (!cache.TryGetValue(cached, out var img) || !img.TryGetTarget(out var target))
        {
            img = MakeImage(cached);
        }
        //TODO:
        //g.DrawImage(target.image, identity, null);

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
    public static Image GetCachedTeXFormulaImage(string f, int style, int type, int size, int inset, Color? fgcolor)
    {
        return GetCachedTeXFormulaImage(new CachedTeXFormula(f, style, type, size, inset, fgcolor));
    }

    public static Image GetCachedTeXFormulaImage(string f, int style, int size, int inset)
    {
        return GetCachedTeXFormulaImage(f, style, 0, size, inset, null);
    }

    /**
     * Get a cached formula
     * @param o an object to identify the image in the cache
     * @return the cached image
     */
    public static Image? GetCachedTeXFormulaImage(object o)
    {
        if (o == null || o is not CachedTeXFormula)
        {
            return null;
        }
        CachedTeXFormula cached = (CachedTeXFormula)o;
        if (!cache.TryGetValue(cached, out var img) || !img.TryGetTarget(out var target))
        {
            img = MakeImage(cached);
            img.TryGetTarget(out target);
        }

        return target?.image;
    }

    private static WeakReference<CachedImage> MakeImage(CachedTeXFormula cached)
    {
        //TeXFormula formula = new TeXFormula(cached.f);
        //TeXIcon icon = formula.CreateTeXIcon(cached.style, cached.size, cached.type, cached.fgcolor);
        //icon.Insets = new Insets(cached.inset, cached.inset, cached.inset, cached.inset);
        //Bitmap image = new Bitmap(icon.GetIconWidth(), icon.GetIconHeight(), Bitmap.TYPE_INT_ARGB);
        //Graphics g2 = image.createGraphics();
        //icon.PaintIcon(null, g2, 0, 0);
        //g2.dispose();
        //cached.SetDimensions(icon.GetIconWidth(), icon.GetIconHeight(), icon.GetIconDepth());
        //WeakReference<CachedImage> img = new WeakReference<CachedImage>(new CachedImage(image, cached), queue);

        //if (cache.Count >= max)
        //{
        //WeakReference<CachedImage> soft;
        //while ((soft = queue.poll()) != null)
        //{
        //    CachedImage ci = (CachedImage)soft.Get();
        //    if (ci != null)
        //    {
        //        cache.Remove(ci.cachedTf);
        //    }
        //}
        //Iterator<CachedTeXFormula> iter = cache.keySet().iterator();
        //if (iter.hasNext())
        //{
        //    CachedTeXFormula c = iter.next();
        //    WeakReference<CachedImage> cachedImage = cache.Get(c);
        //    if (cachedImage != null)
        //    {
        //        cachedImage.Clear();
        //    }
        //    cache.Remove(c);
        //}
        //}
        //cache.TryAdd(cached, img);
        //return img;
        return null;
    }

    public class CachedImage(Image image, JLaTeXMathCache.CachedTeXFormula cachedTf)
    {

        public readonly Image image = image;
        public readonly CachedTeXFormula cachedTf = cachedTf;
    }
    public class CachedTeXFormula(string f, int style, int type, int size, int inset, Color? fgcolor)
    {
        public string f = f;
        public int style = style;
        public int type = type;
        public int size = size;
        public int inset = inset;
        public int width = -1;
        public int height;
        public int depth;
        public Color? fgcolor = fgcolor;

        public void SetDimensions(int width, int height, int depth)
        {
            this.width = width;
            this.height = height;
            this.depth = depth;
        }

        /**
         * {@inheritDoc}
         */
        public override bool Equals(object? o)
        {
            if (o != null && o is CachedTeXFormula formula)
            {
                CachedTeXFormula c = formula;
                bool b = (c.f == (f) && c.style == style && c.type == type && c.size == size && c.inset == inset && c.fgcolor == (fgcolor));
                if (b)
                {
                    if (c.width == -1)
                    {
                        c.width = width;
                        c.height = height;
                        c.depth = depth;
                    }
                    else if (width == -1)
                    {
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
        public override int GetHashCode() => f.GetHashCode();
    }
}
