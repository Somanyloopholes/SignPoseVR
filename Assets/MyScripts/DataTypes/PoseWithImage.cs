using UnityEngine;

[CreateAssetMenu(
    menuName = "SignPoseVR/Pose With Image", 
    fileName = "NewPoseWithImage"
)]
public class PoseWithImage : ScriptableObject
{
    [Tooltip("The actual hand-pose asset used by StaticHandGesture")]
    public ScriptableObject poseAsset;   // or your concrete PoseDefinition type

    [Tooltip("A preview sprite or icon for UI")]
    public Sprite previewImage;

    [Tooltip("A longer description or any notes about this pose")]
    [TextArea(3, 6)]
    public string description;
}
