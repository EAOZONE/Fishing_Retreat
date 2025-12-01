using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Management;


public class XRSceneLoader : MonoBehaviour
{
    public void LoadSceneXR(string sceneName)
    {
        StartCoroutine(LoadXRScene(sceneName));
    }

    IEnumerator LoadXRScene(string sceneName)
    {
        // 1) Stop XR
        XRGeneralSettings.Instance.Manager.StopSubsystems();
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();

        // 2) Load your scene
        yield return SceneManager.LoadSceneAsync(sceneName);

        // 3) Reinitialize XR for the NEW scene
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();
        XRGeneralSettings.Instance.Manager.StartSubsystems();
    }
}
