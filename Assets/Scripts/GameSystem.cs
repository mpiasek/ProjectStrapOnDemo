using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSystem : MonoBehaviour {
	public int round = 0;
    public GameObject[] targetsR1;
    public GameObject[] targetsR2;
    public GameObject[] targetsR3;
    public GameObject playerStatus;
    public Text targetsLeft;
	// Use this for initialization
	void Start () {
        foreach (GameObject t in targetsR1)
            t.SetActive(true);
        foreach (GameObject t in targetsR2)
            t.SetActive(false);
        foreach (GameObject t in targetsR3)
            t.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (targetsLeft.text.Equals("0"))
        {
            if (round == 1)
            {
                foreach (GameObject t in targetsR2)
                    t.SetActive(true);
                round++;
                targetsLeft.text = "7";
            }
            if (round == 2)
            {
                foreach (GameObject t in targetsR3)
                    t.SetActive(true);
                round++;
                targetsLeft.text = "10";
            }
        }
    }
    public void startRound()
    {
        round = 1;
        targetsLeft.text = "5";
        foreach (GameObject t in targetsR1)
            t.SetActive(true);
        foreach (GameObject t in targetsR2)
            t.SetActive(false);
        foreach (GameObject t in targetsR3)
            t.SetActive(false);
        return;
    }
}
