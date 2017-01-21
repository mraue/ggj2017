using UnityEngine;

public class CustomerLogic : MonoBehaviour
{
    public KeyCode keypress;
    public Transform Barman;
    public int SucessfullPressesForBeer;
    private int sucessfullPresses;
    [Range(0.1f, 1.0f)]
    public float AccuracyToHitSucessfuly = 0.8f;

    public static event System.Action<CustomerLogic> OnWavedEnough;

    void Update()
    {

        if (Input.GetKeyDown(keypress))
        {
            float accuracy = Vector3.Dot(Barman.transform.forward, (transform.position - Barman.transform.position).normalized);

            if (accuracy > AccuracyToHitSucessfuly)
            {
                Debug.Log("Sucessful press from " + name + " with accuracy: " + accuracy);
                HandleSucessfullPress();
            }
            else
            {
                Debug.Log("Failed press from " + name + " with accuracy: " + accuracy);
            }

        }

    }

    private void HandleSucessfullPress()
    {
        sucessfullPresses++;

        if (sucessfullPresses >= SucessfullPressesForBeer)
        {
            sucessfullPresses = 0;

            if (OnWavedEnough != null)
            {
                OnWavedEnough(this);
            }
        }
    }
}
