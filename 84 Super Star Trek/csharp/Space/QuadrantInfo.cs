using SuperStarTrek.Objects;

namespace SuperStarTrek.Space
{
    internal class QuadrantInfo
    {
        private bool _isKnown;

        private QuadrantInfo(Coordinates coordinates, string name, int klingonCount, int starCount, bool hasStarbase)
        {
            Coordinates = coordinates;
            Name = name;
            KlingonCount = klingonCount;
            StarCount = starCount;
            HasStarbase = hasStarbase;
        }

        public Coordinates Coordinates { get; }
        public string Name { get; }
        public int KlingonCount { get; private set; }
        public bool HasStarbase { get; private set; }
        public int StarCount { get; }

        public static QuadrantInfo Create(Coordinates coordinates, string name)
        {
            var random = new Random();
            var klingonCount = random.GetDouble() switch
            {
                > 0.98 => 3,
                > 0.95 => 2,
                > 0.80 => 1,
                _ => 0
            };
            var hasStarbase = random.GetDouble() > 0.96;
            var starCount = random.Get1To8Inclusive();

            return new QuadrantInfo(coordinates, name, klingonCount, starCount, hasStarbase);
        }

        internal void AddKlingon() => KlingonCount += 1;

        internal void AddStarbase() => HasStarbase = true;

        internal Quadrant BuildQuadrant(Enterprise enterprise, Random random, Galaxy galaxy) =>
            new(this, enterprise, random, galaxy);

        internal string Scan()
        {
            _isKnown = true;
            return ToString();
        }

        public override string ToString() => _isKnown ? $"{KlingonCount}{(HasStarbase ? 1 : 0)}{StarCount}" : "***";

        internal void RemoveKlingon()
        {
            if (KlingonCount > 0)
            {
                KlingonCount -= 1;
            }
        }

        internal void RemoveStarbase() => HasStarbase = false;
    }
}
