using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levers : MonoBehaviour
{
    public Transform topOfJoystick;  // Объект, который вы перемещаете (джойстик)
    public Transform rotatingSphere;  // Сфера, которую нужно вращать

    private Vector3 previousPosition; // Хранение предыдущей позиции

    void Start()
    {
        // Инициализируем предыдущую позицию
        previousPosition = topOfJoystick.position;
    }

    void Update()
    {
        // Вычисляем смещение позиции джойстика
        Vector3 joystickOffset = topOfJoystick.position - previousPosition;

        // Обновляем предыдущую позицию
        previousPosition = topOfJoystick.position;

        // Обновляем вращение сферы в зависимости от перемещения джойстика
        UpdateSphereRotation(joystickOffset);
    }

    private void UpdateSphereRotation(Vector3 joystickOffset)
    {
        // Получаем текущее вращение сферы
        Vector3 currentRotation = rotatingSphere.rotation.eulerAngles;

        // Обновляем угол по оси X в зависимости от смещения по Y
        // Умножаем на 100 для увеличения чувствительности, можете настроить под себя
        float rotationChange = joystickOffset.y * 100; 
        Vector3 newRotation = new Vector3(currentRotation.x + rotationChange, currentRotation.y, currentRotation.z);

        // Применяем новое вращение к сфере
        rotatingSphere.rotation = Quaternion.Euler(newRotation);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            print("Hand detected!");
            transform.LookAt(other.transform.position, transform.up);
        }
    }
}