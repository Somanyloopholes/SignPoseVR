using UnityEngine;
using UnityEngine.UI;
using TMPro;

[DisallowMultipleComponent]
public class LearnModeController : MonoBehaviour
{
    public PoseLibrary             poseLibrary;          // Reference to the pose array data set
    public DynamicGestureController dynamicController;   // Reference to DynamicGestureController.cs
    public Button                  skipButton;           // UI button for skipping to the next pose
    public Button                  changeModeButton;     // UI button for toggling between Learn and Quiz modes
    public Image                   poseImageUI;          // UI pose image
    public TMP_Text                guessCounterText;
    public TMP_Text                modeIndicatorText;
    public GameObject              tipsCanvas;            // Canvas taht shows the tips

    // Track the last random index to avoid repeats
    private int   _lastIndex = -1;
    // Whether we are currently in quiz mode
    private bool _isQuizMode = false;
    // How many poses have been correctly matched
    private int _correctGuessCount = 0;


    void Awake()
    {
        // Auto-assign the gesture controller if not manually linked
        if (dynamicController == null)
            dynamicController = GetComponent<DynamicGestureController>();

        // Warn if pose library is not assigned
        if (poseLibrary == null)
            Debug.LogError("LearnModeController: PoseLibrary not assigned");

        // Link skip button to next pose selection
        if (skipButton != null)
            skipButton.onClick.AddListener(SelectNextPose);
        else
            Debug.LogWarning("LearnModeController: SkipButton not assigned");

        // Link change mode button to toggle logic
        if (changeModeButton != null)
            changeModeButton.onClick.AddListener(ToggleMode);
        else
            Debug.LogWarning("LearnModeController: ChangeModeButton not assigned");

        // Initialize UI states
        UpdateModeUI();
    }

    // Subscribe to gesture events and start with a pose
    void Start()
    {
        dynamicController.PoseMatched += OnPoseMatched;
        SelectNextPose();
    }


    // Cleanup to avoid memory leaks or lingering event handlers
    void OnDestroy()
    {
        dynamicController.PoseMatched -= OnPoseMatched;
        if (skipButton != null)
            skipButton.onClick.RemoveListener(SelectNextPose);
        if (changeModeButton != null)
            changeModeButton.onClick.RemoveListener(ToggleMode);
    }


    // Toggles between Learn and Quiz modes.
    private void ToggleMode()
    {
        _isQuizMode = !_isQuizMode;
        if (_isQuizMode)
            _correctGuessCount = 0;
        UpdateModeUI();
    }

    
    // -Updates UI visibility and content based on current mode.
    // - Hides pose image in Quiz mode (so user must guess).
    // - Shows/hides guess counter appropriately.
    // - Updates mode indicator text and tips canvas visibility.
    private void UpdateModeUI()
    {
        // Toggle pose image
        if (poseImageUI != null)
            poseImageUI.gameObject.SetActive(!_isQuizMode);

        // Toggle guess counter: visible in Quiz mode only
        if (guessCounterText != null)
            guessCounterText.gameObject.SetActive(_isQuizMode);

        // Update mode indicator text
        if (modeIndicatorText != null)
        {
            modeIndicatorText.gameObject.SetActive(true);
            modeIndicatorText.text = _isQuizMode ? "Quiz Mode" : "Learn Mode";
        }

        // Update score text if visible
        if (_isQuizMode && guessCounterText != null)
            guessCounterText.text = $"Score: {_correctGuessCount}";

        // Hide tips canvas in quiz mode
        if (tipsCanvas != null)
            tipsCanvas.SetActive(!_isQuizMode);
    }


    // Called when a pose is successfully matched adn advances pose and updates score in Quiz mode.
    private void OnPoseMatched()
    {
        if (_isQuizMode)
        {
            _correctGuessCount++;
            if (guessCounterText != null)
                guessCounterText.text = $"Score: {_correctGuessCount}";  // Ensure counter updates immediately
        }
        SelectNextPose();
    }


    // Chooses and applies a new random pose.
    public void SelectNextPose()
    {
        var poses = poseLibrary?.poses;
        if (poses == null || poses.Length == 0)
            return;

        int idx;
        do { idx = Random.Range(0, poses.Length); }
        while (poses.Length > 1 && idx == _lastIndex);

        _lastIndex = idx;
        var entry = poses[idx];
        dynamicController.SetEntry(entry);

        // Update pose image sprite
        if (poseImageUI != null && entry.previewImage != null)
            poseImageUI.sprite = entry.previewImage;
    }
}
