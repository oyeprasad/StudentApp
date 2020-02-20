using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class VideoDownloader : MonoBehaviour
{
    private System.Action<string> _callback;
    public void DownloadVideo(string sourcePath, string destinationPath, Action<string> callback)
    {
        _callback = callback;;
        StartCoroutine(DownloadVideo(sourcePath, destinationPath));
    }
    IEnumerator DownloadVideo(string sourcePath, string destinationPath)
    {
        yield return null;
    }
}
