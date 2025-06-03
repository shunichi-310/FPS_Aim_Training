using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 各ターゲットが命中したかどうかを記録する静的クラス。
/// 配列の初期化やリセット処理を提供。
/// </summary>
public static class hit_flag
{
    /// <summary>
    /// ターゲットの命中状況を記録する配列（1 = 命中, 0 = 未命中）。
    /// Index はターゲット番号（最大12ターゲット）に対応。
    /// </summary>
    public static int[] h_flag = new int[12];

    // クラス初期化時に自動実行される静的コンストラクタ
    static hit_flag()
    {
        ResetHitFlagArray();
    }

    /// <summary>
    /// 命中記録配列をすべて 0 にリセットする。
    /// 新しい試行を開始する前に呼び出す。
    /// </summary>
    public static void ResetHitFlagArray()
    {
        for (int i = 0; i < h_flag.Length; i++)
        {
            h_flag[i] = 0;
        }

        Debug.Log("hit_flag.h_flag 配列をリセットしました。");
    }
}
