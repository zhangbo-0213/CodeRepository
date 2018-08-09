// **********************************************************************
// - FileName:          ReplaceModel.cs
// - Author:              zhangbo
// - CreateTime:       2018/08/09 09:06:35
// - Email:                 bo_zhang1993@163.com
// - Modifier:
// - Dscription:
// - (C)Copyright  
// - All Rights Reserved.
// **********************************************************************

using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// 编辑器下，根据GameObject标签，批量更换模型
/// </summary>
public class ReplaceModel : EditorWindow {
    private GameObject modelForReplace;
    private string originModelTag;
    [MenuItem("Tools/ReplaceModelBatching")]
    static void Init() {
        Rect wr = new Rect(100,100,450,600);
        ReplaceModel window = (ReplaceModel)EditorWindow.GetWindowWithRect(typeof(ReplaceModel),wr,true,"ReplaceGameObjectBatching");
        window.Show();
    }

    void OnGUI() {
        EditorGUILayout.BeginVertical("GroupBox");
        EditorGUILayout.LabelField("OriginGameObjectTag");
        GUILayout.Space(5);
        originModelTag = EditorGUILayout.TextField(originModelTag);

        EditorGUILayout.LabelField("GameObjectForReplace");
        GUILayout.Space(5);
        modelForReplace = (GameObject)EditorGUILayout.ObjectField(modelForReplace, typeof(GameObject), true);
        if (Selection.objects.Length == 1) {
            modelForReplace = Selection.objects[0] as GameObject;
        }

        GUILayout.Space(5);
        if (GUILayout.Button("Replace")) {
            Replace(originModelTag,modelForReplace);
        }
    }

    void Replace(string originGameObjectsTag,GameObject modelForReplace) {
        GameObject[] originObjects = GameObject.FindGameObjectsWithTag(originGameObjectsTag);
        for (int i = 0; i < originObjects.Length; i++) {
            string objName = originObjects[i].name;
            GameObject objParent = originObjects[i].transform.parent.gameObject;
            Vector3 originLocalPosition = originObjects[i].transform.localPosition;
            Vector3 originLocalEulerAngles = originObjects[i].transform.localEulerAngles;
            Vector3 originLocalScale = originObjects[i].transform.localScale;

            GameObject replaceObject = Instantiate(modelForReplace) as GameObject;
            replaceObject.name = objName;
            replaceObject.transform.SetParent(objParent.transform);
            replaceObject.transform.localPosition = originLocalPosition;
            replaceObject.transform.localEulerAngles = originLocalEulerAngles;
            replaceObject.transform.localScale = originLocalScale;

            
            replaceObject.tag = originGameObjectsTag;
            int childNum = 0;
            for (int j = 0; j < objParent.transform.childCount; j++) {
                if (objParent.transform.GetChild(j).gameObject == originObjects[i])
                    childNum = j;
            }
            replaceObject.transform.SetSiblingIndex(childNum);
            //DestroyImmediate(originObjects[i]);
        }
    }
}