using System.Collections;
using UnityEngine;
using TMPro;

public class TextChanger : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private string[] desktopPhrases;
    [SerializeField] private string[] mobilePhrases;
    [SerializeField] private float interval = 5f;

    private void Start()
    {
        if (textComponent == null)
        {
            textComponent = GetComponent<TextMeshProUGUI>();
        }

        // Seleciona as frases baseadas na plataforma
        string[] phrases = Application.isMobilePlatform ? mobilePhrases : desktopPhrases;

        StartCoroutine(ChangeTextRoutine(phrases));
    }

    private IEnumerator ChangeTextRoutine(string[] phrases)
    {
        for (int i = 0; i < phrases.Length; i++)
        {
            textComponent.text = phrases[i];
            yield return new WaitForSeconds(interval);
        }

        // Desabilita o objeto após todas as frases serem exibidas
        gameObject.SetActive(false);
    }
}
