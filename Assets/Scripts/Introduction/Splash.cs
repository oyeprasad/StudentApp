using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField] private GameObject popup;
    void Awake()
    {
        if(IsNetworkConnected())
        {
            if(PlayerPrefs.GetString(Globals.PLAYERKEY_TUTORIALSTATUS,"NODATA") == "NODATA")
            {
                PlayerPrefs.SetString(Globals.PLAYERKEY_TUTORIALSTATUS, "SHOWN");
                Globals.LoadLevel(Globals.INTRO_SCENE);
            } 
            if(PlayerPrefs.GetInt(Globals.PLAYERKEY_LOGINSTATUS) == Globals.LOGGED_IN)
            {
                Globals.LoadUserData();
                Globals.LoadLevel(Globals.HOME_SCENE);
            }  
            else
            {
                Globals.LoadLevel(Globals.LOGIN_SCENE);
            }


        } else
        {
            popup.SetActive(true);
        }
    }
    
    
    
    private bool IsNetworkConnected()
    {
        return !(Application.internetReachability == NetworkReachability.NotReachable);
    }
    
    public void OnOkClicked()
    {
        Application.Quit();
    }
}
