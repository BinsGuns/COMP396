using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private GameObject _popUpContainer;
    private AudioSource _audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GameObject.FindWithTag("MainCamera").GetComponent<AudioSource>();
        // Set the initial volume based on the slider value
        _audioSource.volume = _volumeSlider.value;

        // Subscribe to the slider's OnValueChanged event
        _volumeSlider.onValueChanged.AddListener(ChangeVolume);
        _popUpContainer.SetActive(false);
    }

    
    void ChangeVolume(float volume)
    {
        // Update the audio source volume when the slider value changes
        _audioSource.volume = volume;
    }

    public void ShowOptions()
    {
        _popUpContainer.SetActive(true);
    }
    
    public void CloseOption()
    {
        _popUpContainer.SetActive(false);
    }
    
    public void GoHome()
    {
        SceneManager.LoadScene(0);
    }
}
