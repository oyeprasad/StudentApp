using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    [SerializeField] private InputField firstNameInput, lastNameInput, phoneInput, emailInput, gradeInput, passwordInput, confirmPasswordInput;
    [SerializeField] private Dropdown countryCodeDropDown;
    [SerializeField] private Text screenMessage;
    

    void SubmitClicked()
    {
        if (ValidateInput())
        {
            RegisterUser();
        }
    }

    bool ValidateInput()
    {
        if (!firstNameInput.GetComponent<ValidateInput>())
        {
            return false;
        } else if (!lastNameInput.GetComponent<ValidateInput>())
        {
            return false;
        }
        else if (!phoneInput.GetComponent<ValidateInput>())
        {
            return false;
        }
        else if (!emailInput.GetComponent<ValidateInput>())
        {
            return false;
        }
        else if (!gradeInput.GetComponent<ValidateInput>())
        {
            return false;
        }
        else if (!passwordInput.GetComponent<ValidateInput>())
        {
            return false;
        }
        else if (!confirmPasswordInput.GetComponent<ValidateInput>())
        {
            return false;
        }
        else if (!string.Equals(passwordInput.text, confirmPasswordInput.text))
        {
            screenMessage.color = Color.red;
            screenMessage.text = "Password and confirm password must be same.";
            return false;
        }
        screenMessage.color = Color.green;
        screenMessage.text = string.Empty;
        return true;
    }

    void RegisterUser()
    {
        UserRegisterData _userData = new UserRegisterData();

        _userData.FirstName = firstNameInput.text;
        _userData.LastName = lastNameInput.text;
        _userData.CountryCode = countryCodeDropDown.itemText.text;
        _userData.PhoneNumber = phoneInput.text;
        _userData.Email = emailInput.text;
        _userData.Password = passwordInput.text;
        _userData.ConfirmPassword = confirmPasswordInput.text;

        LoginMenu.userRegisterEvent.Invoke(_userData);
    }
}
