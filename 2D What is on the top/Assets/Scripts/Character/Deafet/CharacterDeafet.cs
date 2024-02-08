using System;

namespace Character.Deafet
{
    public class CharacterDeafet 
    {
        public event Action CharacterDead;

        public void CharacterDefeat() => CharacterDead?.Invoke();
    }
}