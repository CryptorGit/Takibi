using UnityEngine;
using TMPro;  // ← これを追加（重要）

public class FireSlot : MonoBehaviour
{
    // 仮の残りターン
    public int remainingCookTurn = 0;

    // TextMeshProUGUI を使う
    public TextMeshProUGUI turnText;

    // このスロットが空かどうか
    public bool IsEmpty => remainingCookTurn <= 0;

    // 1ターン進める
    public void ProgressOneTurn()
    {
        if (IsEmpty)
        {
            return;
        }

        remainingCookTurn--;

        if (remainingCookTurn <= 0)
        {
            Provide();
        }

        UpdateView();
    }

    // 提供処理（仮）
    private void Provide()
    {
        Debug.Log($"{name} の串が提供されました！");
        // 後でここでスコア加算やスロット解放を書く
    }

    // 串からスロットにセット
    public void SetFromSkewer(SkewerController skewer)
    {
        if (!IsEmpty) return;

        remainingCookTurn = skewer.totalCookTurn;
        // 後で score は GameManager に渡す
        UpdateView();
    }

    // UI更新
    public void UpdateView()
    {
        if (turnText == null)
        {
            return;
        }

        if (IsEmpty)
        {
            turnText.text = "-";
        }
        else
        {
       	    turnText.text = remainingCookTurn.ToString();
        }
    }
}
