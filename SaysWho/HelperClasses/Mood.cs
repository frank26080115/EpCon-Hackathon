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
	public class Mood : INotifyPropertyChanged {
		#region ShortText
		private string m_strShortText;
		public string ShortText {
			get { return m_strShortText; }
			set {
				if(m_strShortText != value) {
					m_strShortText = value;
					OnPropertyChanged("ShortText");
				}
			}
		}
		#endregion
		#region FeedText
		private string m_strFeedText;
		public string FeedText {
			get { return m_strFeedText; }
			set {
				if(m_strFeedText != value) {
					m_strFeedText = value;
					OnPropertyChanged("FeedText");
				}
			}
		}
		#endregion
		#region ImageName
		private string m_strImageName;
		public string ImageName {
			get { return m_strImageName; }
			set {
				if(m_strImageName != value) {
					m_strImageName = value;
					OnPropertyChanged("ImageName");
				}
			}
		}
		#endregion
		#region ImagePath
		public string ImagePath {
			get { return "/Images/" + ImageName;}
		}
		#endregion

		public static Mood[] GetMoods() {
			Mood[] aMoods ={
							   new Mood{ ShortText="Great", FeedText="I feel great - the bug is fixed, the bug is fixed, the bug is fixed...", ImageName="BlueE.png"},
							   new Mood{ ShortText="Lucky", FeedText="I feel lucky - my first WP7 application made passed the stroe review", ImageName="Green4.png"},
							   new Mood{ ShortText="Jubilant", FeedText="I feel jubilant - my first WP7 app was sold", ImageName="GreenE.png"},
							   new Mood{ ShortText="Amorous", FeedText="I feel amorous - ASAP (in about an hour or five) I'll stop coding and check the bedroom for...", ImageName="Purple1.png"},
							   new Mood{ ShortText="Aroused", FeedText="I feel aroused - three options - check if she / he is still awake, have a cold shower, visit www.por...", ImageName="Purple3.png"},
							   new Mood{ ShortText="Tired", FeedText="I feel tired - could someone bring me a power drink please?", ImageName="Yellow2.png"},
							   new Mood{ ShortText="Confused", FeedText="I feel confused - what proves that I shouldn't code WP7 applications", ImageName="BlackC.png"},
							   new Mood{ ShortText="Cold", FeedText="I feel cold - I'll start the new 6D multi-surround game so my PC can raise the temperature", ImageName="Cyan0.png"},
							   new Mood{ ShortText="Lonely", FeedText="I feel lonely - can someone call me to go out for a beer (or two)?", ImageName="Black0.png"},
							   new Mood{ ShortText="Sad", FeedText="I feel sad - can someone post something nice to may wall please?", ImageName="Yellow2.png"},
							   new Mood{ ShortText="Angry", FeedText="I feel angry - you'd better don't talk to me", ImageName="Red5.png"},
							   new Mood{ ShortText="Guilty", FeedText="I feel guilty - I need a common plea", ImageName="BlackE.png"},
							   new Mood{ ShortText="Useless", FeedText="I feel useless - I need some idea to write a great WP7 application", ImageName="Cyan2.png"},
							   new Mood{ ShortText="Bored", FeedText="I feel bored - that's why I post this crap", ImageName="Yellow0.png"},
							   new Mood{ ShortText="Devastated", FeedText="I feel devastated - is anybody out there who can comfort me?", ImageName="UC.png"},
						   };
			return (aMoods);
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
