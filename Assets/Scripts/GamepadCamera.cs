using Cinemachine;
using UnityEngine;
[RequireComponent(typeof(CinemachineFreeLook))]
public class GamepadCamera : MonoBehaviour
{

    private CinemachineFreeLook freeLookCam;
    public Vector2 SensitityVector;
    // Use this for initialization
    void Start()
    {
        freeLookCam = GetComponent<CinemachineFreeLook>();

    }

    // Update is called once per frame
    private void LateUpdate()
    {

        {
            // var x = Input.GetAxis("Right Stick X") * SensitityVector.x * Time.deltaTime;
            // var y = Input.GetAxis("Right Stick Y") * SensitityVector.y * Time.deltaTime;
            // if (y != 0 || x != 0)
            // {
            //     freeLookCam.m_XAxis.Value += x;
            //     freeLookCam.m_YAxis.Value += y;
            // }

            // Debug.Log(x + ":" + y);
        }
    }
}