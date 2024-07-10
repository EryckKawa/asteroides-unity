using System.Collections;
using UnityEngine;
using TMPro;

public class TextChanger : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private string[] phrases;
    [SerializeField] private float interval = 5f;

    private void Start()
    {
        if (textComponent == null)
        {
            textComponent = GetComponent<TextMeshProUGUI>();
        }

        StartCoroutine(ChangeTextRoutine());
    }

    private IEnumerator ChangeTextRoutine()
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
