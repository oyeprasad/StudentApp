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
    [SerializeField] private bool isMobileNumber;
    [SerializeField] private bool lengthRangeCheck = false;
    [SerializeField] private int minLength, maxLength;

    [SerializeField] public Text validationInfo;

    private TouchScreenKeyboard keyboard;

    private void Start()
    {
        inputToValidate = GetComponent<InputField>();
        inputToValidate.onValueChanged.AddListener(OnValueChange);
        inputToValidate.onEndEdit.AddListener(Validate);
       // inputToValidate.onEndEdit.AddListener(Validate); 
    } 
    private void OnEnable()
    {
        validationInfo.text = string.Empty;
    }

    bool backPressed = false;
    bool focusStart = false;
    bool IsCancelledKeyboard = false;
    bool IsLostFocusKeyboard = false;
    bool IsDoneKeyboard = false;
    bool IsVisibleKeyboard = false;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(TouchScreenKeyboard.visible == false)
            {
                backPressed = true; 
            } 
        }
       /* if(!focusStart && inputToValidate.isFocused)
        {
           focusStart = true;  
           keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);   

        }


        if(keyboard != null && keyboard.status == TouchScreenKeyboard.Status.Canceled && !IsCancelledKeyboard)
        {
            IsCancelledKeyboard = true;
            IsVisibleKeyboard = false;
            //EndEdit(oldEditText);
            print("CancelPressed");
        }
        if(keyboard != null && keyboard.status == TouchScreenKeyboard.Status.LostFocus && !IsLostFocusKeyboard)
        {
            IsLostFocusKeyboard = true;
            IsVisibleKeyboard = false;
            EndEdit(editText);
            print("Loast focus");
        }
        if(keyboard != null && keyboard.status == TouchScreenKeyboard.Status.Done && !IsDoneKeyboard)
        {
            IsDoneKeyboard = true;
            IsVisibleKeyboard = false;
            print("Done pressed");
            EndEdit(editText);
        }
        // new logic
        if(keyboard != null && keyboard.status == TouchScreenKeyboard.Status.Visible && !IsVisibleKeyboard)
        {
            IsVisibleKeyboard = true;
            IsCancelledKeyboard = false;
            IsLostFocusKeyboard = false;
            IsDoneKeyboard = false; 
        } */
    }

    private void EndEdit(string value)
    {
        inputToValidate.text = value;
        Validate(value);
    }
    
    string oldEditText = "";
    string editText = "";
    private void OnValueChange(string arg0)
    {  
        print("OnValueChange "+oldEditText);
      /*  if(IsCancelledKeyboard)
        {
            inputToValidate.text = oldEditText;
            Validate(oldEditText);
        } else
        {
            oldEditText = editText;
            editText = arg0;  
        } */
        validationInfo.text = string.Empty;
    }
 

    public void Validate(string arg0)
    {
        print("Validate "+arg0);
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
        else if (isMobileNumber && !GetComponent<MobileNumberValidater>().Validate())
        {
            //validationInfo.text = errorMessage;
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
