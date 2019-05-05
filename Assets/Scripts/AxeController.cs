using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool activated, activatedRethrow;
    public Rigidbody rb;
    public Transform AxeSlot;
    public TrailRenderer trail;
    public Collider col;
    public float rotationSpeed;
    public Vector3 target;
    WeaponController wpController;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            transform.localEulerAngles += new Vector3(transform.localEulerAngles.x, 0, 1 * -rotationSpeed) * Time.deltaTime;
            trail.emitting = true;
        }

        if (activatedRethrow)
        {
            transform.localEulerAngles += new Vector3(transform.localEulerAngles.x, 0, 1 * -rotationSpeed) * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, target, 0.3f);
            trail.emitting = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        activated = false;
        activatedRethrow = false;
        rb.isKinematic = true;
        col.isTrigger = true;
    }

    public IEnumerator Rethrow()
    {
        yield return new WaitForSeconds(0.5f);
        activatedRethrow = true;
    }
}
