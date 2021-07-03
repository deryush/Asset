using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class AssetLoader
{
	public static async Task InitAssets<T>(string label, List<T> createdObjs, Transform parent, Slider overall, Text overalltxt, CanvasGroup group)
	where T : UnityEngine.Object
	{
        var locations = await Addressables.LoadResourceLocationsAsync(label).Task;

        float f = 1f / (locations.Count + 1f);
        overall.value = f;
        overalltxt.text = (overall.value * 100) + "%";
        foreach (var location in locations)
        {
            createdObjs.Add(await Addressables.InstantiateAsync(location, parent).Task as T);
            overall.value = overall.value + f;
            overalltxt.text = (overall.value * 100) + "%";
        }

        await FadeLoadingScreen(0, 0.5f, group);
    }

    static async Task FadeLoadingScreen(float targetValue, float duration, CanvasGroup group)
    {
        float startValue = group.alpha;
        float time = 0f;

        while (time < duration)
        {
            group.alpha = Mathf.Lerp(startValue, targetValue, time / duration);
            time += Time.deltaTime;
        }
        group.alpha = targetValue;
    }
}
