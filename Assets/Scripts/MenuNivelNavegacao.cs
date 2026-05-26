using UnityEngine;
using UnityEngine.UI;

public class MenuNivelNavegacao : MonoBehaviour
{
    [System.Serializable]
    public class BotaoMenu
    {
        public Button button;
        public Image image;
        public Sprite normalSprite;
        public Sprite selectedSprite;
        public Sprite pressedSprite;
    }

    public BotaoMenu[] buttons;

    public GameObject Inicial;
    public GameObject SelecaoNivel;

    private int currentIndex = 0;

    void Start()
    {
        UpdateSelection();
    }

    void Update()
    {
        // Baixo
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            currentIndex = (currentIndex + 1) % buttons.Length;
            UpdateSelection();
        }

        // Cima
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            currentIndex--;

            if (currentIndex < 0)
                currentIndex = buttons.Length - 1;

            UpdateSelection();
        }

        // Confirmar
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            PressButton();
        }

        // ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SelecaoNivel.SetActive(false);
            Inicial.SetActive(true);
        }
    }

    void UpdateSelection()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == currentIndex)
            {
                buttons[i].image.sprite = buttons[i].selectedSprite;
                buttons[i].button.Select();
            }
            else
            {
                buttons[i].image.sprite = buttons[i].normalSprite;
            }
        }
    }

    void PressButton()
    {
        StartCoroutine(PressEffect());
    }

    System.Collections.IEnumerator PressEffect()
    {
        buttons[currentIndex].image.sprite = buttons[currentIndex].pressedSprite;

        yield return new WaitForSeconds(0.15f);

        Debug.Log("Abrir nível");
    }
}