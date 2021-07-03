using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{

    public AssetReference scene;
    private AsyncOperationHandle<SceneInstance> _handle;
    public Slider loadingbar;
    public Text loadingtext;
    public CanvasGroup canvasGroup;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Addressables.DownloadDependenciesAsync(scene);
        StartCoroutine(LoadAddresableScene());
    }

    private IEnumerator LoadAddresableScene()
    {
        yield return null;
        AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(scene, LoadSceneMode.Additive, true);
        while (handle.Status != AsyncOperationStatus.Succeeded)
        {
            try
            {
                loadingtext.text = handle.PercentComplete * 100 + "%";
                loadingbar.value = handle.PercentComplete;
            } catch(NullReferenceException ex)
            {
                Debug.Log(ex.Message);
            }
            yield return null;
        }
        _handle = handle;
        yield return StartCoroutine(FadeLoadingScreen(0, 0.3f));
    }

    void Stop()
    {
        Unload();
    }

    void Unload()
    {
        Addressables.UnloadSceneAsync(_handle, true).Completed += l =>
        {
            if (l.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("Scene unloaded");
            }
        };
    }

    IEnumerator FadeLoadingScreen(float targetValue, float duration)
    {
        float startValue = canvasGroup.alpha;
        float time = 0;

        while (time < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startValue, targetValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = targetValue;
    }
}

