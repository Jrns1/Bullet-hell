using UnityEngine;

public class CameraController : Singleton<CameraController>
{

    #region Variables
    public Transform target;

    public float smoothTime;
    public float zoomDistance;
    public bool enableZoom = true;

    private const float IGNORED_SHAKING = 0.0005f;
    public Vector3 offset = new Vector3(0, 0, -300);

    Vector3 velocity;

    Vector3 actualPosition;
    
    // shake
    Vector3 shakingOffset;
    float actualMagnitude = 0f;
    float initialMagnitude = 0f;
    float goalTime; 
    float shakingLast = 0f;

    // limit
    float minX;
    float maxX;
    float minY;
    float maxY;
    #endregion

    private void Awake()
    {
        Camera.main.orthographicSize = Screen.currentResolution.height / 2 / 64;
        actualPosition = transform.position;
    }

    void LateUpdate()
    {
        // Limit position
        Vector3 desiredPosition = target.position + offset;
        if (enableZoom)
            desiredPosition += (Camera.main.ScreenToWorldPoint(Input.mousePosition) - target.position).normalized * zoomDistance;

        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);
        
        actualPosition = Vector3.SmoothDamp(actualPosition, desiredPosition, ref velocity, smoothTime);
        
        // Shake camera
        if (actualMagnitude > IGNORED_SHAKING)
        {
            shakingOffset = Random.insideUnitCircle * actualMagnitude;

            if (shakingLast > 0)
                shakingLast -= Time.deltaTime;
            else
                actualMagnitude -= initialMagnitude * Time.deltaTime / goalTime;
        }

        Vector3 tempPosition = actualPosition + shakingOffset;
        tempPosition.z = tempPosition.z < -.5f ? tempPosition.z : -.5f;
        transform.position = tempPosition;
    }

    public void Shake(float magnitude, float time, float _shakingLast = 0f)
    {
        shakingOffset = Vector3.zero;
        initialMagnitude = magnitude;
        actualMagnitude = magnitude;
        goalTime = time;
        shakingLast = _shakingLast;
    }

    public void SetLimit(Vector2 topRight, Vector2 bottomLeft)
    {
        float halfHeight = Camera.main.orthographicSize;
        float halfWidth = halfHeight * Screen.width / Screen.height;
        maxX = topRight.x - halfWidth;
        minX = bottomLeft.x + halfWidth;
        maxY = topRight.y - halfHeight;
        minY = bottomLeft.y + halfHeight;

        if (maxX < minX)
        {
            float middle = (topRight.x + bottomLeft.x) / 2;
            maxX = middle;
            minX = middle;
        }

        if (maxY < minY)
        {
            float middle = (topRight.y + bottomLeft.y) / 2;
            maxY = middle;
            minY = middle;
        }
    }
}