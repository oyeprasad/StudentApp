using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

[Serializable]
public class ResponseBase
{
    public bool status = false;
    public string message = string.Empty;
    public int code = 0;
}

[Serializable]
public class ResponseData<T> : ResponseBase
{
   public T data = default;
}

[Serializable]
public class FBUserData
{
    public string first_name = string.Empty;
    public string last_name = string.Empty;
    public string email = string.Empty;
    public string id = string.Empty;
    public Sprite profilepic = default;
}

[Serializable]
public class FBLoginResponseData
{
    public int id;
    public string name = string.Empty;
    public string phone = string.Empty;
    public string email = string.Empty;
    public int grade = 0;
    public string email_verified_at = string.Empty;
    public int status = 0;
    public string otp = string.Empty;
    public int otp_verified = 0;
    public int user_type_id = 3;
    public string facebook_id = string.Empty;
    public string password_otp = string.Empty;
    public string created_at = string.Empty;
    public string updated_at = string.Empty;
}

 
[Serializable]
public class UserData
{
    public int user_id = 0;
    public string name = string.Empty;
    public string phone = string.Empty;
    public string email = string.Empty;
    public string username = string.Empty;
    public string grade = string.Empty;
    public int otp_verified = 0;
    public int status = 0;
    public string user_type_id = string.Empty;
    public string access_token = string.Empty;
    public string profile_pic = string.Empty;
}


[Serializable]
public class UserDetail
{
}

public class Globals : MonoBehaviour
{
    public const string BASE_URL = "http://3.6.46.214/whizee/whizee-backend/api/";
    public const string LOGIN_SCENE = "Login";
    public const string HOME_SCENE = "Home";

    public static string USERNAME = "";

    public static int LoginType = 0; // 0 for normal login, 1 for fb login, 2 for ggogle login

    public static FBLoginResponseData fBLoginResponseData = new FBLoginResponseData();
    public static UserData UserLoginDetails = new UserData();

    public static bool ValidateEmail(string emailString)
    {
        return Regex.IsMatch(emailString, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
    } 

    public static void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}


