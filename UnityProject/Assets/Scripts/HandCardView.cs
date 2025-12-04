using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HandCardView : MonoBehaviour
{
    public Image icon;
    
    [Header("ラベル（どちらか一方をアサイン）")]
    public TextMeshProUGUI labelTMP;   // TextMeshPro の場合
    public Text labelText;              // 通常の Text の場合

    // 外部公開用のプロパティ（ドラッグ＆ドロップで使用）
    public MaterialData MaterialData { get; private set; }

    public void Setup(MaterialData mat)
    {
        MaterialData = mat;

        if (icon != null && mat.icon != null)
            icon.sprite = mat.icon;
        
        // TextMeshPro か 通常Text のどちらかにセット
        if (labelTMP != null)
            labelTMP.text = mat.materialName;
        if (labelText != null)
            labelText.text = mat.materialName;
    }

    // 後方互換性のためのオーバーロード（controllerは使用しない）
    public void Setup(MaterialData mat, HandController controller)
    {
        Setup(mat);
    }
}
