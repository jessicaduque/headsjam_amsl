using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("Music Settings")]
    [SerializeField] Button b_music;
    SpriteState _musicState;
    [SerializeField] Sprite sprite_musicOffUnpressed;
    [SerializeField] Sprite sprite_musicOffPressed;
    private Sprite sprite_musicOnUnpressed;
    private Sprite sprite_musicOnPressed;
    private Image im_music;
    bool _musicOn;
    bool _musicCooldown;

    [Header("Sound Effects Settings")]
    [SerializeField] Button b_effects;
    SpriteState _effectsState;
    [SerializeField] Sprite sprite_effectsOffUnpressed;
    [SerializeField] Sprite sprite_effectsOffPressed;
    private Sprite sprite_effectsOnUnpressed;
    private Sprite sprite_effectsOnPressed;
    private Image im_effects;
    bool _effectsOn;
    bool _effectsCooldown;

    private AudioManager _audioManager => AudioManager.I;
    private const string keyMixerEffects = "Sfx";
    private const string keyMixerMusic = "Music";

    private void Awake()
    {
        im_music = b_music.GetComponent<Image>();
        im_effects = b_effects.GetComponent<Image>();

        _musicState = b_music.spriteState;
        sprite_musicOnUnpressed = im_music.sprite;
        sprite_musicOnPressed = _musicState.pressedSprite;

        _effectsState = b_effects.spriteState;
        sprite_effectsOnUnpressed = im_effects.sprite;
        sprite_effectsOnPressed = _effectsState.pressedSprite;
    }

    private void Start()
    {
        b_music.onClick.AddListener(ChangeMusicState);
        b_effects.onClick.AddListener(ChangeEffectsState);
    }

    private void OnEnable()
    {
        CheckInitialAudio();
    }

    #region Audio Changes

    private void CheckInitialAudio()
    {
        _musicOn = !PlayerPrefs.HasKey(keyMixerMusic) || PlayerPrefs.GetInt(keyMixerMusic) != 0;
        _effectsOn = !PlayerPrefs.HasKey(keyMixerEffects) || (PlayerPrefs.GetInt(keyMixerEffects) != 0);

        ChangeSpritesMusic();
        ChangeSpritesEffects();

    }

    private void ChangeMusicState()
    {
        if (!_musicCooldown)
        {
            _audioManager.PlaySfx("buttonclick");
            _musicOn = !_musicOn;
            ChangeSpritesMusic();
            _audioManager.ChangeStateMixerMusic(_musicOn);
            StartCoroutine(MusicCooldown());
            b_music.enabled = false;
            _musicCooldown = true;
        }
    }
    private void ChangeEffectsState()
    {
        if (!_effectsCooldown)
        {
            _effectsOn = !_effectsOn;
            ChangeSpritesEffects();
            _audioManager.ChangeStateMixerSFX(_effectsOn);
            if (_effectsOn) { _audioManager.PlaySfx("buttonclick"); }
            StartCoroutine(EffectsCooldown());
            b_effects.enabled = false;
            _effectsCooldown = true;
        }
    }

    private void ChangeSpritesMusic()
    {
        if (_musicOn)
        {
            im_music.sprite = sprite_musicOnUnpressed;
            _musicState.pressedSprite = sprite_musicOnPressed;
        }
        else
        {
            im_music.sprite = sprite_musicOffUnpressed;
            _musicState.pressedSprite = sprite_musicOffPressed;
        }
    }
    private void ChangeSpritesEffects()
    {
        if (_effectsOn)
        {
            im_effects.sprite = sprite_effectsOnUnpressed;
            _effectsState.pressedSprite = sprite_effectsOnPressed;
        }
        else
        {
            im_effects.sprite = sprite_effectsOffUnpressed;
            _effectsState.pressedSprite = sprite_effectsOffPressed;
        }
    }

    #endregion

    #region Cooldowns

    private IEnumerator MusicCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        b_music.enabled = true;
        _musicCooldown = false;
    }

    private IEnumerator EffectsCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        b_effects.enabled = true;
        _effectsCooldown = false;
    }

    #endregion
}
