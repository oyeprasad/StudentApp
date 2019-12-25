using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    [SerializeField] private InputField firstNameInput, lastNameInput, phoneInput, emailInput, gradeInput, passwordInput, confirmPasswordInput;
    [SerializeField] private Dropdown countryCodeDropDown;
    [SerializeField] private Text screenMessage;

    private void Start()
    {
        LoginMenu.InputFieldEditStart.AddListener(OnEditStart);
    }

    private void Update()
    {
        if (gameObject.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape))
        {
            BackClicked();
        }
    }

    public void BackClicked()
    {
        LoginMenu.BackFromPanelEvent.Invoke("Login");
    }


    public void SubmitClicked()
    {
        if (ValidateInput())
        {
            RegisterUser();
        }
    }

    bool ValidateInput()
    {
        if (string.IsNullOrEmpty(firstNameInput.text) || !firstNameInput.GetComponent<ValidateInput>().isValidInput)
        {
            firstNameInput.GetComponent<ValidateInput>().Validate(firstNameInput.text);
            return false;
        } else if (string.IsNullOrEmpty(lastNameInput.text) || !lastNameInput.GetComponent<ValidateInput>().isValidInput)
        {
            lastNameInput.GetComponent<ValidateInput>().Validate(lastNameInput.text);
            return false;
        }
        else if (string.IsNullOrEmpty(phoneInput.text) || !phoneInput.GetComponent<ValidateInput>().isValidInput)
        {
            phoneInput.GetComponent<ValidateInput>().Validate(phoneInput.text);
            return false;
        }
        else if (string.IsNullOrEmpty(emailInput.text) || !emailInput.GetComponent<ValidateInput>().isValidInput)
        {
            emailInput.GetComponent<ValidateInput>().Validate(emailInput.text);
            return false;
        }
        else if (string.IsNullOrEmpty(gradeInput.text) || !gradeInput.GetComponent<ValidateInput>().isValidInput)
        {
            gradeInput.GetComponent<ValidateInput>().Validate(gradeInput.text);
            return false;
        }
        else if (string.IsNullOrEmpty(passwordInput.text) || !passwordInput.GetComponent<ValidateInput>().isValidInput)
        {
            passwordInput.GetComponent<ValidateInput>().Validate(passwordInput.text);
            return false;
        }
        else if (string.IsNullOrEmpty(confirmPasswordInput.text) || !confirmPasswordInput.GetComponent<ValidateInput>().isValidInput)
        {
            confirmPasswordInput.GetComponent<ValidateInput>().Validate(confirmPasswordInput.text);
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
        print("Register user");
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

    void OnEditStart()
    {
        screenMessage.color = Color.green;
        screenMessage.text = string.Empty;
    }


}
