using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UserDefineEvents : MonoBehaviour
{

} 
public class StringEvent : UnityEvent<string>
{

}
public class IntEvent : UnityEvent<int>
{

} 
public class BooleanEvent : UnityEvent<bool>
{

}
public class IntStringEvent : UnityEvent<int, string>
{

} 
public class StringActionEvent : UnityEvent<string, System.Action>
{
    
}