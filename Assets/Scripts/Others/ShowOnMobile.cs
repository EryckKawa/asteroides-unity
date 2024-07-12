using UnityEngine;

public class ShowOnMobile : MonoBehaviour
{
    private void Start()
    {
        // Verifica se o script está rodando em uma plataforma mobile
        bool isMobile = Application.isMobilePlatform;

        // Ativa ou desativa o GameObject baseado na plataforma
        gameObject.SetActive(isMobile);

        // Debug para verificar qual plataforma está sendo detectada
        Debug.Log("Is Mobile Platform: " + isMobile);
    }
}
