using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Diggable : MonoBehaviour
{
	public float TotalWork;
	public float DiggerShoveStrength;

	public GameController Controller;
	public Treasure ContentsPrefab;
	public int Quantity;

	float _workDone;
	Digger _digger;
	Canvas _digUI;
	Image _progressImage;

	void Awake()
	{
		_digUI = GetComponentInChildren<Canvas>();
		if (_digUI == null) { Debug.Log("no canvas component in diggable children", gameObject); }

		_progressImage = _digUI.GetComponentInChildren<Image>();
		if (_progressImage == null) { Debug.Log("no image component in diggable canvas children", gameObject); }

		_workDone = 0;
		_digger = null;
	}

	void Start()
	{
		if (Controller == null) { Debug.Log("no manager to track this diggable!", gameObject); }
	}

	public void StartDigging(Digger digger)
	{
		_digger = digger;
		_digUI.enabled = true;
	}

	public void StopDigging()
	{
		_digger = null;
	}

    void Update()
    {
		if (_digger != null)
		{
			_workDone += _digger.DigSpeed * Time.deltaTime;

			if (_workDone >= TotalWork)
			{
				FinishDigging();
			}
			else
			{
				_progressImage.fillAmount = 1.0f - _workDone / TotalWork;
			}
		}
    }

	void FinishDigging()
	{
		if (Quantity > 0)
		{
			var rigidbody = _digger.GetComponent<Rigidbody>();
			if (rigidbody != null)
			{
				float fixedUpdatesPerSec = 1 / Time.fixedDeltaTime;
				float dragCompensation = fixedUpdatesPerSec / Mathf.Max(fixedUpdatesPerSec - rigidbody.drag, Mathf.Epsilon);
				float jumpImpulse = DiggerShoveStrength * Mathf.Sqrt(-Physics.gravity.y) * dragCompensation;

				rigidbody.AddForce(Vector3.up * jumpImpulse, ForceMode.Impulse);
			}
		}

		_digger.OutOfRange(this);

		for (var i = 0; i < Quantity; ++i)
		{
			var treasure = Object.Instantiate(ContentsPrefab, transform.position, Quaternion.identity).GetComponent<Treasure>();
			treasure.Controller = Controller;
		}

		Destroy(gameObject);
	}

	void OnTriggerEnter(Collider other)
	{
		var digger = other.GetComponent<Digger>();
		if (digger == null) return;

		digger.InRange(this);
	}

	void OnTriggerExit(Collider other)
	{
		var digger = other.GetComponent<Digger>();
		if (digger == null) return;

		digger.OutOfRange(this);
		_digUI.enabled = false;
	}
}
