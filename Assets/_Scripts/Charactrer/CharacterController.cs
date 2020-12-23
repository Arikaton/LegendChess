using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public CharacterType characterType;
}

public enum CharacterType
{
    Enemy,
    Friend
}
