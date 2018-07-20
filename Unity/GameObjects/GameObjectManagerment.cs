using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class GameObjectManagerment : MonoBehaviour {

    public static GameObjectManagerment instance;

    public GameObject target;
    
    [NonSerialized]
    /// <summary>
    /// 是否获取包含隐藏的物体
    /// </summary>
    public bool getObjIncludehides = false;

    //是否记录 target  下 子物体的信息
    public bool recordObjsInfo = true;

    //是否记录 target 下 子物体 Transform 信息
    public bool recordTransform = true;

    

    [NonSerialized]
    public List<objInfoClass> objsInfo = new List<objInfoClass>();
    [NonSerialized]
    public List<bool> enable = new List<bool>();

    [NonSerialized]
    public List<GameObject> children = new List<GameObject>();
    [NonSerialized]
    public List<Vector3> positions = new List<Vector3>();
    [NonSerialized]
    public List<Quaternion> rotations = new List<Quaternion>();
    [NonSerialized]
    public List<Vector3> scales = new List<Vector3>();

    [Serializable]
    //记录 GameObject 及所属材质信息 的 列表
    public class objInfoClass {
        public GameObject obj;
        public List<MaterialClass> objMaterials = new List<MaterialClass>();
    }

    [Serializable]
    public class MaterialClass {
        public Shader shader;
        public Color color;
    }

    void Awake() {
        instance = this;
        _center = getCenter;
        if (target == null) {
            target = gameObject;
        }
        if (getObjIncludehides)
        {
            GetAll(target);
        }
        else {
            GetAndRecordTransform(target);
        }
        
    }

    private Vector3 _center = new Vector3(-9999,-9999,-9999);
    public Vector3 getCenter{
        get{
            if (_center == new Vector3(-9999, -9999, -9999))
                _center = BaseFunctionZ.GetChildrenBounds(GameObjectManagerment.instance.FindChildrenLevelOne("Environment")).center;
            return _center;
        }
    }

    #region 设置游戏对象透明
    /// <summary>
    /// 设置GameObject 透明
    /// </summary>
    /// <param name="obj">GameObject</param>
    /// <param name="alpha">AlphaValue</param>
    /// <param name="time">Interval time</param>
    public void SetObjTransparent(GameObject obj,float alpha,float time) {
        if (obj != null) {
            if (obj.GetComponent<MeshRenderer>()) {
                int count = obj.GetComponent<MeshRenderer>().materials.Length;
                for (int i = 0; i < count; i++) {
                    if (alpha <= 0.9f)
                    {
                        Shader shader = Resources.Load("Shader/TransparentDiffuse") as Shader;
                        obj.GetComponent<MeshRenderer>().materials[i].shader = shader;
                        try
                        {
                            obj.GetComponent<MeshRenderer>().materials[i].DOFade(alpha, time);
                        }
                        catch { }
                    }
                    else if (alpha > 0.95f) {
                        obj.GetComponent<MeshRenderer>().materials[i].DOFade(alpha,time);
                        if (time > 0) {
                            WaitToReal(obj,time);
                        }
                    }
                }
            }
        }
    }

    public void SetObjsTransparent(List<GameObject> objList,float alpha,float time) {
        int objsCount = objList.Count;
        for (int i = 0; i < objsCount; i++) {
            SetObjTransparent(objList[i],alpha,time);
        }
    }

    public void SetObjsTransparent(GameObject obj, float alpha, float time)
    {   
        SetObjTransparent(obj, alpha, time);

        int count = obj.transform.childCount;
        for (int i=0;i<count;i++)
        {
            SetObjsTransparent(obj.transform.GetChild(i).gameObject, alpha, time);
        }
    }

    IEnumerator WaitToReal(GameObject obj, float time) {
        yield return new WaitForSeconds(time);
        GameObjectManagerment.instance.ResetObjInfo(obj);
    }

    #endregion

    #region 对象查找并返回
    /// <summary>
    /// 查找脚本所属的游戏对象一级子物体
    /// </summary>
    /// <param name="objName"></param>
    /// <returns></returns>
    public GameObject FindChildrenLevelOne(string objName) {
        int c = transform.childCount;
        for (int i = 0; i < c; i++) {
            if (transform.GetChild(i).name == objName) {
                return transform.GetChild(i).gameObject;
            }
        }
        return null;
    }

    /// <summary>
    /// 通过 children 列表中保存的子对象进行查找
    /// </summary>
    /// <param name="objName"></param>
    /// <returns></returns>
    public GameObject FindChildren(string objName) {
        int c = children.Count;
        GameObject returnObj = null;
        for(int i = 0; i < c; i++)
        {
            if (children[i].name == objName) {
                returnObj = children[i];
            }
        }
        return returnObj;
    }
    #endregion

    #region 重置 objsInfo 列表信息
    /// <summary>
    /// 重置 obj 的材质
    /// </summary>
    /// <param name="obj"></param>
    public void ResetObjInfo(GameObject obj) {
        int count = objsInfo.Count;
        for (int i = 0; i < count; i++) {
            if (obj == objsInfo[i].obj) {
                if (objsInfo[i].obj.layer != 8) {
                    for (int j = 0; j < objsInfo[i].objMaterials.Count; j++) {
                        objsInfo[i].obj.GetComponent<MeshRenderer>().materials[j].shader = objsInfo[i].objMaterials[j].shader;
                        objsInfo[i].obj.GetComponent<MeshRenderer>().materials[j].color = objsInfo[i].objMaterials[j].color;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 重置 objInfo 列表中所有的材质
    /// </summary>
    public void ResetObjsInfo() {
        int objsCount = objsInfo.Count;
        for (int i = 0; i < objsCount; i++) {
            if (objsInfo[i].obj.layer != 8) {
                for (int j = 0; j < objsInfo[i].objMaterials.Count; j++) {
                    objsInfo[i].obj.GetComponent<MeshRenderer>().materials[j].shader=objsInfo[i].objMaterials[j].shader;
                    objsInfo[i].obj.GetComponent<MeshRenderer>().materials[j].color = objsInfo[i].objMaterials[j].color;
                }
            }
        }
    }

    public void ResetObjsInfo(GameObject obj) {
        ResetObjInfo(obj);
        int count = obj.transform.childCount;
        for (int i = 0; i < count; i++) {
            ResetObjsInfo(obj.transform.GetChild(i).gameObject);
        }
    }
    #endregion

    #region 记录子物体 Transform 及 材质信息 (objsInfo)
    /// <summary>
    /// 获取当前物体及子物体  并 记录信息
    /// </summary>
    /// <param name="objs"></param>
    public void GetAndRecordTransform(GameObject objs) {
        children.Add(objs);

        //记录 Transform 信息
        if (recordTransform) {
            positions.Add(objs.transform.position);
            rotations.Add(objs.transform.rotation);
            enable.Add(objs.activeSelf);
            scales.Add(objs.transform.localScale);
        }

        if (recordObjsInfo) {
            if (objs.GetComponent<MeshRenderer>()) {
                //通过图层过滤物体
                if (objs.layer != 8)
                {
                    int matsCount = objs.GetComponent<MeshRenderer>().materials.Length;
                    objInfoClass tempObj = new objInfoClass();
                    tempObj.obj = objs;
                    for (int i = 0; i < matsCount; i++)
                    {
                        MaterialClass tempMatInfo = new MaterialClass();
                        tempMatInfo.shader = objs.GetComponent<MeshRenderer>().materials[i].shader;
                        if (objs.GetComponent<MeshRenderer>().materials[i].HasProperty("_Color"))
                        {
                            tempMatInfo.color = objs.GetComponent<MeshRenderer>().materials[i].color;
                        }
                        tempObj.objMaterials.Add(tempMatInfo);
                    }
                    objsInfo.Add(tempObj);
                }
            }       
        }
        // 递归 记录 obj 子物体信息
        foreach (Transform child in objs.transform) {
            GetAndRecordTransform(child.gameObject);
        }
    }

    /// <summary>
    /// 获取所有子物体  并 记录信息
    /// </summary>
    /// <param name="objs"></param>
    public void GetAll(GameObject objs) {
        for (int i = 0; i < objs.transform.childCount; i++) {
            GameObject temp = objs.transform.GetChild(i).gameObject;

            children.Add(temp);
            if (recordTransform) {
                positions.Add(temp.transform.localPosition);
                rotations.Add(temp.transform.localRotation);
                enable.Add(temp.activeSelf);
                scales.Add(temp.transform.localScale);
            }

            if (recordObjsInfo) {
                if (temp.GetComponent<MeshRenderer>()) {
                    //通过图层过滤
                    if (objs.layer != 8) {
                        int matsCount;
                        #if UNITY_EDITOR
                            matsCount = temp.GetComponent<MeshRenderer>().sharedMaterials.Length;
                        #else
							matsCount = temp.GetComponent<MeshRenderer>().materials.Length;
                        #endif
                        objInfoClass tempObj = new objInfoClass();
                        tempObj.obj = temp;
                        for (int j = 0; j < matsCount; j++) {
                            MaterialClass tempMaterials = new MaterialClass();
                            #if UNITY_EDITOR
                            tempMaterials.shader = temp.GetComponent<MeshRenderer>().sharedMaterials[j].shader;
                            if (temp.GetComponent<MeshRenderer>().sharedMaterials[j].HasProperty("_Color"))
                            {
                                tempMaterials.color = temp.GetComponent<MeshRenderer>().sharedMaterials[j].color;
                            }
                            #else
                            tempMaterials.shader = temp.GetComponent<MeshRenderer>().materials[j].shader;
                             if (temp.GetComponent<MeshRenderer>().materials[j].HasProperty("_Color"))
                            {
                                tempMaterials.color = temp.GetComponent<MeshRenderer>().materials[j].color;
                            }
                            #endif
                            tempObj.objMaterials.Add(tempMaterials);
                        }
                        objsInfo.Add(tempObj);
                    }
                }
            }

            if (temp.transform.childCount > 0) {
                GetAll(temp);
            }
        }
    }
    #endregion

    #region 重置target下所有的transform信息
    /// <summary>
    /// 重置 target 下所有子物体的transform信息
    /// </summary>
    public void ResetTransform() {
        int count = children.Count;
        for (int i = 0; i < count; i++) {
            children[i].SetActive(enable[i]);
            children[i].transform.localPosition = positions[i];
            children[i].transform.localRotation = rotations[i];
            children[i].transform.localScale = scales[i];
        }
    }

    public void ResetTransformWithOutActive() {
        int count = children.Count;
        for (int i = 0; i < count; i++)
        {
            //children[i].SetActive(enable[i]);
            children[i].transform.localPosition = positions[i];
            children[i].transform.localRotation = rotations[i];
            children[i].transform.localScale = scales[i];
        }
    }
    #endregion
}
