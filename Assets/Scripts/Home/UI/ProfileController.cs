using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

[System.Serializable] public class ProfileData
{
    public int user_id = 0;
    public string name = "";
    public string phone = "";
    public string email = "";
    public string grade = "";
    public int status = 0;
    public int user_type_id = 0;
    public string profile_pic = "";
     
}

[System.Serializable] public class ProfileResponse : ResponseBase
{
    public ProfileData data = new ProfileData();
}

public class ProfileController : MonoBehaviour
{
     [SerializeField] private GameObject ProfilePanel;
     [SerializeField] private GameObject EditProfilePanel;

    [SerializeField] private Image profilePic, editProfilePic;
    [SerializeField] private InputField username, fullname, email, phone;
    [SerializeField] private Dropdown phoencode, gradeCode;

    [SerializeField] private InputField editUsername, editFullname, editEmail, editPhone;
    [SerializeField] private Dropdown editPhoencode, editGradeCode;
    [SerializeField] private Text gradeText, editGradeText; 
    [SerializeField] private List<string> phoneCodeList = new List<string>();
     
    private Sprite profilePicSprite;  
    void OnEnable()
    {
        editProfilePic.sprite = profilePic.sprite;

    }
    void Start()
    {
        HomeMainUIController.EventProfilePicChoose.AddListener(OnProfilePicChoosen);
        profilePicSprite = profilePic.sprite;
    }
    void OnProfilePicChoosen(Sprite img, float _aspectRatio)
    {
        print("OnProfilePicChoosen");

        profilePic.sprite = img;
        profilePic.GetComponent<AspectRatioFitter>().aspectRatio = _aspectRatio;
        editProfilePic.sprite = img;
        editProfilePic.GetComponent<AspectRatioFitter>().aspectRatio = _aspectRatio;
    }

    public void PopulatePanel()
    {
       string[] PhoneNumberArray = Globals.UserLoginDetails.phone.Split(' ');
 
       username.text = editUsername.text = Globals.UserLoginDetails.username.ToUpper(); 
       fullname.text = editFullname.text = Globals.UserLoginDetails.name.ToUpper();
       email.text = editEmail.text = Globals.UserLoginDetails.email;
       gradeText.text = editGradeText.text = Globals.UserLoginDetails.grade;
       phone.text = editPhone.text = PhoneNumberArray[1]; 
         
       editPhoencode.value = phoencode.value = phoneCodeList.IndexOf(PhoneNumberArray[0]); 
    }

     public void EditButtonClicked()
     { 
         HomeMainUIController.EventMyProfileEditClicked.Invoke();
     }

     public void ChoosePhotoClicked()
     {
         NativeGallery.GetImageFromGallery(OnImageChoose);
     }

      
     void OnImageChoose(string imagePath)
     { 
         print("OnImageChoose "+imagePath);
         if(!string.IsNullOrEmpty(imagePath))
         {
            Texture2D userpicTexture = NativeGallery.LoadImageAtPath(imagePath, 512, false, true);   

            editProfilePic.gameObject.GetComponent<AspectRatioFitter>().aspectRatio = (userpicTexture.width * 1.0f)/(userpicTexture.height * 1.0f); 
            editProfilePic.sprite = Sprite.Create(userpicTexture, new Rect(0.0f, 0.0f, userpicTexture.width, userpicTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            print("User pic set "+(userpicTexture.width * 1.0f)/(userpicTexture.height * 1.0f)); 
            profilePicSprite = editProfilePic.sprite; 
         }


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

        WWWForm form = new WWWForm();
        form.AddField("name",editFullname.text);
        form.AddField("phone",(editPhoencode.options[editPhoencode.value]+" "+editPhone.text));
        form.AddField("email",editEmail.text);
        form.AddField("profile_pic",  GetCurrentImageByte(editProfilePic.sprite.texture));

        string url = Globals.BASE_URL + WebRequests.Instance.editProfileEndPoint;

        using (UnityWebRequest www =  UnityWebRequest.Post(url, form))
        { 
            www.method = "PUT";
            www.SetRequestHeader("Accept", "application/json");//
            www.SetRequestHeader("Authorization", "Bearer "+Globals.UserLoginDetails.access_token);

            yield return www.SendWebRequest();
            while (!www.isDone)
                yield return null;
            

            if (www.isNetworkError || www.isHttpError)
            {
                print(www.error);
                OnProfileSave(null);
                HomeMainUIController.EventShowHideLoader.Invoke(false);     
            }
            else
            {
                print(www.downloadHandler.text);
                ProfileResponse _response = JsonUtility.FromJson<ProfileResponse>(www.downloadHandler.text);
                HomeMainUIController.EventShowHideLoader.Invoke(false);
               OnProfileSave(_response);
            }
        }  

    }

    string GetCurrentImageByte(Texture2D img)
    {    
        byte[] myTextureBytes   = img.EncodeToPNG();
        String myTextureBytesEncodedAsBase64 = System.Convert.ToBase64String(myTextureBytes);
        
        return myTextureBytesEncodedAsBase64;
    }
    void OnProfileSave(ProfileResponse response)
    {
        if(response != null)
        {
            if(response.status)
            {
                // Set Profile pic from here as it is already saved to database
                HomeMainUIController.EventProfilePicChoose.Invoke(profilePicSprite, (profilePicSprite.texture.width * 1.0f)/(profilePicSprite.texture.height * 1.0f));
                StartCoroutine(DownloadProfilePic(response));   
            } 
            else
            {
                
                HomeMainUIController.EventShowHideLoader.Invoke(false); 
                HomeMainUIController.ShowPopup.Invoke(response.message, () => print("Error in response from edit profile submit"));
            }
        } 
        else
        {
            HomeMainUIController.ShowPopup.Invoke("Opps! Some error occurs", () => print("Error in response from edit profile submit"));
        }

    }   

    IEnumerator DownloadProfilePic(ProfileResponse response)
    {
        HomeMainUIController.EventShowHideLoader.Invoke(false);
        HomeMainUIController.ShowPopup.Invoke(response.message, () => print("Response from edit profile submit"));
        HomeMainUIController.EventBackClicked.Invoke();

        print("Url : "+response.data.profile_pic);
         UnityWebRequest www = UnityWebRequestTexture.GetTexture(response.data.profile_pic);   
        yield return www.SendWebRequest();
        while(!www.isDone)
        yield return null;

        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
                
            HomeMainUIController.EventShowHideLoader.Invoke(false); 
            Texture2D myTexture = (Texture2D)((DownloadHandlerTexture)www.downloadHandler).texture;
            Sprite sprite =  Sprite.Create(myTexture, new Rect(0.0f, 0.0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);

            HomeMainUIController.EventProfilePicChoose.Invoke(sprite, (myTexture.width * 1.0f)/(myTexture.height * 1.0f));
             
        }

        
         
        


    }

}
