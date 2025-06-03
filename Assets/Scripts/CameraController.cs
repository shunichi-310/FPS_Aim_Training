using System.Collections;
using UnityEngine;

namespace FPS
{
    /// <summary>
    /// プレイヤー視点のカメラ操作を管理するクラス。
    /// マウス入力に基づき回転処理を行い、クリック時にカメラを初期位置へリセットする。
    /// FPSゲームの照準訓練研究用に開発。
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        private Quaternion initialRotation; // 初期回転（リセット用）

        [Range(0.1f, 20f)] public float lookSensitivity = 5f;   // 視点移動感度
        [Range(0.0f, 1f)] public float lookSmooth = 0.0f;       // 視点移動のスムージング
        public Vector2 MinMaxAngle = new Vector2(-65, 65);      // 視点の上下角度制限

        private float yRot;             // マウスX入力の累積回転
        private float xRot;             // マウスY入力の累積回転
        private float currentYRot;      // スムージング後のY回転
        private float currentXRot;      // スムージング後のX回転
        private float yRotVelocity;     // スムージング補助変数（Y）
        private float xRotVelocity;     // スムージング補助変数（X）

        // マウスの入力データ（解析用）
        public static float Aim;
        public static float[] xpd = new float[6000]; // X軸移動データ
        public static float[] ypd = new float[6000]; // Y軸移動データ
        public static int ms = 0;                    // フレームインデックス

        public static int[] hitflame = new int[11];  // ヒット時のフレーム記録
        int hitflame_cnt = 0;

        float xp = 0, temx = 0; // 一時的なX座標記録
        float yp = 0, temy = 0; // 一時的なY座標記録

        [SerializeField] bool m_cursor = false; // カーソル表示設定
        public bool isReset = false;            // リセット中かどうか
        private Coroutine resetCoroutine;

        void Start()
        {
            lookSensitivity = Aim; // グローバル変数から感度を取得

            // カーソル表示とロックの設定
            Cursor.visible = m_cursor;
            Cursor.lockState = m_cursor ? CursorLockMode.None : CursorLockMode.Locked;

            // FPS安定化設定
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;

            // 初期回転角を保存
            initialRotation = transform.rotation;
        }

        void Update()
        {
            if (isReset) return;

            // マウス入力から視点回転を加算
            yRot += Input.GetAxis("Mouse X") * lookSensitivity;
            xRot -= Input.GetAxis("Mouse Y") * lookSensitivity;

            // 入力記録：ターゲット表示中のみ
            if (ms < xpd.Length && ms < ypd.Length && Create_Target.TargetVisible)
            {
                xp = Input.GetAxis("Mouse X");
                yp = Input.GetAxis("Mouse Y");
                xpd[ms] = xp + temx;
                temx = xpd[ms];
                ypd[ms] = yp + temy;
                temy = ypd[ms];
                ms++;
            }

            // 視点の角度制限とスムージング処理
            xRot = Mathf.Clamp(xRot, MinMaxAngle.x, MinMaxAngle.y);
            currentXRot = Mathf.SmoothDamp(currentXRot, xRot, ref xRotVelocity, lookSmooth);
            currentYRot = Mathf.SmoothDamp(currentYRot, yRot, ref yRotVelocity, lookSmooth);

            transform.rotation = Quaternion.Euler(currentXRot, currentYRot, 0);

            // クリック時の処理（ターゲット命中）
            if (Input.GetMouseButtonDown(0) && Create_Target.TargetVisible)
            {
                if (hitflame_cnt < hitflame.Length)
                {
                    hitflame[hitflame_cnt] = ms;
                    Debug.Log($"Hit recorded: {ms} at index {hitflame_cnt}");
                    hitflame_cnt++;
                }
                else
                {
                    Debug.LogWarning("hitflame配列が上限に達しています。");
                }

                // カメラリセットとクールダウン処理
                ResetCameraOrientation();
                isReset = true;
                if (resetCoroutine != null) StopCoroutine(resetCoroutine);
                resetCoroutine = StartCoroutine(ResetCameraAfterDelay(0.5f));
                Create_Target.TargetVisible = false;
            }
        }

        /// <summary>
        /// 指定時間後にカメラリセットを解除するコルーチン
        /// </summary>
        private IEnumerator ResetCameraAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            isReset = false;
            resetCoroutine = null;
        }

        /// <summary>
        /// カメラの回転を初期状態にリセット
        /// </summary>
        public void ResetCameraOrientation()
        {
            transform.rotation = initialRotation;
            Debug.Log("Camera rotation reset called.");

            xRot = yRot = 0f;
            currentXRot = currentYRot = 0f;
        }

        /// <summary>
        /// ヒットフレーム配列とカウンタをリセット
        /// </summary>
        public void ResetHitflameArray()
        {
            for (int i = 0; i < hitflame.Length; i++)
            {
                hitflame[i] = 0;
            }
            hitflame_cnt = 0;
            Debug.Log("hitflame array and counter reset.");
        }
    }
}
