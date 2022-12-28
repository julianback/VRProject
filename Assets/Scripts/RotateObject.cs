using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    public int rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //transform.RotateAround(_target.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);

        // https://stackoverflow.com/questions/66286142/unity-2d-how-to-rotate-an-object-smoothly-inside-a-coroutine
        StopAllCoroutines(); //to prevent overlapping
        StartCoroutine(Rotate());
    }

    IEnumerator Rotate()
    {
        Quaternion initialRotation = transform.rotation;
        float t = 0;

        while (t < 1f)
        {
            t = Mathf.Min(1f, t + Time.deltaTime);
            Vector3 newEulerOffset = Vector3.up * rotationSpeed * t;
            transform.rotation = Quaternion.Euler(newEulerOffset) * initialRotation; // global rotation
            // transform.rotation = startRotation * Quaternion.Euler(newEulerOffset); // local rotation
            yield return null;
        }
    }
}
