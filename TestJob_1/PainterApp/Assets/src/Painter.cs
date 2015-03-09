using UnityEngine;
using System.Collections;

public static class Painter
{
    public static void DrawLine(Vector2 from, Vector2 to, Color color, float radius, float hardness, Texture2D texture)
    {
        int startY = Mathf.Clamp((int)(Mathf.Min(from.y, to.y) - radius), 0, texture.height);
        int startX = Mathf.Clamp((int)(Mathf.Min(from.x, to.x) - radius), 0, texture.width);
        int endY = Mathf.Clamp((int)(Mathf.Max(from.y, to.y) + radius), 0, texture.height);
        int endX = Mathf.Clamp((int)(Mathf.Max(from.x, to.x) + radius), 0, texture.width);

        int lengthX = Mathf.Abs(endX - startX);
        int lengthY = Mathf.Abs(endY - startY);

        float sqrRad2 = (radius + 1) * (radius + 1);
        Color[] pixels = texture.GetPixels((int)startX, (int)startY, (int)lengthX, (int)lengthY, 0);
        Vector2 start = new Vector2(startX, startY);
        for (int y = 0; y < lengthY; y++)
        {
            for (int x = 0; x < lengthX; x++)
            {
                Vector2 p = new Vector2(x, y) + start;
                Vector2 center = p + new Vector2(0.5f, 0.5f);
                float dist = (center - _NearestPointStrict(from, to, center)).sqrMagnitude;
                if (dist > sqrRad2)
                {
                    continue;
                }
                dist = _GaussFalloff(Mathf.Sqrt(dist), radius) * hardness;
                Color c = Color.white;
                if (dist > 0)
                {
                    c = Color.Lerp(pixels[y * lengthX + x], color, dist);
                }
                else
                {
                    c = pixels[y * lengthX + x];
                }

                pixels[y * lengthX + x] = c;
            }
        }
        texture.SetPixels((int)start.x, (int)start.y, (int)lengthX, (int)lengthY, pixels, 0);
    }

    private static Vector2 _NearestPointStrict(Vector2 lineStart, Vector2 lineEnd, Vector2 point)
    {
        Vector2 fullDirection = lineEnd - lineStart;
        Vector2 lineDirection = _Normalize(fullDirection);
        float closestPoint = Vector2.Dot((point - lineStart), lineDirection) / Vector3.Dot(lineDirection, lineDirection);
        return lineStart + (Mathf.Clamp(closestPoint, 0.0f, fullDirection.magnitude) * lineDirection);
    }
    private static Vector2 _Normalize(Vector2 p)
    {
        float mag = p.magnitude;
        return p / mag;
    }
    private static float _GaussFalloff(float distance, float radius)
    {
        return Mathf.Clamp01(Mathf.Pow(360, -Mathf.Pow(distance / radius, 2.5f) - 0.01f));
    }
}
