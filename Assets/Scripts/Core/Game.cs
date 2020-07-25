using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour
{
	public GameObject[] levels;
	public float activationRange;
	public float levelCheckFrequency;
	float nextCheck;

	void InitLevel ()
	{
		for (int i = 0; i<levels.Length; i++) {
			GameObject go = (GameObject)GameObject.Instantiate (levels [i]);
			go.transform.position = new Vector3 (0f, 0.05f, go.GetComponent<Sprite> ().height * i);
		}		
	}
	
	void Start ()
	{
		InitLevel ();
		TouchManager.tapEvent += OnTap;
	}
	
	void Update ()
	{
		if (Time.timeSinceLevelLoad > nextCheck) {
			foreach (var o in Physics.OverlapSphere(Camera.mainCamera.transform.position,activationRange)) {
				if (o.tag == "Spawn") {
					o.GetComponent<SpawnPoint> ().Trigger ();		
				}
			}
			nextCheck = Time.timeSinceLevelLoad + levelCheckFrequency;
		}
		if(!audio.isPlaying)
			audio.Play();
	}
	
	void OnDestroy ()
	{
		TouchManager.tapEvent -= OnTap;
	}
	
	void OnTap (TouchItem touchItem)
	{
		RaycastHit hitInfo = new RaycastHit ();
		if (Physics.Raycast (Camera.mainCamera.ScreenPointToRay (touchItem.position), out hitInfo))
		if (hitInfo.transform.GetComponent (typeof(ITouchable)) != null)
			((ITouchable)hitInfo.transform.GetComponent (typeof(ITouchable))).OnTouch (Camera.mainCamera.ScreenToWorldPoint (touchItem.position));
	}
	
}
