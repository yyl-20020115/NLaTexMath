/* TeXFormulaParser.java
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

    private interface ActionParser
    { // NOPMD
        public void Parse(XElement el);
    }

    private interface ArgumentValueParser
    { // NOPMD
        public object ParseValue(string value, string type)
        ;
    }

    public class MethodInvocationParser : ActionParser
    {

        public MethodInvocationParser()
        {
            // avoids creation of special accessor type
        }

        public void Parse(XElement el)
        {
            // get required string attributes
            string methodName = getAttrValueAndCheckIfNotNull("name", el);
            string objectName = getAttrValueAndCheckIfNotNull(ARG_OBJ_ATTR, el);
            // check if temporary TeXFormula exists
            object o = tempFormulas.Get(objectName);
            if (o == null)
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
                List<XNode> args = el.getElementsByTagName("Argument");
                // get argument classes and values
                Type[] argClasses = getArgumentClasses(args);
                object[] argValues = getArgumentValues(args);
                // invoke method
                try
                {
                    typeof(TeXFormula).getMethod(methodName, argClasses).invoke((TeXFormula)o, argValues);
                }
                catch (Exception e)
                {
                    throw new XMLResourceParseException(
                        "Error invoking the method '" + methodName
                        + "' on the temporary TeXFormula '" + objectName
                        + "' while constructing the predefined TeXFormula '"
                        + formulaName + "'!\n" + e.ToString());
                }
            }
        }
    }

    private class CreateTeXFormulaParser : ActionParser
    {

        CreateTeXFormulaParser()
        {
            // avoids creation of special accessor type
        }

        public void Parse(XElement el)
        {
            // get required string attribute
            string name = getAttrValueAndCheckIfNotNull("name", el);
            // parse arguments
            List<XNode> args = el.getElementsByTagName("Argument");
            // get argument classes and values
            Type[] argClasses = getArgumentClasses(args);
            object[] argValues = getArgumentValues(args);
            // create TeXFormula object
            //string code = "TeXFormula.predefinedTeXFormulasAsString.Add(\"%s\", \"%s\");";
            //System._out.println(string.format(code, formulaName, argValues[0]));
            try
            {
                TeXFormula f = TeXFormula..getConstructor(argClasses).newInstance(argValues);
                // succesfully created, so Add to "temporary formula's"-hashtable
                tempFormulas.Add(name, f);
            }
            catch (Exception e)
            {
                throw new XMLResourceParseException(
                    "Error creating the temporary TeXFormula '" + name
                    + "' while constructing the predefined TeXFormula '"
                    + formulaName + "'!\n" + e.ToString());
            }
        }
    }

    private class CreateCommandParser : ActionParser
    {

        CreateCommandParser()
        {
            // avoids creation of special accessor type
        }

        public void Parse(XElement el)
        {
            // get required string attribute
            string name = getAttrValueAndCheckIfNotNull("name", el);
            // parse arguments
            List<XNode> args = el.getElementsByTagName("Argument");
            // get argument classes and values
            Type[] argClasses = getArgumentClasses(args);
            object[] argValues = getArgumentValues(args);
            // create TeXFormula object
            try
            {
                MacroInfo f = MacroInfo.getConstructor(argClasses).newInstance(argValues);
                // succesfully created, so Add to "temporary formula's"-hashtable
                tempCommands.Add(name, f);
            }
            catch (IllegalArgumentException e)
            {
                string err = "IllegalArgumentException:\n";
                err += "ClassLoader to load this class (TeXFormulaParser): " + this.GetType().getClassLoader() + "\n";
                foreach (Type cl in argClasses)
                {
                    err += "Created class: " + cl + " loaded with the ClassLoader: " + cl.getClassLoader() + "\n";
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
            catch (Exception e)
            {
                throw new XMLResourceParseException(
                    "Error creating the temporary command '" + name
                    + "' while constructing the predefined command '"
                    + formulaName + "'!\n" + e.ToString());
            }
        }
    }

    private class FloatValueParser : ArgumentValueParser
    {

        FloatValueParser()
        {
            // avoids creation of special accessor type
        }

        public object ParseValue(string value, string type)
        {
            checkNullValue(value, type);
            try
            {
                return new float(float.parseFloat(value));
            }
            catch (Exception e)
            {
                throw new XMLResourceParseException(
                    PredefinedTeXFormulaParser.RESOURCE_NAME, "Argument",
                    ARG_VAL_ATTR, "has an invalid '" + type + "'-value : '"
                    + value + "'!", e);
            }
        }
    }

    private class CharValueParser : ArgumentValueParser
    {

        CharValueParser()
        {
            // avoids creation of special accessor type
        }

        public object ParseValue(string value, string type)
        {
            checkNullValue(value, type);
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

    private class BooleanValueParser : ArgumentValueParser
    {

        BooleanValueParser()
        {
            // avoids creation of special accessor type
        }

        public object ParseValue(string value, string type)
        {
            checkNullValue(value, type);
            if ("true" == (value))
            {
                return true;
            }
            else if ("false" == (value))
            {
                return false;
            }
            else
            {
                throw new XMLResourceParseException(
                    PredefinedTeXFormulaParser.RESOURCE_NAME, "Argument",
                    ARG_VAL_ATTR, "has an invalid '" + type + "'-value : '"
                    + value + "'!");
            }
        }
    }

    private class IntValueParser : ArgumentValueParser
    {

        IntValueParser()
        {
            // avoids creation of special accessor type
        }

        public object ParseValue(string value, string type)
        {
            checkNullValue(value, type);
            try
            {
                int val = int.TryParse(value, out var v) ? v : 0;
                return new float(val);
            }
            catch (Exception e)
            {
                throw new XMLResourceParseException(
                    PredefinedTeXFormulaParser.RESOURCE_NAME, "Argument",
                    ARG_VAL_ATTR, "has an invalid '" + type + "'-value : '"
                    + value + "'!", e);
            }
        }
    }

    private class ReturnParser : ActionParser
    {

        ReturnParser()
        {
            // avoids creation of special accessor type
        }

        public void Parse(XElement el)
        {
            // get required string attribute
            string name = getAttrValueAndCheckIfNotNull("name", el);
            object res = type == COMMAND ? tempCommands.Get(name) : tempFormulas.Get(name);
            if (res == null)
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

    private class StringValueParser : ArgumentValueParser
    {

        StringValueParser()
        {
            // avoids creation of special accessor type
        }

        public object ParseValue(string value, string type)
        {
            return value;
        }
    }

    private class TeXFormulaValueParser : ArgumentValueParser
    {

        TeXFormulaValueParser()
        {
            // avoids creation of special accessor type
        }

        public object ParseValue(string value, string type)
        {
            if (value == null)
            {// null pointer argument
                return null;
            }
            else
            {
                object formula = tempFormulas.Get(value);
                if (formula == null)
                {// unknown temporary TeXFormula!
                    throw new XMLResourceParseException(
                        PredefinedTeXFormulaParser.RESOURCE_NAME, "Argument",
                        ARG_VAL_ATTR,
                        "has an unknown temporary TeXFormula name as value : '"
                        + value + "'!");
                }
                else
                {
                    return (TeXFormula)formula;
                }
            }
        }
    }

    private class TeXConstantsValueParser : ArgumentValueParser
    {

        TeXConstantsValueParser()
        {
            // avoids creation of special accessor type
        }

        public object ParseValue(string value, string type)
        {
            checkNullValue(value, type);
            try
            {
                // get constant value (if present)
                int constant = TeXConstants..getDeclaredField(value).getInt(
                                   null);
                // return constant integer value
                return int.valueOf(constant);
            }
            catch (Exception e)
            {
                throw new XMLResourceParseException(
                    PredefinedTeXFormulaParser.RESOURCE_NAME, "Argument",
                    ARG_VAL_ATTR, "has an unknown constant name as value : '"
                    + value + "'!", e);
            }
        }
    }

    private class ColorConstantValueParser : ArgumentValueParser
    {

        ColorConstantValueParser()
        {
            // avoids creation of special accessor type
        }

        public object ParseValue(string value, string type)
        {
            checkNullValue(value, type);
            try
            {
                // return Color constant (if present)
                return Color.getDeclaredField(value).Get(null);
            }
            catch (Exception e)
            {
                throw new XMLResourceParseException(
                    PredefinedTeXFormulaParser.RESOURCE_NAME, "Argument",
                    ARG_VAL_ATTR,
                    "has an unknown color constant name as value : '" + value
                    + "'!", e);
            }
        }
    }

    private const string ARG_VAL_ATTR = "value", RETURN_EL = "Return",
                                ARG_OBJ_ATTR = "formula";

    private static Dictionary<string, Type> classMappings = new();

    private Dictionary<string, ArgumentValueParser> argValueParsers = new();
    private Dictionary<string, ActionParser> actionParsers = new();
    private Dictionary<string, TeXFormula> tempFormulas = new();
    private Dictionary<string, MacroInfo> tempCommands = new();

    private object result = new object();

    private string formulaName;

    private XElement formula;

    private const int COMMAND = 0, TEXFORMULA = 1;
    private int type;

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
        if ("Command" == (type))
            actionParsers.Add("CreateCommand", new CreateCommandParser());
        else
            actionParsers.Add("CreateTeXFormula", new CreateTeXFormulaParser());

        actionParsers.Add("MethodInvocation", new MethodInvocationParser());
        actionParsers.Add(RETURN_EL, new ReturnParser());

        // argument value parsers
        argValueParsers.Add("TeXConstants", new TeXConstantsValueParser());
        argValueParsers.Add("TeXFormula", new TeXFormulaValueParser());
        argValueParsers.Add("string", new StringValueParser());
        argValueParsers.Add("float", new FloatValueParser());
        argValueParsers.Add("int", new IntValueParser());
        argValueParsers.Add("boolean", new BooleanValueParser());
        argValueParsers.Add("char", new CharValueParser());
        argValueParsers.Add("ColorConstant", new ColorConstantValueParser());
    }

    public object parse()
    {
        // parse and execute actions
        List<XNode> list = formula.getChildNodes();
        for (int i = 0; i < list.Count; i++)
        {
            XNode node = list[i];
            if (node.NodeType != XNode.TEXT_NODE)
            {
                XElement el = (XElement)node;
                ActionParser p = actionParsers[(el.Name.LocalName)];
                if (p != null)
                {// ignore unknown elements
                    p.Parse(el);
                }
            }
        }
        return result;
    }

    private object[] getArgumentValues(List<XNode> args)
    {
        object[] res = new object[args.Count];
        int i = 0;
        for (int j = 0; j < args.Count; j++)
        {
            XElement arg = (XElement)args.item(j);
            // get required string attribute
            string type = getAttrValueAndCheckIfNotNull("type", arg);
            // get value, not present means a nullpointer
            string value = arg.Attribute(ARG_VAL_ATTR)?.Value ?? "";
            // parse value, hashtable will certainly contain a parser for the class type,
            // because the class types have been checked before!
            res[i] = argValueParsers.Get(type).parseValue(value, type);
            i++;
        }
        return res;
    }

    private static Type[] getArgumentClasses(List<XNode> args)
    {
        Type[] res = new Type[args.Count];
        int i = 0;
        for (int j = 0; j < args.Count; j++)
        {
            XElement arg = (XElement)args.item(j);
            // get required string attribute
            string type = getAttrValueAndCheckIfNotNull("type", arg);
            // find class mapping
            object cl = classMappings[(type)];
            if (cl == null)
            {// no class mapping found
                throw new XMLResourceParseException(
                    PredefinedTeXFormulaParser.RESOURCE_NAME, "Argument", "type",
                    "has an invalid class name value!");
            }
            else
            {
                res[i] = (Type)cl;
            }
            i++;
        }
        return res;
    }

    private static void checkNullValue(string value, string type)
    {
        if (value == "")
        {
            throw new XMLResourceParseException(
                PredefinedTeXFormulaParser.RESOURCE_NAME, "Argument",
                ARG_VAL_ATTR, "is required for an argument of type '" + type
                + "'!");
        }
    }

    private static string getAttrValueAndCheckIfNotNull(string attrName,
            XElement element)
    {
        string attrValue = element.Attribute(attrName)?.Value ?? "";
        if (attrValue == "")
        {
            throw new XMLResourceParseException(
                PredefinedTeXFormulaParser.RESOURCE_NAME, element.Name.LocalName,
                attrName, null);
        }
        return attrValue;
    }
}
