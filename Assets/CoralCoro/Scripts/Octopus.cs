using System.Collections;
using System.Collections.Generic;

//using System.Numerics;
using UnityEngine;
using UnityEngine.XR;

public class Octopus : MonoBehaviour {

    [SerializeField] private Spawner spawner;
    [SerializeField] private Transform fog;

    [SerializeField] private ClickChushou clickChushouPrefab;
    [SerializeField] private SwipeChushou swipeChushouPrefab;
    [SerializeField] private ClickBubble clickBubblePrefab;
    [SerializeField] private SwipeBubble swipeBubblePrefab;

    [SerializeField] private Transform clickChushouSet;
    [SerializeField] private Transform swipeChushouSet;
    [SerializeField] private Transform bubbleSet;

    //[SerializeField] private GameObject HintClickBubbleVisual;

    private float bubblePromptDuration;
    private float musicBlockAverageHeight;
    private float clickMusicBlockLifeSpan;
    private float clickChushouZ;
    private float clickScale;
    private float swipeScaleStart;
    private float swipeScaleEnd;
    private float swipeRatio;

    //private float swipeAttackingDistance = (float)(19.0 / 16.0);

    int mirror = 1;

    private float swipeMusicBlockStartLifeSpan;
    private float swipeMusicBlockEndLifeSpan;

    private void Awake() {
        bubblePromptDuration = spawner.hintDuration;
    }
    private void Start() {

        spawner.OnClickChushouEmerge += Spawner_OnClickChushouEmerge;
        clickChushouZ = clickChushouPrefab.GetClickChushouZ();
        fog.transform.position = new Vector3(0, 0, clickChushouZ * 2.5f / 3f);//qyj modify 2/3 to 2.5/3

        spawner.OnSwipeChushouEmerge += Spawner_OnSwipeChushouEmerge;
    }

    //-------------------------------------------------Swipe-------------------------------------------------------------//
    //-------------------------------------------------Swipe-------------------------------------------------------------//
    private void Spawner_OnSwipeChushouEmerge(object sender, Spawner.OnSwipeChushouEmergeEventArgs e) {
        //Debug.Log("Swipe!");
        swipeMusicBlockStartLifeSpan = spawner.musicBlocks[e.swipeMusicBlockStartIndex].lifespan;
        swipeMusicBlockEndLifeSpan = spawner.musicBlocks[e.swipeMusicBlockEndIndex].lifespan;

        Vector3 swipeCubeMidPosition = new Vector3((float)((e.swipeCubeStartPosition.x + e.swipeCubeEndPosition.x) / 2.0), (float)((e.swipeCubeStartPosition.y + e.swipeCubeEndPosition.y) / 2.0), (float)((e.swipeCubeStartPosition.z + e.swipeCubeEndPosition.z) / 2.0));
        swipeScaleStart = FindDistance(FindSwipeChushouPosition(swipeCubeMidPosition), e.swipeCubeStartPosition);
        swipeScaleEnd = FindDistance(FindSwipeChushouPosition(swipeCubeMidPosition), e.swipeCubeEndPosition);
        swipeRatio = FindDistance(e.swipeCubeStartPosition, new Vector3(swipeCubeMidPosition.x, e.swipeCubeStartPosition.y, swipeCubeMidPosition.z)) * 2 / swipeScaleStart / (11 / 8.0f);
        //Debug.Log(swipeRatio);

        StartCoroutine(SpawnBubbleSwipe(e.blockCount,e.swipeCubeStartPosition, e.swipeCubeEndPosition, swipeMusicBlockStartLifeSpan, swipeMusicBlockEndLifeSpan));
        StartCoroutine(SpawnAttackingSwipeChushouWithDelay(swipeRatio, GetSwipeDelayTime(), e.swipeCubeStartPosition, swipeMusicBlockStartLifeSpan, e.swipeCubeEndPosition, swipeMusicBlockEndLifeSpan));
    }
    private IEnumerator SpawnBubbleSwipe(int blockCount, Vector3 swipeCubeStartPosition, Vector3 swipeCubeEndPosition, float swipeMusicBlockStartLifeSpan, float swipeMusicBlockEndLifeSpan) {

        SwipeBubble swipeBubble = Instantiate(swipeBubblePrefab, bubbleSet);
        swipeBubble.completeBubbleStartVisual.transform.position = swipeCubeStartPosition;
        swipeBubble.completeBubbleEndVisual.transform.position = swipeCubeEndPosition;
        swipeBubble.brokenBubbleStartVisual.transform.position = swipeCubeStartPosition;
        swipeBubble.brokenBubbleEndVisual.transform.position = swipeCubeEndPosition;

        for (int i = 0; i < 3; i++) {
            swipeBubble.smallBubbles[i].transform.position = CalculateQuartilePoints(swipeCubeStartPosition, swipeCubeEndPosition)[i];
        }

        swipeBubble.StartFadeIn(bubblePromptDuration, swipeMusicBlockStartLifeSpan, swipeMusicBlockEndLifeSpan, blockCount);

        yield return null;
    }

    private IEnumerator SpawnAttackingSwipeChushouWithDelay(float swipeRatio, float swipeDelayTime, Vector3 swipeCubeStartPosition, float swipeMusicBlockStartLifeSpan, Vector3 swipeCubeEndPosition, float swipeMusicBlockEndLifeSpan) {

        yield return new WaitForSeconds(swipeDelayTime);
        AttackSwipe(swipeRatio, swipeCubeStartPosition, swipeMusicBlockStartLifeSpan, swipeCubeEndPosition, swipeMusicBlockEndLifeSpan);
    }

    private void AttackSwipe(float swipeRatio, Vector3 swipeCubeStartPosition, float swipeMusicBlockStartLifeSpan, Vector3 swipeCubeEndPosition, float swipeMusicBlockEndLifeSpan) {
        Vector3 swipeCubeMidPosition = new Vector3((float)((swipeCubeStartPosition.x + swipeCubeEndPosition.x) / 2.0), (float)((swipeCubeStartPosition.y + swipeCubeEndPosition.y) / 2.0), (float)((swipeCubeStartPosition.z + swipeCubeEndPosition.z) / 2.0));

        SwipeChushou attackingSwipeChushou = Instantiate(swipeChushouPrefab, swipeChushouSet);
        attackingSwipeChushou.transform.position = FindSwipeChushouPosition(swipeCubeMidPosition);

        int right = 0;
        if (swipeCubeMidPosition.x >= 0) {
            right = 1;
        } else {
            right = -1;
        }

        attackingSwipeChushou.transform.RotateAround(FindSwipeChushouPosition(swipeCubeMidPosition), FindVector2Pt(FindSwipeChushouPosition(swipeCubeMidPosition), swipeCubeMidPosition), FindSwipeChushouDegreeRotate(swipeCubeStartPosition, swipeCubeEndPosition, right));
        attackingSwipeChushou.transform.localScale = new Vector3 (mirror, 1, 1);

        attackingSwipeChushou.SwipeChuchouGradientScale(swipeRatio, swipeScaleStart, swipeScaleEnd, swipeMusicBlockStartLifeSpan, swipeMusicBlockEndLifeSpan);
    }

    private Vector3 FindSwipeChushouPosition(Vector3 swipeCubeMidPosition) {
        Vector3 swipeChushouPosition = new Vector3(swipeCubeMidPosition.x, swipeCubeMidPosition.y, clickChushouZ);
        return swipeChushouPosition;
    }

    private float FindSwipeChushouDegreeRotate(Vector3 swipeCubeStartPosition, Vector3 swipeCubeEndPosition, int right) {
        Vector3 vector1 = -FindVector2Pt(swipeCubeStartPosition, new Vector3(swipeCubeEndPosition.x, swipeCubeEndPosition.y, swipeCubeStartPosition.z)).normalized;
        Vector3 vector2 = new Vector3(0, 1, 0);
        float angle = Vector3.Angle(vector1, vector2) ;

        Vector3 normal = Vector3.Cross(vector1, vector2);
        angle *= Mathf.Sign(Vector3.Dot(normal, new Vector3(0, 0, 1)));
        if(90 - angle < 0) {
            angle = 90 - angle + 360;
        } else {
            angle = 90 - angle;
        }

        if(right == 1 && angle < 180) {
            angle += 180;
            mirror = -1;
        }else if(right == -1 && angle > 180) {
            angle -= 180;
            mirror = -1;
        } else {
            mirror = 1;
        }

        return angle;
    }

//-------------------------------------------------Click-------------------------------------------------------------//
//-------------------------------------------------Click-------------------------------------------------------------//
    private void Spawner_OnClickChushouEmerge(object sender, Spawner.OnClickChushouEmergeEventArgs e) {
        clickMusicBlockLifeSpan = spawner.musicBlocks[e.clickMusicBlockIndex].lifespan;
        //SpawnHintClickBubble(e.clickCubeEndPosition, clickMusicBlockLifeSpan);

        StartCoroutine(SpawnBubbleClick(e.blockCount, e.clickCubeEndPosition, clickMusicBlockLifeSpan));
        StartCoroutine(SpawnAttackingClickChushouWithDelay(bubblePromptDuration - clickChushouPrefab.GetAttackingTime(), e.clickCubeEndPosition, clickMusicBlockLifeSpan));
    }
    private IEnumerator SpawnBubbleClick(int blockCount, Vector3 clickCubeEndPosition, float clickMusicBlockLifeSpan) {

        ClickBubble bubbleClick = Instantiate(clickBubblePrefab, bubbleSet);
        bubbleClick.transform.position = clickCubeEndPosition;
        bubbleClick.StartFadeIn(bubblePromptDuration, clickMusicBlockLifeSpan, blockCount);

        yield return null;
    }

    private IEnumerator SpawnAttackingClickChushouWithDelay(float clickDelayTime, Vector3 clickCubeEndPosition, float clickMusicBlockLifeSpan) {

        yield return new WaitForSeconds(clickDelayTime);
        AttackClick(clickCubeEndPosition, clickMusicBlockLifeSpan);
    }
    private void AttackClick(Vector3 clickCubeEndPosition, float clickMusicBlockLifeSpan) {

        ClickChushou attackingClickChushou = Instantiate(clickChushouPrefab, clickChushouSet);
        clickScale = FindDistance(FindClickChushouPosition(clickCubeEndPosition), clickCubeEndPosition);
        attackingClickChushou.transform.position = FindClickChushouPosition(clickCubeEndPosition);

        attackingClickChushou.transform.rotation = Quaternion.Euler(FindClickChushouDegreeX(clickCubeEndPosition), 0, 0);
        attackingClickChushou.transform.RotateAround(FindClickChushouPosition(clickCubeEndPosition), FindVector2Pt(FindClickChushouPosition(clickCubeEndPosition), clickCubeEndPosition), FindClickChushouDegreeRotate(clickCubeEndPosition));

        attackingClickChushou.ClickChuchouGradientScale(clickScale, clickMusicBlockLifeSpan);
    }


    private float FindClickChushouDegreeX(Vector3 clickCubeEndPosition) {
        float attackingClickDistance = FindDistance(FindClickChushouPosition(clickCubeEndPosition), clickCubeEndPosition);
        float clickHeightDifference = clickCubeEndPosition.y - GetAverageHeight();
        float clickSinValue = clickHeightDifference / attackingClickDistance;
        float degreeX = Mathf.Asin(clickSinValue) * 180 / Mathf.PI;
        return degreeX;
    }
    private float FindClickChushouDegreeRotate(Vector3 clickCubeEndPosition) {
        float clickDegreeRotate = 0;
        if (Mathf.Abs(clickCubeEndPosition.x) > GetRadius() * 0.8) {
            clickDegreeRotate = 16f * clickCubeEndPosition.x / Mathf.Abs(clickCubeEndPosition.x) * -1;
        } else if (Mathf.Abs(clickCubeEndPosition.x) > GetRadius() * 0.5) {
            clickDegreeRotate = 12f * clickCubeEndPosition.x / Mathf.Abs(clickCubeEndPosition.x) * -1;
        } else {
            clickDegreeRotate = 8f * clickCubeEndPosition.x / Mathf.Abs(clickCubeEndPosition.x) * -1;
        }

        if (clickCubeEndPosition.y < GetAverageHeight() & Mathf.Abs(clickCubeEndPosition.x) > GetRadius() * 0.8) {
            clickDegreeRotate += clickDegreeRotate / Mathf.Abs(clickDegreeRotate) * 120;
        } else if (clickCubeEndPosition.y < GetAverageHeight() & Mathf.Abs(clickCubeEndPosition.x) > GetRadius() * 0.5) {
            clickDegreeRotate += clickDegreeRotate / Mathf.Abs(clickDegreeRotate) * 20;
        }
        return clickDegreeRotate;
    }
    private Vector3 FindVector2Pt(Vector3 PtA, Vector3 PtB) {
        return new Vector3(PtA.x - PtB.x, PtA.y - PtB.y, PtA.z - PtB.z);
    }

    private float FindDistance(Vector3 P_1, Vector3 P_2) {
        float distance = Mathf.Sqrt(Mathf.Pow((P_1.x - P_2.x), 2) + Mathf.Pow((P_1.y - P_2.y), 2) + Mathf.Pow((P_1.z - P_2.z), 2));
        return distance;
    }

    private Vector3 FindClickChushouPosition(Vector3 cubeEndPosition) {
        return new Vector3(cubeEndPosition.x, GetAverageHeight(), clickChushouZ);
    }

    private float GetAverageHeight() {
        musicBlockAverageHeight = MusicBlock.origin.y;
        return musicBlockAverageHeight;
    }
    private float GetRadius() {
        return MusicBlock.radius;
    }

    private float GetSwipeDelayTime() {
        Animator animator = swipeChushouPrefab.swipeChushouVisual.GetComponent<Animator>();
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        float attackingTime = clips[0].length + clips[1].length * (1 / swipeChushouPrefab.swipeSpeed) * ((1 - swipeRatio) / 2.0f);
        return bubblePromptDuration - attackingTime;
    }
    public static Vector3[] CalculateQuartilePoints(Vector3 pointA, Vector3 pointB) {
        Vector3[] quartilePoints = new Vector3[3];
        quartilePoints[0] = Vector3.Lerp(pointA, pointB, 2/6f);
        quartilePoints[1] = Vector3.Lerp(pointA, pointB, 3/6f);
        quartilePoints[2] = Vector3.Lerp(pointA, pointB, 4/6f);

        return quartilePoints;
    }
}
