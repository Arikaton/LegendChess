using System.Threading.Tasks;
using UnityEngine;


public class CharacterAnimationController : MonoBehaviour
{
    private const string WalkAnimation = "Walk";
    private const string RandomAttackAnimation = "RandomAttack";
    
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void StartWalk()
    {
        _animator.SetBool(WalkAnimation, true);
    }

    public void StopWalk()
    {
        _animator.SetBool(WalkAnimation, false);
    }

    public async void RandomAttack()
    {
        int randomValue = Random.Range(0, 3) + 1;
        _animator.SetInteger(RandomAttackAnimation, randomValue);
        await Task.Delay(100);
        _animator.SetInteger(RandomAttackAnimation, 0);
    }
}
