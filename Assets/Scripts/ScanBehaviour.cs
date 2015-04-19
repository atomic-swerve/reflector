using UnityEngine;
using System.Collections;

public class ScanBehaviour : MonoBehaviour {

	public float scanLength = 10f;
	public LayerMask layerMask;

	public string programmingInput = "Fire1";
	public string scanningInput = "Fire2";

	public Color programmingColour = Color.magenta;
	public Color scanningColour = Color.green;
	public Color missingColour = Color.red;

	private Transform gunT;
	private Animator gunA;
	private Transform camT;
	private Transform laserT;
	private LineRenderer trail;

	private Scannable scannable;
	private bool scanning = false;
	private bool finishedScan = false;
	private float timeToScan = 0f;

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
			if (!finishedScan) {
				if (Input.GetAxis(programmingInput) > 0f) {
					yield return StartCoroutine(Scan(programmingInput, programmingColour, true));
				}
				if (Input.GetAxis(scanningInput) > 0f) {
					yield return StartCoroutine(Scan(scanningInput, scanningColour, false));
				}
			}
			yield return null;
		}
	}

	IEnumerator Scan(string programming, Color c, bool readOrWrite) {
		gunA.StopPlayback();
		gunA.Play("GunCenter");
		trail.enabled = true;
		trail.SetColors(c,c);
		while (true) {
			if (Input.GetAxis(programming) < 1) {
				StopCoroutine("BeginReading");
				scanning = false;
				gunA.StopPlayback();
				gunA.Play("GunReturn");
				gunT.localRotation = Quaternion.identity;
				trail.enabled = false;
				finishedScan = false;
				break;
			}
			trail.SetPosition(0,laserT.position);
			
			if (!finishedScan && Physics.Raycast(camT.position, camT.forward, out ray, scanLength, layerMask)) {
				gunT.rotation = Quaternion.RotateTowards(gunT.rotation, Quaternion.LookRotation((ray.point - gunT.position).normalized), 5f);
				scannable = ray.transform.GetComponent<Scannable>();
				if (scannable != null) {
					SetLaser(c, ray.point);
					if (!scanning) {
						StartCoroutine(BeginReading(scannable, readOrWrite));
					}
				} else {
					SetLaser(missingColour, ray.point);
				}
			} else {
				StopCoroutine("BeginReading");
				scanning = false;
				gunT.localRotation = Quaternion.RotateTowards(gunT.localRotation, Quaternion.identity, 5f);
				SetLaser(missingColour, laserT.position + laserT.up * scanLength);
			}

			yield return null;
		}
	}

	void SetLaser(Color c, Vector3 end) {
		trail.SetPosition(1,end);
		trail.SetColors(c,c);
	}

	IEnumerator BeginReading(Scannable scannable, bool readOrWrite) {
		print ("Begin reading...");
		scanning = true;
		timeToScan = scannable.TimeToScan(readOrWrite); 
		yield return new WaitForSeconds(timeToScan);
		print ("Finished reading!");
		scanning = false;
		finishedScan = true;
		trail.enabled = false;
	}
}
