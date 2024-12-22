/* GlueSettingsParser.cs
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

namespace NLaTexMath;

/**
 * Parses the glue settings (different types and rules) from an XML-file.
 */
public class GlueSettingsParser
{

    private const string RESOURCE_NAME = "GlueSettings.xml";

    private readonly Dictionary<string, int> typeMappings = [];
    private readonly Dictionary<string, int> glueTypeMappings = [];
    private Glue[] glueTypes;

    private readonly Dictionary<string, int> styleMappings = [];

    private readonly XElement root;

    public GlueSettingsParser()
    {
        try
        {
            SetTypeMappings();
            SetStyleMappings();
            root = XDocument.Load(typeof(GlueSettingsParser)?.GetResourceAsStream(RESOURCE_NAME))?.Root;
            ParseGlueTypes();
        }
        catch (Exception e)
        { // JDOMException or IOException
            throw new XMLResourceParseException(RESOURCE_NAME, e);
        }
    }

    private void SetStyleMappings()
    {
        styleMappings.Add("display", TeXConstants.STYLE_DISPLAY / 2);
        styleMappings.Add("text", TeXConstants.STYLE_TEXT / 2);
        styleMappings.Add("script", TeXConstants.STYLE_SCRIPT / 2);
        styleMappings.Add("script_script", TeXConstants.STYLE_SCRIPT_SCRIPT / 2); // autoboxing
    }

    private void ParseGlueTypes()
    {
        List<Glue> glueTypesList = [];
        var types = root.Element("GlueTypes");
        int defaultIndex = -1;
        int index = 0;
        if (types != null)
        { // element present
            var list = types.Elements("GlueType").ToList();
            for (int i = 0; i < list.Count; i++)
            {
                var type = list[i];
                // retrieve required attribute value, throw exception if not set
                var name = GetAttrValueAndCheckIfNotNull("name", type);
                var glue = CreateGlue(type, name);
                if (name.Equals("default", StringComparison.OrdinalIgnoreCase)) // default must have value
                    defaultIndex = index;
                glueTypesList.Add(glue);
                index++;
            }
        }
        if (defaultIndex < 0)
        {
            // create a default glue object if missing
            defaultIndex = index;
            glueTypesList.Add(new Glue(0, 0, 0, "default"));
        }

        glueTypes = [.. glueTypesList];

        // make sure default glue is at the front
        if (defaultIndex > 0)
        {
            (glueTypes[0], glueTypes[defaultIndex]) = (glueTypes[defaultIndex], glueTypes[0]);
        }

        // make reverse map
        for (int i = 0; i < glueTypes.Length; i++)
        {
            glueTypeMappings.Add(glueTypes[i].Name, i);
        }
    }

    private Glue CreateGlue(XElement type, string name)
    {
        string[] names = ["space", "stretch", "shrink"];
        float[] values = new float[names.Length];
        for (int i = 0; i < names.Length; i++)
        {
            double val = 0; // default value if attribute not present
            string attrVal = null;
            try
            {
                attrVal = type.Attribute(names[i])?.Value ?? "";
                if (attrVal != ("")) // attribute present
                    val = Double.TryParse(attrVal, out var u) ? u : 0;
            }
            catch (Exception e)
            {
                throw new XMLResourceParseException(RESOURCE_NAME, "GlueType",
                                                    names[i], $"has an invalid real value '{attrVal}'!");
            }
            values[i] = (float)val;
        }
        return new Glue(values[0], values[1], values[2], name);
    }

    private void SetTypeMappings()
    {
        typeMappings.Add("ord", TeXConstants.TYPE_ORDINARY);
        typeMappings.Add("op", TeXConstants.TYPE_BIG_OPERATOR);
        typeMappings.Add("bin", TeXConstants.TYPE_BINARY_OPERATOR);
        typeMappings.Add("rel", TeXConstants.TYPE_RELATION);
        typeMappings.Add("open", TeXConstants.TYPE_OPENING);
        typeMappings.Add("close", TeXConstants.TYPE_CLOSING);
        typeMappings.Add("punct", TeXConstants.TYPE_PUNCTUATION);
        typeMappings.Add("inner", TeXConstants.TYPE_INNER); // autoboxing
    }

    public Glue[] GlueTypes => glueTypes;

    public int[,,] CreateGlueTable()
    {
        int size = typeMappings.Count;
        int[,,] table = new int[size, size, styleMappings.Count];
        var glueTable = root.Element("GlueTable");
        if (glueTable != null)
        { // element present
            // iterate all the "Glue"-elements
            var list = glueTable.Elements("Glue").ToList();
            for (int i = 0; i < list.Count; i++)
            {
                var glue = list[i];
                // retrieve required attribute values and throw exception if they're not set
                string left = GetAttrValueAndCheckIfNotNull("lefttype", glue);
                string right = GetAttrValueAndCheckIfNotNull("righttype", glue);
                string type = GetAttrValueAndCheckIfNotNull("gluetype", glue);
                // iterate all the "Style"-elements
                var listG = glue.Elements("Style").ToList();
                for (int j = 0; j < listG.Count; j++)
                {
                    var style = listG[(j)];
                    string styleName = GetAttrValueAndCheckIfNotNull("name", style);
                    // retrieve mappings
                    object l = typeMappings[(left)];
                    object r = typeMappings[(right)];
                    object st = styleMappings[(styleName)];
                    object val = glueTypeMappings[(type)];
                    // throw exception if unknown value set 
                    CheckMapping(l, "Glue", "lefttype", left);
                    CheckMapping(r, "Glue", "righttype", right);
                    CheckMapping(val, "Glue", "gluetype", type);
                    CheckMapping(st, "Style", "name", styleName);
                    // put value in table
                    table[((int)l),((int)r),((int)st)] = ((int)val);
                }
            }
        }
        return table;
    }

    private static void CheckMapping(object val, string elementName,
                                     string attrName, string attrValue)
    {
        if (val == null)
            throw new XMLResourceParseException(RESOURCE_NAME, elementName,
                                                attrName, "has an unknown value '" + attrValue + "'!");
    }

    private static string GetAttrValueAndCheckIfNotNull(string attrName, XElement element)
    {
        var attrValue = element.Attribute(attrName)?.Value ?? "";
        return attrValue == ("") ? throw new XMLResourceParseException(RESOURCE_NAME, element.Name.LocalName, attrName, null) : attrValue;
    }
}
