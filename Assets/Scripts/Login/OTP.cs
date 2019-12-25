using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OTP : MonoBehaviour
{
    [SerializeField] private InputField otpInput;
    [SerializeField] private Text screenMessage; 

    private void Start()
    { 

    }

    public void Submit()
    {
        if (otpInput.GetComponent<ValidateInput>().isValidInput)
        {

        }
    }

}
