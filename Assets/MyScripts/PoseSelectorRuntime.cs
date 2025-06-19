using UnityEngine;

[DisallowMultipleComponent]
public class PoseSelectorRuntime : MonoBehaviour
{
    [Header("Assign your PoseLibrary asset here")] 
    public PoseLibrary poseLibrary;   // Now holds PoseWithImage[]

    [Header("Which pose to detect (0-based index)")]
    [Tooltip("Change this at any time in Play Mode")]
    public int poseIndex = 0;

    int _lastIndex = -1;  // tracks the last applied index

    // ← Updated to return our new type
    public PoseWithImage CurrentEntry
    {
        get
        {
            if (poseLibrary == null) return null;
            if (poseIndex < 0 || poseIndex >= poseLibrary.poses.Length) return null;
            return poseLibrary.poses[poseIndex];
        }
    }

    void Start()
    {
        ApplyPose(poseIndex);
        _lastIndex = poseIndex;
    }

    void Update()
    {
        if (poseIndex != _lastIndex)
        {
            ApplyPose(poseIndex);
            _lastIndex = poseIndex;
        }
    }

    void ApplyPose(int index)
    {
        var entry = CurrentEntry;
        if (entry == null)
        {
            Debug.LogError($"PoseSelectorRuntime: poseIndex {index} out of range or entry is null");
            return;
        }

        // Log both the pose name and preview image name
        Debug.Log($"[PoseSelectorRuntime] Selecting pose: {entry.poseAsset.name} (preview: {entry.previewImage?.name})");

        // TODO: inject entry.poseAsset into your evaluators
        // e.g.: DynamicGestureController.Instance.InjectPose(entry.poseAsset);

        // TODO: display entry.previewImage in your UI
        // e.g.: previewImageUI.sprite = entry.previewImage;
    }
}
