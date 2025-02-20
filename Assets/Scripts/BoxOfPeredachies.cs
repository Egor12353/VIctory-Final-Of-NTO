using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class BoxOfPeredachies : MonoBehaviour
{
    public Transform gearLever; // Ссылка на объект коробки передач
    public Vector3[] gearPositions; // Позиции для каждой передачи
    private EnterInCar enterCar;
    public float switchDelay = 0.5f; // Задержка перед переключением
    public int currentGear = 0; // Текущая передача
    private Vector3 initialPosition; // Начальная позиция рычага
    private bool isSwitching = false;

    [SerializeField]
    private SteamVR_Action_Boolean forwardperedacha; // Кнопка для увеличения передачи

    [SerializeField]
    private SteamVR_Action_Boolean backperedacha; // Кнопка для уменьшения передачи

    private void Start()
    {
        gearLever.position = gearPositions[0];
        initialPosition = gearLever.position;
    }

    private void Update()
    {
        // Проверяем, нажата ли кнопка для увеличения передачи
        if (forwardperedacha.GetStateDown(SteamVR_Input_Sources.RightHand) && enterCar.inDrive == true )
        {
            if (!isSwitching)
            {
                print("clic");
                StartCoroutine(SwitchGear(1)); // Увеличиваем передачу
            }
        }

        // Проверяем, нажата ли кнопка для уменьшения передачи
        if (backperedacha.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            if (!isSwitching)
            {
                print("clic");
                StartCoroutine(SwitchGear(-1)); // Уменьшаем передачу
            }
        }
        
    }

    private IEnumerator SwitchGear(int direction)
    {
        isSwitching = true;

        // Изменяем текущую передачу в зависимости от направления
        currentGear = (currentGear + direction) % gearPositions.Length;

        // Обрабатываем случай, когда передача становится отрицательной
        if (currentGear < 0)
        {
            currentGear -= 1; // Устанавливаем на последнюю передачу
        }

        // Позиция для переключения
        Vector3 targetPosition = gearPositions[currentGear];

        // Перемещение рычага к новой позиции
        float elapsedTime = 0f;
        while (elapsedTime < switchDelay)
        {
            gearLever.position = Vector3.Lerp(initialPosition, targetPosition, (elapsedTime / switchDelay));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Устанавливаем окончательную позицию
        gearLever.position = targetPosition;

        // Возвращаем рычаг в начальную позицию
        elapsedTime = 0f;
        while (elapsedTime < switchDelay)
        {
            gearLever.position = Vector3.Lerp(targetPosition, initialPosition, (elapsedTime / switchDelay));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gearLever.position = initialPosition;

        isSwitching = false;
    }
}
