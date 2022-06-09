using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePageController : Singleton<PausePageController>
{

    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void OnContinueClick()
    {
        this.gameObject.SetActive(false);
        PlayManager.Instance.ContinueGame();
    }

    public void OnReStartClick()
    {
        this.gameObject.SetActive(false);
        PlayManager.Instance.StartPlay();
        PauseHandler.Instance.StopPressEsc();
        PlayManager.Instance._audioSource.Stop();
    }

    private void OnEnable()
    {
        StartCoroutine("PressEnter");
    }

    private void OnDisable()
    {
        StopCoroutine("PressEnter");
    }

    IEnumerator PressEnter()
    {

        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                OnContinueClick();
            }
            yield return null;
        }

    }
}
