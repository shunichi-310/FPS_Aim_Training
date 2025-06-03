using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 実験やゲーム終了後に「Start」シーンへ戻る処理を行うクラス。
/// UIボタンに割り当てて使用する。
/// </summary>
public class SceneToStart : MonoBehaviour
{
    /// <summary>
    /// ボタンがクリックされた際に呼び出されるメソッド。
    /// Startシーンへ遷移し、クリック数を初期化する。
    /// </summary>
    public void onClick()
    {
        SceneManager.LoadScene("Start"); // Startシーンに戻る
        Shooting.Click_cnt = 0;          // クリックカウントのリセット
    }
}
