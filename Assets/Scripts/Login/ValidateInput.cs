﻿using System;
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

    [SerializeField] public Text validationInfo;
    private void Start()
    {
        inputToValidate = GetComponent<InputField>();
        inputToValidate.onValueChanged.AddListener(OnValueChange);
        inputToValidate.onEndEdit.AddListener(Validate);
       // LoginMenu.InputFieldEditStart.AddListener(OnEditStart);
    }

    private void OnEnable()
    {
        validationInfo.text = string.Empty;
    }

    bool wasfocus = false;
    bool  keepOldTextInField = false;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && gameObject.activeInHierarchy)
        {
            if(TouchScreenKeyboard.visible == false && wasfocus)
            {
                wasfocus = false;
                KeyBoardHide();
            }
        }
    }

    void KeyBoardHide()
    {
         keepOldTextInField = true;
    }
    string oldEditText;
    string editText;
    private void OnValueChange(string arg0)
    {

        wasfocus = true;
        //LoginMenu.InputFieldEditStart.Invoke();
        validationInfo.text = string.Empty;

        //--- new logic
        oldEditText = editText;
        editText = arg0;
    }

    void OnEditStart()
    {
        validationInfo.text = string.Empty;
    }

    public void Validate(string arg0)
    {

        // new logic
        if (keepOldTextInField && !string.IsNullOrEmpty(oldEditText))
        {
            print("Keep old "+oldEditText);
        //IMPORTANT ORDER
            editText = oldEditText;
            inputToValidate.text = oldEditText;
            arg0 = oldEditText;
            keepOldTextInField = false;
        }
        //===========

        print("Validate");
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
