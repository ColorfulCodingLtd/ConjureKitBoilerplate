using UnityEngine;
using Auki.ConjureKit;
using UnityEngine.UI;
using Auki.ConjureKit.Manna;
using Auki.Ur;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;
using AukiHandTrackerSample;
public class UR : MonoBehaviour
{

    private HandTracker _handTracker;
    public Camera arCamera;
    [SerializeField] private ARSession arSession;
    [SerializeField] private ARRaycastManager arRaycastManager;
    [SerializeField] private Renderer finger;
    public int NumberOfTrackedHands;
    private List<HandLandmark> _handLandmarks;
    // Start is called before the first frame update
    void Start()
    {
        _handTracker = HandTracker.GetInstance();


        _handTracker.SetARSystem(arSession, arCamera, arRaycastManager);
        _handLandmarks = new List<HandLandmark>(NumberOfTrackedHands * HandTracker.LandmarksCount);
        finger.transform.SetParent(arCamera.transform);

        _handTracker.OnUpdate += (landmarks, translations, isRightHand, score) =>
        {
            for (int h = 0; h < NumberOfTrackedHands; ++h)
            {
                // Toggle the visibility of the hand landmarks based on the confidence score of the hand detection
                ToggleHandLandmarks(score[h] > 0);
                if (score[h] > 0)
                {
                    var handPosition = new Vector3(
                        translations[h * 3 + 0],
                        translations[h * 3 + 1],
                        translations[h * 3 + 2]);

                    var handLandmarkIndex = h * HandTracker.LandmarksCount * 3;
                    for (int l = 0; l < HandTracker.LandmarksCount; ++l)
                    {
                        var landMarkPosition = new Vector3(
                            landmarks[handLandmarkIndex + (l * 3) + 0],
                            landmarks[handLandmarkIndex + (l * 3) + 1],
                            landmarks[handLandmarkIndex + (l * 3) + 2]);

                        // Update the landmarks position 
                        _handLandmarks[l].transform.localPosition = handPosition + landMarkPosition;
                    }
                }
            }
        };


        _handTracker.Start();
        _handTracker.ShowHandMesh();
    }

    // Update is called once per frame
    void Update()
    {
        _handTracker.Update();
    }
    private void ToggleHandLandmarks(bool visible)
    {
        foreach (var handLandmark in _handLandmarks)
        {
            handLandmark.gameObject.SetActive(visible);
        }
    }
}
