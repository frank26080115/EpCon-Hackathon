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

namespace SaysWho
{
    public partial class FacebookLogin : PhoneApplicationPage
    {
        public FacebookLogin()
        {
            InitializeComponent();
        }

        private void wbLogin_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                wbFacebook.Navigate(FBUris.GetLoginUri());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }
    }
}