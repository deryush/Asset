using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class AssetController : MonoBehaviour
{
	[SerializeField]
	string _label;

	public Slider overallbar;
	public Text loadingtext;
	public CanvasGroup canvasGroup;

	private Transform _parent;
	private List<GameObject> _createdObjs { get; } = new List<GameObject>();

	private void Start()
	{
		_parent = transform;
		Instantiate();
	}

	private async void Instantiate()
	{
		await AssetLoader.InitAssets(_label, _createdObjs, _parent, overallbar, loadingtext, canvasGroup);
	}
}
