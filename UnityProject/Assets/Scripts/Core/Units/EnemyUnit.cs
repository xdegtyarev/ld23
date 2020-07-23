using UnityEngine;
using System.Collections;

public class EnemyUnit : Unit
{
	public AliasUnit target;
	public float agressionRange;
	
	public override void Start ()
	{
		CameraControl.FinishReached += FinishReachedHandler;
		base.Start ();
	}

	void FinishReachedHandler ()
	{
		Kill ();
	}
	
	void OnDestroy ()
	{
		CameraControl.FinishReached -= FinishReachedHandler;
	}
	
	void Update ()
	{
		CheckEnemiesAround ();
		CheckIsDead();
	}
	
	public void CheckEnemiesAround ()
	{
		if (hitPoints > 0) {
			if (Time.timeSinceLevelLoad > nextCheck) {
				if (target == null || target.hitPoints <= 0) {
					foreach (var o in Physics.OverlapSphere(transform.position,agressionRange))
						if (o.tag == "Alias") {
							target = o.GetComponent<AliasUnit> ();
							break;
						}	
				}
				ReachTarget ();
				nextCheck = Time.timeSinceLevelLoad + checkDelay;
			}
		}
	}
	
	public void ReachTarget ()
	{
		if (hitPoints > 0) {
			if (target == null) {
				Vector3 targ = Camera.mainCamera.ScreenToWorldPoint (new Vector2 (Screen.width / 2f, Screen.height / 2f));
				targ.y = 0;
				Move (targ);
			} else {
				if (Vector3.Distance (target.transform.position, transform.position) > weapon.attackRange) {
					Move (target.transform.position);
				} else {
					iTween.Stop (gameObject);
					Attack ();
				}
			}
		}
	}
	
	public void Attack ()
	{
		if (hitPoints > 0) {
			if (target.hitPoints > 0)
			if (Time.timeSinceLevelLoad > nextAttack) {
				renderer.PlayAnim ("Attack");
				transform.LookAt (target.transform.position);
				Debug.DrawLine (transform.position, target.transform.position, Color.blue, weapon.attackDelay);
				weapon.Shoot (target);
				nextAttack = Time.timeSinceLevelLoad + weapon.attackDelay;
			}
		}
	}
	
	public override void Die ()
	{
		Statistics.EnemiesKilled++;
		base.Die ();
	}
	
	
}
