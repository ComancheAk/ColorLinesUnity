using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    public GameObject background;
    public AudioSource audioSource;
    private bool musicStarted;
    public GameObject loadingAnimation;

    void Awake()
    {
        loadingAnimation.SetActive(false);
        Helpers.Set2DCameraToObject(background);
    }

    void OnMouseUp()
    {
        audioSource.Stop();
        GotoGameplay();
    }

    void Start()
    {
        var soundSystemMode = Config.GetSoundSystemMode();
        if (soundSystemMode == SoundSystemConfig.Music ||
            soundSystemMode == SoundSystemConfig.MusicAndSound)
        {
            musicStarted = true;
            audioSource.Play();
        }
        else
        {
            Invoke("GotoGameplay", 0.7f);
        }
    }

    void GotoGameplay()
    {
        loadingAnimation.SetActive(true);
        StartCoroutine(LoadGamePlayAsyncScene());
    }

    IEnumerator LoadGamePlayAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Single);

        while (!asyncLoad.isDone)
            yield return null;
    }

    void Update()
    {
        if (musicStarted && !audioSource.isPlaying)
            GotoGameplay();
#if UNITY_ANDROID
        if (Input.GetKeyUp(KeyCode.Escape))
            Application.Quit();
#endif
    }
}
