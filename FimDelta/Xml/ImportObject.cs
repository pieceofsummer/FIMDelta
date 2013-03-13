using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

namespace FimDelta.Xml
{

    [Serializable]
    public class ImportObject : INotifyPropertyChanged
    {
        [XmlElement]
        public string SourceObjectIdentifier { get; set; }

        [XmlElement]
        public string TargetObjectIdentifier { get; set; }

        [XmlElement]
        public string ObjectType { get; set; }

        [XmlElement]
        public DeltaState State { get; set; }

        [XmlArray("Changes"), XmlArrayItem("ImportChange")]
        public ImportChange[] Changes { get; set; }

        [XmlArray("AnchorPairs"), XmlArrayItem("JoinPair")]
        public JoinPair[] AnchorPairs { get; set; }

        private bool isIncluded = true;

        [XmlIgnore]
        public bool IsIncluded 
        {
            get { return isIncluded; }
            set 
            {
                isIncluded = value;
                OnPropertyChanged("IsIncluded");
            }
        }

        internal bool NeedsInclude()
        {
            return IsIncluded || (Changes != null && Changes.Any(x => x.IsIncluded));
        }

        protected void OnPropertyChanged(string property)
        {
            var e = PropertyChanged;
            if (e != null)
                e(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

}
