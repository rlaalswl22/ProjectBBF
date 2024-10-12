using System.Collections;
using System.Collections.Generic;
using ProjectBBF.Event;
using UnityEngine;

[CreateAssetMenu(menuName = "ProjectBBF/Event/LyllaPath", fileName = "New LyllaPath")]
public class ESOLyllaPath : ESOVoid
{
    [SerializeField] private GameObject _patrolPointPathGameObject;
    
    private PatrolPointPath _patrolPointPath;

    public PatrolPointPath PatrolPointPath
    {
        get
        {
            if (_patrolPointPath == null)
            {
                _patrolPointPath = _patrolPointPathGameObject.GetComponent<PatrolPointPath>();
                
                if (_patrolPointPath == false)
                {
                    Debug.LogError("잘못된 PatrolPointPath 오브젝트 지정");
                }
            }
            
            return _patrolPointPath;
        }
    }
    
    public override void Release()
    {
        base.Release();
        
        _patrolPointPath = null;
    }
}