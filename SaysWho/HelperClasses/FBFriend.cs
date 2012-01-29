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

namespace SaysWho.HelperClasses {
	[DataContract]
	public class FBFriend {

		#region Name
		private string m_strName;
		[DataMember(Name = "name")]
		public string Name {
			get { return m_strName; }
			set { m_strName = value; }
		}
		#endregion

		#region ID
		private string m_strID;
		[DataMember(Name = "id")]
		public string ID {
			get { return m_strID; }
			set { m_strID = value; }
		}
		#endregion

		#region PicLink
		public string PicLink {
			get { return string.Format("http://graph.facebook.com/{0}/picture", m_strID ?? "ManniAT"); }
		}
		#endregion

	}
}
