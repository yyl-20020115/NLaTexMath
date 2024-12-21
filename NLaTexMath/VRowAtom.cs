/* VRowAtom.cs
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

/**
 * An atom representing a vertical row of other atoms.
 */
public class VRowAtom : Atom
{
    // atoms to be displayed horizontally next to eachother
    protected List<Atom> elements = [];
    private SpaceAtom raise = new(TeXConstants.UNIT_EX, 0, 0, 0);
    protected bool addInterline = false;
    protected bool vtop = false;
    protected int halign = TeXConstants.ALIGN_NONE;

    public VRowAtom() { }

    public VRowAtom(Atom el)
    {
        if (el != null)
        {
            if (el is VRowAtom atom)
                // no need to make an mrow the only element of an mrow
                elements.AddRange(atom.elements);
            else
                elements.Add(el);
        }
    }

    public bool AddInterline { get => this.addInterline; set => this.addInterline = value; }

    public int Halign { get => halign; set => this.halign = value; }

    public bool Vtop { get => vtop; set => this.vtop = value; }

    public void SetRaise(int unit, float r)
    {
        raise = new SpaceAtom(unit, r, 0, 0);
    }

    public Atom? GetLastAtom()
    {
        var a = elements.LastOrDefault();
        elements.RemoveAt(elements.Count - 1);
        return a;
    }

    public void Add(Atom el)
    {
        if (el != null)
            elements.Insert(0, el);
    }

    public void Append(Atom el)
    {
        if (el != null)
            elements.Add(el);
    }

    public override Box CreateBox(TeXEnvironment env)
    {
        var vb = new VerticalBox();
        if (halign != TeXConstants.ALIGN_NONE)
        {
            float maxWidth = -float.PositiveInfinity;
            List<Box> boxes = new();
            foreach (var it in elements)
            {
                Box b = it.CreateBox(env);
                boxes.Add(b);
                if (maxWidth < b.Width)
                {
                    maxWidth = b.Width;
                }
            }
            Box interline = new StrutBox(0, env.Interline, 0, 0);

            // convert atoms to boxes and Add to the horizontal box
            for (int i = 0; i < boxes.Count; i++)
            {
                Box b = boxes[i];
                vb.Add(new HorizontalBox(b, maxWidth, halign));
                if (addInterline && i < boxes.Count - 1)
                {
                    vb.Add(interline);
                }
            }
        }
        else
        {
            Box interline = new StrutBox(0, env.Interline, 0, 0);

            // convert atoms to boxes and Add to the horizontal box
            for (int i = 0; i < elements.Count; i++)
            {
                var it = elements[i];
                vb.Add(it.CreateBox(env));
                if (addInterline && i < elements.Count - 1)
                {
                    vb.Add(interline);
                }
            }
        }

        vb.Shift = -raise.CreateBox(env).Width;
        if (vtop)
        {
            float t = vb.Size == 0 ? 0 : vb.Children.First().Height;
            vb.Height = t;
            vb.Depth = vb.Depth + vb.Height - t;
        }
        else
        {
            float t = vb.Size == 0 ? 0 : vb.Children.Last().Depth;
            vb.Height = vb.Depth + vb.Height - t;
            vb.Depth = t;
        }

        return vb;
    }
}
