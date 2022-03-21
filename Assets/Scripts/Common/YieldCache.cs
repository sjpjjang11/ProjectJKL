using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal static class YieldCache
{
	public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
	public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();

	private static readonly Dictionary<float, WaitForSeconds> DicTimeInterval = new Dictionary<float, WaitForSeconds>(new FloatCompare());

	public static WaitForSeconds WaitForSeconds(float _seconds)
	{
		WaitForSeconds Wfs;

		if (!DicTimeInterval.TryGetValue(_seconds, out Wfs))
		{
			DicTimeInterval.Add(_seconds, Wfs = new WaitForSeconds(_seconds));
		}
			
		return Wfs;
	}
}
