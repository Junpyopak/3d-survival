using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : IPlayerState
{
    private PlayerMovement player;

    public RunState(PlayerMovement player)
    {
        this.player = player;
        player.animator.SetBool("isIdle", false);
        player.animator.SetBool("isWalking", false);
        player.animator.SetBool("isRunning", true);
        player.animator.SetBool("isSittingIdle", false);
    }

    public void KeyInput(Vector3 moveInput, bool runInput, bool sitInput)
    {
        if (!runInput) player.SetState(new WalkState(player));
        else if (sitInput) player.SetState(new SitIdleState(player));
    }

    public void Move()
    {
        player.MovePlayer(player.runSpeed);
    }
}
