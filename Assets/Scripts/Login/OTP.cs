using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
 
public class OTP : MonoBehaviour
{
    public IntEvent OnValueChangeEvent = new IntEvent();
    [SerializeField] private List<InputField> allInpufields;
    [SerializeField] private Text message;


    Coroutine routine = null;
    private void Start()
    {
        OnValueChangeEvent.AddListener(OnValueChange);
    }
    void OnValueChange(int index)
    {
        print("On value change "+index);
        print("All input field " +allInpufields.Count);
        //if(routine != null)
       // StopCoroutine(routine);
        //routine = StartCoroutine(SetInputFocus(index));
        //message.text = string.Empty;
       
    }

    IEnumerator SetInputFocus(int _index)
    {
        yield return new WaitForEndOfFrame();
         if ((_index + 1) < allInpufields.Count)
        {
            print("In the if statement");

            print(allInpufields[_index + 1]);
            print("Now check this");
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
        for (int i = 0; i < allInpufields.Count; i++)
        {
            if (string.IsNullOrEmpty(allInpufields[i].text))
            {
                message.color = Color.red;
                message.text = "OTP is invalid";
                isValid = false;
                break;
            }
            otpInput += allInpufields[i].text;
        }

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
