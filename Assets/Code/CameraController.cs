using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
	public List<Transform> targets;
	public Vector3 offset;
	private float smoothTime = 0.05f;
	public float minZoom = 40;
	public float maxZoom = 10;
	public float zoomLimiter = 50;

	private Vector3 currentVelocity;
	private Camera cam;
	private CharacterSpawner cSpawner;

	private float bigSmoothTime = 0.5f;
	private Transform mid;
	private void Start()
	{
		mid = FindObjectOfType<CharacterSpawner>().transform;
		cam = GetComponent<Camera>();
		cSpawner = FindObjectOfType<CharacterSpawner>();
		var playercontrollers = FindObjectsOfType<PlayerConroller>();
		var rbBodies = playercontrollers.Select(x => x.rbBody.transform);
		targets.AddRange(rbBodies);
		cSpawner.OnTargetRevived -= OnTargetRevived;
		cSpawner.OnDeath -= OnTargetDie;
		cSpawner.OnTargetRevived += OnTargetRevived;
		cSpawner.OnDeath += OnTargetDie;

		var dist = GetGreatestDistance();
		float newZoom = Mathf.Lerp(maxZoom, minZoom, dist / zoomLimiter);
		cam.fieldOfView =  newZoom;
	}

	private void OnDestroy()
	{
	}

	private void OnTargetRevived(Transform obj)
	{
		targets.Add(obj);
		smoothTime = bigSmoothTime;		
	}

	private void OnTargetDie(Transform target)
	{
		targets.Remove(target);
		smoothTime = bigSmoothTime;
		if(target.parent.GetComponent<PlayerConroller>() is PlayerConroller pc)
		{
			if(pc.HP <= 0)
			{
				return;
			}
		}
		cSpawner.ResetTarget(target);
	}

	private void LateUpdate()
	{
		if(targets.Count != 0)
		{
			Zoom();
		}
		Move();

		if(smoothTime > 0.02f)
		{
			smoothTime -= Time.deltaTime;
		}
	}

	private void Zoom()
	{
		var dist = GetGreatestDistance();
		float newZoom = Mathf.Lerp(maxZoom, minZoom, dist / zoomLimiter);
		cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
	}

	private float GetGreatestDistance()
	{
		if(targets.Count == 0)
		{
			return mid.position.x;
		}
		var bounds = new Bounds(targets[0].position, Vector3.zero);
		foreach(var t in targets)
		{
			bounds.Encapsulate(t.position);
		}
		return bounds.size.x;
	}

	private void Move()
	{
		Vector3 centerPoint = GetCenterPoint();
		Debug.DrawLine(Vector3.zero, centerPoint);
		Vector3 newPosition = centerPoint + offset;
		transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref currentVelocity, smoothTime);
	}
	private Vector3 GetCenterPoint()
	{
		if(targets.Count == 0)
		{
			return mid.position;
		}
		Vector3 center = Vector3.zero;
		for(int i = 0; i < targets.Count; i++)
		{
			center += targets[i].position;		
		}
		return center / targets.Count;
	}
}
