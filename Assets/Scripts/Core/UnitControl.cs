using UnityEngine;
using System.Collections;

public static class UnitControl {
	public static AliasUnit selectedUnit;
	public static bool anythingSelected{get{return selectedUnit != null;}}
	
	public static void RegisterSelection(this AliasUnit selected)
	{
		if(anythingSelected)
			selectedUnit.Deselect();
		selectedUnit = selected;
	}
	
	public static void RegisterDeselection(this AliasUnit unit)
	{
		selectedUnit = null;
	}
}
