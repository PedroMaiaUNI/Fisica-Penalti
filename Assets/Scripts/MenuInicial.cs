using UnityEngine;

public class MenuInicial : MonoBehaviour
{
    public GameObject Inicial;
    public GameObject SelecaoNivel;

    public void Jogar()
    {
        Inicial.SetActive(false);
        SelecaoNivel.SetActive(true);
    }

    public void SairDoJogo()
    {
        Application.Quit();
    }
}