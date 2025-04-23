using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class CarController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private LinearDrive ld;
    [SerializeField] private CircularDrive steeringWheel;
    [SerializeField] private AudioSource idleMotorSound;

    [Header("Wheel Colliders")]
    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [Header("Wheel Transforms")]
    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    [Header("VR Input Actions")]
    [SerializeField] private SteamVR_Action_Single throttleButton;
    [SerializeField] private SteamVR_Action_Single brakeButton;
    [SerializeField] private SteamVR_Action_Boolean fdButton;
    [SerializeField] private SteamVR_Action_Boolean backButton;

    [Header("Settings")]
    [SerializeField] private float motorForce = 2000f;
    [SerializeField] private float brakeForce = 3000f;
    [SerializeField] private float maxSteerAngle = 30f;
    [SerializeField] private float idleSpeed = 5f;
    [SerializeField] private float backwardSpeedMultiplier = 1.3f;

    private float throttleInput;
    private float brakeInput;
    private float steeringInput;
    private bool isMovingForward = true;
    public bool isStopped = false;
    public bool inCityMode = false;

    private void FixedUpdate()
    {
        UpdateCarState();

        if (!isStopped)
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

    private void UpdateCarState()
    {
        isStopped = ld.stop;
        isMovingForward = !isStopped && ld.fd;
        inCityMode = isMovingForward;
    }

    private void GetInput()
    {
        // Throttle and sound
        throttleInput = throttleButton.GetAxis(SteamVR_Input_Sources.RightHand);
        idleMotorSound.pitch = 1 + throttleInput / 3;
        idleMotorSound.volume = 0.5f + throttleInput / 2;

        // Brake and steering
        brakeInput = brakeButton.GetAxis(SteamVR_Input_Sources.LeftHand);
        steeringInput = steeringWheel.outAngle / 540;
    }

    private void HandleMotor()
    {
        float speed = throttleInput > 0 ? throttleInput * motorForce : idleSpeed;
        float direction = isMovingForward ? -1 : backwardSpeedMultiplier;

        float torque = speed * direction;

        ApplyTorqueToAllWheels(torque);
    }

    private void ApplyTorqueToAllWheels(float torque)
    {
        frontLeftWheelCollider.motorTorque = torque;
        frontRightWheelCollider.motorTorque = torque;
        rearLeftWheelCollider.motorTorque = torque;
        rearRightWheelCollider.motorTorque = torque;
    }

    private void HandleBrake()
    {
        float brakeTorque = brakeInput * brakeForce;
        ApplyBrakeToAllWheels(brakeTorque);
    }

    private void ApplyBrakeToAllWheels(float brakeTorque)
    {
        frontLeftWheelCollider.brakeTorque = brakeTorque;
        frontRightWheelCollider.brakeTorque = brakeTorque;
        rearLeftWheelCollider.brakeTorque = brakeTorque;
        rearRightWheelCollider.brakeTorque = brakeTorque;
    }

    private void StopVehicle()
    {
        ApplyTorqueToAllWheels(0);
        ApplyBrakeToAllWheels(brakeForce);
    }

    private void HandleSteering()
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

    private void UpdateWheels()
    {
        UpdateWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateWheel(rearRightWheelCollider, rearRightWheelTransform);
    }

    private void UpdateWheel(WheelCollider collider, Transform wheelTransform)
    {
        collider.GetWorldPose(out Vector3 position, out Quaternion rotation);
        wheelTransform.position = position;
        wheelTransform.rotation = rotation;
    }
}