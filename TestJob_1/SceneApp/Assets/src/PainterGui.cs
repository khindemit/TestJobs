using UnityEngine;
using System.Collections;

public class PainterGui : MonoBehaviour
{
    public static void OnTextureCreated(Texture2D texture)
    {
        GameObject go = GameObject.Find("WhiteBoard");
        if (go != null)
        {
            Material mat = go.GetComponent<MeshRenderer>().material;
            mat.SetTexture(0, texture);
        }
    }
}
