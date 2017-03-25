using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorPlacements : MonoBehaviour {
	GameObject sensor1, sensor2, sensor3, sensor4, sensor5,
		sensor6, sensor7, sensor8, sensor9, sensor10;

	// Use this for initialization
	void Start () {
		sensor1 = transform.Find ("Sensor1").gameObject;
		sensor2 = transform.Find ("Sensor2").gameObject;
		sensor3 = transform.Find ("Sensor3").gameObject;
		sensor4 = transform.Find ("Sensor4").gameObject;
		sensor5 = transform.Find ("Sensor5").gameObject;
		sensor6 = transform.Find ("Sensor6").gameObject;
		sensor7 = transform.Find ("Sensor7").gameObject;
		sensor8 = transform.Find ("Sensor8").gameObject;
		sensor9 = transform.Find ("Sensor9").gameObject;
		sensor10 = transform.Find ("Sensor10").gameObject;

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

		Debug.Log ("Sensor 6 Distance to Center: " +
			Vector3.Distance (sensor6.transform.position, transform.position) +
			"Sensor 6 Direction to Center: " +
			(sensor6.transform.localPosition / sensor6.transform.localPosition.magnitude));
		
		Debug.Log ("Sensor 7 Distance to Center: " +
			Vector3.Distance (sensor7.transform.position, transform.position) +
			"Sensor 7 Direction to Center: " +
			(sensor7.transform.localPosition / sensor7.transform.localPosition.magnitude));

		Debug.Log ("Sensor 8 Distance to Center: " +
			Vector3.Distance (sensor8.transform.position, transform.position) +
			"Sensor 8 Direction to Center: " +
			(sensor8.transform.localPosition / sensor8.transform.localPosition.magnitude));

		Debug.Log ("Sensor 9 Distance to Center: " +
			Vector3.Distance (sensor9.transform.position, transform.position) +
			"Sensor 9 Direction to Center: " +
			(sensor9.transform.localPosition / sensor9.transform.localPosition.magnitude));

		Debug.Log ("Sensor 10 Distance to Center: " +
			Vector3.Distance (sensor10.transform.position, transform.position) +
			"Sensor 10 Direction to Center: " +
			(sensor10.transform.localPosition / sensor10.transform.localPosition.magnitude));
		
		/*
		** Sensor 1 to Other Sensors
		*/
		Debug.Log ("Sensor1 - Sensor 2 Distance: " + 
			Vector3.Distance (sensor1.transform.position, sensor2.transform.position));

		Debug.Log ("Sensor 1 - Sensor 3 Distance: " + 
			Vector3.Distance (sensor1.transform.position, sensor3.transform.position));

		Debug.Log ("Sensor 1 - Sensor 4 Distance: " + 
			Vector3.Distance (sensor1.transform.position, sensor4.transform.position));
		
		Debug.Log ("Sensor 1 - Sensor 5 Distance: " + 
			Vector3.Distance (sensor1.transform.position, sensor5.transform.position));

		Debug.Log ("Sensor 1 - Sensor 6 Distance: " + 
			Vector3.Distance (sensor1.transform.position, sensor6.transform.position));

		Debug.Log ("Sensor 1 - Sensor 7 Distance: " + 
			Vector3.Distance (sensor1.transform.position, sensor7.transform.position));

		Debug.Log ("Sensor 1 - Sensor 8 Distance: " + 
			Vector3.Distance (sensor1.transform.position, sensor8.transform.position));

		Debug.Log ("Sensor 1 - Sensor 9 Distance: " + 
			Vector3.Distance (sensor1.transform.position, sensor9.transform.position));

		Debug.Log ("Sensor 1 - Sensor 10 Distance: " + 
			Vector3.Distance (sensor1.transform.position, sensor10.transform.position));


		/*
		** Sensor 2 to Other Sensors
		*/
		Debug.Log ("Sensor 2 - Sensor 3 Distance: " + 
			Vector3.Distance (sensor2.transform.position, sensor3.transform.position));

		Debug.Log ("Sensor 2 - Sensor 4 Distance: " + 
			Vector3.Distance (sensor2.transform.position, sensor4.transform.position));

		Debug.Log ("Sensor 2 - Sensor 5 Distance: " + 
			Vector3.Distance (sensor2.transform.position, sensor5.transform.position));

		Debug.Log ("Sensor 2 - Sensor 6 Distance: " + 
			Vector3.Distance (sensor2.transform.position, sensor6.transform.position));

		Debug.Log ("Sensor 2 - Sensor 7 Distance: " + 
			Vector3.Distance (sensor2.transform.position, sensor7.transform.position));

		Debug.Log ("Sensor 2 - Sensor 8 Distance: " + 
			Vector3.Distance (sensor2.transform.position, sensor8.transform.position));

		Debug.Log ("Sensor 2 - Sensor 9 Distance: " + 
			Vector3.Distance (sensor2.transform.position, sensor9.transform.position));

		Debug.Log ("Sensor 2 - Sensor 10 Distance: " + 
			Vector3.Distance (sensor2.transform.position, sensor10.transform.position));

		/*
		** Sensor 3 to Other Sensors
		*/
		Debug.Log ("Sensor 3 - Sensor 4 Distance: " + 
			Vector3.Distance (sensor3.transform.position, sensor4.transform.position));

		Debug.Log ("Sensor 3 - Sensor 5 Distance: " + 
			Vector3.Distance (sensor3.transform.position, sensor5.transform.position));

		Debug.Log ("Sensor 3 - Sensor 6 Distance: " + 
			Vector3.Distance (sensor3.transform.position, sensor6.transform.position));

		Debug.Log ("Sensor 3 - Sensor 7 Distance: " + 
			Vector3.Distance (sensor3.transform.position, sensor7.transform.position));

		Debug.Log ("Sensor 3 - Sensor 8 Distance: " + 
			Vector3.Distance (sensor3.transform.position, sensor8.transform.position));

		Debug.Log ("Sensor 3 - Sensor 9 Distance: " + 
			Vector3.Distance (sensor3.transform.position, sensor9.transform.position));

		Debug.Log ("Sensor 3 - Sensor 10 Distance: " + 
			Vector3.Distance (sensor3.transform.position, sensor10.transform.position));

		/*
		** Sensor 4 to Other Sensors
		*/
		Debug.Log ("Sensor 4 - Sensor 5 Distance: " + 
			Vector3.Distance (sensor4.transform.position, sensor5.transform.position));

		Debug.Log ("Sensor 4 - Sensor 6 Distance: " + 
			Vector3.Distance (sensor4.transform.position, sensor6.transform.position));

		Debug.Log ("Sensor 4 - Sensor 7 Distance: " + 
			Vector3.Distance (sensor4.transform.position, sensor7.transform.position));

		Debug.Log ("Sensor 4 - Sensor 8 Distance: " + 
			Vector3.Distance (sensor4.transform.position, sensor8.transform.position));

		Debug.Log ("Sensor 4 - Sensor 9 Distance: " + 
			Vector3.Distance (sensor4.transform.position, sensor9.transform.position));

		Debug.Log ("Sensor 4 - Sensor 10 Distance: " + 
			Vector3.Distance (sensor4.transform.position, sensor10.transform.position));

		/*
		** Sensor 5 to Other Sensors
		*/
		Debug.Log ("Sensor 5 - Sensor 6 Distance: " + 
			Vector3.Distance (sensor5.transform.position, sensor6.transform.position));

		Debug.Log ("Sensor 5 - Sensor 7 Distance: " + 
			Vector3.Distance (sensor5.transform.position, sensor7.transform.position));

		Debug.Log ("Sensor 5 - Sensor 8 Distance: " + 
			Vector3.Distance (sensor5.transform.position, sensor8.transform.position));

		Debug.Log ("Sensor 5 - Sensor 9 Distance: " + 
			Vector3.Distance (sensor5.transform.position, sensor9.transform.position));

		Debug.Log ("Sensor 5 - Sensor 10 Distance: " + 
			Vector3.Distance (sensor5.transform.position, sensor10.transform.position));

		/*
		** Sensor 6 to Other Sensors
		*/
		Debug.Log ("Sensor 6 - Sensor 7 Distance: " + 
			Vector3.Distance (sensor6.transform.position, sensor7.transform.position));

		Debug.Log ("Sensor 6 - Sensor 8 Distance: " + 
			Vector3.Distance (sensor6.transform.position, sensor8.transform.position));

		Debug.Log ("Sensor 6 - Sensor 9 Distance: " + 
			Vector3.Distance (sensor6.transform.position, sensor9.transform.position));

		Debug.Log ("Sensor 6 - Sensor 10 Distance: " + 
			Vector3.Distance (sensor6.transform.position, sensor10.transform.position));

		/*
		** Sensor 7 to Other Sensors
		*/
		Debug.Log ("Sensor 7 - Sensor 8 Distance: " + 
			Vector3.Distance (sensor7.transform.position, sensor8.transform.position));

		Debug.Log ("Sensor 7 - Sensor 9 Distance: " + 
			Vector3.Distance (sensor7.transform.position, sensor9.transform.position));
	
		Debug.Log ("Sensor 7 - Sensor 10 Distance: " + 
			Vector3.Distance (sensor7.transform.position, sensor10.transform.position));

		/*
		** Sensor 8 to Other Sensors
		*/
		Debug.Log ("Sensor 8 - Sensor 9 Distance: " + 
			Vector3.Distance (sensor8.transform.position, sensor9.transform.position));

		Debug.Log ("Sensor 8 - Sensor 10 Distance: " + 
			Vector3.Distance (sensor8.transform.position, sensor10.transform.position));

		/*
		** Sensor 9 to Other Sensors
		*/
		Debug.Log ("Sensor 9 - Sensor 10 Distance: " + 
			Vector3.Distance (sensor9.transform.position, sensor10.transform.position));
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
