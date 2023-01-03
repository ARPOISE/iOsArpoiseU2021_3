//-----------------------------------------------------------------------
// <copyright file="AugmentedImageVisualizer.cs" company="Google">
//
// Copyright 2018 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

/*
AugmentedImageVisualizer.cs - MonoBehaviour for handling detected image triggers of the Android version of image trigger ARpoise, aka AR-vos.

This file is part of ARpoise.

This file is derived from image trigger example of the Google ARCore SDK for Unity

https://github.com/google-ar/arcore-unity-sdk

The license of the original file is shown above.

For more information on 

Tamiko Thiel, see www.TamikoThiel.com/
Peter Graf, see www.mission-base.com/peter/
ARpoise, see www.ARpoise.com/

*/

namespace GoogleARCore.Examples.AugmentedImage
{
    using com.arpoise.arpoiseapp;
#if HAS_AR_CORE
    using GoogleARCore;
    using System;
#endif
    using UnityEngine;

    /// <summary>
    /// Uses 4 frame corner objects to visualize an AugmentedImage.
    /// </summary>
    public class AugmentedImageVisualizer : MonoBehaviour
    {
#if HAS_AR_CORE
        /// <summary>
        /// The AugmentedImage to visualize.
        /// </summary>
        public AugmentedImage Image;

        /// <summary>
        /// The hit pose use to place the TriggerObject.
        /// </summary>
        public Pose? Pose = null;

        /// <summary>
        /// The object to visualize.
        /// </summary>
        public TriggerObject TriggerObject { get; set; }

        /// <summary>
        /// The behaviour.
        /// </summary>
        public ArBehaviourImage ArBehaviour { get; set; }

        private GameObject _gameObject = null;
        private bool _gameObjectCreated = false;

        public void Start()
        {
        }

        private bool _first = true;

        public void Update()
        {
            if (Pose != null)
            {
                // Check that motion tracking is tracking.
                if (Session.Status != SessionStatus.Tracking)
                {
                    if (_gameObject != null)
                    {
                        _gameObject.SetActive(false);
                        _first = true;
                    }
                    return;
                }
            }
            else if (Image == null || Image.TrackingState != TrackingState.Tracking)
            {
                if (_gameObject != null)
                {
                    _gameObject.SetActive(false);
                    _first = true;
                }
                return;
            }

            var arObjectState = ArBehaviour.ArObjectState;
            if (arObjectState != null && TriggerObject != null && !_gameObjectCreated)
            {
                _gameObjectCreated = true;

                if (Pose != null)
                {
                    transform.position = Pose.Value.position;
                    transform.rotation = Pose.Value.rotation;
                }
                else
                {
                    transform.position = Image.CenterPose.position;
                    transform.rotation = Image.CenterPose.rotation;
                }

                var result = ArBehaviour.CreateArObject(
                    arObjectState,
                    TriggerObject.gameObject,
                    null,
                    transform,
                    TriggerObject.poi,
                    TriggerObject.poi.id,
                    out _gameObject
                    );
                if (!string.IsNullOrWhiteSpace(result))
                {
                    ArBehaviour.ErrorMessage = result;
                    return;
                }
            }

            if (_gameObject != null)
            {
                _gameObject.SetActive(true);

                if (Pose != null)
                {
                    if (_first)
                    {
                        _first = false;
                        _gameObject.transform.position = Pose.Value.position;
                        _gameObject.transform.rotation = Pose.Value.rotation;
                    }
                }
                else
                {
                    //if (_first)
                    //{
                        _first = false;
                        _gameObject.transform.position = Image.CenterPose.position;
                        _gameObject.transform.rotation = Image.CenterPose.rotation;
                    //}
                    //else
                    //{
                    //    _gameObject.transform.position = Vector3.Lerp(_gameObject.transform.position, Image.CenterPose.position, 0.02f);
                    //    _gameObject.transform.rotation = Quaternion.Lerp(_gameObject.transform.rotation, Image.CenterPose.rotation, 0.02f);
                    //}
                }
            }
        }
#endif
    }
}
