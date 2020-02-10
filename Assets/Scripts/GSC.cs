using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GSC : MonoBehaviour
{
    
    public static GSC Instance;

    public List<CountryCodeMobileDigitPair> countryCodeMobileDigitPairs;
    public Sprite UserProfilePic;
    public float userpicAspectRatio = 1;

    void Awake(){
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else
        {
            Debug.Log("Instance already available");
            Destroy(this.gameObject);
        }
    }


    public void DownloadUSerprofilePic()
    {
        print("Download profile pic ");
        string url = Globals.UserLoginDetails.profile_pic;
        print("Profile pic url "+url);
        if(!string.IsNullOrEmpty(url))
            StartCoroutine(DownloadUserprofilePic(url));
    }
    IEnumerator DownloadUserprofilePic(string _url)
    {  print("Downloading unser image");
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(Globals.UserLoginDetails.profile_pic);   
        yield return www.SendWebRequest();
        while(!www.isDone)
        yield return null;

        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else { 
        print("Downloaded unser image");
            Texture2D myTexture = (Texture2D)((DownloadHandlerTexture)www.downloadHandler).texture;
            print("Image is " + myTexture);
            userpicAspectRatio = (myTexture.width * 1.0f)/(myTexture.height * 1.0f);
            UserProfilePic =  Sprite.Create(myTexture, new Rect(0.0f, 0.0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            HomeMainUIController.EventProfilePicChoose.Invoke(UserProfilePic, (myTexture.width * 1.0f)/(myTexture.height * 1.0f));
        }
    }
}
