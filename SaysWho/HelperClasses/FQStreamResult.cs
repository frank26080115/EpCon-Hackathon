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
	public class FQStreamResult : FBFeed {
		[DataContract]
		public class IAttachment {
			#region Name
			private string m_strName;
			[DataMember(Name = "name")]
			public string Name {
				get { return m_strName; }
				set { m_strName = value; }
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

			#region Description
			private string m_strDescription;
			[DataMember(Name = "description")]
			public string Description {
				get { return m_strDescription; }
				set { m_strDescription = value; }
			}
			#endregion
		}
		#region Attachment
		private IAttachment m_iaAttachment;
		[DataMember(Name = "attachment")]
		public IAttachment Attachment {
			get { return m_iaAttachment; }
			set {
				m_iaAttachment = value;
				if(m_iaAttachment != null) {
					Name = m_iaAttachment.Name;
					Caption = m_iaAttachment.Caption;
					Description = m_iaAttachment.Description;
				}
			}
		}
		#endregion
		#region PostID
		[DataMember(Name = "post_id")]
		public string PostID {
			get { return m_strID; }
			set { m_strID = value; }
		}
		#endregion

		#region ActorID - the user who made the post with the application stored in ID
		[DataMember(Name = "actor_id")]
		public string ActorID {
			get { return ""; }
			set {
				//here we could resolve our friend (or ourself)
				m_fbfFrom = new FBFriend() { Name = value, ID = value };
			}
		}
		#endregion
		#region SourceID - the wall where the post was found strored in Name
		[DataMember(Name = "source_id")]
		public string SourceID {
			get { return ""; }
			set {
				//here we could resolve our friend (or ourself)
				m_fbfToFirst = new FBFriend() { Name = value, ID = value };
			}
		}
		#endregion
	}
}