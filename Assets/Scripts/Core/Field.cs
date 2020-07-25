using UnityEngine;
using System.Collections;

public class Field : MonoBehaviour, ITouchable, IDragable {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void OnTouch(Vector3 position)
	{
		position.y = 0;
		if(UnitControl.anythingSelected)
			UnitControl.selectedUnit.Move(position);
	}
	
	public void OnDrag(Vector3 position,Vector3 amount)
	{
		
	}
	public void OnMove(Vector3 delta)
	{
		
		
	}
}
