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
        if (character is null)
        {
            fieldController.TurnOffFields();
            return;
        }
        if (character != _currentCharacter)
            fieldController.TurnOffFields();
        
        _currentCharacter = character;
        fieldController.TurnOnFields(
            character.moveComponent.MaxMoveDistance, 
            character.moveComponent.Position, 
            character.moveComponent.direction);
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
