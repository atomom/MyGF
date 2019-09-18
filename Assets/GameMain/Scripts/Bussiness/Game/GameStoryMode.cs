namespace IUV.SDN
{
    public class GameStoryMode : GameBase
    {
        public override GameMode GameMode
        {
            get
            {
                return GameMode.Story;
            }
        }

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            base.Update(elapseSeconds, realElapseSeconds);

        }
    }
}