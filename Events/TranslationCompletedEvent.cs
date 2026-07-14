using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlankApp.Events
{
    public class TranslationCompletedEvent : PubSubEvent<TranslationCompletedEventArgs>
    {

    }

    public class TranslationCompletedEventArgs
    {
        public string OriginalText { get; set; }
        public string TranslatedText { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
