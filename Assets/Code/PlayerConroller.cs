using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConroller : MonoBehaviour
{
	public Rigidbody2D rbHead;
	public Rigidbody2D rbBody;
	public string inputHorizontal;
	public string inputVertical;
	public float force = 1.0f;
	public float maxVelocity = 5000;
	public float minVelocity = -5000;
	public float impulseMultiplier = 5.0f;

	void Update()
    {
		float horizontal = Input.GetAxis(inputHorizontal) * Time.deltaTime * force;
		float vertical = Input.GetAxis(inputVertical) * Time.deltaTime * force;
		
		Debug.Log("haha:" + horizontal);


		rbBody.AddForce(horizontal * Vector3.right, ForceMode2D.Impulse);
		rbBody.AddForce(vertical * Vector3.up, ForceMode2D.Impulse);

		LimitAndClampVelocity();
	}

	private void LimitAndClampVelocity()
	{
		rbBody.velocity = Vector2.ClampMagnitude(rbBody.velocity, maxVelocity);
	}
}
