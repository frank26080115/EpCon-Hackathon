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
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Diagnostics;

namespace SaysWho.HelperClasses {
    [DataContract]
	public class FBFriendsList : INotifyPropertyChanged {
		private WebClient m_wcLoadFriendsList;
		private Stopwatch m_swLoadTime;
		public class FBFriendsListDesirializeHelper {
			#region Friends
			private FBFriend[] m_aFriends;
			[DataMember(Name = "data")]
			public FBFriend[] Friends {
				get { return m_aFriends; }
				set {
					if(m_aFriends != value) {
						m_aFriends = value;
					}
				}
			}
			#endregion
		}

		#region AccessToken
		private string m_strAccessToken;
		public string AccessToken {
			get { return m_strAccessToken; }
			set { m_strAccessToken = value; }
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
		#region OwnerID
		private string m_strOwnerID;
		public string OwnerID {
			get { return m_strOwnerID; }
			set {
				if(m_strOwnerID != value) {
					m_strOwnerID = value;
					if(Friends != null && Friends.Length > 0) {	//remove friends from other user
						Friends = null;
					}
					OnPropertyChanged("OwnerID");
				}
			}
		}
		#endregion
		#region Friends
		private FBFriend[] m_aFriends;
        [DataMember(Name = "data")]
		public FBFriend[] Friends {
			get { return m_aFriends; }
			set {
				if(m_aFriends != value) {
					m_aFriends = value;
					OnPropertyChanged("Friends");
				}
			}
		}
		#endregion

		#region LoadEnabled
		private bool m_bLoadEnabled;
		public bool LoadEnabled {
			get { return m_bLoadEnabled; }
			private set {
				if(m_bLoadEnabled != value) {
					m_bLoadEnabled = value;
					OnPropertyChanged("LoadEnabled");
				}
			}
		}
		#endregion

		public FBFriendsList() {
			m_swLoadTime = new Stopwatch();
			m_strLastError = "OK";
			m_strLastMessage = "No friends loaded";
			if(m_wcLoadFriendsList == null) {
				m_wcLoadFriendsList = new WebClient();
				m_wcLoadFriendsList.DownloadStringCompleted += new DownloadStringCompletedEventHandler(m_wcLoadFriendsList_DownloadStringCompleted);
			}
			LoadEnabled = true;
		}
		void m_wcLoadFriendsList_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e) {
			if(e.Error != null) {
				UpdateStatus("Error loading friends", e.Error.Message);
			}
			else {
				try {
					FBFriendsListDesirializeHelper lFriends = JsonStringSerializer.Deserialize<FBFriendsListDesirializeHelper>(e.Result);
					m_swLoadTime.Stop();
					string strWebTime = m_swLoadTime.ElapsedMilliseconds.ToString("N");
					m_swLoadTime.Reset();
					m_swLoadTime.Start();
					DBHelpers.StoreFriends(App.CurUserHolder.User.ID, lFriends.Friends);
					lFriends.Friends = DBHelpers.ReadFriends(App.CurUserHolder.User.ID);
					m_swLoadTime.Stop();
					string strDBTime = m_swLoadTime.ElapsedMilliseconds.ToString("N");
					Friends = lFriends.Friends;
					UpdateStatus("Friends loaded", "Web: " + strWebTime + "  DB: " + strDBTime);
				}
				catch(Exception eX) {
					UpdateStatus("Error parsing friends list", eX.Message);
				}
			}
			LoadEnabled = true;
		}
		public void ClearFriendsInMemory() {
			Friends = null;
		}
		#region LoadFriendsIfNeeded
		public void LoadFriendsIfNeeded() {
			if(!LoadEnabled) {	//loading in progress
				Debug.WriteLine("Load friends was blocked");
				return;
			}
			LoadEnabled = false;
			if(Friends != null && Friends.Length > 0) {	//friends are there
				LoadEnabled = true;
				UpdateStatus("Friends are loaded", "No load needed");
				return;
			}
			m_swLoadTime.Start();
			try {
				FBFriend[] aFriends = DBHelpers.ReadFriends(OwnerID);
				if(aFriends.Length > 0) {
					Friends = aFriends;
					m_swLoadTime.Stop();
					UpdateStatus("Friends loaded", "DB: " + m_swLoadTime.ElapsedMilliseconds.ToString("N"));
					LoadEnabled = true;
				}
				else {
					Uri uT = FBUris.GetLoadFriendsUri(AccessToken);
					m_wcLoadFriendsList.DownloadStringAsync(uT);
					UpdateStatus("Loading Friends", "OK");
				}
			}
			catch(Exception eX) {
				LoadEnabled = true;
				UpdateStatus("Loading friends failed", eX.Message);
			}
		}
		#endregion
		public void ReloadFriends() {
			if(!LoadEnabled) {	//loading in progress
				Debug.WriteLine("Reload friends was blocked");
				return;
			}
			LoadEnabled = false;
			m_swLoadTime.Start();
			try {
				Uri uT = FBUris.GetLoadFriendsUri(AccessToken);
				m_wcLoadFriendsList.DownloadStringAsync(uT);
				UpdateStatus("Loading Friends", "OK");
			}
			catch(Exception eX) {
				LoadEnabled = true;
				UpdateStatus("Loading friends failed", eX.Message);
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