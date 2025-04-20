using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    [RequireComponent(typeof(Interactable))]
    public class LinearDrive : MonoBehaviour
    {
        [Header("Car Control")]
        public CarController carController;
        
        [Header("Position Settings")]
        public Transform startPosition;
        public Transform endPosition;
        
        [Header("Mapping Settings")]
        public LinearMapping linearMapping;
        public bool repositionGameObject = true;
        public bool maintainMomentum = true;
        [Range(1f, 10f)] public float momentumDampenRate = 5.0f;

        // Control zones (normalized 0-1)
        private const float FORWARD_ZONE = 0.25f;
        private const float REVERSE_ZONE = 0.75f;

        // Protected variables
        protected Hand.AttachmentFlags attachmentFlags = Hand.AttachmentFlags.DetachFromOtherHand;
        protected float initialMappingOffset;
        protected float[] mappingChangeSamples;
        protected float prevMapping;
        protected float mappingChangeRate;
        protected int sampleCount;
        protected Interactable interactable;
        private const int SAMPLE_COUNT = 5;

        protected virtual void Awake()
        {
            interactable = GetComponent<Interactable>();
            mappingChangeSamples = new float[SAMPLE_COUNT];
        }

        protected virtual void Start()
        {
            InitializeLinearMapping();
            initialMappingOffset = linearMapping.value;
            
            if (repositionGameObject)
                UpdatePositionAndControl();
        }

        private void InitializeLinearMapping()
        {
            if (linearMapping == null)
                linearMapping = GetComponent<LinearMapping>();
            
            if (linearMapping == null)
                linearMapping = gameObject.AddComponent<LinearMapping>();
        }

        protected virtual void HandHoverUpdate(Hand hand)
        {
            if (interactable.attachedToHand == null && hand.GetGrabStarting() != GrabTypes.None)
            {
                initialMappingOffset = linearMapping.value - CalculateLinearMapping(hand.transform);
                sampleCount = 0;
                mappingChangeRate = 0f;
                hand.AttachObject(gameObject, hand.GetGrabStarting(), attachmentFlags);
            }
        }

        protected virtual void HandAttachedUpdate(Hand hand)
        {
            UpdateLinearMapping(hand.transform);
            
            if (hand.IsGrabEnding(gameObject))
                hand.DetachObject(gameObject);
        }

        protected virtual void OnDetachedFromHand(Hand hand)
        {
            CalculateMappingChangeRate();
        }

        protected void UpdateLinearMapping(Transform updateTransform)
        {
            prevMapping = linearMapping.value;
            linearMapping.value = Mathf.Clamp01(initialMappingOffset + CalculateLinearMapping(updateTransform));

            mappingChangeSamples[sampleCount % SAMPLE_COUNT] = (1.0f / Time.deltaTime) * (linearMapping.value - prevMapping);
            sampleCount++;

            UpdatePositionAndControl();
        }

        private void UpdatePositionAndControl()
        {
            if (!repositionGameObject && carController == null)
                return;

            float mappedValue = linearMapping.value;
            UpdatePosition(mappedValue);
            UpdateCarControl(mappedValue);
        }

        private void UpdatePosition(float mappedValue)
        {
            if (!repositionGameObject) return;

            if (mappedValue < FORWARD_ZONE)
            {
                transform.position = Vector3.Lerp(startPosition.position, endPosition.position, mappedValue * 4f);
            }
            else if (mappedValue >= REVERSE_ZONE)
            {
                transform.position = Vector3.Lerp(endPosition.position, startPosition.position, (mappedValue - 0.75f) * 4f);
            }
        }

        private void UpdateCarControl(float mappedValue)
        {
            if (carController != null)
            {
                carController.fd = mappedValue < FORWARD_ZONE;
            }
        }

        protected float CalculateLinearMapping(Transform updateTransform)
        {
            Vector3 direction = endPosition.position - startPosition.position;
            float length = direction.magnitude;
            direction.Normalize();
            
            Vector3 displacement = updateTransform.position - startPosition.position;
            return Vector3.Dot(displacement, direction) / length;
        }

        protected void CalculateMappingChangeRate()
        {
            mappingChangeRate = 0f;
            int validSamples = Mathf.Min(sampleCount, SAMPLE_COUNT);
            
            for (int i = 0; i < validSamples; i++)
                mappingChangeRate += mappingChangeSamples[i];
            
            if (validSamples > 0)
                mappingChangeRate /= validSamples;
        }

        protected virtual void Update()
        {
            if (maintainMomentum && Mathf.Abs(mappingChangeRate) > 0.001f)
            {
                mappingChangeRate = Mathf.Lerp(mappingChangeRate, 0f, momentumDampenRate * Time.deltaTime);
                linearMapping.value = Mathf.Clamp01(linearMapping.value + (mappingChangeRate * Time.deltaTime));
                
                UpdatePositionAndControl();
            }
        }
    }
}