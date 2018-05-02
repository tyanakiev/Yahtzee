// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProbabilityMachine.cs" company="NWTC">
//   Copyright
// </copyright>
// <summary>
//   Defines the ProbabilityMachine type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Yahtzee
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The probability machine.
    /// </summary>
    public class ProbabilityMachine
    {
        /// <summary>
        /// The probability dictionary.
        /// </summary>
        private Dictionary<string, double> probabilityDict = new Dictionary<string, double>();

        /// <summary>
        /// The dices.
        /// </summary>
        private int[] dices;

        /// <summary>
        /// The chance dictionary.
        /// </summary>
        private Dictionary<int, double> chanceDict = new Dictionary<int, double>
                                                         {
                                                             { 0, 1 },
                                                             { 1, 0.166 },
                                                             { 2, 0.027 },
                                                             { 3, 0.004 },
                                                             { 4, 0.0007 },
                                                         };

        /// <summary>
        /// Map of dices and theirs names.
        /// </summary>
        private Dictionary<int, string> mapDices = new Dictionary<int, string>
                                                       {
                                                           { 1, "Ones" },
                                                           { 2, "Twos" },
                                                           { 3, "Threes" },
                                                           { 4, "Fours" },
                                                           { 5, "Fives" },
                                                           { 6, "Sixes" },
                                                       };

        /// <summary>
        /// This Dictionary will hold as key the dice and as value the quantity of that dice.
        /// </summary>
        private Dictionary<int, int> dicesDict = new Dictionary<int, int>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ProbabilityMachine"/> class.
        /// </summary>
        /// <param name="dices">
        /// The dices.
        /// </param>
        public ProbabilityMachine(int[] dices)
        {
            this.Dices = dices;
        }

        /// <summary>
        /// Gets the map dices.
        /// </summary>
        public Dictionary<int, string> MapDices
        {
            get
            {
                return this.mapDices;
            }
        }

        /// <summary>
        /// Gets or sets the dices.
        /// </summary>
        public int[] Dices
        {
            get
            {
                return this.dices;
            }

            set
            {
                this.dices = value;
            }
        }

        /// <summary>
        /// Checks possible games and returns a dictionary containing the results.
        /// </summary>
        /// <returns>
        /// The <see cref="Dictionary"/>.
        /// </returns>
        public Dictionary<string, double> Campute()
        {
            this.PopulateDicesDict();
            this.EvaluateDicesDict();
            return this.probabilityDict;
        }

        /// <summary>
        /// The populate dices dictionary.
        /// </summary>
        private void PopulateDicesDict()
        {
            foreach (var dice in this.Dices)
            {
                if (!this.dicesDict.ContainsKey(dice))
                {
                    this.dicesDict.Add(dice, 1);
                }
                else
                {
                    this.dicesDict[dice]++;
                }
            }
        }

        /// <summary>
        /// Evaluates the dices dictionary for possible scores.
        /// </summary>
        private void EvaluateDicesDict()
        {
            // A A A A A 
            if (this.dicesDict.Count == 1)
            {
                this.probabilityDict.Add("Yahtzee", this.chanceDict[0]);
                this.probabilityDict.Add("FourOfAKind", this.chanceDict[0]);
                this.probabilityDict.Add("ThreeOfAKind", this.chanceDict[0]);
                this.probabilityDict.Add(this.mapDices[this.dicesDict.Keys.ElementAt(0)], this.chanceDict[0]);
            }
            else if (this.dicesDict.Count == 2)
            {
                if (this.dicesDict.Values.ElementAt(0) > 3 || this.dicesDict.Values.ElementAt(1) > 3)
                {
                    // A A A A B
                    this.probabilityDict.Add("Yahtzee", this.chanceDict[1]);
                    this.probabilityDict.Add("FourOfAKind", this.chanceDict[0]);
                    this.probabilityDict.Add("ThreeOfAKind", this.chanceDict[0]);
                    this.probabilityDict.Add("FullHouse", this.chanceDict[1]);
                    this.probabilityDict.Add(this.mapDices[this.GetBestDice()], this.chanceDict[0]);
                }
                else
                {
                    // A A A B B
                    this.probabilityDict.Add("Yahtzee", this.chanceDict[2]);
                    this.probabilityDict.Add("FourOfAKind", this.chanceDict[1]);
                    this.probabilityDict.Add("ThreeOfAKind", this.chanceDict[0]);
                    this.probabilityDict.Add("FullHouse", this.chanceDict[0]);
                    this.probabilityDict.Add(this.mapDices[this.GetBestDice()], this.chanceDict[0]);
                }
            }
            else if (this.dicesDict.Count == 3)
            {
                // AAA B C[B]
                if (this.dicesDict.Values.ElementAt(0) > 2 || this.dicesDict.Values.ElementAt(1) > 2 || this.dicesDict.Values.ElementAt(2) > 2)
                {
                    this.probabilityDict.Add("Yahtzee", this.chanceDict[2]);
                    this.probabilityDict.Add("FourOfAKind", this.chanceDict[1]);
                    this.probabilityDict.Add("ThreeOfAKind", this.chanceDict[0]);
                    this.probabilityDict.Add("FullHouse", this.chanceDict[1]);
                    this.probabilityDict.Add(this.mapDices[this.GetBestDice()], this.chanceDict[0]);
                }
                else
                {
                    // if (this.dicesDict[0] < 3 && this.dicesDict[1] < 3 && this.dicesDict[2] < 3)
                    // AA BB C
                    this.probabilityDict.Add("Yahtzee", this.chanceDict[3]);
                    this.probabilityDict.Add("FourOfAKind", this.chanceDict[2]);
                    this.probabilityDict.Add("ThreeOfAKind", this.chanceDict[1]);
                    this.probabilityDict.Add("FullHouse", this.chanceDict[1]);
                    this.probabilityDict.Add(this.mapDices[this.GetBestDice()], this.chanceDict[0]);
                }
            }
            else if (this.dicesDict.Count == 4)
            {
                // AA B C D
                this.probabilityDict.Add("Yahtzee", this.chanceDict[3]);
                this.probabilityDict.Add("FourOfAKind", this.chanceDict[2]);
                this.probabilityDict.Add("ThreeOfAKind", this.chanceDict[1]);
                this.probabilityDict.Add("FullHouse", this.chanceDict[2]);
                this.probabilityDict.Add("BigStraight", this.chanceDict[5 - this.CheckForConsecutiveNumbers()]);
                this.probabilityDict.Add("SmallStraight", this.chanceDict[5 - this.CheckForConsecutiveNumbers()]);
                this.probabilityDict.Add(this.mapDices[this.GetBestDice()], this.chanceDict[0]);
            }
            else
            {
                // A B C D E
                this.probabilityDict.Add("Yahtzee", this.chanceDict[4]);
                this.probabilityDict.Add("FourOfAKind", this.chanceDict[3]);
                this.probabilityDict.Add("ThreeOfAKind", this.chanceDict[2]);
                this.probabilityDict.Add("FullHouse", this.chanceDict[4]);
                this.probabilityDict.Add("BigStraight", this.chanceDict[5 - this.CheckForConsecutiveNumbers()]);
                this.probabilityDict.Add("SmallStraight", this.chanceDict[5 - this.CheckForConsecutiveNumbers()]);
            }
        }

        /// <summary>
        /// The check for consecutive numbers.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        private int CheckForConsecutiveNumbers()
        {
            int tmpConsNumbers = 1;
            int[] tmpArray = (from element in this.Dices orderby element ascending select element).ToArray();
            for (int i = 1; i <= tmpArray.Length - 1; i++)
            {
                if (tmpArray[i] == tmpArray[i - 1] + 1)
                {
                    tmpConsNumbers++;
                }
            }

            return tmpConsNumbers;
        }

        /// <summary>
        /// The get best dice.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        private int GetBestDice()
        {
            // Cool way to do it with Linq but will not work with dict.count = 1.
            // var max = this.dicesDict.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
            int highestValue = -1;
            int key = 0;
            foreach (var dice in this.dicesDict)
            {
                if (highestValue < dice.Value)
                {
                    highestValue = dice.Value;
                    key = dice.Key;
                }
            }

            return key;
        }
    }
}
