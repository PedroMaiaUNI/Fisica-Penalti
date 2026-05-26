using UnityEngine;
using UnityEngine.InputSystem;

public class MenuSelecaoNivel : MonoBehaviour
{
    public GameObject telaInicial;
    public GameObject telaSelecaoNivel;
    public GameObject telaPausa;

    bool pausado = false;

    void Update()
    {
        // ESC volta ao menu inicial
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            VoltarMenuInicial();
        }

        // P abre/fecha pausa
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            AlternarPausa();
        }
    }

    // Abrir/fechar pausa
    public void AlternarPausa()
    {
        pausado = !pausado;

        telaPausa.SetActive(pausado);
        telaSelecaoNivel.SetActive(!pausado);
    }

    // Voltar menu inicial
    public void VoltarMenuInicial()
    {
        telaPausa.SetActive(false);
        telaSelecaoNivel.SetActive(false);
        telaInicial.SetActive(true);
    }

    // Nível 1
    public void AbrirNivel1()
    {
        Debug.Log("Abrir Nível 1");
    }

    // Nível 2
    public void AbrirNivel2()
    {
        Debug.Log("Abrir Nível 2");
    }

    // Nível 3
    public void AbrirNivel3()
    {
        Debug.Log("Abrir Nível 3");
    }

    // Nível Final
    public void AbrirNivelFinal()
    {
        Debug.Log("Abrir Nível Final");
    }
}