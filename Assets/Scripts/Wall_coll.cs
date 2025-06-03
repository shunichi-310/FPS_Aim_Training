using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ターゲットが壁に衝突した際の処理を行うクラス。
/// 壁オブジェクトにアタッチして使用する。
/// </summary>
public class Wall_coll : MonoBehaviour
{
    /// <summary>
    /// 他スクリプト（Shootingなど）から呼び出される外部公開用のメソッド。
    /// 壁とターゲットの接触時に、ターゲットを消去して再生成を促す。
    /// </summary>
    public void HandleCollisionWithTarget()
    {
        Debug.Log("wall_coll");

        // 現在のターゲットを削除
        Destroy(Create_Target.obj.gameObject);

        // 次のターゲット生成を許可
        Create_Target.Create = true;

        // 命中失敗として記録する場合（必要に応じて有効化）
        // hit_flag.h_flag[Create_Target.Create_count] = 0;
    }

    /// <summary>
    /// Unityの物理エンジンによる衝突検出。
    /// 他オブジェクトが壁に接触した場合の処理。
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            HandleCollisionWithTarget();
        }
    }
}
