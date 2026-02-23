using UnityEngine;
using UnityEngine.UI;
using Utils.Singleton;
using TMPro;
using System.Collections;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class DialogueManager : Singleton<DialogueManager>
{
    [Header("Tutorial!")]
    [SerializeField] private GameObject tutorial1Ligar;
    [SerializeField] private GameObject tutorial2Ligar;
    [SerializeField] private GameObject continue2;
    
    [SerializeField] private GameObject[] dialogueManagers;
    [SerializeField] private int numberDialogueManager;
    [SerializeField] private PlayerBase player1;
    [SerializeField] private PlayerBase player2;
    [Header("DIALOGUE")]
    [SerializeField] private Button b_pular;
    [SerializeField] private Button b_continuarFalas;
    [SerializeField] private DialogueDetails[] DialogueDetailsArray;
    [SerializeField] private DialogueSpeaker[] DialogueSpeakerArray;

    private int numeroFala = 0;
    private float tempo = 0.0f;
    private float tempoLetras = 0.0f;
    private int letra = 1;
    private bool falasRodando;
    private bool podeClicar;
    private bool dialogueOver;
    private string dialogue;

    [Header("UI")]
    [SerializeField] private GameObject DialoguePanel;
    [SerializeField] private CanvasGroup cg_DialoguePanel;
    [SerializeField] private TextMeshProUGUI falaTexto;
    [SerializeField] private TextMeshProUGUI NomeFalante_Text;
    [SerializeField] private Image Speaker_Image;

    [Header("DIALOGUE SPECIFICS")]
    [SerializeField] public string nextScene;
    [SerializeField] GameObject PretoExtra;
    [SerializeField] GameObject BrancoExtra;
    private BlackScreenController _blackScreenController => BlackScreenController.I;
    //private PausePanel _pausePanel => PausePanel.I;
    
    private new void Awake()
    {
        
        numeroFala = 0;
        falaTexto.text = "";

        
        DialoguePanel.SetActive(false);
        cg_DialoguePanel.alpha = 0;
    }
    

    private void OnEnable()
    {
        b_continuarFalas.onClick.AddListener(MouseClick);
    }


    private void Start()
    {
        if (numberDialogueManager == 0)
        {
            //_audioManager.FadeInMusic("mainmusic");
        }
        StartCoroutine(ComecarFalas());
        
    }

    void Update()
    {
        DialogueControl();
    }

    private IEnumerator ComecarFalas()
    {
        yield return new WaitForSeconds(1.4f);
        DialoguePanel.SetActive(true);
        cg_DialoguePanel.DOFade(1, 0.3f).OnComplete(() =>
        {
            b_pular.onClick.AddListener(() => DialogueOver());
            falasRodando = true;
        });
        falasRodando = true;
    }

    void DialogueControl()
    {
        if (falasRodando)
        {
            tempo += Time.deltaTime;
        }

        if (numeroFala == DialogueDetailsArray.Length)
        {
            podeClicar = false;
            DialogueOver();
        }
        else
        {
            if (tempo >= DialogueDetailsArray[numeroFala].pauseBeforeDialogue)
            {
                ScriptFalas();
            }
            else
            {
                podeClicar = false;
                falaTexto.text = "";
            }
        }
    }

    void ScriptFalas()
    {
        //ControleDosObjetosEspecificos(falas);

        string speaker = DialogueSpeakerArray[DialogueDetailsArray[numeroFala].speakerID].speaker;
        if (string.IsNullOrEmpty(speaker))
        {
            Speaker_Image.enabled = false;
        }
        else
        {
            Speaker_Image.enabled = true;
            Speaker_Image.sprite = DialogueSpeakerArray[DialogueDetailsArray[numeroFala].speakerID].speakerSprite;
        }

        string line = DialogueDetailsArray[numeroFala].dialogue;

        NomeFalante_Text.text = speaker;

        LettersOneByOne(line);

        dialogue = line;
        podeClicar = true;

    }

    void MouseClick()
    {
        if (podeClicar && !dialogueOver)
        {
            if (letra != dialogue.Length + 1)
            {
                falaTexto.text = dialogue;
                letra = dialogue.Length + 1;
            }
            else
            {
                if (tempo > 0.4f)
                {
                    if (numeroFala != DialogueDetailsArray.Length)
                    {
                        tempoLetras = 0.0f;
                        letra = 1;
                        tempo = 0.0f;
                        numeroFala++;
                    }
                }
            }
        }
    }

    void LettersOneByOne(string dialogue)
    {
        tempoLetras += Time.deltaTime;

        if (tempoLetras > 0.05 * letra && letra != dialogue.Length + 1)
        {
            falaTexto.text = dialogue.Substring(0, letra);
            letra++;
        }
    }

    

    void DialogueOver()
    {
        if (dialogueOver) return;
        b_continuarFalas.onClick.RemoveAllListeners();
        dialogueOver = true;
        
        if (numberDialogueManager == 0)
        {
            cg_DialoguePanel.DOFade(0, 0.3f).OnComplete(() =>
            {
                player1.EnableInputs();
                player2.EnableInputs();
                tutorial1Ligar.SetActive(true);
            });
        }
        else if (numberDialogueManager == 1)
        {
            cg_DialoguePanel.DOFade(0, 0.3f).OnComplete(() =>
            {
                player1.LigarCanto();
                player2.LigarCanto();
                player1.EnableInputs();
                player2.EnableInputs();
                tutorial2Ligar.SetActive(true);
                tutorial1Ligar.SetActive(false);
            });
        }
        else if (numberDialogueManager == 2)
        {
            cg_DialoguePanel.DOFade(0, 0.3f);
            PretoExtra.SetActive(true);
            PretoExtra.GetComponent<CanvasGroup>().alpha = 0;
            PretoExtra.GetComponent<CanvasGroup>().DOFade(1, 0.6f).OnComplete(() =>
            {
                continue2.SetActive(true);
                AudioManager.I.PlaySfx("windowbreak");
            });
        }
        else if (numberDialogueManager == 3)
        {
            cg_DialoguePanel.DOFade(0, 0.3f);
            _blackScreenController.FadeOutScene("InitialDialogue");
        }
        else if (numberDialogueManager == 4)
        {
            cg_DialoguePanel.DOFade(0, 0.3f);
            AudioManager.I.FadeInMusic("mainmusic2");
            _blackScreenController.FadeOutScene("Level1");
        }
        else if (numberDialogueManager == 5)
        {
            BrancoExtra.SetActive(true);
            BrancoExtra.GetComponent<CanvasGroup>().alpha = 0;
            AudioManager.I.FadeOutMusic("mainmusic2");
            BrancoExtra.GetComponent<CanvasGroup>().DOFade(1, 0.6f).SetUpdate(true).OnComplete(() =>
            {
                SceneManager.LoadScene("FinalDialogue");
            });
        }
    }

    #region Extra Classes
    [System.Serializable]
    public class DialogueDetails
    {
        public string dialogue;
        public int speakerID;
        public float pauseBeforeDialogue;
    }
    [System.Serializable]
    public class DialogueSpeaker
    {
        public string speaker;
        public Sprite speakerSprite;
    }
    #endregion
}
