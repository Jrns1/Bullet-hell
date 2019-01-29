using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {

    public float runningSpeed;
    public float dashSpeed;
    public float dashDst;

    const float linearDrag = 10f;

    Rigidbody2D rb2d;
    new BoxCollider2D collider;
    Camera cam;

    Vector2 input;
    bool isDashing;


    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();

        cam = Camera.main;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update () {
        if (isDashing)
            return;

        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetMouseButtonDown(1) && input != Vector2.zero)
        {
            StartCoroutine(Dodge());
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Ins.isActionInhibited)
            return;

        rb2d.MovePosition(rb2d.position + input * (isDashing ? dashSpeed : runningSpeed) * Time.deltaTime);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Equals("Primary Loading"))
            return;

        cam = Camera.main;
        cam.transform.position = transform.position - Vector3.forward * 10;

        CameraController.Ins.target = transform;

        MapManager.Ins.InitScene();

        GameManager.Ins.isActionInhibited = false;
    }

    IEnumerator Dodge()
    {
        isDashing = true;
        collider.enabled = false;

        yield return new WaitForSeconds(dashDst / dashSpeed);
        Vector2 tmp = transform.position;

        collider.enabled = true;

        //Vector2 drag = new Vector2(input.x, input.y) * linearDrag;
        //while (input.sqrMagnitude > .1f)
        //{
        //    input -= drag * Time.deltaTime;
        //    yield return null;
        //}
        //input = Vector2.zero;

        isDashing = false;
    }
}
