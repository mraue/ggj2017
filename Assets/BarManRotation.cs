using UnityEngine;


public class BarManRotation : MonoBehaviour {

	public GameObject[] rotationPoints;
	public bool ignore;
	public int index = 0;
	int direction = 1;

	public int attentionTime = 60 * 2;

	[Range(0f, 1.0f)]
	public float slerpInterpolation = 0.1f;
	public AnimationCurve slerpCurve;

	void Start () {
	}

	// Update is called once per frame
	void Update () {
		
	}

	int tick = 0 ;

	public void IngnoreRotation(){
		ignore = true;
	}

	public void DontIngnoreRotation(){
		ignore = false;
	}

	void LateUpdate(){
		if (ignore){
			return;
		}
		tick ++;
		var oldRotation = transform.rotation;
		if (tick % attentionTime == 0) {
			if (index == rotationPoints.Length - 1){
				direction = -1;
			} else if (index == 0){
				direction = 1;
			}
			index = index + direction; 
		}
		transform.LookAt(rotationPoints[index].transform);
		transform.rotation = transform.rotation * Quaternion.Euler(0f,90f,0f);
		var newRotation = transform.rotation;
		float progress = ((tick % attentionTime) * 1f) / attentionTime;
		slerpInterpolation = slerpCurve.Evaluate(progress);
		transform.rotation = Quaternion.Slerp(oldRotation, newRotation, slerpInterpolation);
	}
}
