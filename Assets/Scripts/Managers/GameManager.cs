using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public void StartGame()
	{
		// Carrega a cena do jogo
		SceneManager.LoadScene("Asteroids");
	}
	public void RestartGame()
    {
        // Carrega novamente a cena atual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
