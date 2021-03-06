﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using System.Text;

public enum WebRequestMethod
{
GET,
POST,
PUT
}

public class Communications : MonoBehaviour
{
    private Coroutine routine = default;
    public WebRequestMethod Method = WebRequestMethod.POST;

    public void PostForm(string endpoint, WWWForm form, System.Action<ResponseData<UserData>> callback, WebRequestMethod method = WebRequestMethod.POST)
    { 
        routine = StartCoroutine(Communicate(Path.Combine(Globals.BASE_URL, endpoint), form, callback, method)); 
        
    }
    public void PostForm(string endpoint, WWWForm form, System.Action<ResponseData<FBLoginResponseData>> callback, WebRequestMethod method = WebRequestMethod.POST)
    {
        routine = StartCoroutine(Communicate(Path.Combine(Globals.BASE_URL, endpoint), form, callback, method));

    }

    IEnumerator Communicate(string url, WWWForm form, System.Action<ResponseData<UserData>> _callback, WebRequestMethod method)
    {
         
        using (UnityWebRequest www =  UnityWebRequest.Post(url, form))
        {
            if (method == WebRequestMethod.PUT)
                www.method = "PUT";

            yield return www.SendWebRequest();
            while (!www.isDone)
                yield return null;

            if (www.isNetworkError || www.isHttpError)
            {
                print(www.error);
                ResponseData<UserData> error = new ResponseData<UserData>();
                _callback(null);
            }
            else
            {
                print(www.downloadHandler.text);
                ResponseData<UserData> data = JsonUtility.FromJson<ResponseData<UserData>>(www.downloadHandler.text);

                _callback(data);
            }
        } 
    }


    IEnumerator Communicate(string url, WWWForm form, System.Action<ResponseData<FBLoginResponseData>> _callback, WebRequestMethod method)
    {

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();
            while (!www.isDone)
                yield return null;

            if (www.isNetworkError || www.isHttpError)
            {
                print(www.error);
                ResponseData<FBLoginResponseData> error = new ResponseData<FBLoginResponseData>();
                _callback(null);
            }
            else
            {
                print(www.downloadHandler.text);
                ResponseData<FBLoginResponseData> data = JsonUtility.FromJson<ResponseData<FBLoginResponseData>>(www.downloadHandler.text);
                _callback(data);
            }
        }
    }

    public void Terminate()
    {
        if (routine != null) { 
            StopCoroutine(routine);
        }
    }

} 
