using UnityEngine;
using System;
using System.Collections;

public class WindowTracker : MonoBehaviour {

    [SerializeField]
    private Material m_Material;

#if !UNITY_EDITOR
    private IntPtr hwndApp;

    // Use this for initialization
    void OnEnable () {
        hwndApp = Win32.FindWindow("UnityWndClass", "GW2 Burrito");

        Win32.MakeWindowTransparent(hwndApp);

        StartCoroutine(TrackWindow());
    }

    private IEnumerator TrackWindow () {
        

        while (isActiveAndEnabled) {

            IntPtr hwndGw2 = Win32.FindWindow("ArenaNet_Dx_Window_Class", null);
            if (hwndGw2 != IntPtr.Zero) {

                var rect = new Win32.RECT();
                Win32.GetWindowRect(hwndGw2, out rect);

                Win32.SetWindowPos(hwndApp, new IntPtr(-1), rect.Left, rect.Top, rect.GetWidth(), rect.GetHeight(), Win32.SWP_SHOWWINDOW);
            }

            yield return new WaitForSeconds(1.0f);
            
        }
    }
#endif

    void OnRenderImage (RenderTexture from, RenderTexture to) {
        Graphics.Blit(from, to, m_Material);
    }

}
