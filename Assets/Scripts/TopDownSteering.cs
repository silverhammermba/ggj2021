using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// steers a rigidbody towards a given direction
public class TopDownSteering : MonoBehaviour
{
	public Vector2 GoalDirection; // direction you want to steer towards
	public float SteeringStrength; // how hard we will try to steer towards the goal
	public float Speed; // multiplier applied to movement
	public float XZDrag; // drag applied only in the XZ plane
	public GameObject Visuals; // object that will be rotated to match steering

	Rigidbody _rigidbody;
	Vector2 _steeringDirection; // actual steering direction

	void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
		if (_rigidbody == null)
		{
			Debug.Log("no Rigidbody component found", this);
		}
		else
		{
			if (_rigidbody.drag > 0)
			{
				Debug.Log("rigidbody with topdownsteering has nonzero drag. this will behave badly", gameObject);
			}
		}

		if (Visuals == null) { Debug.Log("you forgot to set the visuals for steering", this); }

		_steeringDirection = Vector2.zero;
	}

    void Start()
    {
    }

    void Update()
    {
		_steeringDirection = NewSteering(_steeringDirection, GoalDirection, SteeringStrength);
		Visuals.transform.localEulerAngles = FacingEulerAngles(_steeringDirection);
    }

	void FixedUpdate()
	{
		_rigidbody.AddForce(SteeringForce(_steeringDirection, Speed), ForceMode.VelocityChange);
		_rigidbody.velocity = DragXZVelocity(XZDrag, _rigidbody.velocity);
	}

	// mimic unity's drag calculation on velocity, but only in the XZ plane
	Vector3 DragXZVelocity(float drag, Vector3 currentVelocity)
	{
		float multiplier = 1.0f - drag * Time.fixedDeltaTime;
		if (multiplier < 0.0f) multiplier = 0.0f;
		return Vector3.Scale(currentVelocity, new Vector3(multiplier, 1.0f, multiplier));
	}

	Vector3 FacingEulerAngles(Vector2 newSteeringDirection)
	{
		return new Vector3(0, -Mathf.Atan2(_steeringDirection.y, _steeringDirection.x) * 180.0f / Mathf.PI, 0);
	}

	Vector3 SteeringForce(Vector3 newSteeringDirection, float speed)
	{
		return new Vector3(newSteeringDirection.x, 0, newSteeringDirection.y) * speed;
	}

	Vector2 NewSteering(Vector2 oldSteeringDirection, Vector2 newGoalDirection, float lerp)
	{
		// TODO: if the new goal flips direction, this lerps through 0, which doesn't look good
		// it would be nicer to somehow lerp *around* 0 so you get a turn instead of an instantaneous change
		// i think you want to separately lerp rotation and magnitude
		Vector2 truncatedGoal = newGoalDirection;
		if (truncatedGoal.sqrMagnitude > 1.0f) {
			truncatedGoal = truncatedGoal.normalized;
		}
		return Vector2.Lerp(oldSteeringDirection, truncatedGoal, lerp);
	}
}
