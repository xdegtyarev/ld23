using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
	public Vector3 pos;
	public float speed;

	public static event System.Action FinishReached;
	// Use this for initialization
	void Start ()
	{
		iTween.MoveTo (gameObject, iTween.Hash ("position", pos, "speed", speed, "easetype", iTween.EaseType.linear, "oncomplete", "MoveCompleteHandler", "oncompletetarget", gameObject));
	}
	
	public void MoveCompleteHandler ()
	{
		if (FinishReached != null)
			FinishReached ();
		Debug.Log ("FINISH");
		gameObject.guiText.text = "Enemies killed " + Statistics.EnemiesKilled + " Alias killed: " + Statistics.AliasKilled + " AliasSaved: " + Statistics.AliasSaved + "\n Touch to restart";	
	}
	
}
