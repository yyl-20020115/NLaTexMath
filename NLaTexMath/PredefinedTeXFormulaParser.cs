/* PredefinedTeXFormulaParser.java
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

namespace NLaTexMath;

using System.Xml.Linq;

/**
 * Parses and creates predefined TeXFormula objects form an XML-file.
 */
public class PredefinedTeXFormulaParser
{

    public static readonly string RESOURCE_NAME = "PredefinedTeXFormulas.xml";

    private XElement root;
    private string type;

    public PredefinedTeXFormulaParser(Stream file, string type)
    {
        try
        {
            this.type = type;
            //DocumentBuilderFactory factory = DocumentBuilderFactory.newInstance();
            //factory.setIgnoringElementContentWhitespace(true);
            //factory.setIgnoringComments(true);
            root = factory.newDocumentBuilder().parse(file).getDocumentElement();
        }
        catch (Exception e)
        { // JDOMException or IOException
            throw new XMLResourceParseException("", e);
        }
    }

    public PredefinedTeXFormulaParser(string PredefFile, string type) : this(PredefinedTeXFormulaParser.getResourceAsStream(PredefFile), type)
    {
    }

    public void Parse(Dictionary<string, string> predefinedTeXFormulas)
    {
        // get required string attribute
        string enabledAll = GetAttrValueAndCheckIfNotNull("enabled", root);
        if ("true" == (enabledAll))
        { // parse formula's
            // iterate all "Font"-elements
            List<XNode> list = root.getElementsByTagName(this.type);
            for (int i = 0; i < list.Count; i++)
            {
                XElement formula = (XElement)list[i];
                // get required string attribute
                string enabled = GetAttrValueAndCheckIfNotNull("enabled", formula);
                if ("true" == (enabled))
                { // parse this formula
                    // get required string attribute
                    string name = GetAttrValueAndCheckIfNotNull("name", formula);

                    // parse and build the formula and Add it to the table
                    if ("TeXFormula" == (this.type))
                        predefinedTeXFormulas.Add(name, (TeXFormula)new TeXFormulaParser(name, formula, this.type).parse());
                    else
                        predefinedTeXFormulas.Add(name, (MacroInfo)new TeXFormulaParser(name, formula, this.type).parse());
                }
            }
        }
    }

    private static string GetAttrValueAndCheckIfNotNull(string attrName,
            XElement element)
    {
        var attrValue = element.Attribute(attrName)?.Value ?? "";
        return attrValue == ""
            ? throw new XMLResourceParseException(RESOURCE_NAME, element.Name.LocalName, attrName, null)
            : attrValue
            ;
    }
}
