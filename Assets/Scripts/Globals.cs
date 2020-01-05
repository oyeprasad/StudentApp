using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

[Serializable]
public class UserRegisterData
{
    public string FirstName = string.Empty;
    public string LastName = string.Empty;
    public string CountryCode = string.Empty;
    public string PhoneNumber = string.Empty;
    public string Email = string.Empty;
    public string Password = string.Empty;
    public string ConfirmPassword =string.Empty;
} //

[Serializable]
public class UserLoginData
{
    public string Username = string.Empty;
    public string Password = string.Empty; 
}

[Serializable]
public class RegisterResponseData
{

}

[Serializable]
public class LoginResponseData
{

}
[Serializable]
public class ForgotPasswordResponseData
{

}


[Serializable]
public class OTPResponseData
{

}
public class Globals : MonoBehaviour
{
    public const string BASE_URL = "mybaseurl";
    public const string LOGIN_SCENE = "Login";
    public const string HOME_SCENE = "Home";

    public static string USERNAME = "";

    public static bool ValidateEmail(string emailString)
    {
        return Regex.IsMatch(emailString, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
    } 

    public static void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}


