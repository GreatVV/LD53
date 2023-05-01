using UnityEngine;

public static class SpriteRendererExtensions
{
    public static void SetAlpha(this SpriteRenderer s, float alpha)
    {
        var c = s.color;
        c.a = alpha;
        s.color = c;
    }
}