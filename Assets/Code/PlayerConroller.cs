using System;
using UnityEngine;

public class PlayerConroller : MonoBehaviour
{
	public Rigidbody2D rbHead;
	public Rigidbody2D rbBody;
	public Rigidbody2D child1;
	public Rigidbody2D child2;
	public string inputHorizontal;
	public string inputVertical;

	public float horizontalForce;
	public float verticalForce;

	public float maxVelocity = 5000;
	public float impulseMultiplier = 5.0f;
	public float currentHorizontalForce;
	public float currentVerticalForce;
	public float crashMultiplier = 5.0f;
	public float floorHitDistance = 1.0f;

	public Vector2 hitDir = Vector2.up;
	public LayerMask groundLayerMask;

	public bool isGrounded = true;
	private void Start()
	{
		child1 = rbBody.transform.Find("Arm_R").GetChild(3).GetComponent<Rigidbody2D>();
		child2 = rbBody.transform.Find("Arm_L").GetChild(3).GetComponent<Rigidbody2D>();
	}
	void Update()
    {
		HandleMovement();
		HandleRayCast();
	}
	public float GetCurrentHorizontalForce()
	{
		return currentHorizontalForce;
	}

	public void MultiplyScale(float multiplier)
	{
		/*
		foreach(Transform child in transform)
		{
			child.localScale *= multiplier;
		}
		*/
		rbBody.transform.localScale *= multiplier;
	}

	public float GetBodyScale()
	{
		return rbBody.transform.localScale.x;
	}

	private void HandleRayCast()
	{
		Vector2 startPos = rbBody.transform.position;
		var dir = Vector2.down;
		RaycastHit2D hit = Physics2D.Raycast(startPos, dir, floorHitDistance, groundLayerMask);
		Debug.DrawRay(startPos, dir * floorHitDistance, Color.red);
		if(hit.collider != null)
		{
			Debug.Log("GROUNDED");
			dir = hit.point - new Vector2(transform.position.x, transform.position.y);
			hitDir = dir.normalized;
			isGrounded = true;
		}
		else
		{
			Debug.Log("NOT GROUNDED");
			isGrounded = false;
		}		
	}

	private void HandleMovement()
	{
		float horizontal = Input.GetAxis(inputHorizontal);
		float vertical = Input.GetAxis(inputVertical);
		currentHorizontalForce = horizontal * Time.deltaTime * horizontalForce;
		currentVerticalForce = vertical * Time.deltaTime * verticalForce;

		if(vertical < 0)
		{
			currentVerticalForce *= crashMultiplier;
		}
		if(!isGrounded && currentVerticalForce > 0)
		{
			currentVerticalForce = 0;
		}
		AddForces(rbBody);
		//AddForces(child1);
		//AddForces(child2);
	}

	private void AddForces(Rigidbody2D rb)
	{
		rb.AddForce(currentHorizontalForce * Vector3.right, ForceMode2D.Impulse);
		rb.AddForce(currentVerticalForce * Vector3.up, ForceMode2D.Impulse);
		rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);
	}
}
