using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
	public float MaxFloorDistance;
	public float Strength;

	Rigidbody _rigidbody;
	bool _shouldTryJump;
	bool _jumping;
	bool _airborne;

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
				_rigidbody.AddForce(Vector3.up * Strength, ForceMode.Impulse);
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
