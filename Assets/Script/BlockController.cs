using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    TetrisBlock tetrisBlock;
    /* Down */
    float initFallingInterval;
    [SerializeField] float downInterval;
    [SerializeField] float lastDownTimeSize;
    //index 0->left, 1->right
    [SerializeField] float[] sideInterval;
    [SerializeField] float[] lastsideTimeSize;
    [SerializeField] bool[] concurrentMode;
    [SerializeField] float toConcurrentSideMovingInterval;
    [SerializeField] AudioSource audMove;

    void Start()
    {
        tetrisBlock = GetComponent<TetrisBlock>();
        audMove = GetComponent<AudioSource>();

        /* Down */
        initFallingInterval = DevSetting.Instance.InitFallingInterval;
        downInterval = initFallingInterval * DevSetting.Instance.DownMovingSpeedMult;
        lastDownTimeSize = downInterval;

        /* Side */
        sideInterval = new float[2] { initFallingInterval * DevSetting.Instance.SideMovingSpeedMult, initFallingInterval * DevSetting.Instance.SideMovingSpeedMult };
        lastsideTimeSize = new float[2] { sideInterval[0], sideInterval[1] };
        toConcurrentSideMovingInterval = DevSetting.Instance.ToConcurrentSideMovingInterval;
        concurrentMode = new bool[2] { false, false };
    }

    void Update()
    {
        lastDownTimeSize += Time.deltaTime;
        lastsideTimeSize[0] += Time.deltaTime;
        lastsideTimeSize[1] += Time.deltaTime;
        DownMotionHandler();
        SideMotionHandler(0);
        SideMotionHandler(1);
        RotationMotionHandler();
    }

    void RotationMotionHandler()
    {
        if (Input.GetButtonDown("Rotate"))
        {
            tetrisBlock.Rotate();
            audMove.Play();
        }
    }

    void SideMotionHandler(int index)
    {
        string side = "";
        side = (index == 0) ? "Left" : "Right";
        MoveType type = (index == 0) ? MoveType.Left : MoveType.Right;

        if (Input.GetButtonDown(side))
        {
            tetrisBlock.Move(type);
            
            lastsideTimeSize[index] = 0.0f;
        }
        else if (Input.GetButton(side))
        {
            if(concurrentMode[index] == false)
            {
                //not in concurrent mode yet
                if(lastsideTimeSize[index] >= toConcurrentSideMovingInterval)
                {
                    concurrentMode[index] = true;
                }
            }
            else
            {
                //concurrent mode
                if(lastsideTimeSize[index] >= sideInterval[index])
                {
                    tetrisBlock.Move(type);
                    
                    lastsideTimeSize[index] = 0.0f;

                }
            }
        }
        else if (Input.GetButtonUp(side))
        {
            concurrentMode[index] = false;
        }
    }

    void DownMotionHandler()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            tetrisBlock.HardDrop();
        }
        else if (Input.GetButtonDown("Down"))
        {
            //instant move down
            SoftDrop();
            audMove.Play();
        }
        else if (Input.GetButton("Down"))
        {
            //concurrently move down
            if(lastDownTimeSize >= downInterval)
            {
                SoftDrop();
                audMove.Play();
            }
        }
        else
        {
            //auto falling
            if(lastDownTimeSize >= initFallingInterval)
            {
                AutoDrop();
            }
        }
    }

    void SoftDrop()
    {
        tetrisBlock.Move(MoveType.Down);
        lastDownTimeSize = 0.0f;
    }

    void AutoDrop()
    {
        tetrisBlock.Move(MoveType.Down);
        lastDownTimeSize = 0.0f;
    }
}
