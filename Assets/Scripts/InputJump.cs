using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputJump : MonoBehaviour
{
	Jumper _jumper;

	void Awake()
	{
		_jumper = GetComponent<Jumper>();
		if (_jumper == null) { Debug.Log("no jumper found", this); }
	}

    void Start()
    {
    }

    void Update()
    {
		if (Input.GetButtonDown("Jump"))
		{
			_jumper.Jump();
		}
    }
}
