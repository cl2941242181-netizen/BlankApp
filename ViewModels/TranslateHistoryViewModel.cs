using BlankApp.Events;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlankApp.ViewModels
{
    internal class TranslateHistoryViewModel:BindableBase
    {
        private readonly IEventAggregator _eventAggregator;

        private ObservableCollection<TranslationCompletedEventArgs> historyList;
        public ObservableCollection<TranslationCompletedEventArgs> HistoryList
        {
            get
            {
                return historyList;
            }
            set
            {
                historyList = value;
                SetProperty(ref historyList, value);
            }
        }

        public TranslateHistoryViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<TranslationCompletedEvent>().Subscribe(OnTranslationCompleted);

            HistoryList = new ObservableCollection<TranslationCompletedEventArgs>();
        }

        private void OnTranslationCompleted(TranslationCompletedEventArgs args)
        {
            HistoryList.Insert(0, args);
        }

        
    }
}
