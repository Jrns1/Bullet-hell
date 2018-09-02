using UnityEngine;

public class CameraController : Singleton<CameraController>
{

    #region Variables
    public GameObject traget;

    public float smoothTime;
    public float zoomDistance;
    
    private const float ignoreShakingMagnitude = 0.0005f;
    public Vector3 offset = new Vector3(0, 0, -10);

    Vector3 velocity;

    Vector3 originPosition;
    
    // shake
    Vector3 shakingOffset;
    float shakingMagnitude = 0f;
    float originalMagnitude = 0f;
    float goalTime;
    float shakingLast = 0f;
    float angle;

    // limit
    float minX;
    float maxX;
    float minY;
    float maxY;
    #endregion

    private void Awake()
    {
        Camera.main.orthographicSize = Screen.currentResolution.height / 2 / 64;
        originPosition = transform.position;
    }

    void LateUpdate()
    {
        // Limit position
        Vector3 desiredPosition = traget.transform.position + offset + (Camera.main.ScreenToWorldPoint(Input.mousePosition) - traget.transform.position).normalized * zoomDistance;

        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);
        
        originPosition = Vector3.SmoothDamp(originPosition, desiredPosition, ref velocity, smoothTime);
        
        // Shake camera
        if (shakingMagnitude > ignoreShakingMagnitude)
        {
            angle += Random.Range(-200, 200);
            shakingOffset = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad) * shakingMagnitude, Mathf.Sin(angle * Mathf.Deg2Rad) * shakingMagnitude);

            if (shakingLast > 0)
                shakingLast -= Time.deltaTime;
            else
                shakingMagnitude -= originalMagnitude * Time.fixedDeltaTime / goalTime;
        }

        transform.position = originPosition + shakingOffset;
    }

    public void Shake(float magnitude, float time, float _shakingLast = 0f)
    {
        shakingOffset = Vector3.zero;
        originalMagnitude = magnitude;
        shakingMagnitude = magnitude;
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