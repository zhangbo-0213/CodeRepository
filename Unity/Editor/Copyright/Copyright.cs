#region 模块信息
// **********************************************************************
// - FileName:          Copyright.cs
// - Author:              #AuthorName#
// - CreateTime:       #CreateTime#
// - Email:                #AuthorEmail#
// - Modifier:
// - Dscription:
// - (C)Copyright  
// - All Rights Reserved.
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using System.IO;

public class Copyright : UnityEditor.AssetModificationProcessor
{
    private const string AuthorName = "zhangbo";
    private const string AuthorEmail = "bo_zhang1993@163.com";

    private const string DateFormat = "yyyy/MM/dd HH:mm:ss";

    private static void OnWillCreateAsset(string path)
    {
        path = path.Replace(".meta", "");
        if (path.EndsWith(".cs"))
        {
            string allText = File.ReadAllText(path);
            allText = allText.Replace("#AuthorName#",AuthorName);
            allText = allText.Replace("#AuthorEmail#",AuthorEmail);
            allText = allText.Replace("#CreateTime#",System.DateTime.Now.ToString(DateFormat));
            File.WriteAllText(path, allText);
            UnityEditor.AssetDatabase.Refresh();
        }

    }
}