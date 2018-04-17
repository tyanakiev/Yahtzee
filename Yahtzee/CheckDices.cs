// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckDices.cs" company="NWTC">
//   Copyright
// </copyright>
// <summary>
//   Defines the CheckDices type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Yahtzee
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;

    /// <summary>
    /// The check dices.
    /// </summary>
    public class CheckDices
    {
        /// <summary>
        /// The scores find.
        /// </summary>
        private Dictionary<string, int> combinationsFind = new Dictionary<string, int>();

        /// <summary>
        /// The dices.
        /// </summary>
        private int[] dices;
        
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
        /// Initializes a new instance of the <see cref="CheckDices"/> class.
        /// </summary>
        /// <param name="dices">
        /// The dices.
        /// </param>
        public void AssignDices(int[] dices)
        {
            this.Dices = dices;
        }
        
        /// <summary>
        /// The calculate.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public Dictionary<string, int> CheckCombinations()
        {
            this.combinationsFind.Clear();
            this.combinationsFind.Add("Chance", this.GetSumOfAllDice());
            this.CheckOnes();
            this.CheckTwos();
            this.CheckThrees();
            this.CheckFours();
            this.CheckFives();
            this.CheckSixes();
            this.CheckBigStraight();
            this.CheckSmallStraight();
            this.CheckThreeOfAKind();
            this.CheckFourOfAKindOrFullHouse();
            this.CheckYahtzee();
            return this.combinationsFind;
        }

        /// <summary>
        /// The check ones.
        /// </summary>
        private void CheckOnes()
        {
            if (this.Dices.Contains(1))
            {
                this.combinationsFind.Add("Ones", this.GetSumOfSpecificDice(1));
            }
        }

        /// <summary>
        /// The check twos.
        /// </summary>
        private void CheckTwos()
        {
            if (this.Dices.Contains(2))
            {
                this.combinationsFind.Add("Twos", this.GetSumOfSpecificDice(2));
            }
        }

        /// <summary>
        /// The check threes.
        /// </summary>
        private void CheckThrees()
        {
            if (this.Dices.Contains(3))
            {
                this.combinationsFind.Add("Threes", this.GetSumOfSpecificDice(3));
            }
        }

        /// <summary>
        /// The check fours.
        /// </summary>
        private void CheckFours()
        {
            if (this.Dices.Contains(4))
            {
                this.combinationsFind.Add("Fours", this.GetSumOfSpecificDice(4));
            }
        }

        /// <summary>
        /// The check fives.
        /// </summary>
        private void CheckFives()
        {
            if (this.Dices.Contains(5))
            {
                this.combinationsFind.Add("Fives", this.GetSumOfSpecificDice(5));
            }
        }

        /// <summary>
        /// The check sixes.
        /// </summary>
        private void CheckSixes()
        {
            if (this.Dices.Contains(6))
            {
                this.combinationsFind.Add("Sixes", this.GetSumOfSpecificDice(6));
            }
        }

        /// <summary>
        /// The check Yahtzee.
        /// </summary>
        private void CheckYahtzee()
        {
            if (this.Dices.Distinct().Count() == 1)
            {
                this.combinationsFind.Add("Yahtzee", 50);
            }
        }

        /// <summary>
        /// The check big straight.
        /// </summary>
        private void CheckBigStraight()
        {
            if (this.Dices.Distinct().Count() == 5)
            {
                if (this.CheckForConsecutiveNumbers() == 4)
                {
                    this.combinationsFind.Add("BigStraight", 40);
                }
            }
        }

        /// <summary>
        /// The check small straight.
        /// </summary>
        private void CheckSmallStraight()
        {
            if (this.Dices.Distinct().Count() > 3)
            {
                if (this.CheckForConsecutiveNumbers() == 3)
                {
                    this.combinationsFind.Add("SmallStraight", 30);
                }
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
            int tmpConsNumbers = 0;
            int[] tmpArray = (from element in this.Dices orderby element ascending select element).ToArray();
            for (int i = 0; i <= tmpArray.Length - 2; i++)
            {
                if (tmpArray[i + 1] == tmpArray[i] + 1)
                {
                    tmpConsNumbers++;
                }
            }

            return tmpConsNumbers;
        }

        /// <summary>
        /// The check four of a kind or full house.
        /// </summary>
        private void CheckFourOfAKindOrFullHouse()
        {
            int count1 = 0;
            int count2 = 0;
            int tmpDice1 = this.Dices.Distinct().First();
            if (this.Dices.Distinct().Count() == 2)
            {
                foreach (int dice in this.Dices)
                {
                    if (tmpDice1 == dice)
                    {
                        count1++;
                    }
                    else
                    {
                        count2++;
                    }
                }

                if (count1 <= 1 || count2 <= 1)
                {
                    this.combinationsFind.Add("FourOfAKind", this.GetSumOfAllDice());
                }
                else
                {
                    this.combinationsFind.Add("FullHouse", this.GetSumOfAllDice());
                }
            }
        }

        /// <summary>
        /// The check three of a kind.
        /// </summary>
        private void CheckThreeOfAKind()
        {
            int count1 = 0;
            int count2 = 0;
            int count3 = 0;
            int tmpDice1 = this.Dices.Distinct().First();
            int tmpDice3 = this.Dices.Distinct().Last();
            if (this.Dices.Distinct().Count() == 3)
            {
                foreach (int dice in this.Dices)
                {
                    if (tmpDice1 == dice)
                    {
                        count1++;
                    }
                    else if (tmpDice3 == dice)
                    {
                        count3++;
                    }
                    else
                    {
                        count2++;
                    }
                }

                if (count1 > 2 || count2 > 2 || count3 > 2)
                {
                    this.combinationsFind.Add("ThreeOfAKind", this.GetSumOfAllDice());
                }
            }
        }

        /// <summary>
        /// The get sum of specific dice.
        /// </summary>
        /// <param name="diceNumber">
        /// The dice Number.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        private int GetSumOfSpecificDice(int diceNumber)
        {
            int sum = 0;
            foreach (int dice in this.Dices)
            {
                if (dice == diceNumber)
                {
                    sum += dice;
                }
            }

            return sum;
        }

        /// <summary>
        /// The get sum of all dice.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        private int GetSumOfAllDice()
        {
            int sum = 0;
            foreach (int dice in this.Dices)
            {
                    sum += dice;
            }

            return sum;
        }
    }
}
