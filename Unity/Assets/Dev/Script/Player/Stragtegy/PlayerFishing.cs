using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cinemachine.Utility;
using Cysharp.Threading.Tasks;
using MyBox;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;

public class PlayerFishing : MonoBehaviour, IPlayerStrategy
{
    [Serializable]
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    [SerializeField] private LineRenderer _line;
    [SerializeField] private float _horizontalDirAngle;
    [SerializeField] private float _verticalUpFactor;
    [SerializeField] private float _verticalDownFactor;
    [SerializeField] private float _speed;
    [SerializeField] private float _maxHandleInTagent;
    [SerializeField] private float _maxLineLength;
    [SerializeField] private float _beginLineLength;
    [SerializeField] private float _lineHeightFactor;
    
    [SerializeField] private int _lineIteraction = 20 ;
    
    [SerializeField] private Transform _handle;

    public Transform _testHandle;
    [SerializeField] SplineContainer _splineContainer;

    private IEnumerator _co;
    
    public void Init(PlayerController controller)
    {
    }

    [ButtonMethod]
    private void LeftDirSet()
    {
        var spline = GetCalculatedSpline(Direction.Left, _testHandle.position);
        _splineContainer.Spline = spline;
        
    }

    [ButtonMethod]
    private void RightDirSet()
    {
        var spline = GetCalculatedSpline(Direction.Right, _testHandle.position);
        _splineContainer.Spline = spline;
        
    }

    [ButtonMethod]
    private void UpDirSet()
    {
        var spline = GetCalculatedSpline(Direction.Up, _testHandle.position);
        _splineContainer.Spline = spline;
        
    }

    [ButtonMethod]
    private void DownDirSet()
    {
        var spline = GetCalculatedSpline(Direction.Down, _testHandle.position);
        _splineContainer.Spline = spline;
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Fishing(Direction.Up, _testHandle.position);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Fishing(Direction.Down, _testHandle.position);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Fishing(Direction.Left, _testHandle.position);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Fishing(Direction.Right, _testHandle.position);
        }
    }

    public void Fishing(Direction direction, Vector3 targetPosition)
    {
        if (_co is not null)
        {
            StopCoroutine(_co);
            _co = null;
        }

        _handle.position = transform.position;
        StartCoroutine(_co = _Fishing(direction, targetPosition));
    }
    
    private IEnumerator _Fishing(Direction dir, Vector3 targetPoint)
    {
        var spline = GetCalculatedSpline(dir, targetPoint);
        float t = 0f;
        var wPos = _handle.position;
        
        _handle.gameObject.SetActive(true);
        _line.positionCount = _lineIteraction;

        var lineSpline = new Spline();
        lineSpline.Add(new BezierKnot(transform.position, float3.zero, float3.zero, quaternion.AxisAngle(new float3(0f, 0f, 1f), 0f)), TangentMode.AutoSmooth );
        lineSpline.Add(new BezierKnot(transform.position, float3.zero, float3.zero, quaternion.AxisAngle(new float3(0f, 0f, 1f), 0f)), TangentMode.AutoSmooth );
        lineSpline.Add(new BezierKnot(transform.position, float3.zero, float3.zero, quaternion.AxisAngle(new float3(0f, 0f, 1f), 0f)), 
            dir == Direction.Up || dir == Direction.Down? TangentMode.AutoSmooth : TangentMode.Broken );
        
        float toTargetPointDis = Vector2.Distance(transform.position, targetPoint);
        
        while (t <= 1f)
        {
            var pos = (Vector3)spline.EvaluatePosition(t);
            t += Time.deltaTime * _speed / toTargetPointDis;
            
            _handle.position = pos + wPos;

            lineSpline.SetKnot(0,
                new BezierKnot(transform.position, float3.zero, float3.zero,
                    quaternion.AxisAngle(new float3(0f, 0f, 1f), 0f)));

            Vector3 toHandleDir = (_handle.position - transform.position);
            float toHandleDis = Mathf.Lerp(0f, _lineHeightFactor, Mathf.Clamp(Mathf.Abs(toHandleDir.x) - _beginLineLength, 0, _maxLineLength- _beginLineLength) / (_maxLineLength- _beginLineLength));

            float handleToTargetDis = Vector3.Distance(targetPoint, _handle.position);

            if (handleToTargetDis <= _maxHandleInTagent)
            {
                handleToTargetDis /= _maxHandleInTagent;
            }
            else
            {
                handleToTargetDis = 1f;
            }

            Vector3 middlePos =
                Vector3.Lerp(transform.position, _handle.position, 0.5f) + Vector3.down * toHandleDis;
            
            lineSpline.SetKnot(1,
                new BezierKnot(middlePos, float3.zero, float3.zero,
                    quaternion.AxisAngle(new float3(0f, 0f, 1f), 0f)));
            
            lineSpline.SetKnot(2,
                new BezierKnot(_handle.position, new float3(2.19f, 3.26f, 0f) * handleToTargetDis, float3.zero,
                    quaternion.AxisAngle(new float3(0f, 0f, 1f), 0f)));

            for (int i = 0; i < _lineIteraction; i++)
            {
                var linePos = lineSpline.EvaluatePosition(i / (float)(_lineIteraction - 1));
                
                _line.SetPosition(i, linePos);
            }
            
            yield return null;
        }
        _handle.gameObject.SetActive(false);
        _handle.transform.position = wPos;
    }
    

    private Spline GetCalculatedSplineVertical(Direction dir, Vector3 targetPoint)
    {
        Spline spline = new Spline();
        
        Vector3 startPoint = Vector3.zero;
        Vector3 dirV = Vector3.one;
        float factor = 0f;

        targetPoint = transform.worldToLocalMatrix.MultiplyPoint(targetPoint);

        switch (dir)
        {
            case Direction.Up:
                dirV = Vector3.down;
                factor = _verticalUpFactor;
                break;
            case Direction.Down:
                dirV = Vector3.up;
                factor = _verticalDownFactor;
                break;
            default:
                Debug.Assert(false);
                break;
        }
        
        factor = Vector2.Distance(targetPoint, startPoint) * factor;
        dirV = dirV.normalized  * factor;

        
        spline.Add(new BezierKnot(startPoint, float3.zero, float3.zero, quaternion.AxisAngle(new float3(0f, 0f, 1f), 0f)), TangentMode.Continuous);
        spline.Add(new BezierKnot(startPoint + dirV, float3.zero, float3.zero, quaternion.AxisAngle(new float3(0f, 0f, 1f), 0f)), TangentMode.Continuous);
        spline.Add(new BezierKnot(targetPoint, float3.zero, float3.zero, quaternion.AxisAngle(new float3(0f, 0f, 1f), 0f)), TangentMode.Continuous);

        return spline;
    }
    private Spline GetCalculatedSplineHorizontal(Direction dir, Vector3 targetPoint)
    {
        Spline spline = new Spline();

        Vector3 startPoint = Vector3.zero;
        Vector3 dirV = Vector3.one;

        targetPoint = transform.worldToLocalMatrix.MultiplyPoint(targetPoint);

        
        switch (dir)
        {
            case Direction.Left:
                dirV = Quaternion.AngleAxis(_horizontalDirAngle, Vector3.forward) * Vector3.right;
                dirV.x *= -1f;
                break;
            case Direction.Right:
                dirV = Quaternion.AngleAxis(_horizontalDirAngle, Vector3.forward) * Vector3.right;
                break;
            default:
                Debug.Assert(false);
                break;
        }

        dirV = dirV.normalized;

        spline.Add(
            new BezierKnot(startPoint, float3.zero, float3.zero, quaternion.AxisAngle(new float3(0f, 0f, 1f), 0f)),
            TangentMode.Continuous);


        /*
        OB: ToTarget
        OB = |OA`| * cos * OB/|OB|
        OB * 1 / cos = |OA`| * OB/|OB|
        |(OB * 1/cos)| = |OA`|
         */
        var toTarget = (Vector3)((Vector3)targetPoint - startPoint);
        Vector3 projV = toTarget
                        * (1f / (Vector3.Dot(dirV.normalized, toTarget.normalized)))
            ;

        float projVMag = projV.magnitude;
        dirV = dirV.normalized * projVMag;
        dirV.x = targetPoint.x;
        
        spline.Add(
            new BezierKnot(startPoint + Vector3.one * 0.000001f, float3.zero, dirV,
                quaternion.AxisAngle(new float3(0f, 0f, 1f), 0f)), TangentMode.Broken);


        spline.Add(
            new BezierKnot((Vector3)targetPoint, float3.zero, float3.zero,
                quaternion.AxisAngle(new float3(0f, 0f, 1f), 0f)), TangentMode.Broken);
        
        return spline;
    }
    
    public Spline GetCalculatedSpline(Direction dir, Vector2 targetPoint)
    {
        if (dir == Direction.Up || dir == Direction.Down)
        {
            return GetCalculatedSplineVertical(dir, targetPoint);
        }
        else
        {
            return GetCalculatedSplineHorizontal(dir, targetPoint);
        }
    }
}