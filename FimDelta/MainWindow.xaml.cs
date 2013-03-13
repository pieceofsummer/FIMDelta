using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;
using FimDelta.Properties;
using FimDelta.Xml;

namespace FimDelta
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly CollectionViewSource view;
        private Delta delta;
        private DeltaViewController deltaVC;

        public MainWindow()
        {
            InitializeComponent();
                
            view = (CollectionViewSource)FindResource("ObjectsView");

            Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                delta = DeltaParser.ReadDelta(Settings.Default.SourceFile, Settings.Default.TargetFile, Settings.Default.DeltaFile);
                
                deltaVC = new DeltaViewController(delta);
                view.Source = deltaVC.View;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }

        private void sortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (view == null || deltaVC == null) return;

            var sortType = ((ComboBox)sender).SelectedIndex;

            if (sortType == 1)
                deltaVC.Grouping = GroupType.State;
            else if (sortType == 2)
                deltaVC.Grouping = GroupType.ObjectType;
            else
                deltaVC.Grouping = GroupType.None;

            view.Source = deltaVC.View;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (delta == null) return;

            DeltaParser.SaveDelta(delta, Settings.Default.SaveTo);
        }
    }
}
