using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("コントローラー参照")]
    public HandController handController;
    public TurnController turnController;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel;

    [Header("ゲーム状態")]
    [SerializeField] private int currentScore = 0;
    [SerializeField] private bool isGameOver = false;

    public int CurrentScore => currentScore;
    public bool IsGameOver => isGameOver;

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        Debug.Log("========== ゲーム開始 ==========");
        
        currentScore = 0;
        isGameOver = false;
        
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        
        UpdateScoreUI();

        if (handController != null)
        {
            handController.Init();
        }

        if (turnController != null)
        {
            turnController.StartGame();
        }
    }

    /// <summary>
    /// スコアを加算する
    /// </summary>
    public void AddScore(int score)
    {
        currentScore += score;
        Debug.Log($"スコア加算: +{score} (合計: {currentScore})");
        UpdateScoreUI();
    }

    /// <summary>
    /// ゲームオーバー処理
    /// </summary>
    public void TriggerGameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Debug.Log($"========== ゲームオーバー！ 最終スコア: {currentScore} ==========");

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {currentScore}";
        }
    }
}
