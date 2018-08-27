using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {

    public float runningSpeed;
    public float walkingSpeed;
    public bool isMoving;

    float speed;

    ContactFilter2D contactFilter;
    Collider2D[] buffer;


    Rigidbody2D rb2d;
    new BoxCollider2D collider;
    Camera cam;


    private void Awake()
    {
        speed = runningSpeed;

        rb2d = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        buffer = new Collider2D[8];

        contactFilter = new ContactFilter2D();
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(1 << LayerMask.NameToLayer("Wall"));

        cam = Camera.main;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        cam = Camera.main;
        CameraController cc = cam.GetComponent<CameraController>();
        if (cc)
            cc.traget = gameObject;
        MapManager mm = MapManager.Instance;
        if (mm)
            transform.position = mm.GetScenePosAndSomeFunc(GameManager.Instance.sceneEntryPortalName);
        isMoving = true;
        collider.enabled = true;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    void Update () {
        if (!isMoving)
            return;
        
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb2d.MovePosition(rb2d.position + input * speed * Time.deltaTime);

        if (Input.GetMouseButtonDown(1))
        {
            if (speed == runningSpeed)
                speed = walkingSpeed;
            else
                speed = runningSpeed;

            //Teleport();
        }
    }

    void Teleport()
    {
        Vector2 desiredPos = cam.ScreenToWorldPoint(Input.mousePosition);
        
        if (Physics2D.OverlapBox(desiredPos, collider.size, 0, contactFilter, buffer) > 0)
            return;
        
        rb2d.position = desiredPos;
    }
}
