using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public enum VideoState
{
    Preparing,
    Playing,
    Paused,
    Finished
}
public class MainVideoPlayer : MonoBehaviour
{
     [SerializeField] private VideoPlayer m_VideoPlayer;
     [SerializeField] private Sprite playSprite;
     [SerializeField] private Sprite pauseSprite;
     [SerializeField] private Button PlayPuaseBtn;
    [SerializeField] private Button fullscreenButtonBtn;
    [SerializeField] private Button minimiseButton;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject coverScreen;
    [SerializeField] private Text currentSeekTime;
    [SerializeField] private Text totalDuration;
    [SerializeField] private Slider seekSlider;
    private bool _isPlaying = false;
    private VideoState _videoState = VideoState.Preparing;
    private bool isSliding = false;
    void Start()
    {
        m_VideoPlayer.prepareCompleted += PrepareCompleted;
        m_VideoPlayer.seekCompleted += SeekCompleted;
        m_VideoPlayer.loopPointReached += VideoFinished;
 
        PlayPuaseBtn.onClick.AddListener(OnPlayPuaseClicked);
    }
     public void PlayNewVideo(string url)
     {
         coverScreen.SetActive(true);
         m_VideoPlayer.url = url;
         m_VideoPlayer.Prepare();
         _videoState = VideoState.Preparing;
         loadingScreen.SetActive(true);
     }
     void OnPlayPuaseClicked()
     {
         switch(_videoState)
         {
             case VideoState.Preparing:
             break;
             case VideoState.Playing:
                m_VideoPlayer.Pause();
                _videoState = VideoState.Paused;
                PlayPuaseBtn.GetComponent<Image>().sprite = playSprite;
                break;
             case VideoState.Paused:
                m_VideoPlayer.Play();
                _videoState = VideoState.Playing;
                PlayPuaseBtn.GetComponent<Image>().sprite = pauseSprite;
                break;
             case VideoState.Finished:
             break;
         }
     }

     void PrepareCompleted(VideoPlayer player)
     {
         coverScreen.SetActive(false);
         loadingScreen.SetActive(false);
        m_VideoPlayer.Play(); 
        _videoState = VideoState.Playing;
     }
     
     void SeekCompleted(VideoPlayer player)
     {
        loadingScreen.SetActive(false);  
        _videoState = VideoState.Playing;
     }
     void VideoFinished(VideoPlayer player)
     {
         coverScreen.SetActive(true);
         VideoPanelController.EventVideoFinish.Invoke();
     }
     public void OnPointerDownSlider()
     {
         isSliding = true;
     }
     public void OnPointerUpSlider()
     {  
         float frame = (float) seekSlider.value * (float)m_VideoPlayer.frameCount;

         m_VideoPlayer.frame = (long) frame;
         loadingScreen.SetActive(true);
         _videoState = VideoState.Preparing;
         isSliding = false;
     }
     private void Update()
     {
        if(!isSliding && _videoState == VideoState.Playing)
        {
            seekSlider.value = (float)m_VideoPlayer.frame/(float)m_VideoPlayer.frameCount; 
            SetVideoTime();
        }
     }
     
     void SetVideoTime()
     {
        currentSeekTime.text = string.Format("{0}:{1}", Mathf.FloorToInt((float)m_VideoPlayer.time/60).ToString("00"), Mathf.FloorToInt((float)m_VideoPlayer.time%60).ToString("00"));
        totalDuration.text = string.Format("{0}:{1}", Mathf.FloorToInt((float)m_VideoPlayer.length/60).ToString("00"), Mathf.FloorToInt((float)m_VideoPlayer.length%60).ToString("00"));
     }

}
