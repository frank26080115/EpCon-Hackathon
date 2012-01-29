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
	public class FBFeed {
		[DataContract]
		public class ToList {
			#region To
			protected FBFriend[] m_aTo;
			[DataMember(Name = "data")]
			public FBFriend[] To {
				get { return m_aTo; }
				set { m_aTo = value; }
			}
			#endregion
		}
		#region ID
		protected string m_strID;
		[DataMember(Name = "id")]
		public string ID {
			get { return m_strID; }
			set { m_strID = value; }
		}
		#endregion
		#region From
		protected FBFriend m_fbfFrom;
		[DataMember(Name = "from")]
		public FBFriend From {
			get { return m_fbfFrom; }
			set { m_fbfFrom = value; }
		}
		#endregion
		#region ToFirst (first name from "to array")
		protected FBFriend m_fbfToFirst;
		public FBFriend ToFirst {
			get { return m_fbfToFirst; }
			set { m_fbfToFirst = value; }
		}
		#endregion
		#region To
		protected ToList m_aTo;
		[DataMember(Name = "to")]
		public ToList To {
			get { return m_aTo; }
			set {
				m_aTo = value;
				if(m_aTo != null && m_aTo.To!=null && m_aTo.To.Length > 0) {
					m_fbfToFirst = m_aTo.To[0];
				}
			}
		}
		#endregion

		#region Name
		private string m_strName;
		[DataMember(Name="name")]
		public string Name {
			get { return m_strName; }
			set { m_strName = value; }
		}
		#endregion

		#region Description
		private string m_strDescription;
		[DataMember(Name="description")]
		public string Description {
			get { return m_strDescription; }
			set { m_strDescription = value; }
		}
		#endregion
		
		#region Attribution
		private string m_strAttribution;
		[DataMember(Name="attribution")]
		public string Attribution {
			get { return m_strAttribution; }
			set { m_strAttribution = value; }
		}
		#endregion

		#region Caption
		private string m_strCaption;
		[DataMember(Name = "caption")]
		public string Caption {
			get { return m_strCaption; }
			set { m_strCaption = value; }
		}
		#endregion
		#region Message
		private string m_strMessage;
		[DataMember(Name="message")]
		public string Message {
			get { return m_strMessage; }
			set { m_strMessage = value; }
		}
		#endregion
	}
}
