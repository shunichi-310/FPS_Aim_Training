using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

/// <summary>
/// マウスDPI（感度比）をUIから選択し、FPSゲーム内のカメラ感度に反映させるクラス。
/// 研究で扱うコントロール・ディスプレイ比（CD比）の切り替えにも使用される。
/// </summary>
public class Info : MonoBehaviour
{
    /// <summary>
    /// ユーザーがDPIを選択するためのDropdown UI
    /// </summary>
    [SerializeField] private Dropdown DPI_dropdown;

    /// <summary>
    /// 選択されたDPIのラベルを保持（例："1.5:1", "3:1", "6:1"）
    /// </summary>
    public static string DPI;

    /// <summary>
    /// 毎フレーム実行され、DPI選択値に応じてCameraController.Aimを設定
    /// </summary>
    void Update()
    {
        switch (DPI_dropdown.value)
        {
            case 0: // CD比が1.5:1（おおよそ20cm/360°）
                DPI = "1.5:1";
                FPS.CameraController.Aim = 1.7f;
                break;

            case 1: // CD比が3:1（約10cm/360°）
                DPI = "3:1";
                FPS.CameraController.Aim = 0.9f;
                break;

            case 2: // CD比が6:1（約5cm/360°）
                DPI = "6:1";
                FPS.CameraController.Aim = 0.4f;
                break;
        }
    }

    /// <summary>
    /// UIの開始ボタンがクリックされたときに呼ばれ、DPI設定をログに出力しPlayシーンへ遷移
    /// </summary>
    public void onClick()
    {
        Debug.Log(DPI); // 現在選択されているDPIを表示
        SceneManager.LoadScene("Play"); // 本編シーンへ遷移
    }
}
