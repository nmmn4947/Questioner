using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Assuming you still use TextMeshPro
using Unity.Burst.CompilerServices; // Not directly used in this logic, but kept if you need it elsewhere
using UnityEngine.UIElements; // Not directly used in this logic, but kept if you need it elsewhere
using Unity.VisualScripting; // Not directly used in this logic, but kept if you need it elsewhere

public class TransparentWindow : MonoBehaviour
{
    // --- Existing DllImports for Window Manipulation ---
    [DllImport("user32.dll")]
    public static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    private struct MARGINS
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cxTopHeight;
        public int cxBottomHeight;
    }

    [DllImport("Dwmapi.dll")]
    private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);

    // --- Constants for Window Styles ---
    const int GWL_EXSTYLE = -20;
    const uint WS_EX_LAYERED = 0x00080000;
    const uint WS_EX_TRANSPARENT = 0x00000020;

    private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

    private IntPtr hWnd;
    /// ////////////////// /// ////////////////// /// ////////////////// /// ////////////////// /// //////////////////

    // --- New DllImport and Constants for Global Input Detection ---
    [DllImport("user32.dll")]
    private static extern short GetAsyncKeyState(int vKey); // To check key state globally

    // Virtual key codes (common ones)
    private const int VK_LBUTTON = 0x01; // Left mouse button
    private const int VK_RBUTTON = 0x02; // Right mouse button (not used in this logic but good to have)
    private const int VK_LMENU = 0xA4;   // Left ALT key
    private const int VK_RMENU = 0xA5;   // Right ALT key

    // --- Public Reference to TextMeshProUGUI ---
    public TextMeshProUGUI m_TextMeshProUGUI;

    // --- Internal State Variable ---
    private bool isWindowClickthrough = true; // Tracks the current state of the window's click-through property

    private void Start()
    {
        // MessageBox(new IntPtr(0), "Hello World!", "HelloDialog", 0); // Kept for debugging if needed

#if !UNITY_EDITOR
        // Get the handle to the current Unity game window
        hWnd = GetActiveWindow();

        // Set the window to be always on top (optional, but common for overlays)
        // Parameters: hWnd, HWND_TOPMOST, X, Y, Width, Height, Flags
        // SWP_NOMOVE (0x0002) and SWP_NOSIZE (0x0001) flags are often used to keep current position/size
        // 0 for all flags might work, but explicit is better for clarity if you don't want to move/resize
        SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, 0); 

        // Initial state: Set window to be transparent and layered (click-through)
        SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_LAYERED | WS_EX_TRANSPARENT);

        // Extend the DWM frame into the client area to enable full transparency
        // cxLeftWidth = -1 means to extend the frame over the entire window
        MARGINS margins = new MARGINS { cxLeftWidth = -1 };
        DwmExtendFrameIntoClientArea(hWnd, ref margins);
#endif
        // Ensure the application continues to run even when not in focus
        Application.runInBackground = true;

        // Initialize TextMeshPro text
        if (m_TextMeshProUGUI != null)
        {
            m_TextMeshProUGUI.text = "Initializing: Click-Through";
        }
    }

    private void Update()
    {
        // Check if Left ALT key is pressed globally
        bool isAltPressed = (GetAsyncKeyState(VK_LMENU) & 0x8000) != 0 || (GetAsyncKeyState(VK_RMENU) & 0x8000) != 0;

        // Check if Left Mouse Button is pressed globally
        bool isLeftClickPressed = (GetAsyncKeyState(VK_LBUTTON) & 0x8000) != 0;

        // Determine if the window should currently be interactive (not click-through)
        bool shouldBeInteractive = isAltPressed && isLeftClickPressed;

        // --- Manage Window Click-Through State ---
        if (shouldBeInteractive && isWindowClickthrough)
        {
            // If ALT+LeftClick is held AND the window is currently click-through, make it interactive
            SetClickthrough(false);
            PingWheel.instance.activatePing();
            if (m_TextMeshProUGUI != null)
            {
                m_TextMeshProUGUI.text = "Interactive Mode (ALT+Click held)";
            }
        }
        else if (!shouldBeInteractive && !isWindowClickthrough)
        {
            // If ALT+LeftClick is NOT held AND the window is currently interactive, make it click-through again
            SetClickthrough(true);
            PingWheel.instance.deActivatePing();
            if (m_TextMeshProUGUI != null)
            {
                m_TextMeshProUGUI.text = "Click-Through Mode";
            }
        }
    }

    /// <summary>
    /// Sets the window's click-through property by modifying its extended style.
    /// </summary>
    /// <param name="clickthrough">If true, the window will be click-through. If false, it will be interactive.</param>
    private void SetClickthrough(bool clickthrough)
    {
        // Only apply changes if the state is actually changing
        if (isWindowClickthrough != clickthrough)
        {
            if (clickthrough)
            {
                // Set window to be layered and transparent (click-through)
                SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_LAYERED | WS_EX_TRANSPARENT);
            }
            else
            {
                // Set window to be layered but NOT transparent (interactive)
                SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_LAYERED);
            }
            isWindowClickthrough = clickthrough; // Update the internal state
        }
    }

    // You might want to add an OnApplicationQuit to gracefully restore original window styles
    // if your application has more complex lifecycle needs.
    // However, for simple overlays, letting the OS clean up on process termination is often fine.
}