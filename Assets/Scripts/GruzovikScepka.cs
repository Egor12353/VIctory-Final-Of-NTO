using UnityEngine;

public class TriggerHingeJoint : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToJoin; // Объект, к которому будет присоединяться HingeJoint
    [SerializeField]
    private GameObject objectIsJoin;
    [SerializeField]
    private float massScale = 40f; // Масса для HingeJoint

    [SerializeField]
    private GameObject trail;
    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что триггер сработал для нужного объекта
        if (other.CompareTag("Trailer"))
        {
            // Снимаем isKinematic у Rigidbody объекта, к которому присоединяем
            Rigidbody objectToJoinRigidbody = objectToJoin.GetComponent<Rigidbody>();
            Rigidbody obj = objectIsJoin.GetComponent<Rigidbody>();
            Rigidbody Trailer = trail.GetComponent<Rigidbody>();
            if (Trailer != null)
            {
                Trailer.isKinematic = false; // Снимаем кинематику
            }

            // Добавляем HingeJoint к объекту, к которому присоединяем
            HingeJoint hingeJoint = objectToJoin.AddComponent<HingeJoint>();

            // Настройка параметров HingeJoint
            hingeJoint.connectedBody = other.GetComponent<Rigidbody>(); // Соединяем с Rigidbody объекта, который вошел в триггер
            hingeJoint.axis = Vector3.up; // Устанавливаем ось вращения
            hingeJoint.anchor = Vector3.zero; // Устанавливаем анкор в центр

            // Установка mass scale
            hingeJoint.massScale = massScale;
        }
    }
}
