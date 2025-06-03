using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 実験セットの進行管理と、セット回数表示を行うクラス。
/// ユーザーのプレイ回数をカウントしてUIに表示。
/// </summary>
public class Score : MonoBehaviour
{
    /// <summary>
    /// UI表示用のText。現在のセット回数を画面に表示。
    /// </summary>
    public Text t_cnt;

    /// <summary>
    /// ヒット時間の合計（未使用変数、将来の分析用などに活用可能）
    /// </summary>
    public float sum = 0;

    /// <summary>
    /// 平均ヒット時間（未使用、CSVやUI出力に発展可能）
    /// </summary>
    public float ans = 0;

    /// <summary>
    /// 現在のセット回数（静的、全シーンを通して保持）
    /// </summary>
    public static int set_cnt;

    void Start()
    {
        // セット回数を1加算（シーン遷移ごとに1回実行）
        set_cnt++;
        Debug.Log(set_cnt);

        // UIテキストにセット回数を表示
        if (set_cnt >= 50)
        {
            t_cnt.text = "50 回目終了";
        }
        else
        {
            t_cnt.text = set_cnt + " 回目終了";
        }
    }
}
