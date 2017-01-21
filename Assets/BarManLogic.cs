using System.Collections;
using UnityEngine;

public class BarManLogic : MonoBehaviour
{
    [Header("LookAt")]
    private Animator ani;

    void Start()
    {
        ani = GetComponent<Animator>();
    }

    void Update()
    {
        if (Random.Range(0f, 500f) < 1f)
        {
            ani.SetTrigger("Dance");
            BroadcastMessage("IngnoreRotation");
            StartCoroutine(Idle());
        }
    }

    IEnumerator Idle()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 4f));
        ani.SetTrigger("Idle");
        BroadcastMessage("DontIngnoreRotation");
    }

}