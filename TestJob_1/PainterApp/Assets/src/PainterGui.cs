using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class PainterGui : MonoBehaviour
{
    private Vector3 _storedPos = Vector3.zero;
    void Start()
    {
        _CreateTexture();
    }
    public void OnCreateTextureButtonClick()
    {
        _CreateTexture();
    }
    private void _CreateTexture()
    {
        GetComponent<NetworkView>().RPC("CreateTexture", RPCMode.AllBuffered, Screen.width, Screen.height);
    }
    public void OnChangeColorToggleClick(GameObject toogle)
    {
        Color color = toogle.GetComponent<Image>().color;
        Vector3 colorVec = new Vector3(color.r, color.g, color.b);
        GetComponent<NetworkView>().RPC("UpdateBrushColor", RPCMode.All, colorVec);
    }
    public void OnBrushSizeChanged(float brushSize)
    {
        GetComponent<NetworkView>().RPC("UpdateBrushSize", RPCMode.All, brushSize);
    }
    public void OnBrushHardnessChanged(float brushHardness)
    {
        GetComponent<NetworkView>().RPC("UpdateBrushHardness", RPCMode.All, brushHardness);
    }

    void Update()
    {
        if (PainterRPCMethods.Texture != null)
        {
            if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == null)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    GetComponent<NetworkView>().RPC("UpdateMousePosition", RPCMode.All, Input.mousePosition);
                    _storedPos = Input.mousePosition;
                }
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    if ((_storedPos - Input.mousePosition).sqrMagnitude > 200f)
                    {
                        GetComponent<NetworkView>().RPC("UpdateTexture", RPCMode.All, Input.mousePosition);
                        _storedPos = Input.mousePosition;
                    }
                }
            }
        }
    }
    public static void OnTextureCreated(Texture2D texture)
    {
        GameObject go = GameObject.Find("RawImage");
        RawImage rawImage = go.GetComponent<RawImage>();
        rawImage.texture = texture;
        rawImage.enabled = true;
        rawImage.SetNativeSize();
    }
}
