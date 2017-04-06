using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorPlacements : MonoBehaviour {
	GameObject sensor1, sensor2, sensor3, sensor4, sensor5,
		sensor6, sensor7, sensor8, sensor9, sensor10;

    public double[,] SensorDistances;

	// Use this for initialization
	void Start () {
        SensorDistances = transform.parent.GetComponent<BraceletTrackedObjectThread>().SensorDistances;

		sensor1 = transform.Find ("Sensor1").gameObject;
		sensor2 = transform.Find ("Sensor2").gameObject;
		sensor3 = transform.Find ("Sensor3").gameObject;
		sensor4 = transform.Find ("Sensor4").gameObject;
		sensor5 = transform.Find ("Sensor5").gameObject;

		/*
		** Sensor Distance & Direction to Center
		*/
		Debug.Log ("Sensor 1 Distance to Center: " +
			Vector3.Distance (sensor1.transform.position, transform.position) +
			"Sensor 1 Direction to Center: " +
			(sensor1.transform.localPosition / sensor1.transform.localPosition.magnitude));

		Debug.Log ("Sensor 2 Distance to Center: " +
			Vector3.Distance (sensor2.transform.position, transform.position) +
			"Sensor 2 Direction to Center: " +
			(sensor2.transform.localPosition / sensor2.transform.localPosition.magnitude));

		Debug.Log ("Sensor 3 Distance to Center: " +
			Vector3.Distance (sensor3.transform.position, transform.position) +
			"Sensor 1 Direction to Center: " +
			(sensor3.transform.localPosition / sensor3.transform.localPosition.magnitude));

		Debug.Log ("Sensor 4 Distance to Center: " +
			Vector3.Distance (sensor4.transform.position, transform.position) +
			"Sensor 4 Direction to Center: " +
			(sensor4.transform.localPosition / sensor4.transform.localPosition.magnitude));
		
		Debug.Log ("Sensor 5 Distance to Center: " +
			Vector3.Distance (sensor5.transform.position, transform.position) +
			"Sensor 5 Direction to Center: " +
			(sensor5.transform.localPosition / sensor5.transform.localPosition.magnitude));
		
		/*
		** Sensor 1 to Other Sensors
		*/
		Debug.Log ("Sensor1 - Sensor 2 Distance: " + 
			Vector3.Distance (sensor1.transform.position, sensor2.transform.position));
        SensorDistances[0, 1] = Vector3.Distance(sensor1.transform.position, sensor2.transform.position);
        SensorDistances[1, 0] = Vector3.Distance(sensor1.transform.position, sensor2.transform.position);

        Debug.Log ("Sensor 1 - Sensor 3 Distance: " + 
			Vector3.Distance (sensor1.transform.position, sensor3.transform.position));
        SensorDistances[0, 2] = Vector3.Distance(sensor1.transform.position, sensor3.transform.position);
        SensorDistances[2, 0] = Vector3.Distance(sensor1.transform.position, sensor3.transform.position);

        Debug.Log ("Sensor 1 - Sensor 4 Distance: " + 
			Vector3.Distance (sensor1.transform.position, sensor4.transform.position));
        SensorDistances[0, 3] = Vector3.Distance(sensor1.transform.position, sensor4.transform.position);
        SensorDistances[3, 0] = Vector3.Distance(sensor1.transform.position, sensor4.transform.position);

        Debug.Log ("Sensor 1 - Sensor 5 Distance: " + 
			Vector3.Distance (sensor1.transform.position, sensor5.transform.position));
        SensorDistances[0, 4] = Vector3.Distance(sensor1.transform.position, sensor5.transform.position);
        SensorDistances[4, 0] = Vector3.Distance(sensor1.transform.position, sensor5.transform.position);

        /*
		** Sensor 2 to Other Sensors
		*/
        Debug.Log ("Sensor 2 - Sensor 3 Distance: " + 
			Vector3.Distance (sensor2.transform.position, sensor3.transform.position));
        SensorDistances[1, 2] = Vector3.Distance(sensor2.transform.position, sensor3.transform.position);
        SensorDistances[2, 1] = Vector3.Distance(sensor2.transform.position, sensor3.transform.position);

        Debug.Log ("Sensor 2 - Sensor 4 Distance: " + 
			Vector3.Distance (sensor2.transform.position, sensor4.transform.position));
        SensorDistances[1, 3] = Vector3.Distance(sensor2.transform.position, sensor4.transform.position);
        SensorDistances[3, 1] = Vector3.Distance(sensor2.transform.position, sensor4.transform.position);

        Debug.Log ("Sensor 2 - Sensor 5 Distance: " + 
			Vector3.Distance (sensor2.transform.position, sensor5.transform.position));
        SensorDistances[1, 4] = Vector3.Distance(sensor2.transform.position, sensor5.transform.position);
        SensorDistances[4, 1] = Vector3.Distance(sensor2.transform.position, sensor5.transform.position);

        /*
		** Sensor 3 to Other Sensors
		*/
        Debug.Log ("Sensor 3 - Sensor 4 Distance: " + 
			Vector3.Distance (sensor3.transform.position, sensor4.transform.position));
        SensorDistances[2, 3] = Vector3.Distance(sensor3.transform.position, sensor4.transform.position);
        SensorDistances[3, 2] = Vector3.Distance(sensor3.transform.position, sensor4.transform.position);

        Debug.Log ("Sensor 3 - Sensor 5 Distance: " + 
			Vector3.Distance (sensor3.transform.position, sensor5.transform.position));
        SensorDistances[2, 4] = Vector3.Distance(sensor3.transform.position, sensor5.transform.position);
        SensorDistances[4, 2] = Vector3.Distance(sensor3.transform.position, sensor5.transform.position);


        /*
		** Sensor 4 to Other Sensors
		*/
        Debug.Log ("Sensor 4 - Sensor 5 Distance: " + 
			Vector3.Distance (sensor4.transform.position, sensor5.transform.position));
        SensorDistances[3, 4] = Vector3.Distance(sensor3.transform.position, sensor5.transform.position);
        SensorDistances[4, 3] = Vector3.Distance(sensor3.transform.position, sensor5.transform.position);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
