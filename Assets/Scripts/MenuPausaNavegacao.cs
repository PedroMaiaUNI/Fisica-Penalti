using UnityEngine;
using UnityEngine.UI;

public class MenuPausaNavegacao : MonoBehaviour
{
    [System.Serializable]
    public class PauseButton
    {
        public Button button;
        public Image image;
        public Sprite normalSprite;
        public Sprite selectedSprite;
        public string action;
    }

    public PauseButton[] buttons;

    public GameObject Inicial;
    public GameObject SelecaoNivel;
    public GameObject Pausa;

    private int currentIndex = 0;

    void Start()
    {
        UpdateSelection();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            currentIndex = (currentIndex + 1) % buttons.Length;
            UpdateSelection();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            currentIndex--;

            if (currentIndex < 0)
                currentIndex = buttons.Length - 1;

            UpdateSelection();
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            ExecuteAction();
        }
    }

    void UpdateSelection()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].image.sprite =
                (i == currentIndex) ? buttons[i].selectedSprite : buttons[i].normalSprite;
        }
    }

    void ExecuteAction()
    {
        switch (buttons[currentIndex].action)
        {
            case "continue":

                Pausa.SetActive(false);
                SelecaoNivel.SetActive(true);

                Time.timeScale = 1f;

                break;

            case "options":

                Debug.Log("Abrir opções");

                break;

            case "exit":

                Pausa.SetActive(false);
                SelecaoNivel.SetActive(false);
                Inicial.SetActive(true);

                Time.timeScale = 1f;

                break;
        }
    }
}