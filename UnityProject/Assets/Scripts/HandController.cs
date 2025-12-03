using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public MaterialDatabase materialDatabase;
    public int handLimit = 20;
    public int drawPerTurn = 3;

    // UI表示用
    public Transform handPanel;          // HandPanel の Transform
    public GameObject handCardPrefab;    // HandCard プレハブ

    // 串
    public SkewerController skewerController;  // 現在の串

    // 実際に持っている素材の一覧
    public List<MaterialData> hand = new();

    // GameManager から最初に呼ぶ用
    public void Init()
    {
        hand.Clear();
        DrawMaterials(drawPerTurn);
        RefreshHandView();
    }

    // 1ターン終了時に呼ぶ用
    public void OnTurnEnded()
    {
        DrawMaterials(drawPerTurn);
        CheckGameOver();
    }

    private void DrawMaterials(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var mat = GetRandomMaterial();
            if (mat != null)
            {
                hand.Add(mat);
                Debug.Log($"手札に {mat.materialName} を追加");
            }
        }

        Debug.Log($"現在の手札枚数: {hand.Count}");
        RefreshHandView();
    }

    // 手札UIを更新
    public void RefreshHandView()
    {
        if (handPanel == null || handCardPrefab == null)
        {
            return;
        }

        // ① 既存の子オブジェクトを全部消す
        foreach (Transform child in handPanel)
        {
            GameObject.Destroy(child.gameObject);
        }

        // ② 手札の中身を1枚ずつ表示
        for (int i = 0; i < hand.Count; i++)
        {
            var mat = hand[i];
            var go = GameObject.Instantiate(handCardPrefab, handPanel);

            var view = go.GetComponent<HandCardView>();
            if (view != null)
            {
                view.Setup(mat, this);
            }
        }
    }

    // 手札カードがクリックされた時
    public void OnHandCardClicked(MaterialData mat)
    {
        if (skewerController == null) return;

        skewerController.AddMaterial(mat);
        hand.Remove(mat);  // 1枚消費（本当は index 指定が安全だが今は簡易で）
        RefreshHandView();
    }

    // 重み付きランダム
    private MaterialData GetRandomMaterial()
    {
        if (materialDatabase == null || materialDatabase.materials.Count == 0)
        {
            Debug.LogWarning("MaterialDatabase が設定されていません");
            return null;
        }

        float totalWeight = 0f;
        foreach (var m in materialDatabase.materials)
        {
            totalWeight += m.spawnWeight;
        }

        float r = Random.Range(0, totalWeight);
        float accum = 0f;
        foreach (var m in materialDatabase.materials)
        {
            accum += m.spawnWeight;
            if (r <= accum)
            {
                return m;
            }
        }
        return materialDatabase.materials[materialDatabase.materials.Count - 1];
    }

    private void CheckGameOver()
    {
        if (hand.Count > handLimit)
        {
            Debug.Log("手札が上限を超えました。Game Over。");
            // 後で GameManager に通知する
        }
    }
}
