using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.InputSystem; // Necessário para Unity 6

public class GerenciadorUI : MonoBehaviour
{
    [Header("Vídeo de Introdução")]
    public RawImage imagemDoVideo; 
    public VideoPlayer reprodutorVideo; 

    [Header("Referências")]
    public Canvas meuCanvas;
    public Sprite imagemDeFundoSprite; 

    [Header("Estilo do Menu Central")]
    public Vector2 tamanhoDoPainel = new Vector2(400, 600);
    public Color corDoPainel = new Color(0, 0, 0, 0.7f);
    public Color corDoBotao = new Color(0.2f, 0.2f, 0.25f);
    public Color corDoTexto = Color.white;
    public int spacingEntreElementos = 20;

    [Header("Tela a ser carregada")]
    public string loadScene;

    private readonly Vector2 refResolution = new Vector2(1920, 1080);
    private GameObject painelMenuReferencia; 

    void Start()
    {
        AjustarCanvasScaler();
        ConstruirMenu();
        
        if (reprodutorVideo != null)
        {
            IniciarIntroducao();
        }
    }

    void IniciarIntroducao()
    {
        if(painelMenuReferencia != null) painelMenuReferencia.SetActive(false);
        reprodutorVideo.Prepare();
        reprodutorVideo.loopPointReached += FinalizarVideo;
        reprodutorVideo.Play();
    }

    void FinalizarVideo(VideoPlayer vp)
    {
        //if (imagemDoVideo != null) imagemDoVideo.gameObject.SetActive(false);
        SceneManager.LoadScene(loadScene);
    }

    void Update()
    {
        // Pular vídeo no Novo Input System (Unity 6)
        if (imagemDoVideo != null && imagemDoVideo.gameObject.activeSelf)
        {
            if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
            {
                FinalizarVideo(reprodutorVideo);
            }
        }
    }

    void AjustarCanvasScaler()
    {
        var scaler = meuCanvas.GetComponent<CanvasScaler>();
        if (scaler == null) scaler = meuCanvas.gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = refResolution;
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;
    }

    void ConstruirMenu()
    {
        if (imagemDeFundoSprite != null) CriarImagemDeFundoCobertura(imagemDeFundoSprite);
        painelMenuReferencia = CriarPainel("PainelMenu", meuCanvas.transform);
        CriarTexto("MENU PRINCIPAL", painelMenuReferencia.transform, 50);
        CriarTexto("SUPER CHUTE F.C.", painelMenuReferencia.transform, 30);
        CriarBotao("COMEÇAR", painelMenuReferencia.transform, () => SceneManager.LoadScene("Jogo"));
        CriarBotao("OPÇÕES", painelMenuReferencia.transform, () => Debug.Log("Opções"));
        CriarBotao("SAIR", painelMenuReferencia.transform, () => Application.Quit());
    }

    GameObject CriarPainel(string nome, Transform pai) {
        GameObject obj = new GameObject(nome);
        obj.transform.SetParent(pai, false);
        var img = obj.AddComponent<Image>();
        img.color = corDoPainel;
        var rect = obj.GetComponent<RectTransform>();
        rect.sizeDelta = tamanhoDoPainel;
        var layout = obj.AddComponent<VerticalLayoutGroup>();
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.spacing = spacingEntreElementos;
        return obj;
    }

    void CriarTexto(string conteudo, Transform pai, int tamanho) {
        GameObject obj = new GameObject("Texto_" + conteudo);
        obj.transform.SetParent(pai, false);
        var txt = obj.AddComponent<Text>();
        txt.text = conteudo;
        txt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        txt.fontSize = tamanho;
        txt.color = corDoTexto;
        txt.alignment = TextAnchor.MiddleCenter;
        obj.GetComponent<RectTransform>().sizeDelta = new Vector2(tamanhoDoPainel.x, tamanho * 1.5f);
    }

    void CriarBotao(string rotulo, Transform pai, UnityEngine.Events.UnityAction acao) {
        GameObject objBotao = new GameObject("Botao_" + rotulo);
        objBotao.transform.SetParent(pai, false);
        var img = objBotao.AddComponent<Image>();
        img.color = corDoBotao;
        var btn = objBotao.AddComponent<Button>();
        btn.onClick.AddListener(acao);
        var rect = objBotao.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(tamanhoDoPainel.x * 0.8f, 60);
        GameObject objTexto = new GameObject("TextoBotao");
        objTexto.transform.SetParent(objBotao.transform, false);
        var txt = objTexto.AddComponent<Text>();
        txt.text = rotulo;
        txt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        txt.alignment = TextAnchor.MiddleCenter;
        txt.color = corDoTexto;
        var rectTxt = objTexto.GetComponent<RectTransform>();
        rectTxt.anchorMin = Vector2.zero; rectTxt.anchorMax = Vector2.one; rectTxt.sizeDelta = Vector2.zero;
    }

    void CriarImagemDeFundoCobertura(Sprite spriteImagem) {
        GameObject fundo = new GameObject("BackgroundImagem");
        fundo.transform.SetParent(meuCanvas.transform, false);
        fundo.transform.SetAsFirstSibling();
        var img = fundo.AddComponent<Image>();
        img.sprite = spriteImagem;
        var rect = fundo.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero; rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero; rect.offsetMax = Vector2.zero;
    }
}
