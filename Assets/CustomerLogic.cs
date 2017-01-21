using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerLogic : MonoBehaviour {
    private Animator ani;
    public KeyCode keypress;
	// Use this for initialization
	void Start () {
        ani = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(keypress)) {
            ani.SetTrigger("Wave");

        }
        if (Input.GetKeyUp(keypress))
        {
            ani.SetTrigger("Idle");

        }
    }
}
