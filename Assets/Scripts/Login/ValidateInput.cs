using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValidateInput : MonoBehaviour
{
    [HideInInspector] public bool isValidInput = false;

    [SerializeField] private string errorMessage;
    [SerializeField] private string blankErrorMessage;

    private InputField inputToValidate;
    [SerializeField] private bool isEmail;
    [SerializeField] private bool lengthRangeCheck = false;
    [SerializeField] private int minLength, maxLength;

    [SerializeField] Text validationInfo;
    private void Start()
    {
        validationInfo.text = string.Empty;
        inputToValidate = GetComponent<InputField>();
        inputToValidate.onValueChanged.AddListener(OnValueChange);
        inputToValidate.onEndEdit.AddListener(Validate);
        LoginMenu.InputFieldEditStart.AddListener(OnEditStart);
    }

    private void OnValueChange(string arg0)
    {
        LoginMenu.InputFieldEditStart.Invoke();
        validationInfo.text = string.Empty;
    }

    void OnEditStart()
    {
        validationInfo.text = string.Empty;
    }

    public void Validate(string arg0)
    {
        if (arg0.Length <= 0)
        {
            validationInfo.text = blankErrorMessage;
            isValidInput = false;
        }
        else if (isEmail && !Globals.ValidateEmail(arg0))
        {
            validationInfo.text = errorMessage;
            isValidInput = false;
        }
        else if (lengthRangeCheck && (arg0.Length < minLength || arg0.Length > maxLength))
        {
            validationInfo.text = errorMessage;
            isValidInput = false;
        }
        else
        {
            isValidInput = true;
        }
            
    }
}
