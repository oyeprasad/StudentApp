using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{

    [SerializeField] private InputField usernameInput, passwordInput;

    public void LoginClicked()
    {
        if (!usernameInput.GetComponent<ValidateInput>().isValidInput)
        {
            print("Username is not valid");
        }
        else if (!passwordInput.GetComponent<ValidateInput>().isValidInput)
        {
            print("Password is not valid");
        }
        else
        {
            UserLoginData userlogindata = new UserLoginData();
            userlogindata.Username = usernameInput.text;
            userlogindata.Password = passwordInput.text;
            LoginMenu.LoginButtonClickEvent.Invoke(userlogindata);
        }
    }
    public void ForgotPasswordClicked()
    {

    }
    public void RegiterClicked()
    {
        LoginMenu.RegisterButtonClickEvent.Invoke();
    }
    public void FbLoginClicked()
    {

    }
    public void GoogleLoginClicked()
    {

    }
}
