using UnityEngine;

namespace Root.Projectiles 
{
    public class Projectile : MonoBehaviour 
    {
        private Vector2 direction; 
        public void SetDirection(Vector2 direction) 
        {
            this.direction = direction;
        }
    }

}