using UnityEngine;

public class CaptureFrames : MonoBehaviour
{
    public Camera cameraToCapture;
    public int frameRate = 25;
    private string folderPath = "CapturedFrames/";
    private int frameCount;

    void Start()
    {
        // Create the folder if it doesn't exist
        if (!System.IO.Directory.Exists(folderPath))
            System.IO.Directory.CreateDirectory(folderPath);

        // Set the playback framerate (real time will not influence time anymore)
        Time.captureFramerate = frameRate;
    }

    void Update()
    {
        // Construct the filename
        string filename = string.Format("{0}/frame{1:D04}.png", folderPath, frameCount);

        // Capture the screenshot to the specified file
        ScreenCapture.CaptureScreenshot(filename);

        frameCount++;
    }
}
