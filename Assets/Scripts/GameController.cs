using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameController : MonoBehaviour
{
	public static GameController Instance;
	public bool GameStart;
	public bool LoseControls;
	public int pickUpRemaining;

	public TextMeshProUGUI gameLoseTextMessage;
	public CanvasGroup gameWinUI;
	public CanvasGroup gameLoseUI;
	public ParticleSystem confetti;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		} else
		{
			DestroyImmediate(gameObject);
		}

	}
	void Start()
	{

	}

	void Update()
	{
		if (_check)
		{
			if (Input.GetMouseButtonDown(0))
			{
				Reload();
			}
		}
	}
	public void Reload()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}

	private bool _check;
	// Update is called once per frame
	public void GameWin()
	{
		LoseControls = true;
		gameWinUI.alpha = 1f;
		gameWinUI.transform.GetChild(1).transform.GetComponent<Animation>().Play();
		confetti.Play();
		_check = true;
	}

	public void GameLose(string text = "Game Over :(")
	{
		gameLoseTextMessage.text = text;
		LoseControls = true;
		gameLoseUI.alpha = 1f;
		gameLoseUI.transform.GetChild(1).transform.GetComponent<Animation>().Play();
		_check = true;
	}
}
