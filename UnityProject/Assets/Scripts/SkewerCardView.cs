using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 串上のアイコン。ドラッグして手札に戻せる。
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(Image))]
public class SkewerCardView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private MaterialData materialData;
    private SkewerController skewerController;
    
    private Transform originalParent;
    private CanvasGroup canvasGroup;
    private Transform canvasTransform;
    private int originalSiblingIndex;

    public MaterialData MaterialData => materialData;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            canvasTransform = canvas.transform;
        }
    }

    /// <summary>
    /// 初期化：どの素材か、どの串に属しているか
    /// </summary>
    public void Setup(MaterialData mat, SkewerController controller)
    {
        materialData = mat;
        skewerController = controller;

        var image = GetComponent<Image>();
        if (image != null && mat.icon != null)
        {
            image.sprite = mat.icon;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalSiblingIndex = transform.GetSiblingIndex();

        // 最前面に移動
        transform.SetParent(canvasTransform);
        canvasGroup.blocksRaycasts = false;

        Debug.Log($"串からドラッグ開始: {materialData?.materialName}");
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // ドロップ処理が成功していればオブジェクトは破棄されているはず
        // まだ親が canvasTransform のままなら、ドロップ失敗 → 串に戻す
        if (transform.parent == canvasTransform)
        {
            ReturnToSkewer();
        }
    }

    /// <summary>
    /// ドラッグキャンセル時：元の串に戻る
    /// </summary>
    public void ReturnToSkewer()
    {
        transform.SetParent(originalParent);
        transform.SetSiblingIndex(originalSiblingIndex);
        transform.localPosition = Vector3.zero;
        Debug.Log($"串に戻る: {materialData?.materialName}");
    }
}
