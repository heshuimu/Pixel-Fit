﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using UnityEngine;
using MiniJSON;

public static class MSHealthInterface  
{
	private const string ClientId = "0000000040186653";
	private const string ClientSecret = "4PUCOWzczvm4GhpECz3sVgf-7DLBo4bi";
	private const string Scopes = "mshealth.ReadProfile mshealth.ReadDevices mshealth.ReadActivityHistory mshealth.ReadActivityLocation mshealth.ReadDevices";
	public const string BaseHealthUri = "https://api.microsofthealth.net/v1/me/";
	public const string RedirectUri = "https://login.live.com/oauth20_desktop.srf";
	
	public static string AccessToken
	{
		get
		{
			return PlayerPrefs.GetString("pxFit_MSHealthInterface_AccessToken", "");
		}
		set
		{
			PlayerPrefs.SetString("pxFit_MSHealthInterface_AccessToken", value);
			PlayerPrefs.Save();
		}
	}
	
	public static long TokenExpireTime
	{
		get
		{
			return PlayerPrefs.GetInt("pxFit_MSHealthInterface_TokenExpireTime", 0);
		}
		set
		{
			PlayerPrefs.SetInt("pxFit_MSHealthInterface_TokenExpireTime", (int)value);
			PlayerPrefs.Save();
		}
	}
	
	public static string RefreshToken
	{
		get
		{
			return PlayerPrefs.GetString("pxFit_MSHealthInterface_RefreshToken", "");
		}
		set
		{
			PlayerPrefs.SetString("pxFit_MSHealthInterface_RefreshToken", value);
			PlayerPrefs.Save();
		}
	}
	
	public static string UserID
	{
		get
		{
			return PlayerPrefs.GetString("pxFit_MSHealthInterface_UserID", "");
		}
		set
		{
			PlayerPrefs.SetString("pxFit_MSHealthInterface_UserID", value);
			PlayerPrefs.Save();
		}
	}
	
	public static string AuthToken
	{
		get
		{
			return PlayerPrefs.GetString("pxFit_MSHealthInterface_AuthToken", "");
		}
		set
		{
			PlayerPrefs.SetString("pxFit_MSHealthInterface_AuthToken", value);
			PlayerPrefs.Save();
		}
	}
		
	 public static void signinButton_Click()
       {
            string url = "https://login.live.com/oauth20_authorize.srf?";

            url += String.Format("redirect_uri={0}", Uri.EscapeDataString(RedirectUri));
            url += String.Format("&client_id={0}", Uri.EscapeDataString(ClientId));

            url += String.Format("&scope={0}", Uri.EscapeDataString(Scopes));
            url += String.Format("&response_type=code");
		
		GameObject go = new GameObject("Sign In Webview");
		UniWebView uwv = go.AddComponent<UniWebView>();
		uwv.toolBarShow = true;
		uwv.url  = url;
		uwv.OnLoadComplete += WhenSignInViewFinishLoading;
		
		uwv.Load();
		uwv.Show();
       }
	 
	 public static void WhenSignInViewFinishLoading	(UniWebView webView, bool success, string errorMessage)	
	 {
		 if(success)
		 {
			Debug.Log(webView.currentUrl);
			Uri myUri = new Uri(webView.currentUrl);
			
			if (!myUri.LocalPath.StartsWith("/oauth20_desktop.srf", StringComparison.OrdinalIgnoreCase))
				return;
				
			string AccessCode = ExtractValueWithHeader(myUri.Query, "code", '&');
			
			Debug.Log(AccessCode);
			
			if(!String.IsNullOrEmpty(AccessCode))
			{
				GameObject go =  new GameObject();
				CoroutineInvoker ci = go.AddComponent<CoroutineInvoker>();
				ci.coroutine = WaitForAccessToken(webView, AccessCode, false);
				ci.InvokeCoroutine();
			}
		 }
		 else
		 {
			 Debug.Log("Webpage Load Unsuccessful: " + errorMessage);
		 }
	 }
	 
	 private static string ExtractValueWithHeader(string query, string header, char divider)
	 {
		 int indexOfHeader = query.IndexOf(header);
		 switch(indexOfHeader)
		 {
			case -1: return null;
			default: 
				int startOfTheValue = indexOfHeader+header.Length+1;
				string s = "";
				while(startOfTheValue < query.Length && query[startOfTheValue] != divider)
				{
					s += query[startOfTheValue];
					startOfTheValue++;
				}
				return s;
		 }
	 }
	 
	 private static IEnumerator WaitForAccessToken(UniWebView webView, string code, bool isRefresh)
	 {
		Debug.Log("bool");
		string url = "https://login.live.com/oauth20_token.srf?";
		
		url += String.Format("redirect_uri={0}", Uri.EscapeDataString(RedirectUri));
		url += String.Format("&client_id={0}", Uri.EscapeDataString(ClientId));
		url += String.Format("&client_secret={0}", Uri.EscapeDataString(ClientSecret));
		
		if (isRefresh)
		{
			url += String.Format("&refresh_token={0}", Uri.EscapeDataString(code));
			url += String.Format("&grant_type=refresh_token");
		}
		else
		{
			url += String.Format("&code={0}", Uri.EscapeDataString(code));
			url += String.Format("&grant_type=authorization_code");
		}
		
		Debug.Log(url);
		
		WWW tokenReq = new WWW(url);
		
		yield return tokenReq;
		
		Dictionary<string, object> JSONData = (Dictionary<string, object>) Json.Deserialize(tokenReq.text);
		
		Debug.Log(tokenReq.text);
		
		object JSONError;
		
		if(tokenReq.error != null) 
		{
			Debug.Log(tokenReq.error);
		}
		else if(JSONData.TryGetValue("error", out JSONError))
		{
			Debug.Log((string)JSONError);
		}
		else
		{
			if (isRefresh)
			{
				MSHealthInterface.RefreshToken = (string)JSONData["refresh_token"];
			}
			else
			{
				MSHealthInterface.AccessToken = (string)JSONData["access_token"];
			}
			
			MSHealthInterface.TokenExpireTime = (long)JSONData["expires_in"];
			MSHealthInterface.UserID = (string)JSONData["user_id"];
			MSHealthInterface.AuthToken = (string)JSONData["authentication_token"];
			
			GameObject.Destroy(webView.gameObject);
		
			Debug.Log(AccessToken);
			Debug.Log(TokenExpireTime);
			Debug.Log(UserID);
		}
	 }
	 
	 public static void PerformRequest(IMSHealthJSONResponder responder, string baseURI, string relativePath, string queryParams = null)
	 {
		#if UNITY_IOS
		
		PerformRequestSync(responder, baseURI, relativePath, queryParams);
		
		#else
		
		GameObject go =  new GameObject();
		CoroutineInvoker ci = go.AddComponent<CoroutineInvoker>();
		ci.coroutine = PerformRequestSequence(responder, relativePath, queryParams);
		ci.InvokeCoroutine();
		
		#endif
	 }
	 
	 #if UNITY_IOS
	 
	private static void PerformRequestSync(IMSHealthJSONResponder responder, string baseURI, string relativePath, string queryParams = null)
      {
		var uriBuilder = new UriBuilder(baseURI);
		uriBuilder.Path += relativePath;
		uriBuilder.Query = queryParams;
		
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create (uriBuilder.Uri);
		request.Headers[HttpRequestHeader.Authorization] = string.Format("bearer {0}", AccessToken);
		
		HttpWebResponse response = (HttpWebResponse)request.GetResponse();
		
		Stream receiveStream = response.GetResponseStream ();
		StreamReader reader =  new StreamReader(receiveStream);
		
		Debug.Log("DONE");
		
		Dictionary<string, object> JSONData = (Dictionary<string, object>) Json.Deserialize(reader.ReadToEnd());
		
		if(responder.RespondToJSONData(JSONData))
		{
			Debug.Log("Target consumed JSON Data successfully");
		}
		else
		{
			Debug.Log("Target failed to consume the JSON Data");
		}
	 }
	 
	 #else
	 
	 private static IEnumerator PerformRequestSequence(IMSHealthJSONResponder responder, string relativePath, string queryParams = null)
       {
		var uriBuilder = new UriBuilder(BaseHealthUri);
		uriBuilder.Path += relativePath;
		uriBuilder.Query = queryParams;
		
		Debug.Log(uriBuilder.Uri.AbsoluteUri);
		
		Dictionary<string, string> WWWHeaderDictionary = new Dictionary<string, string>();
		
		WWWHeaderDictionary.Add("Authorization", string.Format("bearer {0}", AccessToken));
		WWW infoReq = new WWW(uriBuilder.Uri.AbsoluteUri, null, WWWHeaderDictionary);
		
		float timer = 0;
		
		while(timer < 5 && !infoReq.isDone)
		{
			yield return null;
			timer += Time.deltaTime;
		}
		if(timer > 5)
		{
			Debug.Log("Timeout");
		}
		
		Debug.Log("DONE");
		
		Dictionary<string, object> JSONData = (Dictionary<string, object>) Json.Deserialize(infoReq.text);
		
		if(responder.RespondToJSONData(JSONData))
		{
			Debug.Log("Target consumed JSON Data successfully");
		}
		else
		{
			Debug.Log("Target failed to consume the JSON Data");
		}
	 }
	 
	 #endif 
}

public interface IMSHealthJSONResponder
{
	bool RespondToJSONData(Dictionary<string, object> dic);
}
