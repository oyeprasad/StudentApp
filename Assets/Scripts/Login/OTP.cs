using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; 
using TMPro;

public class OTP : MonoBehaviour
{
    public IntEvent OnValueChangeEvent = new IntEvent();
    [SerializeField] private List<InputField> allInpufields;
    [SerializeField] private TMP_InputField tmp_InputField;
    [SerializeField] private Text message; 

    Coroutine routine = null;
    void OnEnable()
    {
        for(int i = 0; i < allInpufields.Count; i++)
        {
            allInpufields[i].text = string.Empty;
        }   
        tmp_InputField.text = string.Empty;
    }

    private void Start()
    {
       OnValueChangeEvent.AddListener(OnValueChange);
    }
    void OnValueChange(int index)
    {
        print("On value change "+index);
        print("All input field " +allInpufields.Count);
        if(routine != null)
            StopCoroutine(routine);
        routine = StartCoroutine(SetInputFocus(index));
        message.text = string.Empty; 
    }

    IEnumerator SetInputFocus(int _index)
    {
        yield return new WaitForEndOfFrame();
         if ((_index + 1) < allInpufields.Count)
        { 
            //EventSystem.current.SetSelectedGameObject(allInpufields[_index + 1].gameObject, null);
 
            //allInpufields[_index + 1].ActivateInputField();
            allInpufields[_index + 1].Select(); 
            print("Select input doen");
        }
        else
        {
            print("Into else");
        }

    }
    public void OnSubmit()
    {
        string otpInput = string.Empty;
        bool isValid = true;
        /*for (int i = 0; i < allInpufields.Count; i++)
        {
            if (string.IsNullOrEmpty(allInpufields[i].text))
            {
                message.color = Color.red;
                message.text = "OTP is invalid";
                isValid = false;
                break;
            }
            otpInput += allInpufields[i].text;
        }*/
        otpInput = tmp_InputField.text;
        if (isValid)
        { 
            message.text = string.Empty;
            Login.OTPSubmitEvent.Invoke(otpInput);
        }
         
    }

    public void Resend()
    {
        message.color = Color.green;
        message.text = "Code is sent successfully";
    }
     
}
