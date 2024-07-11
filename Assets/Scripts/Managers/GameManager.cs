using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private string nextSceneName = "Asteroids";
	[SerializeField] private GameObject gameOverScreen;
	[SerializeField] private AudioSource gameOverMusic;

	private float musicTime;
	private AudioManager audioManager;

	private void Start()
	{
		audioManager = FindObjectOfType<AudioManager>();
	}

	public void StartGame()
	{
		StartCoroutine(PlaySoundAndLoadScene());
	}

	private IEnumerator PlaySoundAndLoadScene()
	{
		audioSource.Play();
		yield return new WaitForSecondsRealtime(audioSource.clip.length);
		SceneManager.LoadScene(nextSceneName);
	}

	public void RestartGame()
	{
		StartCoroutine(PlaySoundAndRestartScene());
	}

	private IEnumerator PlaySoundAndRestartScene()
	{
		Time.timeScale = 1.0f;
		audioSource.Play();
		yield return new WaitForSecondsRealtime(audioSource.clip.length);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void ContinueGame()
	{
		SaveGameState();
		gameOverScreen.SetActive(false);
		gameOverMusic.Stop();

		StartCoroutine(ResumeGame());
	}

	private IEnumerator ResumeGame()
	{
		Time.timeScale = 1.0f;
		yield return new WaitForSecondsRealtime(1f);
		RestoreGameState();
	}

	private void SaveGameState()
	{
		musicTime = audioSource.time;
	}

	private void RestoreGameState()
	{
		audioSource.time = musicTime;
		audioSource.Play();
		audioManager.RestartMusic();
		PlayerLife playerLife = FindObjectOfType<PlayerLife>();
		if (playerLife != null)
		{
			playerLife.ResetLife(3);
		}
	}

	public void TriggerGameOver()
	{
		gameOverScreen.SetActive(true);
		Time.timeScale = 0f;
		audioManager.StopMusic();
		gameOverMusic.Play();
	}
}
