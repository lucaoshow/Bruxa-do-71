namespace Root.Runes
{
    [System.Serializable]
    public struct EffectOnEnemy {
        public float duration;
        public float radius;
        public bool activateInstantaneously;
        public bool continuous;
        public float attackDebuff;
        public float speedDebuff;
    }
}