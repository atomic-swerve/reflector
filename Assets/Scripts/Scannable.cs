using UnityEngine;
using System.Collections;

public class Scannable : MonoBehaviour {

	public float timeToRead = .2f;
	public float timeToWrite = .5f;
	public Program[] methods;

	public float TimeToScan (bool readOrWrite)
	{
		if (readOrWrite) {
			return timeToRead;
		} else {
			return timeToWrite;
		}
	}

	public Program[] DoneScanning (bool readOrWrite, int programIndex = 0) {
		if (readOrWrite) {
			return methods;
		} else {
			return null;
		}
	}

}

public class Program {
	public string programName = "Default";
	public string programDescription = "Default Description";
	public Texture2D programIcon;
}