using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector3 _movementVector;
    CharacterController _chController;
    WeaponController wpController;
    public Animator anim;
    public float Speed = 2;

    // Start is called before the first frame update
    void Start()
    {
        _chController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        wpController = GetComponentInChildren<WeaponController>();
    }

    // Update is called once per frame
    void Update()
    {
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");

        var forward = Camera.main.transform.forward;
        var right = Camera.main.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        var direction = forward * z + right * x;
        _movementVector = new Vector3(x, 0, z);

        _chController.Move(direction * Speed * Time.deltaTime);

        var isMoving = x != 0 || z != 0;
        anim.SetFloat("Speed", Mathf.Abs(x) + Mathf.Abs(z));
        if (isMoving)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), .1f);
        }
    }
}
