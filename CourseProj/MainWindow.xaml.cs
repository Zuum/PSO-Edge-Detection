using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CourseProj
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int ID_RANGE = 1000000000;
         CourseProj.PSODBDataSet pSODBDataSet;
            
            CourseProj.PSODBDataSetTableAdapters.ImgSetTableAdapter pSODBDataSetImgSetTableAdapter;
            
           
            CourseProj.PSODBDataSetTableAdapters.ResultTableAdapter pSODBDataSetResultTableAdapter;
            
           
           
            CourseProj.PSODBDataSetTableAdapters.SessionTableAdapter pSODBDataSetSessionTableAdapter;



        private string imageSetName;
        public MainWindow()
        {
            InitializeComponent();
            pSODBDataSet = ((CourseProj.PSODBDataSet)(this.FindResource("pSODBDataSet")));
            pSODBDataSetImgSetTableAdapter = new CourseProj.PSODBDataSetTableAdapters.ImgSetTableAdapter();
            pSODBDataSetImgSetTableAdapter.Fill(pSODBDataSet.ImgSet);
            pSODBDataSetResultTableAdapter = new CourseProj.PSODBDataSetTableAdapters.ResultTableAdapter();
            pSODBDataSetResultTableAdapter.Fill(pSODBDataSet.Result);
            pSODBDataSetSessionTableAdapter = new CourseProj.PSODBDataSetTableAdapters.SessionTableAdapter();
            pSODBDataSetSessionTableAdapter.Fill(pSODBDataSet.Session);
        }

        private void OpenImageSet(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                imageSetName = openFileDialog.FileName;
            }
                

        }

        private void StartProcessing(object sender, RoutedEventArgs e)
        {
            string[] elements;
            string[] imgPathes = File.ReadAllLines(imageSetName);
            elements = imgPathes.ElementAt(0).Split(' ');
                Random r = new Random();
                PSODBDataSet.ImgSetRow newImgSetRow = pSODBDataSet.ImgSet.NewImgSetRow();
                newImgSetRow.Id = r.Next(ID_RANGE);
                newImgSetRow.Path_to_list = imageSetName;
                newImgSetRow.Number_of_images = Convert.ToInt32(elements[0]);
                newImgSetRow.Noise_rating = Convert.ToDouble(elements[1]);
                newImgSetRow.Resolution = elements[2];
                pSODBDataSet.ImgSet.AddImgSetRow(newImgSetRow);

                this.pSODBDataSetImgSetTableAdapter.Update(pSODBDataSet.ImgSet);
                for (int i = 1; i < imgPathes.Length; i++)
                {
                    ImageProcessor imageProcessor = new ImageProcessor(imgPathes.ElementAt(i));
                    
                    PSODBDataSet.SessionRow newSessionRow = pSODBDataSet.Session.NewSessionRow();
                    
                    newSessionRow.Id = r.Next(ID_RANGE);
                    newSessionRow.w = imageProcessor.w;
                    newSessionRow.c1 = imageProcessor.c1;
                    newSessionRow.c2 = imageProcessor.c2;
                    newSessionRow.treshold = imageProcessor.treshold;
                    newSessionRow.ImgSetId = newImgSetRow.Id;
                    pSODBDataSet.Session.AddSessionRow(newSessionRow);
                    this.pSODBDataSetSessionTableAdapter.Update(pSODBDataSet.Session);
                    imageProcessor.EdgeDetectPSO();
                    PSODBDataSet.ResultRow newResultRow = pSODBDataSet.Result.NewResultRow();
                    newResultRow.Id = r.Next(ID_RANGE);
                    newResultRow.Time = imageProcessor.time;
                    newResultRow.Accuracy = imageProcessor.accuracy;
                    newResultRow.Image_path = imgPathes.ElementAt(i);
                    newResultRow.SessionId = newSessionRow.Id;
                    pSODBDataSet.Result.AddResultRow(newResultRow);
                    this.pSODBDataSetResultTableAdapter.Update(pSODBDataSet.Result);
                }
                


        }

        private void ShowImage(object sender, RoutedEventArgs e)
        {
            string shownImageName; 
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                shownImageName = openFileDialog.FileName;


                BitmapImage shownImage = new BitmapImage(new Uri(shownImageName));
                imageImage.Source = shownImage;
            }
        }

        private void About(object sender, RoutedEventArgs e)
        {
            AboutWindow newAboutWindow = new AboutWindow();
            newAboutWindow.ShowDialog();
        }

        private void Help(object sender, RoutedEventArgs e)
        {
            HelpWindow newHelpWindow = new HelpWindow();
            newHelpWindow.ShowDialog();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

           
          
        }


    }
}
