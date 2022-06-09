using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum BlockType
{
    None,
    Blue,
    Orange,
    Green,
    Red,
    LighBlue,
    Purple,
    Yellow
}

public enum MoveType
{
    Left,
    Right,
    Down,
}


//Serve as basic mechanism for Tetris block
public class TetrisBlock : MonoBehaviour
{
    public BlockType blockType = BlockType.None;
    [HideInInspector] public int matrixLen;
    public int[,] blockMatrix;
    public Vector2 posInGrid { get => _posInGrid;}
    private Vector2 _posInGrid;
    private Transform _transform;
    private Transform _gridOrigin;
    private Transform _pivot;
    private int rotateHelpUpMaxCnt;
    private int rotHelpCnt;
    AudioSource audioSource;


    /* Event */
    public UnityEvent<TetrisBlock> DownHitEvent;
    public UnityEvent SideRotateEvent;

    /* Event handler */
    void OnHitDown()
    {
        PlayManager.Instance.hitGround.Play();
        Destroy(this.gameObject);
    }


    /* Public Method */

    public void Move(MoveType moveType)
    {
        Vector2 wantPos = new Vector2();
        switch (moveType)
        {
            case MoveType.Left:
                wantPos = (_posInGrid + Vector2.left);
                break;
            case MoveType.Right:
                wantPos = (_posInGrid + Vector2.right);
                break;
            case MoveType.Down:
                wantPos = (_posInGrid + Vector2.down);
                break;
        }

        if(moveType == MoveType.Down)
        {
            if(CheckDownWall(wantPos, blockMatrix))
            {
                //can go down
                SetPosInGrid(wantPos);
                //audioSource.Play();
            }
            else
            {
                //hit something
                DownHitEvent.Invoke(this);
                OnHitDown();
            }
        }
        else
        {
            //side move
            if(CheckSideWall(wantPos, blockMatrix))
            {
                SetPosInGrid(wantPos);
                audioSource.Play();

                //event happend
                SideRotateEvent.Invoke();
            }
        }
    }

    public void HardDrop()
    {
        Vector2 wantPos = posInGrid;
        for(int i = 0; i < 30; i++)
        {
            if (CheckDownWall(wantPos, blockMatrix))
            {
                wantPos += Vector2.down;
            }
            else {
                wantPos += Vector2.up;
                break;
            }
        }
        SetPosInGrid(wantPos);
        //audioSource.Play();
        DownHitEvent.Invoke(this);
        OnHitDown();
    }

    public void Rotate()
    {
        int[,] _mat = new int[matrixLen, matrixLen];

        //left rotation
        //for (int i = 0; i < matrixLen; i++)
        //    for (int j = 0; j < matrixLen; j++)
        //    {
        //        _mat[i, j] = blockMatrix[j, matrixLen - 1 - i];
        //    }

        //right rotation
        for (int i = 0; i < matrixLen; i++)
            for (int j = 0; j < matrixLen; j++)
            {
                _mat[i, j] = blockMatrix[matrixLen - 1 - j, i];
            }

        bool flag = false;
        foreach(var j in new int[3] {0, 1, -1})
        {
            foreach(var i in new int[5] { 0, 1, -1, 2, -2 })
            {
                Vector2 wantPos = posInGrid + Vector2.right * i + Vector2.up * j;
                if (CheckSideWall(wantPos, _mat) && CheckDownWall(wantPos, _mat) && CheckUpWall(wantPos, _mat))
                {
                    if (j == 1) rotHelpCnt++;
                    _pivot.Rotate(Vector3.forward * -90);
                    blockMatrix = _mat;
                    SetPosInGrid(wantPos);
                    //audioSource.Play();
                    //Event happend
                    SideRotateEvent.Invoke();
                    
                    flag = true;
                    break;
                }
            }
            if (flag) break;
            if (rotHelpCnt == rotateHelpUpMaxCnt) break;
        }
    }


    /* Private Method */

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        _transform = transform;
        _gridOrigin = DevSetting.Instance.GridBottomLeft;
        _pivot = _transform.Find("Pivot");
        if (_pivot == null) Debug.LogError("[TetrisBlock] Can't find Pivot");
        rotateHelpUpMaxCnt = DevSetting.Instance.RotateHelpUpMaxCnt;
        
        rotHelpCnt = 0;
        InitBlockMatrix();
        InitPosInGrid();
    }


    void InitBlockMatrix()
    {
        //Init blockMatrix
        switch (blockType)
        {
            case BlockType.Blue:
                matrixLen = 3;
                blockMatrix = new int[3, 3]{
                    { 1,0,0},
                    { 1,1,1},
                    { 0,0,0}
                };
                break;
            case BlockType.Orange:
                matrixLen = 3;
                blockMatrix = new int[3, 3]{
                    { 0,0,1},
                    { 1,1,1},
                    { 0,0,0}
                };
                break;
            case BlockType.Red:
                matrixLen = 3;
                blockMatrix = new int[3, 3]{
                    { 1,1,0},
                    { 0,1,1},
                    { 0,0,0}
                };
                break;
            case BlockType.Green:
                matrixLen = 3;
                blockMatrix = new int[3, 3]{
                    { 0,1,1},
                    { 1,1,0},
                    { 0,0,0}
                };
                break;
            case BlockType.LighBlue:
                matrixLen = 4;
                blockMatrix = new int[4, 4]{
                    { 0,0,0,0},
                    { 1,1,1,1},
                    { 0,0,0,0},
                    { 0,0,0,0}
                };
                break;
            case BlockType.Purple:
                matrixLen = 3;
                blockMatrix = new int[3, 3]{
                    { 0,1,0},
                    { 1,1,1},
                    { 0,0,0}
                };
                break;
            case BlockType.Yellow:
                matrixLen = 2;
                blockMatrix = new int[2, 2]{
                    { 1,1},
                    { 1,1}
                };
                break;
            case BlockType.None:
                Debug.LogError("[TetrsBlock] BlockType is None");
                break;
        }
    }

    void InitPosInGrid()
    {
        int yLen = DevSetting.Instance.HeightBlockCount;
        switch (blockType)
        {
            case BlockType.LighBlue:
                SetPosInGrid(new Vector2(3, yLen - 1 - 2));
                break;
            case BlockType.Yellow:
                SetPosInGrid(new Vector2(4, yLen - 1 - 1));
                break;
            default:
                // other block type
                SetPosInGrid(new Vector2(3, yLen - 1 - 2));
                break;
        }

        for (int i = 0; i < yLen - 20; i++) Move(MoveType.Down);
    }

    void SetPosInGrid(Vector2 pos)
    {
        _posInGrid = pos;
        _transform.position = _gridOrigin.position + (Vector3)pos;
    }

    public bool CheckSideWall(Vector2 wantPos, int[,] mat)
    {
        for (int i = 0; i < matrixLen; i++)
        {
            for (int j = 0; j < matrixLen; j++)
            {
                if (mat[i, j] == 1)
                {
                    Vector2 wantGridPos = wantPos + new Vector2(j, matrixLen - 1 - i);
                    if (wantGridPos.x < 0 || wantGridPos.x >= 10)
                    {
                        //Debug.Log("Side wall bound");
                        //Debug.Log($"{i}, {j}, pos {wantPos}");
                        return false;
                    }
                    else if (wantGridPos.x >= 0 && wantGridPos.x < 10 && wantGridPos.y >= 0 && wantGridPos.y < 20)
                    {
                        if (PlayState.Instance.GridState[(int)wantGridPos.x, (int)wantGridPos.y] != BlockType.None)
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }

    public bool CheckDownWall(Vector2 wantPos, int[,] wantMat)
    {
        for (int i = 0; i < matrixLen; i++)
        {
            for (int j = 0; j < matrixLen; j++)
            {
                if (wantMat[i, j] == 1)
                {
                    Vector2 wantGridPos = wantPos + new Vector2(j, matrixLen - 1 - i);
                    if(wantGridPos.y < 0)
                    {
                        return false;
                    }
                    else if(wantGridPos.x >= 0 && wantGridPos.x < 10 && wantGridPos.y >= 0 && wantGridPos.y < 20)
                    {
                        if(PlayState.Instance.GridState[(int)wantGridPos.x, (int)wantGridPos.y] != BlockType.None)
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }

    bool CheckUpWall(Vector2 wantPos, int[,] wantMat)
    {
        for (int i = 0; i < matrixLen; i++)
        {
            for (int j = 0; j < matrixLen; j++)
            {
                if (wantMat[i, j] == 1)
                {
                    Vector2 wantGridPos = wantPos + new Vector2(j, matrixLen - 1 - i);
                    if (wantGridPos.y >= 20)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

}


