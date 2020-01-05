using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.Events;

public class LoginMenu : MonoBehaviour
{
    // Static Events 
    [SerializeField] private string registerEndPoint, regOTPEndPoint, forgotPasswordEndPoint, loginEndPoint;
     

    #region MonoBehaviour
    #endregion MonoBehaviour

     

    public void SuccessfullRegistered()
    {
        Globals.LoadLevel(Globals.HOME_SCENE);
    }

    #region Login 
    #endregion Login

    
}
