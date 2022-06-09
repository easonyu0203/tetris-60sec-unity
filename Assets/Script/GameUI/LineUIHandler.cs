using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LineUIHandler : MonoBehaviour
{
    TextMeshProUGUI textMeshPro;

    /* Event Listener */
    void OnLineCntChange(int lineCnt)
    {
        ChangeLineCnt(lineCnt);
    }

    void ChangeLineCnt(int lineCnt)
    {
        string s = $"Lines\n{lineCnt}";
        textMeshPro.text = s;
    }

    void HookEvent()
    {
        ScoreLineManager.Instance.LineCntChangeEvent.AddListener(OnLineCntChange);
    }


    void Start()
    {
        HookEvent();

        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

}
