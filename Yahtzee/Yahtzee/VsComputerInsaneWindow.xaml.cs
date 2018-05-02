// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VsComputerInsaneWindow.xaml.cs" company="NWTC">
//   Copyright
// </copyright>
// <summary>
//   Interaction logic for VsComputerInsaneWindow
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
    /// Interaction logic for ComputerInsaneWindow
    /// </summary>
    public partial class VsComputerInsaneWindow : Window
    {
        /// <summary>
        /// The insane computer.
        /// </summary>
        private Insane insaneComputer = new Insane();

        /// <summary>
        /// The player 1 rolls.
        /// </summary>
        private int player1Rolls = 0;

        /// <summary>
        /// The R2D2 rolls.
        /// </summary>
        private int r2d2Rolls = 0;

        /// <summary>
        /// The rolls.
        /// </summary>
        private int rolls = 3;

        /// <summary>
        /// The player 1 dices.
        /// </summary>
        private int[] player1Dices = new int[5];

        /// <summary>
        /// The R2D2 dices.
        /// </summary>
        private int[] r2d2Dices = new int[5];

        /// <summary>
        /// The player 1 dices hold.
        /// </summary>
        private List<int> player1DicesHold = new List<int>();

        /// <summary>
        /// The R2D2 dices hold.
        /// </summary>
        private List<int> r2d2DicesHold = new List<int>();

        /// <summary>
        /// The player 1 scores played.
        /// </summary>
        private List<string> player1ScoresPlayed = new List<string> { "TotalScore", "Sum", "Bonus" };

        /// <summary>
        /// R2D2 scores played.
        /// </summary>
        private List<string> r2d2ScoresPlayed = new List<string> { "TotalScore", "Sum", "Bonus" };

        /// <summary>
        /// The player 1 dices score.
        /// </summary>
        private CheckDices player1DicesScore = new CheckDices();

        /// <summary>
        /// R2D2 dices score.
        /// </summary>
        private CheckDices r2d2DicesScore = new CheckDices();

        /// <summary>
        /// The player 1 score.
        /// </summary>
        private Dictionary<string, int> player1Score;

        /// <summary>
        /// The player 2 score.
        /// </summary>
        private Dictionary<string, int> r2d2Score;

        /// <summary>
        /// The player 1 score buttons.
        /// </summary>
        private Dictionary<string, Button> player1ScoreButtons = new Dictionary<string, Button>();

        /// <summary>
        /// R2D2 score buttons.
        /// </summary>
        private Dictionary<string, Button> r2d2ScoreButtons = new Dictionary<string, Button>();

        /// <summary>
        /// The r 2 d 2 image map.
        /// </summary>
        private Dictionary<int, Image> r2d2ImageMap = new Dictionary<int, Image>();

        /// <summary>
        /// The turn machine.
        /// </summary>
        private TurnMachine turnMachine = new TurnMachine("Player1", "R2D2");

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
        /// R2D2 sum.
        /// </summary>
        private int r2d2Sum = 0;

        /// <summary>
        /// R2D2 sum for lower section.
        /// </summary>
        private int r2d2LowerSectionSum = 0;

        /// <summary>
        /// R2D2 bonus.
        /// </summary>
        private int r2d2Bonus;

        /// <summary>
        /// R2D2 total sum.
        /// </summary>
        private int r2d2TotalSum = 0;

        /// <summary>
        /// The player 1 score count.
        /// </summary>
        private int player1ScoreCount = 0;

        /// <summary>
        /// The R2D2 score count.
        /// </summary>
        private int r2d2ScoreCount = 0;

        /// <summary>
        /// R2D2 quotes.
        /// </summary>
        private List<string> r2d2Speach = new List<string>(
            new string[]
                {
                    "Help me, Obi-Wan Kenobi. You’re my only hope.",
                    "I find your lack of faith disturbing.",
                    "The Force will be with you. Always.",
                    "Never tell me the odds!",
                    "Do. Or do not. There is no try.",
                    "No. I am your father.",
                    "There’s always a bigger fish.",
                    "Fear is the path to the dark side. Fear leads to anger; anger leads to hate; hate leads to suffering. I sense much fear in you.",
                    "I’m just a simple man trying to make my way in the universe.",
                    "Power! Unlimited power!",
                    "So this is how liberty dies. With thunderous applause.",
                    "Chewie, we’re home.",
                    "I’m one with the Force. The Force is with me."
                });

        /// <summary>
        /// Initializes a new instance of the <see cref="VsComputerInsaneWindow"/> class.
        /// </summary>
        public VsComputerInsaneWindow()
        {
            this.InitializeComponent();
            this.ControlDicesPlayer1(false);
            this.ControlDicesR2D2(false);
            this.IsEnabledButtonsPlayer1(false);
            this.IsEnabledButtonsR2D2(false);
            this.Player1ScoreButtonsMap();
            this.R2D2ScoreButtonsMap();
            this.R2D2ImagesMap();
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
                        MessageBox.Show("Sorry, you don't have any moves that can give you points. Just pick one of those good looking zero.");
                        this.ResetDicesPlayer1();
                        this.rolls = 3;
                    }

                    this.ControlDicesPlayer1(false);
                    this.UnholdImagesPlayer1();
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
        /// R2D2 roll dice click.
        /// </summary>
        private async void R2D2Turn()
        {
            if (this.TurnMachine.AskForTurn() == "R2D2" && this.r2d2Rolls != 3)
            {
                Random rnd = new Random();
                for (int i = 0; i < 5; i++)
                {
                    if (!this.r2d2DicesHold.Contains(i))
                    {
                        this.r2d2Dices[i] = rnd.Next(1, 7);
                    }
                }

                // this.r2d2Dices = new int[] { 1, 3, 4, 5, 5 }; - Just for testing
                this.insaneComputer.R2D2ScoresPlayed = this.r2d2ScoresPlayed;
                this.ControlDicesR2D2(true);
                this.r2d2Rolls++;
                this.r2d2DicesScore.AssignDices(this.r2d2Dices);
                this.r2d2Score = this.r2d2DicesScore.CheckCombinations();
                if (this.r2d2Rolls == 3)
                {
                    if (this.CheckForValidMoveR2D2())
                    {
                        this.AddScoreR2D2();
                    }

                    this.R2D2PickScore();
                    this.ControlDicesR2D2(false);
                    this.UnholdImagesPlayer2();
                    this.r2d2DicesHold.Clear();
                }

                this.imgPlayer2Dice1.Source = new BitmapImage(new Uri("static/dice" + this.r2d2Dices[0] + ".png", UriKind.Relative));
                this.imgPlayer2Dice2.Source = new BitmapImage(new Uri("static/dice" + this.r2d2Dices[1] + ".png", UriKind.Relative));
                this.imgPlayer2Dice3.Source = new BitmapImage(new Uri("static/dice" + this.r2d2Dices[2] + ".png", UriKind.Relative));
                this.imgPlayer2Dice4.Source = new BitmapImage(new Uri("static/dice" + this.r2d2Dices[3] + ".png", UriKind.Relative));
                this.imgPlayer2Dice5.Source = new BitmapImage(new Uri("static/dice" + this.r2d2Dices[4] + ".png", UriKind.Relative));
                this.ResetButtonsR2D2();
                this.AddScoreR2D2();
                this.rolls--;
                this.lblRollsLeft.Content = this.rolls;
                this.insaneComputer.R2D2Dices = this.r2d2Dices;
                if (this.insaneComputer.Compute())
                {
                    this.UnholdImagesPlayer2();
                    this.r2d2DicesHold.Clear();
                    foreach (var holdDiceIndex in this.insaneComputer.HoldDiceIndex)
                    {
                        this.Player2DiceClick(this.r2d2ImageMap[holdDiceIndex], holdDiceIndex);
                    }

                    await this.PutTaskDelay();
                    this.R2D2Turn();
                }
                else
                {
                    if (this.r2d2Rolls != 3)
                    {
                        this.R2D2PickScore();
                    }
                        
                    this.UnholdImagesPlayer2();
                    this.r2d2DicesHold.Clear();
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
        /// The check for valid move R2D2.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool CheckForValidMoveR2D2()
        {
            foreach (var score in this.r2d2Score)
            {
                if (!this.r2d2ScoresPlayed.Contains(score.Key))
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
        /// The image.
        /// </param>
        /// <param name="dice">
        /// The dice.
        /// </param>
        private void Player2DiceClick(Image image, int dice)
        {
            if (image.Opacity == 1)
            {
                image.Opacity = 0.5;
                this.r2d2DicesHold.Add(dice);
            }
            else
            {
                image.Opacity = 1;
                this.r2d2DicesHold.Remove(dice);
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
        private void ControlDicesR2D2(bool value)
        {
            this.imgPlayer2Dice1.IsEnabled = value;
            this.imgPlayer2Dice2.IsEnabled = value;
            this.imgPlayer2Dice3.IsEnabled = value;
            this.imgPlayer2Dice4.IsEnabled = value;
            this.imgPlayer2Dice5.IsEnabled = value;
        }

        /// <summary>
        /// Removes opacity from player 1 dice images.
        /// </summary>
        private void UnholdImagesPlayer1()
        {
            this.imgPlayer1Dice1.Opacity = 1;
            this.imgPlayer1Dice2.Opacity = 1;
            this.imgPlayer1Dice3.Opacity = 1;
            this.imgPlayer1Dice4.Opacity = 1;
            this.imgPlayer1Dice5.Opacity = 1;
        }

        /// <summary>
        /// TRemoves opacity from player 2 dice images.
        /// </summary>
        private void UnholdImagesPlayer2()
        {
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
        private void IsEnabledButtonsR2D2(bool status)
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
        /// The R2D2 images map.
        /// </summary>
        private void R2D2ImagesMap()
        {
            this.r2d2ImageMap.Add(0, this.imgPlayer2Dice1);
            this.r2d2ImageMap.Add(1, this.imgPlayer2Dice2);
            this.r2d2ImageMap.Add(2, this.imgPlayer2Dice3);
            this.r2d2ImageMap.Add(3, this.imgPlayer2Dice4);
            this.r2d2ImageMap.Add(4, this.imgPlayer2Dice5);
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
        private void R2D2ScoreButtonsMap()
        {
            this.r2d2ScoreButtons.Add("Ones", this.btnPlayer2Ones);
            this.r2d2ScoreButtons.Add("Twos", this.btnPlayer2Twos);
            this.r2d2ScoreButtons.Add("Threes", this.btnPlayer2Threes);
            this.r2d2ScoreButtons.Add("Fours", this.btnPlayer2Fours);
            this.r2d2ScoreButtons.Add("Fives", this.btnPlayer2Fives);
            this.r2d2ScoreButtons.Add("Sixes", this.btnPlayer2Sixes);
            this.r2d2ScoreButtons.Add("Yahtzee", this.btnPlayer2Yahtzee);
            this.r2d2ScoreButtons.Add("BigStraight", this.btnPlayer2LargeStraight);
            this.r2d2ScoreButtons.Add("SmallStraight", this.btnPlayer2SmallStraight);
            this.r2d2ScoreButtons.Add("FourOfAKind", this.btnPlayer2FourOfAKind);
            this.r2d2ScoreButtons.Add("FullHouse", this.btnPlayer2FullHouse);
            this.r2d2ScoreButtons.Add("ThreeOfAKind", this.btnPlayer2ThreeOfAKind);
            this.r2d2ScoreButtons.Add("Sum", this.btnPlayer2Sum);
            this.r2d2ScoreButtons.Add("Bonus", this.btnPlayer2Bonus);
            this.r2d2ScoreButtons.Add("Chance", this.btnPlayer2Chance);
            this.r2d2ScoreButtons.Add("TotalScore", this.btnPlayer2TotalScore);
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
        private void AddScoreR2D2()
        {
            this.IsEnabledButtonsR2D2(true);
            foreach (var combination in this.r2d2ScoresPlayed)
            {
                this.r2d2ScoreButtons[combination].IsEnabled = false;
            }

            foreach (KeyValuePair<string, int> pair in this.r2d2Score)
            {
                if (!this.r2d2ScoresPlayed.Contains(pair.Key))
                {
                    this.r2d2ScoreButtons[pair.Key].Content = pair.Value;
                }
                else
                {
                    this.r2d2ScoreButtons[pair.Key].IsEnabled = false;
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
            this.UnholdImagesPlayer1();
            this.player1ScoreCount++;
            this.CheckVictory();
            this.rolls = 3;
            this.lblRollsLeft.Content = '3';
            this.R2D2Turn();
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
            if (this.r2d2Sum > 63)
            {
                this.r2d2Bonus = 35;
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
        private void ResetButtonsR2D2()
        {
            foreach (KeyValuePair<string, Button> pair in this.r2d2ScoreButtons)
            {
                if (!this.r2d2ScoresPlayed.Contains(pair.Key))
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
            int r = rnd.Next(this.r2d2Speach.Count);
            this.txtBoxR2D2Speach.Text = this.r2d2Speach[r];
            this.r2d2ScoresPlayed.Add(dice);
            this.ControlDicesR2D2(false);
            this.IsEnabledButtonsR2D2(false);
            this.ResetButtonsR2D2();
            this.TurnMachine.GetTurn();
            this.lblPlayerTurn.Content = this.TurnMachine.AskForTurn();
            this.r2d2Rolls = 0;
            this.CheckAndAddBonusPlayer2();
            this.ResetDicesR2D2();
            this.UnholdImagesPlayer2();
            this.r2d2ScoreCount++;
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
            this.r2d2Sum += int.Parse(this.btnPlayer2Ones.Content.ToString());
            this.r2d2TotalSum = this.r2d2Sum + this.r2d2LowerSectionSum;
            this.btnPlayer2Sum.Content = this.r2d2Sum;
            this.btnPlayer2TotalScore.Content = this.r2d2TotalSum;
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
            this.r2d2Sum += int.Parse(this.btnPlayer2Twos.Content.ToString());
            this.r2d2TotalSum = this.r2d2Sum + this.r2d2LowerSectionSum;
            this.btnPlayer2Sum.Content = this.r2d2Sum;
            this.btnPlayer2TotalScore.Content = this.r2d2TotalSum;
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
            this.r2d2Sum += int.Parse(this.btnPlayer2Threes.Content.ToString());
            this.r2d2TotalSum = this.r2d2Sum + this.r2d2LowerSectionSum;
            this.btnPlayer2Sum.Content = this.r2d2Sum;
            this.btnPlayer2TotalScore.Content = this.r2d2TotalSum;
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
            this.r2d2Sum += int.Parse(this.btnPlayer2Fours.Content.ToString());
            this.r2d2TotalSum = this.r2d2Sum + this.r2d2LowerSectionSum;
            this.btnPlayer2Sum.Content = this.r2d2Sum;
            this.btnPlayer2TotalScore.Content = this.r2d2TotalSum;
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
            this.r2d2Sum += int.Parse(this.btnPlayer2Fives.Content.ToString());
            this.r2d2TotalSum = this.r2d2Sum + this.r2d2LowerSectionSum;
            this.btnPlayer2Sum.Content = this.r2d2Sum;
            this.btnPlayer2TotalScore.Content = this.r2d2TotalSum;
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
            this.r2d2Sum += int.Parse(this.btnPlayer2Sixes.Content.ToString());
            this.r2d2TotalSum = this.r2d2Sum + this.r2d2LowerSectionSum;
            this.btnPlayer2Sum.Content = this.r2d2Sum;
            this.btnPlayer2TotalScore.Content = this.r2d2TotalSum;
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
            this.r2d2LowerSectionSum += int.Parse(this.btnPlayer2ThreeOfAKind.Content.ToString());
            this.r2d2TotalSum = this.r2d2Sum + this.r2d2LowerSectionSum;
            this.btnPlayer2TotalScore.Content = this.r2d2TotalSum;
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
            this.r2d2LowerSectionSum += int.Parse(this.btnPlayer2FourOfAKind.Content.ToString());
            this.r2d2TotalSum = this.r2d2Sum + this.r2d2LowerSectionSum;
            this.btnPlayer2TotalScore.Content = this.r2d2TotalSum;
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
            this.r2d2LowerSectionSum += int.Parse(this.btnPlayer2FullHouse.Content.ToString());
            this.r2d2TotalSum = this.r2d2Sum + this.r2d2LowerSectionSum;
            this.btnPlayer2TotalScore.Content = this.r2d2TotalSum;
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
            this.r2d2LowerSectionSum += int.Parse(this.btnPlayer2SmallStraight.Content.ToString());
            this.r2d2TotalSum = this.r2d2Sum + this.r2d2LowerSectionSum;
            this.btnPlayer2TotalScore.Content = this.r2d2TotalSum;
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
            this.r2d2LowerSectionSum += int.Parse(this.btnPlayer2LargeStraight.Content.ToString());
            this.r2d2TotalSum = this.r2d2Sum + this.r2d2LowerSectionSum;
            this.btnPlayer2TotalScore.Content = this.r2d2TotalSum;
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
            this.r2d2LowerSectionSum += int.Parse(this.btnPlayer2Chance.Content.ToString());
            this.r2d2TotalSum = this.r2d2Sum + this.r2d2LowerSectionSum;
            this.btnPlayer2TotalScore.Content = this.r2d2TotalSum;
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
            this.r2d2LowerSectionSum += int.Parse(this.btnPlayer2Yahtzee.Content.ToString());
            this.r2d2TotalSum = this.r2d2Sum + this.r2d2LowerSectionSum;
            this.btnPlayer2TotalScore.Content = this.r2d2TotalSum;
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
        private void ResetDicesR2D2()
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
            if (this.player1ScoreCount == this.r2d2ScoreCount && this.player1ScoreCount == 13)
            {
                if (this.player1TotalSum > this.r2d2TotalSum)
                {
                    MessageBox.Show("You WON !!!");
                }
                else if (this.player1TotalSum < this.r2d2TotalSum)
                {
                    MessageBox.Show("R2D2 WON !!!");
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
        /// The R2D2 pick score.
        /// </summary>
        private async void R2D2PickScore()
        { 
            await this.PutTaskDelay();
            this.r2d2DicesHold.Clear();
            int tmpHighestScore = -1;
            string currentScoreName = string.Empty;

            // Checks for highest possible combination from the first dice.
            foreach (KeyValuePair<string, int> pair in this.r2d2Score)
            {
                if (!this.r2d2ScoresPlayed.Contains(pair.Key))
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
                foreach (KeyValuePair<string, Button> pair in this.r2d2ScoreButtons)
                {
                    if (!this.r2d2ScoresPlayed.Contains(pair.Key))
                    {
                        // Simulate a button click.
                        ButtonAutomationPeer peer = new ButtonAutomationPeer(pair.Value);
                        IInvokeProvider r2d2ButtonOfChoice = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                        r2d2ButtonOfChoice.Invoke();
                        break;
                    }
                }
            }
            else
            {
                // Simulate a button click.
                ButtonAutomationPeer peer = new ButtonAutomationPeer(this.r2d2ScoreButtons[currentScoreName]);
                IInvokeProvider r2d2ButtonOfChoice = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                r2d2ButtonOfChoice.Invoke();
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
            VsComputerInsaneWindow insaneComputerWindow = new VsComputerInsaneWindow();
            insaneComputerWindow.Left = this.Left;
            insaneComputerWindow.Top = this.Top;
            this.Close();
            insaneComputerWindow.Show();
        }
    }
}
