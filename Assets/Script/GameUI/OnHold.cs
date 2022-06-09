using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHold : MonoBehaviour
{
    //On Hold
    [SerializeField] Transform onHoldbottomLeft, onHoldtopRight;
    Vector2 onHoldDisplayPos;
    GameObject onHoldDisplayObj;

    /* Event Listener */
    void OnOnHoldChange()
    {
        BlockType displayType = OnHoldNextManager.Instance.onHold;
        Destroy(onHoldDisplayObj);
        if (displayType == BlockType.None)
        {
            //dont display
            onHoldDisplayObj = null;
        }
        else
        {
            GameObject displayPrefab = Ulti.Instance.BlockType_to_UI_dic[displayType];
            onHoldDisplayObj = Instantiate(displayPrefab, onHoldDisplayPos, displayPrefab.transform.rotation, transform);
        }
    }

    void HookEvent()
    {
        OnHoldNextManager.Instance.OnHoldChangeEvent.AddListener(OnOnHoldChange);
    }
    
    void Start()
    {
        HookEvent();

        //On Hold
        onHoldDisplayPos = (onHoldbottomLeft.position + onHoldtopRight.position) / 2;
    }
}
