using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverPageController : Singleton<GameOverPageController>
{

    [SerializeField] TextMeshProUGUI _score, _line;

    /* Event Listener */
    void OnGameOver()
    {
        _score.text = PlayState.Instance.Score.ToString();
        _line.text = PlayState.Instance.Lines.ToString();

        gameObject.SetActive(true);
    }

    public void OnStartPlay()
    {
        gameObject.SetActive(false);
    }

    void Start()
    {
        //Hook Event
        PlayManager.Instance.GameOverEvent.AddListener(OnGameOver);
        //PlayManager.Instance.StartPlayEvent.AddListener(OnStartPlay);

        gameObject.SetActive(false);
    }
}
