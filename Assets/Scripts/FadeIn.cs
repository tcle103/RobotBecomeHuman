using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeIn : MonoBehaviour
{
    public float duration;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate()
    {
        StartCoroutine(TransitionFade(1, duration));
    }

    IEnumerator TransitionFade(float end, float duration)
    {
        var sceneTo = SceneManager.LoadSceneAsync("OptionMenu");
        sceneTo.allowSceneActivation = false;

        float elapsed = 0;
        SpriteRenderer overlayRenderer = this.GetComponent<SpriteRenderer>();
        Color baseColor = overlayRenderer.color;
        float start = baseColor.a;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(start, end, elapsed / duration);
            Debug.Log(newAlpha);
            overlayRenderer.color = new Color(baseColor.r, baseColor.g, baseColor.b, newAlpha);
            yield return null;
        }
        Debug.Log("fade done" + overlayRenderer.color.a);
        yield return new WaitForSeconds(5);
        sceneTo.allowSceneActivation = true;
    }
}
