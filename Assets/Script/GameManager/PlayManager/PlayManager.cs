using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayManager : Singleton<PlayManager>
{

    //Time handler
    int timeLeft;
    public AudioSource _audioSource;
    public AudioSource hitGround;


    /* EVENT */
    public UnityEvent StartPlayEvent;
    public UnityEvent NewBlockEvent;
    public UnityEvent<int> TimeChangeEvent; //give time left
    public UnityEvent GameOverEvent;

    /* Event Handler */
    void HookEvent()
    { 
        OnHoldNextManager.Instance.PopNextUpEvent.AddListener(OnPopNextUp);
    }

    void OnDownHit(TetrisBlock block)
    {
        if (ChechGameOver(block))
        {
            GameOver();
        }
        else
        {
            //get new block from queue
            OnHoldNextManager.Instance.popNextUp();
        }
    }

    void OnNewBLock()
    {
        PlayState.Instance.ControlingBlock.GetComponent<TetrisBlock>().DownHitEvent.AddListener(OnDownHit);
    }

    void OnPopNextUp(BlockType blockType)
    {
        InstantiateTetrisBlock(blockType);
    }



    /* PUBLIC */
    public void StartPlay()
    {
        //GameOverController
        GameOverPageController.Instance.OnStartPlay();
        // Controlling block
        if (PlayState.Instance.ControlingBlock != null)
            Destroy(PlayState.Instance.ControlingBlock);
        OnHoldNextManager.Instance.ClearOnHold();

        OnHoldNextManager.Instance.OnStartPlay();

        InitPlayState();
        StartGameANimationHandler.Instance.StartAnimation();

    }

    public void EndAnimationStartPlay()
    {



        //InstantiateTetrisBlock(Ulti.Instance.RandomPickBlock());
        OnHoldNextManager.Instance.popNextUp();

        //pause button
        PauseHandler.Instance.CanPause();

        //start hold controller
        StartCoroutine("HoldController");

        //Start Timer
        StartCoroutine("StartTimer");

        // Event happend
        StartPlayEvent.Invoke();

        _audioSource.volume = 1.0f;
        _audioSource.Play();
    }

    public void StopGame()
    {

        //stop all controller
        StopCoroutine("HoldController");
        if(PlayState.Instance.ControlingBlock != null)
            PlayState.Instance.ControlingBlock.GetComponent<BlockController>().enabled = false;

        // pause time
        StopCoroutine("StartTimer");

        _audioSource.volume = 0.2f;

    }

    public void ContinueGame()
    {
        _audioSource.volume = 1.0f;

        //start Controller
        StartCoroutine("HoldController");
        PlayState.Instance.ControlingBlock.GetComponent<BlockController>().enabled = true;

        //continue time
        StartCoroutine("StartTimer");
    }

    /* PRIVATE */
    void Start()
    {
        HookEvent();
        //StartPlay();
        _audioSource = GetComponent<AudioSource>();


    }


    void GameOver()
    {
        StopGame();

        //Event happend
        GameOverEvent.Invoke();

        _audioSource.Stop();
    }

    IEnumerator StartTimer()
    {
        while(timeLeft > 0)
        {
            yield return new WaitForSeconds(1);
            timeLeft--;
            //Event happend
            TimeChangeEvent.Invoke(timeLeft);
        }
        //Time up -> Game Over
        GameOver();
    }

    void InitPlayState()
    {
        PlayState.Instance.Playing = true;
        PlayState.Instance.TimeLeft = DevSetting.Instance.TimeSize;
        PlayState.Instance.Lines = 0;
        PlayState.Instance.Score = 0;
        PlayState.Instance.FallSpeed = DevSetting.Instance.InitFallingInterval;
        

        //grid
        LandGridManager.Instance.Clear();

        //time left
        timeLeft = DevSetting.Instance.TimeSize;

        //Event happend
        TimeChangeEvent.Invoke(timeLeft);
        ScoreLineManager.Instance.LineCntChangeEvent.Invoke(0);
        ScoreLineManager.Instance.ScoreChangeEvent.Invoke(0);
    }

    public void InstantiateTetrisBlock(BlockType type)
    {
        var obj = Instantiate(Ulti.Instance.BlockType_to_GameObject_dic[type]);
        PlayState.Instance.ControlingBlock = obj;

        // Event happend
        NewBlockEvent.Invoke();
        OnNewBLock();
    }

    bool ChechGameOver(TetrisBlock block)
    {
        Vector2 pos = block.posInGrid;
        int[,] matrix = block.blockMatrix;
        int matLen = block.matrixLen;
        BlockType blockType = block.blockType;

        //Instatiate grid and update grid state
        for (int i = 0; i < matLen; i++)
        {
            for (int j = 0; j < matLen; j++)
            {
                if (matrix[i, j] == 1)
                {
                    //here need new grid
                    Vector2 newGridPos = pos + new Vector2(j, matLen - 1 - i);
                    if(newGridPos.y > 20)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    IEnumerator HoldController()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                OnHoldNextManager.Instance.OnHoldSwitching();
            }
            yield return null;
        }
    }
}
