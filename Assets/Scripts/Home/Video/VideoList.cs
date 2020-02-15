﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Events;
using UnityEngine.Networking;

public class VideoList : MonoBehaviour
{
    public static StringEvent VideoDownLaoded = new StringEvent();
    [SerializeField] private Text titleText;
    [SerializeField] private GameObject thumbnailPrefab;
    [SerializeField] private Transform _parent;
    private int subCatId;
    public void Populate(int _subCatId, string _subCatName)
    {
        Clear();
        titleText.text = _subCatName;
        subCatId = _subCatId;
        HomeMainUIController.EventShowHideLoader.Invoke(true); 
        StartCoroutine(LoadVideoDetails(subCatId, OnCompleteVideoLoad));
    }
    IEnumerator LoadVideoDetails(int _subCatId, System.Action<VideoResponseData> callback)
    {
        string endpoint = WebRequests.Instance.getVideoEndPoint;
        print("request video for subcat id "+_subCatId);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(Globals.BASE_URL + endpoint+_subCatId))
        {
            // Request and wait for the desired page.
            print("URL "+webRequest.url);
            webRequest.SetRequestHeader("Accept", "application/json");//
            webRequest.SetRequestHeader("Authorization", "Bearer "+Globals.UserLoginDetails.access_token);

            yield return webRequest.SendWebRequest();
            while(!webRequest.isDone)
                yield return webRequest;
            if (webRequest.isNetworkError)
            {
              callback(null);
              print("Error! " + webRequest.responseCode);  
            }
            else
            {
                print(webRequest.downloadHandler.text);
                callback(JsonUtility.FromJson<VideoResponseData> (webRequest.downloadHandler.text));
            }
        } 
    }
     private void OnCompleteVideoLoad(VideoResponseData videoData)
    {     
        if (videoData.status)
        {
            PopulatePanel(videoData.data.videos);
        }
        HomeMainUIController.EventShowHideLoader.Invoke(false); 

    }
    
    private void PopulatePanel(List<VideoData> allVideosData)
    {   
        foreach(VideoData item in allVideosData)
        {
           StartCoroutine(DownloadThumbnail(item));
        }
    } 

    IEnumerator DownloadThumbnail(VideoData videoData)
    {
        // code for DownloadThumbnail
        yield return null;
        GameObject newItem = Instantiate(thumbnailPrefab, _parent);
        newItem.GetComponent<Thumbnail>().Populate(videoData, null);
    }
    
    void Clear()
    {
        for (int i = 0; i < _parent.childCount; i++)
        {
            Destroy(_parent.GetChild(0).gameObject);
        }
    }
}
