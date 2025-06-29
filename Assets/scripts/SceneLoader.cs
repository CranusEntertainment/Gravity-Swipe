using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public Slider loadingSlider;
    public Text progressText; // opsiyonel

    void Start()
    {
        StartCoroutine(LoadSceneAsync("Orange_PresetScene Lite"));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        float targetProgress = 0f;

        while (operation.progress < 0.9f)
        {
            targetProgress = Mathf.Clamp01(operation.progress / 0.9f);

            // Yumuþak geçiþ
            loadingSlider.value = Mathf.Lerp(loadingSlider.value, targetProgress, Time.deltaTime * 5f);

            if (progressText != null)
                progressText.text = (loadingSlider.value * 100f).ToString("F0") + "%";

            yield return null;
        }

        // %90'a ulaþtýysa son %10'u da göster:
        while (loadingSlider.value < 1f)
        {
            loadingSlider.value = Mathf.Lerp(loadingSlider.value, 1f, Time.deltaTime * 5f);

            if (progressText != null)
                progressText.text = (loadingSlider.value * 100f).ToString("F0") + "%";

            yield return null;
        }

        yield return new WaitForSeconds(0.5f); // geçmeden önce son bir bekleme
        operation.allowSceneActivation = true;
    }

}
