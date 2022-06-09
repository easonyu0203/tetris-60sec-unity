using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnHoldNextManager : Singleton<OnHoldNextManager>
{
    //control variables
    public BlockType onHold;
    public Queue<BlockType> nextUp;
    int nextUpQueueSize;
    bool canHold;

    /* EVENT */
    public UnityEvent NextUpChangeEvent;
    public UnityEvent OnHoldChangeEvent; /* call when on hold change */
    public UnityEvent<BlockType> PopNextUpEvent;


    /* EVENT LISTENER */

    void HookEvents()
    {
        /* hooking event listener to event emitter */
        //PlayManager.Instance.StartPlayEvent.AddListener(OnStartPlay);
        PlayManager.Instance.NewBlockEvent.AddListener(OnNewBlock);

    }

    public void OnStartPlay()
    {
        ClearOnHold();
        ClearNextUp();
        FullNextUp();
    }

    void OnDownHit(TetrisBlock block)
    {
        //can hold block
        canHold = true;
    }

    void OnNewBlock()
    {
        PlayState.Instance.ControlingBlock.GetComponent<TetrisBlock>().DownHitEvent.AddListener(OnDownHit);
    }

    /* PUBLIC */

    public BlockType popNextUp()
    {
        /* pop next up queue */

        //check is full or not
        if (NextUpIsFull() == false)
        {
            Debug.Log("[OnHoldManager] cant pop nextup when not full");
        }

        //get one out and add one in
        BlockType _out = nextUp.Dequeue();
        nextUp.Enqueue(Ulti.Instance.RandomPickBlock());

        //Event happen
        PopNextUpEvent.Invoke(_out);
        NextUpChangeEvent.Invoke();

        return _out;
    }

    public void OnHoldSwitching()
    {
        if (canHold == false) return;
        canHold = false;
        /* switch on hold with current block */
        GameObject controlBlock = PlayState.Instance.ControlingBlock;
        if (onHold == BlockType.None)
        {
            SetOnHold(controlBlock.GetComponent<TetrisBlock>().blockType);
            Destroy(controlBlock);
            popNextUp();
        }
        else
        {
            BlockType _old = onHold;
            BlockType _new = controlBlock.GetComponent<TetrisBlock>().blockType;
            SetOnHold(_new);
            Destroy(controlBlock);
            PlayManager.Instance.InstantiateTetrisBlock(_old);
        }
    }

    public void LogNextUpQueue()
    {
        string s = "[Next Up Queue]";
        foreach (var t in nextUp) s += $"{t}, ";
        Debug.Log(s);
    }


    /* PRIVATE */

    void Start()
    {
        //Set variable
        onHold = PlayState.Instance.OnHold;
        nextUp = PlayState.Instance.NextUp;
        nextUpQueueSize = DevSetting.Instance.NextUpQueueSize;

        //hook event
        HookEvents();
    }

    void ClearNextUp()
    {
        /* clear next up queue and on hold block */
        nextUp.Clear();

        //event happend
        NextUpChangeEvent.Invoke();
    }

    public void ClearOnHold()
    {
        onHold = BlockType.None;
        canHold = true;

        //event happend
        OnHoldChangeEvent.Invoke();
    }

    void FullNextUp()
    {
        while(NextUpIsFull() == false)
        {
            nextUp.Enqueue(Ulti.Instance.RandomPickBlock());
        }

        //event happen
        NextUpChangeEvent.Invoke();
    }


    void SetOnHold(BlockType type)
    {
        onHold = type;

        //event happend
        OnHoldChangeEvent.Invoke();
    }





    bool NextUpIsFull()
    {
        if (nextUp.Count == nextUpQueueSize) return true;
        if (nextUp.Count < nextUpQueueSize) return false;
        Debug.LogError("[OnHoldManager] Nextup queue have size overflow");
        return false;
    }


}
