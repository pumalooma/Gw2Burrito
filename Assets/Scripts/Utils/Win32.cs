
using System;
using System.Runtime.InteropServices;
using System.Text;

public static class Win32 {



    [StructLayout(LayoutKind.Sequential)]
    public struct RECT {
        public int Left, Top, Right, Bottom;

        public int GetHeight () { return Bottom - Top; }
        public int GetWidth () { return Right - Left; }
    }

    public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
	
    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr FindWindow (string lpClassName, string lpWindowName);
    [DllImport("user32.dll", SetLastError = true)]
    public static extern int GetClassName (IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
    [DllImport("user32.dll")]
    //[return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetWindowRect (IntPtr hWnd, out RECT lpRect);
    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    public static extern bool SetWindowPos (IntPtr hwnd, IntPtr hwndInsertAfter, int x, int y, int cx, int cy, uint uFlags);


    public const int VK_LSHIFT = 0xA0;
    public const int VK_RSHIFT = 0xA1;
    public const int VK_LCONTROL = 0xA2;
    public const int VK_RCONTROL = 0xA3;
    public const int VK_LMENU = 0xA4;
    public const int VK_RMENU = 0xA5;

    public const int KEY_PRESSED = 0x8000;


    [DllImport("user32.dll")]
    public static extern short GetKeyState (int nVirtKey);
    [DllImport("user32.dll")]
    public static extern short GetAsyncKeyState (int nVirtKey);

    private struct MARGINS {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow ();

    [DllImport("user32.dll")]
    private static extern int SetWindowLong (IntPtr hWnd, int nIndex, uint dwNewLong);

    [DllImport("user32.dll")]
    static extern bool ShowWindowAsync (IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll", EntryPoint = "SetLayeredWindowAttributes")]
    static extern int SetLayeredWindowAttributes (IntPtr hwnd, int crKey, byte bAlpha, int dwFlags);

    [DllImport("Dwmapi.dll")]
    private static extern uint DwmExtendFrameIntoClientArea (IntPtr hWnd, ref MARGINS margins);

    const int GWL_STYLE = -16;
	const int GWL_EXSTYLE = -20;
	const uint WS_POPUP = 0x80000000;
    const uint WS_VISIBLE = 0x10000000;
	const uint WS_EX_LAYERED = 524288;
	const uint WS_EX_TRANSPARENT = 32;

	const uint SWP_NOSIZE = 0x0001;
    const uint SWP_NOMOVE = 0x0002;
	const uint SWP_FRAMECHANGED = 0x0020;
    public const uint SWP_SHOWWINDOW = 0x0040;


	
	public static readonly IntPtr HWND_NOT_TOPMOST = new IntPtr(-2);
	
    public static void MakeWindowTransparent (IntPtr hwnd) {
        var margins = new MARGINS() { cxLeftWidth = -1 };

        SetWindowLong(hwnd, GWL_STYLE, WS_POPUP | WS_VISIBLE);

        SetLayeredWindowAttributes(hwnd, 0, 255, 2); // LWA_ALPHA=2
        SetWindowPos(hwnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_FRAMECHANGED | SWP_SHOWWINDOW | SWP_NOMOVE | SWP_NOSIZE);
        DwmExtendFrameIntoClientArea(hwnd, ref margins);
	}

	public static void AllowClicking(IntPtr hwnd, bool enable) {
		if(enable)
			SetWindowLong(hwnd, GWL_EXSTYLE, 0);
		else
			SetWindowLong(hwnd, GWL_EXSTYLE, WS_EX_LAYERED | WS_EX_TRANSPARENT);
	}
}
