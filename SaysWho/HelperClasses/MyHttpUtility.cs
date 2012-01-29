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
using System.Collections.Generic;

namespace SaysWho.HelperClasses {
	public static class MyHttpUtility {
		internal static Dictionary<string, string> ParseQueryString(string strQuery) {
			Dictionary<string, string> result = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
			if(string.IsNullOrEmpty(strQuery)) {
				return result;
			}
			string strDec = HttpUtility.HtmlDecode(strQuery);
			int nDecLength = strDec.Length;
			int nPosName = 0;
			bool bIsFirst = true;
			string strName;
			int nPosValue;
			int nValueEndPos;
			
			while(nPosName <= nDecLength) {
				nPosValue = -1;
				nValueEndPos = -1;
				for(int nTmp = nPosName; nTmp < nDecLength; nTmp++) {
					if(nPosValue == -1 && strDec[nTmp] == '=') {
						nPosValue = nTmp + 1;
					}
					else if(strDec[nTmp] == '&') {
						nValueEndPos = nTmp;
						break;
					}
				}

				if(bIsFirst) {
					bIsFirst = false;
					if(strDec[nPosName] == '?') {
						nPosName++;
					}
				}

				if(nPosValue == -1) {
					strName = null;
					nPosValue = nPosName;
				}
				else {
					strName = HttpUtility.UrlDecode(strDec.Substring(nPosName, nPosValue - nPosName - 1));
				}
				if(nValueEndPos < 0) {
					nPosName = -1;
					nValueEndPos = strDec.Length;
				}
				else {
					nPosName = nValueEndPos + 1;
				}
				if(string.IsNullOrEmpty(strName))	{
					strName = strDec.Substring(nPosValue, nValueEndPos - nPosValue);
				}
				result.Add(strName, HttpUtility.UrlDecode(strDec.Substring(nPosValue, nValueEndPos - nPosValue)));
				if(nPosName == -1) {
					break;
				}
			}
			return (result);
		}
	}
}
