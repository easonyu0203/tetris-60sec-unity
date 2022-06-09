using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayState : Singleton<PlayState>
{
    /* PLAYER MANAGER */
    public GameObject ControlingBlock;
    public bool Playing = false;
    public int TimeLeft;
    public int Lines;
    public int Score;
    public float FallSpeed;//block falling speed (cube/sec)


    /* LANDGRID MANAGER */
    public BlockType[,] GridState;
    public GameObject[,] GridOfGameObject;


    /* ONHOLDNEXT MANAGER */
    public BlockType OnHold;
    public Queue<BlockType> NextUp = new Queue<BlockType>();


    protected override void Awake()
    {
        base.Awake();
        GridState = new BlockType[DevSetting.Instance.WidthBlockCount, DevSetting.Instance.HeightBlockCount];
        GridOfGameObject = new GameObject[DevSetting.Instance.WidthBlockCount, DevSetting.Instance.HeightBlockCount];
    }

}
