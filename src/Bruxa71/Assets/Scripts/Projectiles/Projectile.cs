using System;
using Root.EditorExtensions.PropertyDrawers;
using UnityEngine;

namespace Root.Projectiles 
{
    public class Projectile : MonoBehaviour 
    {
        [SerializeField] private float secondsLifespan = -1;
        [SerializeField] private float maxTravelDistance;
        [SerializeField] private float maxMoveSpeed;
        [SerializeField] private bool matchRotationToDirection;
        [SerializeField] private bool circularTrajectory;

        [DrawIf("circularTrajectory", true)]
        [SerializeField] private float distanceFromCenter;

        [DrawIf("circularTrajectory", true)]
        [SerializeField] private float maxSpinSpeed;

        [DrawIf("circularTrajectory", true)]
        [SerializeField] private AnimationCurve spinSpeedCurve;

        [DrawIf("circularTrajectory", false)]
        [SerializeField] private float maxHeight;

        [DrawIf("circularTrajectory", false)]
        [SerializeField] private AnimationCurve trajectory;

        [SerializeField] private AnimationCurve moveSpeedCurve;

        private Vector3 direction;
        private float timeAlive;
        private Vector3 startPoint;
        private Vector3 trajectoryRange;
        private float moveSpeed;
        private float spinSpeed;
        private Transform rotateAround;

        private void Start() 
        {
            this.startPoint = this.transform.position;
            this.trajectoryRange = this.direction * this.maxTravelDistance - this.startPoint;
            if (this.circularTrajectory) 
            {
                this.rotateAround = new GameObject().transform;
                this.rotateAround.position = this.transform.position + this.direction * this.distanceFromCenter;
                this.transform.parent = this.rotateAround;
            }
        }

        private void Update()
        {
            if (Math.Abs(this.trajectoryRange.normalized.x) < Math.Abs(this.trajectoryRange.normalized.y)) 
            {
                this.UpdatePositionForHorizontalTrajectory();
            }
            else 
            {
                this.UpdatePositionForVerticalTrajectory();
            }

            if ((this.transform.position - this.startPoint).magnitude >= this.maxTravelDistance || (this.secondsLifespan > -1 && this.timeAlive >= this.secondsLifespan)) 
            {
                Destroy(this.gameObject);
                if (this.circularTrajectory) { Destroy(this.rotateAround.gameObject); }
            }
        }

        public void SetDirection(Vector2 direction) 
        {
            this.direction = direction;
        }

        private void UpdatePositionForHorizontalTrajectory() 
        {
            if (this.circularTrajectory) 
            {
                this.RotateAround(this.rotateAround.position, this.rotateAround.forward, this.spinSpeed);
                this.rotateAround.position += this.direction * this.moveSpeed * Time.deltaTime;
                this.moveSpeed = this.moveSpeedCurve.Evaluate(this.rotateAround.position.normalized.x) * this.maxMoveSpeed * Mathf.Sign(this.trajectoryRange.y);
                this.spinSpeed = this.spinSpeedCurve.Evaluate(this.rotateAround.position.normalized.x) * this.maxSpinSpeed * Mathf.Sign(this.trajectoryRange.y);
                return;
            }

            float nextY = this.transform.position.y + this.moveSpeed * Time.deltaTime;
            float nextYNormalized = (nextY - this.startPoint.y) / this.maxTravelDistance;

            float nextXNormalized = this.trajectory.Evaluate(nextYNormalized);

            float nextX = this.startPoint.x + nextXNormalized * this.maxHeight;

            Vector3 newPosition = new Vector3(nextX, nextY, 0);

            if (this.matchRotationToDirection) 
            {
                Vector2 moveDir = newPosition - this.transform.position;
                this.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg);
            }

            this.transform.position = newPosition;

            this.moveSpeed = this.moveSpeedCurve.Evaluate(nextYNormalized) * this.maxMoveSpeed * Mathf.Sign(this.trajectoryRange.x);
        }

        private void UpdatePositionForVerticalTrajectory() 
        {
            if (this.circularTrajectory) 
            {
                this.RotateAround(this.rotateAround.position, this.rotateAround.forward, this.spinSpeed);
                this.rotateAround.position += this.direction * this.moveSpeed * Time.deltaTime;
                this.moveSpeed = this.moveSpeedCurve.Evaluate(this.rotateAround.position.normalized.y) * this.maxMoveSpeed * Mathf.Sign(this.trajectoryRange.x);
                this.spinSpeed = this.spinSpeedCurve.Evaluate(this.rotateAround.position.normalized.y) * this.maxSpinSpeed * Mathf.Sign(this.trajectoryRange.x);
                return;
            }

            float nextX = this.transform.position.x + this.moveSpeed * Time.deltaTime;
            float nextXNormalized = (nextX - this.startPoint.x) / this.maxTravelDistance;

            float nextYNormalized = this.trajectory.Evaluate(Math.Abs(nextXNormalized));

            float nextY = this.startPoint.y + nextYNormalized * this.maxHeight;

            Vector3 newPosition = new Vector3(nextX, nextY, 0);

            if (this.matchRotationToDirection) 
            {
                Vector2 moveDir = newPosition - this.transform.position;
                this.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg);
            }

            this.transform.position = newPosition;

            this.moveSpeed = this.moveSpeedCurve.Evaluate(Math.Abs(nextXNormalized)) * this.maxMoveSpeed * Mathf.Sign(this.trajectoryRange.y);
        }

        private void RotateAround(Vector3 center, Vector3 axis, float angle)
        {
            Vector3 pos = this.transform.position;
            Quaternion rot = Quaternion.AngleAxis(angle, axis);
            Vector3 dir = pos - center;
            dir = rot * dir;
            Vector3 newPosition = center + dir;

            if (this.matchRotationToDirection) 
            {
                Vector2 moveDir = newPosition - this.transform.position;
                this.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg);
            }
            
            this.transform.position = newPosition;
        }

        private void OnValidate()
        {
            bool added = false;
            
            if (!this.TryGetComponent(out Animator animator)) 
            {
                this.gameObject.AddComponent<Animator>(); 
            }

            if (!this.TryGetComponent(out SpriteRenderer spriteRenderer)) 
            {
                this.gameObject.AddComponent<SpriteRenderer>(); 
            }

            if (added)
            {
                Debug.Log("Added the necessary components for the projectile animation. Please, add the correspondent Animator Controller to the Animator component and a collider shape to this GameObject (" + this.gameObject.name + ").");
            }
        }

    }
}