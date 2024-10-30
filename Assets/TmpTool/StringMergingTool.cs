/// 2024/10/30更新，加入v1.2版
/// 程式腳本 由louise-bwsx提供第一版 詳情可見https://github.com/jkl54555/Unity-TMPro-Chinese-Font/discussions/2#discussioncomment-7875455
/// 補充文字工具
/// 1. 確保補充文字原字庫沒有
/// 2. 確認字庫中是否有重複的字
/// 3. 處理結果不會更動原字庫檔案，需自行修改字庫檔 (如果沒犯懶惰病會在未來優化)
using System.Collections.Generic;
using UnityEngine;

public class StringMergingTool : MonoBehaviour
{
    [Header("補充字")]
    [TextArea(2, 10)]
    [SerializeField] private string content; // 補充字
    [Header("原字庫")]
    [SerializeField] private TextAsset textFile; // 原字庫
    [Header("輸出結果")]
    [TextArea(10, 10)]
    [SerializeField] private string blank; // 最終結果

    [ContextMenu("Convert")] // 按鈕名稱，按鈕方法於 StringMergingToolEditor 程式碼中
    public void Filter() // 方法名稱為 Filter
    {
        blank = "";
        List<char> originalChars = new List<char>(textFile.text.ToCharArray());
        HashSet<char> seenInContent = new HashSet<char>(); // 用來跟蹤補充字
        HashSet<char> duplicatesInOriginal = new HashSet<char>(); // 用來跟蹤原字庫的重複字元
        HashSet<char> addedToBlank = new HashSet<char>(); // 用來跟蹤已加入 blank 的字元
        List<char> validContentChars = new List<char>(); // 用來儲存有效的補充字元

        // 檢查原字庫中的重複字元
        Dictionary<char, int> originalCharCount = new Dictionary<char, int>();
        foreach (char c in originalChars)
        {
            if (originalCharCount.ContainsKey(c))
            {
                originalCharCount[c]++;
                duplicatesInOriginal.Add(c); // 如果重複，則加入集合
            }
            else
            {
                originalCharCount[c] = 1;
            }
        }

        // 將補充字元加入集合並檢查重複
        foreach (char currentChar in content)
        {
            if (seenInContent.Add(currentChar)) // 只有當字元未見過時，才添加
            {
                if (!originalChars.Contains(currentChar))
                {
                    validContentChars.Add(currentChar); // 只有當原字庫中沒有此字時，才加入有效字元
                }
            }
        }

        // 輸出補充的字
        if (validContentChars.Count > 0)
        {
            Debug.Log("以補充: " + string.Join("、", validContentChars)); // 有效的補充字
        }
        else
        {
            Debug.Log("無需補充");
        }

        // 輸出原字庫中的重複字
        if (duplicatesInOriginal.Count > 0)
        {
            Debug.Log("原字庫中重複字: " + string.Join("、", duplicatesInOriginal));
        }

        // 檢查補充字與原字庫的重複字
        List<char> duplicatesInContent = new List<char>();
        foreach (char currentChar in seenInContent)
        {
            if (originalChars.Contains(currentChar))
            {
                duplicatesInContent.Add(currentChar); // 如果重複則加入
            }
        }

        if (duplicatesInContent.Count > 0)
        {
            Debug.Log("原字庫已有: " + string.Join("、", duplicatesInContent));
        }

        // 最終生成 blank 字串
        // 添加有效的補充字元到 blank
        foreach (char validChar in validContentChars)
        {
            blank += validChar; // 添加有效的補充字元
            addedToBlank.Add(validChar); // 標記為已添加
        }

        // 添加原字庫中的字元（只添加不在補充字中的字元且只出現一次）
        foreach (char currentChar in originalChars)
        {
            if (!seenInContent.Contains(currentChar) && !addedToBlank.Contains(currentChar))
            {
                blank += currentChar; // 添加原字庫中未重複且未添加的字元
                addedToBlank.Add(currentChar); // 標記為已添加
            }
            else if (originalCharCount[currentChar] > 1 && !addedToBlank.Contains(currentChar))
            {
                blank += currentChar; // 如果原字庫中出現多次，也加入一次
                addedToBlank.Add(currentChar); // 標記為已添加
            }
        }

        // 輸出整合的結果
        Debug.Log($"已將文字整合至blank。\nOutput: \n{blank}");
    }
}

