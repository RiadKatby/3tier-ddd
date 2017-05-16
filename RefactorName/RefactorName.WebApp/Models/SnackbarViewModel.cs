using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RefactorName.WebApp.Models
{
    /// <summary>
    /// Specify the type of Snackbar (Alert Message).
    /// </summary>
    public enum SnackbarType
    {
        info,
        danger,
        success,
        warning
    }

    /// <summary>
    /// Represent data model of Snackbar (Alert Message) that sent to Users
    /// </summary>
    public class SnackbarViewModel
    {
        /// <summary>
        /// Gets or sets Snackbar (Alert Message) type.
        /// </summary>
        public SnackbarType Type { get; set; }

        /// <summary>
        /// Gets or sets message text.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets timeout to hide the message after show it, zero mean forever.
        /// </summary>
        public int Timeout { get; set; }

        public SnackbarViewModel(string message, SnackbarType type, int timeout)
        {
            Message = message;
            Type = type;
            Timeout = timeout;
        }
    }
}