using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class TransparentWindow : MonoBehaviour
{
    [SerializeField]
    private Material m_Material;

    private struct MARGINS
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

    [DllImport("Dwmapi.dll")]
    private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);

    const int GWL_STYLE = -16;
    const uint WS_POPUP = 0x80000000;
    const uint WS_VISIBLE = 0x10000000;

    void Start()
    {
#if !UNITY_EDITOR   // You really don't want to enable this in the editor..
        var margins = new MARGINS() { cxLeftWidth = -1 };
        var hwnd = GetActiveWindow();

        SetWindowLong(hwnd, GWL_STYLE, WS_POPUP | WS_VISIBLE);

        DwmExtendFrameIntoClientArea(hwnd, ref margins);
#endif
    }

    void OnRenderImage(RenderTexture from, RenderTexture to)
    {
        Graphics.Blit(from, to, m_Material);
    }
}