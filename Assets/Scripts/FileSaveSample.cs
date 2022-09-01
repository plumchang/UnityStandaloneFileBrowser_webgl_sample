using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class FileSaveSample : MonoBehaviour, IPointerDownHandler
{
    // テキストアウトプット
    [SerializeField] private Text outputText;

#if UNITY_WEBGL && !UNITY_EDITOR
    //
    // WebGL
    //

    // StandaloneFileBrowserのブラウザスクリプトプラグインから呼び出す
    [DllImport("__Internal")]
    private static extern void DownloadFile(string gameObjectName, string methodName, string filename, byte[] byteArray, int byteArraySize);

    // ファイルを保存する
    public void OnPointerDown(PointerEventData eventData) {
        var str = outputText.text + "_saved";

        if (str.Length > 0)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            DownloadFile(gameObject.name, "OnFileDownload", "sample_saved.txt", bytes, bytes.Length);
        }
    }

    // ファイルダウンロード後の処理
    public void OnFileDownload() {
        Debug.Log("CSV file saved");
        outputText.text = "File Saved";
    }

#else

    //
    // OSビルド & Unity editor上
    //

    public void OnPointerDown(PointerEventData eventData) { }

    void Start()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(() => SaveFile());
    }

    // ファイルを保存する
    public void SaveFile()
    {
        var str = outputText.text + "_saved";

        if (str.Length > 0)
        {
            var path = StandaloneFileBrowser.SaveFilePanel("ファイルの保存", "", "sample_saved", "txt");
            if (!string.IsNullOrEmpty(path))
            {
                File.WriteAllText(path, str);
                Debug.Log("File saved");
                outputText.text = "File Saved";
            }
        }
    }

#endif

}
