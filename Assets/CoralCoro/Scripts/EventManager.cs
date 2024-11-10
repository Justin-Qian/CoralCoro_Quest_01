using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static Action<int> OnTouchBubble;
    public static void TriggerOnTouchBubble(int number) {
        // 触发事件时传递参数
        OnTouchBubble?.Invoke(number);
    }
}
