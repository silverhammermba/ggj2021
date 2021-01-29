using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteerTowardTarget : MonoBehaviour
{
	public GameObject Target;

	TopDownSteering _steering;

	void Awake()
	{
		_steering = GetComponent<TopDownSteering>();
		if (_steering == null) { Debug.Log("no top down steering on steer toward target!", gameObject); }
	}

    // Update is called once per frame
    void Update()
    {
		if (Target != null)
		{
			var diff = Target.transform.position - transform.position;
			_steering.GoalDirection = new Vector2(diff.x, diff.z);
		}
    }
}
