using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneIntroCutscene : MonoBehaviour
{
    [Header("Player & Camera")]
    public PlayerMovement player;
    public Transform[] lookTargets;
    public float moveSpeed = 2f;           // ī�޶� �̵� �ð�
    public float lookDuration = 1.5f;      // �� lookTarget ü�� �ð�

    [Header("Fade & Blur")]
    public Image fadeImage;                // ȭ�� ���̵�
    public ScreenBlur screenBlur;          // ī�޶� ��
    public float fadeDuration = 2f;        // �� �߱� �� �ð�
    public float blurDuration = 2f;        // �� ���� �ð�

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

        // ���� ī�޶� ��ġ/ȸ�� ����
        originalCamPos = player.playerCamera.position;
        originalCamRot = player.playerCamera.rotation;

        // �ʱ� Fade & Blur ����
        if (fadeImage != null)
            fadeImage.color = Color.black;

        if (screenBlur != null)
            screenBlur.blurSize = 5f;

        StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
        cutscenePlaying = true;

        //õõ�� �����̴� �� �߱�
        if (fadeImage != null)
        {
            int blinkCount = 2;
            float blinkDuration = 2f;
            for (int i = 0; i < blinkCount; i++)
            {
                // ����: 1 -> 0.6
                float timer = 0f;
                while (timer < blinkDuration)
                {
                    timer += Time.deltaTime;
                    float t = timer / blinkDuration;
                    fadeImage.color = new Color(0, 0, 0, Mathf.Lerp(1f, 0.6f, t));
                    yield return null;
                }
                // ����: 0.6 -> 1
                timer = 0f;
                while (timer < blinkDuration)
                {
                    timer += Time.deltaTime;
                    float t = timer / blinkDuration;
                    fadeImage.color = new Color(0, 0, 0, Mathf.Lerp(0.6f, 1f, t));
                    yield return null;
                }
            }

            // ������ ������ ����: 1 -> 0
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

        //�� ����
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

        //ī�޶� lookTargets ������� �̵�
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
