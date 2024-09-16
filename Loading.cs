using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Loading : MonoBehaviour
{
    [SerializeField] Image _loadingImage;
    [SerializeField] Text _loadingText;
    void Awake()
    {
        StartCoroutine(LoadScene());
        _loadingImage.fillAmount = 0;
        _loadingText.text = "Progress: 0%";
    }
    IEnumerator LoadScene()
    {
        yield return null;
        

        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(2);
        //When the load is still in progress, output the Text and progress bar
        while (!asyncOperation.isDone)
        {
            _loadingImage.fillAmount = asyncOperation.progress;

            Debug.Log(asyncOperation.progress);
            _loadingText.text = "Progress : " + Mathf.Round(asyncOperation.progress * 100).ToString() + "%";
            //yield return new WaitForEndOfFrame();
            yield return null;

        }
        Debug.Log("scene Load Complete");
    }
    private IEnumerator LoadScene2()
    {
        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(2);
        asyncOperation.allowSceneActivation = false; // Prevents the scene from activating immediately.

        while (!asyncOperation.isDone)
        {
            // Unity's progress stalls at 0.9, so handle this by using Mathf.Clamp01 to map to 0-1 range.
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            _loadingImage.fillAmount = progress;
            _loadingText.text = "Progress: " + (progress * 100f).ToString("F0") + "%";

            // Only allow scene activation when the progress is fully done.
            if (asyncOperation.progress >= 0.9f)
            {
                _loadingText.text = "Progress: 100%";
                _loadingImage.fillAmount = 1.0f;
                yield return new WaitForSeconds(1); // Optional delay to ensure everything is ready
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}


