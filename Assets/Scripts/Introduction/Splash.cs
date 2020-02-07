using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Splash : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField] UnityEngine.Video.VideoPlayer _player; 
    [SerializeField] private GameObject popup;
    void Awake()
    {    
        _player.loopPointReached += VideoPlayerLoopReached;

    }
    void Start()
    {
        LoadPlayerData();
        GSC.Instance.DownloadUSerprofilePic();
    }

    void VideoPlayerLoopReached(UnityEngine.Video.VideoPlayer vp)
    {
        ProceedAfterVideoFinished();
    }
    void ProceedAfterVideoFinished()
    {
        if(IsNetworkConnected())
        {
            print("Tutorial "+PlayerPrefs.GetString(Globals.PLAYERKEY_TUTORIALSTATUS));
            if(PlayerPrefs.GetString(Globals.PLAYERKEY_TUTORIALSTATUS,"NODATA") == "NODATA")
            {
                print("Tutorila not shown before");
                PlayerPrefs.SetString(Globals.PLAYERKEY_TUTORIALSTATUS, "SHOWN");
                Globals.LoadLevel(Globals.INTRO_SCENE);
            } 
            else if(PlayerPrefs.GetInt(Globals.PLAYERKEY_LOGINSTATUS) == Globals.LOGGED_IN)
            {
                print("User already");
                Globals.LoadLevel(Globals.HOME_SCENE);
            }  
            else
            {
                print("User not logged in");
                Globals.LoadLevel(Globals.LOGIN_SCENE);
            }


        } else
        {
            popup.SetActive(true);
        }
    }
    
    void LoadPlayerData()
    {
         if(PlayerPrefs.GetInt(Globals.PLAYERKEY_LOGINSTATUS) == Globals.LOGGED_IN)
            {
                Globals.LoadUserData();
            }  
    }
    private bool IsNetworkConnected()
    {
        return !(Application.internetReachability == NetworkReachability.NotReachable);
    }
    
    public void OnOkClicked()
    {
        print("On Ok CLicked");
        Application.Quit();
    }
}
