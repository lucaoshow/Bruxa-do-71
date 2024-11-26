namespace Root.Runes
{
    [System.Serializable]
    public struct EffectOnPlayer {
        public float duration;
        public float speedBuff;
        public float defenseBuff;
        public float distanceToTravel;

        public EffectOnPlayer(float seconds, float speed, float defense,  float distance)
        {
            duration = seconds;
            speedBuff = speed;
            defenseBuff = defense;
            distanceToTravel = distance;
        }

        public static EffectOnPlayer operator -(EffectOnPlayer e) 
        {
            return new EffectOnPlayer(0, -e.speedBuff, -e.defenseBuff, 0);
        }
    }
}