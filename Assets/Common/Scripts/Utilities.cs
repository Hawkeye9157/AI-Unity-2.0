using UnityEngine;

public static class Utilities
{
    public static float Wrap(float y, float min, float max)
    {
        if (y < min) return max - .25f;
        if (y > max) return min + .25f;
        return y;
    }
    public static Vector3 Wrap(Vector3 v, Vector3 min, Vector3 max)
    {
        v.x = Wrap(v.x, min.x, max.x);
        v.z = Wrap(v.z, min.z, max.z);
        return v;
    }
}
