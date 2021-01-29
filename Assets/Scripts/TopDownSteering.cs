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
	float _steeringDirectionR; // direction we are steering in radians
	float _steeringSpeed; // how hard we are moving in steering direction

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

		if (Visuals == null)
		{
			Debug.Log("you forgot to set the visuals for steering", this);
		}
		else
		{
			_steeringDirectionR = Angle.Radians(Visuals.transform.localEulerAngles.y);
		}

		_steeringSpeed = 0;
	}

    void Start()
    {
    }

    void Update()
    {
		var (dir, sp) = NewSteering(_steeringDirectionR, _steeringSpeed, GoalDirection, SteeringStrength);
		_steeringDirectionR = dir;
		_steeringSpeed = sp;

		if (_steeringSpeed > 0)
		{
			Visuals.transform.localEulerAngles = FacingEulerAngles(_steeringDirectionR);
		}
    }

	void FixedUpdate()
	{
		_rigidbody.AddForce(SteeringForce(_steeringDirectionR, Speed * _steeringSpeed), ForceMode.VelocityChange);
		_rigidbody.velocity = DragXZVelocity(XZDrag, _rigidbody.velocity);
	}

	// mimic unity's drag calculation on velocity, but only in the XZ plane
	static Vector3 DragXZVelocity(float drag, Vector3 currentVelocity)
	{
		float multiplier = 1.0f - drag * Time.fixedDeltaTime;
		if (multiplier < 0.0f) multiplier = 0.0f;
		return Vector3.Scale(currentVelocity, new Vector3(multiplier, 1.0f, multiplier));
	}

	static Vector3 FacingEulerAngles(float newSteeringDirectionR)
	{
		return new Vector3(0, Angle.Degrees(newSteeringDirectionR), 0);
	}

	static Vector3 SteeringForce(float newSteeringDirectionR, float speed)
	{
		return new Vector3(Mathf.Cos(newSteeringDirectionR), 0, -Mathf.Sin(newSteeringDirectionR)) * speed;
	}

	static (float newDirectionR, float newSpeed) NewSteering(float oldSteeringDirectionR, float oldSpeed, Vector2 newGoalDirection, float lerp)
	{
		// first cap goal at 1 magnitude
		Vector2 truncatedGoal = newGoalDirection;
		if (truncatedGoal.sqrMagnitude > 1.0f) {
			truncatedGoal = truncatedGoal.normalized;
		}

		// convert that to angle, defaulting to current facing if 0
		float goalRadians = oldSteeringDirectionR;
		if (truncatedGoal.sqrMagnitude > 0)
		{
			goalRadians = -Angle.Radians(truncatedGoal);
		}

		// find difference between old facing and goal facing
		float diffR = Angle.RadianDiff(oldSteeringDirectionR, goalRadians);

		// lerp facing and magnitude separately
		return (oldSteeringDirectionR + Mathf.Lerp(0, diffR, lerp), Mathf.Lerp(oldSpeed, truncatedGoal.magnitude, lerp));
	}
}
