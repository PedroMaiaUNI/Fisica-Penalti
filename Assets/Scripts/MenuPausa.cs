using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    public GameObject pausaUI;

    public void ContinuarJogo()
    {
        Debug.Log("Continuar");
        pausaUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void AbrirOpcoes()
    {
        Debug.Log("Abrir opções");
    }

    public void VoltarMenuInicial()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void SairDoJogo()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
