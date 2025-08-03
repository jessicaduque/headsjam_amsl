using System;
using UnityEngine;
using UnityEngine.UI;
using Utils.Singleton;
using TMPro;
using System.Collections;
using DG.Tweening;

public class DialogueManager : Singleton<DialogueManager>
{
    [Header("DIALOGUE")]
    [SerializeField] private Button b_pular;
    [SerializeField] private DialogueDetails[] DialogueDetailsArray;
    [SerializeField] private DialogueSpeaker[] DialogueSpeakerArray;

    private int numeroFala = 0;
    private float tempo = 0.0f;
    private float tempoLetras = 0.0f;
    private int letra = 1;
    private bool falasRodando;

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

    public event Action DialogueEndEvent; 
    
    // Objetos espec�ficos
    // NENHUM AINDA

    private BlackScreenController _blackScreenController => BlackScreenController.I;
    private AudioManager _audioManager => AudioManager.I;
    //private PausePanel _pausePanel => PausePanel.I;

    private void OnValidate()
    {
        if(cg_DialoguePanel == null)
        {
            cg_DialoguePanel = DialoguePanel.GetComponent<CanvasGroup>();
        }
    }

    private new void Awake()
    {
        b_pular.onClick.AddListener(() => DialogueOver());
        numeroFala = 0;
        falaTexto.text = "";

        DialoguePanel.SetActive(false);
        cg_DialoguePanel.alpha = 0;

        StartCoroutine(ComecarFalas());
    }

    void Update()
    {
        DialogueControl();
    }

    private IEnumerator ComecarFalas()
    {
        yield return new WaitForSeconds(0.4f);
        _audioManager.PlaySfx("DoorKnock");
        DialoguePanel.SetActive(true);
        cg_DialoguePanel.DOFade(1, 0.8f).OnComplete(() => falasRodando = true);
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
            DialogueOver();
        }
        else
        {
            if (numeroFala == 9)
            {
                PretoExtra.GetComponent<CanvasGroup>().DOFade(0, 0.6f);
            }

            if (tempo >= DialogueDetailsArray[numeroFala].pauseBeforeDialogue)
            {
                ScriptFalas();
            }
            else
            {
                falaTexto.text = "";
            }
        }
    }

    void ScriptFalas()
    {

        //ControleDosObjetosEspecificos(falas);

        string speaker = DialogueSpeakerArray[DialogueDetailsArray[numeroFala].speakerID].speaker;
        if (speaker == null || speaker == "???")
        {
            Speaker_Image.enabled = false;
        }
        if(speaker == "Berry")
        {
            _audioManager.PlaySfx("BerryGrowl");
        }
        else
        {
            Speaker_Image.enabled = true;
            Speaker_Image.sprite = DialogueSpeakerArray[DialogueDetailsArray[numeroFala].speakerID].speakerSprite;
        }

        string line = DialogueDetailsArray[numeroFala].dialogue;

        NomeFalante_Text.text = speaker;

        LettersOneByOne(line);

        MouseClick(line);

    }

    void MouseClick(string dialogue)
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.JoystickButton0))
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
        DialogueEndEvent?.Invoke();
        //_blackScreenController.FadeOutScene(nextScene);
    }


    #region Possibly Useful Extra Functions
    //private void OlharParaAlgo()
    //{
    //    if (ondeOlhar != null)
    //    {
    //        if (ondeOlhar.transform.position.x > Player.transform.position.x)
    //        {
    //            Player.transform.localScale = new Vector3(-1, 1, 1);
    //        }
    //        else
    //        {
    //            Player.transform.localScale = new Vector3(1, 1, 1);
    //        }
    //    }
    //}

    //void ControleDosObjetosEspecificos(List<string> falas)
    //{
    //    if (falas[numeroFala] == "EI!" || falas[numeroFala] == "HEY!")
    //    {
    //        LigarObjetosEspecificos("Drag�o");
    //    }
    //    else if (falas[numeroFala] == "Voc� adquiriu uma escama da Senhora Drag�o." || falas[numeroFala] == "You have acquired a scale from Ms. Dragon.")
    //    {
    //        LigarObjetosEspecificos("Escama");
    //    }
    //    else if (falas[numeroFala] == "T� vendo essa porta atr�s de mim? Ela sempre levar� voc� ao caminho que precisar� seguir." || falas[numeroFala] == "Ya see that door behind me? It will always take you to the path you�ll need to follow.")
    //    {
    //        LigarObjetosEspecificos("Porta");
    //        Mago.transform.localScale = new Vector3(-1, 1, 1);
    //    }
    //    else if (falas[numeroFala] == "VOC�!" || falas[numeroFala] == "YOU!")
    //    {
    //        Mago.transform.localScale = new Vector3(1, 1, 1);
    //    }
    //    else if (falas[numeroFala] == "Agora v�!" || falas[numeroFala] == "Now go!")
    //    {
    //        Mago.transform.localScale = new Vector3(1, 1, 1);
    //    }
    //    else if (falas[numeroFala] == "Enfim! � s� seguir pela mesma porta que voc� foi anteriormente." || falas[numeroFala] == "Anyways! Just go through the same door from before." || falas[numeroFala] == "Fique atento." || falas[numeroFala] == "Be aware.")
    //    {
    //        LigarObjetosEspecificos("PortaPassada");
    //        LigarObjetosEspecificos("Porta");
    //    }
    //    else if (falas[numeroFala] == "V� se n�o demora!" || falas[numeroFala] == "Try not to take long!" || falas[numeroFala] == "Encontre o Grande P� de Feij�o." || falas[numeroFala] == "Find the Big Beanstalk.")
    //    {
    //        LigarObjetosEspecificos("Porta");
    //    }

    //}

    //void LigarObjetosEspecificos(string objeto, bool state)
    //{
    //    if (objeto == "Drag�o")
    //    {
    //        DragonEyes.SetActive(true);
    //    }

    //    if (objeto == "Escama")
    //    {
    //        Escama.SetActive(false);
    //    }
    //}

    #endregion

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
