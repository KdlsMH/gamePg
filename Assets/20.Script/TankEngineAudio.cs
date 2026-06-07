using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class TankEngineAudio : MonoBehaviour
{
    public AudioClip engineIdleClip;
    public AudioClip engineDrivingClip;

    public float movementThreshold = 0.2f;
    public float pitchRange = 0.2f;

    private AudioSource audioSource;
    private Rigidbody rb;
    private float originalPitch;

    private bool isDrivingSoundPlaying = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();

        originalPitch = audioSource.pitch;

        audioSource.loop = true;
        audioSource.playOnAwake = false;

        PlayIdleSound();
    }

    private void Update()
    {
        if (GameManager.Instance != null && !GameManager.Instance.IsPlaying)
        {
            PlayIdleSound();
            return;
        }

        float speed = rb.linearVelocity.magnitude;

        if (speed > movementThreshold)
        {
            PlayDrivingSound();
        }
        else
        {
            PlayIdleSound();
        }
    }

    private void PlayIdleSound()
    {
        if (!isDrivingSoundPlaying && audioSource.clip == engineIdleClip)
        {
            return;
        }

        audioSource.clip = engineIdleClip;
        audioSource.pitch = originalPitch;
        audioSource.loop = true;

        if (engineIdleClip != null)
        {
            audioSource.Play();
        }

        isDrivingSoundPlaying = false;
    }

    private void PlayDrivingSound()
    {
        if (isDrivingSoundPlaying && audioSource.clip == engineDrivingClip)
        {
            return;
        }

        audioSource.clip = engineDrivingClip;
        audioSource.pitch = Random.Range(originalPitch - pitchRange, originalPitch + pitchRange);
        audioSource.loop = true;

        if (engineDrivingClip != null)
        {
            audioSource.Play();
        }

        isDrivingSoundPlaying = true;
    }
}