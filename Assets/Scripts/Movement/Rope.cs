using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Rope : MonoBehaviour {
    [Header("Values")]
    public AnimationCurve effectOverTime;
    public AnimationCurve curve;
    public AnimationCurve curveEffectOverDistance;
    public float curveSize;
    public float scrollSpeed;
    public float segments;
    public float animSpeed;

    [Header("Data")]
    public LineRenderer lineRenderer;

    private Vector3 _start;
    private Vector3 _end;
    private float _time;
    private bool _active;

    public void UpdateGrapple() {
        lineRenderer.enabled = _active;
        if (_active)
            ProcessBounce();
    }

    private void ProcessBounce() {
        var vectors = new List<Vector3>();

        _time = Mathf.MoveTowards(_time, 1f,
            Mathf.Max(Mathf.Lerp(_time, 1f, animSpeed * Time.deltaTime) - _time, 0.2f * Time.deltaTime));

        vectors.Add(_start);

        var forward = Quaternion.LookRotation(_end - _start);
        var up = forward * Vector3.up;

        for (var i = 1; i < segments + 1; i++) {
            var delta = 1f / segments * i;
            var realDelta = delta * curveSize;
            while (realDelta > 1f) realDelta -= 1f;
            var calcTime = realDelta + -scrollSpeed * _time;
            while (calcTime < 0f) calcTime += 1f;

            var defaultPos = GetPos(delta);
            var effect = Eval(effectOverTime, _time) * Eval(curveEffectOverDistance, delta) * Eval(curve, calcTime);

            vectors.Add(defaultPos + up * effect);
        }

        lineRenderer.positionCount = vectors.Count;
        lineRenderer.SetPositions(vectors.ToArray());
    }

    private Vector3 GetPos(float d) {
        return Vector3.Lerp(_start, _end, d);
    }

    private static float Eval(AnimationCurve ac, float t) {
        return ac.Evaluate(t * ac.keys.Select(k => k.time).Max());
    }

    public void Grapple(Vector3 start, Vector3 end) {
        _active = true;
        _time = 0f;

        _start = start;
        _end = end;
    }

    public void UnGrapple() {
        _active = false;
    }

    public void UpdateStart(Vector3 start) {
        _start = start;
    }

    public bool Grappling => _active;

}
