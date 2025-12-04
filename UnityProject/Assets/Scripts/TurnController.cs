using UnityEngine;

public class TurnController : MonoBehaviour
{
    public FireSlot[] fireSlots;  // 3スロットを Inspector から登録
    public HandController handController;
    public SkewerController skewerController;
    public GameManager gameManager;

    private int turnCount = 0;

    public int TurnCount => turnCount;

    /// <summary>
    /// ゲーム開始時に呼ぶ
    /// </summary>
    public void StartGame()
    {
        turnCount = 1;
        Debug.Log($"=== ターン {turnCount} 開始 ===");

        // 初手は焼き進行なし、ドローのみ（5枚）
        if (handController != null)
        {
            handController.DrawInitialHand(gameManager);
        }

        Debug.Log("プレイヤー操作フェーズ: 素材を串に刺してEndTurnを押してください");
    }

    /// <summary>
    /// ターン開始フェーズ（ターン2以降）
    /// 1. 焼きスロットの remainingCookTurn--
    /// 2. 0になった串は提供（スコア加算）
    /// 3. ドローフェーズへ
    /// </summary>
    private void StartNextTurn()
    {
        turnCount++;
        Debug.Log($"=== ターン {turnCount} 開始 ===");

        // gameManager の確認
        if (gameManager == null)
        {
            Debug.LogError("TurnController: gameManager が未設定です！");
        }

        // 1. 焼きスロットのターン進行 & 提供処理
        foreach (var slot in fireSlots)
        {
            if (slot != null)
            {
                slot.ProgressOneTurn(gameManager);
            }
        }

        // 2. ドローフェーズ（3枚）
        if (handController != null)
        {
            handController.DrawAndCheckGameOver(gameManager);
        }

        Debug.Log("プレイヤー操作フェーズ: 素材を串に刺してEndTurnを押してください");
    }

    /// <summary>
    /// ターン終了ボタンから呼ばれる
    /// 1. 串に素材があれば空きスロットに自動配置
    /// 2. 次のターンへ
    /// </summary>
    public void EndTurn()
    {
        // ゲーム開始前は何もしない
        if (turnCount == 0)
        {
            Debug.Log("ゲームがまだ開始されていません");
            return;
        }

        // ゲームオーバー中は何もしない
        if (gameManager != null && gameManager.IsGameOver)
        {
            Debug.Log("ゲームオーバー中です");
            return;
        }

        Debug.Log($"=== ターン {turnCount} 終了 ===");

        // ターン終了フェーズ: 串を焚き火に自動配置
        PlaceSkewerToFireAuto();

        // 次のターン開始
        StartNextTurn();
    }

    /// <summary>
    /// 串を空いている焚き火スロットに自動配置
    /// </summary>
    private void PlaceSkewerToFireAuto()
    {
        if (skewerController == null || skewerController.IsEmpty)
        {
            Debug.Log("串が空のためスキップ");
            return;
        }

        // 空いている FireSlot を探す
        foreach (var slot in fireSlots)
        {
            if (slot != null && slot.IsEmpty)
            {
                slot.SetFromSkewer(skewerController, gameManager);
                Debug.Log($"串を {slot.name} に配置しました（焼きT:{skewerController.totalCookTurn} / スコア:{skewerController.totalScore}）");
                skewerController.Clear();  // Clear() 内で自動的にUI更新される
                return;
            }
        }

        Debug.Log("空きスロットがないため串は持ち越し");
    }
}
