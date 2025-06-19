using UnityEngine;
using System;
using System.Collections;
using System.Reflection;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Hands.Samples.GestureSample;
using TMPro;
using UnityEngine.UI;

public class DynamicGestureController : MonoBehaviour
{
    public StaticHandGesture[] staticHandGestures;  // Array of static Hand gestures
    public TMP_Text statusText;                     // text that displays the current pose name
    public TMP_Text descriptionText;                // Text that displays the pose description
    public Image previewImageUI;                    // Image that displays the pose
    public float previewScale = 0.01f;              // Scaling for the pose image
    public Transform cubesParent;                   // Parent object containing the border glow cubes
    public Material normalBorderMaterial;           // Assign dark material
    public Material glowBorderMaterial;             // Assign glowing material
    private Renderer[] cubeRenderers;               // Internal reference to each cube's renderer
    PoseWithImage _currentEntry;                    // Current pose entry 
    int _leftHandActiveCount = 0;
    int _rightHandActiveCount = 0;
    bool _poseTriggered = false;
    Coroutine _holdCoroutine;
    public event Action PoseMatched;                // Event fired when pose has been held long enough


    void Awake()
    {   
        //collecting staticHandGedstures
        if (staticHandGestures == null || staticHandGestures.Length == 0)
            staticHandGestures = GetComponentsInChildren<StaticHandGesture>();

        //registering listeners for gesture detection
        foreach (var g in staticHandGestures)
        {
            g.gesturePerformed.AddListener(() => OnGestureDetected(g));
            g.gestureEnded.AddListener(() => OnGestureLost(g));
        }

        // initializing border glow cubes
        if (cubesParent != null)
        {
            cubeRenderers = cubesParent.GetComponentsInChildren<Renderer>();
            if (normalBorderMaterial != null)
            {
                foreach (var rend in cubeRenderers)
                    rend.material = normalBorderMaterial;
            }
        }
    }

   
    // Sets a new pose entry, updates gesture evaluators and the UI.
    public void SetEntry(PoseWithImage entry)
    {
        _currentEntry = entry;
        _leftHandActiveCount = 0;
        _rightHandActiveCount = 0;
        _poseTriggered = false;
        if (_holdCoroutine != null)
        {
            StopCoroutine(_holdCoroutine);
            _holdCoroutine = null;
        }

        // Inject the new pose into all gesture evaluators
        foreach (var g in staticHandGestures)
            InjectPose(g, entry?.poseAsset);

        // Update UI with the new pose
        UpdateUI(entry);
    }


    // Updates the UI text and image with pose data.
    void UpdateUI(PoseWithImage entry)
    {   
        // Clear UI when no pose is selected
        if (entry == null)
        {
            statusText.text = "No pose selected";
            descriptionText.text = string.Empty;
            if (previewImageUI != null)
                previewImageUI.sprite = null;
            return;
        }

        // Display pose name and description text
        statusText.text = (entry.previewImage != null) ? entry.name : string.Empty;
        descriptionText.text = entry.description;

        // Display pose image
        if (previewImageUI != null)
        {
            previewImageUI.sprite = entry.previewImage;
            var rt = previewImageUI.rectTransform;
            var spriteRect = entry.previewImage.rect;
            rt.sizeDelta = new Vector2(spriteRect.width, spriteRect.height) * previewScale;
        }
    }


    //// Called when a static hand gesture is detected
    void OnGestureDetected(StaticHandGesture gesture)
    {   
        // Determine if gesture is from left or right hand
        bool isLeft = gesture.name.ToLower().Contains("left");
        if (isLeft)
            _leftHandActiveCount++;
        else
            _rightHandActiveCount++;

        // Change cube border materials to glowing to indicate detection
        if (cubeRenderers != null && glowBorderMaterial != null)
        {
            foreach (var rend in cubeRenderers)
                rend.material = glowBorderMaterial;
        }

        // Start coroutine to check if the pose is held long enough
        if (!_poseTriggered && _holdCoroutine == null &&
            (_leftHandActiveCount > 0 || _rightHandActiveCount > 0))
        {
            _holdCoroutine = StartCoroutine(HoldAndTrigger());
        }
    }


    // Coroutine that waits and checks to confirm the pose is held steadily for a certain time limit
    IEnumerator HoldAndTrigger()
    {
        yield return new WaitForSeconds(0.8f);
        if ((_leftHandActiveCount > 0 || _rightHandActiveCount > 0) && !_poseTriggered)
        {
            _poseTriggered = true;
            PoseMatched?.Invoke();
        }
    }


    // Called when a static gesture is no longer detected
    void OnGestureLost(StaticHandGesture gesture)
    {   
        // Decrease active count for the appropriate hand
        bool isLeft = gesture.name.ToLower().Contains("left");
        if (isLeft)
            _leftHandActiveCount = Mathf.Max(0, _leftHandActiveCount - 1);
        else
            _rightHandActiveCount = Mathf.Max(0, _rightHandActiveCount - 1);

        // If no gestures are being held by either hand
        if (_leftHandActiveCount == 0 && _rightHandActiveCount == 0)
        {   
            // Stop the hold coroutine if it's still running
            if (_holdCoroutine != null)
            {
                StopCoroutine(_holdCoroutine);
                _holdCoroutine = null;
            }   
            // Reset pose trigger state
            _poseTriggered = false;

            // Revert cube border materials to normal
            if (cubeRenderers != null && normalBorderMaterial != null)
            {
                foreach (var rend in cubeRenderers)
                    rend.material = normalBorderMaterial;
            }
        }
    }


    // Dynamically injects a new pose into the StaticHandGesture evaluator using reflection
    void InjectPose(StaticHandGesture evaluator, ScriptableObject pose)
    {
        if (evaluator == null || pose == null)
            return;

        // Try to set the public HandShapeOrPose property
        var prop = evaluator.GetType().GetProperty("HandShapeOrPose", BindingFlags.Instance | BindingFlags.Public);
        if (prop != null && prop.CanWrite)
            prop.SetValue(evaluator, pose);
        else
        {   
            // Try to set one of the known private fields using reflection
            var field = evaluator.GetType()
                .GetField("m_HandShapeOrPose", BindingFlags.Instance | BindingFlags.NonPublic)
                     ?? evaluator.GetType()
                .GetField("m_HandPose", BindingFlags.Instance | BindingFlags.NonPublic);
            if (field != null)
                field.SetValue(evaluator, pose);
            else Debug.LogError($"Couldn't find pose field on {evaluator.name}", evaluator);
        }

        // Manually call the initialization method to apply the new pose
        var init = evaluator.GetType()
            .GetMethod("OnEnable", BindingFlags.Instance | BindingFlags.NonPublic)
            ?? evaluator.GetType()
            .GetMethod("Initialize", BindingFlags.Instance | BindingFlags.NonPublic);
        init?.Invoke(evaluator, null);
    }
}
