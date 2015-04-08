using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour {

	void Update () {
		this.transform.Rotate(0, 0, 90*Time.deltaTime);
	}
}
