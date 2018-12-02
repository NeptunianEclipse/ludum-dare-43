using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class FloatingTextUI : MonoBehaviour 
{
	public Text TextObject;
	public Vector2 MaxVelocity;

	private Rigidbody2D rigidbody2d;

	private void Awake()
	{
		rigidbody2d = GetComponent<Rigidbody2D>();
		Velocity = new Vector2(Random.Range(-MaxVelocity.x, MaxVelocity.x), Random.Range(-MaxVelocity.y, MaxVelocity.y));
	}

	public Vector2 Velocity {
		get { return rigidbody2d.velocity; }
		set { rigidbody2d.velocity = value; }
	}

	public string Text {
		get { return TextObject.text; }
		set { TextObject.text = value; }
	}

}
