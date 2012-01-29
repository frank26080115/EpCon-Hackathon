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
	public class FBFeedList {
		#region Feeds
		private FQStreamResult[] m_aFeeds;
		[DataMember(Name = "data")]
		public FQStreamResult[] Feeds {
			get { return m_aFeeds; }
			set { m_aFeeds = value; }
		}
		#endregion
	}
}
