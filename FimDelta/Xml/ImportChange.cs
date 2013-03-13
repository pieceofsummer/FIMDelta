using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace FimDelta.Xml
{

    [Serializable]
    public class ImportChange : INotifyPropertyChanged
    {
        [XmlElement]
        public string Operation { get; set; }

        [XmlElement]
        public string AttributeName { get; set; }

        [XmlElement]
        public string AttributeValue { get; set; }

        [XmlElement]
        public bool FullyResolved { get; set; }

        [XmlElement]
        public string Locale { get; set; }

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

        protected void OnPropertyChanged(string property)
        {
            var e = PropertyChanged;
            if (e != null)
                e(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

}
