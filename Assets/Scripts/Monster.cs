using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("Monster Velocity")]
    [SerializeField]
    float _maxVelocity = 0.0f;

    [SerializeField]
    float _minVelocity = 0.0f;
    float _velocity;

    [SerializeField]
    float _timeToDestroy = 0.0f;

    float _elapsedTime = 0.0f;
    
    public void Initialize()
    {
        _velocity = Random.Range(_minVelocity, _maxVelocity);
    }

    public void UpdateExternal()
    {
        Move();
        CheckOutOfBounds();
    }

    private void Move()
    {
        transform.Translate(Vector2.right * _velocity * Time.deltaTime);
    }

    private void CheckOutOfBounds()
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
    }

    public bool IsVisibleOnScreen()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;


        float cameraSize = Camera.main.orthographicSize;
        float cameraAspect = Camera.main.aspect;

       
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        // Check if the screen position is within the visible area
        return screenPos.x > 0 && screenPos.x < screenWidth &&
            screenPos.y > 0 && screenPos.y < screenHeight &&
            screenPos.z > 0 && screenPos.z < cameraSize * 2 * cameraAspect;
    } 
}
