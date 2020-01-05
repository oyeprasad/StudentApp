using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OTPInput : MonoBehaviour
{
    [SerializeField] private OTP otp;
    [SerializeField] private int index;
    InputField input;

    private void Start()
    {
        input = GetComponent<InputField>(); 
        input.onValueChange.AddListener(onValueChangeMethod);
    }
    void onValueChangeMethod(string value)
    {
        otp.OnValueChangeEvent.Invoke(index);
    }
}
