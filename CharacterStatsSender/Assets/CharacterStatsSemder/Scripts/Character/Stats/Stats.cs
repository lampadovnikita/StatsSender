using Assets.CharacterStatsSemder.Scripts.Observable;

namespace Assets.CharacterStatsSemder.Scripts.Character.Stats
{
    public class Stats
    {
        public readonly ObservableValue<int> strengthObservable;

        public readonly ObservableValue<int> wisdomObservable;

        public readonly ObservableValue<int> agilityObservable;

        public StatsData StatsData
        {
            get
            {
                return new StatsData(strengthObservable.Value, wisdomObservable.Value, agilityObservable.Value);
            }

            set
            {
                strengthObservable.Value = value.strength;
                wisdomObservable.Value = value.wisdom;
                agilityObservable.Value = value.agility;
            }
        }

        public Stats()
        {
            strengthObservable = new ObservableValue<int>();
            wisdomObservable = new ObservableValue<int>();
            agilityObservable = new ObservableValue<int>();
        }

        public Stats(StatsData characterStatsData) : this()
        {
            StatsData = characterStatsData;
        }
    }
}