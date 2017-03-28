using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

public class GunControl : NVRInteractableItem
{

	public Transform FirePoint;
	public LayerMask targetMask;
	private Vector3 hitPoint;
	public GameObject playerStatus;

	private bool findTarget(Vector3 hit) {
		GameObject[] gos = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[]; //will return an array of all GameObjects in the scene
		foreach(GameObject go in gos)
		{
			if(go.layer == 8)
			{
				if (go.GetComponentInParent<Collider>().bounds.Contains(hit))
				{
					go.SetActive(false);
					return true;
				}
			}	
		}
		return false;
	}
	public override void UseButtonDown()
	{
		base.UseButtonDown();
        print("hi");
		//if (playerStatus.GetComponent<PlayerInfo>().currentBullets > 0) {
		//	playerStatus.GetComponent<PlayerInfo>().deductBullets(1);
			// Add reference to rounds in magazine
			AttachedHand.TriggerHapticPulse(2000);
			RaycastHit hit;
			// Shoot a ray from controller, if it hits store the hit point, check if it hit a target, and increase score
			if (Physics.Raycast (FirePoint.position, FirePoint.forward, out hit, 100, targetMask)) {
				hitPoint = hit.point;
				if (findTarget (hitPoint)) 
					playerStatus.GetComponent<PlayerInfo> ().targetHit();
			
			}
		//}
		//else {
		//	playerStatus.GetComponent<PlayerInfo> ().resetBullets();
		//}
	}
}
