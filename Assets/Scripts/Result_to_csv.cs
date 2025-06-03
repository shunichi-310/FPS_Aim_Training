using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using System;
using System.IO;

/// <summary>
/// FPS実験後に、プレイヤーの反応時間・マウス移動データをCSVに出力するクラス。
/// デスクトップに結果ファイルを保存し、50セット終了時にStartシーンに戻る。
/// </summary>
public class Result_to_csv : MonoBehaviour
{
    string DPI;
    string hit_time;
    int hit = 0;
    string sum, sum1, hf, top1;
    float time = 0f;
    int i = 0, j = 1;
    int hitf_cnt = 0;
    int operationCount = 1; // セット内での操作回数（ターゲットごと）
    float startX = 0f, startY = 0f; // 操作開始座標

    public Text sec; // 残り時間表示用UIテキスト

    void Update()
    {
        time += Time.deltaTime;
        i = 5 - (int)time;
        sec.text = "リスタートまで残り " + i.ToString() + " 秒";

        if (i <= 0)
        {
            OP_csv();
        }
    }

    /// <summary>
    /// 実験結果を2つのCSVファイルに出力：
    /// ① 反応時間・命中情報 ② マウス移動記録（1フレームごと）
    /// </summary>
    public void OP_csv()
    {
        // 基本情報取得
        hit = Destroy_obj.hit_count;
        hit_time = String.Join(",", Create_Target.T_array);
        DPI = Info.DPI;
        sum = hit_time + "," + DPI;
        hf = String.Join(",", hit_flag.h_flag);

        // 保存先パス（デスクトップ）
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        string resultFilePath = Path.Combine(desktopPath, "sato_result.csv");

        // ▼ ① 反応時間・命中情報の出力
        using (StreamWriter sw = new StreamWriter(resultFilePath, true))
        {
            sw.WriteLine(hf);   // ヒットフラグ（0/1）
            sw.WriteLine(sum);  // 各ターゲットへの反応時間とDPI
        }

        // ▼ ② マウス軌跡データ出力（1フレームごとのx, y, 距離）
        string mouseFilePath = Path.Combine(desktopPath, "sato_mouse.csv");

        using (StreamWriter sw1 = new StreamWriter(mouseFilePath, true))
        {
            // 初回のみヘッダー追加
            if (FPS.CameraController.ms == 0)
            {
                top1 = String.Join(",", "set_num", "operation_count", "frame_num", "x", "y", "distance_from_start");
                sw1.WriteLine(top1);
            }

            for (int n = 0; n <= FPS.CameraController.ms; n++)
            {
                float xCoord = FPS.CameraController.xpd[n];
                float yCoord = FPS.CameraController.ypd[n];

                // ターゲット命中後に操作カウントをリセット
                if (hitf_cnt < FPS.CameraController.hitflame.Length && FPS.CameraController.hitflame[hitf_cnt] <= n)
                {
                    j = 1;
                    operationCount++;
                    hitf_cnt++;
                    startX = xCoord;
                    startY = yCoord;
                }

                float distanceFromStart = Mathf.Sqrt(Mathf.Pow(xCoord - startX, 2) + Mathf.Pow(yCoord - startY, 2));
                sum1 = String.Join(",", Score.set_cnt, operationCount, j, xCoord - startX, yCoord - startY, distanceFromStart);
                sw1.WriteLine(sum1);
                j++;
            }
        }

        Debug.Log("CSVファイルを出力しました。");

        // 各種状態をリセット
        Destroy_obj.hit_count = 0;
        Shooting.Click_cnt = 0;
        FPS.CameraController.ms = 0;
        operationCount = 1;

        // 実験が50セット終了したらStart画面に戻る
        if (Score.set_cnt == 50)
        {
            SceneManager.LoadScene("Start");
        }
        else
        {
            SceneManager.LoadScene("Play");
        }
    }
}
