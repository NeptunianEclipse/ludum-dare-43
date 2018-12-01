using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LudumDare43.Extensions
{
	internal static class Extensions
	{
		/// <summary>
		/// Returns a new vector based on the passed in vector with changes provided by the deltaX, deltaY, and deltaZ arguments.
		/// </summary>
		/// <param name="vector">The vector to use the x, y, and z coordinates from.</param>
		/// <param name="deltaX">The change in x to create on the new vector.</param>
		/// <param name="deltaY">The change in y to create on the new vector.</param>
		/// <param name="deltaZ">The change in z to create on the new vector.</param>
		public static Vector3 NewWithChange(this Vector3 vector, float deltaX = 0f, float deltaY = 0f, float deltaZ = 0f)
		{
			return new Vector3(vector.x + deltaX, vector.y + deltaY, vector.z + deltaZ);
		}

		/// <summary>
		/// Returns a new vector based on the passed in vector with changes provided by the deltaX and deltaY arguments.
		/// </summary>
		/// <param name="vector">The vector to use the x and y coordinates from.</param>
		/// <param name="deltaX">The change in x to create on the new vector.</param>
		/// <param name="deltaY">The change in y to create on the new vector.</param>
		public static Vector2 NewWithChange(this Vector2 vector, float deltaX = 0f, float deltaY = 0f)
		{
			return new Vector2(vector.x + deltaX, vector.y + deltaY);
		}

		/// <summary>
		/// Returns a new vector with the same x value but with y and z set to 0.
		/// </summary>
		/// <param name="vector">The vector to get the x coordinate from.</param>
		public static Vector2 XComponent(this Vector2 vector)
		{
			return new Vector2(vector.x, 0);
		}

		/// <summary>
		/// Returns a new vector with the same y value but with x and z set to 0.
		/// </summary>
		/// <param name="vector">The vector to get the y coordinate from.</param>
		public static Vector2 YComponent(this Vector2 vector)
		{
			return new Vector2(0, vector.y);
		}

		/// <summary>
		/// Call Perpendicular in a new and exciting way.
		/// </summary>
		/// <param name="vector">The vector to be perpindicular from.</param>
		public static Vector2 Perpendicular(this Vector2 vector)
		{
			return Vector2.Perpendicular(vector);
		}

		/// <summary>
		/// Returns a boolean, randomly assigned true or false, either a 50% chance or a custom probability based on the <paramref name="probabailtyOfTrue"/> parameter.
		/// </summary>
		/// <param name="probabailtyOfTrue">The probability of returning true.</param>
		public static bool RandomBit(float probabailtyOfTrue = 0.5f)
		{
			return UnityEngine.Random.value < probabailtyOfTrue;
		}

		/// <summary>
		/// Returns a new vector with the result of calling Mathf.Abs on each of <paramref name="vector"/>'s x and y values.
		/// </summary>
		/// <param name="vector">The vector to use to set the x and y on the new vector, after calling Mathf.Abs on them though.</param>
		public static Vector2 ElementwiseAbosolute(this Vector2 vector)
		{
			return new Vector2(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
		}
	}
}
