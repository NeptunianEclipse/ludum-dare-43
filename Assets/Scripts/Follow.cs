using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
	public Transform FollowObject;
	public Bounds Deadzone;
	public float Stiffness;

	private Vector3 targetPosition;

	private void Update()
	{
		Deadzone.center = FollowObject.position;

		if(Deadzone.Contains(transform.position) == false)
		{
			Vector3 closestPoint = Deadzone.ClosestPoint(transform.position);
			targetPosition = new Vector3(closestPoint.x, closestPoint.y, transform.position.z);

		}
		else
		{
			targetPosition = transform.position;
		}

		transform.position = Vector3.Lerp(transform.position, targetPosition, Stiffness);
	}
}
