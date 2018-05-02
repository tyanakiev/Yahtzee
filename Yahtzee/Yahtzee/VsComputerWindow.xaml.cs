// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VsComputerWindow.xaml.cs" company="NWTC">
//   Copyright
// </copyright>
// <summary>
//   Interaction logic for TwoPlayersWindow
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Yahtzee
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Automation.Peers;
    using System.Windows.Automation.Provider;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Interaction logic for TwoPlayersWindow
    /// </summary>
    public partial class VsComputerWindow : Window
    {
        /// <summary>
        /// The player 1 rolls.
        /// </summary>
        private int player1Rolls = 0;

        /// <summary>
        /// The Bender rolls.
        /// </summary>
        private int benderRolls = 0;

        /// <summary>
        /// The rolls.
        /// </summary>
        private int rolls = 3;

        /// <summary>
        /// The player 1 dices.
        /// </summary>
        private int[] player1Dices = new int[5];

        /// <summary>
        /// The Bender dices.
        /// </summary>
        private int[] benderDices = new int[5];

        /// <summary>
        /// The player 1 dices hold.
        /// </summary>
        private List<int> player1DicesHold = new List<int>();

        /// <summary>
        /// The Bender dices hold.
        /// </summary>
        private List<int> benderDicesHold = new List<int>();

        /// <summary>
        /// The player 1 scores played.
        /// </summary>
        private List<string> player1ScoresPlayed = new List<string> { "TotalScore", "Sum", "Bonus" };

        /// <summary>
        /// Bender scores played.
        /// </summary>
        private List<string> benderScoresPlayed = new List<string> { "TotalScore", "Sum", "Bonus" };

        /// <summary>
        /// The player 1 dices score.
        /// </summary>
        private CheckDices player1DicesScore = new CheckDices();

        /// <summary>
        /// Bender dices score.
        /// </summary>
        private CheckDices benderDicesScore = new CheckDices();

        /// <summary>
        /// The player 1 score.
        /// </summary>
        private Dictionary<string, int> player1Score;

        /// <summary>
        /// The player 2 score.
        /// </summary>
        private Dictionary<string, int> benderScore;

        /// <summary>
        /// The player 1 score buttons.
        /// </summary>
        private Dictionary<string, Button> player1ScoreButtons = new Dictionary<string, Button>();

        /// <summary>
        /// Bender score buttons.
        /// </summary>
        private Dictionary<string, Button> benderScoreButtons = new Dictionary<string, Button>();

        /// <summary>
        /// The turn machine.
        /// </summary>
        private TurnMachine turnMachine = new TurnMachine("Player1", "Bender");

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
        /// Bender sum.
        /// </summary>
        private int benderSum = 0;

        /// <summary>
        /// Bender sum for lower section.
        /// </summary>
        private int benderLowerSectionSum = 0;

        /// <summary>
        /// Bender bonus.
        /// </summary>
        private int benderBonus;

        /// <summary>
        /// Bender total sum.
        /// </summary>
        private int benderTotalSum = 0;

        /// <summary>
        /// The player 1 score count.
        /// </summary>
        private int player1ScoreCount = 0;

        /// <summary>
        /// The bender score count.
        /// </summary>
        private int benderScoreCount = 0;

        /// <summary>
        /// Bender quotes.
        /// </summary>
        private List<string> benderSpeach = new List<string>(
            new string[]
                {
                    "Another pointless turn where I accomplish nothing.",
                    "Of all the friends I've had, you're the first.",
                    "A grim day for robot-kind. But we can always build more killbots.",
                    "Don't roll like that; tragic romances always have a happy ending.",
                    "Let fly the white flag of war.", "Arrr! The laws of science be a harsh mistress.",
                    "My story is a lot like yours, only more interesting ‘cause it involves robots.",
                    "Not everyone turns out like their parents. Look at me: my parents were honest, hard-working people.",
                    "Great is OK, but amazing would be GREAT!",
                    "Yes! I'm going to be rich! You are too, but it's hard to get excited about that.",
                    "I can't afford to keep running people over. I'm not famous enough to get away with it.",
                    "Oh crap! It's a miracle!",
                    "I assure you, I barely know the meaning of the word labor.",
                    "And I'd do it again, and perhaps a third time. But that would be it.",
                    "I'm finally richer than those snooty ATM machines.",
                    "It's always so sad when a friend goes crazy and you have a have a big clambake and cook him! Yee-ha!",
                    "Life is hilariously cruel.", "Robots don't have any emotions and sometimes that makes me very sad."
                });

        /// <summary>
        /// Initializes a new instance of the <see cref="VsComputerWindow"/> class.
        /// </summary>
        public VsComputerWindow()
        {
            this.InitializeComponent();
            this.ControlDicesPlayer1(false);
            this.ControlDicesBender(false);
            this.IsEnabledButtonsPlayer1(false);
            this.IsEnabledButtonsBender(false);
            this.Player1ScoreButtonsMap();
            this.BenderScoreButtonsMap();
        }

        /// <summary>
        /// Gets or sets the turn machine.
        /// </summary>
        public TurnMachine TurnMachine
        {
            get
            {
                return this.turnMachine;
            }

            set
            {
                this.turnMachine = value;
            }
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
            if (this.TurnMachine.AskForTurn() == "Player1" && this.player1Rolls != 3)
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
                if (this.player1Rolls == 3)
                {
                    if (this.CheckForValidMovePlayer1())
                    {
                        this.AddScorePlayer1();
                    }
                    else
                    {
                        MessageBox.Show("You don't have any moves. It's Player 2 turn now.");
                        this.TurnMachine.GetTurn();
                        this.ResetDicesPlayer1();
                        this.player1Rolls = 0;
                        this.rolls = 3;
                        this.BendersTurn();
                    }

                    this.ControlDicesPlayer1(false);
                    this.UnholdImages();
                    this.player1DicesHold.Clear();
                }

                this.imgPlayer1Dice1.Source = new BitmapImage(new Uri("static/dice" + this.player1Dices[0] + ".png", UriKind.Relative));
                this.imgPlayer1Dice2.Source = new BitmapImage(new Uri("static/dice" + this.player1Dices[1] + ".png", UriKind.Relative));
                this.imgPlayer1Dice3.Source = new BitmapImage(new Uri("static/dice" + this.player1Dices[2] + ".png", UriKind.Relative));
                this.imgPlayer1Dice4.Source = new BitmapImage(new Uri("static/dice" + this.player1Dices[3] + ".png", UriKind.Relative));
                this.imgPlayer1Dice5.Source = new BitmapImage(new Uri("static/dice" + this.player1Dices[4] + ".png", UriKind.Relative));
                this.ResetButtonsPlayer1();
                this.AddScorePlayer1();
                this.rolls--;
                this.lblRollsLeft.Content = this.rolls;
            }
        }

        /// <summary>
        /// The button player 2 roll dice click.
        /// </summary>
        private void BendersTurn()
        {
            if (this.TurnMachine.AskForTurn() == "Bender" && this.benderRolls != 3)
            {
                Random rnd = new Random();
                for (int i = 0; i < 5; i++)
                {
                    if (!this.benderDicesHold.Contains(i))
                    {
                        this.benderDices[i] = rnd.Next(1, 7);
                    }
                }

                this.ControlDicesBender(true);
                this.benderRolls++;
                this.benderDicesScore.AssignDices(this.benderDices);
                this.benderScore = this.benderDicesScore.CheckCombinations();
                if (this.benderRolls == 3)
                {
                    if (this.CheckForValidMoveBender())
                    {
                        this.AddScoreBender();
                    }
                    else
                    {
                        MessageBox.Show("You don't have any moves. It's Player 1 turn now.");
                        this.TurnMachine.GetTurn();
                        this.ResetDicesBender();
                        this.benderRolls = 0;
                        this.rolls = 3;
                    }

                    this.ControlDicesBender(false);
                    this.UnholdImages();
                    this.benderDicesHold.Clear();
                }

                this.imgPlayer2Dice1.Source = new BitmapImage(new Uri("static/dice" + this.benderDices[0] + ".png", UriKind.Relative));
                this.imgPlayer2Dice2.Source = new BitmapImage(new Uri("static/dice" + this.benderDices[1] + ".png", UriKind.Relative));
                this.imgPlayer2Dice3.Source = new BitmapImage(new Uri("static/dice" + this.benderDices[2] + ".png", UriKind.Relative));
                this.imgPlayer2Dice4.Source = new BitmapImage(new Uri("static/dice" + this.benderDices[3] + ".png", UriKind.Relative));
                this.imgPlayer2Dice5.Source = new BitmapImage(new Uri("static/dice" + this.benderDices[4] + ".png", UriKind.Relative));
                this.ResetButtonsBender();
                this.AddScoreBender();
                this.rolls--;
                this.lblRollsLeft.Content = this.rolls;
                this.BenderPickScore();
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
        /// The check for valid move player 2.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool CheckForValidMoveBender()
        {
            foreach (var score in this.benderScore)
            {
                if (!this.benderScoresPlayed.Contains(score.Key))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// The player 1 dice click.
        /// </summary>
        /// <param name="image">
        /// The image.
        /// </param>
        /// <param name="dice">
        /// The dice.
        /// </param>
        private void Player1DiceClick(Image image, int dice)
        {
            if (image.Opacity == 1)
            {
                image.Opacity = 0.5;
                this.player1DicesHold.Add(dice);
            }
            else
            {
                image.Opacity = 1;
                this.player1DicesHold.Remove(dice);
            }
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
            this.Player1DiceClick(this.imgPlayer1Dice1, 0);
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
            this.Player1DiceClick(this.imgPlayer1Dice2, 1);
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
            this.Player1DiceClick(this.imgPlayer1Dice3, 2);
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
            this.Player1DiceClick(this.imgPlayer1Dice4, 3);
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
            this.Player1DiceClick(this.imgPlayer1Dice5, 4);
        }

        /// <summary>
        /// The player 2 dice click.
        /// </summary>
        /// <param name="image">
        /// The Image.
        /// </param>
        /// <param name="dice">
        /// The dice.
        /// </param>
        private void Player2DiceClick(Image image, int dice)
        {
            if (image.Opacity == 1)
            {
                image.Opacity = 0.5;
                this.benderDicesHold.Add(dice);
            }
            else
            {
                image.Opacity = 1;
                this.benderDicesHold.Remove(dice);
            }
        }

        /// <summary>
        /// The player 2 dice 1 click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Player2Dice1Click(object sender, RoutedEventArgs e)
        {
            this.Player2DiceClick(this.imgPlayer2Dice1, 0);
        }

        /// <summary>
        /// The player 2 dice 2 click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Player2Dice2Click(object sender, RoutedEventArgs e)
        {
            this.Player2DiceClick(this.imgPlayer2Dice2, 1);
        }

        /// <summary>
        /// The player 2 dice 3 click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Player2Dice3Click(object sender, RoutedEventArgs e)
        {
            this.Player2DiceClick(this.imgPlayer2Dice3, 2);
        }

        /// <summary>
        /// The player 2 dice 4 click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Player2Dice4Click(object sender, RoutedEventArgs e)
        {
            this.Player2DiceClick(this.imgPlayer2Dice4, 3);
        }

        /// <summary>
        /// The player 2 dice 5 click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Player2Dice5Click(object sender, RoutedEventArgs e)
        {
            this.Player2DiceClick(this.imgPlayer2Dice5, 4);
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
        /// Enables controls for player 2.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        private void ControlDicesBender(bool value)
        {
            this.imgPlayer2Dice1.IsEnabled = value;
            this.imgPlayer2Dice2.IsEnabled = value;
            this.imgPlayer2Dice3.IsEnabled = value;
            this.imgPlayer2Dice4.IsEnabled = value;
            this.imgPlayer2Dice5.IsEnabled = value;
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

            this.imgPlayer2Dice1.Opacity = 1;
            this.imgPlayer2Dice2.Opacity = 1;
            this.imgPlayer2Dice3.Opacity = 1;
            this.imgPlayer2Dice4.Opacity = 1;
            this.imgPlayer2Dice5.Opacity = 1;
        }

        /// <summary>
        /// Enables or disables buttons for player 1.
        /// </summary>
        /// <param name="status">
        /// The status.
        /// </param>
        private void IsEnabledButtonsPlayer1(bool status)
        {
            this.btnPlayer1Ones.IsEnabled = status;
            this.btnPlayer1Twos.IsEnabled = status;
            this.btnPlayer1Threes.IsEnabled = status;
            this.btnPlayer1Fours.IsEnabled = status;
            this.btnPlayer1Fives.IsEnabled = status;
            this.btnPlayer1Sixes.IsEnabled = status;
            this.btnPlayer1Sum.IsEnabled = status;
            this.btnPlayer1Bonus.IsEnabled = status;
            this.btnPlayer1ThreeOfAKind.IsEnabled = status;
            this.btnPlayer1FourOfAKind.IsEnabled = status;
            this.btnPlayer1FullHouse.IsEnabled = status;
            this.btnPlayer1SmallStraight.IsEnabled = status;
            this.btnPlayer1LargeStraight.IsEnabled = status;
            this.btnPlayer1Chance.IsEnabled = status;
            this.btnPlayer1Yahtzee.IsEnabled = status;
            this.btnPlayer1TotalScore.IsEnabled = status;
        }

        /// <summary>
        /// Enables or disables buttons for player 2.
        /// </summary>
        /// <param name="status">
        /// The status.
        /// </param>
        private void IsEnabledButtonsBender(bool status)
        {
            this.btnPlayer2Ones.IsEnabled = status;
            this.btnPlayer2Twos.IsEnabled = status;
            this.btnPlayer2Threes.IsEnabled = status;
            this.btnPlayer2Fours.IsEnabled = status;
            this.btnPlayer2Fives.IsEnabled = status;
            this.btnPlayer2Sixes.IsEnabled = status;
            this.btnPlayer2Sum.IsEnabled = status;
            this.btnPlayer2Bonus.IsEnabled = status;
            this.btnPlayer2ThreeOfAKind.IsEnabled = status;
            this.btnPlayer2FourOfAKind.IsEnabled = status;
            this.btnPlayer2FullHouse.IsEnabled = status;
            this.btnPlayer2SmallStraight.IsEnabled = status;
            this.btnPlayer2LargeStraight.IsEnabled = status;
            this.btnPlayer2Chance.IsEnabled = status;
            this.btnPlayer2Yahtzee.IsEnabled = status;
            this.btnPlayer2TotalScore.IsEnabled = status;
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
        /// The player 2 score buttons map.
        /// </summary>
        private void BenderScoreButtonsMap()
        {
            this.benderScoreButtons.Add("Ones", this.btnPlayer2Ones);
            this.benderScoreButtons.Add("Twos", this.btnPlayer2Twos);
            this.benderScoreButtons.Add("Threes", this.btnPlayer2Threes);
            this.benderScoreButtons.Add("Fours", this.btnPlayer2Fours);
            this.benderScoreButtons.Add("Fives", this.btnPlayer2Fives);
            this.benderScoreButtons.Add("Sixes", this.btnPlayer2Sixes);
            this.benderScoreButtons.Add("Yahtzee", this.btnPlayer2Yahtzee);
            this.benderScoreButtons.Add("BigStraight", this.btnPlayer2LargeStraight);
            this.benderScoreButtons.Add("SmallStraight", this.btnPlayer2SmallStraight);
            this.benderScoreButtons.Add("FourOfAKind", this.btnPlayer2FourOfAKind);
            this.benderScoreButtons.Add("FullHouse", this.btnPlayer2FullHouse);
            this.benderScoreButtons.Add("ThreeOfAKind", this.btnPlayer2ThreeOfAKind);
            this.benderScoreButtons.Add("Sum", this.btnPlayer2Sum);
            this.benderScoreButtons.Add("Bonus", this.btnPlayer2Bonus);
            this.benderScoreButtons.Add("Chance", this.btnPlayer2Chance);
            this.benderScoreButtons.Add("TotalScore", this.btnPlayer2TotalScore);
        }

        /// <summary>
        /// The add score player 1.
        /// </summary>
        private void AddScorePlayer1()
        {
            this.IsEnabledButtonsPlayer1(true);
            foreach (var combination in this.player1ScoresPlayed)
            {
                this.player1ScoreButtons[combination].IsEnabled = false;
            }

            foreach (KeyValuePair<string, int> pair in this.player1Score)
            {
                if (!this.player1ScoresPlayed.Contains(pair.Key))
                {
                    this.player1ScoreButtons[pair.Key].Content = pair.Value;
                }
            }
        }

        /// <summary>
        /// The add score player 2.
        /// </summary>
        private void AddScoreBender()
        {
            this.IsEnabledButtonsBender(true);
            foreach (var combination in this.benderScoresPlayed)
            {
                this.benderScoreButtons[combination].IsEnabled = false;
            }

            foreach (KeyValuePair<string, int> pair in this.benderScore)
            {
                if (!this.benderScoresPlayed.Contains(pair.Key))
                {
                    this.benderScoreButtons[pair.Key].Content = pair.Value;
                }
                else
                {
                    this.benderScoreButtons[pair.Key].IsEnabled = false;
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
            this.ControlDicesPlayer1(false);
            this.player1ScoresPlayed.Add(dice);
            this.IsEnabledButtonsPlayer1(false);
            this.ResetButtonsPlayer1();
            this.TurnMachine.GetTurn();
            this.lblPlayerTurn.Content = this.TurnMachine.AskForTurn();
            this.player1Rolls = 0;
            this.CheckAndAddBonusPlayer1();
            this.ResetDicesPlayer1();
            this.UnholdImages();
            this.player1ScoreCount++;
            this.CheckVictory();
            this.rolls = 3;
            this.lblRollsLeft.Content = '3';
            this.BendersTurn();
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
        /// The check and add bonus player 2.
        /// </summary>
        private void CheckAndAddBonusPlayer2()
        {
            if (this.benderSum > 63)
            {
                this.benderBonus = 35;
                this.btnPlayer2Bonus.Content = 35;
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
        /// The reset buttons player 2.
        /// </summary>
        private void ResetButtonsBender()
        {
            foreach (KeyValuePair<string, Button> pair in this.benderScoreButtons)
            {
                if (!this.benderScoresPlayed.Contains(pair.Key))
                {
                    pair.Value.IsEnabled = false;
                    pair.Value.Content = 0;
                }
            }
        }

        /// <summary>
        /// The player 2 pick score.
        /// </summary>
        /// <param name="dice">
        /// The dice.
        /// </param>
        private void Player2PickScore(string dice)
        {
            Random rnd = new Random();
            int r = rnd.Next(this.benderSpeach.Count);
            this.txtBoxBenderSpeach.Text = this.benderSpeach[r];
            this.benderScoresPlayed.Add(dice);
            this.ControlDicesBender(false);
            this.IsEnabledButtonsBender(false);
            this.ResetButtonsBender();
            this.TurnMachine.GetTurn();
            this.lblPlayerTurn.Content = this.TurnMachine.AskForTurn();
            this.benderRolls = 0;
            this.CheckAndAddBonusPlayer2();
            this.ResetDicesBender();
            this.UnholdImages();
            this.benderScoreCount++;
            this.CheckVictory();
            this.rolls = 3;
            this.lblRollsLeft.Content = '3';
        }

        /// <summary>
        /// The button player 2 ones click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BtnPlayer2OnesClick(object sender, RoutedEventArgs e)
        {
            this.Player2PickScore("Ones");
            this.benderSum += int.Parse(this.btnPlayer2Ones.Content.ToString());
            this.benderTotalSum = this.benderSum + this.benderLowerSectionSum;
            this.btnPlayer2Sum.Content = this.benderSum;
            this.btnPlayer2TotalScore.Content = this.benderTotalSum;
        }

        /// <summary>
        /// The button player 2 twos_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BtnPlayer2TwosClick(object sender, RoutedEventArgs e)
        {
            this.Player2PickScore("Twos");
            this.benderSum += int.Parse(this.btnPlayer2Twos.Content.ToString());
            this.benderTotalSum = this.benderSum + this.benderLowerSectionSum;
            this.btnPlayer2Sum.Content = this.benderSum;
            this.btnPlayer2TotalScore.Content = this.benderTotalSum;
        }

        /// <summary>
        /// The button player 2 threes click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BtnPlayer2ThreesClick(object sender, RoutedEventArgs e)
        {
            this.Player2PickScore("Threes");
            this.benderSum += int.Parse(this.btnPlayer2Threes.Content.ToString());
            this.benderTotalSum = this.benderSum + this.benderLowerSectionSum;
            this.btnPlayer2Sum.Content = this.benderSum;
            this.btnPlayer2TotalScore.Content = this.benderTotalSum;
        }

        /// <summary>
        /// The button player 2 fours click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BtnPlayer2FoursClick(object sender, RoutedEventArgs e)
        {
            this.Player2PickScore("Fours");
            this.benderSum += int.Parse(this.btnPlayer2Fours.Content.ToString());
            this.benderTotalSum = this.benderSum + this.benderLowerSectionSum;
            this.btnPlayer2Sum.Content = this.benderSum;
            this.btnPlayer2TotalScore.Content = this.benderTotalSum;
        }

        /// <summary>
        /// The button player 2 fives click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BtnPlayer2FivesClick(object sender, RoutedEventArgs e)
        {
            this.Player2PickScore("Fives");
            this.benderSum += int.Parse(this.btnPlayer2Fives.Content.ToString());
            this.benderTotalSum = this.benderSum + this.benderLowerSectionSum;
            this.btnPlayer2Sum.Content = this.benderSum;
            this.btnPlayer2TotalScore.Content = this.benderTotalSum;
        }

        /// <summary>
        /// The button player 2 sixes click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BtnPlayer2SixesClick(object sender, RoutedEventArgs e)
        {
            this.Player2PickScore("Sixes");
            this.benderSum += int.Parse(this.btnPlayer2Sixes.Content.ToString());
            this.benderTotalSum = this.benderSum + this.benderLowerSectionSum;
            this.btnPlayer2Sum.Content = this.benderSum;
            this.btnPlayer2TotalScore.Content = this.benderTotalSum;
        }

        /// <summary>
        /// The button player 2 three of a kind click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BtnPlayer2ThreeOfAKindClick(object sender, RoutedEventArgs e)
        {
            this.Player2PickScore("ThreeOfAKind");
            this.benderLowerSectionSum += int.Parse(this.btnPlayer2ThreeOfAKind.Content.ToString());
            this.benderTotalSum = this.benderSum + this.benderLowerSectionSum;
            this.btnPlayer2TotalScore.Content = this.benderTotalSum;
        }

        /// <summary>
        /// The button player 2 four of a kind click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BtnPlayer2FourOfAKindClick(object sender, RoutedEventArgs e)
        {
            this.Player2PickScore("FourOfAKind");
            this.benderLowerSectionSum += int.Parse(this.btnPlayer2FourOfAKind.Content.ToString());
            this.benderTotalSum = this.benderSum + this.benderLowerSectionSum;
            this.btnPlayer2TotalScore.Content = this.benderTotalSum;
        }

        /// <summary>
        /// The button player 2 full house click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BtnPlayer2FullHouseClick(object sender, RoutedEventArgs e)
        {
            this.Player2PickScore("FullHouse");
            this.benderLowerSectionSum += int.Parse(this.btnPlayer2FullHouse.Content.ToString());
            this.benderTotalSum = this.benderSum + this.benderLowerSectionSum;
            this.btnPlayer2TotalScore.Content = this.benderTotalSum;
        }

        /// <summary>
        /// The button player 2 small straight click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BtnPlayer2SmallStraightClick(object sender, RoutedEventArgs e)
        {
            this.Player2PickScore("SmallStraight");
            this.benderLowerSectionSum += int.Parse(this.btnPlayer2SmallStraight.Content.ToString());
            this.benderTotalSum = this.benderSum + this.benderLowerSectionSum;
            this.btnPlayer2TotalScore.Content = this.benderTotalSum;
        }

        /// <summary>
        /// The button player 2 large straight click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BtnPlayer2LargeStraightClick(object sender, RoutedEventArgs e)
        {
            this.Player2PickScore("BigStraight");
            this.benderLowerSectionSum += int.Parse(this.btnPlayer2LargeStraight.Content.ToString());
            this.benderTotalSum = this.benderSum + this.benderLowerSectionSum;
            this.btnPlayer2TotalScore.Content = this.benderTotalSum;
        }

        /// <summary>
        /// The button player 2 chance click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BtnPlayer2ChanceClick(object sender, RoutedEventArgs e)
        {
            this.Player2PickScore("Chance");
            this.benderLowerSectionSum += int.Parse(this.btnPlayer2Chance.Content.ToString());
            this.benderTotalSum = this.benderSum + this.benderLowerSectionSum;
            this.btnPlayer2TotalScore.Content = this.benderTotalSum;
        }

        /// <summary>
        /// The button player 2 Yahtzee click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BtnPlayer2YahtzeeClick(object sender, RoutedEventArgs e)
        {
            this.Player2PickScore("Yahtzee");
            this.benderLowerSectionSum += int.Parse(this.btnPlayer2Yahtzee.Content.ToString());
            this.benderTotalSum = this.benderSum + this.benderLowerSectionSum;
            this.btnPlayer2TotalScore.Content = this.benderTotalSum;
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
        /// The reset dices player 2.
        /// </summary>
        private void ResetDicesBender()
        {
            this.imgPlayer2Dice1.Source = new BitmapImage(new Uri("static/emptydice.png", UriKind.Relative));
            this.imgPlayer2Dice2.Source = new BitmapImage(new Uri("static/emptydice.png", UriKind.Relative));
            this.imgPlayer2Dice3.Source = new BitmapImage(new Uri("static/emptydice.png", UriKind.Relative));
            this.imgPlayer2Dice4.Source = new BitmapImage(new Uri("static/emptydice.png", UriKind.Relative));
            this.imgPlayer2Dice5.Source = new BitmapImage(new Uri("static/emptydice.png", UriKind.Relative));
        }

        /// <summary>
        /// The check victory.
        /// </summary>
        private void CheckVictory()
        {
            if (this.player1ScoreCount == this.benderScoreCount && this.player1ScoreCount == 13)
            {
                if (this.player1TotalSum > this.benderTotalSum)
                {
                    MessageBox.Show("You WON !!!");
                }
                else if (this.player1TotalSum < this.benderTotalSum)
                {
                    MessageBox.Show("Bender WON !!!");
                }
                else
                {
                    MessageBox.Show("DRAW !!!");
                }

                MessageBoxResult messageBoxResult = MessageBox.Show("Do you want to play new game?", "Victory", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    VsComputerWindow newEasyComputerWindow = new VsComputerWindow();
                    newEasyComputerWindow.Left = this.Left;
                    newEasyComputerWindow.Top = this.Top;
                    this.Close();
                    newEasyComputerWindow.Show();
                }
                else
                {
                    this.Close();
                }
            }
        }

        /// <summary>
        /// The bender pick score.
        /// </summary>
        private async void BenderPickScore()
        {
            await this.PutTaskDelay();
            int tmpHighestScore = -1;
            string currentScoreName = string.Empty;

            // Checks for highest possible combination from the first dice.
            foreach (KeyValuePair<string, int> pair in this.benderScore)
            {
                if (!this.benderScoresPlayed.Contains(pair.Key))
                {
                    if (tmpHighestScore < pair.Value)
                    {
                        tmpHighestScore = pair.Value;
                        currentScoreName = pair.Key;
                    }
                }
            }

            // If all of the possible values are 0, just pick one.
            if (currentScoreName == string.Empty)
            {
                foreach (KeyValuePair<string, Button> pair in this.benderScoreButtons)
                {
                    if (!this.benderScoresPlayed.Contains(pair.Key))
                    {
                        // Simulate a button click.
                        ButtonAutomationPeer peer = new ButtonAutomationPeer(pair.Value);
                        IInvokeProvider bendersButtonOfChoice = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                        bendersButtonOfChoice.Invoke();
                        break;
                    }
                }
            }
            else
            {
                // Simulate a button click.
                ButtonAutomationPeer peer = new ButtonAutomationPeer(this.benderScoreButtons[currentScoreName]);
                IInvokeProvider bendersButtonOfChoice = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                bendersButtonOfChoice.Invoke();
            }
        }

        /// <summary>
        /// The put task delay.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task PutTaskDelay()
        {
            await Task.Delay(2000);
        }

        /// <summary>
        /// The button exit game click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BtnExitGameClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// The button reset game click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BtnResetGameClick(object sender, RoutedEventArgs e)
        {
            VsComputerWindow easyComputerWindow = new VsComputerWindow();
            easyComputerWindow.Left = this.Left;
            easyComputerWindow.Top = this.Top;
            this.Close();
            easyComputerWindow.Show();
        }
    }
}
