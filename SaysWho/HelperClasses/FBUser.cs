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
	public class FBUser {
		#region ID
		private string m_strID;
		[DataMember(Name = "id")]
		public string ID {
			get { return m_strID ?? ""; }
			set { m_strID = value; }
		}
		#endregion
		#region Name
		private string m_strName;
		[DataMember(Name = "name")]
		public string Name {
			get { return m_strName ?? ""; }
			set { m_strName = value; }
		}
		#endregion
		#region PictureLink
		private string m_strPictureLink;
		[DataMember(Name = "picture")]
		public string PictureLink {
			get { return m_strPictureLink ?? ""; }
			set { m_strPictureLink = value; }
		}
		#endregion

		#region Gender
		private string m_strGender;
		[DataMember(Name = "gender")]
		public string Gender {
			get { return m_strGender ?? ""; }
			set { m_strGender = value; }
		}
		#endregion
		#region Link
		private string m_strLink;
		[DataMember(Name = "link")]
		public string Link {
			get { return m_strLink ?? ""; }
			set { m_strLink = value; }
		}
		#endregion
		#region HomeTown
		private FBHomeTown m_fbjtHomeTown;
		[DataMember(Name = "hometown")]
		public FBHomeTown HomeTown {
			get { return m_fbjtHomeTown ?? new FBHomeTown { Name = "" }; }
			set { m_fbjtHomeTown = value; }
		}
		#endregion
		[DataContract]
		public class FBHomeTown {
			#region Name
			private string m_strName;
			[DataMember(Name = "name")]
			public string Name {
				get { return m_strName ?? ""; }
				set { m_strName = value; }
			}
			#endregion
		}
	}
}
