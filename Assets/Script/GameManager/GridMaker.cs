using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMaker : Singleton<GridMaker>
{
    [SerializeField] Transform bottomLeft, topRight;
    [SerializeField] GameObject vertLine, horizonLine;
    [SerializeField] Transform gridLinesTransform;
    int xGridCnt = 10;
    int yGridCnt = 20;

    //draw line when start
    void Start()
    {

        // Instantiate vertical lines
        float xDrawPoint = bottomLeft.position.x + 1;
        Vector3 linePos = new Vector3(xDrawPoint, vertLine.transform.position.y);
        Quaternion lineRotation = vertLine.transform.rotation;
        for (int i = 0; i < xGridCnt; i++)
        {
            Instantiate(vertLine, linePos, lineRotation, gridLinesTransform);
            linePos.x++;
        }

        // Instantiate horizontal line
        linePos = new Vector3(horizonLine.transform.position.x, bottomLeft.position.y + 1);
        lineRotation = horizonLine.transform.rotation;
        for(int i = 0; i < yGridCnt; i++)
        {
            Instantiate(horizonLine, linePos, lineRotation, gridLinesTransform);
            linePos.y++;
        }
    }
}
