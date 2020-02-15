﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Thumbnail : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Image thumbnailImage;
    [SerializeField] private Sprite defaultThumbnailSprite;
    [SerializeField] private Text title;
    private string videoPath;
    private int videoId;
    private Button button;
    VideoData videoData;

    void Start()
    {
        button = this.GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }
    public void Populate(VideoData data, Sprite _thumbnail)
    {
        videoData = data;
        videoId = data.video_id;
        title.text = data.video_title;
        videoPath = data.video_path;
        if(_thumbnail == null)
        {
            _thumbnail = defaultThumbnailSprite;
        }
        thumbnailImage.sprite = _thumbnail;
    }
    void OnClick()
    {
        HomeMainUIController.EventThumbnailClicked.Invoke(videoData);
    }
 
}
