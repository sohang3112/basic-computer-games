using Poker.Resources;
using static System.StringComparison;

internal class Game
{
    private readonly IReadWrite _io;
    private readonly IRandom _random;

    private class FloatArray
    {
        private float[] _values;

        public FloatArray(float[] values)
        {
            _values = values;
        }

        public float this[float index]
        {
            get => _values[(int)index];
            set => _values[(int)index] = value;
        }
    }

    private readonly FloatArray _cards = new(new float[51]);
    private readonly FloatArray _cardRanks = new(new float[16]);
    private bool _hasWatch;
    private float _computerBalance;
    private float _playerBalance;
    private float _pot;

    private float B;
    private float M;
    private float T;
    private float D;
    private float G;
    private float I;
    private float U;
    private float Z;
    private float X;
    private float K;
    private float V;
    private float Q;

    private string JS;
    private string KS;
    private string HS;
    private string IS;

    public Game(IReadWrite io, IRandom random)
    {
        _io = io;
        _random = random;
    }

    private int Get0To9() => _random.Next(10);

    private static int GetRank(float x) => (int)x % 100;

    private static int GetSuit(float x) => (int)x / 100;

    internal void Play()
    {
        _io.Write(Resource.Streams.Title);
        _io.Write(Resource.Streams.Instructions);

        _hasWatch = true;
        _computerBalance = 200;
        _playerBalance = 200;

        while(PlayHand());
    }

    internal bool PlayHand()
    {
        _pot=0;
        while(true)
        {
            _io.WriteLine();
            if (_computerBalance<=5)
            {
                CongratulatePlayer();
                return false;
            }
            _io.WriteLine("The ante is $5.  I will deal:");
            _io.WriteLine();
            if (_playerBalance <= 5 && PlayerCantRaiseFunds()) { return false; }
            _pot += 10;
            _playerBalance -= 5;
            _computerBalance -= 5;
            for (Z=1; Z <= 10; Z++)
            {
                DealCard((int)Z);
            }
            _io.WriteLine("Your hand:");
            DisplayHand(1);
            I=2;
            (B, JS, KS, M, X) = AnalyzeHand(6);
            _io.WriteLine();
_330:       if (I!=6) { goto _470; }
_340:       if (Get0To9()<=7) { goto _370; }
_350:       X=11100;
_360:       goto _420;
_370:       if (Get0To9()<=7) { goto _400; }
_380:       X=11110;
_390:       goto _420;
_400:       if (Get0To9()>=1) { goto _450; }
_410:       X=11111;
_420:       I=7;
_430:       Z=23;
_440:       goto _580;
_450:       Z=1;
_460:       goto _510;
_470:       if (B >= 13) { goto _540; }
_480:       if (Get0To9()>=2) { goto _500; }
_490:       goto _420;
_500:       Z=0;
_510:       K=0;
_520:       _io.WriteLine("I check.");
_530:       goto _620;
_540:       Z = B <= 16 || Get0To9() < 1 ? 35 : 2;
_580:       V=Z+Get0To9();
_590:       if (CopmuterCantContinue()) { return false; }
_600:       _io.WriteLine($"I'll open with ${V}");
_610:       K=V;
            G = 0;
_620:       if (GetWager()) { return false; }
_630:       var response = IsThereAWinner();
            if (response.HasValue) { return response.Value; }

            _io.WriteLine();
            T = _io.ReadNumber("Now we draw -- How many cards do you want", 3, "You can't draw more than three cards.");
            if (T != 0)
            {
                Z=10;
                _io.WriteLine("What are their numbers:");
                for (Q=1; Q <= T; Q++)
                {
                    U = _io.ReadNumber("");
                    DealCard((int)++Z, (int)U);
                }
                _io.WriteLine("Your new hand:");
                DisplayHand(1);
            }
            Z=10+T;
            for (U=6; U <= 10; U++)
            {
                if ((int)(X/Math.Pow(10, U-6)) == 10*(int)(X/Math.Pow(10, U-5)))
                {
                    DealCard((int)++Z, (int)U);
                }
            }
            _io.WriteLine();
            _io.Write($"I am taking{Z-10-T}card");
            if (Z != 11 + T)
            {
                _io.WriteLine("s");
            }
            _io.WriteLine();
            V=I;
            I=1;
            (B, JS, KS, M, X) = AnalyzeHand(6);
            if (V == 7)
            {
                Z = 28;
            }
            else if (I == 6)
            {
                Z = 1;
            }
            else if (B < 13)
            {
                Z = Get0To9() == 6 ? 19 : 2;
            }
            else
            {
                if (B >= 16)
                {
                    Z = 2;
                }
                else
                {
                    Z = Get0To9() == 8 ? 11 : 19;
                }
            }
_1330:      K=0;
            G=0;
_1340:      if (GetWager()) { return false; }
_1350:      if (T!=.5) { goto _1450; }
_1360:      if (V==7) { goto _1400; }
_1370:      if (I!=6) { goto _1400; }
_1380:      _io.WriteLine("I'll check");
_1390:      goto _1460;
_1400:      V=Z+Get0To9();
_1410:      if (CopmuterCantContinue()) { return false; }
_1420:      _io.WriteLine($"I'll bet ${V}");
_1430:      K=V;
_1440:      if (GetWager()) { return false; }
_1450:      response = IsThereAWinner();
            if (response.HasValue) { return response.Value; }
_1460:      _io.WriteLine();
            _io.WriteLine("Now we compare hands:");
            _io.WriteLine("My hand:");
            DisplayHand(6);
            (U, HS, IS, D, X) = AnalyzeHand(1);
            _io.WriteLine();
            _io.Write("You have ");
            DisplayHandRank(HS, IS, (int)D);
            _io.Write("and I have ");
            DisplayHandRank(JS, KS, (int)M);
            if (B > U || GetRank(M) > GetRank(D)) { return ComputerWins().Value; }
            if (U > B || GetRank(D) > GetRank(M)) { return PlayerWins().Value; }
             _io.WriteLine("The hand is drawn.");
             _io.WriteLine($"All ${_pot}remains in the pot.");
        }

        bool? IsThereAWinner()
        {
            if (I!=3)
            {
                if (I!=4) { return null; }
                _io.WriteLine();
                return PlayerWins();
            }
            _io.WriteLine();
            return ComputerWins();
        }

        bool? ComputerWins()
        {
            _io.WriteLine("I win.");
            _computerBalance += _pot;
            return ShouldContinue();
        }

        bool? ShouldContinue()
        {
            _io.WriteLine($"Now I have ${_computerBalance}and you have ${_playerBalance}");
            return _io.ReadYesNo("Do you wish to continue");
        }

        bool? PlayerWins()
        {
            _io.WriteLine("You win.");
            _playerBalance += _pot;
            return ShouldContinue();
        }

        void DealCard(int index, int indexToReplace = 0)
        {
            while(true)
            {
                _cards[index]=100*_random.Next(4) + _random.Next(100);
                if (GetSuit(_cards[index]) > 3) { continue; }
                if (GetRank(_cards[index]) > 12) { continue; }
                if (index==1) { break; }
                var matchFound = false;
                for (var i=1; i <= index-1; i++)
                {
                    if (_cards[index]==_cards[i])
                    {
                        matchFound = true;
                        break;
                    }
                }
                if (!matchFound) { break; }
            }
            if (index > 10)
            {
                (_cards[indexToReplace], _cards[index]) = (_cards[index], _cards[indexToReplace]);
            }
            return;
        }

        void DisplayHand(int firstCard)
        {
            for (Z = firstCard; Z <= firstCard+4; Z++)
            {
                var card = _cards[Z];
                _io.Write($"{Z}--  ");
                _io.Write(GetRankName(GetRank(card)));
                _io.Write(" of");
                _io.Write(GetSuitName(GetSuit(card)));
                if (Z % 2 == 0)
                {
                    _io.WriteLine();
                }
            }
            _io.WriteLine();
            return;
        }

        string GetRankName(int rank) => rank switch
        {
            9 => "Jack",
            10 => "Queen",
            11 => "King",
            12 => "Ace",
            _ => (rank + 2).ToString()
        };

        string GetSuitName(int suitNumber) => suitNumber switch
        {
            0 => " Clubs",
            1 => " Diamonds",
            2 => " Hearts",
            3 => " Spades"
        };

        (int, string, string, float, int) AnalyzeHand(int firstCard)
        {
            var suitMatchCount = 0;
            for (var i = firstCard; i <= firstCard+4; i++)
            {
               _cardRanks[i]=GetRank(_cards[i]);
               if (i < firstCard+4 && GetSuit(_cards[i]) == GetSuit(_cards[i+1]))
               {
                   suitMatchCount++;
               }
            }
            if (suitMatchCount == 4)
            {
                return (15, "A Flus", "h in", _cards[firstCard], 11111);
            }
            for (var i = firstCard; i <= firstCard+3; i++)
            {
                for (var j = i+1; j <= firstCard+4; j++)
                {
                    if (_cardRanks[i] > _cardRanks[j])
                    {
                        (_cards[i], _cards[j]) = (_cards[j], _cards[i]);
                        (_cardRanks[i], _cardRanks[j]) = (_cardRanks[j], _cardRanks[i]);
                    }
                }
            }
            var handRank = 0;
            var keepMask = 0;
            var highCard = 0f;
            var handName1 = "";
            var handName2 = "";
            for (var i = firstCard; i <= firstCard+3; i++)
            {
                if (_cardRanks[i] == _cardRanks[i+1])
                {
                    keepMask += 11*(int)Math.Pow(10, i-firstCard);
                    highCard = _cards[i];
                    (handRank, handName1, handName2) = AnalyzeMultiples(handRank, i);
                }
            }
            if (keepMask == 0)
            {
                if (_cardRanks[firstCard]+3 == _cardRanks[firstCard+3])
                {
                    keepMask=1111;
                    handRank=10;
                }
                if (_cardRanks[firstCard+1]+3 == _cardRanks[firstCard+4])
                {
                    if (handRank == 10)
                    {
                        return (14, "Straig", "ht", _cards[firstCard+4], 11111);
                    }
                    handRank=10;
                    keepMask=11110;
                }
            }
            if (handRank < 10)
            {
                I = 6;
                return (9, "Schmal", "tz, ", _cards[firstCard+4], 11000);
            }
            else if (handRank == 10)
            {
                if (I == 1) { I = 6; }
            }
            else if (handRank <= 12 && GetRank(highCard) <= 6)
            {
                I = 6;
            }
            return (handRank, handName1, handName2, highCard, keepMask);
        }

        (int, string, string) AnalyzeMultiples(int handStrength, int index) =>
            (handStrength, _cardRanks[index] == _cardRanks[index - 1]) switch
            {
                (<11, _) => (11, "A Pair", " of "),
                (11, true) => (13, "Three", " "),
                (11, _) => (12, "Two P", "air, "),
                (12, _) => (16, "Full H", "ouse, "),
                (_, true) => (17, "Four", " "),
                _ => (16, "Full H", "ouse, ")
            };

        bool GetWager()
        {
_3060:      _io.WriteLine();
            T = _io.ReadNumber("What is your bet");
_3080:      if ((T-(int)T)==0) { goto _3140; }
_3090:      if (K!=0) { goto _3120; }
_3100:      if (G!=0) { goto _3120; }
_3110:      if (T==.5) { return false; }
_3120:      _io.WriteLine("No small change, please.");
_3130:      goto _3060;
_3140:      if (_playerBalance-G-T>=0) { goto _3170; }
_3150:      if (PlayerCantRaiseFunds()) { return true; }
_3160:      goto _3060;
_3170:      if (T!=0) { goto _3200; }
_3180:      I=3;
_3190:      return Line_3380();
_3200:      if (G+T>=K) { goto _3230; }
_3210:      _io.WriteLine("If you can't see my bet, then fold.");
_3220:      goto _3060;
_3230:      G += T;
_3240:      if (G==K) { return Line_3380(); }
_3250:      if (Z!=1) { return Line_3420(); }
_3260:      if (G>5) { goto _3300; }
_3270:      if (Z>=2) { return Line_3350(); }
_3280:      V=5;
_3290:      return Line_3420();
_3300:      if (Z==1) { goto _3320; }
_3310:      if (T<=25) { return Line_3350(); }
_3320:      I=4;
_3330:      _io.WriteLine("I fold.");
_3340:      return false;
        }

        bool Line_3350()
        {
            if (Z==2) { return Line_3430(); }
            return Line_3360();
        }

        bool Line_3360()
        {
            _io.WriteLine("I'll see you.");
            K=G;
            return Line_3380();
        }

        bool Line_3380()
        {
            _playerBalance -= G;
            _computerBalance -= K;
            _pot += G+K;
            return false;
        }

        bool Line_3420()
        {
            if (G>3*Z) { return Line_3350(); }
            return Line_3430();
        }

        bool Line_3430()
        {
            V=G-K+Get0To9();
            if (CopmuterCantContinue()) { return true; }
            _io.WriteLine($"I'll see you, and raise you{V}");
            K=G+V;
            return GetWager();
        }

        bool CopmuterCantContinue()
        {
            if (_computerBalance - G - V >= 0) { return false; }
            if (G == 0)
            {
                V = _computerBalance;
            }
            else if (_computerBalance - G >= 0)
            {
                return Line_3360();
            }
            else if (!_hasWatch)
            {
                var response = _io.ReadString("Would you like to buy back your watch for $50");
                if (!response.StartsWith("N", InvariantCultureIgnoreCase))
                {
                    // The original code did not deduct $50 from the player
                    _computerBalance += 50;
                    _hasWatch = true;
                }
            }
            return false;
        }

        bool CongratulatePlayer()
        {
            _io.WriteLine("I'm busted.  Congratulations!");
            return true;
        }

        void DisplayHandRank(string part1, string part2, int highCard)
        {
            _io.Write($"{part1}{part2}");
            if (part1 == "A Flus")
            {
                _io.Write(GetSuitName(highCard/100));
                _io.WriteLine();
            }
            else
            {
                _io.Write(GetRankName(GetRank(highCard)));
                _io.WriteLine(part1 == "Schmal" || part1 == "Straig" ? " High" : "'s");
            }
        }

        bool PlayerCantRaiseFunds()
        {
            _io.WriteLine();
            _io.WriteLine("You can't bet with what you haven't got.");
            if (_hasWatch)
            {
                var response = _io.ReadString("Would you like to sell your watch");
                if (!response.StartsWith("N", InvariantCultureIgnoreCase))
                {
                    if (Get0To9() < 7)
                    {
                        _io.WriteLine("I'll give you $75 for it.");
                        _playerBalance += 75;
                    }
                    else
                    {
                        _io.WriteLine("That's a pretty crummy watch - I'll give you $25.");
                        _playerBalance += 25;
                    }
                    _hasWatch = false;
                    return false;
                }
            }

            _io.WriteLine("Your wad is shot.  So long, sucker!");
            return true;
        }
    }
}

internal static class IReadWriteExtensions
{
    internal static bool ReadYesNo(this IReadWrite io, string prompt)
    {
        while (true)
        {
            var response = io.ReadString("Do you wish to continue");
            if (response.Equals("YES", InvariantCultureIgnoreCase)) { return true; }
            if (response.Equals("NO", InvariantCultureIgnoreCase)) { return false; }
            io.WriteLine("Answer Yes or No, please.");
        }
    }

    internal static int ReadNumber(this IReadWrite io, string prompt, int max, string maxPrompt)
    {
        io.Write(prompt);
        while (true)
        {
            var response = io.ReadNumber("");
            if (response <= max) { return (int)response; }
            io.WriteLine(maxPrompt);
        }
    }
}