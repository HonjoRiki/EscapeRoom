using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;
using R3;

/// <summary>
/// プレイヤーのビュー
/// モデルの位置をUnityのGameObjectに反映させます
/// </summary>
public sealed class PlayerView : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;
    [SerializeField] private float rotationSpeed = 10f;

    private Vector3 lastDirection = Vector3.forward;

    // --- LookAt IK Properties ---
    public Transform LookAtTarget { get; set; }
    public bool IsLookAtActive { get; set; } = true;

    // LookAtの影響度（外部から変更したい場合はpublicメソッドやプロパティを介して設定）
    private float lookAtOverallWeight = 1.0f;
    private float lookAtBodyWeight = 0.15f;
    private float lookAtHeadWeight = 0.8f;
    private float lookAtEyesWeight = 1.0f;
    private float lookAtClampWeight = 0.5f;
    // --------------------------

    // 現在接触しているトリガーのリスト
    private readonly HashSet<Collider> currentTriggers = new HashSet<Collider>();

    private readonly Subject<Collider> onTriggerEnterSubject = new();
    private readonly Subject<Collider> onTriggerExitSubject = new();

    public Observable<Collider> OnTriggerEnterObservable => onTriggerEnterSubject;
    public Observable<Collider> OnTriggerExitObservable => onTriggerExitSubject;

    public void UpdatePosition(Vector3 position)
    {
        transform.position = position;
    }

    public void MoveCharacter(Vector3 motion)
    {
        animator.SetFloat("Speed", motion.magnitude * 3.0f);
        if (characterController != null)
        {
            characterController.Move(motion);
        }
    }

    public void UpdateRotate(Vector3 direction)
    {
        if (direction.magnitude > 0.01f)
        {
            lastDirection = direction.normalized;
        }

        Quaternion targetRotation = Quaternion.LookRotation(lastDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    /// <summary>
    /// AnimatorのIK処理が実行されるフレームで呼び出される
    /// Animator ControllerのレイヤーでIK Passが有効になっている必要があります
    /// </summary>
    private void OnAnimatorIK(int layerIndex)
    {
        if (animator == null || LookAtTarget == null || !IsLookAtActive)
        {
            // IKを無効にする
            animator.SetLookAtWeight(0);
            return;
        }

        // ターゲットの方向を向くようにIKを設定
        animator.SetLookAtPosition(LookAtTarget.position);
        animator.SetLookAtWeight(lookAtOverallWeight, lookAtBodyWeight, lookAtHeadWeight, lookAtEyesWeight, lookAtClampWeight);
    }

    public IEnumerable<Collider> GetCurrentTriggers()
    {
        // 接触中にオブジェクトが破棄された場合に備え、nullを除去する
        currentTriggers.RemoveWhere(c => c == null);
        return currentTriggers.ToList();
    }

    private void OnTriggerEnter(Collider other)
    {
        currentTriggers.Add(other);
        onTriggerEnterSubject.OnNext(other);
    }

    private void OnTriggerExit(Collider other)
    {
        currentTriggers.Remove(other);
        onTriggerExitSubject.OnNext(other);
    }

    private void OnDestroy()
    {
        onTriggerEnterSubject.Dispose();
        onTriggerExitSubject.Dispose();
    }
}
