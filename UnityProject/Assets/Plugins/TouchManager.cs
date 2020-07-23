using UnityEngine;
using System.Collections;

public delegate void TouchEvent(TouchItem item);
/// <summary>
/// TouchManager processes touches.  It can also simulate single touches when running in the Unity editor.
/// It provides utility functions for detecting Collider touches and rotating Ridigidbodies.
/// Touches will be automatically processed when TouchManager is attached to a GameObject.
/// </summary>
public class TouchManager : MonoBehaviour
{
	public static event TouchEvent tapEvent;
    public static event TouchEvent dragEvent;
	private const int MAX_TOUCHES = 5;
	
    // The last time a finger touched the screen
    private static TouchItem[] touchCache = new TouchItem[MAX_TOUCHES];

    private static int count = 0;

    private static bool mouseLeftButtonDown = false;

    private static TouchItem touchItem;

    /// <summary>
    /// When TouchManager is attached to a GameObject it will automatically process touches on every call to Update, Update is called once per frame.
    /// </summary>
    void Update ()
    {
        ProcessTouches ();
    }

    /// <summary>
    /// Gets the touch count.
    /// </summary>
    /// <value>
    /// The touch count.
    /// </value>
    public static int touchCount {
        get { return count; }
    }

    /// <summary>
    /// Determines whether touch is valid for the specified touch id.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this if the touch is valid for the specified id; otherwise, <c>false</c>.
    /// </returns>
    /// <param name='id'>
    /// Touch id.
    /// </param>
    public static bool IsTouchValid (int id)
    {
        bool retVal = false;
        
        if (id < touchCount) {
            retVal = true;
        }
        
        return retVal;
    }

    /// <summary>
    /// Gets the touch position.
    /// </summary>
    /// <returns>
    /// The screen location of the touch.
    /// </returns>
    /// <param name='id'>
    /// Touch id.
    /// </param>
    /// <param name='pos'>
    /// Set to screen location of touch.
    /// </param>
    public static bool GetTouchPos (int id, out Vector2 pos)
    {
        bool retVal = IsTouchValid (id);
        if (retVal == true) {
            pos = touchCache[id].position;
        } else {
            pos = Vector2.zero;
        }
        
        return retVal;
    }

    /// <summary>
    /// Gets the touch delta position.
    /// </summary>
    /// <returns>
    /// The touch delta position.
    /// </returns>
    /// <param name='id'>
    /// Touch id.
    /// </param>
    /// <param name='deltaPos'>
    /// Set to the touch delta position
    /// </param>
    public static bool GetTouchDeltaPos (int id, out Vector2 deltaPos)
    {
        bool retVal = IsTouchValid (id);
        
        if (retVal == true) {
            deltaPos = touchCache[id].deltaPosition;
        } else {
            deltaPos = Vector2.zero;
        }
        
        return retVal;
    }

    /// <summary>
    /// Gets the touch phase.
    /// </summary>
    /// <returns>
    /// The touch phase.
    /// </returns>
    /// <param name='id'>
    /// Touch id.
    /// </param>
    /// <param name='phase'>
    /// Set to the touch phase.
    /// </param>
    public static bool GetTouchPhase (int id, out TouchPhaseEnum phase)
    {
        phase = TouchPhaseEnum.CANCELED;
        
        bool retVal = IsTouchValid (id);
        
    //    Debug.Log("GetTouchPhase: id=" + id +"  IsTouchValid(id)=" + retVal);
        
        if (retVal == true) {
            phase = touchCache[id].phase;
        }
        
        return retVal;
    }

    /// <summary>
    /// Checks to see if a collider was touched
    /// </summary>
    /// <returns>
    /// True if the collider was touched.
    /// </returns>
    /// <param name='id'>
    /// Touch id.
    /// </param>
    /// <param name='collider'>
    /// Collider to check for touch.
    /// </param>
    public static bool TouchedCollider (int id, Collider collider)
    {
        return TouchedCollider (id, collider, Mathf.Infinity);
    }

    /// <summary>
    /// Checks to see if a collider was touched
    /// </summary>
    /// <returns>
    /// True if the collider was touched.
    /// </returns>
    /// <param name='id'>
    /// Touch id.
    /// </param>
    /// <param name='collider'>
    /// Collider to check for touch.
    /// </param>
    /// <param name='distance'>
    /// Distance away from the touch screen location to check for collider intersection
    /// </param>
    public static bool TouchedCollider (int id, Collider collider, float distance)
    {
        Vector2 screenPos;
        Vector3 worldPos;
        
        return TouchedCollider (id, collider, distance, out screenPos, out worldPos);
    }

    /// <summary>
    /// Checks to see if a collider was touched
    /// </summary>
    /// <returns>
    /// True if the collider was touched.
    /// </returns>
    /// <param name='id'>
    /// Touch id.
    /// </param>
    /// <param name='collider'>
    /// Collider to check for touch.
    /// </param>
    /// <param name='distance'>
    /// Distance away from the touch screen location to check for collider intersection
    /// </param>
    /// <param name='screenPos'>
    /// Set with the screen position of the touch.
    /// </param>
    public static bool TouchedCollider (int id, Collider collider, float distance, out Vector2 screenPos)
    {
        Vector3 worldPos;
        
        return TouchedCollider (id, collider, distance, out screenPos, out worldPos);
    }

    /// <summary>
    /// Checks to see if a collider was touched
    /// </summary>
    /// <returns>
    /// True if the collider was touched.
    /// </returns>
    /// <param name='id'>
    /// Touch id.
    /// </param>
    /// <param name='collider'>
    /// Collider to check for touch.
    /// </param>
    /// <param name='distance'>
    /// Distance away from the touch screen location to check for collider intersection
    /// </param>
    /// <param name='worldPos'>
    /// Set with the world position of the touch
    /// </param>
    public static bool TouchedCollider (int id, Collider collider, float distance, out Vector3 worldPos)
    {
        Vector2 screenPos;
        
        return TouchedCollider (id, collider, distance, out screenPos, out worldPos);
    }

    /// <summary>
    /// Checks to see if a collider was touched
    /// </summary>
    /// <returns>
    /// True if the collider was touched.
    /// </returns>
    /// <param name='id'>
    /// Touch id.
    /// </param>
    /// <param name='collider'>
    /// Collider to check for touch.
    /// </param>
    /// <param name='distance'>
    /// Distance away from the touch screen location to check for collider intersection
    /// </param>
    /// <param name='screenPos'>
    /// Set with the screen position of the touch.
    /// </param>
    /// <param name='worldPos'>
    /// Set with the world position of the touch
    /// </param>
    public static bool TouchedCollider (int id, Collider collider, float distance, out Vector2 pos, out Vector3 worldPos)
    {
        bool touchedCollider = false;
        
        worldPos = Vector3.zero;
        
        if (TouchManager.GetTouchPos (id, out pos)) {
            
            Ray ray = Camera.mainCamera.ScreenPointToRay (pos);
            
            RaycastHit hit;
            
            if (Physics.Raycast (ray, out hit, distance)) {
                
                if (hit.collider == collider) {
                    
                    worldPos = hit.point;
                    
                    touchedCollider = true;
                    
                }
            }
        }
        
        return touchedCollider;
    }



    public static bool TouchedColliderGO (int id, out GameObject go, float distance, out Vector2 pos, out Vector3 worldPos)
    {
		LayerMask invisbleLayer = 1 << 10;
		invisbleLayer = ~invisbleLayer;
		
        bool touchedCollider = false;
        go = null;
        
        worldPos = Vector3.zero;
        
        if (TouchManager.GetTouchPos (id, out pos)) {
            Ray ray = Camera.mainCamera.ScreenPointToRay (pos);
            
            RaycastHit hit;
            
            if (Physics.Raycast (ray, out hit, distance, invisbleLayer)) {
                
                go = hit.collider.gameObject;
                touchedCollider = true;
            }
        }
        
        return touchedCollider;
    }

    /// <summary>
    /// Checks to see if a GUIText object was touched
    /// </summary>
    /// <returns>
    /// True if the GUIText object was touched.
    /// </returns>
    /// <param name='id'>
    /// Touch id.
    /// </param>
    /// <param name='guiText'>
    /// GUIText object to check.
    /// </param>
    public static bool TouchedGUIText (int id, GUIText guiText)
    {
        Vector2 pos;
        
        return TouchedGUIText (id, guiText, out pos);
    }

    /// <summary>
    /// Checks to see if a GUIText object was touched
    /// </summary>
    /// <returns>
    /// True if the GUIText object was touched.
    /// </returns>
    /// <param name='id'>
    /// Touch id.
    /// </param>
    /// <param name='guiText'>
    /// GUIText object to check.
    /// </param>
    /// <param name='pos'>
    /// Set with the screen position of the touch
    /// </param>
    public static bool TouchedGUIText (int id, GUIText guiText, out Vector2 pos)
    {
        bool touchedGUIText = false;
        
        if (TouchManager.GetTouchPos (id, out pos)) {
            
            touchedGUIText = guiText.HitTest (pos);
            
        }
        
        return touchedGUIText;
    }


    public static string TouchedGUITexture (int id)
    {
    	Vector2 pos = Vector2.zero;
        GUILayer test = Camera.main.GetComponent<GUILayer>();
        
        if (TouchManager.GetTouchPos (id, out pos)) {
            
            if(test.HitTest(pos)){
            	return test.HitTest(pos).name;
            }
            
        }
        
        return null;
    }


    /// <summary>
    /// Calculates the rotation axis and torque for a rigid body based on the current touch position and the last touch position.
    /// </summary>
    /// <returns>
    /// True if rotation axis and torque were successfully calculated
    /// </returns>
    /// <param name='id'>
    /// Touch id.
    /// </param>
    /// <param name='axis'>
    /// Sets rotation axis.
    /// </param>
    /// <param name='torque'>
    /// Sets torque.
    /// </param>
    public static bool RotationAxisTorque (int id, out Vector3 axis, out float torque)
    {
        return RotationAxisTorque (id, 1, out axis, out torque);
    }

    /// <summary>
    /// Calculates the rotation axis and torque for a rigid body based on the current touch position and the last touch position.
    /// </summary>
    /// <returns>
    /// True if rotation axis and torque were successfully calculated
    /// </returns>
    /// <param name='id'>
    /// Touch id.
    /// </param>
    /// <param name='torqueScalar'>
    /// Scalar value applied to torque calculation.
    /// </param>
    /// <param name='axis'>
    /// Sets rotation axis.
    /// </param>
    /// <param name='torque'>
    /// Sets torque.
    /// </param>
    public static bool RotationAxisTorque (int id, float torqueScalar, out Vector3 axis, out float torque)
    {
        bool success = false;
        
        axis = Vector3.zero;
        
        torque = 0;
        
        Vector2 curPos, deltPos, lastPos;
        
        if (TouchManager.GetTouchPos (id, out curPos)) {
            
            Ray curRay = Camera.mainCamera.ScreenPointToRay (curPos);
            
            TouchManager.GetTouchDeltaPos (id, out deltPos);
            
            lastPos = curPos - deltPos;
            
            Ray prevRay = Camera.mainCamera.ScreenPointToRay (lastPos);
            
            axis = Vector3.Cross (curRay.direction, prevRay.direction);
            
            torque = (lastPos - curPos).magnitude * torqueScalar;
            
            success = true;
        }
        
        return success;
    }

    /// <summary>
    /// Rotates the rigidbody.
    /// </summary>
    /// <returns>
    /// True if rigid body is successfully rotated.
    /// </returns>
    /// <param name='id'>
    /// Touch id.
    /// </param>
    /// <param name='rigidbody'>
    /// Rigid body to rotate.
    /// </param>
    public static bool RotateRigidbody (int id, Rigidbody rigidbody)
    {
        return RotateRigidbody (id, 1, rigidbody);
    }

    /// <summary>
    /// Rotates the rigidbody.
    /// </summary>
    /// <returns>
    /// True if rigid body is successfully rotated.
    /// </returns>
    /// <param name='id'>
    /// Touch id.
    /// </param>
    /// <param name='torqueScalar'>
    /// Scalar value applied to torque calculation.
    /// </param>
    /// <param name='rigidbody'>
    /// Rigid body to rotate.
    /// </param>
    public static bool RotateRigidbody (int id, float torqueScalar, Rigidbody rigidbody)
    {
        bool success = false;
        
        Vector3 axis;
        
        float torque;
        
        if (TouchManager.RotationAxisTorque (id, torqueScalar, out axis, out torque)) {
            
            rigidbody.AddTorque (axis * torque);
            
            success = true;
        }
        
        return success;
    }

    /// <summary>
    /// Processes the touches.
    /// </summary>
    /// <returns>
    /// The number of touches.
    /// </returns>
    public static int ProcessTouches ()
    {
        #if UNITY_IPHONE
        
        if (Application.platform == RuntimePlatform.IPhonePlayer) {
            
            if (Application.isEditor == false) {
                
                return ProcessRealTouches ();
                
            } else {
                
                return ProcessMouseEvents ();
                
            }
            
        } else {
            #endif
            return ProcessMouseEvents ();
            #if UNITY_IPHONE
            
        }
        #endif
    }

    private static int ProcessMouseEvents ()
    {
        count = 0;
        
        if (Input.GetMouseButtonDown (0) == true && mouseLeftButtonDown == false) {
            
            mouseLeftButtonDown = true;
            
 //           Debug.Log("left mouse button DOWN");
            
            touchItem.phase = TouchPhaseEnum.BEGAN;
            touchItem.position = Input.mousePosition;
            touchItem.deltaPosition = Vector2.zero;
            count = 1;
            touchCache[0] = touchItem;
            if(tapEvent!=null)
				tapEvent(touchItem);
        
        } else {
            
            if (Input.GetMouseButtonUp (0) == true && mouseLeftButtonDown == true) {
                
                mouseLeftButtonDown = false;
                
 //               Debug.Log("left mouse button UP");
                
                touchItem.phase = TouchPhaseEnum.ENDED;
                
                UpdatePosition ();
                
                touchCache[0] = touchItem;
                
                count = 1;
        
            } 
            else if (mouseLeftButtonDown == true) {
                
                //Debug.Log("left mouse button DRAG");
                
                touchItem.phase = TouchPhaseEnum.MOVED;
                
                UpdatePosition ();
                
                touchCache[0] = touchItem;
                count = 1;
				
				if(dragEvent!=null)
					dragEvent(touchItem);
            }
        }
		return count;
    }

    private static void UpdatePosition ()
    {
        Vector2 pos = Input.mousePosition;
        
        touchItem.deltaPosition = (pos - touchItem.position);
        
        touchItem.position = pos;
        
    }

    #if UNITY_IPHONE
    private static int ProcessRealTouches ()
    {
        count = Input.touchCount;
        
        for (int i = 0; i < count; i++) {
            Touch touch = Input.GetTouch (i);
            int fingerId = touch.fingerId;
            
            if (fingerId >= MAX_TOUCHES) {
                
                fingerId = fingerId % MAX_TOUCHES;
            }
            
            // Cache the touch data.
            touchItem.deltaPosition = touch.deltaPosition;
            touchItem.position = touch.position;
            
            switch (touch.phase) {
            
            case TouchPhase.Ended:
                touchItem.phase = TouchPhaseEnum.ENDED;
                break;
            case TouchPhase.Canceled:
                touchItem.phase = TouchPhaseEnum.CANCELED;
                break;
            case TouchPhase.Began:
           		if(tapEvent!=null)
    	        	tapEvent(touchItem);
    
				touchItem.phase = TouchPhaseEnum.BEGAN;
                break;
            case TouchPhase.Moved:
                touchItem.phase = TouchPhaseEnum.MOVED;
				if(dragEvent!=null)
					dragEvent(touchItem);
                break;
            case TouchPhase.Stationary:
                touchItem.phase = TouchPhaseEnum.STATIONARY;
                break;
                
            }
            
            touchCache[fingerId] = touchItem;
	    }
        
        return count;
    }
    #endif
}
