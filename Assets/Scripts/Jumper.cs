using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// make a rigidbody jump
public class Jumper : MonoBehaviour
{
	public float MaxFloorDistance;
	public float Strength;

	Rigidbody _rigidbody;
	bool _shouldTryJump;
	bool _jumping;
	bool _airborne;
	float _fixedUpdatesPerSec;

	public bool Jump()
	{
		if (_jumping || _airborne)
		{
			return false;
		}

		_shouldTryJump = true;
		return true;
	}

	void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
		if (_rigidbody == null) { Debug.Log("no rigidbody for Jumper component", this); }

		_jumping = false;
		_airborne = false;
		_fixedUpdatesPerSec = 1 / Time.fixedDeltaTime;
	}

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

	void FixedUpdate()
	{
		if (_shouldTryJump)
		{
			if (IsWithinJumpDistance(transform.position))
			{
				_jumping = true;
				// according to https://answers.unity.com/questions/49001/how-is-drag-applied-to-force.html
				// drag is basically % velocity lost per fixed update so drag>=1/fixeddeltatime prevents all movement
				// thus as drag approaches that value the jump force must approach infinity
				float dragCompensation = _fixedUpdatesPerSec / Mathf.Max(_fixedUpdatesPerSec - _rigidbody.drag, Mathf.Epsilon);
				float jumpImpulse = Strength * -Physics.gravity.y * dragCompensation;
				_rigidbody.AddForce(Vector3.up * jumpImpulse, ForceMode.Impulse);
			}

			_shouldTryJump = false;
			return;
		}

		if (_jumping)
		{
			bool within = IsWithinJumpDistance(transform.position);

			if (_airborne)
			{
				if (within)
				{
					_airborne = false;
					_jumping = false;
				}
			}
			else
			{
				if (!within)
				{
					_airborne = true;
				}
			}
		}
	}

	// test if floor is within MaxFloorDistance of startPoint
	bool IsWithinJumpDistance(Vector3 startPoint)
	{
		Layers layerMask = Layers.World;

		return Physics.Raycast(startPoint, Vector3.down, MaxFloorDistance, (int)layerMask);
	}
}
