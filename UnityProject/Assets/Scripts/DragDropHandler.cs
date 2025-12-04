using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class DragDropHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform originalParent;
    private CanvasGroup canvasGroup;
    private Transform canvasTransform; // ドラッグ中の親（Canvas直下など）

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        // シーン内のCanvasを探して、ドラッグ中の一時的な親とする
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            canvasTransform = canvas.transform;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;

        // 1. レイアウト計算から外すために親を変更（最前面に描画）
        transform.SetParent(canvasTransform);
        
        // 2. ドラッグ中はマウス入力を透過させる（背後のドロップゾーンが検知できるように）
        canvasGroup.blocksRaycasts = false; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        // マウス位置に追従
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // ドロップ処理が成功していれば、このオブジェクトは既に破棄されているか、処理済み。
        // まだ親が canvasTransform のままなら、ドロップ失敗とみなして元の場所に戻す。
        if (transform.parent == canvasTransform)
        {
            ReturnToHand();
        }
    }

    public void ReturnToHand()
    {
        transform.SetParent(originalParent);
        // レイアウト崩れを防ぐため位置リセットなどはLayoutGroupに任せるが、念のため
        transform.localPosition = Vector3.zero;
    }
}
