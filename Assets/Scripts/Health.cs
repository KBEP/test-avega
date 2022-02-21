using UnityEngine;
using System;

//должен висеть на том же объекте, что и коллайдер, принимающий урон от пуль
public class Health : MonoBehaviour
{
	public static readonly int minValue = 0;
	public static readonly int maxValue = 100;
	
	int value = 100;
	
	public int Value
	{
		get => value;
		set
		{
			int previousValue = this.value;
			this.value = Mathf.Clamp(value, minValue, maxValue);
			if (IsDead && Dead != null) Dead();
			else if (this.value < previousValue && Damaged != null) Damaged();
			//--else if (this.value > previousValue && Healed != null) Healed();
		}
	}

	public bool IsDead => value <= minValue;

	public event Action Dead;
	public event Action Damaged;
	//--public event Action Healed;
}
