using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private AnimationCurve curve;
    private readonly float _shakeDuration = 1f;
    
    //private Player_Penguin_Controller _playerPenguinController => Player_Penguin_Controller.I;


    private void Start()
    {
        //_playerPenguinController.HealthAffectedEvent += DoCameraShake;
    }

    private void DoCameraShake()
    {
        StopAllCoroutines();
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while(elapsedTime < _shakeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float strength = curve.Evaluate(elapsedTime / _shakeDuration);
            Vector3 shake = startPosition + Random.insideUnitSphere * strength;
            shake = new Vector3(shake.x, shake.y, startPosition.z);
            transform.position = shake;
            yield return null;
        }

        transform.position = startPosition;
    }

}
