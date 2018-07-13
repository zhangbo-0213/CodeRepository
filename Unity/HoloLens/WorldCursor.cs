using UnityEngine;

/// <summary>
/// 管理场景中凝视UI，当凝视某物体时，使用一种UI，当没有凝视物体时，使用另一种UI
/// </summary>
public class WorldCursor : MonoBehaviour {

    [SerializeField]
    private GameObject CursorOn;
    [SerializeField]
    private GameObject CursorOff;
    [SerializeField]
    private float cursorDistance = 2.0f;
    [SerializeField]
    private float scaleAmount = 1.0f;

    // Use this for initialization
    void Start () {
        CursorOn.SetActive(false);
        CursorOff.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

        Vector3 headPosition = Camera.main.transform.position;
        Vector3 gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;

        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
        {
            CursorOff.SetActive(false);
            //设置凝视选中物体时UI的位置和旋转，使其在合适位置并始终朝向相机
            CursorOn.transform.position = hitInfo.point-gazeDirection*0.1f;
            Quaternion targetRotation = Quaternion.FromToRotation(CursorOn.transform.up, headPosition - transform.position)* CursorOn.transform.rotation;
            CursorOn.transform.rotation = Quaternion.Lerp(CursorOn.transform.rotation,targetRotation,2.0f*Time.deltaTime);
            ScaleWithTan(CursorOn.transform);
            CursorOn.SetActive(true);          
        }
        else {
            CursorOn.SetActive(false);
            //设置凝视没有选中物体时UI的位置和旋转，使其在合适位置并始终朝向相机
            CursorOff.transform.position = headPosition + gazeDirection*cursorDistance;
            Quaternion targetRotation = Quaternion.FromToRotation(CursorOff.transform.up, headPosition - transform.position)* CursorOff.transform.rotation;
            CursorOff.transform.rotation= Quaternion.Lerp(CursorOff.transform.rotation, targetRotation, 2.0F*Time.deltaTime);
            ScaleWithTan(CursorOff.transform);
            CursorOff.SetActive(true);
        }
	}

    //根据Cursor距离屏幕的远近缩放其scale
    void ScaleWithTan(Transform target) {
        float tan = Mathf.Tan((Camera.main.fieldOfView/180.0f)*Mathf.PI);
        float distance = Vector3.Distance(target.transform.position,Camera.main.transform.position);
        float deltaScale = tan * distance;
        target.transform.localScale = new Vector3(deltaScale,deltaScale,deltaScale)*scaleAmount;
        //Debug.Log(target.transform.localScale);
    }
}
