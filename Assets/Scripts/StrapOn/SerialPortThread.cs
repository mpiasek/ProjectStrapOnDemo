using UnityEngine;
using System;
using System.Collections;
using System.IO.Ports;
using System.Threading;
using AssemblyCSharp;


public class SerialPortThread : MonoBehaviour{
	SerialPort stream;
	public string Port;
	public int BaudRate;
	public int TimeOut;

	private bool _threadrunning;
	Thread _thread;

	public TrackingAlgorithmDouble.objectLocation Sensor1;
	public TrackingAlgorithmDouble.objectLocation Sensor2;
	public TrackingAlgorithmDouble.objectLocation Sensor3;


	/*
	** Setting up thread
	*/
	void Start(){
		BeginThread ();
	}

	void Update(){
		//Debug.Log ("_thread " + _thread);
	}

	/*
	** Function that runs inside the new thread
	*/ 
	void OpenSerialPort(){
		// Initialize Serial Port
		stream = new SerialPort(Port, BaudRate);
		stream.ReadTimeout = TimeOut;
		stream.Open();
		// While we want the thread to keep running
		while (_threadrunning) {
			// If serial port is successfully opened, read from buffer
			//Debug.Log("Running thread");
			if (stream.IsOpen) {
				//Debug.Log ("Stream is open");
				try{
					string arduino = stream.ReadLine ();
				
					Debug.Log ("Line is read");
					string[] parser = arduino.Split (' ');
					float temp;

					if (!float.TryParse (parser [1], out temp)) {
						//Debug.Log ("Unable to convert 1Azimuth");
						continue;
					};
					Sensor1.azimuth = temp * Math.PI / 180.0;

					if (!float.TryParse (parser [2], out temp)) {
						Debug.Log ("Unable to convert 1Elevation");
						continue;
					}
					Sensor1.elevation = (180.0 - temp)* Math.PI / 180.0;

					if (!float.TryParse (parser [4], out temp)) {
						Debug.Log ("Unable to convert 2Azimuth");
						continue;
					};
					Sensor2.azimuth = temp * Math.PI / 180.0;

					if (!float.TryParse (parser [5], out temp)) {
						Debug.Log ("Unable to convert 2Elevation");
						continue;
					}
					Sensor2.elevation = (180.0 - temp) * Math.PI / 180.0;

					if (!float.TryParse (parser [7], out temp)) {
						Debug.Log ("Unable to convert 3Azimuth");
						continue;
					};
					Sensor3.azimuth = temp * Math.PI / 180.0;

					if (!float.TryParse (parser [8], out temp)) {
						Debug.Log ("Unable to convert 3Elevation");
						continue;
					}
					Sensor3.elevation = (180.0 - temp) * Math.PI / 180.0;
			
					//Debug.Log("A" + arduino);
					//Debug.Log ("S1 " + Sensor1.azimuth * 180.0 / Math.PI + " " + Sensor1.elevation* 180.0 / Math.PI  + " 2 " + Sensor2.azimuth * 180.0 / Math.PI + " " + Sensor2.elevation * 180.0 / Math.PI + " 3 " + Sensor3.azimuth * 180.0 / Math.PI + " " + Sensor3.elevation* 180.0 / Math.PI );
				} catch(TimeoutException e){
					continue;
				}
			} else {
				Debug.Log ("Stream not open");
			}

			// If there are more than 700 bytes in buffer,
			// assume it is not latest data and clear buffer
			if (stream.BytesToRead >= 700) {
				stream.DiscardInBuffer ();
				Debug.Log ("Discarding Buffer");
			}		
		}
		stream.Close ();
	}

	/*
	** Setting up thread
	*/ 
	void BeginThread(){
		if (!_threadrunning) {
			if (Port == null || Port.Length == 0) {
				Debug.LogError ("No port was specified");
				return;
			}
			if (BaudRate == 0) {
				BaudRate = 9600;
			}

			// Creating a new thread
			_thread = new Thread(OpenSerialPort);
			_threadrunning = true;
			_thread.Start();
		}
	}

	/*
	** Disabling the thread.
	*/ 
	void OnDisable(){
		if (_threadrunning) {
			_threadrunning = false;
			_thread.Join ();
		}
	}
}
