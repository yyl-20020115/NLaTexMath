/* TeXSymbolParser.cs
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

using NLaTexMath.Internal.Util;
using System.Xml.Linq;
using System.Xml.XPath;

namespace NLaTexMath;

/**
 * Parses TeX symbol definitions from an XML-file.
 */
public class TeXSymbolParser
{

    public static readonly string RESOURCE_NAME = "TeXSymbols.xml",
                               DELIMITER_ATTR = "del", TYPE_ATTR = "type";

    private static readonly Dictionary<string, int> typeMappings = [];

    private readonly XElement? root;

    public TeXSymbolParser() : this(typeof(TeXSymbolParser).GetResourceAsStream(RESOURCE_NAME), RESOURCE_NAME)
    {
    }

    public TeXSymbolParser(Stream file, string name)
    {
        try
        {
            root = XDocument.Load(file).Root;
            // set possible valid symbol type mappings
            SetTypeMappings();
        }
        catch (Exception e)
        { // JDOMException or IOException
            throw new XMLResourceParseException(name, e);
        }
    }

    public Dictionary<string, SymbolAtom> ReadSymbols()
    {
        Dictionary<string, SymbolAtom> res = [];
        // iterate all "symbol"-elements
        var list = root?.XPathSelectElements("Symbol").ToList();
        for (int i = 0; i < list.Count; i++)
        {
            XElement symbol = (XElement)list[i];
            // retrieve and check required attributes
            string name = GetAttrValueAndCheckIfNotNull("name", symbol), type = GetAttrValueAndCheckIfNotNull(
                              TYPE_ATTR, symbol);
            // retrieve optional attribute
            string del = symbol.Attribute(DELIMITER_ATTR)?.Value ?? "";
            bool isDelimiter = (del != null && del == "true");
            // check if type is known
            if (typeMappings.TryGetValue(type,out var typeVal)) // unknown type
                throw new XMLResourceParseException(RESOURCE_NAME, "Symbol",
                                                    "type", "has an unknown value '" + type + "'!");
            // Add symbol to the hash table
            res.Add(name, new SymbolAtom(name, ((int)typeVal), isDelimiter));
        }
        return res;
    }

    private static void SetTypeMappings()
    {
        typeMappings.Add("ord", TeXConstants.TYPE_ORDINARY);
        typeMappings.Add("op", TeXConstants.TYPE_BIG_OPERATOR);
        typeMappings.Add("bin", TeXConstants.TYPE_BINARY_OPERATOR);
        typeMappings.Add("rel", TeXConstants.TYPE_RELATION);
        typeMappings.Add("open", TeXConstants.TYPE_OPENING);
        typeMappings.Add("close", TeXConstants.TYPE_CLOSING);
        typeMappings.Add("punct", TeXConstants.TYPE_PUNCTUATION);
        typeMappings.Add("acc", TeXConstants.TYPE_ACCENT);
    }

    private static string GetAttrValueAndCheckIfNotNull(string attrName, XElement element)
    {
        var attrValue = element.Attribute(attrName)?.Value ?? "";
        return attrValue == "" ? throw new XMLResourceParseException(RESOURCE_NAME, element.Name.LocalName, attrName, null) : attrValue;
    }
}
