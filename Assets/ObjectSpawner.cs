using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
	public GameObject objectToSpawn;

	private float spawnTimer = 10.0f;
	private void Start()
	{
		StartCoroutine(SpawnAt(0));
	}
	internal void StartSpawning()
	{
		StartCoroutine(SpawnAt(spawnTimer));
	}

	private IEnumerator SpawnAt(float v)
	{
		yield return new WaitForSeconds(v);
		var go = Instantiate(objectToSpawn, this.transform.position, Quaternion.identity);
		go.GetComponent<Weapon>().objSpawner = this;
	}
}
