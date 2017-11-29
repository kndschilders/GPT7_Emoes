// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using UnityEngine;

[CreateAssetMenu]
public class PlayerStateVariable : ScriptableObject
{
	#if UNITY_EDITOR
	[Multiline]
	public string DeveloperDescription = "";
	#endif
	public PlayerState Value;

	public void SetValue(PlayerState value)
	{
		Value = value;
	}

	public void SetValue(PlayerStateVariable value)
	{
		Value = value.Value;
	}

}