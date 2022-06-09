using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ulti : Singleton<Ulti>
{
    [SerializeField] private ListGameObjectVariable _tetrisBlocks;
    [SerializeField] private ListGameObjectVariable _gridBlocks;
    [SerializeField] private ListGameObjectVariable _UIBlocks;
    [SerializeField] private ListGameObjectVariable _helpBlocks;
    public List<GameObject> tetrisBlocks { get => _tetrisBlocks.Value; }
    public Dictionary<BlockType, GameObject> BlockType_to_GameObject_dic = new Dictionary<BlockType, GameObject>();
    public Dictionary<BlockType, GameObject> BlockType_to_Grid_dic = new Dictionary<BlockType, GameObject>();
    public Dictionary<BlockType, GameObject> BlockType_to_UI_dic = new Dictionary<BlockType, GameObject>();
    public Dictionary<BlockType, GameObject> BlockType_to_Help_dic = new Dictionary<BlockType, GameObject>();

    protected override void Awake()
    {
        base.Awake();

        //construct GetBlockGameObject_dic
        foreach(var obj in _tetrisBlocks.Value)
        {
            BlockType t = obj.GetComponent<TetrisBlock>().blockType;
            BlockType_to_GameObject_dic.Add(t, obj);
        }

        for(int i = 0; i < 7; i++)
        {
            BlockType t = _tetrisBlocks.Value[i].GetComponent<TetrisBlock>().blockType;
            BlockType_to_Grid_dic.Add(t, _gridBlocks.Value[i]);
            BlockType_to_UI_dic.Add(t, _UIBlocks.Value[i]);
            BlockType_to_Help_dic.Add(t, _helpBlocks.Value[i]);
        }
    }

    public BlockType RandomPickBlock()
    {
        System.Array arr = System.Enum.GetValues(typeof(BlockType));
        return (BlockType)arr.GetValue(Random.Range(1, arr.Length));
    }
}
