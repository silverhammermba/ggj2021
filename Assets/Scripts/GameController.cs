using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public GameObject DigSpotPrefab;
	public GameObject CannonBallPrefab;
	public int DigSpotsSqrRoot;
	public float DigSpotRandomRadius;
	public float DigSpotSpacing;

	void Awake()
	{
		if (DigSpotPrefab == null) { Debug.Log("You forgot to set the dig spot prefab", gameObject); }
		if (CannonBallPrefab == null) { Debug.Log("You forgot to set the cannon ball prefab", gameObject); }
	}

    // Start is called before the first frame update
    void Start()
    {
		var digPositions = DigPositions(DigSpotsSqrRoot, DigSpotSpacing);
		foreach (var pos in digPositions)
		{
			Object.Instantiate(DigSpotPrefab, pos, Quaternion.identity);
		}
    }

    // Update is called once per frame
    void Update()
    {
    }

	// construct list of dig positions, will all be at Y=0
	List<Vector3> DigPositions(int sqrRoot, float spacing)
	{
		float corner = ((sqrRoot - 1) * spacing) / -2.0f;

		var positions = new List<Vector3>();

		for (int x = 0; x < sqrRoot; ++x)
		{
			for (int y = 0; y < sqrRoot; ++y)
			{
				var rand = Random.insideUnitCircle * DigSpotRandomRadius;
				positions.Add(new Vector3(corner + x * spacing + rand.x, 0.0f, corner + y * spacing + rand.y));
			}
		}

		return positions;
	}
}
