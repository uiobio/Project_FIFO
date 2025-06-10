using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    public AudioSource AudioSource;
    public AudioClip AudioClip;

    private void Awake()
    {
        if (Instance == null)
        {
            AudioSource = GetComponent<AudioSource>();
            if (AudioSource == null)
            {
                AudioSource = gameObject.AddComponent<AudioSource>();
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AudioSource.Stop();
        AudioClip = null;
        // FIXME: replace with actual scene names once we start implementing actual rooms
        if (scene.name == "Basic L Room")
        {
            AudioClip = Resources.Load<AudioClip>("FIFO_stage_1");
        }
        else if (scene.name == "Basic Shop Room")
        {

        }
        if (AudioClip != null)
        {
            AudioSource.clip = AudioClip;
            AudioSource.loop = true;
            AudioSource.Play();
        }
        else
        {
            Debug.LogWarning("Could not find audio for scene named: " + scene.name);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
