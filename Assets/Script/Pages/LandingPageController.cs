using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingPageController : MonoBehaviour
{
    [SerializeField] AudioSource _audio;
    [SerializeField] AudioSource land;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PressEnter());   
    }

    IEnumerator PressEnter()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(StartGame());
                land.Stop();
                break;
            }
            yield return null;
        }
    }

    IEnumerator StartGame()
    {
        _audio.Play();
        yield return new WaitForSeconds(0.3f);
        PlayManager.Instance.StartPlay();
        
        this.gameObject.SetActive(false);
    }
}
