// **********************************************************************
// - FileName:          GestureManager.cs
// - Author:              zhangbo
// - CreateTime:       2018/07/16 10:40:50
// - Email:                 bo_zhang1993@163.com
// - Modifier:
// - Dscription:
// - (C)Copyright  
// - All Rights Reserved.
// **********************************************************************

using UnityEngine;
using System.Collections;
using UnityEngine.XR.WSA.Input;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(GazeManager))]
public class GestureManager : MonoBehaviour {

    public Transform mainPanel;
    public GameObject FocusedObject {
        get { return focusedObject; }
    }

    public bool IsManipulating { get; private set; }
    public bool IsNavigation { get; private set; }
    public Vector3 ManipulationPosition {
        get { return manipulationPosition; }
     }
    private Vector3 manipulationPosition;
    private float navigationXValue;

    private GameObject focusedObject;
    private GestureRecognizer gestureRecognizer;

    public static GestureManager Instance;

    void Awake() {
        Instance = this;
    }

    void Start() {
        //��������ʶ��ʵ��
        gestureRecognizer = new GestureRecognizer();
        //ע��ָ����������
        gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap|
                                                                                 GestureSettings.DoubleTap|
                                                                                 GestureSettings.Hold|
                                                                                 GestureSettings.ManipulationTranslate);
        //�������Ƶ���¼�
        gestureRecognizer.Tapped += GestureRecognizer_Tapped;

        //�������ư�ס�¼�
        gestureRecognizer.HoldStarted += GestureRecognizer_HoldStarted;


        //����.Manipulation�����¼�
        gestureRecognizer.ManipulationStartedEvent += GestureRecognizer_ManipulationStartedEvent;
        gestureRecognizer.ManipulationUpdatedEvent += GestureRecognizer_ManipulationUpdatedEvent;
        gestureRecognizer.ManipulationCompletedEvent += GestureRecognizer_ManipulationCompletedEvent;
        gestureRecognizer.ManipulationCanceledEvent += GestureRecognizer_ManipulationCanceledEvent;

        //��ʼ����ʶ��
        gestureRecognizer.StartCapturingGestures();
    }

    void LateUpdate()
    {
        GameObject oldFocusedObject = focusedObject;
        if (GazeManager.Instance.Hit && GazeManager.Instance.HitInfo.collider != null)
        {
            focusedObject = GazeManager.Instance.HitInfo.collider.gameObject;
        }
        if (oldFocusedObject != focusedObject)
        {
            gestureRecognizer.CancelGestures();
            gestureRecognizer.StartCapturingGestures();
        }
    }

    private void GestureRecognizer_HoldStarted(HoldStartedEventArgs obj)
    {
        //�����հ״������˵�
        if (GazeManager.Instance.FocusedObject == null)
        {
            if (!mainPanel.GetChild(0).gameObject.activeSelf) {
                mainPanel.GetComponent<MainPanelShow>().Show();
            }
        }
        OnSelectCallBack(focusedObject, "OnHold");
    }

   

    //����&˫�������¼��ص�
    private void GestureRecognizer_Tapped(TappedEventArgs obj)
    {
        if (obj.tapCount == 1)
        {
            OnSelectCallBack(focusedObject, "OnTap");
        }
        else {
            OnSelectCallBack(focusedObject,"OnDoubleTap");
            //�հ״�˫�����ø�������
            if (GazeManager.Instance.FocusedObject == null)
            {
                if (IDCModelAction.Instance)
                    IDCModelAction.Instance.ResetScene();
                else if (MachineRoomAction.Instance)
                    MachineRoomAction.Instance.ResetScene();
                else if (DynamicModelAction.Instance)
                    DynamicModelAction.Instance.ResetScene();
            }
        }
    }

    //Manipulation�����¼��ص�
    private void GestureRecognizer_ManipulationStartedEvent(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        if (GazeManager.Instance.FocusedObject != null) {
            IsManipulating = true;
            manipulationPosition = cumulativeDelta;
            navigationXValue = .0f;
            if (GazeManager.Instance.FocusedObject)
                GazeManager.Instance.FocusedObject.SendMessageUpwards("PerformManipulationStart",cumulativeDelta,SendMessageOptions.DontRequireReceiver);
        }
    }

    private void GestureRecognizer_ManipulationUpdatedEvent(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        if (GazeManager.Instance.FocusedObject != null)
        {
            IsManipulating = true;
            manipulationPosition = cumulativeDelta;
            if (GazeManager.Instance.FocusedObject)
                GazeManager.Instance.FocusedObject.SendMessageUpwards("PerformManipulationUpdate", cumulativeDelta, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void GestureRecognizer_ManipulationCanceledEvent(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        IsManipulating = false;
    }

    private void GestureRecognizer_ManipulationCompletedEvent(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        IsManipulating = false;
        navigationXValue = cumulativeDelta.x;
        if(GazeManager.Instance.FocusedObject)
            GazeManager.Instance.FocusedObject.SendMessageUpwards("PerformManipulationCompleted", cumulativeDelta, SendMessageOptions.DontRequireReceiver);
    }

    //�����¼�����ʱ�Ļص�����
    void OnSelectCallBack(GameObject focusedObject,string functionName) {
        if (focusedObject != null) {
            focusedObject.SendMessageUpwards(functionName,SendMessageOptions.DontRequireReceiver);
        }
    }

    void OnDestroy() {
        gestureRecognizer.Tapped -= GestureRecognizer_Tapped;

        gestureRecognizer.HoldStarted -= GestureRecognizer_HoldStarted;

        gestureRecognizer.ManipulationStartedEvent -= GestureRecognizer_ManipulationStartedEvent;
        gestureRecognizer.ManipulationUpdatedEvent -= GestureRecognizer_ManipulationUpdatedEvent;
        gestureRecognizer.ManipulationCompletedEvent -= GestureRecognizer_ManipulationCompletedEvent;
        gestureRecognizer.ManipulationCanceledEvent -= GestureRecognizer_ManipulationCanceledEvent;
    }
}