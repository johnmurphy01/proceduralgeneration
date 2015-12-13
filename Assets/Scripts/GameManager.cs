using UnityEngine;
using System.Collections;
using FPSControl;
using UnityStandardAssets.Characters.FirstPerson;

public class GameManager : MonoBehaviour {
	public Maze mazePrefab;
	//public Player playerPrefab;
	public UnityStandardAssets.Characters.FirstPerson.FirstPersonController playerPrefab;
	private Maze mazeInstance;
	//private Player playerInstance;
	private UnityStandardAssets.Characters.FirstPerson.FirstPersonController playerInstance;

	// Use this for initialization
	void Start () {
		StartCoroutine(BeginGame ());
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			RestartGame();
		}
	}

	private IEnumerator BeginGame(){
		//Camera.main.rect = new Rect (0f, 0f, 1f, 1f);
		mazeInstance = Instantiate (mazePrefab) as Maze;
		yield return StartCoroutine(mazeInstance.Generate ());
		playerInstance = Instantiate (playerPrefab) as UnityStandardAssets.Characters.FirstPerson.FirstPersonController;
		var rand = mazeInstance.RandomCoordinates;

		var fpc = GameObject.FindObjectOfType<FirstPersonController>();
		fpc.m_WalkSpeed = (3);
		fpc.m_RunstepLenghten = 0.5f;
		playerInstance.transform.localScale = new Vector3 (0.3f, 0.3f, 0.3f);
		playerInstance.transform.position = new Vector3(rand.x,5.0f,rand.z);
		//mazeInstance.GetCell(mazeInstance.RandomCoordinates);
		//playerInstance.SetLocation (mazeInstance.GetCell (mazeInstance.RandomCoordinates));
		//Camera.main.rect = new Rect (0f, 0f, 0.5f, 0.5f);
	}

	private void RestartGame(){
		StopAllCoroutines ();
		Destroy (mazeInstance.gameObject);
		if (playerInstance != null) {
			Destroy(playerInstance.gameObject);
		}
		StartCoroutine(BeginGame ());
	}
}
