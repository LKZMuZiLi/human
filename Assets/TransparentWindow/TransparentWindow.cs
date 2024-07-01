#if UNITY_STANDALONE_WIN && !UNITY_EDITOR && TRANSPARENT_WINDOW
using System;
using System.Runtime.InteropServices;
#endif
using UnityEngine;

namespace LKZ.TransparentWindow
{
    public sealed class TransparentWindow : MonoBehaviour
    { 

#if UNITY_STANDALONE_WIN && !UNITY_EDITOR && TRANSPARENT_WINDOW
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

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        private static extern int SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int x, int y, int cx, int cy,
            int uFlags);

        [DllImport("user32.dll")]
        static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", EntryPoint = "SetLayeredWindowAttributes")]
        static extern int SetLayeredWindowAttributes(IntPtr hwnd, int crKey, byte bAlpha, int dwFlags);

        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        const int GWL_STYLE = -16;
        const int GWL_EXSTYLE = -20;
        const uint WS_POPUP = 0x80000000;
        const uint WS_VISIBLE = 0x10000000;

        const uint WS_EX_TOPMOST = 0x00000008;
        const uint WS_EX_LAYERED = 0x00080000;
        const uint WS_EX_TRANSPARENT = 0x00000020;

        const int SWP_FRAMECHANGED = 0x0020;
        const int SWP_SHOWWINDOW = 0x0040;
        const int LWA_ALPHA = 2;

        private IntPtr HWND_TOPMOST = new IntPtr(-1);

        private IntPtr _hwnd;

        void Start()
        { 
    MARGINS margins = new MARGINS() { cxLeftWidth = -1 };
    _hwnd = GetActiveWindow();    
     SetWindowLong(_hwnd, GWL_STYLE, WS_POPUP | WS_VISIBLE);

       var currentRes = Screen.currentResolution;
      SetWindowPos(_hwnd, HWND_TOPMOST, currentRes.width-540, currentRes.height-960-30, 540, 960, SWP_SHOWWINDOW);
        DwmExtendFrameIntoClientArea(_hwnd, ref margins);  
            Debug.Log(22);

        } 
       
#endif


    }
}