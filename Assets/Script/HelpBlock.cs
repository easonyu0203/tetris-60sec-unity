using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpBlock : MonoBehaviour
{
    TetrisBlock block;
    GameObject helpBlock;
    Transform pivot, helpPivot;

    /* Event Listener */
    void OnSideRotate()
    {
        SetHelpBlockPosRotation();
    }

    private void Start()
    {
        block = GetComponent<TetrisBlock>();
        pivot = transform.GetChild(0);
        

        //Hook event
        block.SideRotateEvent.AddListener(OnSideRotate);

        //instantiate help block
        GameObject prefab = Ulti.Instance.BlockType_to_Help_dic[block.blockType];
        helpBlock = Instantiate(prefab);
        helpPivot = helpBlock.transform.GetChild(0).transform;

        //set position and rotation
        SetHelpBlockPosRotation();
    }

    private void OnDestroy()
    {
        Destroy(helpBlock);
    }

    void SetHelpBlockPosRotation()
    {
        Vector2 helpPos = block.posInGrid;

        //go down see when to stop
        for(int i = 0; i < 30; i++)
        {
            if(block.CheckDownWall(helpPos, block.blockMatrix))
            {
                helpPos += Vector2.down;
            }
            else
            {
                helpPos += Vector2.up;
                break;
            }
        }

        helpBlock.transform.position = DevSetting.Instance.GridBottomLeft.position + (Vector3)helpPos;
        helpPivot.rotation = pivot.transform.rotation;
    }


}
