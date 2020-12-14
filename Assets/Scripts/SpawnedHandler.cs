using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedHandler : MonoBehaviour, ITapped, ISwipped, IDragged, IPinchSpread, IRotated
{
    public Vector3 targetPosition = Vector3.zero;
    public float speed = 10;
    public float scaleSpeed = 1f;
    public float rotateSpeed = 1f;

    private void OnEnable()
    {
        targetPosition = transform.position;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    public void OnTap()
    {
        Debug.Log($"Hit: {gameObject.name}");
        Destroy(gameObject);
    }

    public void OnSwipe(OnSwipeEventArg args)
    {
        Vector3 dir = Vector3.zero;

        dir = args.SwipeVector.normalized;

        targetPosition += (dir * 5);
    }

    public void OnDrag(OnDragEventArg args)
    {
        if (args.HitObject == gameObject)
        {
            Ray r = Camera.main.ScreenPointToRay(args.TargetFinger.position);
            Vector3 worldPoint = r.GetPoint(10);

            targetPosition = worldPoint;
            transform.position = worldPoint;
        }
    }

    public void OnPinchSpread(OnPinchSpreadEventArg args)
    {
        if (args.HitObject == gameObject)
        {
            float scale = (args.DistanceDifference / Screen.dpi) * scaleSpeed;
            Vector3 scaleVector = new Vector3(scale, scale, scale);

            transform.localScale += scaleVector;
        }
    }

    public void OnRotate(OnRotateEventArg args)
    {
        float angle = args.Angle * rotateSpeed;

        if (args.RotationDirection == RotationDirections.CW)
        {
            angle *= -1;
        }

        Vector3 rot = new Vector3(0, 0, angle);
        transform.Rotate(rot);
    }
}
