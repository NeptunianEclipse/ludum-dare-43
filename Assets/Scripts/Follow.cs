using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
	public Transform FollowObject;
	public Bounds Deadzone;

	private void Update()
	{
		Deadzone.center = FollowObject.position;

		if(Deadzone.Contains(transform.position) == false)
		{
			Vector3 closestPoint = Deadzone.ClosestPoint(transform.position);
			transform.position = new Vector3(closestPoint.x, closestPoint.y, transform.position.z);
		}
	}
}
