using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour
{
	public UnitType type;
	public float speed;
	public int hitPoints;
	public float checkDelay;
	public PackedSprite renderer;
	public Weapon weapon;
	public HealthBar healthBar;
	protected float nextAttack;
	protected float nextCheck;
	public float maxDistanceFromCamera;

	public void Kill ()
	{
		if (hitPoints > 0) {
			hitPoints = 0;
			Die ();
		}
	}
	
	public virtual void Start ()
	{
		healthBar.Show (hitPoints);
	}
	
	public virtual void Move (Vector3 pos)
	{
		if (hitPoints > 0) {
			renderer.PlayAnim ("Run");
			transform.LookAt (pos);
			iTween.MoveTo (gameObject, iTween.Hash ("position", pos, "speed", speed, "easetype", iTween.EaseType.linear, "oncomplete", "MoveCompleteHandler", "oncompletetarget", gameObject));
		}
	}
	
	public void CheckIsDead ()
	{
		if (hitPoints <= 0)
		if (Camera.mainCamera.transform.position.z > transform.position.z)
		if (Camera.mainCamera.transform.position.z - transform.position.z > maxDistanceFromCamera)
			Destroy (gameObject);
	}
	
	public virtual void MoveCompleteHandler ()
	{
		renderer.PlayAnim ("Idle");
	}
	
	public void Hit (int damage)
	{
		hitPoints -= damage;
		healthBar.Show (hitPoints);
		switch (type) {
		case UnitType.Cockroach:
			AudioManager.Play ("CockroachHit");
			break;
		case UnitType.Marine:
			AudioManager.Play ("ManWounded");
			break;
		
		case UnitType.RocketMan:
			AudioManager.Play ("ManWounded");
			break;
		
		case UnitType.Scientist:
			AudioManager.Play ("ManWounded");
			break;
		case UnitType.Wasp:
			AudioManager.Play ("WaspHit");
			break;
		
			
		}
		
		
		if (hitPoints <= 0)
			Die ();
	}
	
	public virtual void Die ()
	{
		switch (type) {
		case UnitType.Cockroach:
			AudioManager.Play ("CockroachDeath");
			break;
		case UnitType.Marine:
			AudioManager.Play ("ManDeath");
			break;
		
		case UnitType.RocketMan:
			AudioManager.Play ("ManDeath");
			break;
		
		case UnitType.Scientist:
			AudioManager.Play ("ManDeath");
			break;
		case UnitType.Wasp:
			AudioManager.Play ("WaspHit");
			break;
		}
		renderer.transform.position = renderer.transform.position - new Vector3 (0f, 0.025f, 0f);
		iTween.Stop (gameObject);
		renderer.PlayAnim ("Death");
		collider.enabled = false;	
	}
	
}

public enum UnitType
{
	Marine,
	RocketMan,
	Scientist,
	Cockroach,
	Wasp
}
