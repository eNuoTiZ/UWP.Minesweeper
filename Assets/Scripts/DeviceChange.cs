﻿using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DeviceChange : MonoBehaviour
{
    public UnityEvent OnResolutionChange = new UnityEvent();
    public UnityEvent OnOrientationChange = new UnityEvent();
    public static float CheckDelay = 0.5f;        // How long to wait until we check again.

    public static Vector2 resolution;                    // Current Resolution
    public static DeviceOrientation orientation;        // Current Device Orientation
    static bool isAlive = true;                    // Keep this script running?

    static bool resizingBoard = false;

    void Start()
    {
        OnOrientationChange.AddListener(delegate { OrientationChanged(); });
        StartCoroutine(CheckForChange());
    }

    void OrientationChanged()
    {
        if (!resizingBoard)
        {
            resizingBoard = true;
            Board.Instance().ResizeBoard(Options.Instance.CellRatio);
            //Board.Instance().ResetBoard();
            resizingBoard = false;
        }
        
    }

    IEnumerator CheckForChange()
    {
        resolution = new Vector2(Screen.width, Screen.height);
        orientation = Input.deviceOrientation;

        while (isAlive)
        {

            // Check for a Resolution Change
            //if (resolution.x != Screen.width || resolution.y != Screen.height)
            //{
            //    resolution = new Vector2(Screen.width, Screen.height);
            //    OnResolutionChange.Invoke();
            //}

            // Check for an Orientation Change
            switch (Input.deviceOrientation)
            {
                case DeviceOrientation.Unknown:            // Ignore
                case DeviceOrientation.FaceUp:            // Ignore
                case DeviceOrientation.FaceDown:        // Ignore
                    break;
                case DeviceOrientation.LandscapeLeft:
                case DeviceOrientation.LandscapeRight:
                case DeviceOrientation.Portrait:
                case DeviceOrientation.PortraitUpsideDown:
                    if (orientation != Input.deviceOrientation)
                    {
                        orientation = Input.deviceOrientation;
                        OnOrientationChange.Invoke();
                    }
                    break;
                default:
                    break;
            }

            yield return new WaitForSeconds(CheckDelay);
        }
    }

    void OnDestroy()
    {
        isAlive = false;
    }

}