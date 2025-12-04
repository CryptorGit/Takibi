using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 手札エリアのドロップゾーン。
/// 串からドラッグしてきたカードを受け取り、手札に戻す。
/// </summary>
public class HandDropZone : MonoBehaviour, IDropHandler
{
    [Header("参照")]
    public HandController handController;
    public SkewerController skewerController;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("=== HandDropZone.OnDrop 呼ばれた ===");

        GameObject droppedObj = eventData.pointerDrag;
        if (droppedObj == null)
        {
            Debug.LogWarning("OnDrop: droppedObj が null");
            return;
        }

        // 串からのドラッグかチェック
        var skewerCard = droppedObj.GetComponent<SkewerCardView>();
        if (skewerCard != null)
        {
            HandleSkewerCardDrop(skewerCard, droppedObj);
            return;
        }

        // 手札からのドラッグ（何もしない or キャンセル扱い）
        var handCard = droppedObj.GetComponent<HandCardView>();
        if (handCard != null)
        {
            Debug.Log("手札カードを手札にドロップ → 何もしない");
            // DragDropHandler.OnEndDrag で自動的に元の位置に戻る
            return;
        }

        Debug.LogWarning("OnDrop: 不明なオブジェクト");
    }

    private void HandleSkewerCardDrop(SkewerCardView skewerCard, GameObject droppedObj)
    {
        MaterialData mat = skewerCard.MaterialData;
        if (mat == null)
        {
            Debug.LogError("OnDrop: SkewerCardView.MaterialData が null");
            return;
        }

        if (skewerController == null)
        {
            Debug.LogError("OnDrop: skewerController が未設定");
            return;
        }

        if (handController == null)
        {
            Debug.LogError("OnDrop: handController が未設定");
            return;
        }

        // 1. 串から素材を削除
        bool removed = skewerController.RemoveMaterial(mat);
        if (!removed)
        {
            Debug.LogWarning($"串から {mat.materialName} を削除できませんでした");
            return;
        }

        // 2. 手札に追加
        handController.AddToHand(mat);

        // 3. 串のアイコンオブジェクトを破棄
        Destroy(droppedObj);

        Debug.Log($"串 → 手札: {mat.materialName} を戻しました");
    }
}
