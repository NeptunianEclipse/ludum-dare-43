using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

	private static T instance;

	public static T Instance {
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<T>();
				if (instance == null)
				{
					GameObject singletonGameObject = new GameObject();
					instance = singletonGameObject.AddComponent<T>();
					singletonGameObject.name = typeof(T).ToString() + " (Singleton)";
				}
			}
			return instance;
		}
	}


}
