using UnityEngine;
using System;
using System.Collections;
using System.IO.Ports;
using System.Threading;
using AssemblyCSharp;

public class BraceletSerialPort : MonoBehaviour {
	SerialPort stream;
	public string Port;
	public int BaudRate;
	public int TimeOut;
	public int MessageCounter = 0;

	private bool _threadrunning;
	Thread _thread;

	public TrackingAlgorithmDouble.objectLocation Sensor1;
	public TrackingAlgorithmDouble.objectLocation Sensor2;
	public TrackingAlgorithmDouble.objectLocation Sensor3;
	public TrackingAlgorithmDouble.Imu Imu;

	/*
	** Setting up thread
	*/
	void Start(){
		BeginThread ();

		Imu.v0.x = 0;
		Imu.v0.y = 0;
		Imu.v0.z = 0;
		Imu.s0.x = 0;
		Imu.s0.y = 0;
		Imu.s0.z = 0;
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
					//Debug.Log ("Line is read");
					string[] parser = arduino.Split (' ');
					float temp;

					MessageCounter++; //Used to enumerate the number of lines read from serial
					//a temporary way of determing when to clear the serial input buffer

					// Right now we don't process IMU. 
					if(parser[0] == "TS3633"){
						//Debug.Log("Read from TS3633");
						if (!float.TryParse (parser [2], out temp)) {
							Debug.Log ("Unable to convert 1Azimuth");
							continue;
						};
						Sensor1.azimuth = temp * Math.PI / 180.0;

						if (!float.TryParse (parser [3], out temp)) {
							Debug.Log ("Unable to convert 1Elevation");
							continue;
						}
						Sensor1.elevation = (180.0 - temp)* Math.PI / 180.0;

						if (!float.TryParse (parser [5], out temp)) {
							Debug.Log ("Unable to convert 2Azimuth");
							continue;
						};
						Sensor2.azimuth = temp * Math.PI / 180.0;

						if (!float.TryParse (parser [6], out temp)) {
							Debug.Log ("Unable to convert 2Elevation");
							continue;
						}
						Sensor2.elevation = (180.0 - temp) * Math.PI / 180.0;

						if (!float.TryParse (parser [8], out temp)) {
							Debug.Log ("Unable to convert 3Azimuth");
							continue;
						};
						Sensor3.azimuth = temp * Math.PI / 180.0;

						if (!float.TryParse (parser [9], out temp)) {
							Debug.Log ("Unable to convert 3Elevation");
							continue;
						}
						Sensor3.elevation = (180.0 - temp) * Math.PI / 180.0;

						//Debug.Log("A" + arduino);
						//Debug.Log ("S1 " + Sensor1.azimuth * 180.0 / Math.PI + " " + Sensor1.elevation* 180.0 / Math.PI  + " 2 " + Sensor2.azimuth * 180.0 / Math.PI + " " + Sensor2.elevation * 180.0 / Math.PI + " 3 " + Sensor3.azimuth * 180.0 / Math.PI + " " + Sensor3.elevation* 180.0 / Math.PI );

					}
					else if(parser[0] == "IMU")
					{
						if (!float.TryParse (parser [1], out temp)) {
							Debug.Log ("Unable to find Qw");
							continue;
						};
						Imu.q.w = temp;

						if (!float.TryParse (parser [2], out temp)) {
							Debug.Log ("Unable to find Qx");
							continue;
						};
						Imu.q.x = temp;

						if (!float.TryParse (parser [3], out temp)) {
							Debug.Log ("Unable to find Qy");
							continue;
						};
						Imu.q.y = temp;

						if (!float.TryParse (parser [4], out temp)) {
							Debug.Log ("Unable to find Qz");
							continue;
						};
						Imu.q.z = temp;

						if (!float.TryParse (parser [5], out temp)) {
							Debug.Log ("Unable to find World Acceleration X");
							continue;
						};
						Imu.a.x = (temp / 8192f) * 981.0f;

						if (!float.TryParse (parser [6], out temp)) {
							Debug.Log ("Unable to find World Acceleration Y");
							continue;
						};
						Imu.a.y = (temp / 8192f) * 981.0f;

						if (!float.TryParse (parser [7], out temp)) {
							Debug.Log ("Unable to find World Acceleration Z");
							continue;
						};
						Imu.a.z = (temp / 8192f) * 981.0f;

						if (!float.TryParse (parser [8], out temp)) {
							Debug.Log ("Unable to find Delta Time");
							continue;
						};
						Imu.deltaT = 10f/1000f; //TODO: systick on uc side affect millis() accuracy. So IMU read time calculations on uc affected.
						//Debug.Log ("Acceleration: " + Imu.a.x + " " + Imu.a.y + " " + Imu.a.z);
						//Debug.Log ("Position: " + Imu.s.x + " " + Imu.s.y + " " + Imu.s.z);
					}
					else if(parser[0] == "OOTC") //timer underflow (sensor blocked)
					{
						Sensor1.azimuth = 0;
						Sensor1.elevation = 0;
						Sensor2.azimuth = 0;
						Sensor2.elevation = 0;
						Sensor3.azimuth = 0;
						Sensor3.elevation = 0;
					}
					else
					{
						continue;
					}
					} catch(TimeoutException e){
					continue;
				}
			} else {
				Debug.Log ("Stream not open");
			}

			// If there are more than 700 bytes in buffer,
			// assume it is not latest data and clear buffer
			/*if (stream.BytesToRead >= 700) {
				stream.DiscardInBuffer ();
				Debug.Log ("Discarding Buffer");
			}*/	

			if (MessageCounter >= 10) {
				stream.DiscardInBuffer ();
				MessageCounter = 0;
				Debug.Log ("Discarding Buffer");
			}
		}
		Debug.Log ("Closing Stream");
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
