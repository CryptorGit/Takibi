using System.Collections.Generic;
using UnityEngine;

public class SkewerController : MonoBehaviour
{
    public List<MaterialData> materials = new();
    public int totalCookTurn;
    public int totalScore;

    public bool IsEmpty => materials.Count == 0;

    public void AddMaterial(MaterialData mat)
    {
        materials.Add(mat);
        Recalculate();
        Debug.Log($"串に {mat.materialName} を追加。現在 {materials.Count} 個。");
    }

    public void Clear()
    {
        materials.Clear();
        totalCookTurn = 0;
        totalScore = 0;
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
