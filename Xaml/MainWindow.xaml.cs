using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XamlTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private const int GWL_STYLE = -16;

        private const int WS_MAXIMIZEBOX = 0x10000; //maximize button
        private const int WS_MINIMIZEBOX = 0x20000; //minimize button

        private const int WS_SIZEBOX = 0x40000; // size button
        private const int SWP_NOZORDER = 0x0004;


        [DllImport("user32.dll")]
        private static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll")]
        private static extern IntPtr DestroyMenu(IntPtr hWnd);

        // Documentation: https://docs.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.dllimportattribute?view=netcore-3.1
        [DllImport("user32.dll")]
        // Documentation: https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-deletemenu
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        private const uint MF_GRAYED = 0x00000001;
        private const int SC_CLOSE = 0xF060;

        private const int WS_SYSMENU = 0x80000;
        private const uint MF_ENABLED = 0x00000000;
        private const uint MF_DISABLED = 0x00000002;

        private const int MF_BYCOMMAND = 0x00000000;
        private const int MF_BYPOSITION = 0x00000400;
        public const int SC_SIZE = 0xF000;
        public const int SC_MOVE = 0xF010;
        public const int SC_MINIMIZE = 0xF020;
        public const int SC_MAXIMIZE = 0xF030;
        public const int SC_RESTORE = 0xF120;
        public const int SC_SEPARATOR = 0xF00F;
        IntPtr menuHandle;
        IntPtr hMenu;

        protected void DisableCloseButton()
        {
            if (_windowHandle == null)
                throw new InvalidOperationException("The window has not yet been completely initialized");

            menuHandle = GetSystemMenu(_windowHandle, true);
            hMenu = GetSystemMenu(_windowHandle, false);
            if (menuHandle == IntPtr.Zero)
            {
                //EnableMenuItem(menuHandle, SC_CLOSE, MF_BYCOMMAND | MF_GRAYED);

                //DeleteMenu(hMenu, SC_CLOSE, MF_BYCOMMAND);
                DeleteMenu(hMenu, SC_SIZE, MF_BYCOMMAND);        // Disable resizing
                //DeleteMenu(hMenu, SC_MOVE, MF_BYCOMMAND);        // Disable moving
                DeleteMenu(hMenu, SC_MINIMIZE, MF_BYCOMMAND);    // Disable minimizing
                DeleteMenu(hMenu, SC_MAXIMIZE, MF_BYCOMMAND);    // Disable maximizing
                DeleteMenu(hMenu, SC_RESTORE, MF_BYCOMMAND);    // Disable restore
                DeleteMenu(hMenu, 1, MF_BYPOSITION);    // Disable separator
                //DeleteMenu(hMenu, 1, MF_BYPOSITION);    // Disable restore
                //DeleteMenu(hMenu, 2, MF_BYPOSITION);    // Disable restore
                //DeleteMenu(hMenu, 3, MF_BYPOSITION);    // Disable restore
                //DeleteMenu(hMenu, 4, MF_BYPOSITION);    // Disable restore

                //DeleteMenu(hMenu, 5, MF_BYPOSITION);    // Disable restore
            }
        }



        protected void EnableCloseButton()
        {
            if (_windowHandle == null)
                throw new InvalidOperationException("The window has not yet been completely initialized");

            if (menuHandle != IntPtr.Zero)
            {
                EnableMenuItem(menuHandle, SC_CLOSE, MF_BYCOMMAND | MF_ENABLED);
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);

            if (menuHandle != IntPtr.Zero)
                DestroyMenu(menuHandle);
        }


        protected void HideAllButtons()
        {
            if (_windowHandle == null)
                throw new InvalidOperationException("The window has not yet been completely initialized");

            SetWindowLong(_windowHandle, GWL_STYLE, GetWindowLong(_windowHandle, GWL_STYLE) & ~WS_SYSMENU);
        }
        protected void ShowAllButtons()
        {
            if (_windowHandle == null)
                throw new InvalidOperationException("The window has not yet been completely initialized");

            SetWindowLong(_windowHandle, GWL_STYLE, GetWindowLong(_windowHandle, GWL_STYLE) | WS_SYSMENU);
        }

        public MainWindow()
        {
            InitializeComponent();
            this.SourceInitialized += MainWindow_SourceInitialized;

        }

        private IntPtr _windowHandle;

        private void MainWindow_SourceInitialized(object sender, EventArgs e)
        {
            _windowHandle = new WindowInteropHelper(this).Handle;

            //disable minimize button
            // DisableMinimizeButton();
            //DisableMaximizeButton();
            //HideMinimizeAndMaximizeButtons();
            DisableCloseButton();
            //EnableCloseButton();
            //OnClosing());
            //HideAllButtons();
            //ShowAllButtons();
            //DisableSizezeButton();

        }

        protected void DisableMinimizeButton()
        {
            if (_windowHandle == IntPtr.Zero)
                throw new InvalidOperationException("The window has not yet been completely initialized");

            SetWindowLong(_windowHandle, GWL_STYLE, GetWindowLong(_windowHandle, GWL_STYLE) & ~WS_MINIMIZEBOX);
        }

        protected void DisableMaximizeButton()
        {
            if (_windowHandle == null)
                throw new InvalidOperationException("The window has not yet been completely initialized");

            SetWindowLong(_windowHandle, GWL_STYLE, GetWindowLong(_windowHandle, GWL_STYLE) & ~WS_MAXIMIZEBOX);
        }
        protected void HideMinimizeAndMaximizeButtons()
        {
            if (_windowHandle == null)
                throw new InvalidOperationException("The window has not yet been completely initialized");

            SetWindowLong(_windowHandle, GWL_STYLE, GetWindowLong(_windowHandle, GWL_STYLE) & ~WS_MAXIMIZEBOX & ~WS_MINIMIZEBOX);
        }
        protected void DisableSizezeButton()
        {
            if (_windowHandle == null)
                throw new InvalidOperationException("The window has not yet been completely initialized");

            SetWindowLong(_windowHandle, GWL_STYLE, GetWindowLong(_windowHandle, GWL_STYLE) & ~WS_SIZEBOX);
            //EnableMenuItem(_windowHandle, 0, MF_DISABLED | MF_GRAYED | MF_BYPOSITION);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
