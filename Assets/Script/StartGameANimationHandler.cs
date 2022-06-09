using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameANimationHandler : Singleton<StartGameANimationHandler>
{
    Animator _animator;
    [SerializeField] AudioSource _countDown;

    //[SerializeField]Animation _countdownAnimation;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        gameObject.SetActive(false);
    }

    void OnAnimationStart()
    {
        _countDown.Play();
    }

    public void StartAnimation()
    {
        gameObject.SetActive(true);
        _animator.Play("CountDown");
    }

    public void OnAnimationEnd()
    {
        _animator.SetTrigger("EndAnimation");
        PlayManager.Instance.EndAnimationStartPlay();

        gameObject.SetActive(false);
    }
}
