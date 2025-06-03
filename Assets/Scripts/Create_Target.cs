using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ターゲットの生成と出現タイミングの管理を行うクラス。
/// 実験におけるターゲット提示位置のランダム化と、反応時間の記録を担う。
/// </summary>
public class Create_Target : MonoBehaviour
{
    public static bool Create = true;        // ターゲットを生成するかどうか
    public GameObject Target;                // ターゲットのプレハブ
    public static GameObject obj;            // 現在表示中のターゲットインスタンス
    public static bool Stt = false;          // スタートフラグ
    public static bool TargetVisible = false; // ターゲットが表示されているか（CameraControllerと連携）
    
    float elapsedTime;                       // ターゲット出現からの経過時間
    public static int Create_count = 0;      // 現在生成済みのターゲット数
    bool dainyuu = false;                    // 時間記録処理フラグ
    public static float[] T_array = new float[12]; // 各ターゲットごとの反応時間記録

    // ターゲットの出現位置（12箇所固定）
    private Vector3[] targetPositions = new Vector3[]
    {
        new Vector3(0f, 45f, 50f),
        new Vector3(12.5f, 41.65f, 50f),
        new Vector3(21.65f, 32.5f, 50f),
        new Vector3(25f, 20f, 50f),
        new Vector3(21.65f, 7.5f, 50f),
        new Vector3(12.5f, -1.65f, 50f),
        new Vector3(0f, -5f, 50f),
        new Vector3(-12.5f, -1.65f, 50f),
        new Vector3(-21.65f, 7.5f, 50f),
        new Vector3(-25f, 20f, 50f),
        new Vector3(-21.65f, 32.5f, 50f),
        new Vector3(-12.5f, 41.65f, 50f)
    };

    // まだ使用していない出現位置のインデックス一覧
    private List<int> remainingPositions = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

    void Start()
    {
        // ヒット判定配列をリセット（開始時）
        hit_flag.ResetHitFlagArray();

        StartCoroutine("Coroutine");

        // 初期化
        Create_Target.Create = true;
        Debug.Log("Create_Target.Create の値: " + Create_Target.Create);
    }

    // 0.5秒待ってからターゲット生成を許可する
    private IEnumerator Coroutine()
    {
        yield return new WaitForSeconds(0.5f);
        Stt = true;
        yield break;
    }

    void Update()
    {
        // カメラがリセット中なら処理をスキップ
        FPS.CameraController cameraController = FindObjectOfType<FPS.CameraController>();
        if (cameraController != null && cameraController.isReset)
        {
            return;
        }

        // ターゲットを生成するタイミング
        if (Create == true && Stt == true && Create_count < 12)
        {
            if (dainyuu == true)
            {
                // 前のターゲットに対する反応時間を記録
                T_array[Create_count] = elapsedTime;
                Debug.Log(T_array[Create_count]);
                elapsedTime = 0.0f;
                Create_count++;
                dainyuu = false;

                // カメラの向きを初期状態に戻す
                if (cameraController != null)
                {
                    cameraController.ResetCameraOrientation();
                }
            }

            if (Create_count < 12)
            {
                // 使用済み位置を除いた中からランダム選択
                if (remainingPositions.Count == 0)
                {
                    remainingPositions = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
                }

                int randomIndex = Random.Range(0, remainingPositions.Count);
                int randomPos = remainingPositions[randomIndex];
                remainingPositions.RemoveAt(randomIndex);

                // ターゲットを生成
                obj = Instantiate(Target, targetPositions[randomPos], Quaternion.identity);
                TargetVisible = true;
            }

            Create = false;
        }
        else if (Stt == true && Create_count < 12)
        {
            // ターゲットに反応するまでの時間を計測中
            elapsedTime += Time.deltaTime;
            dainyuu = true;
        }

        // 12回分のターゲット生成が完了したらシーン遷移
        if (Create_count >= 12)
        {
            Stt = false;
            Create_count = 0;
            SceneManager.LoadScene("End"); // 結果表示シーンなどに遷移
        }
    }
}
