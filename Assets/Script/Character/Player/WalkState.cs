using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : IPlayerState
{
    private PlayerMovement player;

    public WalkState(PlayerMovement player)
    {
        this.player = player;
        player.animator.SetBool("isWalking", true);
        player.animator.SetBool("isRunning", false);
        player.animator.SetBool("isSittingIdle", false);
    }

    public void KeyInput(Vector3 moveInput, bool runInput, bool sitInput)
    {
        if (moveInput.magnitude < 0.1f)
            player.SetState(new IdleState(player));
        if (sitInput)
            player.SetState(new SitIdleState(player));
    }

    public void Move()
    {
        float speed = player.walkSpeed;
        bool runInput = Input.GetKey(KeyCode.LeftShift);
        bool canRun = player.stats.currentStamina > 0;

        if (runInput && canRun)
        {
            speed = player.runSpeed;
            player.stats.UseStamina(5f * Time.deltaTime);
            player.stats.UseHunger(2f * Time.deltaTime);
            player.stats.UseThirst(2f * Time.deltaTime);
            player.animator.SetBool("isRunning", true);
            player.animator.SetBool("isWalking", false);
        }
        else
        {
            player.animator.SetBool("isRunning", false);
            player.animator.SetBool("isWalking", true);
        }

        if (player.isSitting) speed = player.sitSpeed;

        // 이동 처리만
        player.MovePlayer(speed);
    }
}
