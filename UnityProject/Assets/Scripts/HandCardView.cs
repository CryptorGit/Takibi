using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HandCardView : MonoBehaviour
{
    public Image icon;
    
    [Header("ラベル（どちらか一方をアサイン）")]
    public TextMeshProUGUI labelTMP;   // TextMeshPro の場合
    public Text labelText;              // 通常の Text の場合

    private MaterialData material;
    private HandController handController;

    public void Setup(MaterialData mat, HandController controller)
    {
        material = mat;
        handController = controller;

        if (icon != null && mat.icon != null)
            icon.sprite = mat.icon;
        
        // TextMeshPro か 通常Text のどちらかにセット
        if (labelTMP != null)
            labelTMP.text = mat.materialName;
        if (labelText != null)
            labelText.text = mat.materialName;
    }

    // Button の OnClick から呼ぶ
    public void OnClick()
    {
        handController.OnHandCardClicked(material);
    }
}
