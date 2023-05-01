using UnityEngine;

public static class TransformExtensions
{
    public static bool IsNear(this Transform t, Transform target, float distance)
    {
        var d = (t.position - target.position).sqrMagnitude;
        return d < distance * distance;
    }

    public static bool IsNear(this Transform t, Vector3 target, float distance)
    {
        var d = (t.position - target).sqrMagnitude;
        return d < distance * distance;
    }

    public static bool IsNear2D(this Transform t, Transform target, float distance)
    {
        var d = (t.position - target.position);
        d.z = 0f;
        return d.sqrMagnitude < distance * distance;
    }

    public static bool IsNear2D(this Transform t, Vector2 target, float distance)
    {
        var d = new Vector2(t.position.x - target.x, t.position.y - target.y);
        return d.sqrMagnitude < distance * distance;
    }
}