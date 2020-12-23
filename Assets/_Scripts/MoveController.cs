using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MoveController : MonoBehaviour
{
    [SerializeField] private FieldController fieldController;

    private CharacterDataContainer _currentCharacter;
    
    public void OnTapToCharacter(CharacterDataContainer character)
    {
        _currentCharacter = character;
        if (character is null)
        {
            fieldController.TurnOffFields();
            return;
        }
        fieldController.TurnOnFields(
            character.moveComponent.MaxMoveDistance, 
            character.moveComponent.Position, 
            character.moveComponent.moveType);
    }

    public void OnTapToField(Field field)
    {
        if (_currentCharacter != null)
        {
            if (fieldController.ActiveFields.Contains(field))
            {
                _currentCharacter.moveComponent.DoMove(field.Position);
                fieldController.TurnOffFields();
            }
        }
    }
}
