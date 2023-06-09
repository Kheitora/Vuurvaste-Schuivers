using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class CountdownTimer : MonoBehaviour
{
    public float totalTime = 60f; // Total time for the countdown in seconds
    public TextMeshProUGUI timerText; // Reference to the TextMeshProUGUI component to display the timer
    public Transform objectToScale; // Reference to the GameObject you want to scale
    public ParticleSystem fireParticleSystem; // Reference to the particleSystem component on the fire GameObject
    public ParticleSystem smokeParticleSystem; // Reference to the ParticleSystem component on the Smoke GameObject
    public ParticleSystem explosionParticleSystem; // Reference to the ParticleSystem component on the explosion GameObject
    public Vector3 startScale = Vector3.one; // Initial scale of the object
    public Vector3 endScale = Vector3.one * 4f; // Final scale of the object
    public int maxSmokeParticles = 1000; // Maximum number of particles for the Smoke GameObject
    public float maxEmissionRate = 100f; // Maximum emission rate for the Smoke GameObject
    public Color startColor = Color.white; // Initial color of the smoke particles
    public Color endColor = Color.black; // Final color of the smoke particles
    public AudioClip audioClipStart; // Audio clip for the start scale phase
    public AudioClip audioClipMiddle; // Audio clip for the middle scale phase
    public AudioClip audioClipEnd; // Audio clip for the end scale phase
    public AnimationCurve particleCurve; // Reference to the AnimationCurve of the Smoke GameObject
    public int sceneIndex; // Integer for the scene to move to once game is done.

    private float currentTime; // Current time remaining
    private AudioSource audioSource; // Reference to the AudioSource component
    private AudioSource explosionSource; // Reference to the AudioSource component

    private void Start()
    {
        currentTime = totalTime;
        objectToScale.localScale = startScale;
        explosionParticleSystem.Stop();

        audioSource = objectToScale.GetComponent<AudioSource>();
        explosionSource = explosionParticleSystem.GetComponent<AudioSource>();

        explosionSource.Stop();
    }

    private void Update()
    {
        if (currentTime > 0f)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerText();
            UpdateObjectScale();
            UpdateSmokeParticles();
            UpdateSmokeDensity();
            UpdateSmokeColor();
            UpdateAudio();
        }
        else
        {
            currentTime = 0f;
            UpdateTimerText();
            TimerCompleted();
        }
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void UpdateObjectScale()
    {
        float t = 1f - (currentTime / totalTime); // Calculate the normalized time (0 to 1)
        objectToScale.localScale = Vector3.Lerp(startScale, endScale, t);
    }

    private void UpdateSmokeParticles()
    {
        float normalizedTime = 1f - (currentTime / totalTime); // Calculate the normalized time (0 to 1)
        float curveValue = particleCurve.Evaluate(normalizedTime); // Get the value from the animation curve
        int newMaxParticles = Mathf.RoundToInt(curveValue * maxSmokeParticles);
        smokeParticleSystem.maxParticles = newMaxParticles;
    }

    private void UpdateSmokeDensity()
    {    
        float normalizedTime = 1f - (currentTime / totalTime); // Calculate the normalized time (0 to 1)
        float newEmissionRate = normalizedTime * maxEmissionRate;
        var emissionModule = smokeParticleSystem.emission;
        emissionModule.rateOverTime = newEmissionRate;
    }

    private void UpdateSmokeColor()
    {
        float normalizedTime = 1f - (currentTime / totalTime); // Calculate the normalized time (0 to 1)
        Color newColor = Color.Lerp(startColor, endColor, normalizedTime);
        var mainModule = smokeParticleSystem.main;
        mainModule.startColor = newColor;
    }

    private void UpdateAudio()
    {
        float scalePercentage = objectToScale.localScale.magnitude / endScale.magnitude;

        if (scalePercentage <= 1f / 3f)
        {
            if (audioSource.clip != audioClipStart)
            {
                audioSource.clip = audioClipStart;
                audioSource.Play();
            }
        }
        else if (scalePercentage <= 2f / 3f)
        {
            if (audioSource.clip != audioClipMiddle)
            {
                audioSource.clip = audioClipMiddle;
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.clip != audioClipEnd)
            {
                audioSource.clip = audioClipEnd;
                audioSource.Play();
            }
        }
    }

    private void TimerCompleted()
    {
        // Activate a specific particle system
        explosionParticleSystem.Play();

        // Deactivate the main smoke particle system
        smokeParticleSystem.Stop();
        fireParticleSystem.Stop();

        explosionSource.Play();
        StartCoroutine(WaitForAudioToFinish());
        ChangeScene();


    }

    private IEnumerator WaitForAudioToFinish()
    {
        yield return new WaitForSeconds(2f); // Wait for 3 seconds
    }

    private void ChangeScene()
    {
        
        explosionParticleSystem.Stop();
        SceneManager.LoadScene(sceneIndex);
    }
}
