namespace gather
{
    public class Sarlac : Unit
    {

        void Start()
        {
            TimeManager timeManager = FindFirstObjectByType<TimeManager>();
            timeManager.Dusk.AddListener(WakeUp);
            timeManager.Dawn.AddListener(StartSleep);
        }

        void Update()
        {
        
        }

        void StartSleep()
        {
            /*
             * if at home then sleep, else move to home
            */
        }

        void WakeUp()
        {
            /*
             * play sound and animation, start hunting
            */
        }
    }
}
