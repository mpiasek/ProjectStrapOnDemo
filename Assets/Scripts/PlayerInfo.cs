﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerInfo : MonoBehaviour {
	public int currentScore = 0;

	public int maximumBullets = 12;
	public int currentBullets = 12;

	/*	Score UI
	 */
	public Text scoreValue;


	/*	Bullet UI
	 */
	public Text bulletValue;

	// Use this for initialization
	void Start () {
		
		/*
		 * 	Health Initialization
		 */
//		healthBar	= transform.Find ("HealthBar").GetComponent<Slider>();
//		healthBar.maxValue = maximumHealth;
//		healthBar.minValue = 0;
		//resetScore ();

		/*
		 * 	Mana Initialization
		 */
//		manaBar 	= transform.Find ("ManaBar").GetComponent<Slider>();
//		manaBar.maxValue = maximumMana;
//		manaBar.minValue = 0;
		//resetBullets ();
	}
	
	// Update is called once per frame
	void Update () {
		/*
		 * 		Keyboard Testing
		 */
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
            deductBullets(1);
//			deductMana (5);
		} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
//			increaseHealth (5);
//			increaseMana (5);
	}
	
	}

	/*
	 * 		Affects HP
	 */
	public void targetHit(){
		currentScore -= 1;
		if (currentScore < 0) {
			currentScore = 0;
		}
		scoreValue.text = currentScore.ToString ();
	}

	public void increaseScore(int amount){
		currentScore += amount;
//		if (currentScore > maximumHealth) {
//			resetHealth ();
//		}
		scoreValue.text = currentScore.ToString();
	}

	public void resetScore(){
		currentScore = 5;
		scoreValue.text = currentScore.ToString ();
	}


	/*
	 * 		Affects Mana
	 */
	public void deductBullets(int amount){
		currentBullets -= amount;
		if (currentBullets < 1) {
			currentBullets = 0;
		}
		bulletValue.text = currentBullets.ToString() + "/" + maximumBullets.ToString();
	}
		
	public void increaseBullets(int amount){
		currentBullets += amount;
		if (currentBullets > maximumBullets) {
			resetBullets ();
		}
		bulletValue.text = currentBullets.ToString() + "/" + maximumBullets.ToString();
	}

	public void resetBullets(){
		currentBullets = maximumBullets;
		bulletValue.text = currentBullets.ToString() + "/" + maximumBullets.ToString();
	}
}
