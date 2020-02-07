using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MobileNumberValidater : MonoBehaviour
{
    [SerializeField] private Dropdown countryCode;
    private InputField mobileInputField;
    private ValidateInput validator;
    [SerializeField] private Text validateText;


    void Awake()
    {
        mobileInputField = GetComponent<InputField>();
    }

   public bool Validate()
   {
       string reqDigit = GSC.Instance.countryCodeMobileDigitPairs.Find(s => s.countryCode == countryCode.options[countryCode.value].text).mobileNumber;
        if(! (int.Parse(reqDigit) == mobileInputField.text.Length))
       {
           validateText.text = "Phone number must be "+reqDigit+" characters in length.";
           return false;
       }
       return true;
   }



}
