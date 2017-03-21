using System;
using System.IO;
using System.Collections;
using UnityEngine;

namespace AssemblyCSharp
{

	static public class TrackingAlgorithmDouble
	{
		public struct objectLocation{
			public double azimuth;
			public double elevation;
			public double distance;
		};

		public struct sensorDistance{
			public double AB;
			public double BC;
			public double AC;
		};

		public struct Imu{
			public Quaternion q;
			public Vector3 a;
			public float deltaT;
			public Vector3 s;
			public Vector3 s0;
			public Vector3 v;
			public Vector3 v0;
		};
			
		//Uses equation (where B = Elevation and O = Azymuth)
		//sin(B1)cos(O1)sin(B2)cos(O2) + sin(B1)sin(O1)sin(b2)sin(o1) + cos(b1)cos(b2)
		//Only calculate once for each cosAB, cosBC, cosAC
		static private double calculateCos( ref objectLocation s1,  ref objectLocation s2){
			return  Math.Sin (s1.elevation) * Math.Cos (s1.azimuth) * Math.Sin (s2.elevation) *
				Math.Cos (s2.azimuth) + Math.Sin (s1.elevation) * Math.Sin (s1.azimuth) *
				Math.Sin(s2.elevation) * Math.Sin(s2.azimuth) + Math.Cos(s1.elevation) *
				Math.Cos(s2.elevation);
		}

		//Calculate F(Rx, Ry) 
		//Uses equation:
		// Rx^2 + Ry^2 - 2RxRyCosxy - xy^2
		//Where xy is lenght of side xy and cosxy is calculated in calculateCos
		static private double calculateF(double Rx, double Ry, double xy, double cosxy){
			return ((Rx*Rx) + (Ry*Ry) - (2*Rx*Ry*cosxy) - (xy*xy));
		}

		static private double jacobianCalc(double Rx, double Ry, double cosxy){
			return 2*Rx - 2*Ry*cosxy;
		}


		// FIND C# Library Equivalent

		static private bool isWithinAcceptableBounds(ref double[] F){
			double bounds = 0.01;
			if(Math.Abs(F[0]) < bounds && Math.Abs(F[1]) < bounds && Math.Abs(F[2]) < bounds){
				return true;
			} 
			return false;
		}


		static public void runAlgorithm(ref objectLocation s1, ref objectLocation s2, ref objectLocation s3, ref sensorDistance triangle){
			
			//Debug.Log ("Sensor1 Azimuth: " + s1.azimuth * 180.0 / Math.PI + "\nSensor1 Elevation: " + s1.elevation * 180.0 / Math.PI);
			//Debug.Log ("Sensor2 Azimuth: " + s2.azimuth * 180.0 / Math.PI + "\nSensor2 Elevation: " + s2.elevation * 180.0 / Math.PI);
			//Debug.Log ("Sensor3 Azimuth: " + s3.azimuth * 180.0 / Math.PI + "\nSensor3 Elevation: " + s3.elevation *180.0 / Math.PI);

			//Calculate each CosAB, CosBC, CosAC
			double cos12 = calculateCos(ref s1,ref s2); 
			double cos23 = calculateCos(ref s2,ref s3);
			double cos13 = calculateCos(ref s1,ref s3);

			// Consider changing to previous state
			//Generate random values for RA, RB, RC
			//TODO: Make it actually random
			s1.distance = 20.66;
			s2.distance = 20.58;
			s3.distance = 20.79;
			//Debug.Log ("Position: " + s1.distance + " " + s2.distance + " " + s3.distance);

			// C# Library Equivalent
			double [,] J 		= new double[3,3];
			double[,] JInverse 	= new double[3,3];
			double [] F 		= new double[3];
			double [] Last_R = new double[3];
			double [] Curr_R = new double[3];

			string path = "algorithmIterationsTime.txt";

			var watch = System.Diagnostics.Stopwatch.StartNew();
			//Begin itteration with a for loop or whatever
			for (int i = 0; i < 2000; i++) {
				Last_R [0] = s1.distance;
				Last_R [1] = s2.distance;
				Last_R [2] = s3.distance;

				//Calculate F matrix and check if it is within acceptable error threshold
				F [0] = calculateF (s1.distance, s2.distance, triangle.AB, cos12);
				F [1] = calculateF (s2.distance, s3.distance, triangle.BC, cos23);
				F [2] = calculateF (s1.distance, s3.distance, triangle.AC, cos13);

				if (isWithinAcceptableBounds (ref F)) {
					break;
				} 

				//If not then Calculate Jacobian Matrix (can probably be separate Function)	
				J [0, 0] = jacobianCalc (s1.distance, s2.distance, cos12);
				J [1, 0] = 0;
				J [2, 0] = jacobianCalc (s1.distance, s3.distance, cos13);

				J [0, 1] = jacobianCalc (s2.distance, s1.distance, cos12);
				J [1, 1] = jacobianCalc (s2.distance, s3.distance, cos23);
				J [2, 1] = 0;

				J [0, 2] = 0;
				J [1, 2] = jacobianCalc (s3.distance, s2.distance, cos23);
				J [2, 2] = jacobianCalc (s3.distance, s1.distance, cos13);



				// Calculate inverse matrix of J
				// A = (ei - fh)
				JInverse [0, 0] = J [1, 1] * J [2, 2] - J [1, 2] * J [2, 1];
				// D = -(bi-ch)
				JInverse [0, 1] = -(J [0, 1] * J [2, 2] - J [0, 2] * J [2, 1]);
				// G = (bf-ce)
				JInverse [0, 2] = J [0, 1] * J [1, 2] - J [0, 2] * J [1, 1];

				// B = -(di -fg)
				JInverse [1, 0] = -(J [1, 0] * J [2, 2] - J [1, 2] * J [2, 0]);
				// E = (ai-cg)
				JInverse [1, 1] = J [0, 0] * J [2, 2] - J [0, 2] * J [2, 0];
				// H = -(af-cd)
				JInverse [1, 2] = -(J [0, 0] * J [1, 2] - J [0, 2] * J [1, 0]);


				// C = (dh-eg)
				JInverse [2, 0] = J [1, 0] * J [2, 1] - J [1, 1] * J [2, 0];
				// F = -(ah-bg)
				JInverse [2, 1] = -(J [0, 0] * J [2, 1] - J [0, 1] * J [2, 0]);
				// I = (ae-bd)
				JInverse [2, 2] = J [0, 0] * J [1, 1] - J [0, 1] * J [1, 0];
				/*
				Debug.Log ("J[0,0]: " + J[0,0] );
				Debug.Log ("J[1,0]: " + J[1,0] );
				Debug.Log ("J[2,0]: " + J[2,0] );

				Debug.Log ("J[0,1]: " + J[0,1] );
				Debug.Log ("J[1,1]: " + J[1,1] );
				Debug.Log ("J[2,1]: " + J[2,1] );

				Debug.Log ("J[0,2]: " + J[0,2] );
				Debug.Log ("J[1,2]: " + J[1,2] );
				Debug.Log ("J[2,2]: " + J[2,2] );

				Debug.Log ("JInverse[0,0]: " + JInverse[0,0] );
				Debug.Log ("JInverse[0,1]: " + JInverse[0,1] );
				Debug.Log ("JInverse[0,2]: " + JInverse[0,2] );

				Debug.Log ("JInverse[1,0]: " + JInverse[1,0] );
				Debug.Log ("JInverse[1,1]: " + JInverse[1,1] );
				Debug.Log ("JInverse[1,2]: " + JInverse[1,2] );

				Debug.Log ("JInverse[2,0]: " + JInverse[2,0] );
				Debug.Log ("JInverse[2,1]: " + JInverse[2,1] );
				Debug.Log ("JInverse[2,2]: " + JInverse[2,2] );
				*/
				double detA = J [0, 0] * JInverse [0, 0] + J [0, 1] * JInverse [1, 0] + J [0, 2] * JInverse [2, 0];


				for (int y = 0; y < 3; y++) {
					for (int x = 0; x < 3; x++) {
						JInverse [y, x] = JInverse [y, x] / detA;
					}
				}

				//Calculate RA_k+1, RB_k+1, RC_k+1 with previously calulated values as done in equation 18
				Curr_R [0] = Last_R [0] - (JInverse [0, 0] * F [0] + JInverse [0, 1] * F [1] + JInverse [0, 2] * F [2]);
				Curr_R [1] = Last_R [1] - (JInverse [1, 0] * F [0] + JInverse [1, 1] * F [1] + JInverse [1, 2] * F [2]);
				Curr_R [2] = Last_R [2] - (JInverse [2, 0] * F [0] + JInverse [2, 1] * F [1] + JInverse [2, 2] * F [2]);

				s1.distance = Curr_R [0];
				s2.distance = Curr_R [1];
				s3.distance = Curr_R [2];

				/*
				Debug.Log ("F[0]: " + F[0]);
				Debug.Log ("F[1]: " + F[1]);
				Debug.Log ("F[2]: " + F[2]);
				
				Debug.Log ("Curr_R[0]: " + Curr_R[0] );
				Debug.Log ("Curr_R[1]: " + Curr_R[1] );
				Debug.Log ("Curr_R[2]: " + Curr_R[2] );

				Debug.Log ("J[0,0]: " + J[0,0] );
				Debug.Log ("J[1,0]: " + J[1,0] );
				Debug.Log ("J[2,0]: " + J[2,0] );

				Debug.Log ("J[0,1]: " + J[0,1] );
				Debug.Log ("J[1,1]: " + J[1,1] );
				Debug.Log ("J[2,1]: " + J[2,1] );

				Debug.Log ("J[0,2]: " + J[0,2] );
				Debug.Log ("J[1,2]: " + J[1,2] );
				Debug.Log ("J[2,2]: " + J[2,2] );

				Debug.Log ("JInverse[0,0]: " + JInverse[0,0] );
				Debug.Log ("JInverse[0,1]: " + JInverse[0,1] );
				Debug.Log ("JInverse[0,2]: " + JInverse[0,2] );

				Debug.Log ("JInverse[1,0]: " + JInverse[1,0] );
				Debug.Log ("JInverse[1,1]: " + JInverse[1,1] );
				Debug.Log ("JInverse[1,2]: " + JInverse[1,2] );

				Debug.Log ("JInverse[2,0]: " + JInverse[2,0] );
				Debug.Log ("JInverse[2,1]: " + JInverse[2,1] );
				Debug.Log ("JInverse[2,2]: " + JInverse[2,2] );

				Debug.Log ("detA: " + detA);
				*/
			}
			watch.Stop();
			//var elapsedMs = watch.Elapsed.TotalMilliseconds;
			//File.AppendAllText (path, elapsedMs.ToString() + "\n");
		}
	}

}