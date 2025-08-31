using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IPlayerState
{
    private PlayerMovement player;

    public IdleState(PlayerMovement player)
    {
        this.player = player;
        player.animator.Play("Breathing Idle");
        player.animator.SetBool("isWalking", false);
        player.animator.SetBool("isRunning", false);
        player.animator.SetBool("isSittingIdle", false);
    }

    public void KeyInput(Vector3 moveInput, bool runInput, bool sitInput)
    {
        if (sitInput) player.SetState(new SitIdleState(player));
        else if (moveInput.magnitude > 0.1f) player.SetState(new WalkState(player));
    }

    public void Move()
    {
        // 앉아있으면 스테미나 회복 (Idle 상태에서만)
        if (player.isSitting)
            player.stats.RecoverStamina(player.stats.staminaRecoveryRate * Time.deltaTime);
    }
}