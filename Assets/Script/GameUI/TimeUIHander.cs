using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeUIHander : MonoBehaviour
{
    TextMeshProUGUI textMeshPro;


    /* Event Listener */
    void OnTimeChange(int timeLeft)
    {
        ChangeTime(timeLeft);
    }

    void HookEvent()
    {
        PlayManager.Instance.TimeChangeEvent.AddListener(OnTimeChange);
    }

    void Start()
    {
        HookEvent();

        textMeshPro = GetComponent<TextMeshProUGUI>();
        
    }

    void ChangeTime(int t)
    {
        string s = $"Time\n{t}";
        textMeshPro.text = s;
    }
}
