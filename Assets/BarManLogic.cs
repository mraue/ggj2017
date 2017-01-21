using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarManLogic : MonoBehaviour {
    private Animator ani;
	// Use this for initialization
	void Start () {
        ani = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Random.Range(0f, 500f) < 1f) {
            ani.SetTrigger("Dance");
            StartCoroutine(Idle());
        }
	}
    IEnumerator Idle() {
        yield return new WaitForSeconds(Random.Range(0.5f, 4f));
        ani.SetTrigger("Idle");
    }
}
