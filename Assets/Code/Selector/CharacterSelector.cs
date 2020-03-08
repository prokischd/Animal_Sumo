using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
	public class ImageData
	{
		public Image image;
		public bool taken;

		public ImageData(Image image, bool taken)
		{
			this.image = image;
			this.taken = taken;
		}
	}
	public List<Image> images;

	public Image leftImage;
	public Image rightImage;

	private List<ImageData> imageData;
	public List<GameObject> characters;

	public int leftIdx;
	public int rightIdx;
	private bool isLoading = false;
	public bool selectorEnabled = true;

	public List<Image> splashArts;

	private void Start()
	{
		imageData = new List<ImageData>();
		foreach(var im in images)
		{
			imageData.Add(new ImageData(im, false));
		}
		DontDestroyOnLoad(this);

		leftIdx = 0;
		var takenData = imageData[leftIdx];
		takenData.taken = true;
		leftImage.sprite = takenData.image.sprite;

		rightIdx = 1;
		takenData = imageData[rightIdx];
		takenData.taken = true;
		rightImage.sprite = takenData.image.sprite;
	}


	private void Update()
	{
		if(selectorEnabled)
		{
			float hor1 = Input.GetAxis("Player2Horizontal");
			float hor2 = Input.GetAxis("Player1Horizontal");

			if(Input.GetButtonDown("Player2Horizontal"))
			{
				Configure(hor1, leftImage, ref leftIdx);
			}
			if(Input.GetButtonDown("Player1Horizontal"))
			{
				Configure(hor2, rightImage, ref rightIdx);
			}

			if(Input.GetKeyDown(KeyCode.H) && !isLoading)
			{
				StartCoroutine(StartGame());
			}
		}

	}

	IEnumerator StartGame()
	{
		isLoading = true;
		var asyncLoadLevel = SceneManager.LoadSceneAsync(1);
		while(!asyncLoadLevel.isDone)
		{
			yield return null;
		}
		isLoading = false;
		selectorEnabled = false;
	}

	private void Configure(float hor, Image image, ref int idx)
	{
		imageData[idx % imageData.Count].taken = false;
		do
		{
			if(hor > 0)
			{
				idx++;
			}
			else
			{
				idx--;
			}
			if(idx < 0)
			{
				idx = imageData.Count - 1;
			}
			else if(idx > imageData.Count - 1)
			{
				idx = 0;
			}
		}
		while(imageData[idx % imageData.Count].taken);
		var takenData = imageData[idx % imageData.Count];
		takenData.taken = true;
		image.sprite = takenData.image.sprite;
		
	}
}
