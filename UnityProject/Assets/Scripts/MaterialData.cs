using UnityEngine;

[CreateAssetMenu(menuName = "Takibi/MaterialData")]
public class MaterialData : ScriptableObject
{
    public string materialName;
    public float cookTurnPerPiece;  // 例: 0.4, 1.0, 1.2
    public int scorePerPiece;       // 例: 1, 2, 4
    public float spawnWeight;       // 出現しやすさ (比率)
    public Sprite icon;             // 手札用アイコン
}
