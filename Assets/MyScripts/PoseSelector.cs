using UnityEngine;

public class PoseSelectorTest : MonoBehaviour
{
    [Header("Pose Library (assign your ASL_Poses asset)")]
    public PoseLibrary poseLibrary;

    [Header("Pose to test")]
    [Tooltip("Index into poseLibrary.poses (0-based)")]
    public int poseIndex = 0;

    void Start()
    {
        // 1. Library assigned?
        if (poseLibrary == null)
        {
            Debug.LogError("PoseSelectorTest: No PoseLibrary assigned!");
            return;
        }

        // 2. Index in range?
        if (poseIndex < 0 || poseIndex >= poseLibrary.poses.Length)
        {
            Debug.LogError($"PoseSelectorTest: poseIndex {poseIndex} is out of range 0..{poseLibrary.poses.Length - 1}");
            return;
        }

        // 3. Grab and log the asset
        var poseAsset = poseLibrary.poses[poseIndex];
        Debug.Log($"PoseSelectorTest: Selected pose → {poseAsset.name}");
    }
}
