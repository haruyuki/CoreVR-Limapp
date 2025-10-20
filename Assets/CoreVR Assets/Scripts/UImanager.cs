using System.Collections;
using Liminal.Core.Fader;
using Liminal.Platform.Experimental.App.Experiences;
using Liminal.SDK.Core;
using Liminal.SDK.VR;
using Liminal.SDK.VR.Avatars;
using Liminal.SDK.VR.Input;
using UnityEngine;

namespace Liminal.Examples
{
    public class UImanager : MonoBehaviour
    {
        public Transform Cube;
        public Animator fadeAnimator;
        public float startDelay = 0.5f;
        public bool startGame = false;

        private void Update()
        {
            var avatar = VRAvatar.Active;
            if (avatar == null)
                return;

            if(startGame){
                startGame = false;
                StartGame();
            }

            var rightInput = GetInput(VRInputDeviceHand.Right);
            var leftInput = GetInput(VRInputDeviceHand.Left);

            // Input Examples
            if (rightInput != null)
            {
                if (rightInput.GetButtonDown(VRButton.Back))
                    Debug.Log("Back button pressed");

                if (rightInput.GetButtonDown(VRButton.One))
                    Debug.Log("Trigger button pressed");
            }

            if (leftInput != null)
            {
                if (leftInput.GetButtonDown(VRButton.Back))
                    Debug.Log("Back button pressed");

                if (leftInput.GetButtonDown(VRButton.One))
                    Debug.Log("Trigger button pressed");
            }

            // Any input
            // VRDevice.Device.GetButtonDown(VRButton.One);
        }

        private IVRInputDevice GetInput(VRInputDeviceHand hand)
        {
            var device = VRDevice.Device;
            return hand == VRInputDeviceHand.Left ? device.SecondaryInputDevice : device.PrimaryInputDevice;
        }

        /// <summary>
        /// End will only close the application when you're within the platform
        /// </summary>
        public void End()
        {
            ExperienceApp.End();
        }

        public void Start()
        {
            Time.timeScale = 0f;
        }

        public void StartGame()
        {
            StartCoroutine(StartRoutine());
        }

        public void PauseGame()
        {
            StartCoroutine(PauseRoutine());
        }

        public void PlayGame()
        {
            StartCoroutine(PlayRoutine());
        }

        private IEnumerator StartRoutine()
        {
            
            yield return new WaitForSecondsRealtime(startDelay);
            fadeAnimator.SetBool("Fade", true);
            Time.timeScale = 1f;
        }

        private IEnumerator PauseRoutine()
        {
            Time.timeScale = 0f;
            fadeAnimator.SetBool("Pause", true);
            fadeAnimator.SetBool("Play", false);
            yield break;
        }

        private IEnumerator PlayRoutine()
        {
            
            yield return new WaitForSecondsRealtime(startDelay);
            fadeAnimator.SetBool("Play", true);
            fadeAnimator.SetBool("Pause", false);
            Time.timeScale = 1f;
        }

        public void ChangeCubeSize()
        {
            Cube.localScale = Vector3.one * Random.Range(0.1f, 0.35F);
        }
    }
}
