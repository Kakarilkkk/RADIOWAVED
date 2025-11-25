using Unity.VisualScripting;
using UnityEngine;

public class RoundController : MonoBehaviour
{
    public GameObject secondObject;

    public int value;

    public float minAngle = -90f;    // Минимальный угол
    public float maxAngle = 90f;     // Максимальный угол
    public float smoothSpeed = 5f;   // Скорость сглаживания поворота
    public float movementRange = 5f; // Диапазон движения второго объекта по X
    public float ofset = 3f;

    public int sections = 7;

    private Camera cam;
    private bool isDragging = false; // Флаг, что мы перетаскиваем

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (isDragging)
        {
            // Получаем позицию мыши в мировых координатах
            Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);

            // Вертикальная разница
            float dy = mouseWorld.y - transform.position.y;

            // Нормализация от -1 до 1
            float t = Mathf.Clamp(dy, -1f, 1f);

            // Таргет угол
            float targetAngle = Mathf.Lerp(minAngle, maxAngle, (t + 1f) / 2f);

            // Плавный поворот
            float newAngle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, Time.deltaTime * smoothSpeed);

            // Применяем поворот
            transform.rotation = Quaternion.Euler(0, 0, newAngle);

            // Движение второго объекта по оси X в зависимости от угла
            float signedAngle = transform.eulerAngles.z;
            if (signedAngle > 180f)
                signedAngle -= 360f;

            // Нормализация угла в диапазон 0..1 для Lerp
            float angle01 = Mathf.InverseLerp(minAngle, maxAngle, signedAngle);

            // Двигаем второй объект
            float x = Mathf.Lerp(-movementRange + ofset, movementRange + ofset, angle01);

            secondObject.transform.position = new Vector3(
                x,
                secondObject.transform.position.y,
                secondObject.transform.position.z
            );
        }

        // Debug.DrawLine(
        // new Vector3(movementRange + ofset, transform.position.y, transform.position.z), 
        // new Vector3(-movementRange + ofset, transform.position.y, transform.position.z), 
        // Color.cyan
        // );
    }

    // Этот метод срабатывает, когда мышь нажата на объект
    void OnMouseDown()
    {
        isDragging = true; // Начинаем перетаскивать
    }

    // Этот метод срабатывает, когда мышь отпускается
    void OnMouseUp()
    {
        isDragging = false; // Останавливаем перетаскивание

        // Примагничиваем угол к ближайшему значению int
        float signedAngle = transform.eulerAngles.z;
        if (signedAngle > 180f)
            signedAngle -= 360f;

        int magnetizedValue = AngleToInt(signedAngle); // Получаем значение int
        float targetAngle = Mathf.Lerp(minAngle, maxAngle, magnetizedValue / sections); // Приводим к углу

        // Примагничиваем объект к этому углу
        transform.rotation = Quaternion.Euler(0, 0, targetAngle);

        Debug.Log(magnetizedValue);
        value = magnetizedValue;
    }

    // Преобразование угла в целое число
    int AngleToInt(float angle)
    {
        // Преобразуем угол в диапазон 0..1
        float t = Mathf.InverseLerp(minAngle, maxAngle, angle);

        // Лерпим в диапазон 0..5 и округляем до целого
        int value = Mathf.RoundToInt(Mathf.Lerp(0, sections, t));

        return value;
    }
}
