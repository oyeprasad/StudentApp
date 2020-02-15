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
     [SerializeField] private Sprite fullVideoSprite;
     [SerializeField] private Sprite minVideoSprite;
     [SerializeField] private Button PlayPuaseBtn;
    [SerializeField] private Button fullscreenButtonBtn;

    [SerializeField] private GameObject fullScrrenPanel;
    [SerializeField] private Button fullScrrenPlyPauseBtn;
    [SerializeField] private Button minimiseButton;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject outerLaoding;
    [SerializeField] private GameObject coverScreen;
    [SerializeField] private Text currentSeekTime;
    [SerializeField] private Text totalDuration;
    [SerializeField] private Text currentSeekTimeFS;
    [SerializeField] private Text totalDurationFS;
    [SerializeField] private Slider seekSlider;
    [SerializeField] private Slider fullScrrenSeekSlider;
    public bool _isPlaying = false;
    private VideoState _videoState = VideoState.Preparing;
    private bool isSliding = false; 


    void OnEnable()
    {
         loadingScreen.SetActive(true);
         outerLaoding.SetActive(true);
        if(_isPlaying )
        {
            m_VideoPlayer.Play();
            _isPlaying = false;
            print("Play video on enable as it was previously playing");
        }
    }
    void OnDisable()
    {
        if(m_VideoPlayer.isPlaying)
        {
            m_VideoPlayer.Pause();
        }
    }
    void Start()
    {
        m_VideoPlayer.prepareCompleted += PrepareCompleted;
        m_VideoPlayer.seekCompleted += SeekCompleted;
        m_VideoPlayer.loopPointReached += VideoFinished;
 
        PlayPuaseBtn.onClick.AddListener(OnPlayPuaseClicked);
        fullScrrenPlyPauseBtn.onClick.AddListener(OnPlayPuaseClicked);
        fullscreenButtonBtn.onClick.AddListener(FullScreenClicked);
        minimiseButton.onClick.AddListener(MinimiseClicked);
    }

    void FullScreenClicked()
    {
        HomeMainUIController.CanUseSystemBack = false;
        fullScrrenPanel.SetActive(true);
    }
    void MinimiseClicked()
    {
        HomeMainUIController.CanUseSystemBack = true;
        fullScrrenPanel.SetActive(false);
    }

     public void PlayNewVideo(string url)
     { 
          
         print("Play new video");
         coverScreen.SetActive(true);
         m_VideoPlayer.url = url;
         m_VideoPlayer.Prepare();
         _videoState = VideoState.Preparing;
         loadingScreen.SetActive(true);
     }

     public void Replay()
     {
         m_VideoPlayer.Play();
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
                fullScrrenPlyPauseBtn.GetComponent<Image>().sprite = playSprite;
                break;
             case VideoState.Paused:
                m_VideoPlayer.Play();
                _videoState = VideoState.Playing;
                PlayPuaseBtn.GetComponent<Image>().sprite = pauseSprite;
                fullScrrenPlyPauseBtn.GetComponent<Image>().sprite = pauseSprite;
                break;
             case VideoState.Finished:
             break;
         }
     }

     void PrepareCompleted(VideoPlayer player)
     {
         coverScreen.SetActive(false);
         loadingScreen.SetActive(false);
         outerLaoding.SetActive(false);
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
         _isPlaying = false;
         MinimiseClicked();
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
     public void FullScreenOnPointerDownSlider()
     {
         isSliding = true;
     }
     public void FullScreenOnPointerUpSlider()
     {
        float frame = (float) fullScrrenSeekSlider.value * (float)m_VideoPlayer.frameCount;

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
            fullScrrenSeekSlider.value = (float)m_VideoPlayer.frame/(float)m_VideoPlayer.frameCount; 
            SetVideoTime();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(fullScrrenPanel.activeInHierarchy)
            {
                MinimiseClicked();
            }
        }
     }
     
     void SetVideoTime()
     {
        currentSeekTimeFS.text = currentSeekTime.text = string.Format("{0}:{1}", Mathf.FloorToInt((float)m_VideoPlayer.time/60).ToString("00"), Mathf.FloorToInt((float)m_VideoPlayer.time%60).ToString("00"));
        totalDurationFS.text = totalDuration.text = string.Format("{0}:{1}", Mathf.FloorToInt((float)m_VideoPlayer.length/60).ToString("00"), Mathf.FloorToInt((float)m_VideoPlayer.length%60).ToString("00"));
     }

}
