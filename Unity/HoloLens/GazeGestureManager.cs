using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.WSA.Input;

public class GazeGestureManager : MonoBehaviour {

    public static GazeGestureManager Instance { get; private set; }
    public GameObject FocusedObject { get; private set; }
    public GameObject MainPanel;

    GestureRecognizer gestureRecognizer;

    void Awake() {
        Instance = this;
        //实例化 手势识别对象
        gestureRecognizer = new GestureRecognizer();
        //指定手势识别类型
        gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.Hold );
        //注册手势事件
        gestureRecognizer.Tapped += Recognizer_TappedEvent;
        gestureRecognizer.HoldStarted += Recognizer_HoldStartedEvent;
        gestureRecognizer.HoldCompleted += Recognizer_HoldCompletedEvent;
        gestureRecognizer.StartCapturingGestures();
    }
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        GameObject oldFocusObject = FocusedObject;

        Vector3 headPosition = Camera.main.transform.position;
        Vector3 gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;
        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
        {
            FocusedObject = hitInfo.collider.gameObject;
        }
        else {
            FocusedObject = null;
        }

        if (FocusedObject != oldFocusObject) {
            gestureRecognizer.CancelGestures();
            gestureRecognizer.StartCapturingGestures();
        }
		
	}

    void Recognizer_TappedEvent(TappedEventArgs args) {
        //Debug.Log("Tap");
        OnSelectedOption("OnTapSelected");
    }

    void Recognizer_HoldStartedEvent(HoldStartedEventArgs args) {
        //Debug.Log("Hold");    
        OnSelectedOption("OnHoldSelected");
    }

    void Recognizer_HoldCompletedEvent(HoldCompletedEventArgs args) {
       //Debug.Log("HoldCompleted");
        OnSelectedOption("OnHoldCompleted");
    }

    void OnSelectedOption(string functionName) {
        //如果此时凝视对象不为空，执行凝视对象的自身响应
        if (FocusedObject != null) {
            //Debug.Log("FocusedObjected:" + FocusedObject.name);
            FocusedObject.SendMessageUpwards(functionName,SendMessageOptions.DontRequireReceiver);
        }

        //否则如果没有主UI面板没有显示，则显示主UI面板
        else{
            if (MainPanel.activeSelf != true) {
                if (SceneManager.GetActiveScene().name == "IDC_MachineRoom") {
                    MainPanel.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1f;
                }
                MainPanel.SetActive(true);
            }    
        }
    }

    void OnDestroy()
    {
        gestureRecognizer.Tapped -= Recognizer_TappedEvent;
        gestureRecognizer.HoldStarted -= Recognizer_HoldStartedEvent;
        gestureRecognizer.HoldCompleted -= Recognizer_HoldCompletedEvent;
    }
}
