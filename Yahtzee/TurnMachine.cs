// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TurnsMachine.cs" company="NWTC">
//   Copyright
// </copyright>
// <summary>
//   The turns machine.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Yahtzee
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The turns machine.
    /// </summary>
    public class TurnMachine
    {
        /// <summary>
        /// The player one.
        /// </summary>
        private string playerOne;

        /// <summary>
        /// The player two.
        /// </summary>
        private string playerTwo;

        /// <summary>
        /// The turn list.
        /// </summary>
        private LinkedList<string> turnList = new LinkedList<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TurnMachine"/> class. 
        /// </summary>
        /// <param name="player1">
        /// The player 1.
        /// </param>
        /// <param name="player2">
        /// The player 2.
        /// </param>
        public TurnMachine(string player1, string player2)
        {
            this.playerOne = player1;
            this.playerTwo = player2;
            this.AddToListTwoPlayers();
        }

        /// <summary>
        /// Clear Linked list.
        /// </summary>
        public void ClearList()
        {
            this.turnList.Clear();
        }

        /// <summary>
        /// The restart list.
        /// </summary>
        public void RestartListTwoPlayers()
        {
            this.ClearList();
            this.AddToListTwoPlayers();
        }

        /// <summary>
        /// Players can ask if it is their turn to play.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string AskForTurn()
        {
            try
            {
                return this.turnList.First.Value;
            }
            catch
            {
                Console.WriteLine("No players in the Turn List");
                return string.Empty;
            }
        }

        /// <summary>
        /// The get turn.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string GetTurn()
        {
            string currentPlayer = this.turnList.First.Value;
            this.turnList.RemoveFirst();
            this.turnList.AddLast(currentPlayer);
            return currentPlayer;
        }

        /// <summary>
        /// Removes player from the LinkedList
        /// </summary>
        /// <param name="playerName">
        /// The player name.
        /// </param>
        public void RemoveNode(string playerName)
        {
            this.turnList.Remove(playerName);
        }

        /// <summary>
        /// Returns the turns left.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int TurnsLeft()
        {
            return this.turnList.Count;
        }

        /// <summary>
        /// The add to list for two players
        /// </summary>
        private void AddToListTwoPlayers()
        {
            this.turnList.AddFirst(this.playerOne);
            this.turnList.AddLast(this.playerTwo);
        }
    }
}
