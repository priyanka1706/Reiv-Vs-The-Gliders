using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

	public static GameMaster gm;

    [SerializeField]
    private int maxLives = 3;
    private static int _remainingLives = 3;
    public static int RemainingLives
    {
        get { return _remainingLives; }
    }
    [SerializeField]
    private static int _currentScore = 0;
    public static int CurrentScore
    {
        get { return _currentScore; }
    }
    private AudioManager audioManager;

    void Awake()
    {
        if (gm == null)
        {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        }

        _remainingLives = maxLives;
        _currentScore = 0;
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("No Audio Manager found in the scene");
        }
    }

    public Transform playerPrefab;
	public Transform spawnPoint;
	public float spawnDelay = 2f;
	public GameObject spawnPrefab;

    public string spawnSoundName;

    [SerializeField]
    private GameObject gameOverUI;

    public void EndGame()
    {
        Debug.Log("GAME OVER");
        gameOverUI.SetActive(true);
    }

    public IEnumerator _RespawnPlayer(){
		GetComponent<AudioSource> ().Play ();
		yield return new WaitForSeconds (spawnDelay);

		Instantiate (playerPrefab, spawnPoint.position, spawnPoint.rotation);
		GameObject clone = Instantiate (spawnPrefab, spawnPoint.position, spawnPoint.rotation) as GameObject;
		Destroy (clone, 3f);
	}

	public static void KillPlayer(Player player){
		Destroy (player.gameObject);
        _remainingLives -= 1;
        if (_remainingLives <= 0)
        {
            gm.EndGame();
        }
        else
        {
            gm.StartCoroutine(gm._RespawnPlayer());
        }
    }

    public static void DamageEnemy(Weapon weapon)
    {
        Debug.Log("Is it getting here");
        Debug.Log("GM Damage enemy this one" + _currentScore);
        if (weapon.hitEnemy == true)
        {
            _currentScore += 10;
        }
    }

    public static void KillEnemy(Enemy enemy) {
        gm._KillEnemy(enemy);
    }

    public void _KillEnemy(Enemy _enemy)
    {
        Transform clone = Instantiate(_enemy.deathParticles,_enemy.transform.position,Quaternion.identity) as Transform;
        Destroy(clone, 5f);
        Destroy(_enemy.gameObject);
    }
}
