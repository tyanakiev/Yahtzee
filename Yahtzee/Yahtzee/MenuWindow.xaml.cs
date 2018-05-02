// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MenuWindow.xaml.cs" company="NWTC">
//   Copyright
// </copyright>
// <summary>
//   Interaction logic for MenuWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Yahtzee
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for MenuWindow
    /// </summary>
    public partial class MenuWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MenuWindow"/> class.
        /// </summary>
        public MenuWindow()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// The button two players click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BtnTwoPlayersClick(object sender, RoutedEventArgs e)
        {
            TwoPlayersWindow twoPlayersWindow = new TwoPlayersWindow();
            twoPlayersWindow.Left = this.Left;
            twoPlayersWindow.Top = this.Top;
            this.Close();
            twoPlayersWindow.Show();
        }

        /// <summary>
        /// The button single player click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BtnSinglePlayerClick(object sender, RoutedEventArgs e)
        {
            SinglePlayerWindow singlePlayerWindow = new SinglePlayerWindow();
            singlePlayerWindow.Left = this.Left;
            singlePlayerWindow.Top = this.Top;
            this.Close();
            singlePlayerWindow.Show();
        }

        /// <summary>
        /// The button exit click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BtnExitClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// The button against computer click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BtnVsComputerClick(object sender, RoutedEventArgs e)
        {
            VsComputerWindow easyComputer = new VsComputerWindow();
            easyComputer.Left = this.Left;
            easyComputer.Top = this.Top;
            this.Close();
            easyComputer.Show();
        }

        /// <summary>
        /// The button against computer insane click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BtnVsComputerInsaneClick(object sender, RoutedEventArgs e)
        {
            VsComputerInsaneWindow insaneComputerWindow = new VsComputerInsaneWindow();
            insaneComputerWindow.Left = this.Left;
            insaneComputerWindow.Top = this.Top;
            this.Close();
            insaneComputerWindow.Show();
        }
    }
}
