/* GraphicsAtom.cs
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

using System.Drawing;

namespace NLaTexMath;


/**
 * An atom representing an atom containing a graphic.
 */
public class GraphicsAtom : Atom
{

    private Image image;
    private Image bimage;
    //TODO:
    //private Label c;
    private int w, h;

    private Atom _base;
    private bool first = true;
    private int interp = -1;

    public GraphicsAtom(string path, string option)
    {
        if (!File.Exists(path))
        {
        }
        else
        {
            image = Bitmap.FromFile(path);
        }

        if (image != null)
        {
            //TODO:
            //c = new Label();
            //MediaTracker tracker = new MediaTracker(c);
            //tracker.addImage(image, 0);
            //try
            //{
            //    tracker.waitForID(0);
            //}
            //catch (InterruptedException e)
            //{
            //    image = null;
            //}
        }
        Draw();
        BuildAtom(option);
    }

    protected void BuildAtom(string option)
    {
        _base = this;
        Dictionary<string, string> options = ParseOption.ParseMap(option);
        if (options.ContainsKey("width") || options.ContainsKey("height"))
        {
            _base = new ResizeAtom(_base, options[("width")], options[("height")], options.ContainsKey("keepaspectratio"));
        }
        if (options.ContainsKey("scale"))
        {
            double scl = Double.TryParse(options[("scale")], out var c) ? c : 0;
            _base = new ScaleAtom(_base, scl, scl);
        }
        if (options.ContainsKey("angle") || options.ContainsKey("origin"))
        {
            _base = new RotateAtom(_base, options[("angle")], options[("origin")]);
        }
        if (options.TryGetValue("interpolation", out string? meth))
        {
            if (meth.Equals("bilinear", StringComparison.OrdinalIgnoreCase))
            {
                interp = GraphicsBox.BILINEAR;
            }
            else if (meth.Equals("bicubic",StringComparison.OrdinalIgnoreCase))
            {
                interp = GraphicsBox.BICUBIC;
            }
            else if (meth.Equals("nearest_neighbor", StringComparison.OrdinalIgnoreCase))
            {
                interp = GraphicsBox.NEAREST_NEIGHBOR;
            }
        }
    }

    public void Draw()
    {
        if (image != null)
        {
            //TODO:
            //w = image.getWidth(c);
            //h = image.getHeight(c);
            //bimage = new Bitmap(w, h, Bitmap.TYPE_INT_ARGB);
            //Graphics g2d = bimage.createGraphics();
            //g2d.drawImage(image, 0, 0, null);
            //g2d.dispose();
        }
    }

    public override Box CreateBox(TeXEnvironment env)
    {
        if (image != null)
        {
            if (first)
            {
                first = false;
                return _base.CreateBox(env);
            }
            else
            {
                env.isColored = true;
                float width = w * SpaceAtom.GetFactor(TeXConstants.UNIT_PIXEL, env);
                float height = h * SpaceAtom.GetFactor(TeXConstants.UNIT_PIXEL, env);
                return new GraphicsBox(bimage, width, height, env.Size, interp);
            }
        }

        return new TeXFormula("\\text{ No such image file ! }").root.CreateBox(env);
    }
}
