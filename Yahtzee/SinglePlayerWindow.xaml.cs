// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SinglePlayerWindow.xaml.cs" company="NWTC">
//   Copyright
// </copyright>
// <summary>
//   Interaction logic for SinglePlayerWindow
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Yahtzee
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Media.Imaging;

    using Button = System.Windows.Controls.Button;
    using MessageBox = System.Windows.MessageBox;

    /// <summary>
    /// Interaction logic for SinglePlayerWindow
    /// </summary>
    public partial class SinglePlayerWindow : Window
    {
        /// <summary>
        /// The player 1 rolls.
        /// </summary>
        private int player1Rolls = 0;

        /// <summary>
        /// The player 1 dices.
        /// </summary>
        private int[] player1Dices = new int[] { 0, 0, 0, 0, 0 };

        /// <summary>
        /// The player 1 dices hold.
        /// </summary>
        private List<int> player1DicesHold = new List<int>();

        /// <summary>
        /// The player 1 scores played.
        /// </summary>
        private List<string> player1ScoresPlayed = new List<string> { "TotalScore", "Sum", "Bonus" };

        /// <summary>
        /// The player 1 dices score.
        /// </summary>
        private CheckDices player1DicesScore = new CheckDices();

        /// <summary>
        /// The player 1 score.
        /// </summary>
        private Dictionary<string, int> player1Score;

        /// <summary>
        /// The player 1 score buttons.
        /// </summary>
        private Dictionary<string, Button> player1ScoreButtons = new Dictionary<string, Button>();

        /// <summary>
        /// The player 1 sum.
        /// </summary>
        private int player1Sum = 0;

        /// <summary>
        /// The player 1 sum for lower section.
        /// </summary>
        private int player1LowerSectionSum = 0;

        /// <summary>
        /// The player 1 bonus.
        /// </summary>
        private int player1Bonus;

        /// <summary>
        /// The player 1 total sum.
        /// </summary>
        private int player1TotalSum = 0;

        /// <summary>
        /// The player 1 score count.
        /// </summary>
        private int player1ScoreCount = 0;

        /// <summary>
        /// The player 1 wait for score.
        /// </summary>
        private bool player1WaitForScore = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="SinglePlayerWindow"/> class.
        /// </summary>
        public SinglePlayerWindow()
        {
            this.InitializeComponent();
            this.ControlDicesPlayer1(false);
            this.DisableButtons();
            this.Player1ScoreButtonsMap();
        }

        /// <summary>
        /// The button player 1 roll dice click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BtnPlayer1RollDiceClick(object sender, RoutedEventArgs e)
        {
            if (!this.player1WaitForScore)
            {
                if (this.player1Rolls == 3)
                {
                    if (this.CheckForValidMovePlayer1())
                    {
                        this.AddScorePlayer1();
                        this.player1WaitForScore = true;
                    }
                    else
                    {
                        MessageBox.Show("You don't have any moves. Roll again !");
                        this.ResetDicesPlayer1();
                        this.player1Rolls = 0;
                    }

                    this.ControlDicesPlayer1(false);
                    this.UnholdImages();
                    this.player1DicesHold.Clear();
                }
                else
                {
                    Random rnd = new Random();
                    for (int i = 0; i < 5; i++)
                    {
                        if (!this.player1DicesHold.Contains(i))
                        {
                            this.player1Dices[i] = rnd.Next(1, 7);
                        }
                    }

                    this.ControlDicesPlayer1(true);
                    this.player1Rolls++;
                    this.player1DicesScore.AssignDices(this.player1Dices);
                    this.player1Score = this.player1DicesScore.CheckCombinations();
                    this.imgPlayer1Dice1.Source = new BitmapImage(new Uri("static/dice" + this.player1Dices[0] + ".png", UriKind.Relative));
                    this.imgPlayer1Dice2.Source = new BitmapImage(new Uri("static/dice" + this.player1Dices[1] + ".png", UriKind.Relative));
                    this.imgPlayer1Dice3.Source = new BitmapImage(new Uri("static/dice" + this.player1Dices[2] + ".png", UriKind.Relative));
                    this.imgPlayer1Dice4.Source = new BitmapImage(new Uri("static/dice" + this.player1Dices[3] + ".png", UriKind.Relative));
                    this.imgPlayer1Dice5.Source = new BitmapImage(new Uri("static/dice" + this.player1Dices[4] + ".png", UriKind.Relative));
                    this.ResetButtonsPlayer1();
                    this.AddScorePlayer1();
                }
            }
        }

        /// <summary>
        /// The check for valid move player 1.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool CheckForValidMovePlayer1()
        {
            foreach (var score in this.player1Score)
            {
                if (!this.player1ScoresPlayed.Contains(score.Key))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// The player 1 dice 1 click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Player1Dice1Click(object sender, RoutedEventArgs e)
        {
            if (this.imgPlayer1Dice1.Opacity == 1)
            {
                this.imgPlayer1Dice1.Opacity = 0.5;
                this.player1DicesHold.Add(0);
            }
            else
            {
                this.imgPlayer1Dice1.Opacity = 1;
                this.player1DicesHold.Remove(0);
            }
        }

        /// <summary>
        /// The player 1 dice 2 click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Player1Dice2Click(object sender, RoutedEventArgs e)
        {
            if (this.imgPlayer1Dice2.Opacity == 1)
            {
                this.imgPlayer1Dice2.Opacity = 0.5;
                this.player1DicesHold.Add(1);
            }
            else
            {
                this.imgPlayer1Dice2.Opacity = 1;
                this.player1DicesHold.Remove(1);
            }
        }

        /// <summary>
        /// The player 1 dice 3 click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Player1Dice3Click(object sender, RoutedEventArgs e)
        {
            if (this.imgPlayer1Dice3.Opacity == 1)
            {
                this.imgPlayer1Dice3.Opacity = 0.5;
                this.player1DicesHold.Add(2);
            }
            else
            {
                this.imgPlayer1Dice3.Opacity = 1;
                this.player1DicesHold.Remove(2);
            }
        }

        /// <summary>
        /// The player 1 dice 4 click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Player1Dice4Click(object sender, RoutedEventArgs e)
        {
            if (this.imgPlayer1Dice4.Opacity == 1)
            {
                this.imgPlayer1Dice4.Opacity = 0.5;
                this.player1DicesHold.Add(3);
            }
            else
            {
                this.imgPlayer1Dice4.Opacity = 1;
                this.player1DicesHold.Remove(3);
            }
        }

        /// <summary>
        /// The player 1 dice 5 click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Player1Dice5Click(object sender, RoutedEventArgs e)
        {
            if (this.imgPlayer1Dice5.Opacity == 1)
            {
                this.imgPlayer1Dice5.Opacity = 0.5;
                this.player1DicesHold.Add(4);
            }
            else
            {
                this.imgPlayer1Dice5.Opacity = 1;
                this.player1DicesHold.Remove(4);
            }
        }

        /// <summary>
        /// Enables controls for player 1.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        private void ControlDicesPlayer1(bool value)
        {
            this.imgPlayer1Dice1.IsEnabled = value;
            this.imgPlayer1Dice2.IsEnabled = value;
            this.imgPlayer1Dice3.IsEnabled = value;
            this.imgPlayer1Dice4.IsEnabled = value;
            this.imgPlayer1Dice5.IsEnabled = value;
        }

        /// <summary>
        /// Removes opacity from all dice images.
        /// </summary>
        private void UnholdImages()
        {
            this.imgPlayer1Dice1.Opacity = 1;
            this.imgPlayer1Dice2.Opacity = 1;
            this.imgPlayer1Dice3.Opacity = 1;
            this.imgPlayer1Dice4.Opacity = 1;
            this.imgPlayer1Dice5.Opacity = 1;
        }

        /// <summary>
        /// The disable buttons.
        /// </summary>
        private void DisableButtons()
        {
            this.btnPlayer1Ones.IsEnabled = false;
            this.btnPlayer1Twos.IsEnabled = false;
            this.btnPlayer1Threes.IsEnabled = false;
            this.btnPlayer1Fours.IsEnabled = false;
            this.btnPlayer1Fives.IsEnabled = false;
            this.btnPlayer1Sixes.IsEnabled = false;
            this.btnPlayer1Sum.IsEnabled = false;
            this.btnPlayer1Bonus.IsEnabled = false;
            this.btnPlayer1ThreeOfAKind.IsEnabled = false;
            this.btnPlayer1FourOfAKind.IsEnabled = false;
            this.btnPlayer1FullHouse.IsEnabled = false;
            this.btnPlayer1SmallStraight.IsEnabled = false;
            this.btnPlayer1LargeStraight.IsEnabled = false;
            this.btnPlayer1Chance.IsEnabled = false;
            this.btnPlayer1Yahtzee.IsEnabled = false;
            this.btnPlayer1TotalScore.IsEnabled = false;
        }

        /// <summary>
        /// The player 1 score buttons map.
        /// </summary>
        private void Player1ScoreButtonsMap()
        {
            this.player1ScoreButtons.Add("Ones", this.btnPlayer1Ones);
            this.player1ScoreButtons.Add("Twos", this.btnPlayer1Twos);
            this.player1ScoreButtons.Add("Threes", this.btnPlayer1Threes);
            this.player1ScoreButtons.Add("Fours", this.btnPlayer1Fours);
            this.player1ScoreButtons.Add("Fives", this.btnPlayer1Fives);
            this.player1ScoreButtons.Add("Sixes", this.btnPlayer1Sixes);
            this.player1ScoreButtons.Add("Yahtzee", this.btnPlayer1Yahtzee);
            this.player1ScoreButtons.Add("BigStraight", this.btnPlayer1LargeStraight);
            this.player1ScoreButtons.Add("SmallStraight", this.btnPlayer1SmallStraight);
            this.player1ScoreButtons.Add("FourOfAKind", this.btnPlayer1FourOfAKind);
            this.player1ScoreButtons.Add("FullHouse", this.btnPlayer1FullHouse);
            this.player1ScoreButtons.Add("ThreeOfAKind", this.btnPlayer1ThreeOfAKind);
            this.player1ScoreButtons.Add("Sum", this.btnPlayer1Sum);
            this.player1ScoreButtons.Add("Bonus", this.btnPlayer1Bonus);
            this.player1ScoreButtons.Add("Chance", this.btnPlayer1Chance);
            this.player1ScoreButtons.Add("TotalScore", this.btnPlayer1TotalScore);
        }

        /// <summary>
        /// The add score player 1.
        /// </summary>
        private void AddScorePlayer1()
        {
            foreach (KeyValuePair<string, int> pair in this.player1Score)
            {
                if (!this.player1ScoresPlayed.Contains(pair.Key))
                {
                    this.player1ScoreButtons[pair.Key].IsEnabled = true;
                    this.player1ScoreButtons[pair.Key].Content = pair.Value;
                }
            }
        }

        /// <summary>
        /// The player 1 pick score.
        /// </summary>
        /// <param name="dice">
        /// The dice.
        /// </param>
        private void Player1PickScore(string dice)
        {
            this.player1ScoresPlayed.Add(dice);
            this.DisableButtons();
            this.ResetButtonsPlayer1();
            this.player1Rolls = 0;
            this.CheckAndAddBonusPlayer1();
            this.ResetDicesPlayer1();
            this.UnholdImages();
            this.player1ScoreCount++;
            this.CheckVictory();
            this.player1WaitForScore = false;
            this.player1DicesHold.Clear();
        }

        /// <summary>
        /// The check and add bonus player 1.
        /// </summary>
        private void CheckAndAddBonusPlayer1()
        {
            if (this.player1Sum > 63)
            {
                this.player1Bonus = 35;
                this.btnPlayer1Bonus.Content = 35;
            }
        }

        /// <summary>
        /// The button player 1 ones click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BtnPlayer1OnesClick(object sender, RoutedEventArgs e)
        {
            this.Player1PickScore("Ones");
            this.player1Sum += int.Parse(this.btnPlayer1Ones.Content.ToString());
            this.player1TotalSum = this.player1Sum + this.player1LowerSectionSum;
            this.btnPlayer1Sum.Content = this.player1Sum;
            this.btnPlayer1TotalScore.Content = this.player1TotalSum;
        }

        /// <summary>
        /// The button player 1 twos click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BtnPlayer1TwosClick(object sender, RoutedEventArgs e)
        {
            this.Player1PickScore("Twos");
            this.player1Sum += int.Parse(this.btnPlayer1Twos.Content.ToString());
            this.player1TotalSum = this.player1Sum + this.player1LowerSectionSum;
            this.btnPlayer1Sum.Content = this.player1Sum;
            this.btnPlayer1TotalScore.Content = this.player1TotalSum;
        }

        /// <summary>
        /// The button player 1 threes click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BtnPlayer1ThreesClick(object sender, RoutedEventArgs e)
        {
            this.Player1PickScore("Threes");
            this.player1Sum += int.Parse(this.btnPlayer1Threes.Content.ToString());
            this.player1TotalSum = this.player1Sum + this.player1LowerSectionSum;
            this.btnPlayer1Sum.Content = this.player1Sum;
            this.btnPlayer1TotalScore.Content = this.player1TotalSum;
        }

        /// <summary>
        /// The button player 1 fours click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BtnPlayer1FoursClick(object sender, RoutedEventArgs e)
        {
            this.Player1PickScore("Fours");
            this.player1Sum += int.Parse(this.btnPlayer1Fours.Content.ToString());
            this.player1TotalSum = this.player1Sum + this.player1LowerSectionSum;
            this.btnPlayer1Sum.Content = this.player1Sum;
            this.btnPlayer1TotalScore.Content = this.player1TotalSum;
        }

        /// <summary>
        /// The button player 1 fives click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BtnPlayer1FivesClick(object sender, RoutedEventArgs e)
        {
            this.Player1PickScore("Fives");
            this.player1Sum += int.Parse(this.btnPlayer1Fives.Content.ToString());
            this.player1TotalSum = this.player1Sum + this.player1LowerSectionSum;
            this.btnPlayer1Sum.Content = this.player1Sum;
            this.btnPlayer1TotalScore.Content = this.player1TotalSum;
        }

        /// <summary>
        /// The button player 1 sixes click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BtnPlayer1SixesClick(object sender, RoutedEventArgs e)
        {
            this.Player1PickScore("Sixes");
            this.player1Sum += int.Parse(this.btnPlayer1Sixes.Content.ToString());
            this.player1TotalSum = this.player1Sum + this.player1LowerSectionSum;
            this.btnPlayer1Sum.Content = this.player1Sum;
            this.btnPlayer1TotalScore.Content = this.player1TotalSum;
        }

        /// <summary>
        /// The button player 1 three of a kind click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BtnPlayer1ThreeOfAKindClick(object sender, RoutedEventArgs e)
        {
            this.Player1PickScore("ThreeOfAKind");
            this.player1LowerSectionSum += int.Parse(this.btnPlayer1ThreeOfAKind.Content.ToString());
            this.player1TotalSum = this.player1Sum + this.player1LowerSectionSum;
            this.btnPlayer1TotalScore.Content = this.player1TotalSum;
        }

        /// <summary>
        /// The button player 1 four of a kind click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BtnPlayer1FourOfAKindClick(object sender, RoutedEventArgs e)
        {
            this.Player1PickScore("FourOfAKind");
            this.player1LowerSectionSum += int.Parse(this.btnPlayer1FourOfAKind.Content.ToString());
            this.player1TotalSum = this.player1Sum + this.player1LowerSectionSum;
            this.btnPlayer1TotalScore.Content = this.player1TotalSum;
        }

        /// <summary>
        /// The button player 1 full house click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BtnPlayer1FullHouseClick(object sender, RoutedEventArgs e)
        {
            this.Player1PickScore("FullHouse");
            this.player1LowerSectionSum += int.Parse(this.btnPlayer1FullHouse.Content.ToString());
            this.player1TotalSum = this.player1Sum + this.player1LowerSectionSum;
            this.btnPlayer1TotalScore.Content = this.player1TotalSum;
        }

        /// <summary>
        /// The button player 1 small straight click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BtnPlayer1SmallStraightClick(object sender, RoutedEventArgs e)
        {
            this.Player1PickScore("SmallStraight");
            this.player1LowerSectionSum += int.Parse(this.btnPlayer1SmallStraight.Content.ToString());
            this.player1TotalSum = this.player1Sum + this.player1LowerSectionSum;
            this.btnPlayer1TotalScore.Content = this.player1TotalSum;
        }

        /// <summary>
        /// The button player 1 large straight click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BtnPlayer1LargeStraightClick(object sender, RoutedEventArgs e)
        {
            this.Player1PickScore("BigStraight");
            this.player1LowerSectionSum += int.Parse(this.btnPlayer1LargeStraight.Content.ToString());
            this.player1TotalSum = this.player1Sum + this.player1LowerSectionSum;
            this.btnPlayer1TotalScore.Content = this.player1TotalSum;
        }

        /// <summary>
        /// The button player 1 chance click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BtnPlayer1ChanceClick(object sender, RoutedEventArgs e)
        {
            this.Player1PickScore("Chance");
            this.player1LowerSectionSum += int.Parse(this.btnPlayer1Chance.Content.ToString());
            this.player1TotalSum = this.player1Sum + this.player1LowerSectionSum;
            this.btnPlayer1TotalScore.Content = this.player1TotalSum;
        }

        /// <summary>
        /// The button player 1 Yahtzee click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BtnPlayer1YahtzeeClick(object sender, RoutedEventArgs e)
        {
            this.Player1PickScore("Yahtzee");
            this.player1LowerSectionSum += int.Parse(this.btnPlayer1Yahtzee.Content.ToString());
            this.player1TotalSum = this.player1Sum + this.player1LowerSectionSum;
            this.btnPlayer1TotalScore.Content = this.player1TotalSum;
        }

        /// <summary>
        /// The reset buttons player 1.
        /// </summary>
        private void ResetButtonsPlayer1()
        {
            foreach (KeyValuePair<string, Button> pair in this.player1ScoreButtons)
            {
                if (!this.player1ScoresPlayed.Contains(pair.Key))
                {
                    pair.Value.IsEnabled = false;
                    pair.Value.Content = 0;
                }
            }
        }

        /// <summary>
        /// The reset dices player 1.
        /// </summary>
        private void ResetDicesPlayer1()
        {
            this.imgPlayer1Dice1.Source = new BitmapImage(new Uri("static/emptydice.png", UriKind.Relative));
            this.imgPlayer1Dice2.Source = new BitmapImage(new Uri("static/emptydice.png", UriKind.Relative));
            this.imgPlayer1Dice3.Source = new BitmapImage(new Uri("static/emptydice.png", UriKind.Relative));
            this.imgPlayer1Dice4.Source = new BitmapImage(new Uri("static/emptydice.png", UriKind.Relative));
            this.imgPlayer1Dice5.Source = new BitmapImage(new Uri("static/emptydice.png", UriKind.Relative));
        }

        /// <summary>
        /// The check victory.
        /// </summary>
        private void CheckVictory()
        {
            if (this.player1ScoreCount == 13)
            {
                var result = MessageBox.Show("Player 1 WON !!!\nDo you want to play again?", "VICTORY!!!", MessageBoxButton.YesNo);
                if (result == (MessageBoxResult)System.Windows.Forms.DialogResult.Yes)
                {
                    foreach (KeyValuePair<string, Button> pair in this.player1ScoreButtons)
                    {
                        pair.Value.IsEnabled = false;
                        pair.Value.Content = 0;
                    }

                    this.player1Sum = 0;
                    this.player1TotalSum = 0;
                    this.player1Bonus = 0;
                    this.player1LowerSectionSum = 0;
                    this.player1ScoreCount = 0;
                    this.player1ScoresPlayed = new List<string> { "TotalScore", "Sum", "Bonus" };
                    this.ControlDicesPlayer1(false);
                }
                else
                {
                    this.Close();
                }
            }
        }
    }
}
