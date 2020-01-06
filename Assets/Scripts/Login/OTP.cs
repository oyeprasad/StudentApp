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



    private void Start()
    {
        OnValueChangeEvent.AddListener(OnValueChange);
    }
    void OnValueChange(int index)
    {
        message.text = string.Empty;
        if ((index + 1) < allInpufields.Count)
        {
            EventSystem.current.SetSelectedGameObject(allInpufields[index + 1].gameObject, null);
        }
        else
        {
            // Do nothing
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
            print("Username is assigned");
            message.text = string.Empty;
            Login.OTPSubmitEvent.Invoke("Amit");
        }
         
    }

    public void Resend()
    {
        message.color = Color.green;
        message.text = "Code is sent successfully";
    }
     
}
