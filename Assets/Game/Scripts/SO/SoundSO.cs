using UnityEngine;
[CreateAssetMenu(fileName = "SoundSO", menuName = "Audio Manager/Sound")]
public class SoundSO : ScriptableObject
{
    public string soundName;
    public AudioClip clip;
    public bool loop;

    [Range(0, 1)]
    public float volume;
    [Range(0.1f, 3)]
    public float pitch;
}