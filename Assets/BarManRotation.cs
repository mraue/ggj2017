using UnityEngine;


public class BarManRotation : MonoBehaviour
{

    public GameObject[] rotationPoints;
    public bool ignore;
    public int index = 0;
    int direction = 1;

    public int attentionTime = 60 * 2;

    [Range(0f, 1.0f)]
    public float slerpInterpolation = 0.1f;
    public AnimationCurve slerpCurve;
    [Range(0f, 1.0f)]
    public float LookAtThreshhold = 0.8f;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        for (int playerID = 0; playerID < rotationPoints.Length; playerID++)
        {
            Debug.DrawLine(transform.position, rotationPoints[playerID].transform.position, Color.cyan);

            float dot = Vector3.Dot(GetCorrectedForwardRotation(),
                (rotationPoints[playerID].transform.position - transform.position).normalized);
        }

        Debug.DrawRay(transform.position, GetCorrectedForwardRotation() * 5, Color.green);
        Debug.DrawRay(transform.position, Quaternion.Euler(0, 90 * (1 - LookAtThreshhold), 0) * GetCorrectedForwardRotation() * 5, Color.magenta);
        Debug.DrawRay(transform.position, Quaternion.Euler(0, -90 * (1 - LookAtThreshhold), 0) * GetCorrectedForwardRotation() * 5, Color.magenta);

    }

    private Vector3 GetCorrectedForwardRotation()
    {
        return (Quaternion.Euler(0, -90, 0) * transform.forward).normalized;
    }

    int tick = 0;

    public void IngnoreRotation()
    {
        ignore = true;
    }

    public void DontIngnoreRotation()
    {
        ignore = false;
    }

    void LateUpdate()
    {
        if (ignore)
        {
            return;
        }
        tick++;
        var oldRotation = transform.rotation;
        if (tick % attentionTime == 0)
        {
            if (index == rotationPoints.Length - 1)
            {
                direction = -1;
            }
            else if (index == 0)
            {
                direction = 1;
            }
            index = index + direction;
        }
        transform.LookAt(rotationPoints[index].transform);
        transform.rotation = transform.rotation * Quaternion.Euler(0f, 90f, 0f);
        var newRotation = transform.rotation;
        float progress = ((tick % attentionTime) * 1f) / attentionTime;
        slerpInterpolation = slerpCurve.Evaluate(progress);
        transform.rotation = Quaternion.Slerp(oldRotation, newRotation, slerpInterpolation);
    }

    public bool OnStartedWavinAtBartender(int playerID)
    {
        if (ignore) return false;

        float dot = Vector3.Dot(GetCorrectedForwardRotation(), (rotationPoints[playerID].transform.position - transform.position).normalized);

        bool isLookingAtPlayer = dot >= LookAtThreshhold;

        if (isLookingAtPlayer)
        {
            index = playerID;
            tick = 0;
        }
        return isLookingAtPlayer;

    }


}
