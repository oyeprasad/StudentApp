using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using Facebook.Unity;
using System.Text.RegularExpressions;
using System;
using UnityEngine.UI;

public class FBManager : MonoBehaviour
{
    private FBUserData userfbdata = new FBUserData(); 


    #region Monobehaviour
    void Awake()
    {
        if (!FB.IsInitialized)
        {
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            FB.ActivateApp();
        }
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    #endregion Monobehaviour


    #region FBLogin 

    System.Action<FBUserData> AfterLoginCallback;
    public void FBLogin()
    {
        List<string> permissions = new List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(permissions, FBLoginCallback);
    }
    private AccessToken token;
    private void FBLoginCallback(ILoginResult result)
    {
        if (result.Cancelled)
        {
            print("User cancled login");
        }
        else
        {

            if (FB.IsLoggedIn)
            {
                token = AccessToken.CurrentAccessToken;
                Login.FBLoginEvent.Invoke(token.TokenString);
                // FB.API("/me?fields=first_name,last_name,email", HttpMethod.GET, FbDataCallback);
                //   FB.API("/me?fields=first_name,last_name,email", HttpMethod.GET, FbDataCallback, new Dictionary<string, string>() { });
            }

        }
    }

    private void FbDataCallback(IGraphResult result)
    {
        print("result.RawResult " + result.RawResult);
        FBUserData userfbdata = JsonUtility.FromJson<FBUserData>(result.RawResult);
        userfbdata.email = Regex.Replace(userfbdata.email, @"\\u([\dA-Fa-f]{4})", v => ((char)Convert.ToInt32(v.Groups[1].Value, 16)).ToString());
        FB.API("me/picture?type=square&height=128&width=128", HttpMethod.GET, GetProfilePicture);
    }
    private void GetProfilePicture(IGraphResult result)
    {

        Texture2D texture2D = result.Texture;
         
        if (result.Error == null && result.Texture != null)
        {
            userfbdata.profilepic = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), Vector2.zero); 

            AfterLoginCallback(userfbdata);
       
        }
    }

    #endregion FBLogin
}
