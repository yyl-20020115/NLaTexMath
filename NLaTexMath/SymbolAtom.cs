/* SymbolAtom.cs
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
 * A box representing a symbol (a non-alphanumeric character).
 */
public class SymbolAtom : CharSymbol
{

    // whether it's is a delimiter symbol
    private readonly bool delimiter;

    // symbol name
    private readonly string name;

    // Contains all defined symbols
    public static Dictionary<string, SymbolAtom> symbols;

    // Contains all the possible valid symbol types
    private static BitSet validSymbolTypes;

    private char unicode;

    static SymbolAtom()
    {
        symbols = new TeXSymbolParser().ReadSymbols();

        // set valid symbol types
        validSymbolTypes = new BitSet(16)
        {
            [(TeXConstants.TYPE_ORDINARY)] = true,
            [(TeXConstants.TYPE_BIG_OPERATOR)] = true,
            [(TeXConstants.TYPE_BINARY_OPERATOR)] = true,
            [(TeXConstants.TYPE_RELATION)] = true,
            [(TeXConstants.TYPE_OPENING)] = true,
            [(TeXConstants.TYPE_CLOSING)] = true,
            [(TeXConstants.TYPE_PUNCTUATION)] = true,
            [(TeXConstants.TYPE_ACCENT)] = true
        };
    }

    public SymbolAtom(SymbolAtom s, int type)
    {
        if (!validSymbolTypes[type])
            throw new InvalidSymbolTypeException(
                "The symbol type was not valid! "
                + "Use one of the symbol type constants from the class 'TeXConstants'.");
        name = s.name;
        this.Type = type;
        if (type == TeXConstants.TYPE_BIG_OPERATOR)
            this.TypeLimits = TeXConstants.SCRIPT_NORMAL;

        delimiter = s.delimiter;
    }

    /**
     * Constructs a new symbol. This used by "TeXSymbolParser" and the symbol
     * types are guaranteed to be valid.
     *
     * @param name symbol name
     * @param type symbol type constant
     * @param del whether the symbol is a delimiter
     */
    public SymbolAtom(string name, int type, bool del)
    {
        this.name = name;
        this.Type = type;
        if (type == TeXConstants.TYPE_BIG_OPERATOR)
            this.TypeLimits = TeXConstants.SCRIPT_NORMAL;

        delimiter = del;
    }

    public SymbolAtom SetUnicode(char c)
    {
        this.unicode = c;
        return this;
    }

    public char Unicode => unicode;

    public static void AddSymbolAtom(string file)
    {
        FileStream _in;
        try
        {
            _in = new FileStream(file,FileMode.Open);
        }
        catch (FileNotFoundException e)
        {
            throw new Exception(file, e);
        }
        AddSymbolAtom(_in, file);
    }

    public static void AddSymbolAtom(Stream _in, string name)
    {
        TeXSymbolParser tsp = new TeXSymbolParser(_in, name);
        foreach(var s in tsp.ReadSymbols())
        {
            symbols.Add(s.Key,s.Value);
        }
    }

    public static void AddSymbolAtom(SymbolAtom sym)
    {
        symbols.Add(sym.name, sym);
    }

    /**
     * Looks up the name in the table and returns the corresponding SymbolAtom representing
     * the symbol (if it's found).
     *
     * @param name the name of the symbol
     * @return a SymbolAtom representing the found symbol
     * @ if no symbol with the given name was found
     */
    public static SymbolAtom Get(string name)
        => !symbols.TryGetValue(name, out var v) ? throw new SymbolNotFoundException(name) : v;

    /**
     *
     * @return true if this symbol can act as a delimiter to embrace formulas
     */
    public bool IsDelimiter => delimiter;

    public string Name => name;

    public override Box CreateBox(TeXEnvironment env)
    {
        TeXFont tf = env.TeXFont;
        int style = env.Style;
        Char c = tf.GetChar(name, style);
        Box cb = new CharBox(c);
        if (env.SmallCap && unicode != 0 && char.IsLower(unicode))
        {
            try
            {
                cb = new ScaleBox(new CharBox(tf.GetChar(TeXFormula.symbolTextMappings[char.ToUpper(unicode)], style)), 0.8, 0.8);
            }
            catch (SymbolMappingNotFoundException e) { }
        }

        if (Type == TeXConstants.TYPE_BIG_OPERATOR)
        {
            if (style < TeXConstants.STYLE_TEXT && tf.HasNextLarger(c))
                c = tf.GetNextLarger(c, style);
            cb = new CharBox(c);
            cb.Shift = -(cb.Height + cb.Depth) / 2 - env.TeXFont.GetAxisHeight(env.Style);
            float delta = c.Italic;
            HorizontalBox hb = new HorizontalBox(cb);
            if (delta > TeXFormula.PREC)
                hb.Add(new StrutBox(delta, 0, 0, 0));
            return hb;
        }
        return cb;
    }

    public override CharFont GetCharFont(TeXFont tf) =>
        // style doesn't matter here
        tf.GetChar(name, TeXConstants.STYLE_DISPLAY).CharFont;
}
