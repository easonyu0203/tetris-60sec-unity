using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextUp : MonoBehaviour
{
    [SerializeField] Transform bottomLeft, topRight;
    int nextUpQueueSize;
    List<Vector3> blocksPos = new List<Vector3>();
    List<GameObject> blocksGameObject = new List<GameObject>();


    /* Event Listener */
    void OnNextUpChangeEvent()
    {
        //First destroy all
        foreach(GameObject g in blocksGameObject)
        {
            Destroy(g);
        }

        //clear gameobject list
        blocksGameObject.Clear();

        //fill with new game object;
        GameObject newPrefab;
        int i = 0;
        foreach(BlockType t in OnHoldNextManager.Instance.nextUp)
        {
            newPrefab = Ulti.Instance.BlockType_to_UI_dic[t];
            GameObject newObj = Instantiate(newPrefab, blocksPos[i], newPrefab.transform.rotation, transform);
            blocksGameObject.Add(newObj);

            i++;
        }
    }

    void HookEvent()
    {
        OnHoldNextManager.Instance.NextUpChangeEvent.AddListener(OnNextUpChangeEvent);
    }

    void Start()
    {
        //hook event
        HookEvent();


        /* init variable */
        nextUpQueueSize = DevSetting.Instance.NextUpQueueSize;
         
        //init BlocksYPos
        float height = topRight.position.y - bottomLeft.position.y;
        float heightPerBlock = height / nextUpQueueSize;
        float xPos = (topRight.position.x + bottomLeft.position.x) / 2;
        float yInitPos = topRight.position.y;
        for(int i = 0; i < nextUpQueueSize; i++)
        {
            float yPos = -(i * heightPerBlock + heightPerBlock / 2) + yInitPos;
            blocksPos.Add(new Vector3(xPos, yPos, 0));
        }
    }

}
