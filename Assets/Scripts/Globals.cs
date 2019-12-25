using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System;

[Serializable]
public class UserRegisterData
{
    public string FirstName;
    public string LastName;
    public string CountryCode;
    public string PhoneNumber;
    public string Email;
    public string Password;
    public string ConfirmPassword;
}

public class Globals : MonoBehaviour
{
    public const string BASE_URL = "mybaseurl";

    public static bool ValidateEmail(string emailString)
    {
        return Regex.IsMatch(emailString, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
    }
}
