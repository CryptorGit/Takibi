using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HandCardView : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI label;

    private MaterialData material;
    private HandController handController;

    public void Setup(MaterialData mat, HandController controller)
    {
        material = mat;
        handController = controller;

        if (icon != null && mat.icon != null)
            icon.sprite = mat.icon;
        if (label != null)
            label.text = mat.materialName;
    }

    // Button の OnClick から呼ぶ
    public void OnClick()
    {
        handController.OnHandCardClicked(material);
    }
}
