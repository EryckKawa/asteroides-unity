using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<AudioClip> audioClips;
    private AudioSource audioSource1;
    private AudioSource audioSource2;
    private int currentClipIndex = 0;
    public float crossFadeTime = 3.0f;
    public float targetVolume = 0.3f;

    void Start()
    {
        audioSource1 = gameObject.AddComponent<AudioSource>();
        audioSource2 = gameObject.AddComponent<AudioSource>();

        Shuffle(audioClips);

        audioSource1.clip = audioClips[currentClipIndex];
        audioSource1.volume = targetVolume;
        audioSource1.Play();
        StartCoroutine(CrossFadeMusic());
    }

    IEnumerator CrossFadeMusic()
    {
        while (true)
        {
            yield return new WaitForSeconds(audioSource1.clip.length - crossFadeTime);

            currentClipIndex = (currentClipIndex + 1) % audioClips.Count;

            audioSource2.clip = audioClips[currentClipIndex];
            audioSource2.volume = 0;
            audioSource2.Play();

            float startTime = Time.time;
            while (Time.time < startTime + crossFadeTime)
            {
                float elapsed = Time.time - startTime;
                audioSource1.volume = Mathf.Lerp(targetVolume, 0, elapsed / crossFadeTime);
                audioSource2.volume = Mathf.Lerp(0, targetVolume, elapsed / crossFadeTime);
                yield return null;
            }

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

    public void RestartMusic()
    {
        if (audioSource1.clip != null && !audioSource1.isPlaying)
        {
            audioSource1.Play();
        }
        if (audioSource2.clip != null && !audioSource2.isPlaying)
        {
            audioSource2.Play();
        }
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
