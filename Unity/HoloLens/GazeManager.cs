// **********************************************************************
// - FileName:          GazeGestureManager.cs
// - Author:              zhangbo
// - CreateTime:       2018/07/16 09:38:17
// - Email:                 bo_zhang1993@163.com
// - Modifier:
// - Dscription:
// - (C)Copyright  
// - All Rights Reserved.
// **********************************************************************

using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class GazeManager : MonoBehaviour
{
    [Tooltip("Maximum gaze distance, in meters, for calculating a hit. ")]
    public float MaxGazeDistance = 15.0f;
    [Tooltip("Select the layers raycast should target")]
    public LayerMask RaycastLayerMask = Physics.DefaultRaycastLayers;

    public GameObject FocusedObject { get; private set; }
    public bool Hit { get; private set; }
    public RaycastHit HitInfo { get; private set; }
    public Vector3 Position { get; private set; }
    public Vector3 Normal { get; private set;  }

    private Vector3 gazeOrigin;
    private Vector3 gazeDirection;
    private float lastHitDistance = 15.0f;

    public static GazeManager Instance;

    void Awake() {
        Instance = this;
    }


    // Update is called once per frame
    void Update()
    {
        gazeOrigin = Camera.main.transform.position;
        gazeDirection = Camera.main.transform.forward;

        UpdateRaycast();
    }

    void UpdateRaycast() {
        RaycastHit hitInfo;
        Hit = Physics.Raycast(gazeOrigin,gazeDirection,out hitInfo,MaxGazeDistance,RaycastLayerMask);

        GameObject oldFocusedObject = FocusedObject;
        HitInfo = hitInfo;
        //如果射线检测到凝视目标，记录相应信息
        if (Hit)
        {
            Position = hitInfo.point;
            Normal = hitInfo.normal;
            lastHitDistance = hitInfo.distance;
            FocusedObject = hitInfo.collider.gameObject;
        }
        //如果没检测到凝视目标，赋值默认值
        else {
            Position = gazeOrigin + (gazeDirection*lastHitDistance);
            Normal = -gazeDirection;
            FocusedObject = null;
        }

        if (oldFocusedObject != FocusedObject) {
            if (oldFocusedObject != null) {
                oldFocusedObject.SendMessage("OnGazeLeave",SendMessageOptions.DontRequireReceiver);
            }
            if (FocusedObject != null) {
                FocusedObject.SendMessage("OnGazeEnter",SendMessageOptions.DontRequireReceiver);
            }
        }

    }
}