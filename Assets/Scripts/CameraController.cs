using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

	public Transform targetToFollow;

	private Vector3 _defaultPosition;

	void Start()
	{
		_defaultPosition = transform.position;
	}

	// Update is called once per frame
	void Update()
	{
		Vector3 _targetPos = _defaultPosition + new Vector3(targetToFollow.position.x, 0f, targetToFollow.position.z);
		transform.position = Vector3.Lerp(transform.position, _targetPos, Time.unscaledDeltaTime * 6f);
	}
}
