using System.Text;


namespace NLaTexMath.Internal.util;

public static class MiscUtils
{
    public static Stream GetResourceAsStream(this Type type, string resourceName)
    {
        return null;
    }

    public static StringBuilder Replace(this StringBuilder builder, int start,int end, string replace)
    {
        builder.Remove(start, end - start);
        builder.Insert(start, replace);
        return builder;
    }
}
