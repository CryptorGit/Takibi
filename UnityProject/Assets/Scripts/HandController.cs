using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public MaterialDatabase materialDatabase;
    public int handLimit = 20;
    public int drawPerTurn = 3;
    public int initialDraw = 5;  // 初手のドロー数

    // UI表示用
    public Transform handPanel;          // HandPanel の Transform
    public GameObject handCardPrefab;    // HandCard プレハブ

    // 串
    public SkewerController skewerController;  // 現在の串

    // 実際に持っている素材の一覧
    public List<MaterialData> hand = new();

    /// <summary>
    /// ゲーム開始時の初期化
    /// </summary>
    public void Init()
    {
        hand.Clear();
        RefreshHandView();
    }

    /// <summary>
    /// 初手ドロー（5枚）
    /// </summary>
    public void DrawInitialHand(GameManager gameManager)
    {
        Debug.Log($"初手ドロー: {initialDraw}枚");
        DrawMaterials(initialDraw);
        CheckGameOver(gameManager);
    }

    /// <summary>
    /// ドローフェーズ（ターン2以降のターン開始時に呼ばれる）
    /// </summary>
    public void DrawAndCheckGameOver(GameManager gameManager)
    {
        DrawMaterials(drawPerTurn);
        CheckGameOver(gameManager);
    }

    private void CheckGameOver(GameManager gameManager)
    {
        if (hand.Count > handLimit)
        {
            Debug.Log($"手札が上限を超えました ({hand.Count}/{handLimit})。Game Over。");
            if (gameManager != null)
            {
                gameManager.TriggerGameOver();
            }
        }
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

    /// <summary>
    /// 手札カードがクリックされた時：串に素材を追加
    /// </summary>
    public void OnHandCardClicked(MaterialData mat)
    {
        if (skewerController == null) return;

        // 串に追加（上限チェック込み）
        if (skewerController.AddMaterial(mat))
        {
            hand.Remove(mat);
            RefreshHandView();
        }
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
}
