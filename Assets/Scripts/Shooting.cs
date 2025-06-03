using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの射撃処理を管理するクラス。
/// マウスクリックで中央からレイを飛ばし、ターゲットへの命中判定を行う。
/// 命中状況は実験用のログとして保存される。
/// </summary>
public class Shooting : MonoBehaviour
{
    private Camera mainCamera;      // メインカメラへの参照
    private bool Flag = true;       // 連続クリック防止フラグ
    public static int Click_cnt = 0; // 総クリック数（全体で共有）

    void Start()
    {
        mainCamera = Camera.main; // カメラを取得
    }

    void Update()
    {
        // 左クリック検出
        if (Input.GetMouseButtonDown(0))
        {
            if (Flag)
            {
                // 実験が開始されている状態ならクリックをカウント
                if (Create_Target.Stt == true)
                {
                    Click_cnt++;
                }

                // 画面中央からレイを飛ばす（照準位置）
                Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    // ターゲットに命中した場合
                    if (hit.collider.CompareTag("Target"))
                    {
                        Debug.Log("Target hit!");
                        Destroy_obj.hit_count++;                             // 命中数加算
                        Destroy(hit.collider.gameObject);                   // ターゲット削除
                        Create_Target.Create = true;                        // 次ターゲット生成フラグ
                        hit_flag.h_flag[Create_Target.Create_count] = 1;   // 命中記録（1 = 命中）
                    }
                    // 壁に命中した場合（研究で利用している場合）
                    else if (hit.collider.CompareTag("Wall"))
                    {
                        Debug.Log("Wall hit!");
                        Wall_coll wallColl = hit.collider.GetComponent<Wall_coll>();
                        if (wallColl != null)
                        {
                            wallColl.HandleCollisionWithTarget(); // 壁との衝突時の処理呼び出し
                        }
                    }
                }

                // フラグをOFF（連続クリック防止）
                Flag = false;
            }
        }
        else
        {
            // マウスボタンが離されたらフラグをリセット
            Flag = true;
        }
    }
}
