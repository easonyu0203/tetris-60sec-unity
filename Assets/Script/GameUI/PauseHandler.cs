using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseHandler : Singleton<PauseHandler>
{

    [SerializeField] Button _button;
    
    public void OnClick()
    {
        PlayManager.Instance.StopGame();

        //call pause page
        PausePageController.Instance.gameObject.SetActive(true);
    }

    public void CanPause()
    {
        StartCoroutine("PressEsc");
        _button.onClick.AddListener(OnClick);
    }

    void OnGameOver()
    {
        StopPressEsc();
    }

    public void StopPressEsc()
    {
        StopCoroutine("PressEsc");
        _button.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        PlayManager.Instance.GameOverEvent.AddListener(OnGameOver);
        _button = GetComponent<Button>();
        //_button.onClick.AddListener(OnClick);
    }

    IEnumerator PressEsc()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnClick();
            }
            yield return null;
        }
    }
}
