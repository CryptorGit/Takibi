using UnityEngine;

public class GameManager : MonoBehaviour
{
    public HandController handController;

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        Debug.Log("ゲーム開始");
        if (handController != null)
        {
            handController.Init();
        }
    }
}
