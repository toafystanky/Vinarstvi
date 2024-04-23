using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Vinarstvi.buttons;
using Vinarstvi.forms;

namespace Vinarstvi.utilities
{
    /// <summary>
    /// Represents a utility class for managing the Add Entries button functionality.
    /// </summary>
    public class AddEntriesButton
    {
        private FormManager _formManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddEntriesButton"/> class.
        /// </summary>
        /// <param name="formManager">The form manager instance.</param>
        public AddEntriesButton(FormManager formManager)
        {
            _formManager = formManager;
        }

        /// <summary>
        /// Handles the click event of the Add Entries button.
        /// </summary>
        public void AddEntriesButton_Click()
        {
        }
    }
}