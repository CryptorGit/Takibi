# 焚き火パズル v1.0.2 仕様書（ターン仕様アップデート版）

## 0. コンセプト
“焚火の前で、落ち着いて串を作り、素材の管理パズルをまったり楽しむゲーム。”

- チル × パズル
- プレイヤー操作は最小限
- 焼き時間はすべて自動
- 提供は完全自動
- 串は **常に1本**
- ゲームオーバー条件は手札オーバーのみ

---

## 1. ゲームループ（最新仕様）

### 🔥 ターン開始フェーズ（自動）
1. 焼きスロットの `remainingCookTurn--`
2. 0 になった串は提供（スコア加算 → スロット空になる）
3. 空きスロットが確定する

---

### 🔥 ドローフェーズ（自動）
4. 手札を3枚ドロー  
5. 手札が20を超えたらゲームオーバー

---

### 🔥 プレイヤー操作フェーズ
6. 手札素材をドラッグ＆ドロップで「現在の串」に追加  
7. 串に刺せる素材は最大4個  
8. プレイヤーは「End Turn」を押す

---

### 🔥 ターン終了フェーズ（自動）
9. 串に素材がある場合：  
   - 空いている焚き火スロットに自動配置  
   - totalCookTurn → remainingCookTurn 設定  
   - 串（SkewerController）は Clear() で空に戻る  
10. 満席の場合は串は持ち越し  
11. 次のターン開始へ戻る

---

## 2. 素材データ仕様

| 素材       | 焼きターン/個 | スコア/個 | 出現重み |
|-----------|---------------|-----------|----------|
| マシュマロ | 0.4           | 1         | 30       |
| ウインナー | 1.0           | 2         | 30       |
| 団子       | 1.0           | 3         | 25       |
| 豚バラ     | 1.2           | 4         | 15       |

計算式：
```
totalCookTurn = floor( Σ cookTurnPerPiece )
```

---

## 3. 串データ構造

```
Skewer {
  List<MaterialData> materials
  int totalCookTurn
  int remainingCookTurn
  int totalScore
}
```

---

## 4. スコア計算

### 基本スコア
Σ(素材のスコア × 個数)

### コンボボーナス（同一素材連続）
```
(連続数 - 1) × scorePerPiece
```

---

## 5. ゲームオーバー条件
- 手札20枚超過

---

## 6. UI仕様

- 手札パネル（Horizontal）
- HandCard プレハブ（アイコン＋名称）
- 串表示（1本）
- 焚き火スロット4個
- EndTurn ボタン
- スコア表示
- GameOverビュー

---

## 7. Unity スクリプト構成

### ScriptableObject
- MaterialData.cs
- MaterialDatabase.cs

### Game Logic
- GameManager.cs
- TurnController.cs（最新版のターン順序に対応）
- HandController.cs
- SkewerController.cs
- FireSlot.cs

### UI Scripts
- HandCardView.cs
- DragDropHandler.cs
- SkewerView.cs

---

## 8. ターン例

### ターン1開始
- 焼きカウントダウン：なし  
- 提供：なし  
- 手札3枚ドロー

### プレイヤー操作
- 素材を串に刺す  
- EndTurn  

### ターン終了
- 串を焚き火の空きスロットに自動配置  
- 焼きターン：floor(0.4) → 0 → 即提供  

### ターン2開始  
- 焼き更新 → 提供  
- ドロー  
- プレイヤー操作へ  

---

## 9. 実装優先度

1. TurnController の StartTurn / EndTurn を仕様に合わせる  
2. SkewerController で AddMaterial / Clear / Recalculate  
3. FireSlot の SetFromSkewer  
4. 手札UI実装  
5. 手札→串のドラッグ処理  
6. EndTurn の動作確認  
7. 全体テスト  

---

## 10. v1.0.1 → v1.0.2 変更点

- 串は1本に固定  
- ドローをターン開始へ移動  
- 焚き火への自動配置をターン終了に移動  
- 焼き時間更新はターン開始へ統合  
- プレイヤー操作は「刺して EndTurn する」だけ  
- ゲームループ全体を4フェーズ構造に整理  
