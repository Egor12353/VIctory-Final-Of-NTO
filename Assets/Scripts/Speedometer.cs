using System.Runtime.ExceptionServices;
using UnityEngine;

public class Speedometer : MonoBehaviour
{
    [Header("References")]
    public Transform needle; // Объект стрелки спидометра
    public CarController carController; // Ссылка на скрипт управления машиной

    [Header("Settings")]
    public float maxSpeedKPH = 45f; // Максимальная скорость в км/ч
    public float minNeedleAngle = -90f; // Угол стрелки при 0 км/ч
    public float maxNeedleAngle = 90f; // Угол стрелки при максимальной скорости
    public float smoothingSpeed = 5f; // Скорость плавности движения стрелки
    public bool isBus;

    private float currentSpeed;
    private float currentNeedleAngle;

    void Update()
    {
        // Получаем текущую скорость из скрипта машины
        currentSpeed = carController.GetCurrentSpeedKPH();

        // Рассчитываем целевой угол для стрелки
        float targetAngle = Mathf.Lerp(minNeedleAngle, maxNeedleAngle, currentSpeed / maxSpeedKPH);

        // Плавно интерполируем угол стрелки
        currentNeedleAngle = Mathf.LerpAngle(currentNeedleAngle, targetAngle, smoothingSpeed * Time.deltaTime);

        // Применяем вращение к стрелке
        if (isBus == true)
        {
            needle.localEulerAngles = new Vector3(currentNeedleAngle, 0, 0);
        }
        else
        {
            needle.localEulerAngles = new Vector3(currentNeedleAngle, 0, 0);
        }
    }
}