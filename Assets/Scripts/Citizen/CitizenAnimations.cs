using Internal;
using UnityEngine;

public class CitizenAnimations : MonoBehaviour
{
    private CitizenOpenVariables _citizenVariables;

    private Rigidbody _rb;
    private Animator _animator;
    private enum AnimationState { idle, walk, work };
    private AnimationState currentAnimationState;
    private void Awake()
    {
        Locator.Register<CitizenAnimations>(this);
    }
    private void Start()
    {
        _citizenVariables = Locator.GetObject<CitizenOpenVariables>();
        _animator = _citizenVariables.CitizenAnimator;
        _rb = _citizenVariables.CitizenRB;
    }
    private void Update()
    {
        AnimationControlling();
    }
    private void AnimationControlling()
    {
        if (_rb.velocity.y > 2 || _rb.velocity.z > 2)
            SetAnimationState(AnimationState.walk);
        else SetAnimationState(AnimationState.idle);
    }
    private void SetAnimationState(AnimationState state)
    {
        currentAnimationState = state;
        _animator.SetInteger("state", (int)currentAnimationState);
    }
}
