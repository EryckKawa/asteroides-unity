using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;
    public GameObject comboVisualizer;

    private int score;
    private int combo;
    private float comboResetTime = 5f; // Tempo em segundos para resetar o combo
    private float comboTimer;
    private int multiplier = 1;
    private Tween comboTween;

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
        if (comboTween != null)
        {
            comboTween.Kill(); // Para a animação quando o combo é resetado
            comboVisualizer.transform.localScale = Vector3.one; // Reseta a escala do visualizador de combo
        }
    }

    private void UpdateScoreUI()
    {
        // Formata o score com quatro dígitos, preenchendo com zeros à esquerda
        scoreText.text = "HighScore " + score.ToString("D8");
    }

    private void UpdateComboUI()
    {
        comboText.text = $"Combo: {combo} (x{multiplier})";
        
        // Lista de cores predefinidas para múltiplos de 10
        Color[] comboColors = new Color[]
        {
            Color.white,
            Color.green,
            Color.yellow,
            Color.magenta,
            Color.cyan,
            new Color(1f, 0.5f, 0f), // Laranja
            new Color(0.5f, 0f, 1f), // Roxo
            new Color(0f, 0.5f, 0.5f), // Teal
            Color.gray,
            new Color(0.5f, 0.5f, 0.5f) // Cinza
        };
        
        // Seleciona a cor com base no índice do array de cores
        int colorIndex = combo / 10;
        Color defaultColor = comboColors[Mathf.Clamp(colorIndex, 0, comboColors.Length - 1)];
        
        // Muda a cor do visualizador com base no combo
        float normalizedCombo = (float)combo / 100f;
        Color lerpedColor = Color.Lerp(defaultColor, Color.red, normalizedCombo);
        comboVisualizer.GetComponent<Image>().color = lerpedColor;
    }

    private void AnimateComboVisualizer()
    {
        if (combo % 10 == 0) // Animação a cada 10 combos
        {
            // Adiciona uma animação de pulsação para tornar a animação mais dinâmica
            comboTween = comboVisualizer.transform.DOPunchScale(new Vector3(0.4f, 0.4f, 0), 0.5f, 10, 1).SetLoops(-1, LoopType.Yoyo);
        }
    }
}
