using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneIntroCutscene : MonoBehaviour
{
    [Header("Player & Camera")]
    public PlayerMovement player;
    public Transform[] lookTargets;
    public float moveSpeed = 2f;           // 카메라 이동 시간
    public float lookDuration = 1.5f;      // 각 lookTarget 체류 시간

    [Header("Fade & Blur")]
    public Image fadeImage;                // 화면 페이드
    public ScreenBlur screenBlur;          // 카메라 블러
    public float fadeDuration = 2f;        // 눈 뜨기 총 시간
    public float blurDuration = 2f;        // 블러 해제 시간

    private bool cutscenePlaying = false;

    private Vector3 originalCamPos;
    private Quaternion originalCamRot;

    void Start()
    {
        if (player == null || player.playerCamera == null)
        {
            Debug.LogError("Player or playerCamera not assigned!");
            return;
        }

        player.enabled = false;
        player.animator.enabled = false;

        // 원래 카메라 위치/회전 저장
        originalCamPos = player.playerCamera.position;
        originalCamRot = player.playerCamera.rotation;

        // 초기 Fade & Blur 세팅
        if (fadeImage != null)
            fadeImage.color = Color.black;

        if (screenBlur != null)
            screenBlur.blurSize = 5f;

        StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
        cutscenePlaying = true;

        //천천히 깜빡이는 눈 뜨기
        if (fadeImage != null)
        {
            int blinkCount = 2;
            float blinkDuration = 2f;
            for (int i = 0; i < blinkCount; i++)
            {
                // 열림: 1 -> 0.6
                float timer = 0f;
                while (timer < blinkDuration)
                {
                    timer += Time.deltaTime;
                    float t = timer / blinkDuration;
                    fadeImage.color = new Color(0, 0, 0, Mathf.Lerp(1f, 0.6f, t));
                    yield return null;
                }
                // 닫힘: 0.6 -> 1
                timer = 0f;
                while (timer < blinkDuration)
                {
                    timer += Time.deltaTime;
                    float t = timer / blinkDuration;
                    fadeImage.color = new Color(0, 0, 0, Mathf.Lerp(0.6f, 1f, t));
                    yield return null;
                }
            }

            // 마지막 완전히 열림: 1 -> 0
            float finalOpenDuration = 1.5f;
            float finalTimer = 0f;
            while (finalTimer < finalOpenDuration)
            {
                finalTimer += Time.deltaTime;
                float t = finalTimer / finalOpenDuration;
                fadeImage.color = new Color(0, 0, 0, Mathf.Lerp(1f, 0f, t));
                yield return null;
            }

            fadeImage.color = new Color(0, 0, 0, 0f);
        }

        //블러 해제
        if (screenBlur != null)
        {
            float timer = 0f;
            float startBlur = screenBlur.blurSize;
            while (timer < blurDuration)
            {
                timer += Time.deltaTime;
                float t = timer / blurDuration;
                screenBlur.blurSize = Mathf.Lerp(startBlur, 0f, t);
                yield return null;
            }
            screenBlur.blurSize = 0f;
        }

        //카메라 lookTargets 순서대로 이동
        foreach (Transform target in lookTargets)
        {
            Vector3 startPos = player.playerCamera.position;
            Quaternion startRot = player.playerCamera.rotation;
            float timer = 0f;

            while (timer < moveSpeed)
            {
                timer += Time.deltaTime;
                float t = timer / moveSpeed;
                player.playerCamera.position = Vector3.Lerp(startPos, target.position, t);
                player.playerCamera.rotation = Quaternion.Slerp(startRot, target.rotation, t);
                yield return null;
            }

            yield return new WaitForSeconds(lookDuration);
        }


        Vector3 camStartPos = player.playerCamera.position;
        Quaternion camStartRot = player.playerCamera.rotation;
        float returnDuration = 1f;
        float returnTimer = 0f;

        while (returnTimer < returnDuration)
        {
            returnTimer += Time.deltaTime;
            float t = returnTimer / returnDuration;
            player.playerCamera.position = Vector3.Lerp(camStartPos, originalCamPos, t);
            player.playerCamera.rotation = Quaternion.Slerp(camStartRot, originalCamRot, t);
            yield return null;
        }

        player.playerCamera.position = originalCamPos;
        player.playerCamera.rotation = originalCamRot;

        player.enabled = true;
        player.animator.enabled = true;
        cutscenePlaying = false;
    }
}
