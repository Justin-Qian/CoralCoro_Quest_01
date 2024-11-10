using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickBubble : MonoBehaviour
{
    [SerializeField] private GameObject completeBubbleClickVisual;
    [SerializeField] private GameObject brokenBubbleClickVisual;

    private Material material;
    private float initialAlpha = 0.0f;
    bool isTouched = false;
    private int touchedBlockCount = 0;
    private float brokenTime = 0.8f;


    private void Awake() {
        brokenBubbleClickVisual.SetActive(false);
        completeBubbleClickVisual.SetActive(true);

        material =  FindChildByName(completeBubbleClickVisual.transform, "Bubble").GetComponent<Renderer>().material;
        // 确保材质支持透明并设置初始透明度
        if (material.HasProperty("_BaseColor")) {
            Color color = material.GetColor("_BaseColor");
            color.a = initialAlpha;
            material.SetColor("_BaseColor", color);
        }
    }

    private void Start() {
        EventManager.OnTouchBubble += InformSelfIsTouched;
    }

    private void InformSelfIsTouched(int touchedBlockCount) {
        isTouched = true;
        this.touchedBlockCount = touchedBlockCount;
    }

    public void StartFadeIn(float duration, float lifeSpan, int blockCount) {
        StartCoroutine(FadeIn(duration, lifeSpan, blockCount));
    }

    private IEnumerator FadeIn(float duration, float lifeSpan, int blockCount) {

        //yield return new WaitForSeconds(duration);

        float elapsedTime = 0.0f;
        Color color = material.GetColor("_BaseColor");

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            float currentAlpha = Mathf.Clamp01(elapsedTime / duration) * 70/255f;
            color.a = currentAlpha;
            material.SetColor("_BaseColor", color);
            yield return null;
        }

        // 确保最终透明度是完全不透明
        color.a = 70/255f;
        material.SetColor("_BaseColor", color);
        material.EnableKeyword("_EMISSION");

        float waitTime = 0.0f;
        while (waitTime < lifeSpan) {
            if (isTouched && blockCount == touchedBlockCount) {
                brokenBubbleClickVisual.SetActive(true);
                break;
            } 
            waitTime += Time.deltaTime;
            yield return null;
        }
        Destroy(completeBubbleClickVisual);
        Debug.Log(lifeSpan);

        float newWaitTime = 0.0f;

        if (isTouched && blockCount == touchedBlockCount) {
            while (newWaitTime < brokenTime) {
                newWaitTime += Time.deltaTime;
                yield return null;
            }
            Destroy(brokenBubbleClickVisual);
        } 
    }
    Transform FindChildByName(Transform parent, string name) {
        foreach (Transform child in parent) {
            if (child.name == name) {
                return child;
            }
            Transform result = FindChildByName(child, name);
            if (result != null) {
                return result;
            }
        }
        return null;
    }

}
