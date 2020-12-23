using System.Threading.Tasks;
using UnityEngine;


public class CharacterAnimationController : MonoBehaviour
{
    private const string Walk_Animation = "Walk";
    private const string Random_Attack_Animation = "RandomAttack";
    
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void StartWalkAnimation()
    {
        _animator.SetBool(Walk_Animation, true);
    }

    public void StopMoveAnimation()
    {
        _animator.SetBool(Walk_Animation, false);
    }

    public async void RandomAttack()
    {
        int randomValue = Random.Range(0, 3) + 1;
        _animator.SetInteger(Random_Attack_Animation, randomValue);
        await Task.Delay(100);
        _animator.SetInteger(Random_Attack_Animation, 0);
    }
}
