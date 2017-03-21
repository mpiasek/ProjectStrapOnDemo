using UnityEngine;
using System.Collections;
using System;
using AssemblyCSharp;

public class TrackedObjectThread : MonoBehaviour {

	public TrackingAlgorithmDouble.objectLocation Sensor1;
	public TrackingAlgorithmDouble.objectLocation Sensor2;
	public TrackingAlgorithmDouble.objectLocation Sensor3;
	public TrackingAlgorithmDouble.sensorDistance array;

	/*
	** DON'T TOUCH. This runs every frame and does distance calculations
	*/

	void Start(){
		array.AB = 6.5;
		array.AC = 6.5;
		array.BC = 6.5;
		Sensor1.distance = 20.66;
		Sensor2.distance = 20.58;
		Sensor3.distance = 20.79;
	}
	
	void Update () {
		//actualPosition ();
		Debug.Log ("Position: " + Sensor1.distance + " " + Sensor2.distance + " " + Sensor3.distance);
		// Grab new data from the other SerialPortThread script
		Sensor1.azimuth = gameObject.GetComponent<SerialPortThread>().Sensor1.azimuth;
		Sensor1.elevation = gameObject.GetComponent<SerialPortThread>().Sensor1.elevation;

		Sensor2.azimuth = gameObject.GetComponent<SerialPortThread>().Sensor2.azimuth;
		Sensor2.elevation = gameObject.GetComponent<SerialPortThread>().Sensor2.elevation;

		Sensor3.azimuth = gameObject.GetComponent<SerialPortThread>().Sensor3.azimuth;
		Sensor3.elevation = gameObject.GetComponent<SerialPortThread>().Sensor3.elevation;

		TrackingAlgorithmDouble.runAlgorithm (ref Sensor1, ref Sensor2, ref Sensor3, ref array);
		//Debug.Log ("1 " + Sensor1.azimuth * 180.0 / Math.PI + " " + Sensor1.elevation* 180.0 / Math.PI  + " 2 " + Sensor2.azimuth * 180.0 / Math.PI + " " + Sensor2.elevation * 180.0 / Math.PI + " 3 " + Sensor3.azimuth * 180.0 / Math.PI + " " + Sensor3.elevation* 180.0 / Math.PI );
		//Debug.Log ("Position: " + Sensor1.distance + " " + Sensor2.distance + " " + Sensor3.distance);
		updatePosition (ref Sensor1, ref Sensor2, ref Sensor3);
	}

	/*
	** Update position of the object
	*/
	void updatePosition(ref TrackingAlgorithmDouble.objectLocation sensor1, ref TrackingAlgorithmDouble.objectLocation sensor2, ref TrackingAlgorithmDouble.objectLocation sensor3 ){
		// Convert to X Y Z Coordinates

		// Sensor 1
		double sens1XZ = Math.Sin(sensor1.elevation) * sensor1.distance;
		float sens1x =  (float) (Math.Cos(sensor1.azimuth) * sens1XZ);
		float sens1y = (float) (Math.Cos(sensor1.elevation) * sensor1.distance);
		float sens1z = (float) (Math.Sin(sensor1.azimuth) * sens1XZ);

		// Sensor 2
		double sens2XZ = Math.Sin(sensor2.elevation) * sensor2.distance;
		float sens2x =  (float) (Math.Cos(sensor2.azimuth) * sens2XZ);
		float sens2y = (float) (Math.Cos(sensor2.elevation) * sensor2.distance);
		float sens2z = (float) (Math.Sin(sensor2.azimuth) * sens2XZ);

		// Sensor 3
		double sens3XZ = Math.Sin(sensor3.elevation) * sensor3.distance;
		float sens3x =  (float) (Math.Cos(sensor3.azimuth) * sens3XZ);
		float sens3y = (float) (Math.Cos(sensor3.elevation) * sensor3.distance);
		float sens3z = (float) (Math.Sin(sensor3.azimuth) * sens3XZ);

		transform.position = new Vector3(sens1x, sens1y, sens1z);


		Vector3 u = new Vector3 (sens2x - sens1x, sens2y - sens1y, sens2z - sens1z);
		Vector3 v = new Vector3 (sens3x - sens1x, sens3y - sens1y, sens3z - sens1z);

		// Get CrossProduct
		Vector3 UxV = new Vector3( u.y*v.z-u.z*v.y, u.z*v.x-u.x*v.z, u.x*v.y-u.y*v.x  );

		// Get Cross Product
		Vector3 r = new Vector3( v.x, 0 ,v.z );
		Vector3 RxV = new Vector3( r.y*v.z-r.z*v.y, r.z*v.x-r.x*v.z , r.x*v.y-r.y*v.x );

		// 

		float rotX = 0;

			//Mathf.Acos( (UxV.x*RxV.x+UxV.y*RxV.y+UxV.z*RxV.z)/ (UxV.magnitude*RxV.magnitude));
			
		float rotY = Mathf.Acos (new Vector2 (v.x, 0).magnitude / new Vector2 (v.x, v.z).magnitude) + Mathf.PI;
		if (v.z < 0) {
			rotY = 2.0f * Mathf.PI - rotY;
		}

		float rotZ = Mathf.Acos (new Vector2(v.x, 0).magnitude / new Vector2(v.x, v.y).magnitude );
		if (v.y < 0) {
			rotZ = 2.0f * Mathf.PI - rotZ;
		}
		//Debug.Log (v.x + " " + v.y + " " + v.z);
		Debug.Log ("rotX = " + rotX + " rotY = " + rotY + " rotZ = " + rotZ);
		//Quaternion rotateTowards = Quaternion.RotateTowards (transform.rotation, Quaternion.Euler(0, rotY * Mathf.Rad2Deg, rotZ * Mathf.Rad2Deg)*Quaternion.AngleAxis(rotX * Mathf.Rad2Deg, transform.right), 270f);
		Quaternion rotateTowards = Quaternion.RotateTowards (transform.rotation, Quaternion.Euler(rotX * Mathf.Rad2Deg, rotY * Mathf.Rad2Deg, rotZ * Mathf.Rad2Deg), 270f);
		transform.rotation = rotateTowards;


		// U = sens2 - sens1
		// V = sens3 - sens1
		// N = U X V

		// Nx = Uy * Vz - Uz * Vy
		float normX = (sens2y - sens1y)*(sens3z - sens1z) - (sens2z - sens1z)*(sens3y - sens1y);

		// Ny = Uz * Vx - Ux * Vz
		float normY = (sens2z - sens1z)*(sens3x - sens1x) - (sens2x - sens1x)*(sens3z - sens1z);

		// Nz = Uz * Vx - Uy - Vx
		float normZ = (sens2x - sens1x)*(sens3y - sens1y) - (sens2y - sens1y)*(sens3x - sens1x);

		transform.LookAt (new Vector3 (sens1x+normX, sens1y+normY, sens1z+normZ));

	}


}
