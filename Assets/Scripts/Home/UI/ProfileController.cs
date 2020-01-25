﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;

public class ProfileController : MonoBehaviour
{
     [SerializeField] private GameObject ProfilePanel;
     [SerializeField] private GameObject EditProfilePanel;

    [SerializeField] private InputField username, fullname, email, phone;
    [SerializeField] private Dropdown phoencode, gradeCode;

    [SerializeField] private InputField editUsername, editFullname, editEmail, editPhone;
    [SerializeField] private Dropdown editPhoencode, editGradeCode;

    [SerializeField] private List<string> phoneCodeList = new List<string>();


    public void PopulatePanel()
    {
       string[] PhoneNumberArray = Globals.UserLoginDetails.phone.Split(' ');
 
       username.text = editUsername.text = Globals.UserLoginDetails.username.ToUpper(); 
       fullname.text = editFullname.text = Globals.UserLoginDetails.name.ToUpper();
       email.text = editEmail.text = Globals.UserLoginDetails.email;
       phone.text = editPhone.text = PhoneNumberArray[1]; 

       editPhoencode.value = phoencode.value = phoneCodeList.IndexOf(PhoneNumberArray[0]); 
    }

     public void EditButtonClicked()
     { 
         HomeMainUIController.EventMyProfileEditClicked.Invoke();
     }
     public void SaveButtonClicked()
     {
         if(!editUsername.gameObject.GetComponent<ValidateInput>().isValidInput)
         {
             editUsername.gameObject.GetComponent<ValidateInput>().Validate(editUsername.text);

         } 
          if(!editFullname.gameObject.GetComponent<ValidateInput>().isValidInput)
         {
             editFullname.gameObject.GetComponent<ValidateInput>().Validate(editFullname.text);

         }
         if(!editEmail.gameObject.GetComponent<ValidateInput>().isValidInput)
         {
             editEmail.gameObject.GetComponent<ValidateInput>().Validate(editEmail.text);
         }
          if(!editPhone.gameObject.GetComponent<ValidateInput>().isValidInput)
         {
             editPhone.gameObject.GetComponent<ValidateInput>().Validate(editPhone.text);
         } 
         if(editUsername.gameObject.GetComponent<ValidateInput>().isValidInput && 
            editFullname.gameObject.GetComponent<ValidateInput>().isValidInput &&
            editEmail.gameObject.GetComponent<ValidateInput>().isValidInput &&
            editPhone.gameObject.GetComponent<ValidateInput>().isValidInput)
         {
            StartCoroutine(SubmitEditProfile());
         }
     }

     
    IEnumerator SubmitEditProfile()
    {
        print("Change user profile info...");
        HomeMainUIController.EventShowHideLoader.Invoke(true);
        yield return new WaitForSeconds(2);
        HomeMainUIController.EventShowHideLoader.Invoke(false);
        HomeMainUIController.ShowPopup.Invoke("Some error in update, Please try again later.",
                                                     () =>{ 
                                                         HomeMainUIController.EventMyProfileSaveClicked.Invoke();
                                                          });
    }   

}
