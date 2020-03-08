using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerConroller : MonoBehaviour
{
	private Rigidbody2D rbHead;
	public Rigidbody2D rbBody { get; set; }
	private Rigidbody2D child1;
	private Rigidbody2D child2;
	public string inputHorizontal;
	public string inputVertical;

	public float horizontalForce;
	public int inputBlocked { get; private set; } = 0;
	public float verticalForce;
	public float jumpForceMultiplier = 1.0f; 

	private float maxVelocity = 60;
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
	private CrashForce crashForce;

	public bool isGrounded = true;
	public bool crashing;
	private float explosionForce = 220;

	private CharacterSpawner cSpawner;
	public int HP { get; private set; }
	public bool CanGrow { get; set; } = true;

	List<Image> hps = new List<Image>();
	public GameObject vfxDizzy;

	internal void ConfigureHealth(GameObject gameObject)
	{
		HealthObject = gameObject;
		foreach(Transform t in HealthObject.transform)
		{
			var im = t.GetComponent<Image>();
			hps.Add(im);
		}
		HP = hps.Count;
	}

	public GameObject HealthObject { get; set; }

	private void Awake()
	{
		cSpawner = FindObjectOfType<CharacterSpawner>();
		rbBody = transform.Find("Body").GetComponent<Rigidbody2D>();
		rbHead = rbBody.transform.Find("Head").GetComponent<Rigidbody2D>();
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
		crashForce = rbBody.GetComponent<CrashForce>();
	}

	internal void MultiplyRayLine(float growMultiplier)
	{
		floorHitDistance *= growMultiplier;
	}

	internal float GetExplosionForce()
	{
		var force = explosionForce * crashBonus * rbBody.transform.localScale.x;
		return force;
	}

	internal void LoseInput(float v)
	{
		inputBlocked++;
		PlayDizzyEffect();
		StartCoroutine(RegainInput(v));
	}

	private void PlayDizzyEffect()
	{
		var go = Instantiate(vfxDizzy, rbHead.transform.position, Quaternion.identity);
		go.transform.parent = rbHead.transform;
	}

	private IEnumerator RegainInput(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		inputBlocked--;
	}

	private IEnumerable<PlayerConroller> Yield()
	{
		yield return this;
	}

	public bool alive = true;
	void Update()
    {
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			var selector = FindObjectOfType<CharacterSelector>();
			Destroy(selector.gameObject);
			SceneManager.LoadScene(0);
		}
		if(HP <= 0)
		{
			return;
		}
		if(inputBlocked == 0)
		{
			HandleMovement();
			HandleRayCast();
		}		
		if(ShouldDie())
		{
			HandleDeath();
		}
	}

	private bool ShouldDie()
	{
		bool isBelowY = rbBody.transform.position.y < -30;
		bool isBelowZ = rbBody.transform.position.x < -65;
		bool isAboveZ = rbBody.transform.position.x > 50;
		bool shouldDie = isBelowY || isBelowZ || isAboveZ;
		return shouldDie && alive;
	}

	private void HandleDeath()
	{
		hps[HP - 1].color = Color.black;
		alive = false;
		HP--;
		cSpawner.OnDeath?.Invoke(rbBody.transform);
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
			line.startWidth *= multiplier;
			line.endWidth *= multiplier;
		}
	}

	public PlayerConroller GetRandomEnemy()
	{
		int val = rng.Next(enemies.Count);
		return enemies[val];
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
			dir = hit.point - new Vector2(transform.position.x, transform.position.y);
			hitDir = dir.normalized;
			isGrounded = true;
			crashForce.Crash(hit.point);
		}
		else
		{
			isGrounded = false;
		}		
	}

	public float crashBonus = 10.0f;

	private void FixedUpdate()
	{
		rbBody.velocity = Vector2.ClampMagnitude(rbBody.velocity, maxVelocity);
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
			crashing = true;
			crashBonus += 5 * Time.deltaTime;
			currentVerticalForce *= crashMultiplier;
			AddForces(rbBody, ForceMode2D.Force);
		}
		else
		{
			AddForces(rbBody, ForceMode2D.Impulse);
		}
	}

	private void AddForces(Rigidbody2D rb, ForceMode2D mode = ForceMode2D.Force)
	{
		float multiplier = 1.0f;
		if(mode == ForceMode2D.Force)
		{
			multiplier *= 100;
		}
		if(!crashing)
		{
			currentHorizontalForce *= 2;
		}
		float verticalMultiplier = 1.0f;
		if(!isGrounded && crashing)
		{
			verticalMultiplier /= 2;
			multiplier /= 4;
		}
		rb.AddForce(currentHorizontalForce * multiplier * Vector3.right, ForceMode2D.Force);
		rb.AddForce(currentVerticalForce * verticalMultiplier * Vector3.up, mode);
	}
}
