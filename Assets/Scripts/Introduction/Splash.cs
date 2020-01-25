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
                Globals.LoadUserData();
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
    
    
    
    private bool IsNetworkConnected()
    {
        return !(Application.internetReachability == NetworkReachability.NotReachable);
    }
    
    public void OnOkClicked()
    {
        Application.Quit();
    }
}
