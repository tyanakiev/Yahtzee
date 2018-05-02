// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Insane.cs" company="NWTC">
//   Copyright
// </copyright>
// <summary>
//   The insane AI logic.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Yahtzee
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The insane AI logic.
    /// </summary>
    public class Insane
    {
        /// <summary>
        /// The probability machine.
        /// </summary>
        private ProbabilityMachine probabilityMachine;

        /// <summary>
        /// The r 2 d 2 dices score.
        /// </summary>
        private CheckDices r2d2DicesScore = new CheckDices();

        /// <summary>
        /// The probability dictionary.
        /// </summary>
        private Dictionary<string, double> probabilityDict = new Dictionary<string, double>();

        /// <summary>
        /// The R2D2 dices.
        /// </summary>
        private int[] r2d2Dices;

        /// <summary>
        /// The R2D2 scores played.
        /// </summary>
        private List<string> r2d2ScoresPlayed;

        /// <summary>
        /// The hold dice index.
        /// </summary>
        private List<int> holdDiceIndex = new List<int> { 0, 1, 2, 3, 4 };

        /// <summary>
        /// The r 2 d 2 possible score.
        /// </summary>
        private Dictionary<string, int> r2d2PossibleScore;

        /// <summary>
        /// The best choice.
        /// </summary>
        private string bestChoice = string.Empty; 

        /// <summary>
        /// The all scores.
        /// </summary>
        private List<string> allScores = new List<string>
                                             {
                                                 "TotalScore",
                                                 "Sum",
                                                 "Bonus",
                                                 "Ones",
                                                 "Twos",
                                                 "Threes",
                                                 "Fours",
                                                 "Fives",
                                                 "Sixes",
                                                 "Yahtzee",
                                                 "BigStraight",
                                                 "SmallStraight",
                                                 "FourOfAKind",
                                                 "FullHouse",
                                                 "ThreeOfAKind",
                                                 "Chance"
                                             };

        /// <summary>
        /// Sets the r 2 d 2 dices.
        /// </summary>
        public int[] R2D2Dices
        {
            set
            {
                this.r2d2Dices = value;
            }
        }

        /// <summary>
        /// Sets the R2D2 scores played.
        /// </summary>
        public List<string> R2D2ScoresPlayed
        {
            set
            {
                this.r2d2ScoresPlayed = value;
            }
        }

        /// <summary>
        /// Gets the hold dice index.
        /// </summary>
        public List<int> HoldDiceIndex
        {
            get
            {
                return this.holdDiceIndex;
            }
        }

        /// <summary>
        /// Gets the best choice.
        /// </summary>
        public string BestChoice
        {
            get
            {
                return this.bestChoice;
            }
        }

        /// <summary>
        /// Returns list of indexes of dices that needs to be hold before the roll.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<int> HoldDicesIndex()
        {
            return this.HoldDiceIndex;
        }

        /// <summary>
        /// Main method, the brain of the AI. If this method return True, the AI will roll again,
        /// if it return False that means that  the dices are very good and the score is either Yahtzee, Big Straight.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Compute()
        {
            this.holdDiceIndex = new List<int> { 0, 1, 2, 3, 4 };
            this.probabilityMachine = new ProbabilityMachine(this.r2d2Dices);
            this.r2d2DicesScore.AssignDices(this.r2d2Dices);
            this.r2d2PossibleScore = this.r2d2DicesScore.CheckCombinations();
            if (this.IsYahtzee())
            {
                return false;
            }
            else if (this.IsBigStraight())
            {
                return false;
            }
            else if (this.IsSmallStraight())
            {
                return this.IsSmallStraight();
            }
            else if (this.IsFullHouse())
            {
                return false;
            }
            else
            {
                this.probabilityDict = this.probabilityMachine.Campute();
                if (this.probabilityDict.ContainsKey("Yahtzee"))
                {
                    if (this.probabilityDict.ContainsKey("Yahtzee") && this.probabilityDict["Yahtzee"] > 0.05 && !this.r2d2ScoresPlayed.Contains("Yahtzee"))
                    {
                        this.HoldForYahtzee();
                        return true;
                    }
                    else if (!this.r2d2ScoresPlayed.Contains("BigStraigt") && this.probabilityDict.ContainsKey("BigStraight") && this.probabilityDict["BigStraight"] > 0.02)
                    {
                        this.HoldForStraight();
                        return true;
                    }
                    else if (!this.r2d2ScoresPlayed.Contains("SmallStraight") && this.probabilityDict.ContainsKey("SmallStraight") && this.probabilityDict["SmallStraight"] > 0.05)
                    {
                        this.HoldForStraight();
                        return true;
                    }
                    else if (!this.r2d2ScoresPlayed.Contains("FullHouse") && this.probabilityDict.ContainsKey("FullHouse") && this.probabilityDict["FullHouse"] > 0.05)
                    {
                        this.HoldForFullHouse();
                        return true;
                    }
                    else if (this.probabilityDict.ContainsKey("FourOfAKind") && this.probabilityDict["FourOfAKind"] > 0.05 && !this.r2d2ScoresPlayed.Contains("FourOfAKind"))
                    {
                        this.HoldForYahtzee();
                        return true;
                    }
                    else if (this.probabilityDict.ContainsKey("ThreeOfAKind") && this.probabilityDict["ThreeOfAKind"] > 0.1 && !this.r2d2ScoresPlayed.Contains("ThreeOfAKind"))
                    {
                        this.HoldForThreeOfAKind();
                        return true;
                    }
                    else
                    {
                        this.MaximizePoints();
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// The reset hold list.
        /// </summary>
        public void ResetHoldList()
        {
            this.holdDiceIndex = new List<int> { 0, 1, 2, 3, 4 };
        }

        /// <summary>
        /// Checks for Yahtzee.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool IsYahtzee()
        {
            if (this.r2d2PossibleScore.ContainsKey("Yahtzee") && !this.r2d2ScoresPlayed.Contains("Yahtzee"))
            {
                this.bestChoice = "Yahtzee";
                return true;
            }

            return false;
        }

        /// <summary>
        /// The is full house.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool IsFullHouse()
        {
            if (this.r2d2PossibleScore.ContainsKey("FullHouse") && !this.r2d2ScoresPlayed.Contains("FullHouse"))
            {
                this.bestChoice = "FullHouse";
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks for Big Straight.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool IsBigStraight()
        {
            if (this.r2d2PossibleScore.ContainsKey("BigStraight") && !this.r2d2ScoresPlayed.Contains("BigStraight"))
            {
                this.bestChoice = "BigStraight";
                return true;
            }

            return false;
        }

        /// <summary>
        /// The is small straight.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool IsSmallStraight()
        {
            if (this.r2d2PossibleScore.ContainsKey("SmallStraight") && !this.r2d2ScoresPlayed.Contains("BigStraight"))
            {
                this.bestChoice = "SmallStraight";
                this.HoldForStraight();
                return true;
            }
            else if (this.r2d2PossibleScore.ContainsKey("SmallStraight")
                     && !this.r2d2ScoresPlayed.Contains("SmallStraight"))
            {
                this.bestChoice = "SmallStraight";
                return false;
            }

            return false;
        }

        /// <summary>
        /// Sorts the dices, checks for consecutive numbers, looks for the one that is not consecutive and adds it to the list.
        /// </summary>
        private void HoldForStraight()
        {
            var tmpArray = (from element in this.r2d2Dices orderby element ascending select element).ToArray();
            for (int i = 0; i <= tmpArray.Length - 2; i++)
            {
                if (tmpArray[i] + 1 != tmpArray[i + 1])
                {
                    // TODO
                    if (i > 0)
                    {
                        if (tmpArray[i - 1] + 1 != tmpArray[i])
                        {
                            this.holdDiceIndex.Remove(Array.IndexOf(this.r2d2Dices, tmpArray[i]));
                        }
                        else
                        {
                            this.holdDiceIndex.Remove(Array.IndexOf(this.r2d2Dices, tmpArray[i + 1]));
                        }
                    }
                    else
                    {
                        this.holdDiceIndex.Remove(Array.IndexOf(this.r2d2Dices, tmpArray[i]));
                    }
                }
            }
        }

        /// <summary>
        /// The hold for yahtzee.
        /// </summary>
        private void HoldForYahtzee()
        {
            var tmpDict = this.PopulateDict();
            foreach (var dice in tmpDict)
            {
                if (dice.Value <= 2)
                {
                    this.holdDiceIndex.Remove(Array.IndexOf(this.r2d2Dices, dice.Key));
                }
            }
        }

        /// <summary>
        /// The maximize points.
        /// </summary>
        private void MaximizePoints()
        {
            var tmpDict = this.PopulateDict();
            foreach (var dice in tmpDict)
            {
                if (dice.Value <= 2)
                {
                    if (!this.r2d2ScoresPlayed.Contains(this.probabilityMachine.MapDices[dice.Key]))
                    {
                        this.holdDiceIndex.Remove(Array.IndexOf(this.r2d2Dices, dice.Key));
                    }
                }
            }
        }

        /// <summary>
        /// The hold for three of a kind.
        /// </summary>
        private void HoldForThreeOfAKind()
        {
            var tmpDict = this.PopulateDict();
            foreach (var dice in tmpDict)
            {
                if (dice.Value < 2)
                {
                    this.holdDiceIndex.Remove(Array.IndexOf(this.r2d2Dices, dice.Key));
                }
            }
        }

        /// <summary>
        /// Checks for the two different dices for possible FullHouse. Checks which one has higher value and then rerolls the other.
        /// </summary>
        private void HoldForFullHouse()
        {
            var tmpDict = this.PopulateDict();
            int tmp1 = 0;
            int tmp2 = 0;
            foreach (var dice in tmpDict)
            {
                if (dice.Value < 2)
                {
                    if (tmp1 == 0)
                    {
                        tmp1 = dice.Key;
                    }
                    else
                    {
                        tmp2 = dice.Key;
                    }
                }
            }

            this.holdDiceIndex.Remove(tmp1 > tmp2 ? Array.IndexOf(this.r2d2Dices, tmp1) : Array.IndexOf(this.r2d2Dices, tmp2));
        }

        /// <summary>
        /// Populates how many of each dice there are in all the dices.
        /// </summary>
        /// <returns>
        /// The <see cref="Dictionary"/>.
        /// </returns>
        private Dictionary<int, int> PopulateDict()
        {
            Dictionary<int, int> tmpDict = new Dictionary<int, int>();
            foreach (var dice in this.r2d2Dices)
            {
                if (tmpDict.ContainsKey(dice))
                {
                    tmpDict[dice]++;
                }
                else
                {
                    tmpDict.Add(dice, 1);
                }
            }

            return tmpDict;
        }
    }
}