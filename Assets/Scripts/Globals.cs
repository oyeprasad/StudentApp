using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

[Serializable]
public class CountryCodeMobileDigitPair
{
    public string countryCode = "";
    public string mobileNumber = "";
}
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
 
 public class WorkSheetData 
 {
     public int id = 0;
     public int sub_category_id = 0;
     public string title = "";
     public string worksheet_document = "";
 }

[Serializable]
 public class WorkSheetResponse : ResponseBase
 {
    public WorkSheetList data = new WorkSheetList();
 }
 [Serializable]
 public class WorkSheetList
 {
     public List<WorkSheetData> worksheets = new List<WorkSheetData>();
 }

[System.Serializable] public class PasswordIcon
{
    public int icon_id = 0;
    public string icon_image = "";
    public string code = "";
}
[System.Serializable] public class PasswordIconItem
{
    public int icon_id = 0;
    public Sprite icon_image = null;
    public string code = "";
}
[System.Serializable] public class PasswordIconList
{
    public List<PasswordIcon> password_icons = new List<PasswordIcon>();
}
[System.Serializable] public class PasswordIconResponse : ResponseBase
{
    public PasswordIconList data = new PasswordIconList();
}

public class Globals : MonoBehaviour
{
    public const string BASE_URL = "http://3.6.46.214/whizee/whizee-backend/api/";
    public const string LOGIN_SCENE = "Login";
    public const string HOME_SCENE = "Home"; 
    public const string INTRO_SCENE = "Introduction";

    public const string PLAYERKEY_TUTORIALSTATUS = "tutorial";
    
    public const string PLAYERKEY_LOGINSTATUS = "isloggedin";
    public const string PLAYERKEY_USERDATA = "userdata";
    public const int LOGGED_IN = 1;
    public const int LOGGED_OUT = 2;
    public static string USERNAME = "";

    public static int LoginType = 0; // 0 for normal login, 1 for fb login, 2 for ggogle login

    public static FBLoginResponseData fBLoginResponseData = new FBLoginResponseData();
    public static UserData UserLoginDetails = new UserData();
 
    public static List<PasswordIconItem> PasswordItems = new List<PasswordIconItem>();
    public static bool ValidateEmail(string emailString)
    {
        return Regex.IsMatch(emailString, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
    } 


    public static void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public static void SaveUserData(UserData userdata)
    {
        print("Save user data");
        PlayerPrefs.SetInt(Globals.PLAYERKEY_LOGINSTATUS, Globals.LOGGED_IN);
        print("Save user details : \n"+JsonUtility.ToJson(userdata));
        PlayerPrefs.SetString(Globals.PLAYERKEY_USERDATA, JsonUtility.ToJson(userdata));
    }
    public static void LoadUserData()
    {
        print("Loaduserdata \n"+PlayerPrefs.GetString(Globals.PLAYERKEY_USERDATA));
        UserLoginDetails = JsonUtility.FromJson<UserData>(PlayerPrefs.GetString(Globals.PLAYERKEY_USERDATA));
    }
}


