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
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Media.Imaging;

namespace SaysWho
{
    public partial class Login : PhoneApplicationPage
    {
        public Login()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.AccessToken = "TOKEN SET";
        }

        private void MainBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            MainBrowser.Navigate(SaysWho.HelperClasses.FBUris.GetLoginUri());
        }

        private void MainBrowser_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            string strLoweredAddress = e.Uri.OriginalString.ToLower();
            if (strLoweredAddress.StartsWith("http://www.facebook.com/connect/login_success.html?code="))
            {
                txtStatus.Text = "Trying to retrieve access token";
                TryToGetToken(e.Uri.OriginalString.Substring(56));                
                return;
            }
            if (strLoweredAddress.StartsWith("http://www.facebook.com/connect/login_success.html?") && strLoweredAddress.Contains("user_denied"))
            {
                txtStatus.Text = "Problem - use back to leave page";
                txtError.Text = "User pressed cancel - or didn't allow app to work";
                return;
            }
            string strTest = MainBrowser.SaveToString();
            if (strTest.Contains("access_token"))
            {
                int nPos = strTest.IndexOf("access_token");
                string strPart = strTest.Substring(nPos + 13);
                nPos = strPart.IndexOf("</PRE>");
                if (nPos < 0)
                {
                    nPos = strPart.IndexOf("</pre>");
                }

                strPart = strPart.Substring(0, nPos);
                nPos = strPart.IndexOf("&amp;expires");
                if (nPos != -1)
                {
                    strPart = strPart.Substring(0, nPos);
                }
                App.AccessToken = strPart;
                App.CurUserHolder.LoadUserData();	//fetch user data in background
                txtStatus.Text = "Authenticated";
                txtError.Text = "OK";

                WebClient wc = new WebClient();

                wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_get_completed);

                string at = App.AccessToken;
                Uri u = SaysWho.HelperClasses.FBUris.GetLoadUserDataUri(at);

                wc.DownloadStringAsync(u);

                return;
            }
            if (strLoweredAddress.StartsWith("https://touch.facebook.com/login.php") || strLoweredAddress.StartsWith("http://touch.facebook.com/login.php"))
            {
                txtStatus.Text = "Please login using your credentials";
                txtError.Text = "OK";
                return;
            }
            if (strLoweredAddress.StartsWith("http://www.facebook.com/connect/uiserver.php"))
            {	//app requirements are checked
                txtStatus.Text = "Please accept application requirements";
                txtError.Text = "Accept button missing? - scroll down";
                return;
            }
            if (strLoweredAddress.StartsWith("https://login.facebook.com/login.php?app_id"))
            {	//new login error :)
                txtStatus.Text = "New Login problems?";
                txtError.Text = "We try a login again - this should help";
                //donno why (it is already true) but setting is script enabled enables helps here:)
                //wbLogin.IsScriptEnabled = true;
                MainBrowser.Navigate(SaysWho.HelperClasses.FBUris.GetLoginUri());	//login again
                return;
            }
            //txtStatus.Text = "Unkown page loaded (login problems?)";
            //txtError.Text = e.Uri.OriginalString;
        }

        public void wc_get_completed(object sender, DownloadStringCompletedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/GamePlay.xaml?user=" + e.Result, UriKind.Relative));
        }

        private void TryToGetToken(string strCode)
        {
            MainBrowser.Navigate(SaysWho.HelperClasses.FBUris.GetTokenLoadUri(strCode));

        }
    }
}