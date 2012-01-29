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
	public class DBUser : FBUser	{
		#region Token
		private string m_strToken;
		public string Token {
			get { return m_strToken; }
			set { m_strToken = value; }
		}
		#endregion
		
		public DBUser(string strToken, string strID, string strName, string strPictureLink, string strGender, string strLink, string strHometown) {
			m_strToken = strToken;
			ID = strID;
			Name = strName;
			PictureLink = strPictureLink;
			Gender = strGender;
			Link = strLink;
			HomeTown = new FBHomeTown() { Name = strHometown };
		}
	}
}
