using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource; // Referência ao componente AudioSource
    [SerializeField] private string nextSceneName = "Asteroids"; // Nome da próxima cena

    public void StartGame()
    {
        StartCoroutine(PlaySoundAndLoadScene());
    }

    private IEnumerator PlaySoundAndLoadScene()
    {
        audioSource.Play();
        yield return new WaitForSecondsRealtime(audioSource.clip.length); // Usar WaitForSecondsRealtime para ignorar Time.timeScale
        SceneManager.LoadScene(nextSceneName);
    }

    public void RestartGame()
    {
        StartCoroutine(PlaySoundAndRestartScene());
    }
    
    private IEnumerator PlaySoundAndRestartScene()
    {
        Time.timeScale = 1.0f; // Definir Time.timeScale para 1 antes de esperar
        audioSource.Play();
        yield return new WaitForSecondsRealtime(audioSource.clip.length); // Usar WaitForSecondsRealtime para ignorar Time.timeScale
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
