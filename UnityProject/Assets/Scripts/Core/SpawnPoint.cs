using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour
{
	public float spawnFrequencyDelay;
	public float spawnStartDelay;
	public int maxQuantity;
	public int spawnedQuantity;
	public GameObject[] enemyTypes;
	public float TopX;
	public float TopY;
	private float nextSpawnTime;
	
	public void Trigger ()
	{
		if (!enabled) {
			enabled = true;
			nextSpawnTime = Time.timeSinceLevelLoad + spawnStartDelay;
		}
	}
	
	void Spawn ()
	{
		if (spawnedQuantity < maxQuantity) {
			nextSpawnTime = Time.timeSinceLevelLoad + spawnFrequencyDelay;
			GameObject.Instantiate (
				enemyTypes [UnityEngine.Random.Range (0, enemyTypes.Length)],
				transform.position + new Vector3 (UnityEngine.Random.Range (-TopX, TopX), 0, UnityEngine.Random.Range (-TopY, TopY)),
				Quaternion.identity);
			spawnedQuantity++;
		} else
			Destroy (gameObject);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Time.timeSinceLevelLoad > nextSpawnTime)
			Spawn ();		
	}
}
