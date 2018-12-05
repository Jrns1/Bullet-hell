using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {

    public float runningSpeed;
    public float walkingSpeed;
    public bool isAllowedToMove;

    ContactFilter2D contactFilter;

    Rigidbody2D rb2d;
    Camera cam;

    float speed;
    Vector2 input;

    private void Awake()
    {
        speed = runningSpeed;

        rb2d = GetComponent<Rigidbody2D>();

        contactFilter = new ContactFilter2D();
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(1 << LayerMask.NameToLayer("Wall"));

        cam = Camera.main;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update () {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetMouseButtonDown(1))
        {
            if (speed == runningSpeed)
                speed = walkingSpeed;
            else
                speed = runningSpeed;

            //Teleport();
        }
    }

    private void FixedUpdate()
    {
        if (!isAllowedToMove)
            return;

        rb2d.MovePosition(rb2d.position + input * speed * Time.deltaTime);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        cam = Camera.main;
        cam.transform.position = transform.position - Vector3.forward * 10;

        CameraController cc = cam.GetComponent<CameraController>();
        if (cc)
            cc.target = transform;

        MapManager mm = MapManager.Instance;
        if (mm)
            mm.InitScene();

        isAllowedToMove = true;
        transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;
    }
}
