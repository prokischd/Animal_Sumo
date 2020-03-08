using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponKnife : Weapon
{
	private Rigidbody2D rb;
	private BoxCollider2D col;
	public AudioClip audioClip;
	void Start()
	{
		col = GetComponent<BoxCollider2D>();
		rb = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
	}
	public override bool CanExecute(PlayerConroller target)
	{
		if(target.inputBlocked != 0)
		{
			return false;
		}
		var dist = Vector3.Distance(this.transform.position, target.rbBody.position);
		return dist < 1.3f;
	}
	public override void Execute(float v, PlayerConroller target)
	{
		AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);
		target.LoseInput(1.5f);
		Destroy(this.gameObject);
	}
}
