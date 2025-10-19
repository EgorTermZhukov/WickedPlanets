using System.Collections;
using Game.Src.Gameplay;
using Game.Src.Tags;
using TMPro;
using UnityEngine;

namespace Game.Src.General
{
    public class EnemyState
    {
        public CMSEntity EnemyModel;
        public int EnemyHp;
        public int EnemyDamage;
        public EnemyView EnemyView;

        public void SetState(CMSEntity model)
        {
            EnemyHp = model.Get<TagHp>().HP;
            EnemyDamage = model.Get<TagEnemyBaseDamage>().Damage;
        }
    }
    public class PlayerState
    {
        public int PlayerHp;
        public int AvaliableTurns;
    }
    public class EncounterState
    {
        public CMSEntity Model;
        public EnemyState EnemyState;
        public PlayerState PlayerState;
        public int Round;
    }
    public class EncounterManager : MonoBehaviour
    {
        public TMP_Text PlayerHPText;
        public TMP_Text TurnsText;
        public TMP_Text EnemyDamageText;
        public TMP_Text EnemyHPText;
        
        
        public EncounterState State;
        public OrbitManager Orbit;
        
        public IEnumerator Setup(EncounterState state, OrbitManager orbit)
        {
            G.Main.ShowCombatUI();
            
            var enemypfb = state.EnemyState.EnemyModel.Get<TagEnemyView>();
            var enemyView = Instantiate(enemypfb.View);
            
            state.EnemyState.EnemyView = enemyView;
            
            var orbitManager = Instantiate(orbit);
            Orbit = orbitManager;
            G.Main.Orbits = Orbit;
            
            State = state;
            UpdateEverything();
            yield break;
        }
        public void UpdateEverything()
        {
            PlayerHPText.text = State.PlayerState.PlayerHp.ToString();
            EnemyHPText.text = State.EnemyState.EnemyHp.ToString();
            TurnsText.text = State.PlayerState.AvaliableTurns.ToString();
            EnemyDamageText.text = State.EnemyState.EnemyDamage.ToString();
        }
        public IEnumerator StartEncounter()
        {
            yield break;
        }
        public IEnumerator FinishEncounter()
        {
            yield break;
        }
    }
}