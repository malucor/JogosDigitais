using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    private bool gameWon = false;

    void Update()
    {
        if (gameWon) return;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0)
        {
            gameWon = true;
            uiManager.ShowVictory();
        }
    }
}