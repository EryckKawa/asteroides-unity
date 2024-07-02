using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public Text scoreText;
    public Text comboText;
    public GameObject comboVisualizer;

    private int score;
    private int combo;
    private float comboResetTime = 5f; // Tempo em segundos para resetar o combo
    private float comboTimer;
    private int multiplier = 1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        score = 0;
        combo = 0;
        UpdateScoreUI();
        UpdateComboUI();
    }

    private void Update()
    {
        if (combo > 0)
        {
            comboTimer += Time.deltaTime;
            if (comboTimer >= comboResetTime)
            {
                ResetCombo();
            }
        }
    }

    public void AddScore(int points)
    {
        score += points * multiplier;
        combo++;
        multiplier = combo / 10 + 1; // Aumenta o multiplicador conforme o combo
        comboTimer = 0f; // Reseta o timer ao adicionar pontos
        UpdateScoreUI();
        UpdateComboUI();
        AnimateComboVisualizer();
    }

    public void ResetCombo()
    {
        combo = 0;
        multiplier = 1;
        comboTimer = 0f;
        UpdateComboUI();
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score;
    }

    private void UpdateComboUI()
    {
        comboText.text = "Combo: " + combo + " (x" + multiplier + ")";
        // Muda a cor do visualizador com base no combo
        comboVisualizer.GetComponent<Image>().color = Color.Lerp(Color.white, Color.red, (float)combo / 100);
    }

    private void AnimateComboVisualizer()
    {
        if (combo % 10 == 0) // Animação a cada 10 combos
        {
            comboVisualizer.transform.DOScale(1.5f, 0.2f).OnComplete(() => comboVisualizer.transform.DOScale(1f, 0.2f));
        }
    }
}
