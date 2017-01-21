using UnityEngine;

public class CustomerLogic : MonoBehaviour
{
    public KeyCode keypress;
    public Transform Barman;
    public int SucessfullPressesForBeer;
    private int sucessfullPresses;
    [Range(0.1f, 1.0f)]
    public float AccuraczToHitSucessfuly = 0.8f;

    public static event System.Action<CustomerLogic> OnWavedEnough;

    void Update()
    {
        transform.LookAt(Barman.transform);
        if (Input.GetKeyDown(keypress))
        {
            float accuracy = Vector3.Dot(Barman.transform.forward, (transform.position - Barman.transform.position).normalized);
            Debug.Log(name + " pressed with accuracy: " + accuracy);
            if (accuracy > AccuraczToHitSucessfuly)
            {
                HandleSucessfullPress();
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
