using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	[System.Serializable]
	public class PlayerStats
	{
		public int maxHealth = 100;

        private int _curHealth;
        public int curHealth
        {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public void Init()
        {
            curHealth = maxHealth;
        }
	}

	//Named here as playerStatsObj, not playerStats so as to avoid confusion :)
	public PlayerStats stats = new PlayerStats();
	public int fallBoundary = -20;

    [SerializeField]
    private StatusIndicator statusIndicator;

    void Start()
    {
        stats.Init();

        if (statusIndicator == null)
        {
            Debug.LogError("No status indicator referenced on player code");
        }
        else
        {
            statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
        }
    }

	void Update(){
		// if he falls down, player damaged at infinity, therefore killed
		if (transform.position.y <= fallBoundary) {
			DamagePlayer (9999999);
		}
	}

	public void DamagePlayer(int damage){
		stats.curHealth -= damage;
		if (stats.curHealth <= 0) {
			GameMaster.KillPlayer(this);
		}

        statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);

    }

}
