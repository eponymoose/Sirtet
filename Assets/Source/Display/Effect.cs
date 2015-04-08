using UnityEngine;
using System.Collections;

public class Effect : MonoBehaviour {

	public float ttl;

	void Update()
	{
		ttl -= Time.deltaTime;
		if( ttl < 0 )
		{
			Destroy( this.gameObject );
		}
	}
}
