using System.Collections;
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
        } else
        {
            HomeMainUIController.ShowPopup.Invoke(videoData.message, () => HomeMainUIController.EventBackClicked.Invoke());
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
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(videoData.video_thumbnail);
        yield return www.SendWebRequest();
        while(!www.isDone)
        yield return null;

        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
            float height = 0;
            float width = 0;
            Texture2D myTexture = (Texture2D)((DownloadHandlerTexture)www.downloadHandler).texture;
            Sprite sprite =  Sprite.Create(myTexture, new Rect(0.0f, 0.0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            width = myTexture.width;
            height = myTexture.height;
            GameObject newItem = Instantiate(thumbnailPrefab, _parent);
            newItem.GetComponent<Thumbnail>().Populate(videoData, sprite, width, height);
 
        }
 
    }
    
    void Clear()
    {
        for (int i = 0; i < _parent.childCount; i++)
        {
            Destroy(_parent.GetChild(i).gameObject);
        }
    }
}
