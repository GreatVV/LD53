using Leopotam.Ecs;
using UnityEngine;

namespace LD52
{
    internal class ShowArrowToTargetSystem : IEcsRunSystem
    {
        private RuntimeData _runtimeData;
        private SceneData _sceneData;
        public void Run()
        {
            if (_runtimeData.PlayerCharacter)
            {
                var quester = _runtimeData.PlayerCharacter.GetComponent<Quester>();
                var targetPosition = Vector3.zero;
                if (quester && quester.TakenQuests.Count > 0)
                {
                    var firstQuest = quester.TakenQuests[0];
                    if (firstQuest.QuestState == QuestState.NeedItem)
                    {
                        targetPosition = _runtimeData.Runner.FindObject(firstQuest.From).transform.position;
                    }
                    else
                    {
                        targetPosition = _runtimeData.Runner.FindObject(firstQuest.To).transform.position;
                    }
                }
                else
                {
                    targetPosition = _sceneData.QuestManager.transform.position;
                    
                }

                _sceneData.BeamParticle.transform.position = targetPosition;
                _sceneData.BeamParticle.SetActive(true);
                targetPosition.y = _runtimeData.PlayerCharacter.Arrow.transform.position.y;
                var distance = Vector3.Distance(targetPosition, _runtimeData.PlayerCharacter.Arrow.transform.position);
                _runtimeData.PlayerCharacter.Arrow.DistanceLabel.text = $"{distance:F1}m";
                
                _runtimeData.PlayerCharacter.Arrow.transform.LookAt(targetPosition, Vector3.up);
            }
        }
    }
}