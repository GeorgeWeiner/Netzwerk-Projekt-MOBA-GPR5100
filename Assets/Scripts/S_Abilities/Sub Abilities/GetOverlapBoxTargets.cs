using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using S_Combat;
using TypeReferences;
using UnityEngine;
using UnityEngine.AI;

namespace S_Abilities
{
    [CreateAssetMenu(menuName = "Sub Abilities/Box Overlap", fileName = "New Box Overlap")]
    public class GetOverlapBoxTargets : SubAbility 
    {
        //https://github.com/SolidAlloy/ClassTypeReference-for-Unity
        [Inherits(typeof(Component), IncludeBaseType = true)]
        [SerializeField] private TypeReference queryType;
        
        public IEnumerable<Component> Targets { get; private set; }
        
        [SerializeField] 
        private float boxWidth, boxHeight, boxLength;
        
        //Type Reference can be used in stead of System.Type.
        public override void ExecuteSubAbility()
        {
            var methodInfo = typeof(GetOverlapBoxTargets).GetMethod(nameof(FindTargets));
            if (methodInfo == null) return;
            
            var method = methodInfo.MakeGenericMethod(queryType);
            method.Invoke(CreateInstance<GetOverlapBoxTargets>(), null);
        }

        //Pass the type of component you want to query the colliders for.
        private IEnumerable<T> FindTargets<T>() where T : Component
        {
            var center = TransformSelf.position + new Vector3(boxWidth, 0f, boxLength);
            var halfExtends = new Vector3(boxWidth, boxHeight, boxLength);
            var colliders = Physics.OverlapBox(center, halfExtends, TransformSelf.rotation);

            var targets = new List<T>();

            foreach (var col in colliders)
            {
                if (col.TryGetComponent(out T component))
                {
                    targets.Add(component);
                    Debug.Log($"Found component! {component.name}");
                }
            }

            return targets.ToArray();
        }
    }
}