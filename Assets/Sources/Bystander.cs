using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bystander : MonoBehaviour
{

    void Start()
    {
        transform.localScale = Vector3.one * Random.Range(0.9f, 1.1f);
        StartCoroutine(StartAni());    }
    IEnumerator StartAni()
    {
        yield return new WaitForSeconds(Random.Range(0f, 2f));
        GetComponent<Animator>().SetTrigger(Random.Range(1, 7).ToString());
    }
}