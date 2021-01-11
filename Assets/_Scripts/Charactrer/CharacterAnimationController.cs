using System.Collections;
using System.Threading.Tasks;
using UnityEngine;


public class CharacterAnimationController : MonoBehaviour
{
    private const string WalkAnimation = "Walk";
    private const string RandomAttackAnimation = "RandomAttack";
    private const string DamageAnimation = "Damage";
    private const string DeathAnimation = "Death";
    
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

    public void RandomAttack()
    {
        StartCoroutine(RandomAttackCor());
    }

    public void Death()
    {
        _animator.SetTrigger(DeathAnimation);
    }

    public void GetDamage()
    {
        _animator.SetTrigger(DamageAnimation);
    }

    private IEnumerator RandomAttackCor()
    {
        int randomValue = Random.Range(0, 3) + 1;
        _animator.SetInteger(RandomAttackAnimation, randomValue);
        yield return new WaitForSeconds(0.1f);
        _animator.SetInteger(RandomAttackAnimation, 0);
    }
}
