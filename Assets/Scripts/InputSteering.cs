using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSteering : MonoBehaviour
{
	TopDownSteering _steering;

	void Awake()
	{
		_steering = GetComponent<TopDownSteering>();
		if (_steering == null) { Debug.Log("can't find TopDownSteering component", this); }
	}

    void Start()
    {
    }

    void Update()
    {
		_steering.GoalDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
}
