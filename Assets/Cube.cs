using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour 
{
	
	// Update is called once per frame
	void Update () {
		UpdatePosition();
	}

	void UpdatePosition()
	{
		if( Manager.values == null ||  Manager.values.Count < 1 )
		{
			return;
		}

		foreach( KeyValuePair<string, float> pair in Manager.values )
		{
			if( pair.Key.Contains("positionX") )
			{
				var current = pair.Value;
				if( current != transform.position.x )
				{
					transform.position = Vector3.right * current;
				}
			}
		}
	}
}
