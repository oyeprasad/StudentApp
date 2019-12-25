using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForgotPassword : MonoBehaviour
{
    [SerializeField] private InputField emailInput;
    [SerializeField] private Text scrrenMessage;


    public void OnSubmit()
    {
        if (string.IsNullOrEmpty(emailInput.text) || !emailInput.GetComponent<ValidateInput>().isValidInput)
        {
            emailInput.GetComponent<ValidateInput>().Validate(emailInput.text);
        }
        else
        {
            LoginMenu.ForgotPasswordSubmit.Invoke(emailInput.text);
        }
    }

}
 