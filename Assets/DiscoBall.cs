using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DiscoBall : MonoBehaviour {
    public float speed;
	// Use this for initialization
	void Start () {
        transform.DORotate(new Vector3(0f, 360, 0f),speed,RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);	
	}
	
	
    
}
