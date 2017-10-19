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

		foreach( KeyValuePair<string, List<object>> pair in Manager.values )
		{
			if( pair.Key.Contains("positionXYZ") )
			{
				var fValues = new List<float>();
				var current = pair.Value;
				for( int i=0 ; i<current.Count ; ++i )
				{
					float fVal;
					if( float.TryParse( current[i].ToString(), out fVal ) )
					{
						fValues.Add( fVal );
					}
				}
					
				var newPos = new Vector3( fValues[0], fValues[1], fValues[2] );
				if( newPos != transform.position )
				{
					transform.position = newPos;
				}
			}
		}
	}
}
