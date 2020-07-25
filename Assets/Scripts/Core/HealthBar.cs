using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {
	public GameObject hitPointPrefab;
	public Transform bar;
	public float distance;
	public ArrayList hitPointBar = new ArrayList();
	
	void Start()
	{
		bar.parent = transform;
	}
	
	public void Show(int hitPoints)
	{
		foreach(GameObject o in hitPointBar)
			Destroy(o);
		
		float startDistance = -hitPoints/2f*distance+distance/2;
		
		for(int i = 0; i<hitPoints;i++)
		{
			GameObject hp = (GameObject)GameObject.Instantiate(hitPointPrefab,bar.position + new Vector3(startDistance,0f,0f),Quaternion.identity);
			hp.transform.parent = bar;
			hitPointBar.Add(hp);
			startDistance += distance;
		}
	}
	
	void Update()
	{
		transform.rotation = Quaternion.identity;
		bar.rotation = Quaternion.identity;
	}
}