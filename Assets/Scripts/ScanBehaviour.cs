using UnityEngine;
using System.Collections;

public class ScanBehaviour : MonoBehaviour {

	private Transform gun;

	void Start() {
		gun = GetComponentInChildren<Camera>().transform.FindChild("Gun").transform;
		StartCoroutine(ScanInput());
	}

	IEnumerator ScanInput() {
		while (true) {
			if (Input.GetAxis("Fire2") > 0f) {
				yield return StartCoroutine(Scan ());
			}
			yield return null;
		}
	}

	IEnumerator Scan() {
		while (true) {
			if (Input.GetAxis("Fire2") < 1) {
				break;
			}
			Debug.DrawRay(gun.position, gun.forward * 50, Color.green);
			Physics.Raycast(gun.position, gun.forward, 50);

			yield return null;
		}
	}
}
