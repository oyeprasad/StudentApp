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
    private Dictionary<int ,string> videosLocalPath = new Dictionary<int ,string>();
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
    void ResolveYoutubeUrl(string YoutubeUrl, string videoname)
    {
        print("Youtube URL to resolve "+YoutubeUrl);
        YoutubePlayer.YoutubePlayer youtubePlayer = new YoutubePlayer.YoutubePlayer();
        
        print("video name "+videoname);
        youtubePlayer.GetVideoResolvedUrl(YoutubeUrl, videoname,OnYoutubeUrlResolved);
    }

    void OnYoutubeUrlResolved(string resolvedUrl, string videoName)
    {  
        OnVideoDownloaded(resolvedUrl);
         // StartCoroutine(DownloadVideo(resolvedUrl, videoName, OnVideoDownloaded));
    }
    IEnumerator DownloadVideo(string path, string videoname, System.Action<string> callback)
    {  
        print("path "+path);
        print("videoname "+videoname);

        UnityWebRequest www = UnityWebRequest.Get(path);
        yield return www.SendWebRequest();

        if(www.isNetworkError || www.isHttpError) {
            callback(string.Empty);
        } else {
            print("Writing...  "+videoname);
            File.WriteAllBytes(Path.Combine(Application.persistentDataPath, videoname), www.downloadHandler.data);
            callback(Path.Combine(Application.persistentDataPath, videoname));
        }
    }

    void OnVideoDownloaded(string path)
    {
        VideoDownLaoded.Invoke(path);
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
