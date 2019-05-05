using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
public class WeaponController : MonoBehaviour
{
    // Start is called before the first frame update
    public float Force, ForceUp, RotationSpeed, cameraFovValue;
    public Transform curvePoint;
    public Image reticle;
    public Transform Spine, point;
    public CinemachineFreeLook freeLookCamera;
    public ParticleSystem CatchEffect;
    PlayerMovement player;
    AxeController axe;
    bool hasAxe = true, recalling = false, canRethrow = false, isAiming = false;
    float returnTime;
    Vector3 pullPosition;
    void Start()
    {
        axe = GetComponentInChildren<AxeController>();
        player = GetComponentInChildren<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        point.rotation = Camera.main.transform.rotation;
        isAiming = (Input.GetButton("Fire2") || Input.GetAxis("LeftTrigger") > 0);
        player.anim.SetBool("aiming", isAiming);

        //UI
        var newColor = isAiming ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0);
        reticle.color = Color.Lerp(reticle.color, newColor, 0.1f);

        if (isAiming && !recalling)
        {
            var staticRot = Camera.main.transform.localEulerAngles;
            transform.localEulerAngles = new Vector3(0, staticRot.y, 0);

            if ((Input.GetButton("Fire1") || (Input.GetAxis("RightTrigger") > 0)) && axe.rb.isKinematic && canRethrow && !hasAxe)
            {
                axe.rb.isKinematic = false;
                axe.activated = true;
                axe.rb.AddForce(Vector3.up * 10, ForceMode.Impulse);
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
                {
                    axe.target = hit.point;
                    StartCoroutine(axe.Rethrow());
                };
            }

            if ((Input.GetButton("Fire1") || (Input.GetAxis("RightTrigger") > 0)) && hasAxe)
            {
                player.anim.SetTrigger("throw");
                hasAxe = false;
            }

            if (Input.GetButton("Action") && !hasAxe)
            {
                player.anim.SetBool("recalling", true);
                WeaponRecall();
            }
        }

        if (recalling)
        {
            player.anim.SetBool("recalling", true);
            axe.transform.position = GetQuadraticCurvePoint(returnTime, pullPosition, curvePoint.position, axe.AxeSlot.position);
            returnTime += Time.deltaTime * 1f;

            axe.transform.localEulerAngles += axe.transform.forward * axe.rotationSpeed * Time.deltaTime;

            if (Vector3.Distance(transform.position, axe.transform.position) < 2f)
            {
                recalling = false;
                axe.transform.parent = axe.AxeSlot.transform;
                axe.transform.rotation = axe.AxeSlot.rotation;
                axe.transform.position = axe.AxeSlot.position;
                Recalled();
            }
        }
    }

    private void LateUpdate()
    {
        if (isAiming)
        {
            CameraFov(cameraFovValue);
        }
        else
        {
            CameraFov(50);
        }
    }

    void CameraFov(float fov)
    {
        freeLookCamera.m_Lens.FieldOfView = Mathf.Lerp(freeLookCamera.m_Lens.FieldOfView, fov, 0.1f);
    }

    public void Throw()
    {
        canRethrow = true;
        axe.transform.parent = null;
        axe.activated = true;
        axe.rb.isKinematic = false;
        axe.rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        axe.transform.eulerAngles = new Vector3(0, -90 + transform.eulerAngles.y, 0);
        axe.transform.position += transform.right / 5;
        axe.col.isTrigger = false;
        axe.rb.AddForce(point.forward * Force + transform.up * ForceUp, ForceMode.Impulse);
    }

    public void Recall()
    {
        recalling = true;
        pullPosition = axe.transform.position;
        axe.transform.eulerAngles = Vector3.zero;
        axe.activated = false;
        axe.rb.isKinematic = true;
        axe.col.isTrigger = true;
        axe.rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        axe.transform.position += transform.right / 5;
    }

    public void Recalled()
    {
        player.anim.SetBool("recalling", false);
        CatchEffect.Play();
        recalling = false;
        canRethrow = false;
        axe.trail.emitting = false;
        returnTime = 0;
        hasAxe = true;
    }

    public void WeaponThrow()
    {
        Throw();
    }

    public void WeaponRecall()
    {
        Recall();
    }

    public Vector3 GetQuadraticCurvePoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        return (uu * p0) + (2 * u * t * p1) + (tt * p2);
    }

}
