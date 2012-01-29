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

namespace SaysWho.HelperClasses {
	public class FBWallPost {	
		#region TheCaption
		private string m_strTheCaption;
		public string TheCaption {
			get { return m_strTheCaption; }
			set { m_strTheCaption = value; }
		}
		#endregion
		#region TheDescription
		private string m_strTheDescription;
		public string TheDescription {
			get { return m_strTheDescription; }
			set { m_strTheDescription = value; }
		}
		#endregion
		#region TheLink
		private string m_strTheLink;
		public string TheLink {
			get { return m_strTheLink; }
			set { m_strTheLink = value; }
		}
		#endregion
		#region TheMessage
		private string m_strTheMessage;
		public string TheMessage {
			get { return m_strTheMessage; }
			set { m_strTheMessage = value; }
		}
		#endregion
		#region TheName
		private string m_strTheName;
		public string TheName {
			get { return m_strTheName; }
			set { m_strTheName = value; }
		}
		#endregion
		#region ThePictureLink
		private string m_strThePictureLink;
		public string ThePictureLink {
			get { return m_strThePictureLink; }
			set { m_strThePictureLink = value; }
		}
		#endregion

		public FBWallPost(){}
		public FBWallPost(bool bFillDefaults){
			if(bFillDefaults){
				TheCaption = "How I feel";
				TheDescription = "For those who ask - How are you?";
				TheLink = "http://iphone.pp-p.net";
				TheMessage = "I'm not sure";
				TheName = "My mood";
				ThePictureLink = "http://games.pp-p.net/Badges/RedAE.png";
			}
		}
		public FBWallPost(Mood moOD) {
			TheLink = "http://effiproz.pp-p.net";
			TheDescription = "For those who ask - How are you?";
			TheName = "I feel...";
			TheCaption = moOD.ShortText;
			TheMessage = moOD.FeedText;
			ThePictureLink = "http://games.pp-p.net/Badges/" + moOD.ImageName;
		}
	

		public string GetPostParameters(string strAccessToken) {
			try {
				string strRet = "access_token=" + strAccessToken;
				//string strRet = "access_token=" + HttpUtility.UrlEncode(strAccessToken);
				if (!string.IsNullOrEmpty(m_strTheCaption))	{
					strRet += "&caption=" + HttpUtility.UrlEncode(m_strTheCaption);
				}
				if(!string.IsNullOrEmpty(m_strTheDescription)){
					strRet += "&description=" + HttpUtility.UrlEncode(m_strTheDescription);
				}
				if(!string.IsNullOrEmpty(m_strTheLink)){
					strRet += "&link=" + HttpUtility.UrlEncode(m_strTheLink);
				}
				if(!string.IsNullOrEmpty(m_strTheMessage)){
					strRet += "&message=" + HttpUtility.UrlEncode(m_strTheMessage);
				}
				if(!string.IsNullOrEmpty(m_strTheName)){
					strRet += "&name=" + HttpUtility.UrlEncode(m_strTheName);
				}
				if(!string.IsNullOrEmpty(m_strThePictureLink)){
					strRet += "&picture=" + HttpUtility.UrlEncode(m_strThePictureLink);
				}
				return (strRet);
			}
			catch { return (""); }
		}
		public static FBWallPost GetTestPost(){
			FBWallPost fbwpRet = new FBWallPost();
			fbwpRet.TheCaption = "Capt: Test";
			fbwpRet.TheDescription = "Desc: Test";
			fbwpRet.TheLink = "http://games.pp-p.net";
			fbwpRet.TheMessage = "Message sent from windows phone 7";
			fbwpRet.TheName = "Name: Test";
			fbwpRet.ThePictureLink = "http://games.pp-p.net/Badges/RedAE.png";
			return (fbwpRet);
		}
	}
}
