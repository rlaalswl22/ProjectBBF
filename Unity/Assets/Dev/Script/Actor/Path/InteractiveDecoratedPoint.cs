using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using ProjectBBF.Event;
using UnityEngine;

    public class InteractiveDecoratedPoint : MonoBehaviour, IBObjectDecoratedPoint
    {
        [SerializeField] private CollisionInteraction _interaction;
        [SerializeField] private Vector2 _interactingPivot;
        [SerializeField] private float _waitDuration;


        public Vector2 InteractingPositionWorld
        {
            get => transform.TransformPoint(InteractingPositionLocal);
            set=> InteractingPositionLocal = transform.InverseTransformPoint(value);
        }

        private Vector2 InteractingPositionLocal
        {
            get => _interactingPivot;
            set => _interactingPivot = value;
        }
        
        public CollisionInteraction Interaction => _interaction;
        public async UniTask<IBObjectDecoratedPoint> Interact(CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_waitDuration), DelayType.DeltaTime, PlayerLoopTiming.Update, token);

            return this;
        }

        private void Awake()
        {
            var info = ObjectContractInfo.Create(()=>gameObject);
            Interaction.SetContractInfo(info, this);

            info.AddBehaivour<IBObjectDecoratedPoint>(this);
        }
    }
