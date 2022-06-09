using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Only Care about Blocks that have landed
//activitive which change landed grids will go here
// Add Block, Check eliminate line
public class LandGridManager : Singleton<LandGridManager>
{
    BlockType[,] _gridState;
    GameObject[,] gridOfGameObject;
    [SerializeField]Transform GridCubes;
    [SerializeField] AudioSource audioSource;
    int xCnt, yCnt;

    /* Event */
    public UnityEvent<List<int>> EliminateLineEvent; // give list of index which is eliminate

    /* Event listener */
    void OnDownHit(TetrisBlock block)
    {
        GridUpdate(block);
        CheckAndEliminateLines();
    }

    void OnNewBlock()
    {
        // subcribe to new block
        PlayState.Instance.ControlingBlock.GetComponent<TetrisBlock>().DownHitEvent.AddListener(OnDownHit);
    }

    /* public method */

    public void Clear()
    {
        /* Reset gird state to empty */

        for (int x = 0; x < xCnt; x++)
            for (int y = 0; y < yCnt; y++)
            {
                _gridState[x, y] = BlockType.None;
                if(gridOfGameObject[x, y] != null)
                {
                    Destroy(gridOfGameObject[x, y]);
                }
            }
    }

    public void LogGridState()
    {
        /* Log Grid State in Debug.Log() */

        string _out = "[GridState]\n";
        for(int j = yCnt - 1; j >= 0; j--)
        {
            for (int i = 0; i < xCnt; i++)
            {
                _out += $"{_gridState[i, j]},";
            }
            _out += "\n";
        }

        Debug.Log(_out);
    }


    /* private method */

    // Start is called before the first frame update
    void Start()
    {
        /* Subscribe Event */
        PlayManager.Instance.NewBlockEvent.AddListener(OnNewBlock);

        _gridState = PlayState.Instance.GridState;
        gridOfGameObject = PlayState.Instance.GridOfGameObject;
        xCnt = DevSetting.Instance.WidthBlockCount;
        yCnt = DevSetting.Instance.HeightBlockCount;
        //Debug.Log(gridOfGameObject);
        //Debug.Log(_gridState);
    }

    void GridUpdate(TetrisBlock block)
    {
        Vector2 pos = block.posInGrid;
        int[,] matrix = block.blockMatrix;
        int matLen = block.matrixLen;
        BlockType blockType = block.blockType;

        //Instatiate grid and update grid state
        for(int i = 0; i < matLen; i++)
        {
            for(int j = 0; j < matLen; j++)
            {
                if(matrix[i,j] == 1)
                {
                    //here need new grid
                    Vector2 newGridPos = pos + new Vector2(j, matLen - 1 - i);
                    if(newGridPos.x >= 0 && newGridPos.x < 10 && newGridPos.y >= 0 && newGridPos.y < 20)
                    {
                        Vector2 newWorldPos = pos + new Vector2(j, matLen - 1 - i) + (Vector2)DevSetting.Instance.GridBottomLeft.position;
                        GameObject grid = Ulti.Instance.BlockType_to_Grid_dic[blockType];
                        PlayState.Instance.GridOfGameObject[(int)newGridPos.x, (int)newGridPos.y] = Instantiate(grid, (Vector3)newWorldPos, grid.transform.rotation, GridCubes);
                        PlayState.Instance.GridState[(int)newGridPos.x, (int)newGridPos.y] = blockType;
                    }
                }
            }
        }
    }

    void CheckAndEliminateLines()
    {
        //Chech for eliminate 
        List<int> linesIndex = new List<int>();
        for(int j = 0; j < 20; j++)
        {
            bool need_elim = true;
            for(int i = 0; i < 10; i++)
            {
                if(_gridState[i,j] == BlockType.None)
                {
                    need_elim = false;
                    break;
                }
            }
            if (need_elim)
            {
                linesIndex.Add(j);
            }
        }

        // EliminateLine
        if (linesIndex.Count == 0) return;
        linesIndex.Reverse();
        foreach(int index in linesIndex)
        {
            StartCoroutine(EliminateLine(index));
            audioSource.Play();
        }
        EliminateLineEvent.Invoke(linesIndex);
    }

    IEnumerator EliminateLine(int index)
    {
        //animation and partical effect maybe...


        // gird of game object
        for(int i = 0; i < 10; i++)
        {
            Destroy(gridOfGameObject[i, index]);
        }

        // grid state update
        for(int i = index; i < 19; i++)
        {
            for(int j = 0; j < 10; j++)
            {
                _gridState[j, i] = _gridState[j, i+1];
            }
        }
        yield return new WaitForSeconds(DevSetting.Instance.waitsecdrop);

        //move grid down
        for (int i = index; i < 19; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                gridOfGameObject[j, i] = gridOfGameObject[j , i + 1];
                if(gridOfGameObject[j, i] != null)
                    gridOfGameObject[j, i].transform.position += Vector3.down;
            }
        }
    }

}
