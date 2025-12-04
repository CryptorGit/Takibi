using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkewerController : MonoBehaviour
{
    public const int MaxMaterials = 4;  // 串に刺せる素材の上限

    public List<MaterialData> materials = new();
    public int totalCookTurn;
    public int totalScore;

    // --- UI表示用 ---
    [Header("UI Reference")]
    public Transform skewerPanel;       // 串の中身を表示する親パネル (Horizontal Layout Group)
    public GameObject iconPrefab;       // 具材のアイコンを表示するプレハブ
    // ----------------

    public bool IsEmpty => materials.Count == 0;
    public bool IsFull => materials.Count >= MaxMaterials;

    /// <summary>
    /// 素材を串に追加する。上限4個まで。
    /// </summary>
    public bool AddMaterial(MaterialData mat)
    {
        if (IsFull)
        {
            Debug.Log("串がいっぱいです（最大4個）");
            return false;
        }

        materials.Add(mat);
        Recalculate();
        Debug.Log($"串に {mat.materialName} を追加。現在 {materials.Count}/{MaxMaterials} 個。");
        
        // 見た目を更新
        RefreshSkewerView();
        return true;
    }

    public void Clear()
    {
        materials.Clear();
        totalCookTurn = 0;
        totalScore = 0;
        
        // 見た目を更新
        RefreshSkewerView();
    }

    /// <summary>
    /// 素材を串から削除する（手札に戻す時など）
    /// </summary>
    public bool RemoveMaterial(MaterialData mat)
    {
        if (materials.Contains(mat))
        {
            materials.Remove(mat);
            Recalculate();
            Debug.Log($"串から {mat.materialName} を削除。現在 {materials.Count}/{MaxMaterials} 個。");
            // 注意: RefreshSkewerView()はここでは呼ばない（ドロップ処理で個別にDestroyするため）
            return true;
        }
        return false;
    }

    /// <summary>
    /// 串UIを描画更新
    /// </summary>
    private void RefreshSkewerView()
    {
        if (skewerPanel == null)
        {
            Debug.LogWarning("SkewerController: skewerPanel が未設定です！");
            return;
        }
        if (iconPrefab == null)
        {
            Debug.LogWarning("SkewerController: iconPrefab が未設定です！");
            return;
        }

        // 1. 今ある表示を全消し
        foreach (Transform child in skewerPanel)
        {
            Destroy(child.gameObject);
        }

        // 2. 現在のリストの内容に合わせて生成
        foreach (var mat in materials)
        {
            var go = Instantiate(iconPrefab, skewerPanel);
            
            // SkewerCardView があれば初期化（ドラッグ対応）
            var skewerCardView = go.GetComponent<SkewerCardView>();
            if (skewerCardView != null)
            {
                skewerCardView.Setup(mat, this);
            }
            else
            {
                // 旧来のImage直接設定（SkewerCardViewがない場合のフォールバック）
                var image = go.GetComponent<Image>();
                if (image != null && mat.icon != null)
                {
                    image.sprite = mat.icon;
                }
                else
                {
                    Debug.LogWarning($"アイコン表示失敗: image={image != null}, icon={mat.icon != null}");
                }
            }
        }

        Debug.Log($"串UI更新: {materials.Count}個 / 焼きT:{totalCookTurn} / スコア:{totalScore}");
    }

    public void Recalculate()
    {
        // 焼きターン計算
        float cookSum = 0f;
        foreach (var m in materials)
        {
            cookSum += m.cookTurnPerPiece;
        }
        totalCookTurn = Mathf.FloorToInt(cookSum);
        if (totalCookTurn < 0) totalCookTurn = 0;

        // 基本スコア
        int baseScore = 0;
        foreach (var m in materials)
        {
            baseScore += m.scorePerPiece;
        }

        // コンボ（連続同一素材）
        int comboBonus = 0;
        int streak = 1;
        for (int i = 1; i < materials.Count; i++)
        {
            if (materials[i] == materials[i - 1])
            {
                streak++;
            }
            else
            {
                if (streak >= 2)
                {
                    comboBonus += (streak - 1) * materials[i - 1].scorePerPiece;
                }
                streak = 1;
            }
        }
        if (streak >= 2)
        {
            comboBonus += (streak - 1) * materials[^1].scorePerPiece;
        }

        totalScore = baseScore + comboBonus;
    }
}
