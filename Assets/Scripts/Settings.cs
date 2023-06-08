using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Canvas canvas;
    public Toggle toggle;
    public Image image;
    public Slider volumeSlider;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = SoundManager.Instance.GetComponent<AudioSource>();
        toggle.onValueChanged.AddListener(OnMusicValueChanged);
        volumeSlider.onValueChanged.AddListener(OnSliderValueChanged);
        volumeSlider.value = 0.5f;

        volumeSlider.gameObject.SetActive(toggle.isOn);
    }

    public void OnSettingsButtonClick()
    {
        canvas.gameObject.SetActive(!canvas.gameObject.activeSelf); // Toggle the visibility of the canvas
    }

    public void OnMusicValueChanged(bool value)
    {
        // Handle the toggle state change
        if (value)
        {
            Debug.Log("Toggle is ON");
            image.color = Color.green;
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            Debug.Log("Toggle is OFF");
            image.color = Color.red;
            audioSource.Stop();
        }

        volumeSlider.gameObject.SetActive(value);
    }

    public void OnSliderValueChanged(float value)
    {
        // Update the volume based on the slider value
        SetVolume(value);
    }

    private void SetVolume(float value)
    {
        // Update the volume of the audio source
        audioSource.volume = value;
    }
}
