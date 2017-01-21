using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class BarManLogic : MonoBehaviour
{
    [Header("LookAt")]
//    public Transform Target;
//    public float Duration = 1;
//    private Tween lookAtTween;
//    private bool isLooking;

//    [Header("Players")]
//    public List<Transform> Players;
    private Animator ani;

    private void Awake()
    {
        CustomerLogic.OnWavedEnough += OnWavedEnoughHandler;
    }

    private void OnDestroy()
    {
        CustomerLogic.OnWavedEnough -= OnWavedEnoughHandler;
    }

    private void OnWavedEnoughHandler(CustomerLogic customerLogic)
    {
        Debug.Log("gigving drink to " + customerLogic.name);
    }

    void Start()
    {
        ani = GetComponent<Animator>();
	}

	void Update () {
        if (Random.Range(0f, 500f) < 1f) {
            ani.SetTrigger("Dance");
			BroadcastMessage("IngnoreRotation");
            StartCoroutine(Idle());
        }
	}

    IEnumerator Idle() {
        yield return new WaitForSeconds(Random.Range(0.5f, 4f));
        ani.SetTrigger("Idle");
		BroadcastMessage("DontIngnoreRotation");
    }

//    void Update()
//    {
//        transform.LookAt(Target);
//
//        //if (!isLooking)
//        //{
//
//        //    lookAtTween = transform.DOLookAt(Target.transform.position, Duration)
//        //        .OnPlay(() => isLooking = true)
//        //        .OnComplete((() => isLooking = false))
//        //        .Play();
//        //}
//
//        Debug.DrawLine(transform.position, Target.transform.position, Color.green);
//        Debug.DrawRay(transform.position, transform.forward, Color.magenta);
//        Debug.DrawRay(Target.transform.position, Vector3.up, Color.red);
//    }

}