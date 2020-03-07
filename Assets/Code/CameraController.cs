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
	public float deathPosition = -15;
	private Vector3 currentVelocity;
	private Camera cam;
	private CharacterSpawner cSpawner;
	private void Start()
	{
		cam = GetComponent<Camera>();
		cSpawner = FindObjectOfType<CharacterSpawner>();
		var playercontrollers = FindObjectsOfType<PlayerConroller>();
		var rbBodies = playercontrollers.Select(x => x.rbBody.transform);
		targets.AddRange(rbBodies);
		cSpawner.OnTargetRevived += OnTargetRevived;
	}

	private void OnDestroy()
	{
		if(cSpawner != null)
		{
			cSpawner.OnTargetRevived -= OnTargetRevived;
		}
	}

	private void OnTargetRevived(Transform obj)
	{
		targets.Add(obj);
	}

	private void LateUpdate()
	{
		if(targets.Count == 0)
		{
			return;
		}
		Move();
		Zoom();
		RefreshTargets();
	}

	private void RefreshTargets()
	{
		var deadTargets = targets.Where(x => x.position.y < deathPosition).ToList();
		targets.RemoveAll(x => x.position.y < deathPosition);
		cSpawner.ResetTargets(deadTargets);
	}

	private void Zoom()
	{
		var dist = GetGreatestDistance();
		float newZoom = Mathf.Lerp(maxZoom, minZoom, dist / zoomLimiter);
		cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
	}

	private float GetGreatestDistance()
	{
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
		Vector3 center = Vector3.zero;
		for(int i = 0; i < targets.Count; i++)
		{
			center += targets[i].position;		
		}
		return center / targets.Count;
	}
}
