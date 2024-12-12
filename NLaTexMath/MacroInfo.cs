/* MacroInfo.java
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

using System.Reflection;

namespace NLaTexMath;

public class MacroInfo
{
    public static readonly Dictionary<string, MacroInfo> Commands = new(300);
    public static readonly Dictionary<string, object> Packages = [];

    public object pack;
    public MethodInfo macro;
    public int nbArgs;
    public bool hasOptions = false;
    public int posOpts;

    public MacroInfo(object pack, MethodInfo macro, int nbArgs)
    {
        this.pack = pack;
        this.macro = macro;
        this.nbArgs = nbArgs;
    }

    public MacroInfo(object pack, MethodInfo macro, int nbArgs, int posOpts) : this(pack, macro, nbArgs)
    {
        this.hasOptions = true;
        this.posOpts = posOpts;
    }

    public MacroInfo(int nbArgs, int posOpts) : this(null, (MethodInfo)null, nbArgs)
    {
        this.hasOptions = true;
        this.posOpts = posOpts;
    }

    public MacroInfo(int nbArgs) : this(null, (MethodInfo)null, nbArgs)
    {
    }

    public MacroInfo(string className, string methodName, float nbArgs)
    {
        int nba = (int)nbArgs;
        Type[] args = [typeof(TeXParser), typeof(string[])];

        try
        {
            object pack = Packages[className];
            if (pack == null)
            {
                Type cl = Type.forName(className);
                pack = cl.getConstructor(new Type[0]).newInstance(new object[0]);
                Packages.Add(className, pack);
            }
            this.pack = pack;
            this.macro = pack.GetType().getDeclaredMethod(methodName, args);
            this.nbArgs = nba;
        }
        catch (Exception e)
        {
            Console.Error.WriteLine("Cannot load namespace " + className + ":");
            Console.Error.WriteLine(e.ToString());
        }
    }

    public MacroInfo(string className, string methodName, float nbArgs, float posOpts)
    {
        int nba = (int)nbArgs;
        Type[] args = [typeof(TeXParser), typeof(string[])];

        try
        {
            object pack = Packages[className];
            if (pack == null)
            {
                Type cl = Type.forName(className);
                pack = cl.getConstructor(new Type[0]).newInstance(new object[0]);
                Packages.Add(className, pack);
            }
            this.pack = pack;
            this.macro = pack.GetType().getDeclaredMethod(methodName, args);
            this.nbArgs = nba;
            this.hasOptions = true;
            this.posOpts = (int)posOpts;
        }
        catch (Exception e)
        {
            Console.Error.WriteLine("Cannot load namespace " + className + ":");
            Console.Error.WriteLine(e.ToString());
        }
    }

    public object Invoke(TeXParser tp, string[] args)
    {
        object[] argsMethod = [tp, args];
        try
        {
            return macro.Invoke(pack, argsMethod);
        }
        catch (Exception e)
        {
            var th = e.InnerException;
            throw new ParseException($"Problem with command {args[0]} at position {tp.Line}:{tp.Col}\n{th?.Message??""}");
        }
    }
}
