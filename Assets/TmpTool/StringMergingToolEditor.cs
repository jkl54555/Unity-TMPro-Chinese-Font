using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StringMergingTool))]
public class StringMergingToolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        StringMergingTool stringFilter = (StringMergingTool)target;

        // 更新按鈕以調用正確的方法
        if (GUILayout.Button("Convert"))
        {
            stringFilter.Filter(); // 調用 Filter 方法
        }
        // 移除 Filter Text File Content 按鈕，因為我們已經將功能合併至 Filter 方法
        // 如果需要保留，你需要在 StringFilter 類中實作該方法

        // 你可以選擇保留下面的按鈕，如果有需要的話
        // if (GUILayout.Button("Filter Text File Content"))
        // {
        //     stringFilter.TextFileFilter();
        // }
    }
}
