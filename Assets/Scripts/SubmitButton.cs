using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

/// <summary>
/// トグルグループの中から選択された項目を取得し、ログ出力するクラス。
/// 実験開始前の選択肢入力や条件選択UIなどで使用される。
/// </summary>
public class SubmitButton : MonoBehaviour
{
    /// <summary>
    /// UI上のトグルグループへの参照（Inspectorでアサイン）
    /// </summary>
    public ToggleGroup toggleGroup;

    void Start()
    {
        // フレームレート制御（Unityの標準設定）
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        // 更新処理なし（ボタン操作はonClickで処理）
    }

    /// <summary>
    /// ボタンがクリックされたときに実行される処理。
    /// アクティブなトグルを取得し、そのラベルをログ出力する。
    /// </summary>
    public void onClick()
    {
        // 選択されているトグルの子要素（"Label"）テキストを取得
        string selectedLabel = toggleGroup.ActiveToggles()
            .First()
            .GetComponentsInChildren<Text>()
            .First(t => t.name == "Label")
            .text;

        Debug.Log("selected " + selectedLabel); // ログに表示
    }
}
