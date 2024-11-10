using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeBubble : MonoBehaviour
{
    public GameObject completeBubbleStartVisual;
    public GameObject brokenBubbleStartVisual;
    public GameObject completeBubbleEndVisual;
    public GameObject brokenBubbleEndVisual;

    public GameObject[] smallBubbles = new GameObject[3];
    private Material[] smallBubblesMaterial = new Material[3];

    private Material startBubbleMaterial;
    private Material endBubbleMaterial;
    private float initialAlpha = 0.0f;
    private bool isTouched = false;
    private int touchedBlockCount = 0;
    private float transitTime = 0.2f;
    private float brokenTime = 0.8f;

    private void Awake() {
        brokenBubbleStartVisual.SetActive(false);
        brokenBubbleEndVisual.SetActive(false);
    }
    private void Start() {
        EventManager.OnTouchBubble += InformSelfIsTouched;
    }


    private void InformSelfIsTouched(int touchedBlockCount) {
        isTouched = true;
        this.touchedBlockCount = touchedBlockCount;
    }

    public void StartFadeIn(float duration, float startLifeSpan,float endLifeSpan, int blockCount) {
        startBubbleMaterial = FindChildByName(completeBubbleStartVisual.transform, "Bubble").GetComponent<Renderer>().material;
        endBubbleMaterial = FindChildByName(completeBubbleEndVisual.transform, "Bubble").GetComponent<Renderer>().material;
        
        // 确保材质支持透明并设置初始透明度
        if (startBubbleMaterial.HasProperty("_BaseColor")) {
            Color color = startBubbleMaterial.GetColor("_BaseColor");
            color.a = initialAlpha;
            startBubbleMaterial.SetColor("_BaseColor", color);
        }
        if (endBubbleMaterial.HasProperty("_BaseColor")) {
            Color color = endBubbleMaterial.GetColor("_BaseColor");
            color.a = initialAlpha;
            endBubbleMaterial.SetColor("_BaseColor", color);
        }

        for (int i = 0; i < 3; i++) {
            smallBubblesMaterial[i] = FindChildByName(smallBubbles[i].transform, "Bubble").GetComponent<Renderer>().material;

            if (smallBubblesMaterial[i].HasProperty("_BaseColor")) {
                Color color = endBubbleMaterial.GetColor("_BaseColor");
                color.a = initialAlpha;
                smallBubblesMaterial[i].SetColor("_BaseColor", color);
            }
        }

        StartCoroutine(FadeIn(duration, startLifeSpan, endLifeSpan, blockCount));
    }

    private IEnumerator FadeIn(float duration, float startLifeSpan, float endLifeSpan, int blockCount) {
        //yield return new WaitForSeconds(duration);

        float elapsedTime = 0.0f;
        Color startBubbleColor = startBubbleMaterial.GetColor("_BaseColor");
        Color endBubbleColor = endBubbleMaterial.GetColor("_BaseColor");

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            float currentAlpha = Mathf.Clamp01(elapsedTime / duration) * 70/255f;
            startBubbleColor.a = currentAlpha;
            endBubbleColor.a = currentAlpha;
            for (int i = 0; i < 3; i++) {
                Color[] smallBubblesColor = new Color[3];
                smallBubblesColor[i] = smallBubblesMaterial[i].GetColor("_BaseColor");
                smallBubblesColor[i].a = currentAlpha;
                smallBubblesMaterial[i].SetColor("_BaseColor", smallBubblesColor[i]);
            }
            startBubbleMaterial.SetColor("_BaseColor", startBubbleColor);
            endBubbleMaterial.SetColor("_BaseColor", endBubbleColor);

            yield return null;
        }

        // 确保最终透明度是完全不透明
        startBubbleColor.a = 70/255f;
        endBubbleColor.a = 70/255f;
        startBubbleMaterial.SetColor("_BaseColor", startBubbleColor);
        endBubbleMaterial.SetColor("_BaseColor", endBubbleColor);
        startBubbleMaterial.EnableKeyword("_EMISSION");
        endBubbleMaterial.EnableKeyword("_EMISSION");

        float waitTime = 0.0f;
        bool swipeStartHasBroken = false;
        while (waitTime < startLifeSpan) {
            if (isTouched && blockCount == touchedBlockCount) {
                brokenBubbleStartVisual.SetActive(true);
                Destroy(completeBubbleStartVisual);
                swipeStartHasBroken = true;
                StartCoroutine(endBubbleBreakWithDelay(transitTime, brokenTime));
                StartCoroutine(smallBubblesVanish(transitTime));
                break;
            } 
            waitTime += Time.deltaTime;
            yield return null;
        }
        if(!swipeStartHasBroken) {
            Destroy(completeBubbleStartVisual);
            Destroy(completeBubbleEndVisual);
            for (int i = 0; i < 3; i++) {
                Destroy(smallBubbles[i]);
            }
        }

        if (swipeStartHasBroken) {
            yield return new WaitForSeconds(brokenTime);
            Destroy(brokenBubbleStartVisual);
        }
    }
    private IEnumerator smallBubblesVanish(float transitTime) {
        for(int i = 0; i < 3; i++) {
            float smallBubbleWaitTime = (float)((transitTime + brokenTime / 4) * 0.25);
            yield return new WaitForSeconds(smallBubbleWaitTime);
            Destroy(smallBubbles[i]);
        }
    }

    private IEnumerator endBubbleBreakWithDelay(float transitTime, float brokenTime) {
        yield return new WaitForSeconds(transitTime);
        brokenBubbleEndVisual.SetActive(true);
        Destroy(completeBubbleEndVisual);
        Debug.Log(transitTime);

        yield return new WaitForSeconds(brokenTime);
        Destroy(brokenBubbleEndVisual);
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
