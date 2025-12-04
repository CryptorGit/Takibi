using UnityEngine;
using UnityEngine.EventSystems;

public class SkewerDropZone : MonoBehaviour, IDropHandler
{
    public SkewerController skewerController;
    public HandController handController;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("SkewerDropZone: OnDrop が呼ばれました");

        // ドロップされたオブジェクトを取得
        GameObject droppedObj = eventData.pointerDrag;
        if (droppedObj == null)
        {
            Debug.LogWarning("SkewerDropZone: droppedObj が null です");
            return;
        }

        // HandCardView からデータを取得
        HandCardView cardView = droppedObj.GetComponent<HandCardView>();
        if (cardView == null)
        {
            Debug.LogWarning("SkewerDropZone: HandCardView が見つかりません");
            return;
        }

        MaterialData mat = cardView.MaterialData;
        if (mat == null)
        {
            Debug.LogWarning("SkewerDropZone: MaterialData が null です");
            return;
        }

        if (skewerController == null)
        {
            Debug.LogError("SkewerDropZone: skewerController が未設定です！");
            return;
        }

        if (handController == null)
        {
            Debug.LogError("SkewerDropZone: handController が未設定です！");
            return;
        }

        // 串に追加できるかトライ
        if (skewerController.AddMaterial(mat))
        {
            Debug.Log("ドロップ成功: 串に追加しました");
            
            // 手札データから削除してUI更新
            handController.RemoveFromHand(mat); 

            // ドロップしたカード（ドラッグ中の浮遊物体）を即消す
            Destroy(droppedObj);
        }
        else
        {
            Debug.Log("ドロップ拒否: 串が満杯です");
            // 失敗時は DragDropHandler.OnEndDrag で手元に戻る
        }
    }
}
