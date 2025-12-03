using UnityEngine;

public class TurnController : MonoBehaviour
{
    public FireSlot[] fireSlots;
    public HandController handController;  // ← 追加
    public SkewerController skewerController;

    private int turnCount = 0;

    public void EndTurn()
    {
        turnCount++;
        Debug.Log($"=== ターン終了: {turnCount} ===");

        // 1. スロットのターン進行
        foreach (var slot in fireSlots)
        {
            if (slot != null)
            {
                slot.ProgressOneTurn();
            }
        }

        // 2. 手札ドロー
        if (handController != null)
        {
            handController.OnTurnEnded();
        }
    }

    public void PlaceSkewerToFire()
    {
        if (skewerController == null || skewerController.IsEmpty)
        {
            Debug.Log("串が空です");
            return;
        }

        // 空いている FireSlot を探す
        foreach (var slot in fireSlots)
        {
            if (slot.IsEmpty)
            {
                slot.SetFromSkewer(skewerController);
                skewerController.Clear();
                return;
            }
        }

        Debug.Log("空きスロットがありません");
    }
}
