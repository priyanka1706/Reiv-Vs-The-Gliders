using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

	public float fireRate = 0;
	public int Damage = 25;
	// Layer mask type is a type of layer
	// So we can check off which layer we want our player to hit, which is everything except our player, and ignore raycast (automatically generated)
	public LayerMask whatToHit;

    public bool hitEnemy = false;

	public Transform BulletTrailPrefab;
    public Transform HitPrefab;
	float timeToSpawnEffect = 0;
	public float effectSpawnRate = 10;

	public Transform muzzleFlashPrefab;

	private float timeToFire = 0;
	Transform firePoint;

	void Awake(){
		firePoint = transform.Find ("FirePoint");
		if (firePoint == null) {
			Debug.LogError ("No firepoint. Whoops!");
		}
	}
	
	// Update is called once per frame
	void Update () {
		//single burst
		if (fireRate == 0) {
			if (Input.GetButtonDown ("Fire1")) {
				Shoot ();
			}
		} else {
			if (Input.GetButton ("Fire1") && Time.time > timeToFire) {
				timeToFire = Time.time + 1 / fireRate;
				Shoot ();
			}
		}
	}

	void Shoot(){

        hitEnemy = false;

		Vector2 mousePos = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);
		//screen cord are now positions in the world

		Vector2 firePointPos = new Vector2 (firePoint.position.x, firePoint.position.y);
		//pos of firepoint, ie at the tip of the gun

		RaycastHit2D hit = Physics2D.Raycast (firePointPos, mousePos-firePointPos, 100, whatToHit);
		// we need origin, direction, distance it will go and a layer cast (what all to hit)

		Debug.DrawLine (firePointPos, (mousePos-firePointPos)*100, Color.cyan); //MAKE SURE TO TURN GIZMOS ON IN GAMEPLAY

		if (hit.collider != null) {
			Debug.DrawLine (firePointPos, hit.point, Color.red);
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null) {
                enemy.DamageEnemy(Damage);
                Debug.Log("We hit " + hit.collider.name + " and did " + Damage + " damage." + hitEnemy);
                hitEnemy = true;
                Debug.Log("Later" + hitEnemy);
                GameMaster.DamageEnemy(this);
            }
		}

        //READ ABT OBJECT POOLING!!!!!!
        if (Time.time >= timeToSpawnEffect)
        {
            Vector3 hitPos;
            Vector3 hitNormal;

            if (hit.collider == null)
            {
                hitPos = (mousePos - firePointPos) * 30;
                hitNormal = new Vector3(9999, 9999, 9999);
            }
            else
            {
                hitPos = hit.point;
                hitNormal = hit.normal;
            }

            Effect(hitPos,hitNormal);
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }

    }

    void Effect(Vector3 hitPos, Vector3 hitNormal){
		Transform trail = Instantiate (BulletTrailPrefab, firePoint.position, firePoint.rotation) as Transform;
        LineRenderer lr = trail.GetComponent<LineRenderer>();

        if(lr != null)
        {
            lr.SetPosition(0, firePoint.position);
            lr.SetPosition(1, hitPos);
        }

        Destroy(trail.gameObject, 0.04f);

        if(hitNormal != new Vector3(9999,9999,9999))
        {
            Transform hitParticle = Instantiate(HitPrefab,hitPos,Quaternion.FromToRotation(Vector3.right,hitNormal)) as Transform;
            Destroy(hitParticle.gameObject, 1f);
        }

        Transform clone = Instantiate (muzzleFlashPrefab, firePoint.position, firePoint.rotation) as Transform;
		clone.parent = firePoint;
		float size = Random.Range (0.6f, 0.9f);
		clone.localScale = new Vector3 (size, size, size);
		//yield return 0;
		Destroy (clone.gameObject, 0.02f);
	}
}