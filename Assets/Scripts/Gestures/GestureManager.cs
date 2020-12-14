using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GestureManager : MonoBehaviour
{
    #region Singleton
    public static GestureManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    Touch trackedFinger1;
    Touch trackedFinger2;

    public event EventHandler<OnTapEventArg> OnTap;
    public event EventHandler<OnSwipeEventArg> OnSwipe;
    public event EventHandler<OnDragEventArg> OnDrag;
    public event EventHandler<OnTwoFingerPanEventArg> OnTwoFingerPan;
    public event EventHandler<OnPinchSpreadEventArg> OnPinchSpread;
    public event EventHandler<OnRotateEventArg> OnRotate;

    public TapProperty _tapProperty;
    public SwipeProperty _swipeProperty;
    public DragProperty _dragProperty;
    public TwoFingerPanProperty _twoFingerPanProperty;
    public PinchSpreadProperty _pinchSpreadProperty;
    public RotateProperty _rotateProperty;

    private Vector2 startPoint = Vector2.zero;
    private Vector2 endPoint = Vector2.zero;

    private float gestureTime = 0;

    #region Unity In-Built Functions
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ApplicationManager.instance.isApplicationPause)
        {
            return;
        }
        if (Input.touchCount > 0)
        {
            if (Input.touchCount == 1)
            {
                CheckSingleFingerGesture();
            }
            else
            {
                CheckDoubleFingerGesture();
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (Input.touchCount > 0)
        {
            Ray r = Camera.main.ScreenPointToRay(trackedFinger1.position);
            Gizmos.DrawIcon(r.GetPoint(5), "NightmareChibi");
            if (Input.touchCount > 1)
            {
                r = Camera.main.ScreenPointToRay(trackedFinger2.position);
                Gizmos.DrawIcon(r.GetPoint(5), "MousseChibi");
            }
        }
    }
    #endregion

    private GameObject GetHit(Vector2 screenPos)
    {
        Ray r = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit = new RaycastHit();
        GameObject hitObj = null;

        if (Physics.Raycast(r, out hit, Mathf.Infinity))
        {
            hitObj = hit.collider.gameObject;
        }

        return hitObj;
    }

    private Vector2 GetPreviousPoint(Touch t)
    {
        return t.position - t.deltaPosition;
    }

    private Vector2 GetMidPoint(Vector2 p1, Vector2 p2)
    {
        Vector2 midPoint = new Vector2((p1.x + p2.x) / 2, (p1.y + p2.y) / 2);
        return midPoint;
    }

    #region Gestures

    private void CheckSingleFingerGesture()
    {
        trackedFinger1 = Input.GetTouch(0);

        if (trackedFinger1.phase == TouchPhase.Began)
        {
            startPoint = trackedFinger1.position;
            gestureTime = 0;
        }

        if (trackedFinger1.phase == TouchPhase.Ended)
        {
            endPoint = trackedFinger1.position;

            if (gestureTime <= _tapProperty.tapTime &&
                    Vector2.Distance(startPoint, endPoint) < (Screen.dpi * _tapProperty.tapMaxDistance))
            {
                FireTapEvent(endPoint);
                return;
            }

            if (gestureTime <= _swipeProperty.swipeTime &&
                    (Vector2.Distance(startPoint, endPoint) >= (_swipeProperty.minSwipeDistance * Screen.dpi)))
            {
                FireSwipeEvent();
                return;
            }
        }
        else
        {
            gestureTime += Time.deltaTime;

            if (gestureTime >= _dragProperty.dragBufferTime)
            {
                FireDragEvent();
            }
        }
    }

    private void CheckDoubleFingerGesture()
    {
        trackedFinger1 = Input.GetTouch(0);
        trackedFinger2 = Input.GetTouch(1);

        if (trackedFinger1.phase == TouchPhase.Moved && trackedFinger2.phase == TouchPhase.Moved)
        {
            //2FingerPan
            if (Vector2.Distance(trackedFinger1.position, trackedFinger2.position) <= (_twoFingerPanProperty.maxDistance * Screen.dpi))
            {
                //check if fingers are going the same direction
                if (Vector2.Distance(trackedFinger1.deltaPosition, trackedFinger2.deltaPosition) < 2.5f)
                {
                    FireTwoFingerPanEvent();
                    return;
                }
            }

            Vector2 prevPoint1 = GetPreviousPoint(trackedFinger1);
            Vector2 prevPoint2 = GetPreviousPoint(trackedFinger2);

            //Pinch Spread
            float currentDistance = Vector2.Distance(trackedFinger1.position, trackedFinger2.position);
            float previousDistance = Vector2.Distance(prevPoint1, prevPoint2);

            if (Mathf.Abs(currentDistance - previousDistance) >= (_pinchSpreadProperty.minDistanceChange * Screen.dpi))
            {
                FirePinchSpreadEvent(currentDistance - previousDistance);
                return;
            }

            //Rotate
            Vector2 diff_vector = trackedFinger1.position - trackedFinger2.position;
            Vector2 prev_diff_vector = prevPoint1 - prevPoint2;

            float angle = Vector2.Angle(prev_diff_vector, diff_vector);

            if (angle >= _rotateProperty.minChange)
            {
                FireRotateEvent(prev_diff_vector, diff_vector, angle);
            }
        }
    }

    #endregion

    #region Fire Gesture Events
    private void FireTapEvent(Vector2 pos)
    {
        GameObject hitObj = GetHit(pos);
        //Debug.Log("Tap");
        if (OnTap != null)
        {
            OnTapEventArg tapArgs = new OnTapEventArg(pos, hitObj);
            OnTap(this, tapArgs);
        }

        if (hitObj != null)
        {
            ITapped handler = hitObj.GetComponent<ITapped>();
            if (handler != null)
            {
                handler.OnTap();
            }
        }
    }

    private void FireSwipeEvent()
    {
        //Debug.Log("Swipe");

        GameObject hitObj = GetHit(startPoint);

        Vector2 diff = endPoint - startPoint;
        Directions dir;
        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
        {
            if (diff.x > 0)
            {
                //Debug.Log("Swipe Right");
                dir = Directions.RIGHT;
            }
            else
            {
                //Debug.Log("Swipe Left");
                dir = Directions.LEFT;
            }
        }
        else
        {
            if (diff.y > 0)
            {
                //Debug.Log("Swipe Up");
                dir = Directions.UP;
            }
            else
            {
                //Debug.Log("Swipe Down");
                dir = Directions.DOWN;
            }
        }

        OnSwipeEventArg args = new OnSwipeEventArg(startPoint, diff, dir, hitObj);

        if (OnSwipe != null)
        {
            OnSwipe(this, args);
        }

        if (hitObj != null)
        {
            ISwipped iSwipe = hitObj.GetComponent<ISwipped>();
            if (iSwipe != null)
            {
                iSwipe.OnSwipe(args);
            }
        }
    }

    private void FireDragEvent()
    {
        //Debug.Log($"Dragging {trackedFinger1.position.ToString()}");
        GameObject hitObj = GetHit(trackedFinger1.position);

        OnDragEventArg args = new OnDragEventArg(trackedFinger1, hitObj);

        if (OnDrag != null)
        {
            OnDrag(this, args);
        }

        if (hitObj != null)
        {
            IDragged iDrag = hitObj.GetComponent<IDragged>();
            if (iDrag != null)
            {
                iDrag.OnDrag(args);
            }
        }
    }

    private void FireTwoFingerPanEvent()
    {
        //Debug.Log("2Finger Pan!");

        OnTwoFingerPanEventArg args = new OnTwoFingerPanEventArg(trackedFinger1, trackedFinger2);

        if (OnTwoFingerPan != null)
        {
            OnTwoFingerPan(this, args);
        }
    }

    private void FirePinchSpreadEvent(float dist_diff)
    {
        if (dist_diff > 0)
        {
            //Debug.Log("Spread");
        }
        else
        {
            //Debug.Log("Pinch");
        }

        Vector2 midPoint = GetMidPoint(trackedFinger1.position, trackedFinger2.position);
        GameObject hitObj = GetHit(midPoint);

        OnPinchSpreadEventArg args = new OnPinchSpreadEventArg(trackedFinger1, trackedFinger2, dist_diff, hitObj);

        if (OnPinchSpread != null)
        {
            OnPinchSpread(this, args);
        }

        if (hitObj != null)
        {
            IPinchSpread pinchSpread = hitObj.GetComponent<IPinchSpread>();
            if (pinchSpread != null)
            {
                pinchSpread.OnPinchSpread(args);
            }
        }
    }

    private void FireRotateEvent(Vector2 prev_diff_vector, Vector2 diff_vector, float angle)
    {
        Vector3 cross = Vector3.Cross(prev_diff_vector, diff_vector);
        RotationDirections dir = RotationDirections.CW;

        if (cross.z > 0)
        {
            dir = RotationDirections.CCW;
            //Debug.Log($"Rotate Counter CW {angle}");
        }
        else if (cross.z < 0)
        {
            dir = RotationDirections.CW;
            //Debug.Log($"Rotate CW {angle}");

        }

        Vector2 midPoint = GetMidPoint(trackedFinger1.position, trackedFinger2.position);
        GameObject hitObj = GetHit(midPoint);

        OnRotateEventArg args = new OnRotateEventArg(trackedFinger1, trackedFinger2, angle, dir, hitObj);

        if (OnRotate != null)
        {
            OnRotate(this, args);
        }

        if (hitObj != null)
        {
            IRotated rot = hitObj.GetComponent<IRotated>();
            if (rot != null)
            {
                rot.OnRotate(args);
            }
        }
    }
    #endregion
}
