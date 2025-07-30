using System.Collections;
using UnityEngine;
using Utils.Singleton;
using Random = UnityEngine.Random;

public class CameraShake : DontDestroySingleton<CameraShake>
{
    [SerializeField] private AnimationCurve curve;
    private readonly float _shakeDuration = 0.8f;

    public void DoCameraShake()
    {
        StopAllCoroutines();
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
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
