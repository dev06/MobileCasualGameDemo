using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
	public Transform target;
	public float min = 100f;
	public float max = 300f;
	private float speed;
	private Rigidbody _rigidbody;
	private Animator _animator;
	private float timer = 0f;
	void Start()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_animator = GetComponent<Animator>();
		speed = Random.Range(min, max);
	}

	// Update is called once per frame
	void Update()
	{
		if (GameController.Instance.LoseControls)
		{
			_animator.SetBool("isWalking", false);
			return;
		}
		if (!GameController.Instance.GameStart) return;
		if (timer <= .5f)
		{
			timer += Time.deltaTime;
			return;
		}

		float _raw = (target.position - transform.position).sqrMagnitude;
		if (_raw > 1f)
		{
			_rigidbody.velocity = (target.position - transform.position).normalized * Time.deltaTime * speed;
		}
		Vector3 _vec = _rigidbody.velocity.normalized;
		_vec.y = 0f;
		transform.forward = Vector3.Lerp(transform.forward, _vec, Time.unscaledDeltaTime * 10f);
		_animator.SetBool("isWalking", _rigidbody.velocity.magnitude > .1f);
	}
}
