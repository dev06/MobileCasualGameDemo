using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerMovement : MonoBehaviour
{
	[HideInInspector]
	public int Score;


	private float _timer = 20;

	private Vector3 _pointerLast;
	private Vector3 _pointerCurrent;
	private Vector3 _pointerDown;
	private float dx, dz;
	private int _lives = 30;


	[Header("Settings")]
	public float minVelocityMagnitude = .05f;
	public float sensitivity = 2f;
	public float maxVelocity = 8;
	public float movementForce = 50f;

	[Header("UI Fields")]
	public CanvasGroup gameUI;
	public CanvasGroup menuUI;
	public TextMeshProUGUI score;
	public TextMeshProUGUI timer;



	public Animation flash;
	public TextMeshProUGUI livesText;
	public TextMeshProUGUI healthText;

	private Vector3 _targetPosition, _velocity;
	private Rigidbody _rigidbody;

	private Animation _animation;
	private Animator _animator;


	[Header("Animation Fields")]
	public Animation scoreAnimation;
	public Animation timerAnimation;

	void Awake()
	{
		Application.targetFrameRate = 60;
		QualitySettings.SetQualityLevel(3);
	}
	void Start()
	{
		_targetPosition = transform.position;
		_rigidbody = GetComponent<Rigidbody>();
		_animation = GetComponent<Animation>();
		_animator = GetComponent<Animator>();
		livesText.text = "Health " + _lives.ToString();
		gameUI.alpha = 0f;
		menuUI.alpha = 1f;
	}


	void Update()
	{
		if (GameController.Instance.LoseControls)
		{
			_animator.SetBool("isWalking", false);
			return;
		}
		dx = dz = 0;

		if (Input.GetMouseButtonDown(0))
		{
			_pointerDown = Camera.main.ScreenToViewportPoint(Input.mousePosition);

			GameController.Instance.GameStart = true;
			menuUI.alpha = 0f;
			gameUI.alpha = 1f;
		}

		if (GameController.Instance.GameStart)
		{
			_timer -= Time.unscaledDeltaTime;
			_timer = Mathf.Clamp(_timer, 0f, _timer);
			string _prefix = "";
			if (_timer < 10)
			{
				_prefix = "0";
				if (timerAnimation.isPlaying == false)
				{
					timerAnimation.Play();
				}
			}
			timer.text = "0:" + _prefix + (int)(_timer);
			if (_timer <= 0)
			{
				GameController.Instance.GameLose("Timer Up!");
			}
		}

		if (Input.GetMouseButton(0))
		{
			_pointerCurrent = Camera.main.ScreenToViewportPoint(Input.mousePosition);
		}

		if (Input.GetMouseButtonUp(0))
		{
			_pointerDown = _pointerCurrent = Vector3.zero;
		}

		Vector3 v = (_pointerCurrent - _pointerDown) * 2.5f;
		v = Clamp(v, minVelocityMagnitude);

		dx = v.x * movementForce * Time.unscaledDeltaTime;
		dz = v.y * movementForce * Time.unscaledDeltaTime;

		dx = Mathf.Clamp(dx, -maxVelocity, maxVelocity);
		dz = Mathf.Clamp(dz, -maxVelocity, maxVelocity);

		float dy = _rigidbody.velocity.y;
		dy = Mathf.Clamp(dy, _rigidbody.velocity.y, 0f);
		_rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, new Vector3(dx, dy, dz), Time.unscaledDeltaTime * 10f);
		Vector3 vec = _rigidbody.velocity.normalized;
		vec.y = 0f;
		transform.forward = Vector3.Lerp(transform.forward, vec, Time.unscaledDeltaTime * 10f);

		_animator.SetBool("isWalking", _rigidbody.velocity.magnitude > 0f && Input.GetMouseButton(0));
	}


	Vector3 Clamp(Vector3 v, float f)
	{
		float mag = v.magnitude;
		if (mag < f)
		{
			return v.normalized * f;
		}

		return v;
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Objects/Pickup")
		{
			col.gameObject.GetComponent<MeshRenderer>().enabled = false;
			col.gameObject.GetComponent<Collider>().enabled = false;
			col.gameObject.GetComponentInChildren<ParticleSystem>().Play();
			Score += 10;
			score.text = Score.ToString();
			scoreAnimation.Play();
			GameController.Instance.pickUpRemaining--;
			if (GameController.Instance.pickUpRemaining <= 0)
			{
				GameController.Instance.GameWin();
			}
		}
	}

	void OnCollisionEnter(Collision col)
	{

		if (col.gameObject.tag == "Objects/Zombie")
		{
			if (_lives <= 0)
			{

				GameController.Instance.GameLose();
			} else
			{
				_lives--;
				flash.Play();
			}

			//healthText.text = "Health: " + _health;
			livesText.text = "Health: " + _lives;
		}
	}
}
