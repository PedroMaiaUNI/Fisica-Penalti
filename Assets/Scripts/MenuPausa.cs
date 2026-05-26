using UnityEngine;

public class MenuPausa : MonoBehaviour
{
    public GameObject pausaUI;
    public GameObject selecaoNivelUI;
    public GameObject inicialUI;

    // Abrir pausa
    public void AbrirPausa()
    {
        pausaUI.SetActive(true);
        selecaoNivelUI.SetActive(false);

        Time.timeScale = 0f;
    }

    // Continuar jogo
    public void ContinuarJogo()
    {
        pausaUI.SetActive(false);
        selecaoNivelUI.SetActive(true);

        Time.timeScale = 1f;
    }

    // Abrir opções
    public void AbrirOpcoes()
    {
        Debug.Log("Abrir opções");
    }

    // Voltar para o menu inicial
    public void VoltarMenuInicial()
    {
        pausaUI.SetActive(false);
        selecaoNivelUI.SetActive(false);
        inicialUI.SetActive(true);

        Time.timeScale = 1f;
    }
}