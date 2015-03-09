using UnityEngine;
using System.Collections;

public class PainterRPCMethods : MonoBehaviour
{
    public static Texture2D Texture { get; private set; }

    private Vector2 _preDrag = Vector3.zero;
    private Color _currentColor = Color.red;
    private float _currentBrushSize = 10;
    private float _currentBrushHardness = 1;

    [RPC]
    void CreateTexture(int width, int height)
    {
        Debug.LogWarning("RPC::CreateTexture: Create new texture size = " + width + ", " + height);
        Texture = new Texture2D(width, height);
        _ChangeTextureColor(Texture, Color.white);
        PainterGui.OnTextureCreated(Texture);
    }

    private void _ChangeTextureColor(Texture2D texture, Color color)
    {
        Color[] pixels = new Color[texture.width * texture.height];
        for (int i = 0; i < pixels.Length; ++i)
        {
            pixels[i] = color;
        }
        texture.SetPixels(pixels);
        texture.Apply();
    }
    [RPC]
    void UpdateMousePosition(Vector3 mousePos)
    {
        _preDrag = mousePos;
        mousePosition = mousePos;
    }
    [RPC]
    void UpdateBrushColor(Vector3 brushColor)
    {
        _currentColor = new Color(brushColor.x, brushColor.y, brushColor.z, 255);
    }
    [RPC]
    void UpdateBrushSize(float brushSize)
    {
        _currentBrushSize = brushSize;
    }
    [RPC]
    void UpdateBrushHardness(float brushHardness)
    {
        _currentBrushHardness = brushHardness;
    }
    Vector2 mousePosition = Vector2.zero;
    [RPC]
    void UpdateTexture(Vector3 mousePos)
    {
        mousePosition = mousePos;
    }
    void Update()
    {
        if (Texture != null && (_preDrag - mousePosition).sqrMagnitude > 20f)
        {
            Painter.DrawLine(mousePosition, _preDrag, _currentColor, _currentBrushSize, _currentBrushHardness, Texture);
            Texture.Apply();
            _preDrag = mousePosition;
        }
    }
}
