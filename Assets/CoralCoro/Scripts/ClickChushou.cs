using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ClickChushou : MonoBehaviour
{
    [SerializeField] private float clickChushouZ;
    [SerializeField] private Transform clickChushouVisual;
    [SerializeField] private Transform clickChushouBubble;
    [SerializeField] private Transform clickBubbleForHitPlace;
    [SerializeField] private GameObject clickBubbleVisual;


    //private float promptTime = 4; // 同spawner.hintDuration 2024.7.27
    private float bubbleMovingLocalScale = 0.007f;
    private int musicBlockIndex;
    private float attackingTime = 2;

    private Animator animator;
    private float animationDuration;
    private float delayTime;
    private float targetScale;

    private void Awake() {

        //delayTime = promptTime - attackingTime;
        animator = clickChushouVisual.GetComponent<Animator>();
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        AnimationClip clip = clips[0];
        animationDuration = clip.length;

    }

    public void ClickChuchouGradientScale(float clickScale, float clickMusicBlockLifeSpan) {
        targetScale = clickScale * clickChushouVisual.transform.localScale.x;
        StartCoroutine(ClickScaleOverTime(attackingTime, animationDuration, clickMusicBlockLifeSpan));
    }

    private IEnumerator ClickScaleOverTime(float attackTime, float animDuration, float musicBlockLifeSpan) {
        Vector3 originalScale = clickChushouVisual.transform.localScale;
        Vector3 targetScaleVector = new Vector3(targetScale, targetScale, targetScale);

        bool process = false;

        float bubbleLifeSpan = musicBlockLifeSpan;


        /*GameObject bubbleMoving = Instantiate(clickBubbleVisual, clickChushouBubble);
        bubbleMoving.transform.localPosition = new Vector3(0, 0, 0);
        bubbleMoving.transform.localScale = new Vector3(bubbleMovingLocalScale, bubbleMovingLocalScale, bubbleMovingLocalScale);*/

        // 放大阶段
        float currentTime = 0f;
        while (currentTime < attackTime) {
            currentTime += Time.deltaTime;
            float progress = Mathf.Clamp01(currentTime / attackTime);
            clickChushouVisual.transform.localScale = Vector3.Lerp(originalScale, targetScaleVector, progress);
            /*if(currentTime > 1) {
                bubbleMoving.SetActive(true);
            }*/
            yield return null;
        }

        // 确保完全放大到目标大小
        clickChushouVisual.transform.localScale = targetScaleVector;

        /*Vector3 bubbleForHitPosition = bubbleMoving.transform.position;
        Destroy(bubbleMoving);
        clickBubbleForHitPlace.transform.position = bubbleForHitPosition;
        GameObject bubbleForHit = Instantiate(clickBubbleVisual, clickBubbleForHitPlace);
        bubbleForHit.transform.localPosition = new Vector3(0, 0, 0);
        float bubbleForHitLocalScale = bubbleMoving.transform.lossyScale.x;
        bubbleForHit.transform.localScale = new Vector3(bubbleForHitLocalScale, bubbleForHitLocalScale, bubbleForHitLocalScale);
        bubbleForHit.SetActive(true);*/
        // 缩小阶段
        currentTime = 0f;
        float shrinkTime = (animDuration - attackTime) * 0.5f;

        while (currentTime < shrinkTime) {
            currentTime += Time.deltaTime;
            float progress = Mathf.Clamp01(currentTime / shrinkTime);
            clickChushouVisual.transform.localScale = Vector3.Lerp(targetScaleVector, originalScale, progress);

            /*if (currentTime > bubbleLifeSpan)
            {
                Destroy(bubbleForHit);
            }*/
            yield return null;
        }

        // 确保完全缩小回原始大小
        clickChushouVisual.transform.localScale = originalScale;
        process = true;

        if (process == true) {
            Destroy(gameObject);
            process = false;
        }
    }

    public float GetClickChushouZ() {
        return clickChushouZ;
    }
    /*public float GetDelayTime() {
        return promptTime - attackingTime;
    }*/

    public float GetAttackingTime() {
        return attackingTime;
    }
    public float GetVisualMovement() {
        return clickChushouVisual.transform.localPosition.z;
    }

}
