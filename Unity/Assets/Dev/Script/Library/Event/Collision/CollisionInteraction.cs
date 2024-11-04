using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBBF.Event
{
    
    public class CollisionInteraction : CollisionInteractionMono
    {
        public override object Owner { get; internal set; }
        public override event Action<BaseContractInfo> OnContract;
        public override event Action<BaseContractInfo> OnExit;

        public override LayerMask TargetLayerMask => _targetLayerMask;
        public override bool ListeningOnly => _listeningOnly;
        public override bool DetectedOnly => _detectedOnly;
        public override BaseContractInfo ContractInfo { get; internal set; }

        [SerializeField] private bool _isBindChildProxy = true;
        [SerializeField] private LayerMask _targetLayerMask;
        [SerializeField] private bool _listeningOnly;
        [SerializeField] private bool _detectedOnly;

        private List<CollisionInteractionProxy> _proxies = new();
        public IReadOnlyCollection<CollisionInteractionProxy> Proxies => _proxies;
        
        public void SetContractInfo(BaseContractInfo info, object owner)
        {
            Owner = owner;
            ContractInfo = info;
            info._interaction = this;
        }

        public override bool IsEnabled
        {
            get => enabled;
            set => enabled = value;
        }

        public override T GetContractInfoOrNull<T>()
            => ContractInfo as T;

        public override bool TryGetContractInfo<T>(out T info)
        {
            info = GetContractInfoOrNull<T>();
            return info is not null;
        }

        public override void Activate(BaseContractInfo info)
        {
            Debug.Assert(info != null, "ContractInfo can't be null");
            
            if (info is ObjectContractInfo objectContractInfo)
            {
                OnContract?.Invoke(objectContractInfo);
            }
        }

        public override void DeActivate(BaseContractInfo info)
        {
            Debug.Assert(info != null, "ContractInfo can't be null");
            if (info is ObjectContractInfo objectContractInfo)
            {
                OnExit?.Invoke(objectContractInfo);
            }
        }

        public override void ClearContractEvent()
        {
            OnContract = null;
            OnExit = null;
        }

        private CollisionBridge _collisionBridge;

        private void Awake()
        {
            if (_isBindChildProxy)
            {
                InitProxies(transform);
            }
        }

        private void InitProxies(Transform parent)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                var child = parent.GetChild(i);

                if (child.GetComponent<CollisionInteraction>())
                {
                    continue;
                }
                
                if (child.TryGetComponent(out CollisionInteractionProxy proxy))
                {
                    AddProxy(proxy);
                }
                
                InitProxies(child);
            }
        }

        public void AddProxy(CollisionInteractionProxy proxy)
        {
            _proxies.Add(proxy);
            proxy.MainInteraction = this;
        }

        public void AddProxyRange(params CollisionInteractionProxy[] proxies)
        {
            if (proxies.Length == 0) return;
            
            _proxies.AddRange(proxies);
            foreach (var proxy in proxies)
            {
                proxy.MainInteraction = this;
            }
        }
        
        public bool RemoveProxy(CollisionInteractionProxy proxy)
        {
            proxy.MainInteraction = null;
            return _proxies.Remove(proxy);
        }

        private void Start()
        {
            _collisionBridge = Singleton.Singleton.GetSingleton<EventController>().GetBridge<CollisionBridge>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (CollisionInteractionUtil.OnCollision(other.collider, this, true, out var com))
            {
                _collisionBridge.Push(this, com);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (CollisionInteractionUtil.OnCollision(other, this, true, out var com))
            {
                _collisionBridge.Push(this, com);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            CollisionInteractionUtil.OnCollision(other, this, false, out var com);
        }
        private void OnCollisionExit2D(Collision2D other)
        {
            CollisionInteractionUtil.OnCollision(other.collider, this, false, out var com);
        }
    }
}