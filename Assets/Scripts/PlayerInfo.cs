using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerInfo : MonoBehaviour {
	public int maximumHealth = 50;
	public int currentHealth = 50;

	public int maximumMana = 50;
	public int currentMana = 50;

	/*	Health UI
	 */
	private Text healthValue;
	private Slider healthBar;

	/*	Mana UI
	 */
	private Text manaValue;
	private Slider manaBar;

	// Use this for initialization
	void Start () {
		
		/*
		 * 	Health Initialization
		 */
		healthValue = transform.Find ("HealthValue").GetComponent<Text>();
		healthBar	= transform.Find ("HealthBar").GetComponent<Slider>();
		healthBar.maxValue = maximumHealth;
		healthBar.minValue = 0;
		resetHealth ();

		/*
		 * 	Mana Initialization
		 */
		manaValue	= transform.Find ("ManaValue").GetComponent<Text>();
		manaBar 	= transform.Find ("ManaBar").GetComponent<Slider>();
		manaBar.maxValue = maximumMana;
		manaBar.minValue = 0;
		resetMana ();
	}
	
	// Update is called once per frame
	void Update () {
		/*
		 * 		Keyboard Testing
		 */
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			deductHealth (5);
			deductMana (5);
		} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
			increaseHealth (5);
			increaseMana (5);
		}
	
	}

	/*
	 * 		Affects HP
	 */
	void deductHealth(int amount){
		currentHealth -= amount;
		if (currentHealth < 0) {
			currentHealth = 0;
		}
		healthValue.text = currentHealth.ToString() + "/" + maximumHealth.ToString();
		healthBar.value = currentHealth;
	}

	void increaseHealth(int amount){
		currentHealth += amount;
		if (currentHealth > maximumHealth) {
			resetHealth ();
		}
		healthValue.text = currentHealth.ToString() + "/" + maximumHealth.ToString();
		healthBar.value = currentHealth;
	}

	void resetHealth(){
		currentHealth = maximumHealth;
		healthValue.text = currentHealth.ToString() + "/" + maximumHealth.ToString();
		healthBar.value = currentHealth;
	}


	/*
	 * 		Affects Mana
	 */
	void deductMana(int amount){
		currentMana -= amount;
		if (currentMana < 0) {
			currentMana = 0;
		}
		manaValue.text = currentMana.ToString() + "/" + maximumMana.ToString();
		manaBar.value = currentMana;
	}
		
	void increaseMana(int amount){
		currentMana += amount;
		if (currentMana > maximumMana) {
			resetMana ();
		}
		manaValue.text = currentMana.ToString() + "/" + maximumMana.ToString();
		manaBar.value = currentMana;
	}

	void resetMana(){
		currentMana = maximumMana;
		manaValue.text = currentMana.ToString() + "/" + maximumMana.ToString();
		manaBar.value = currentMana;
	}
}
