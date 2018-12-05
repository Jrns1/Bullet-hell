using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorChanger : MonoBehaviour {

    public Texture2D image;

    bool isChanged;

    public void ToggleCursor()
    {
        if (isChanged)
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        else
            Cursor.SetCursor(image, Vector2.zero, CursorMode.Auto);

        isChanged = !isChanged;
    }

}
