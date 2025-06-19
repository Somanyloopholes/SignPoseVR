using UnityEngine;

[CreateAssetMenu(menuName = "SignPoseVR/Pose Asset", fileName = "NewPose")]
public class PoseAsset : ScriptableObject
{
    [Header("Pose Information")]
    public string poseName;
    
    [Header("Visual Reference")]
    [Tooltip("The image that shows how to perform this pose")]
    public Sprite poseSprite;
    
    [Header("Pose Detection")]
    [Tooltip("The hand shape asset used for pose detection")]
    public ScriptableObject handShape;
    
    [Header("Orientation Settings")]
    public RelativeOrientation relativeOrientation;
    
    // Add any other pose-specific data here
    // For example:
    // public HandPose handPose;
    // public float threshold;
    // etc.
}

[System.Serializable]
public class RelativeOrientation
{
    public UserCondition[] userConditions;
    public TargetCondition[] targetConditions;
}

[System.Serializable]
public class UserCondition
{
    public int handAxis;
    public int alignmentCondition;
    public int referenceDirection;
    public float angleTolerance;
    public bool ignorePositionY;
}

[System.Serializable]
public class TargetCondition
{
    public int handAxis;
    public int alignmentCondition;
    public int referenceDirection;
    public float angleTolerance;
    public bool ignorePositionY;
} 