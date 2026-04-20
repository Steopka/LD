using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PortalSignal : MonoBehaviour
{
    [Header("настройка звука")]
    public AudioSource signalAudioSource; // Источник звука
    public AudioClip portalSignalClipl; // Звуковой файл 
    public float fadeDuration = 3.5f; // Время заглучшения 
    public float startPitch = 1.3f; // Начальная громкость 
    public float endPitch = 0.5f; // Конечная громкость 

    private bool isTriggered = false;

    private void Start()
    {
        if (signalAudioSource == null)
        {
            signalAudioSource = gameObject.AddComponent<AudioSource>();

        }

        signalAudioSource.spatialBlend = 0f; 
        signalAudioSource.playOnAwake = false;
        signalAudioSource.loop = false;
        signalAudioSource.clip = portalSignalClipl;


    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isTriggered && collision.gameObject.CompareTag("Player"))
        {
            isTriggered = true;
            StartCoroutine(PlayPortalSignal());
        }

    }

    private IEnumerator PlayPortalSignal()
    {
        signalAudioSource.volume = 1f;
        signalAudioSource.pitch = startPitch;   

        signalAudioSource.Play();

        float timer = 0f;

        // Снижение громкости 
        while (timer < fadeDuration) { 
            timer += Time.deltaTime;
            float pogress = timer / fadeDuration;

            signalAudioSource.volume = Mathf.Lerp(1f, 0f, pogress);

            signalAudioSource.pitch = Mathf.Lerp(startPitch, endPitch, pogress);

            yield return null;
        }

        signalAudioSource.Stop();
        signalAudioSource.volume = 1f;
        signalAudioSource.pitch = 1f;
    }
}
