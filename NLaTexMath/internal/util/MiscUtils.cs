using System.Text;


namespace NLaTexMath.Internal.Util;

public static class MiscUtils
{
    public static double ToDegrees(this double arc) => arc / Math.PI * 180.0;
    public static Stream GetResourceAsStream(this Type type, string resourceName)
    {
        //TODO:
        return null; 
    }

    public static StringBuilder Replace(this StringBuilder builder, int start,int end, string replace)
    {
        builder.Remove(start, end - start);
        builder.Insert(start, replace);
        return builder;
    }
}
