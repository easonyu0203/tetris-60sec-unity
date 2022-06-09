using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreLineManager : Singleton<ScoreLineManager>
{
    bool comboing;
    int comboCnt;
    bool lastHaveElim;
    int[] elimLinePoint = new int[4];
    int comboPoint = 50;

    /* Event */
    public UnityEvent<int> LineCntChangeEvent; // give current line count
    public UnityEvent<int> ScoreChangeEvent; // give current score
   

    /* Event Listener */
    void OnEliminateLine(List<int> linesIndex)
    {
        PlayState.Instance.Lines += linesIndex.Count;

        //Event happend
        LineCntChangeEvent.Invoke(PlayState.Instance.Lines);

        /* Score calculating */
        ScoreCal(linesIndex);


        lastHaveElim = true;
    }

    void OnDownHit(TetrisBlock block)
    {
        comboing = (lastHaveElim) ? true : false;
        comboCnt = (lastHaveElim) ? (comboCnt + 1) : 0;

        lastHaveElim = false;
    }

    void OnNewBlock()
    {
        // subcribe to new block
        PlayState.Instance.ControlingBlock.GetComponent<TetrisBlock>().DownHitEvent.AddListener(OnDownHit);
    }

    void HookEvent()
    {
        LandGridManager.Instance.EliminateLineEvent.AddListener(OnEliminateLine);
        PlayManager.Instance.NewBlockEvent.AddListener(OnNewBlock);

    }

    void ScoreCal(List<int> LinesIndex)
    {
        int addPoint = 0;
        switch (LinesIndex.Count)
        {
            case 1:
                addPoint += elimLinePoint[0];
                break;
            case 2:
                addPoint += elimLinePoint[1];
                break;
            case 3:
                addPoint += elimLinePoint[2];
                break;
            case 4:
                addPoint += elimLinePoint[3];
                break;
        }

        if (comboing)
        {
            Debug.Log($"Combo count: {comboCnt}");
            addPoint += comboPoint * comboCnt;
        }

        PlayState.Instance.Score += addPoint;

        //Event Happend
        ScoreChangeEvent.Invoke(PlayState.Instance.Score);
    }


    void Start()
    {
        HookEvent();

        // init variable
        comboing = false;
        lastHaveElim = false;
        elimLinePoint = new int[4] { 100, 300, 500, 800 };
        comboPoint = 50;
        comboCnt = 0;
    }

}
