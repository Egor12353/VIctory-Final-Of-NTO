using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class CarController : MonoBehaviour
{
    [Header("References")]
    public CarEngineController carEngine;
    public Rigidbody rb;
    public LinearDrive ld;
    public CircularDrive steeringWheel;
    public AudioSource idleMotorSound;

    [Header("Wheel Colliders")]
    public WheelCollider frontLeftWheelCollider;
    public WheelCollider frontRightWheelCollider;
    public WheelCollider rearLeftWheelCollider;
    public WheelCollider rearRightWheelCollider;

    [Header("Wheel Transforms")]
    public Transform frontLeftWheelTransform;
    public Transform frontRightWheelTransform;
    public Transform rearLeftWheelTransform;
    public Transform rearRightWheelTransform;

    [Header("VR Inputs")]
    public SteamVR_Action_Single throttleButton;
    public SteamVR_Action_Single brakeButton;
    public SteamVR_Action_Boolean CarEngine;

    [Header("Settings")]
    public float motorForce = 2000f;
    public float brakeForce = 3000f;
    public float maxSteerAngle = 30f;
    public float idleSpeed = 5f;
    public float backwardSpeedMultiplier = 1.3f;
    public bool isInTruck = true;

    private float throttleInput;
    private float brakeInput;
    private float steeringInput;
    private bool isMovingForward = true;
    public bool isStopped = false;
    public bool inCityMode = false;

    [Header("References")]
    public EnterInCar enterInCar;

    void FixedUpdate()
    {
        UpdateCarState();

        if (!isStopped && carEngine.isEngineStarted && enterInCar.inDrive)
        {
            GetInput();
            HandleMotor();
            HandleSteering();
            HandleBrake();
        }
        else
        {
            StopVehicle();
        }

        UpdateWheels();
    }

    public void OnEngineStarted()
    {
        idleMotorSound.Play();
    }

    public void OnEngineStopped()
    {
        idleMotorSound.Stop();
    }

    void UpdateCarState()
    {
        isStopped = ld.stop;
        isMovingForward = !isStopped && ld.fd;
        inCityMode = isMovingForward;
    }

    void GetInput()
    {
        throttleInput = throttleButton.GetAxis(SteamVR_Input_Sources.RightHand);
        brakeInput = brakeButton.GetAxis(SteamVR_Input_Sources.LeftHand);
        steeringInput = steeringWheel.outAngle / 540;

        if (idleMotorSound.isPlaying)
        {
            idleMotorSound.pitch = 1 + throttleInput / 3;
            idleMotorSound.volume = 0.5f + throttleInput / 2;
        }
    }

    void HandleMotor()
    {
        float speed = throttleInput > 0 ? throttleInput * motorForce : idleSpeed;
        float direction = isMovingForward ? -1 : backwardSpeedMultiplier;
        float torque = speed * direction;

        frontLeftWheelCollider.motorTorque = torque;
        frontRightWheelCollider.motorTorque = torque;
        rearLeftWheelCollider.motorTorque = torque;
        rearRightWheelCollider.motorTorque = torque;
    }

    void HandleBrake()
    {
        float brakeTorque = brakeInput * brakeForce;

        frontLeftWheelCollider.brakeTorque = brakeTorque;
        frontRightWheelCollider.brakeTorque = brakeTorque;
        rearLeftWheelCollider.brakeTorque = brakeTorque;
        rearRightWheelCollider.brakeTorque = brakeTorque;
    }

    void StopVehicle()
    {
        frontLeftWheelCollider.motorTorque = 0;
        frontRightWheelCollider.motorTorque = 0;
        rearLeftWheelCollider.motorTorque = 0;
        rearRightWheelCollider.motorTorque = 0;

        frontLeftWheelCollider.brakeTorque = brakeForce;
        frontRightWheelCollider.brakeTorque = brakeForce;
        rearLeftWheelCollider.brakeTorque = brakeForce;
        rearRightWheelCollider.brakeTorque = brakeForce;
    }

    void HandleSteering()
    {
        float steerAngle = maxSteerAngle * steeringInput;

        if (steeringInput >= 0)
        {
            frontLeftWheelCollider.steerAngle = steerAngle;
            frontRightWheelCollider.steerAngle = steerAngle + steeringInput * 10;
        }
        else
        {
            frontLeftWheelCollider.steerAngle = steerAngle + steeringInput * 10;
            frontRightWheelCollider.steerAngle = steerAngle;
        }
    }

    void UpdateWheels()
    {
        UpdateWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateWheel(rearRightWheelCollider, rearRightWheelTransform);
    }

    void UpdateWheel(WheelCollider collider, Transform wheelTransform)
    {
        collider.GetWorldPose(out Vector3 position, out Quaternion rotation);
        wheelTransform.position = position;
        wheelTransform.rotation = rotation;
    }
    // В вашем скрипте CarController
    public float GetCurrentSpeedKPH()
    {
        // Используем ссылку на Rigidbody из полей класса (rb)
        
        return rb.velocity.magnitude * 5.6f;
    }
}