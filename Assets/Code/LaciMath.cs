using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LaciMath
{
	private static System.Random rng = new System.Random();

	public static int GetRandomSign()
	{		
		return rng.Next(0, 2) * 2 - 1; 
	}

	public static double GetRandomValue()
	{
		return rng.NextDouble();
	}

	public static bool IsInLayerMask(int layer, LayerMask layermask)
	{
		return layermask == (layermask | (1 << layer));
	}
}
