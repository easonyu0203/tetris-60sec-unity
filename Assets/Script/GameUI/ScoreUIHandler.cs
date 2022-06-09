using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUIHandler : MonoBehaviour
{
    TextMeshProUGUI textMeshPro;

    /* Event Listener */
    void OnScoreChange(int score)
    {
        ChangeScore(score);
    }

    void ChangeScore(int score)
    {
        textMeshPro.text = $"Score\n{score}";
    }

    void HookEvent()
    {
        ScoreLineManager.Instance.ScoreChangeEvent.AddListener(OnScoreChange);
    }

    void Start()
    {
        HookEvent();
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }


}
