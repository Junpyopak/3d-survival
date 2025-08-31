using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitIdleState : IPlayerState
{
    private PlayerMovement player;
    private Coroutine sitIdleCoroutine;

    public SitIdleState(PlayerMovement player)
    {
        this.player = player;

        // �ɴ� ���
        player.animator.SetBool("isIdle", false);
        player.animator.SetBool("isWalking", false);
        player.animator.SetBool("isRunning", false);
        player.animator.SetBool("isSittingIdle", true);

        // ĳ���� ��Ʈ�ѷ� �ɱ�
        player.Sit();
        player.isSitting = true;

        // ���� Coroutine ���
        if (sitIdleCoroutine != null)
            player.StopCoroutine(sitIdleCoroutine);

        // SitDown ������ SitIdleLoop�� ��ȯ
        sitIdleCoroutine = player.StartCoroutine(WaitAndGoSitIdleLoop(2f));
    }

    public void KeyInput(Vector3 moveInput, bool runInput, bool sitInput)
    {
        if (sitInput) // �Ͼ��
        {
            // SitIdleLoop ����
            player.animator.SetBool("isSittingLoop", false);

            // ���� ���� Coroutine ���
            if (sitIdleCoroutine != null)
                player.StopCoroutine(sitIdleCoroutine);

            // �Ͼ�� �ִϸ��̼�
            player.animator.SetBool("isSittingIdle", false);
            player.animator.Play("StandUp");

            // ĳ���� ��Ʈ�ѷ� �Ͼ��
            player.Stand();
            player.isSitting = false;

            // StandUp ������ Idle ���·� ��ȯ
            player.StartCoroutine(WaitAndGoIdle(1f));
        }
    }

    public void Move()
    {
        // �ɾ������� ���׹̳� ȸ�� (�ִ� 100����)
        if (player.stats.currentStamina < player.stats.maxStamina)
        {
            player.stats.RecoverStamina(player.stats.staminaRecoveryRate * Time.deltaTime);
        }
    }


    private IEnumerator WaitAndGoIdle(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        player.SetState(new IdleState(player));
    }


    private IEnumerator WaitAndGoSitIdleLoop(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        // SitIdleLoop �ִϸ��̼�
        player.animator.SetBool("isSittingIdle", false);
        player.animator.SetBool("isSittingLoop", true);

        player.isSitting = true; // ī�޶� ������ Ȱ��ȭ
    }
}
