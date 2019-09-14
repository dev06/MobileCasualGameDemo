using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
	public float speed;
	private Vector3 _defaultPos;

	void Start()
	{
		_defaultPos = transform.position;
		bool stayActive = Random.Range(0f, 1f) < .95f;
		gameObject.SetActive(stayActive);
		if (stayActive)
		{
			GameController.Instance.pickUpRemaining++;
		}
	}
	void Update()
	{
		float _height = .5f;
		transform.position = _defaultPos + new Vector3(0f, Mathf.PingPong(Time.time * .5f, _height) - (_height * .5f), 0f);
		transform.Rotate(Vector3.up, speed * Time.deltaTime, Space.World);
	}
}
