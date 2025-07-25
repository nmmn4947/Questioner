using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SceneSetting : MonoBehaviour
{
    public Slider slider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slider.value = 1.0f;
        AudioManager.Instance.SetSFXVolume(slider.value);
    }

    // Update is called once per frame
    void Update()
    {
        AudioManager.Instance.SetSFXVolume(slider.value);
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
