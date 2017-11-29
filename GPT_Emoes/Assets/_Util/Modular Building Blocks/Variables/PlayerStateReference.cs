// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using System;

[Serializable]
public class PlayerStateReference
{
	public bool UseConstant = true;
	public PlayerState ConstantValue;
	public PlayerStateVariable Variable;

	public PlayerStateReference()
	{ }

	public PlayerStateReference(PlayerState value)
	{
		UseConstant = true;
		ConstantValue = value;
	}

	public PlayerState Value
	{
		get { return UseConstant ? ConstantValue : Variable.Value; }
	}

	public static implicit operator PlayerState(PlayerStateReference reference)
	{
		return reference.Value;
	}
}

