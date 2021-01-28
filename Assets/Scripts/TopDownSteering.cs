using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownSteering : MonoBehaviour
{
	public Vector2 GoalDirection; // direction you want to steer towards
	public float SteeringStrength; // how hard we will try to steer towards the goal
	public float Speed;

	private Rigidbody _rigidbody;
	private Vector2 _steeringDirection; // actual steering direction

	void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
		if (_rigidbody == null) { Debug.Log("no Rigidbody component found", this); }

		_steeringDirection = Vector2.zero;
	}

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
		_steeringDirection = NewSteering(_steeringDirection, GoalDirection, SteeringStrength);
    }

	void FixedUpdate()
	{
		_rigidbody.AddForce(SteeringForce(_steeringDirection, Speed));
	}

	Vector3 SteeringForce(Vector3 newSteeringDirection, float speed) {
		return new Vector3(newSteeringDirection.x, 0, newSteeringDirection.y) * speed;
	}

	Vector2 NewSteering(Vector2 oldSteeringDirection, Vector2 newGoalDirection, float lerp) {
		Vector2 truncatedGoal = newGoalDirection;
		if (truncatedGoal.sqrMagnitude > 1.0f) {
			truncatedGoal = truncatedGoal.normalized;
		}
		return Vector2.Lerp(oldSteeringDirection, truncatedGoal, lerp);
	}
}
