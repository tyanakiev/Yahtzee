// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Dice.cs" company="NWTC">
//   copyright
// </copyright>
// <summary>
//   The dice.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Yahtzee
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// The dice.
    /// </summary>
    public class Dice
    {   
        /// <summary>
        /// The value.
        /// </summary>
        private int value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Dice"/> class.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        public Dice(int value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public int Value
        {
            get
            {
                return this.value;
            }

            set
            {
                this.value = value;
            }
        }
    }
}
