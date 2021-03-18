using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using LottoPicker.Annotations;
using LottoPicker.Common;
using LottoPicker.Lists;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LottoPicker.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {

        public MainPageViewModel(INumberPicker numberPicker)
        {
            NumberPicker = numberPicker;
            CopyTicketToClipboardCommand =
                new Command(execute: CopyTicketToClipboard, canExecute: CopyTicketTextToClipboardCanExecute);

            // number picker commands
            Pick649TicketCommand = new Command(Pick649Ticket);
            PickLottoMaxTicketCommand = new Command(PickLottoMaxTicket);

            // This Command will replace all the other number picker commands in this class
            PickLottoNumbersCommand = new Command<object>(selectedGame => PickLottoNumbers(selectedGame));
        }

        #region Bound Commands

        public ICommand CopyTicketToClipboardCommand { get; }
        public ICommand Pick649TicketCommand { get; }
        public ICommand PickLottoMaxTicketCommand { get; }
        public ICommand PickLottoNumbersCommand { get; }

        #endregion

        public INumberPicker NumberPicker { get; set; }

        private async void CopyTicketToClipboard()
        {
            await Clipboard.SetTextAsync(TicketDisplay);
            if (!Clipboard.HasText)
            {
                await Clipboard.SetTextAsync("Couldn't populate the clipboard");
            }
        }

        private bool CopyTicketTextToClipboardCanExecute()
        {
            return TicketDisplay.Length > 0;
        }

        /// <summary>
        /// This method will replace all the other delegate methods defined for this class.
        /// </summary>
        private void PickLottoNumbers(object picker)
        {
            switch (picker)
            {
                case "Lotto Max":
                    PickLottoMaxTicket();
                    break;

                case "Lotto 6/49":
                default:
                    Pick649Ticket();
                    break;
            }
        }

        private void Pick649Ticket()
        {
            var ticketValue = NumberPicker.Pick649();
            TicketDisplay = ticketValue.ToString();
        }

        private void PickLottoMaxTicket()
        {
            var ticketValue = NumberPicker.PickLottoMax();
            TicketDisplay = ticketValue.ToString();
        }

        private string _ticketDisplay;

        public string TicketDisplay
        {
            get => _ticketDisplay ?? "";
            set
            {
                if ((value ?? "") == _ticketDisplay) return;
                _ticketDisplay = value ?? "";
                OnPropertyChanged(nameof(TicketDisplay));
            }
        }

        #region Property Changed Event Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            ((Command)CopyTicketToClipboardCommand).ChangeCanExecute();
        }
        
        #endregion
    }
}
