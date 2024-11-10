using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SwipeChushou : MonoBehaviour{
    [SerializeField] public Transform swipeChushouVisual;

    //public float promptTime = 4; // ͬspawner.hintDuration 2024.7.27

    private float attackingTime;
    private float transitionTime;

    private Animator animator;
    public float swipeSpeed = 0.5f;
    private float swipeRatio;

    private float animationDuration;
    private float targetScaleStart;
    private float targetScaleEnd;

    private void Awake() {
    }

    public void SwipeChuchouGradientScale(float swipeRatio, float swipeScaleStart, float swipeScaleEnd, float swipeMusicBlockStartLifeSpan, float swipeMusicBlockEndLifeSpan) {
        animator = swipeChushouVisual.GetComponent<Animator>();
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;

        attackingTime = clips[0].length + clips[1].length * (1 / swipeSpeed) * ((1 - swipeRatio) / 2.0f);
        transitionTime = clips[1].length * (1 / swipeSpeed) * swipeRatio;

        animationDuration = clips[0].length + clips[1].length * (1 / swipeSpeed) + clips[2].length;

        targetScaleStart = swipeScaleStart * swipeChushouVisual.transform.localScale.x;
        targetScaleEnd = swipeScaleEnd * swipeChushouVisual.transform.localScale.x;
        StartCoroutine(SwipeScaleOverTime(attackingTime, transitionTime, animationDuration, swipeMusicBlockStartLifeSpan, swipeMusicBlockEndLifeSpan));
    }

    private IEnumerator SwipeScaleOverTime(float attackTime, float transitTime, float animDuration, float musicBlockStartLifeSpan, float swipeMusicBlockEndLifeSpan) {
        Vector3 originalScale = swipeChushouVisual.transform.localScale;
        Vector3 targetScaleStartVector = new Vector3(targetScaleStart, targetScaleStart, targetScaleStart);
        Vector3 targetScaleEndVector = new Vector3(targetScaleEnd, targetScaleEnd, targetScaleEnd);

        bool process = false;

        // �Ŵ�׶�
        float currentTime = 0f;
        while (currentTime < attackTime) {
            currentTime += Time.deltaTime;
            float progress = Mathf.Clamp01(currentTime / attackTime);
            swipeChushouVisual.transform.localScale = Vector3.Lerp(originalScale, targetScaleStartVector, progress);

            yield return null;
        }

        swipeChushouVisual.transform.localScale = targetScaleStartVector;

        //���ɽ׶�
        currentTime = 0f;
        while (currentTime < transitTime) {
            currentTime += Time.deltaTime;
            float progress = Mathf.Clamp01(currentTime / transitTime);
            swipeChushouVisual.transform.localScale = Vector3.Lerp(targetScaleStartVector, targetScaleEndVector, progress);

            yield return null;
        }

        swipeChushouVisual.transform.localScale = targetScaleEndVector;

        // ��С�׶�
        currentTime = 0f;
        float shrinkTime = (animDuration - attackTime - transitTime) * 0.5f;

        while (currentTime < shrinkTime) {
            currentTime += Time.deltaTime;
            float progress = Mathf.Clamp01(currentTime / shrinkTime);
            swipeChushouVisual.transform.localScale = Vector3.Lerp(targetScaleEndVector, originalScale, progress);

            yield return null;
        }

        swipeChushouVisual.transform.localScale = originalScale;
        process = true;

        if (process == true) {
            Destroy(gameObject);
            process = false;
        }
    }

}
