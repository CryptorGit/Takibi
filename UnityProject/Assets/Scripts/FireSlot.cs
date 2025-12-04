using UnityEngine;
using TMPro;  // ← これを追加（重要）

public class FireSlot : MonoBehaviour
{
    public int remainingCookTurn = 0;
    public int storedScore = 0;  // この串のスコア
    private bool isOccupied = false;  // 串が配置されているか

    public TextMeshProUGUI turnText;

    // このスロットが空かどうか
    public bool IsEmpty => !isOccupied;

    /// <summary>
    /// 1ターン進める（ターン開始時に呼ばれる）
    /// </summary>
    public void ProgressOneTurn(GameManager gameManager)
    {
        // 串が配置されていないなら何もしない
        if (!isOccupied)
        {
            return;
        }

        remainingCookTurn--;

        if (remainingCookTurn <= 0)
        {
            Provide(gameManager);
        }

        UpdateView();
    }

    /// <summary>
    /// 提供処理：スコア加算してスロットを空にする
    /// </summary>
    private void Provide(GameManager gameManager)
    {
        Debug.Log($"{name} の串が提供されました！ スコア: +{storedScore}");
        
        if (gameManager != null)
        {
            gameManager.AddScore(storedScore);
        }
        else
        {
            Debug.LogError("FireSlot.Provide: gameManager が null です！スコアが加算されません！");
        }

        // スロットをクリア
        storedScore = 0;
        remainingCookTurn = 0;
        isOccupied = false;
    }

    /// <summary>
    /// 串からスロットにセット
    /// 焼きターンが0なら即座に提供
    /// </summary>
    public void SetFromSkewer(SkewerController skewer, GameManager gameManager)
    {
        if (!IsEmpty) return;

        isOccupied = true;
        remainingCookTurn = skewer.totalCookTurn;
        storedScore = skewer.totalScore;

        // 焼きターンが0なら即座に提供
        if (remainingCookTurn <= 0)
        {
            Debug.Log($"{name}: 焼きT=0 のため即提供");
            Provide(gameManager);
        }

        UpdateView();
    }

    // UI更新
    public void UpdateView()
    {
        if (turnText == null)
        {
            return;
        }

        if (!isOccupied)
        {
            turnText.text = "-";
        }
        else
        {
       	    turnText.text = remainingCookTurn.ToString();
        }
    }
}
