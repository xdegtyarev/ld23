using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
	public WeaponType type;
	public float shellSpeed;
	public int damage;
	public float explosionRange;
	public float attackRange;
	public float attackDelay;
	public GameObject shell;
	
	public void Shoot (Unit target)
	{
		if (Vector3.Distance (target.transform.position, transform.position) < attackRange)
			switch (type) {
			case WeaponType.melee:
				//shitcode
				if (tag == "Alias")
					AudioManager.Play ("CockroachAttack");
				else if (tag == "Enemy")
					AudioManager.Play ("CockroachAttack");
			//end
				target.Hit (damage);
				break;
			case WeaponType.exploding:
			//shitcode
				if (tag == "Alias")
					AudioManager.Play ("WaspMissleLaunch");
				else if (tag == "Enemy")
					AudioManager.Play ("RocketmanMissleLaunch");
			//end
				GameObject shellInstance = (GameObject)GameObject.Instantiate (shell, transform.position, Quaternion.identity);
				shellInstance.transform.LookAt (target.transform.position);
				iTween.MoveTo (shellInstance, iTween.Hash ("position", target.transform.position, "speed", shellSpeed, "easetype", iTween.EaseType.linear, "oncomplete", "ShootExplodingHandler", "oncompletetarget", gameObject, "oncompleteparams", shellInstance));
				break;
			case WeaponType.range:
				AudioManager.Play ("MarineShoot");
				GameObject shellRangeInstance = (GameObject)GameObject.Instantiate (shell, transform.position, Quaternion.identity);
				shellRangeInstance.transform.LookAt (target.transform.position);
				Destroy (shellRangeInstance, Vector3.Distance (transform.position, target.transform.position) / shellSpeed + 0.1f);
				iTween.MoveTo (shellRangeInstance, iTween.Hash ("position", target.transform.position, "speed", shellSpeed, "easetype", iTween.EaseType.linear, "oncomplete", "ShootRangeHandler", "oncompletetarget", gameObject, "oncompleteparams", target));
				break;
			}
		
	}
	
	public void ShootRangeHandler (object target)
	{
		
		((Unit)target).Hit (damage);
	}
	
	public void ShootExplodingHandler (object shellInstance)
	{
		GameObject shellInst = (GameObject)shellInstance; 
		PackedSprite shellSprite = shellInst.GetComponentInChildren<PackedSprite> ();
		shellSprite.PlayAnim ("Explode");
		//Shitcode
		if (tag == "Enemy")
			AudioManager.Play ("RocketmanMissleExplosion");
		else if (tag == "Alias")
			AudioManager.Play ("WaspMissleExplosion");
			
			
		iTween.ShakePosition (Camera.mainCamera.gameObject, Vector3.forward * 0.05f, 0.5f);
		Destroy (shellInst, 1f);
		foreach (var o in Physics.OverlapSphere(shellInst.transform.position,explosionRange))
			if (o.tag == tag)
				o.GetComponent<Unit> ().Hit (damage);
	}
}

public enum WeaponType
{
	melee,
	range,
	exploding
}