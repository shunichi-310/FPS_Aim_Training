using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ターゲットオブジェクトにヒットした際の処理を行うクラス。
/// ヒット数のカウント、ターゲットの破壊、次のターゲット生成フラグの設定などを担当。
/// </summary>
public class Destroy_obj : MonoBehaviour
{
    /// <summary>
    /// ターゲットに命中した回数を記録（全体で共有される）
    /// </summary>
    public static int hit_count = 0;

    /// <summary>
    /// 他のコライダーとの接触判定。
    /// タグが "Target" のオブジェクトにヒットした場合の処理を行う。
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        // "Target" タグのオブジェクトに衝突した場合
        if (other.CompareTag("Target"))
        {
            // ヒット回数をカウント
            hit_count++;

            // 自身（ヒット対象オブジェクト）を破壊
            Destroy(this.gameObject);

            // 次のターゲットを生成するフラグを立てる
            Create_Target.Create = true;

            //（任意機能）ヒット状況を配列に記録する場合の処理
            // hit_flag.h_flag[Create_Target.Create_count] = 1;
        }
    }
}
