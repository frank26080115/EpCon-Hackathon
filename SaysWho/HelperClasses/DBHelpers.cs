using System;
using System.Data.EffiProz;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Resources;
using System.Collections.Generic;

// visit http://effiproz.pp-p.net for closer information
namespace SaysWho.HelperClasses {
	public class DBHelpers {
		private const string m_strConnection = "connection type=FILE; initial catalog=DBs/FBData;auto commit=true;user=SA; password=";
		private static EfzCommand m_cmdReadDBUser;
		private static EfzCommand m_cmdDeleteDBUser;
		private static EfzCommand m_cmdStoreDBUser;
		private static EfzCommand m_cmdReadFriends;
		private static EfzCommand m_cmdDeleteFriends;
		private static EfzCommand m_cmdInsertFriend;
		private static EfzConnection m_conDB;
		private static bool m_bDidCheckFiles;
		private static bool EnsureDBInIsolatedStorage() {
			try {	//brute force - should be handled with more details normally
				StreamResourceInfo srInfo;
				IsolatedStorageFile isfStore = IsolatedStorageFile.GetUserStoreForApplication();
				IsolatedStorageFileStream isfsDest;
				if(!isfStore.DirectoryExists("DBs")) {
					isfStore.CreateDirectory("DBs");
				}
				if(!isfStore.FileExists("DBs/FBData.properties")) {	//if properties file is not present copy all files
					srInfo = Application.GetResourceStream(new Uri("InitialData/FBData.properties", UriKind.Relative));
					isfsDest = new IsolatedStorageFileStream("DBs/FBData.properties", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write, isfStore);
					CopyStream(srInfo.Stream, isfsDest);
					isfsDest.Close();
					isfsDest.Dispose();
					srInfo.Stream.Close();
					srInfo.Stream.Dispose();
					srInfo = Application.GetResourceStream(new Uri("InitialData/FBData.script", UriKind.Relative));
					isfsDest = new IsolatedStorageFileStream("DBs/FBData.script", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write, isfStore);
					CopyStream(srInfo.Stream, isfsDest);
					isfsDest.Close();
					isfsDest.Dispose();
					srInfo.Stream.Close();
					srInfo.Stream.Dispose();
				}
			}
			catch(Exception eX) {
				Debug.WriteLine("Error copying DB files: " + eX.Message);
				return (false);
			}
			m_bDidCheckFiles = true;
			return (true);
		}
		public static bool EnsureDBConnection() {
			if(m_conDB != null) {	//already existing
				if(m_conDB.State != ConnectionState.Open) {
					m_conDB.Open();
				}
				return (true);
			}
			if(!m_bDidCheckFiles) {
				if(!EnsureDBInIsolatedStorage()) {
					return (false);
				}
			}
			m_conDB = new EfzConnection(m_strConnection);
			m_conDB.Open();
			return (true);
		}

		public static FBFriend[] ReadFriends(string strOwnerID) {
			EnsureDBConnection();
			if(m_cmdReadFriends == null) {
				m_cmdReadFriends = new EfzCommand("SELECT FriendID, Name FROM Friends WHERE OwnerID=@OwnerID ORDER BY Name", m_conDB);
				m_cmdReadFriends.Parameters.Add(new EfzParameter() { DbType = EfzType.VarChar, ParameterName = "@OwnerID" });
			}
			m_cmdReadFriends.Parameters["@OwnerID"].Value = strOwnerID;
			EfzDataReader eReader = m_cmdReadFriends.ExecuteReader();
			List<FBFriend> lFriends = new List<FBFriend>();
			while(eReader.Read()) {
				lFriends.Add(new FBFriend() { ID = eReader.GetString(0), Name = eReader.GetString(1) });
			}
			eReader.Close();
			eReader.Dispose();
			return (lFriends.ToArray());
		}
		public static void DeleteFriends(string strOwnerID) {
			EnsureDBConnection();
			if(m_cmdDeleteFriends == null) {
				m_cmdDeleteFriends = new EfzCommand("DELETE FROM Friends WHERE OwnerID=@OwnerID", m_conDB);
				m_cmdDeleteFriends.Parameters.Add(new EfzParameter() { DbType = EfzType.VarChar, ParameterName = "@OwnerID" });
			}
			m_cmdDeleteFriends.Parameters["@OwnerID"].Value = strOwnerID;
			m_cmdDeleteFriends.ExecuteNonQuery();
		}
		public static void StoreFriends(string strOwnerID, FBFriend[] aFriends) {
			EnsureDBConnection();
			if(m_cmdInsertFriend == null) {
				m_cmdInsertFriend = new EfzCommand("INSERT INTO Friends(OwnerID, FriendID, Name) VALUES(@OwnerID, @FriendID, @Name)", m_conDB);
				m_cmdInsertFriend.Parameters.Add(new EfzParameter() { DbType = EfzType.VarChar, ParameterName = "@OwnerID" });
				m_cmdInsertFriend.Parameters.Add(new EfzParameter() { DbType = EfzType.VarChar, ParameterName = "@FriendID" });
				m_cmdInsertFriend.Parameters.Add(new EfzParameter() { DbType = EfzType.VarChar, ParameterName = "@Name" });
			}
			//brute force - don't check for existing / new - simply kill the old ones and enter the current friends
			DeleteFriends(strOwnerID);

			m_cmdInsertFriend.Parameters["@OwnerID"].Value = strOwnerID;
			int nCnt = aFriends.Length;
			for(int nX = 0; nX < nCnt; nX++) {
				m_cmdInsertFriend.Parameters["@FriendID"].Value = aFriends[nX].ID;
				m_cmdInsertFriend.Parameters["@Name"].Value = aFriends[nX].Name;
				m_cmdInsertFriend.ExecuteNonQuery();
			}
		}
		//doesn't fire exceptions
		public static DBUser ReadDBUser() {
			EnsureDBConnection();
			if(m_cmdReadDBUser == null) {
				m_cmdReadDBUser = new EfzCommand("SELECT TOKEN, ID, NAME, PICTURE_LINK, GENDER, LINK, HOMETOWN, STORE_TIME FROM DB_USER WHERE STORE_TIME>@AllowedStoreTime", m_conDB);
				m_cmdReadDBUser.Parameters.Add(new EfzParameter() { DbType = EfzType.Date, ParameterName = "@AllowedStoreTime" });
			}
			m_cmdReadDBUser.Parameters["@AllowedStoreTime"].Value = DateTime.Now.AddHours(-1);	//allow access for 1 hour
			try {
				EfzDataReader eRead = m_cmdReadDBUser.ExecuteReader();
				DBUser dbRet = null;
				if(eRead.Read()) {
					dbRet = new DBUser(eRead.GetString(0), eRead.GetString(1), eRead.GetString(2), eRead.GetString(3), eRead.GetString(4), eRead.GetString(5), eRead.GetString(6));
				}
				eRead.Close();
				eRead.Dispose();
				return (dbRet);
			}
			catch { return (null); }
		}
		//doesn't fire exceptions
		public static void DeleteDBUser() {
			EnsureDBConnection();
			if(m_cmdDeleteDBUser == null) {
				m_cmdDeleteDBUser = new EfzCommand("DELETE FROM DB_USER", m_conDB);
			}
			try {
				m_cmdDeleteDBUser.ExecuteNonQuery();
			}
			catch { }
		}
		//doesn't fire exceptions
		public static void StoreDBUser(string strToken, FBUser fbU) {
			EnsureDBConnection();
			if(m_cmdStoreDBUser == null) {
				m_cmdStoreDBUser = new EfzCommand("INSERT INTO DB_USER(TOKEN, ID, NAME, PICTURE_LINK, GENDER, LINK, HOMETOWN, STORE_TIME) VALUES(@Token, @ID, @Name, @PictureLink, @Gender, @Link, @Hometown, @StoreTime)", m_conDB);
				m_cmdStoreDBUser.Parameters.Add(new EfzParameter() { DbType = EfzType.VarChar, ParameterName = "@Token" });
				m_cmdStoreDBUser.Parameters.Add(new EfzParameter() { DbType = EfzType.VarChar, ParameterName = "@ID" });
				m_cmdStoreDBUser.Parameters.Add(new EfzParameter() { DbType = EfzType.VarChar, ParameterName = "@Name" });
				m_cmdStoreDBUser.Parameters.Add(new EfzParameter() { DbType = EfzType.VarChar, ParameterName = "@PictureLink" });
				m_cmdStoreDBUser.Parameters.Add(new EfzParameter() { DbType = EfzType.VarChar, ParameterName = "@Gender" });
				m_cmdStoreDBUser.Parameters.Add(new EfzParameter() { DbType = EfzType.VarChar, ParameterName = "@Link" });
				m_cmdStoreDBUser.Parameters.Add(new EfzParameter() { DbType = EfzType.VarChar, ParameterName = "@Hometown" });
				m_cmdStoreDBUser.Parameters.Add(new EfzParameter() { DbType = EfzType.Date, ParameterName = "@StoreTime" });
			}
			DeleteDBUser();
			m_cmdStoreDBUser.Parameters["@Token"].Value = strToken ?? "";	//no null - instead empty string
			m_cmdStoreDBUser.Parameters["@ID"].Value = fbU.ID;			//FBUser is "null save"
			m_cmdStoreDBUser.Parameters["@Name"].Value = fbU.Name;
			m_cmdStoreDBUser.Parameters["@PictureLink"].Value = fbU.PictureLink;
			m_cmdStoreDBUser.Parameters["@Gender"].Value = fbU.Gender;
			m_cmdStoreDBUser.Parameters["@Link"].Value = fbU.Link;
			m_cmdStoreDBUser.Parameters["@Hometown"].Value = fbU.HomeTown.Name;
			m_cmdStoreDBUser.Parameters["@StoreTime"].Value = DateTime.Now;
			try {
				m_cmdStoreDBUser.ExecuteNonQuery();
			}
			catch { }
		}
		#region Copy helper
		private static void CopyStream(System.IO.Stream sSource, IsolatedStorageFileStream sDestination) {
			byte[] byBuffer = new byte[32768];
			while(true) {
				int nRead = sSource.Read(byBuffer, 0, byBuffer.Length);
				if(nRead <= 0) {
					break;
				}
				sDestination.Write(byBuffer, 0, nRead);
			}
		}
		#endregion
	}
}