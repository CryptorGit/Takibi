using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 串の中身をUIに表示するビュー
/// </summary>
public class SkewerView : MonoBehaviour
{
    public SkewerController skewer;
    public Image[] slots;   // 4個分のImageをInspectorでアサイン

    public Sprite emptySprite;

    /// <summary>
    /// 串の表示を更新する
    /// </summary>
    public void Refresh()
    {
        if (skewer == null || slots == null) return;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null) continue;

            if (i < skewer.materials.Count)
            {
                var mat = skewer.materials[i];
                slots[i].sprite = mat.icon;
                slots[i].enabled = true;
                slots[i].color = Color.white;
            }
            else
            {
                slots[i].sprite = emptySprite;
                slots[i].enabled = emptySprite != null;
                slots[i].color = emptySprite != null ? new Color(1, 1, 1, 0.3f) : Color.clear;
            }
        }

        // デバッグ表示
        if (skewer.materials.Count > 0)
        {
            Debug.Log($"串UI更新: {skewer.materials.Count}個 / 焼きT:{skewer.totalCookTurn} / スコア:{skewer.totalScore}");
        }
    }
}
