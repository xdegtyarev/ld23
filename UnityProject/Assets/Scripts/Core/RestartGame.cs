using UnityEngine;
using System.Collections;

public class RestartGame : MonoBehaviour, ITouchable
{
	void Start ()
	{
		collider.enabled = false;
		CameraControl.FinishReached += FinishReachedHandler;
	}
	
	public void FinishReachedHandler ()
	{
		collider.enabled = true;
	}
	
	public void OnTouch (Vector3 touchPoint)
	{
		Application.LoadLevel ("Game");
	}
}
