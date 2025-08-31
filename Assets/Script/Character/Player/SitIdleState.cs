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

        // 앉는 모션
        player.animator.SetBool("isIdle", false);
        player.animator.SetBool("isWalking", false);
        player.animator.SetBool("isRunning", false);
        player.animator.SetBool("isSittingIdle", true);

        // 캐릭터 컨트롤러 앉기
        player.Sit();
        player.isSitting = true;

        // 기존 Coroutine 취소
        if (sitIdleCoroutine != null)
            player.StopCoroutine(sitIdleCoroutine);

        // SitDown 끝나면 SitIdleLoop로 전환
        sitIdleCoroutine = player.StartCoroutine(WaitAndGoSitIdleLoop(2f));
    }

    public void KeyInput(Vector3 moveInput, bool runInput, bool sitInput)
    {
        if (sitInput) // 일어나기
        {
            // SitIdleLoop 종료
            player.animator.SetBool("isSittingLoop", false);

            // 진행 중인 Coroutine 취소
            if (sitIdleCoroutine != null)
                player.StopCoroutine(sitIdleCoroutine);

            // 일어나는 애니메이션
            player.animator.SetBool("isSittingIdle", false);
            player.animator.Play("StandUp");

            // 캐릭터 컨트롤러 일어서기
            player.Stand();
            player.isSitting = false;

            // StandUp 끝나면 Idle 상태로 전환
            player.StartCoroutine(WaitAndGoIdle(1f));
        }
    }

    public void Move()
    {
        // 앉아있으면 스테미나 회복 (최대 100까지)
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

        // SitIdleLoop 애니메이션
        player.animator.SetBool("isSittingIdle", false);
        player.animator.SetBool("isSittingLoop", true);

        player.isSitting = true; // 카메라 숨쉬기 활성화
    }
}
