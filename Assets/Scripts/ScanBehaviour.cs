using UnityEngine;
using System.Collections;

public class ScanBehaviour : MonoBehaviour {

	public float scanLength = 10f;
	public LayerMask layerMask;

	private Transform gunT;
	private Animator gunA;
	private Transform camT;
	private Transform laserT;
	private LineRenderer trail;

	private RaycastHit ray;

	void Start() {
		gunT = GetComponentInChildren<Camera>().transform.FindChild("Gun").transform;
		gunA = GetComponentInChildren<Camera>().transform.FindChild("Gun").GetComponent<Animator>();
		camT = GetComponentInChildren<Camera>().transform;
		laserT = GetComponentInChildren<Camera>().transform.Find("Gun/Mesh/LaserPoint").transform;
		trail = GetComponentInChildren<LineRenderer>();
		gunA.Play ("GunReturn");
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
		trail.enabled = true;
		trail.SetColors(new Color(.7f,0,0,.3f),new Color(.7f,0,0,.2f));
		while (true) {
			if (Input.GetAxis("Fire2") < 1) {
				gunA.StopPlayback();
				gunA.Play("GunReturn");
				gunT.localRotation = Quaternion.identity;
				trail.enabled = false;
				break;
			}

			trail.SetPosition(0,laserT.position);

			if (Physics.Raycast(camT.position, camT.forward, out ray, scanLength, layerMask)) {
				gunT.rotation = Quaternion.RotateTowards(gunT.rotation, Quaternion.LookRotation((ray.point - gunT.position).normalized), 5f);
				trail.SetPosition(1,ray.point);
				trail.SetColors(new Color(.7f,0,0,.6f),new Color(.7f,0,0,.5f));
			} else {
				gunT.localRotation = Quaternion.RotateTowards(gunT.localRotation, Quaternion.identity, 5f);
				trail.SetPosition(1, laserT.position + laserT.up * scanLength);
				trail.SetColors(new Color(.7f,0,0,.3f),new Color(.7f,0,0,.2f));
			}

			yield return null;
		}
	}

	/*IEnumerator LaserFlicker() {
		while (true) {
			yield return new WaitForSeconds;
		}
	}*/
}
