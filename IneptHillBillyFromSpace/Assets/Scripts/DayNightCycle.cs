using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
	public float timeChange = 10.0f;	//The number of seconds between each change in angle
	public float angleChange = 1.0f;	//The amount of change to the Rotation param in Transform per function call
	
	
    void Awake()
	{
		InvokeRepeating( "changeAngle", 0, timeChange );
	}
	
	//----------------------------------------------------------------------------------------------
	// changeAngle - increases the x param in this.transform.Rotation by angleChange
	//
	void changeAngle()
	{
		transform.Rotate( angleChange, 0, 0, Space.Self );
	}
}
