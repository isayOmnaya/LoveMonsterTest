
using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("Monster Velocity")]
    [SerializeField]
    float _maxVelocity = 0.0f;

    [SerializeField]
    float _minVelocity = 0.0f;
    
    float _targetSpeed;

    [SerializeField]
    float _timeToDestroy = 0.0f;

    float _initialTimeToDestroy = 0f;

    float _elapsedTime = 0.0f;
    
    public void Initialize()
    {
        _targetSpeed = Random.Range(_minVelocity, _maxVelocity);
        _initialTimeToDestroy = _timeToDestroy;
    }

    public void UpdateExternal()
    {
        Move();
        CheckOutOfBounds();
    }

    void Move()
    {
        transform.Translate(Vector2.right * _targetSpeed * Time.deltaTime);
    }

    void CheckOutOfBounds()
    {
        if(!IsVisibleOnScreen())
        {
            _elapsedTime += Time.deltaTime;

            if(_elapsedTime >= _timeToDestroy)
            {
                gameObject.SetActive(false);
                _elapsedTime = 0f;
            }
        }
        else
        {
            _timeToDestroy = _initialTimeToDestroy;
        }
    }

    public bool IsVisibleOnScreen()
    {
        // Get the screen dimensions
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        float cameraSize = Camera.main.orthographicSize;

        // Adjust aspect ratio based on screen orientation
        float cameraAspect = Screen.width > Screen.height
            ? (float)Screen.width / Screen.height
            : (float)Screen.height / Screen.width;

        // Convert object's position to screen coordinates
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        // Check if the screen position is within the visible area
        return screenPos.x > 0 && screenPos.x < screenWidth &&
            screenPos.y > 0 && screenPos.y < screenHeight &&
            screenPos.z > 0 && screenPos.z < cameraSize * 2 * cameraAspect;
    } 
}
