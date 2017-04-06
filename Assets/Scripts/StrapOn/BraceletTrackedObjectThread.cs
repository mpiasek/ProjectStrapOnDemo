using UnityEngine;
using System.Collections;
using System;
using AssemblyCSharp;

public class BraceletTrackedObjectThread : MonoBehaviour {
    public TrackingAlgorithmDouble.objectLocation [] Sensors;
    public TrackingAlgorithmDouble.objectLocation[] ChosenSensors;
    private bool [] PickSensors;
    
    /*
	public TrackingAlgorithmDouble.objectLocation Sensor1;
	public TrackingAlgorithmDouble.objectLocation Sensor2;
	public TrackingAlgorithmDouble.objectLocation Sensor3;

    
	public TrackingAlgorithmDouble.objectLocation Sensor4;
	public TrackingAlgorithmDouble.objectLocation Sensor5;
	public TrackingAlgorithmDouble.objectLocation Sensor6;

	public TrackingAlgorithmDouble.objectLocation Sensor7;
	public TrackingAlgorithmDouble.objectLocation Sensor8;
	public TrackingAlgorithmDouble.objectLocation Sensor9;

	public TrackingAlgorithmDouble.objectLocation Sensor10;
    */

    public Transform basestation;
	public TrackingAlgorithmDouble.sensorDistance array;
	public TrackingAlgorithmDouble.Imu Imu;
	
    private Quaternion localAxisFix;
    private Quaternion lastQuaternion;
    /*
	** DON'T TOUCH. This runs every frame and does distance calculations
	*/

    void Start(){
        Sensors = new TrackingAlgorithmDouble.objectLocation[5];
        ChosenSensors = new TrackingAlgorithmDouble.objectLocation[3];
        PickSensors = new bool[5];

		array.AB = 0.06;
		array.AC = 0.055;
		array.BC = 0.065;

        Sensors[0].distance = 2;
        Sensors[1].distance = 2;
        Sensors[2].distance = 2;
        Sensors[3].distance = 2;
        Sensors[4].distance = 2;

        basestation = transform.parent.Find("Basestation").transform;
        lastQuaternion = transform.rotation;
	}

	void Update () {
		//actualPosition ();
		//Debug.Log ("Position: " + Sensor1.distance + " " + Sensor2.distance + " " + Sensor3.distance);
		
        /*
		Sensor1.azimuth = gameObject.GetComponent<SerialPortThread>().Sensor1.azimuth;
		Sensor1.elevation = gameObject.GetComponent<SerialPortThread>().Sensor1.elevation;

		Sensor2.azimuth = gameObject.GetComponent<SerialPortThread>().Sensor2.azimuth;
		Sensor2.elevation = gameObject.GetComponent<SerialPortThread>().Sensor2.elevation;

		Sensor3.azimuth = gameObject.GetComponent<SerialPortThread>().Sensor3.azimuth;
		Sensor3.elevation = gameObject.GetComponent<SerialPortThread>().Sensor3.elevation;
		*/

		// Grab new data from the other SerialPortThread script
		Sensors[0].azimuth = gameObject.GetComponent<BraceletSerialPort>().Sensor1.azimuth;
        Sensors[0].elevation = gameObject.GetComponent<BraceletSerialPort>().Sensor1.elevation;

        Sensors[1].azimuth = gameObject.GetComponent<BraceletSerialPort>().Sensor2.azimuth;
        Sensors[1].elevation = gameObject.GetComponent<BraceletSerialPort>().Sensor2.elevation;

        Sensors[2].azimuth = gameObject.GetComponent<BraceletSerialPort>().Sensor3.azimuth;
        Sensors[2].elevation = gameObject.GetComponent<BraceletSerialPort>().Sensor3.elevation;

        Sensors[3].azimuth = gameObject.GetComponent<BraceletSerialPort>().Sensor4.azimuth;
        Sensors[3].elevation = gameObject.GetComponent<BraceletSerialPort>().Sensor4.elevation;

		Sensors[4].azimuth = gameObject.GetComponent<BraceletSerialPort>().Sensor5.azimuth;
		Sensors[4].elevation = gameObject.GetComponent<BraceletSerialPort>().Sensor5.elevation;

        /*
        ** Pick Sensors to run algorithm with
        ** 
        */
        for(int i=0; i<PickSensors.Length; i++) {
            PickSensors[i] = false;
        }

        
		TrackingAlgorithmDouble.runAlgorithm (ref ChosenSensors[0], ref ChosenSensors[1], ref ChosenSensors[2], ref array);

		//Debug.Log ("1 " + Sensor1.azimuth * 180.0 / Math.PI + " " + Sensor1.elevation* 180.0 / Math.PI  + " 2 " + Sensor2.azimuth * 180.0 / Math.PI + " " + Sensor2.elevation * 180.0 / Math.PI + " 3 " + Sensor3.azimuth * 180.0 / Math.PI + " " + Sensor3.elevation* 180.0 / Math.PI );
		//Debug.Log ("Position: " + Sensor1.distance + " " + Sensor2.distance + " " + Sensor3.distance);
		updatePosition (ref ChosenSensors[0], ref ChosenSensors[1], ref ChosenSensors[2], gameObject.GetComponent<BraceletSerialPort>().Imu.a, gameObject.GetComponent<BraceletSerialPort>().Imu.deltaT, gameObject.GetComponent<BraceletSerialPort>().Imu.q);
	}

	/*
	** 
	*/
	void chooseSensors (){
        int j = 0;
        for (int i = 0; i < Sensors.Length; i++) {
            if (Sensors[i].azimuth != 0) {
                ChosenSensors[j] = Sensors[i];
                PickSensors[i] = true;
                j++;
            }
            if (j == 3) {
                break;
            }

        }
    }

    void updateSensors() {

    }

	/*
	** Update position of the object
	*/
	void updatePosition(ref TrackingAlgorithmDouble.objectLocation sensor1, ref TrackingAlgorithmDouble.objectLocation sensor2, ref TrackingAlgorithmDouble.objectLocation sensor3, Vector3 a, float deltaT, Quaternion q ){
		Vector3 Position;
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

        //transform.localPosition = basestation.localRotation * Position + basestation.localPosition;
        transform.position =  Position + basestation.position;

        localAxisFix.w = (float)Math.Cos (Math.PI / 4f);
		localAxisFix.x = 0;
        localAxisFix.y = 0;
        localAxisFix.z = (float)Math.Cos(Math.PI / 4f);
        
		transform.rotation = q* localAxisFix;

        /*
        ** Other quaternion method
        */
        /*
        Quaternion diffQuat = Quaternion.Inverse(q) * lastQuaternion;
        transform.Rotate(diffQuat.eulerAngles, Space.World);
        lastQuaternion = q;
        */
	}


}
