using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public RectTransform screen;
    public RectTransform playableScreen;

    private float widthEdge = 0;
    private float heightEdge = 0;

    // Start is called before the first frame update
    void Start()
    {
        GestureManager.instance.OnDrag += OnDrag;
    }

    private void OnDisable()
    {
        GestureManager.instance.OnDrag -= OnDrag;

    }

    private void OnDrag(object sender, OnDragEventArg e)
    {
        Vector2 delta = e.TargetFinger.deltaPosition;

        delta = delta / Screen.dpi;

        Vector3 change = (Vector3)delta * Player.instance.stats.cameraSpeed;

        transform.position += change;

        widthEdge = (playableScreen.rect.width * playableScreen.lossyScale.x - screen.rect.width * screen.lossyScale.x) / 2;
        heightEdge = (playableScreen.rect.height * playableScreen.lossyScale.y - screen.rect.height * screen.lossyScale.y) / 2;

        float width = Mathf.Clamp(transform.position.x, -widthEdge, widthEdge);
        float height = Mathf.Clamp(transform.position.y, -heightEdge, heightEdge);

        transform.position = new Vector3(width, height, transform.position.z);
    }
}
