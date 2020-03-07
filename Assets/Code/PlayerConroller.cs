using System;
using System.Collections.Generic;
using System.Linq;
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
	public float crashMultiplier = 20.0f;
	public float floorHitDistance = 1.0f;

	public Vector2 hitDir = Vector2.up;
	public LayerMask groundLayerMask;

	public List<PlayerConroller> enemies = new List<PlayerConroller>();
	public List<LineRenderer> lines = new List<LineRenderer>();
	private System.Random rng = new System.Random();

	public bool isGrounded = true;
	private void Start()
	{
		child1 = rbBody.transform.Find("Arm_R").GetChild(3).GetComponent<Rigidbody2D>();
		child2 = rbBody.transform.Find("Arm_L").GetChild(3).GetComponent<Rigidbody2D>();
		enemies = FindObjectsOfType<PlayerConroller>().Except(this.Yield()).ToList();
		foreach(Transform ch in rbBody.transform)
		{
			var lr = ch.GetComponent<LineRenderer>();
			if(lr != null)
			{
				lines.Add(lr);
			}
		}
	}

	private IEnumerable<PlayerConroller> Yield()
	{
		yield return this;
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
		rbBody.transform.localScale *= multiplier;
	}

	public void MultiplyLines(float multiplier)
	{
		foreach(var line in lines)
		{
			line.SetWidth(line.startWidth * multiplier, line.endWidth * multiplier);
		}
	}

	public Transform GetRandomEnemyTransform()
	{
		int val = rng.Next(enemies.Count + 1);
		return enemies[val].transform;
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

		
		if(!isGrounded && currentVerticalForce > 0)
		{
			currentVerticalForce = 0;
		}
		if(currentVerticalForce < 0 && !isGrounded)
		{
			Debug.Log("Crashing");
			currentVerticalForce *= crashMultiplier;
			AddForces(rbBody, ForceMode2D.Force);
		}
		else
		{
			AddForces(rbBody, ForceMode2D.Impulse);
		}
		
		//AddForces(child1);
		//AddForces(child2);
	}

	private void AddForces(Rigidbody2D rb, ForceMode2D mode = ForceMode2D.Force)
	{
		float multiplier = 1.0f;
		if(mode == ForceMode2D.Force)
		{
			multiplier *= 100;
		}
		rb.AddForce(currentHorizontalForce * multiplier * Vector3.right, mode);
		rb.AddForce(currentVerticalForce * Vector3.up, mode);
		rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);
	}
}
