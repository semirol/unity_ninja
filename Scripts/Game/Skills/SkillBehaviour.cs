using Core;
using UnityEngine;
using Utils;

namespace Game.Skills
{
    public class SkillBehaviour : MonoBehaviour
    {
        protected BattleOperation BattleOperation;

        protected Rigidbody Rigidbody;
        
        public virtual void Init(BattleOperation battleOperation)
        {
            Rigidbody = GetComponent<Rigidbody>();
            BattleOperation = battleOperation;
        }
        
        protected string GetEnemyName()
        {
            string prefix = gameObject.name.Substring(0, 3);
            if (prefix == "Ene")
            {
                return "Player";
            }
            return "Enemy";
        }
        
        protected string GetPlayerName()
        {
            string prefix = gameObject.name.Substring(0, 3);
            if (prefix == "Ene")
            {
                return "Enemy";
            }
            return "Player";
        }
        
    }
}