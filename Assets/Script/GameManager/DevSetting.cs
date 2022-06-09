using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevSetting : Singleton<DevSetting>
{
    public int WidthBlockCount = 10, HeightBlockCount = 23;
    public Transform GridBottomLeft, GridTopRight;
    public int NextUpQueueSize = 5;
    // time size of playing time
    public int TimeSize = 60;
    public float InitFallingInterval = 1.0f;//sec per block
    public float ToConcurrentSideMovingInterval = 0.3f;
    public float SideMovingSpeedMult = 0.1f; //times InitFallingSpeed
    public float DownMovingSpeedMult = 0.5f; //times InitFallingSpeed
    public int RotateHelpUpMaxCnt = 2;
    public float waitsecdrop = 0.1f;
}
