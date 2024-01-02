using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    public static GameManager Instance { get { return instance; } }

    private float playTime;
    private float startTime;
    private List<float> activateTimes;
    public Texture endStatus;
    public Camera topView;

    public Material nightSkybox;

    private AudioSource currentBGM;
    public AudioSource mainSunsetBGM;
    public AudioSource mainNightBGM;
    public AudioSource exitBGM;

    public float PlayTime { get { return playTime; } }
    public List<float> ActivateTimes { get { return activateTimes; } }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        startTime = Time.time;
        currentBGM = mainSunsetBGM;
        currentBGM.Play();
    }
    void Update()
    {
        if(Time.time - startTime > 900f)
        {
            currentBGM.Pause();
            mainNightBGM.Play();
            currentBGM = mainNightBGM;
            RenderSettings.skybox = nightSkybox;
        }
        // For test
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ExitMaze();
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            PathFinder.Instance.FindPath();
        }
    }

    void ActivatePoneglyph()
    {
        activateTimes.Add(Time.time - startTime);
    }

    public void ExitMaze()
    {
        playTime = Time.time - startTime;

        RenderTexture srcOrigin = topView.targetTexture;
        Texture2D src = new Texture2D(srcOrigin.width, srcOrigin.height, TextureFormat.RGB24, false);
        
        RenderTexture.active = srcOrigin;
        src.ReadPixels(new Rect(0, 0, src.width, src.height), 0, 0);
        src.Apply();

        Texture2D dest = new Texture2D(src.width, src.height, src.format, false);
        Graphics.CopyTexture(src, dest);

        endStatus = dest;

        currentBGM.Pause();
        exitBGM.Play();
        currentBGM = exitBGM;

        SceneManager.LoadScene("Scenes/Exit");
        Debug.Log("Loaded");

        
    }
}
