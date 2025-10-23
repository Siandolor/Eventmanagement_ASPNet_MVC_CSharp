// ============================================================================
// File: ErrorViewModel.cs
// Description: ViewModel used for error handling and diagnostics in the Event 
//              Management System. Provides request tracking for debugging and 
//              displaying error details in the UI.
// ============================================================================

namespace Eventmanagement.ViewModels
{
    public class ErrorViewModel
    {
        // --------------------------------------------------------------------
        // Unique identifier of the current request for debugging purposes
        // --------------------------------------------------------------------
        public string? RequestId { get; set; }

        // --------------------------------------------------------------------
        // Indicates whether the RequestId should be displayed in the view
        // --------------------------------------------------------------------
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
