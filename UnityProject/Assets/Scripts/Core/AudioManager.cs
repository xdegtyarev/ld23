using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {
	
	public static bool created = false;
	public Dictionary<string,AudioClip> audioBase;
	public AudioClip[] audioClips;
	public static AudioManager current;
	
	void Start()
	{
		if(created) Destroy(gameObject);
		created = true;
		
		audioBase = new Dictionary<string, AudioClip>();
		current = this;
		foreach(var o in audioClips)
			audioBase.Add(o.name,o);
		DontDestroyOnLoad(gameObject);
	}
	
	public static void Play(string clipName)
	{
		AudioClip _clip;
		if(current.audioBase.TryGetValue(clipName,out _clip)){
			AudioSource.PlayClipAtPoint(_clip,Vector3.zero);
		}
		else
			Debug.LogError("No Audio file with name:" + clipName);
	}
	
}
