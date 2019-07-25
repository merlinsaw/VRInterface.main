//-------------------------------------------------------
//	
//-------------------------------------------------------



public static class ExtensionMethods
{
    /// <summary>
    /// Remaps a float range to a nother range.
    /// </summary>
    /// <param name="from">The float to remap.</param>
    /// <param name="fromMin">The minimum of the given float.</param>
    /// <param name="fromMax">The maximum of the given float.</param>
    /// <param name="toMin">the new mapped minimum of the new float.</param>
    /// <param name="toMax">The new mapped maximum of the new float.</param>
    /// <returns></returns>
    public static float Remap(this float from, float fromMin, float fromMax, float toMin, float toMax)
    {
        var fromAbs = from - fromMin;
        var fromMaxAbs = fromMax - fromMin;

        var normal = fromAbs / fromMaxAbs;

        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;

        var to = toAbs + toMin;

        return to;
    }
}