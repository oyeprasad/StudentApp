using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using UnityEngine.Events;
 
[System.Serializable] public class VideoResponseData : ResponseBase
{
    public VideoDataList data = new VideoDataList();
}
[System.Serializable] public class VideoData
{
    public int video_id = 0;
    public int sub_category_id = 0;
    public string video_title = "";
    public string thumbnail_path = "";
    public string video_path = "";
    public string sub_category_name = "";
}

[System.Serializable] public class VideoDataList
{
    public List<VideoData> videos = new List<VideoData>();
}


public class VideoPanelController : MonoBehaviour
{ 
    public static UnityEvent EventVideoFinish = new UnityEvent();
    [SerializeField] private Text titleText;
    [SerializeField] GameObject playVideoPanel, rePlayVideoPanel; 
    private int videoId = 0;
    private int _subCatId;
    private List<string> videosLocalPath = new List<string>();
    private int currentVideoIndex = -1;

    void Start()
    {
        EventVideoFinish.AddListener(OnVideoFinished);
    }
    void OnDisable()
    {
        currentVideoIndex = -1;
    }
    public void PlayNewVideo(string videopath)
    {
        if(videopath.Contains("youtube"))
        {
            YoutubePlayer.YoutubePlayer youtubePlayer = new YoutubePlayer.YoutubePlayer();
            youtubePlayer.GetVideoResolvedUrl(videopath, "",OnYoutubeUrlResolved);
        } 
        else
        {
            playVideoPanel.GetComponent<MainVideoPlayer>().PlayNewVideo(videopath);
        }
    }

   /* void OnYoutubeUrlResolved(string Path)
    {
        print("Youtube video resolved "+Path);
        playVideoPanel.GetComponent<MainVideoPlayer>().PlayNewVideo(Path);
    }*/

    /*public void PopulatePanel(int subCatId, string subCatName)
    {
        _subCatId = subCatId;
        titleText.text = subCatName.ToUpper();
        playVideoPanel.SetActive(true);
        rePlayVideoPanel.SetActive(false); 
        HomeMainUIController.EventShowHideLoader.Invoke(true); 
        StartCoroutine(LoadVideoDetails(subCatId, OnCompleteVideoLoad));
    }*/

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
            for(int i = 0; i < videoData.data.videos.Count; i++)
            {
              // videoData.data.videos[i].video_path = "https://sample-videos.com/video123/mp4/720/big_buck_bunny_720p_5mb.mp4";
                if(!File.Exists(Path.Combine(Application.persistentDataPath, videoData.data.videos[i].video_id + ".mp4")))
                {
                    if(videoData.data.videos[i].video_path.Contains("youtube"))
                    {
                        ResolveYoutubeUrl(videoData.data.videos[i].video_path, videoData.data.videos[i].video_id + ".mp4");
                    }
                    else
                    {
                        StartCoroutine(DownloadVideo(videoData.data.videos[i].video_path, videoData.data.videos[i].video_id+".mp4", OnVideoDownloaded));  
                    }
                } 
                else
                {
                    OnVideoDownloaded(Path.Combine(Application.persistentDataPath, videoData.data.videos[i].video_id + ".mp4"));
                }
            }
        }
        HomeMainUIController.EventShowHideLoader.Invoke(false); 

    }

    public void LowerButtonClicked(string buttonName)
    {
        switch(buttonName)
        {
            case "quizzes":
                HomeMainUIController.EventQuizzesClicked.Invoke(videoId);
                break;
            case "worksheet":
                print("Clicked for worksheet");
                 HomeMainUIController.EventWorkSheetClicked.Invoke(_subCatId);
                HomeMainUIController.ShowPopup.Invoke("COMING SOON!", () => print("No action on worksheet clicked"));
                break;
            case "games":
                print("Clicked for games");
                HomeMainUIController.ShowPopup.Invoke("COMING SOON!", () => print("No action on worksheet clicked"));
                break;
            case "help":
                print("Clicked for help");
                HomeMainUIController.ShowPopup.Invoke("COMING SOON!", () => print("No action on worksheet clicked"));
                break;
        }
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
        print("Youtube video resolved "+resolvedUrl);
        playVideoPanel.GetComponent<MainVideoPlayer>().PlayNewVideo(resolvedUrl);
        //StartCoroutine(DownloadVideo(resolvedUrl, videoName, OnVideoDownloaded));
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
        print("videosLocalPath downloaded at "+ path);
        
       /* if(!string.IsNullOrEmpty(path))
        {
            videosLocalPath.Add(path);
            if(currentVideoIndex == -1)
            {
                currentVideoIndex += 1;
                playVideoPanel.GetComponent<MainVideoPlayer>().PlayNewVideo(path);
            }
        }*/
    }

    void OnVideoFinished()
    {
        /*currentVideoIndex += 1;
        if(currentVideoIndex < videosLocalPath.Count)
        {
           playVideoPanel.GetComponent<MainVideoPlayer>().PlayNewVideo(videosLocalPath[currentVideoIndex]);
        } else
        {
            rePlayVideoPanel.SetActive(true);
        }*/
    }
 

}
