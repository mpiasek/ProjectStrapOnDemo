using UnityEngine;
using System.Collections;
using System;
using AssemblyCSharp;

public class BraceletTrackedObjectThread : MonoBehaviour {
	public TrackingAlgorithmDouble.objectLocation Sensor1;
	public TrackingAlgorithmDouble.objectLocation Sensor2;
	public TrackingAlgorithmDouble.objectLocation Sensor3;
	public TrackingAlgorithmDouble.sensorDistance array;
	public TrackingAlgorithmDouble.Imu Imu;
	public Vector3 Position;
	/*
	** DON'T TOUCH. This runs every frame and does distance calculations
	*/

	void Start(){
		array.AB = 3;
		array.AC = 5.5;
		array.BC = 3;
		Sensor1.distance = 20.66;
		Sensor2.distance = 20.58;
		Sensor3.distance = 20.79;
	}

	void Update () {
		//actualPosition ();
		//Debug.Log ("Position: " + Sensor1.distance + " " + Sensor2.distance + " " + Sensor3.distance);
		// Grab new data from the other SerialPortThread script
		Sensor1.azimuth = gameObject.GetComponent<BraceletSerialPort>().Sensor1.azimuth;
		Sensor1.elevation = gameObject.GetComponent<BraceletSerialPort>().Sensor1.elevation;

		Sensor2.azimuth = gameObject.GetComponent<BraceletSerialPort>().Sensor2.azimuth;
		Sensor2.elevation = gameObject.GetComponent<BraceletSerialPort>().Sensor2.elevation;

		Sensor3.azimuth = gameObject.GetComponent<BraceletSerialPort>().Sensor3.azimuth;
		Sensor3.elevation = gameObject.GetComponent<BraceletSerialPort>().Sensor3.elevation;

		TrackingAlgorithmDouble.runAlgorithm (ref Sensor1, ref Sensor2, ref Sensor3, ref array);

		//Debug.Log ("1 " + Sensor1.azimuth * 180.0 / Math.PI + " " + Sensor1.elevation* 180.0 / Math.PI  + " 2 " + Sensor2.azimuth * 180.0 / Math.PI + " " + Sensor2.elevation * 180.0 / Math.PI + " 3 " + Sensor3.azimuth * 180.0 / Math.PI + " " + Sensor3.elevation* 180.0 / Math.PI );
		//Debug.Log ("Position: " + Sensor1.distance + " " + Sensor2.distance + " " + Sensor3.distance);
		updatePosition (ref Sensor1, ref Sensor2, ref Sensor3, gameObject.GetComponent<BraceletSerialPort>().Imu.a, gameObject.GetComponent<BraceletSerialPort>().Imu.deltaT, gameObject.GetComponent<BraceletSerialPort>().Imu.q);
	}

	/*
	** Update position of the object
	*/
	void updatePosition(ref TrackingAlgorithmDouble.objectLocation sensor1, ref TrackingAlgorithmDouble.objectLocation sensor2, ref TrackingAlgorithmDouble.objectLocation sensor3, Vector3 a, float deltaT, Quaternion q ){
		Quaternion localAxisFix;
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

		//Dead Reckoning
		Imu.v.x = (a.x * deltaT) + Imu.v0.x;
		Imu.s.x = (0.5f * Imu.a.x * Imu.deltaT * deltaT) + (Imu.v0.x * deltaT) + Imu.s0.x;
		Imu.v0.x = Imu.v.x;
		Imu.s0.x = Imu.s.x;

		Imu.v.y = (a.y * deltaT) + Imu.v0.y;
		Imu.s.y = (0.5f * Imu.a.y * deltaT * deltaT) + (Imu.v0.y * deltaT) + Imu.s0.y;
		Imu.v0.y = Imu.v.y;
		Imu.s0.y = Imu.s.y;

		Imu.v.z = (a.z * deltaT) + Imu.v0.z;
		Imu.s.z = (0.5f * Imu.a.z * deltaT * deltaT) + (Imu.v0.z * deltaT) + Imu.s0.z;
		Imu.v0.z = Imu.v.z;
		Imu.s0.z = Imu.s.z;

		//Sensors return 0 if reading is erroneous. So let IMU take over position.
		if (sensor1.azimuth != 0 && sensor1.elevation != 0 && sensor2.azimuth != 0 && sensor2.elevation != 0 && sensor3.azimuth != 0 && sensor3.elevation != 0) 
		{
			Position = new Vector3 (sens1x, sens1y, sens1z);
			Imu.s.x = sens1x;
			Imu.s.y = sens1y;
			Imu.s.z = sens1z;
			Imu.s0.x = sens1x;
			Imu.s0.y = sens1y;
			Imu.s0.z = sens1z;
			Imu.v0.x = 0;
			Imu.v0.y = 0;
			Imu.v0.z = 0;
		} 
		else 
		{
			Position = Imu.s;
		}

		transform.position = Position;

		localAxisFix.w = (float)Math.Cos (Math.PI / 4f);
		localAxisFix.x = 0;
		localAxisFix.y = 0;
		localAxisFix.z = (float)Math.Cos (Math.PI / 4f);
		transform.rotation = q * localAxisFix;
	}


}
