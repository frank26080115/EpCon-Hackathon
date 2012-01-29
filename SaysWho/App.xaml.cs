using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SaysWho.HelperClasses;
using System.Threading;
using System.ComponentModel;


namespace SaysWho
{
    public partial class App : Application
    {
        #region AccessToken
        private static string m_strAccessToken;
        public static string AccessToken
        {
            get { return m_strAccessToken; }
            set
            {
                m_strAccessToken = value;
                m_ulCurUserHolder.AccessToken = value;
                m_fbflFriendsList.AccessToken = value;
            }
        }
        #endregion
        #region CurUserHolder
        private static UserLoader m_ulCurUserHolder;
        public static UserLoader CurUserHolder
        {
            get { return m_ulCurUserHolder; }
            private set { m_ulCurUserHolder = value; }
        }
        #endregion

        #region FriendsList
        private static FBFriendsList m_fbflFriendsList;
        public static FBFriendsList FriendsList
        {
            get { return m_fbflFriendsList; }
            set { m_fbflFriendsList = value; }
        }
        #endregion


        // Easy access to the root frame
        public PhoneApplicationFrame RootFrame { get; private set; }

        // Constructor
        public App()
        {
            UnhandledException += Application_UnhandledException;
            m_fbflFriendsList = new FBFriendsList();
            m_ulCurUserHolder = new UserLoader();
            m_ulCurUserHolder.PropertyChanged += new PropertyChangedEventHandler(m_ulCurUserHolder_PropertyChanged);
            InitDatabase();

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();
            string[] uris = new string[] 
            {
                this.GetThemeSettingsUri()
			};
            this.LoadResources(uris);
            DoSomeOtherStuff(500);	//loading and so on (splash screen)

        }
        private void InitDatabase()
        {
            DBUser dbErg = DBHelpers.ReadDBUser();	//try to read temp token
            if (dbErg != null)
            {
                AccessToken = dbErg.Token;
                m_ulCurUserHolder.User = dbErg;
            }
        }
        void m_ulCurUserHolder_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "User")
            {
                if (m_ulCurUserHolder.User != null)
                {
                    m_fbflFriendsList.OwnerID = m_ulCurUserHolder.User.ID;
                }
                else
                {
                    m_fbflFriendsList.OwnerID = null;
                }
            }
        }
        private void DoSomeOtherStuff(int nMilisecs = 1000)
        {
            Thread.Sleep(nMilisecs);	//simulate a bit loading (splash screen :))
        }

        private string GetThemeSettingsUri()
        {
            return "";
            string path = "/WPGesicht;Component/Themes/";
            string dictionary;

            if ((Visibility)this.Resources["PhoneDarkThemeVisibility"] == Visibility.Visible)
            {
                dictionary = "Dark.xaml";
            }
            else
            {
                dictionary = "Light.xaml";
            }

            return path + dictionary;
        }

        private void LoadResources(string[] uriStrings)
        {
            return;
            foreach (string uri in uriStrings)
            {
                ResourceDictionary resources = new ResourceDictionary();
                resources.Source = new Uri(uri, UriKind.Relative);

                foreach (object key in resources.Keys)
                {
                    object value = resources[key];
                    resources.Remove(key);
                    this.Resources.Add(key, value);
                }
            }
        }


        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        // Code to execute if a navigation fails
        void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}