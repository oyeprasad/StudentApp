using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Demo : MonoBehaviour
{
    [SerializeField] private string youtubeurl = "";

    void Start()
    {
        //YoutubePlayer.YoutubePlayer.Instance.GetVideoResolvedUrl(youtubeurl, Callback);
    }
 
    void Callback(string resolvedUrl)
    {
        print("Resolved url is \n"+resolvedUrl);
    }
}
