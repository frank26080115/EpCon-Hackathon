using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace SaysWho.HelperClasses {
	public class UserLoader : INotifyPropertyChanged {
		private WebClient m_wcLoadUserData;

		#region AccessToken
		private string m_strAccessToken;
		public string AccessToken {
			get { return m_strAccessToken; }
			set { m_strAccessToken = value; }
		}
		#endregion

		#region User
		private FBUser m_fbUser;
		public FBUser User {
			get { return m_fbUser; }
			set {
				if(m_fbUser != value) {
					m_fbUser = value;
					if(m_fbUser == null) {
						m_strLastError = "OK";
						m_strLastMessage = "Login to see user data";
						DBHelpers.DeleteDBUser();
					}
					OnPropertyChanged("User");
					OnPropertyChanged("ErrorVisible");	//either error or user
					OnPropertyChanged("UserVisible");
					OnPropertyChanged("UserEnabled");
					OnPropertyChanged("LoginEnabled");
				}
			}
		}
		#endregion
		#region UserEnabled
		public bool UserEnabled {
			get { return m_fbUser != null; }
		}
		#endregion
		#region LoginEnabled
		public bool LoginEnabled {
			get { return m_fbUser == null; }
		}
		#endregion
		#region LastMessage
		private string m_strLastMessage;
		public string LastMessage {
			get { return m_strLastMessage; }
			set {
				if(m_strLastMessage != value) {
					m_strLastMessage = value;
					OnPropertyChanged("LastMessage");
				}
			}
		}
		#endregion
		#region LastError
		private string m_strLastError;
		public string LastError {
			get { return m_strLastError; }
			set {
				if(m_strLastError != value) {
					m_strLastError = value;
					OnPropertyChanged("LastError");
				}
			}
		}
		#endregion
		#region UserVisible
		public Visibility UserVisible {
			get { return m_fbUser != null ? Visibility.Visible : Visibility.Collapsed; }
		}
		#endregion
		#region ErrorVisible
		public Visibility ErrorVisible {
			get { return m_fbUser == null ? Visibility.Visible : Visibility.Collapsed; }
		}
		#endregion

		public UserLoader() {
			m_strLastError = "OK";
			m_strLastMessage = "Login to see user data";
		}
		public void LoadUserData() {
			User = null;
			if(string.IsNullOrEmpty(AccessToken)) {
				UpdateStatus("Login to see user data", "OK");
				return;
			}
			if(m_wcLoadUserData == null) {
				m_wcLoadUserData = new WebClient();
				m_wcLoadUserData.DownloadStringCompleted += new DownloadStringCompletedEventHandler(m_wcLoadUserData_DownloadStringCompleted);
			}
			try {
				m_wcLoadUserData.DownloadStringAsync(FBUris.GetLoadUserDataUri(AccessToken));
				UpdateStatus("Loading user data", "OK");
			}
			catch(Exception eX) {
				UpdateStatus("Could not load user data", eX.Message);
			}
		}

		void m_wcLoadUserData_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e) {
			if(e.Error != null) {
				UpdateStatus("Error loading user data", e.Error.Message);
				return;
			}
			try {
				User = JsonStringSerializer.Deserialize<FBUser>(e.Result);
				//we got the new user - store access token
				DBHelpers.StoreDBUser(m_strAccessToken, User);
				UpdateStatus("User loaded");
			}
			catch(Exception eX) {
				User = null;
				UpdateStatus("Error parsing user data", eX.Message);
			}
		}
		private void UpdateStatus(string strMessage = "", string strError = "OK") {
			LastMessage = strMessage;
			LastError = strError;
		}
		#region INotifyPropertyChanged Members
		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged(string strProp) {
			if(PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(strProp));
			}
		}
		#endregion
	}
}