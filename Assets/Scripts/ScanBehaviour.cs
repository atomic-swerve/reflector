using UnityEngine;
using System.Collections;

public class ScanBehaviour : MonoBehaviour {

	public LayerMask layerMask;

	private Transform gunT;
	private Animator gunA;
	private Transform camT;

	private RaycastHit ray;

	void Start() {
		gunT = GetComponentInChildren<Camera>().transform.FindChild("Gun").transform;
		gunA = GetComponentInChildren<Camera>().transform.FindChild("Gun").GetComponent<Animator>();
		camT = GetComponentInChildren<Camera>().transform;
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
		gunA.StopPlayback();
		gunA.Play("GunCenter");
		while (true) {
			if (Input.GetAxis("Fire2") < 1) {
				gunA.StopPlayback();
				gunA.Play("GunReturn");
				gunT.localRotation = Quaternion.identity;
				break;
			}

			if (Physics.Raycast(camT.position, camT.forward, out ray, 10, layerMask)) {
				Debug.DrawRay(camT.position, camT.forward * Vector3.Distance(ray.point,camT.position), Color.green);
				gunT.LookAt(ray.point);
				Debug.DrawRay(gunT.position, gunT.forward * Vector3.Distance(ray.point,gunT.position), Color.blue);
			} else {
				Debug.DrawRay(camT.position, camT.forward * 10, Color.red);
			}

			yield return null;
		}
	}
}
