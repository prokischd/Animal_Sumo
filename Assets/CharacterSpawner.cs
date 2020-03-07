using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CharacterSpawner :MonoBehaviour
{
	private System.Random rng = new System.Random();
	public List<GameObject> spawningPoints;
	public Action<Transform> OnTargetRevived;
	public Action<Transform> OnDeath;

	private void Start()
	{
		var selector = FindObjectOfType<CharacterSelector>();
		var charactor = selector.characters[selector.leftIdx];
		var rand = rng.Next(spawningPoints.Count);
		SpawnCharactor(charactor, "Player1", rand);
		var charactor2 = selector.characters[selector.rightIdx];
		SpawnCharactor(charactor2, "Player2", (rand + 1) % spawningPoints.Count);
	}

	private void SpawnCharactor(GameObject charactor2, string input, int idx)
	{
		var go	= Instantiate(charactor2, spawningPoints[idx].transform.position, Quaternion.identity);
		var pc = go.GetComponent<PlayerConroller>();
		pc.inputVertical = input + "Vertical";
		pc.inputHorizontal = input + "Horizontal";
		OnTargetRevived?.Invoke(pc.rbBody.transform);
	}

	internal void ResetTarget(Transform target)
	{
		StartCoroutine(ResetTargetsAfterTime(target, 3.0f));		
	}

	private IEnumerator ResetTargetsAfterTime(Transform target, float v)
	{
		yield return new WaitForSeconds(v);
		var rand = rng.Next(spawningPoints.Count);
		target.position = spawningPoints[rand].transform.position;
		target.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
		OnTargetRevived?.Invoke(target);
		target.parent.GetComponent<PlayerConroller>().alive = true;
	}
}
