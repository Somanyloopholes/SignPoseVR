using UnityEngine;

[CreateAssetMenu(
    menuName = "SignPoseVR/Pose Library", 
    fileName = "PoseLibrary"
)]
public class PoseLibrary : ScriptableObject
{
    [Tooltip("List of poses each paired with an image, a name, and a description")]
    public PoseWithImage[] poses;
}
