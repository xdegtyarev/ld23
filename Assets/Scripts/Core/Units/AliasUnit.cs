using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AliasUnit : Unit, ITouchable
{
	public bool isSelected;
	public EnemyUnit target;
	public PackedSprite selection;
	
	// Update is called once per frame
	public override void Start ()
	{
		base.Start ();
		CameraControl.FinishReached += FinishReachedHandler;	
		selection.Hide (true);
	}

	void OnDestroy ()
	{
		CameraControl.FinishReached -= FinishReachedHandler;
	}
	
	public void FinishReachedHandler ()
	{
		if (hitPoints > 0)
			Statistics.AliasSaved++;
	}
	
	void Update ()
	{
		CheckEnemiesAround ();
		Attack ();
		CheckIsDead();
	}
	
	void CheckPosition ()
	{
		if (Vector3.Distance (transform.position, Camera.mainCamera.transform.position) >= maxDistanceFromCamera)
		if (transform.position.z < Camera.mainCamera.transform.position.z) {
			Statistics.AliasKilled++;	
			Kill ();	
		}
	}
	
	public void CheckEnemiesAround ()
	{
		if (hitPoints > 0) {
			CheckPosition ();
			target = null;
			if (Time.timeSinceLevelLoad > nextCheck) {
				foreach (var o in Physics.OverlapSphere(transform.position,weapon.attackRange))
					if (o.tag == "Enemy")
						target = o.GetComponent<EnemyUnit> ();
				nextCheck = Time.timeSinceLevelLoad + checkDelay;
			}
		}
	}
	
	public void Attack ()
	{
		if (hitPoints > 0) {
			if (target != null)
			if (Time.timeSinceLevelLoad > nextAttack) {
				renderer.PlayAnim ("Attack");
				transform.LookAt (target.transform.position);
				weapon.Shoot (target);
				nextAttack = Time.timeSinceLevelLoad + weapon.attackDelay;
			}
		}
	}
	
	public void OnTouch (Vector3 touchPoint)
	{
		if (isSelected)
			Deselect ();
		else
			Select ();
	}
	
	public void Select ()
	{
		if (hitPoints > 0)
			CheckEnemiesAround ();
		isSelected = true;
		selection.Hide (false);
		selection.PlayAnim ("Select");
		this.RegisterSelection ();
	}
	
	public void Deselect ()
	{
		isSelected = false;
		selection.Hide (true);
		this.RegisterDeselection ();
	}
	
	public override void Move (Vector3 pos)
	{
		audio.Play ();
		base.Move (pos);	
	}
	
	public override void MoveCompleteHandler ()
	{
		audio.Pause ();
		base.MoveCompleteHandler ();
	}
	
	public override void Die ()
	{
		if (isSelected)
			Deselect ();
		audio.Pause ();
		selection.Hide (true);
		Statistics.AliasKilled++;
		base.Die ();
	}
}
