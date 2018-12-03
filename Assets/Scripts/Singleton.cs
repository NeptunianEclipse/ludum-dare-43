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
					//Debug.LogWarning($"No instance of singleton {typeof(T).Name}");
				}
			}
			return instance;
		}
	}


}
