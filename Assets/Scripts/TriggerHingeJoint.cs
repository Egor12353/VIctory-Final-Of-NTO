using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruzovikScepka : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb; // Rigidbody для текущего объекта
    [SerializeField]
    private Rigidbody scep; // Rigidbody для сцепляемого объекта

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Scep"))
        {
            // Получаем Rigidbody объекта, который вошел в триггер
            Rigidbody scepRigidbody = other.GetComponent<Rigidbody>();
            if (scepRigidbody != null)
            {
                // Создаем SpringJoint
                SpringJoint springJoint = gameObject.AddComponent<SpringJoint>();

                // Устанавливаем параметры SpringJoint
                springJoint.connectedBody = scepRigidbody; // Соединяем с Rigidbody объекта
                springJoint.anchor = rb.transform.InverseTransformPoint(rb.position); // Устанавливаем анкор
                springJoint.spring = 5f; // Настройка пружины
                springJoint.damper = 2f; // Настройка амортизатора
                springJoint.maxDistance = 0.5f; // Максимальное расстояние между объектами
            }
        }
    }
}
