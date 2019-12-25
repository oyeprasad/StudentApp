using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{

    [SerializeField] private InputField usernameInput, passwordInput;

    public void LoginClicked()
    {
        if (string.IsNullOrEmpty(usernameInput.text) || !usernameInput.GetComponent<ValidateInput>().isValidInput)
        {
            usernameInput.GetComponent<ValidateInput>().Validate(usernameInput.text);
        }
        else if (string.IsNullOrEmpty(passwordInput.text) || !passwordInput.GetComponent<ValidateInput>().isValidInput)
        {
            passwordInput.GetComponent<ValidateInput>().Validate(passwordInput.text);
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
        LoginMenu.ForgotPasswordButtonClickEvent.Invoke();
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
