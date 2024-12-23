/* TeXFormulaParser.cs
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

using System.Drawing;
using System.Xml.Linq;

/**
 * Parses a "TeXFormula"-element representing a predefined TeXFormula's from an XML-file.
 */
public class TeXFormulaParser
{

    public interface ActionParser
    { // NOPMD
        public void Parse(XElement el);
    }

    private interface ArgumentValueParser
    { // NOPMD
        public object ParseValue(string value, string type);
    }

    public class MethodInvocationParser(TeXFormulaParser parser) : ActionParser
    {
        private readonly TeXFormulaParser parser = parser;

        public void Parse(XElement el)
        {
            // get required string attributes
            var methodName = GetAttrValueAndCheckIfNotNull("name", el);
            var objectName = GetAttrValueAndCheckIfNotNull(ARG_OBJ_ATTR, el);
            // check if temporary TeXFormula exists
            if (!parser.tempFormulas.TryGetValue(objectName, out var o))
            {// doesn't exist
                throw new XMLResourceParseException(
                    PredefinedTeXFormulaParser.RESOURCE_NAME, "Argument",
                    ARG_OBJ_ATTR,
                    "has an unknown temporary TeXFormula name as value : '"
                    + objectName + "'!");
            }
            else
            {
                // parse arguments
                List<XElement> args = el.Elements("Argument").ToList();
                // get argument classes and values
                Type[] argClasses = GetArgumentClasses(args);
                object[] argValues = parser.GetArgumentValues(args);
                // invoke method
                try
                {
                    typeof(TeXFormula).GetMethod(methodName, argClasses)?.Invoke((TeXFormula)o, argValues);
                }
                catch (Exception e)
                {
                    throw new XMLResourceParseException(
                        "Error invoking the method '" + methodName
                        + "' on the temporary TeXFormula '" + objectName
                        + "' while constructing the predefined TeXFormula '"
                        + parser.formulaName + "'!\n" + e.ToString());
                }
            }
        }
    }

    public class CreateTeXFormulaParser(TeXFormulaParser parser) : ActionParser
    {
        private readonly TeXFormulaParser parser = parser;

        public void Parse(XElement el)
        {
            // get required string attribute
            var name = GetAttrValueAndCheckIfNotNull("name", el);
            // parse arguments
            List<XElement> args = el.Elements("Argument").ToList();
            // get argument classes and values
            Type[] argClasses = GetArgumentClasses(args);
            object[] argValues = parser.GetArgumentValues(args);
            // create TeXFormula object
            //string code = "TeXFormula.predefinedTeXFormulasAsString.Add(\"%s\", \"%s\");";
            //System._out.println(string.format(code, formulaName, argValues[0]));
            try
            {
                TeXFormula? f = typeof(TeXFormula).GetConstructor(argClasses)?.Invoke(argValues) as TeXFormula;
                // succesfully created, so Add to "temporary formula's"-hashtable
                parser.tempFormulas.Add(name, f);
            }
            catch (Exception e)
            {
                throw new XMLResourceParseException(
                    "Error creating the temporary TeXFormula '" + name
                    + "' while constructing the predefined TeXFormula '"
                    + parser.formulaName + "'!\n" + e.ToString());
            }
        }
    }

    public class CreateCommandParser(TeXFormulaParser parser, string formulaName) : ActionParser
    {
        private readonly TeXFormulaParser parser = parser;
        private string formulaName = formulaName;

        public void Parse(XElement el)
        {
            // get required string attribute
            var name = GetAttrValueAndCheckIfNotNull("name", el);
            // parse arguments 
            List<XElement> args = el.Elements("Argument").ToList();
            // get argument classes and values
            Type[] argClasses = GetArgumentClasses(args);
            object[] argValues = parser.GetArgumentValues(args);
            // create TeXFormula object
            try
            {
                var f = typeof(MacroInfo).GetConstructor(argClasses).Invoke(argValues) as MacroInfo;
                // succesfully created, so Add to "temporary formula's"-hashtable
                parser.tempCommands.Add(name, f);
            }
            catch (Exception e)
            {
                string err = "IllegalArgumentException:\n";
                err += "ClassLoader to load this class (TeXFormulaParser): " + this.GetType() + "\n";
                foreach (Type cl in argClasses)
                {
                    err += "Created class: " + cl + " loaded with the ClassLoader: " + cl + "\n";
                }
                foreach (object obj in argValues)
                {
                    err += "Created object: " + obj + "\n";
                }
                throw new XMLResourceParseException(
                    "Error creating the temporary command '" + name
                    + "' while constructing the predefined command '"
                    + formulaName + "'!\n" + err);
            }
        }
    }

    public class FloatValueParser : ArgumentValueParser
    {

        public FloatValueParser()
        {
            // avoids creation of special accessor type
        }

        public object ParseValue(string value, string type)
        {
            CheckNullValue(value, type);
            try
            {
                return float.TryParse(value, out var v) ? v : 0;
            }
            catch (Exception e)
            {
                throw new XMLResourceParseException(
                    PredefinedTeXFormulaParser.RESOURCE_NAME, "Argument",
                    ARG_VAL_ATTR, $"has an invalid '{type}'-value : '{value}'!", e);
            }
        }
    }

    public class CharValueParser(TeXFormulaParser parser) : ArgumentValueParser
    {
        private readonly TeXFormulaParser parser = parser;

        public object ParseValue(string value, string type)
        {
            CheckNullValue(value, type);
            if (value.Length == 1)
            {
                return value[0];
            }
            else
            {
                throw new XMLResourceParseException(
                    PredefinedTeXFormulaParser.RESOURCE_NAME, "Argument",
                    ARG_VAL_ATTR,
                    "must have a value that consists of exactly 1 character!");
            }
        }
    }

    public class BooleanValueParser : ArgumentValueParser
    {

        public BooleanValueParser()
        {
            // avoids creation of special accessor type
        }

        public object ParseValue(string value, string type)
        {
            CheckNullValue(value, type);
            return value switch
            {
                "true" => true,
                "false" => false,
                _ => throw new XMLResourceParseException(
                    PredefinedTeXFormulaParser.RESOURCE_NAME, "Argument",
                    ARG_VAL_ATTR, $"has an invalid '{type}'-value : '{value}'!")
            };
        }
    }

    public class IntValueParser : ArgumentValueParser
    {

        public IntValueParser()
        {
            // avoids creation of special accessor type
        }

        public object ParseValue(string value, string type)
        {
            CheckNullValue(value, type);
            try
            {
                int val = int.TryParse(value, out var v) ? v : 0;
                return (val);
            }
            catch (Exception e)
            {
                throw new XMLResourceParseException(
                    PredefinedTeXFormulaParser.RESOURCE_NAME, "Argument",
                    ARG_VAL_ATTR, $"has an invalid '{type}'-value : '{value}'!", e);
            }
        }
    }

    public class ReturnParser(TeXFormulaParser parser) : ActionParser
    {
        protected TeXFormulaParser parser = parser;
        private MacroInfo result;

        private Dictionary<string, MacroInfo> tempCommands;
        private Dictionary<string, MacroInfo> tempFormulas;

        private int type;
        private string formulaName;

        public void Parse(XElement el)
        {
            // get required string attribute
            var name = GetAttrValueAndCheckIfNotNull("name", el);
            if (!(type == COMMAND ? tempCommands.TryGetValue(name, out MacroInfo? res)
                : tempFormulas.TryGetValue(name, out res)))
            {
                throw new XMLResourceParseException(
                    PredefinedTeXFormulaParser.RESOURCE_NAME, RETURN_EL, "name",
                    "Contains an unknown temporary TeXFormula variable name '"
                    + name + "' for the predefined TeXFormula '"
                    + formulaName + "'!");
            }
            else
            {
                result = res;
            }
        }
    }

    public class StringValueParser : ArgumentValueParser
    {

        public StringValueParser()
        {
            // avoids creation of special accessor type
        }

        public object ParseValue(string value, string type)
        {
            return value;
        }
    }

    public class TeXFormulaValueParser(TeXFormulaParser parser) : ArgumentValueParser
    {
        private readonly TeXFormulaParser parser = parser;

        public object ParseValue(string value, string type)
        {
            if (value == null)
            {// null pointer argument
                return null;
            }
            else
            {
                if (!parser.tempFormulas.TryGetValue(value, out var formula))
                {// unknown temporary TeXFormula!
                    throw new XMLResourceParseException(
                        PredefinedTeXFormulaParser.RESOURCE_NAME, "Argument",
                        ARG_VAL_ATTR,
                        "has an unknown temporary TeXFormula name as value : '"
                        + value + "'!");
                }
                else
                {
                    return formula;
                }
            }
        }
    }

    public class TeXConstantsValueParser : ArgumentValueParser
    {

        public TeXConstantsValueParser()
        {
            // avoids creation of special accessor type
        }

        public object ParseValue(string value, string type)
        {
            CheckNullValue(value, type);
            try
            {
                // get constant value (if present)
                int constant = (int)typeof(TeXConstants).GetField(value).GetValue(null);
                // return constant integer value
                return (constant);
            }
            catch (Exception e)
            {
                throw new XMLResourceParseException(
                    PredefinedTeXFormulaParser.RESOURCE_NAME, "Argument",
                    ARG_VAL_ATTR, $"has an unknown constant name as value : '{value}'!", e);
            }
        }
    }

    public class ColorConstantValueParser(TeXFormulaParser parser) : ArgumentValueParser
    {
        private readonly TeXFormulaParser parser = parser;

        public object ParseValue(string value, string type)
        {
            CheckNullValue(value, type);
            try
            {
                // return Color constant (if present)
                return typeof(Color).GetField(value)?.GetValue(null);
            }
            catch (Exception e)
            {
                throw new XMLResourceParseException(
                    PredefinedTeXFormulaParser.RESOURCE_NAME, "Argument",
                    ARG_VAL_ATTR,
                    $"has an unknown color constant name as value : '{value}'!", e);
            }
        }
    }

    private const string ARG_VAL_ATTR = "value", RETURN_EL = "Return", ARG_OBJ_ATTR = "formula";

    private static readonly Dictionary<string, Type> classMappings = [];

    private readonly Dictionary<string, ArgumentValueParser> argValueParsers = [];
    private readonly Dictionary<string, ActionParser> actionParsers = [];
    private readonly Dictionary<string, TeXFormula> tempFormulas = [];
    private readonly Dictionary<string, MacroInfo> tempCommands = [];

    private readonly object result = new();

    private readonly string formulaName;

    private readonly XElement formula;

    private const int COMMAND = 0, TEXFORMULA = 1;
    private readonly int type;

    public int Type => type;

    static TeXFormulaParser()
    {
        // string-to-class mappings
        classMappings.Add("TeXConstants", typeof(int)); // all integer constants
        classMappings.Add("TeXFormula", typeof(TeXFormula));
        classMappings.Add("string", typeof(string));
        classMappings.Add("float", typeof(float));
        classMappings.Add("int", typeof(int));
        classMappings.Add("boolean", typeof(bool));
        classMappings.Add("char", typeof(char));
        classMappings.Add("ColorConstant", typeof(Color));
    }

    public TeXFormulaParser(string name, XElement formula, string type)
    {
        formulaName = name;
        this.formula = formula;
        this.type = "Command" == (type) ? COMMAND : TEXFORMULA;

        // action parsers
        if (this.type == COMMAND)
            actionParsers.Add("CreateCommand", new CreateCommandParser(this, formulaName));
        else
            actionParsers.Add("CreateTeXFormula", new CreateTeXFormulaParser(this));

        actionParsers.Add("MethodInvocation", new MethodInvocationParser(this));
        actionParsers.Add(RETURN_EL, new ReturnParser(this));

        // argument value parsers
        argValueParsers.Add("TeXConstants", new TeXConstantsValueParser());
        argValueParsers.Add("TeXFormula", new TeXFormulaValueParser(this));
        argValueParsers.Add("string", new StringValueParser());
        argValueParsers.Add("float", new FloatValueParser());
        argValueParsers.Add("int", new IntValueParser());
        argValueParsers.Add("boolean", new BooleanValueParser());
        argValueParsers.Add("char", new CharValueParser(this));
        argValueParsers.Add("ColorConstant", new ColorConstantValueParser(this));
    }

    public object Parse()
    {
        // parse and execute actions
        var list = formula.Nodes().ToList();
        for (int i = 0; i < list.Count; i++)
        {
            var node = list[i];
            if (node.NodeType != System.Xml.XmlNodeType.Text)
            {
                var el = (XElement)node;
                var p = actionParsers[el.Name.LocalName];
                p?.Parse(el);
            }
        }
        return result;
    }

    private object[] GetArgumentValues(List<XElement> args)
    {
        var res = new object[args.Count];
        int i = 0;
        for (int j = 0; j < args.Count; j++)
        {
            var arg = (XElement)args[(j)];
            // get required string attribute
            var type = GetAttrValueAndCheckIfNotNull("type", arg);
            // get value, not present means a nullpointer
            var value = arg.Attribute(ARG_VAL_ATTR)?.Value ?? "";
            // parse value, hashtable will certainly contain a parser for the class type,
            // because the class types have been checked before!
            res[i] = argValueParsers[(type)].ParseValue(value, type);
            i++;
        }
        return res;
    }

    private static Type[] GetArgumentClasses(List<XElement> args)
    {
        var res = new Type[args.Count];
        int i = 0;
        for (int j = 0; j < args.Count; j++)
        {
            var arg = args[(j)];
            // get required string attribute
            var type = GetAttrValueAndCheckIfNotNull("type", arg);
            // find class mapping
            var cl = classMappings[(type)];
            if (cl == null)
            {// no class mapping found
                throw new XMLResourceParseException(
                    PredefinedTeXFormulaParser.RESOURCE_NAME, "Argument", "type",
                    "has an invalid class name value!");
            }
            else
            {
                res[i] = cl;
            }
            i++;
        }
        return res;
    }

    private static void CheckNullValue(string value, string type)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new XMLResourceParseException(
                PredefinedTeXFormulaParser.RESOURCE_NAME, "Argument",
                ARG_VAL_ATTR, $"is required for an argument of type '{type}'!");
        }
    }

    private static string GetAttrValueAndCheckIfNotNull(string attrName, XElement element)
    {
        var attrValue = element.Attribute(attrName)?.Value ?? "";
        return attrValue == ""
            ? throw new XMLResourceParseException(
                PredefinedTeXFormulaParser.RESOURCE_NAME, element.Name.LocalName,
                attrName, null)
            : attrValue
            ;
    }
}
