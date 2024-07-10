using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<AudioClip> audioClips; // Adicione seus sons aqui no Inspector da Unity
    private AudioSource audioSource1;
    private AudioSource audioSource2;
    private int currentClipIndex = 0;
    public float crossFadeTime = 3.0f; // Tempo de transição entre as músicas
    public float targetVolume = 0.3f; // Volume desejado para as músicas

    void Start()
    {
        // Cria dois AudioSources para a transição
        audioSource1 = gameObject.AddComponent<AudioSource>();
        audioSource2 = gameObject.AddComponent<AudioSource>();

        // Embaralha a lista de clipes de áudio
        Shuffle(audioClips);

        // Toca a primeira música
        audioSource1.clip = audioClips[currentClipIndex];
        audioSource1.volume = targetVolume; // Define o volume inicial
        audioSource1.Play();
        StartCoroutine(CrossFadeMusic());
    }

    IEnumerator CrossFadeMusic()
    {
        while (true)
        {
            // Espera a música terminar
            yield return new WaitForSeconds(audioSource1.clip.length - crossFadeTime);

            // Incrementa o índice e retorna ao primeiro se tiver passado do último
            currentClipIndex = (currentClipIndex + 1) % audioClips.Count;

            // Configura a próxima música no segundo AudioSource e toca
            audioSource2.clip = audioClips[currentClipIndex];
            audioSource2.volume = 0; // Começa com volume zero para crossfade
            audioSource2.Play();

            // Transição suave do volume
            float startTime = Time.time;
            while (Time.time < startTime + crossFadeTime)
            {
                float elapsed = Time.time - startTime;
                audioSource1.volume = Mathf.Lerp(targetVolume, 0, elapsed / crossFadeTime);
                audioSource2.volume = Mathf.Lerp(0, targetVolume, elapsed / crossFadeTime);
                yield return null;
            }

            // Troca os AudioSources para a próxima transição
            AudioSource temp = audioSource1;
            audioSource1 = audioSource2;
            audioSource2 = temp;
        }
    }

    public void StopMusic()
    {
        audioSource1.Stop();
        audioSource2.Stop();
    }

    void Shuffle(List<AudioClip> list)
    {
        int n = list.Count;
        for (int i = 0; i < n - 1; i++)
        {
            int r = i + Random.Range(0, n - i);
            AudioClip t = list[r];
            list[r] = list[i];
            list[i] = t;
        }
    }
}
