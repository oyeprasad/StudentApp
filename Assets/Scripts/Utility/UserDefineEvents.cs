using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UserDefineEvents : MonoBehaviour
{

}
public class UserLoginEvent : UnityEvent<UserLoginData>
{

}
public class StringEvent : UnityEvent<string>
{

}
public class IntEvent : UnityEvent<int>
{

}
public class UserRegisterEvent : UnityEvent<UserRegisterData>
{

}
