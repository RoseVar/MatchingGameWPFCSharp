using System;
using System.Collections.Generic;
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
using System.Windows.Threading;

namespace MatchingGameWPFex5
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Declarar e instanciar un objeto random
        Random random = new Random();

        // Crear una lista de strings, que seran 8 parejas de letras
        List<string> myImages = new List<string>()
        {
            "images/animal01.png", "images/animal01.png",
            "images/animal02.png", "images/animal02.png",
            "images/animal03.png", "images/animal03.png",
            "images/animal04.png", "images/animal04.png",
            "images/animal05.png", "images/animal05.png",
            "images/animal06.png", "images/animal06.png",
            "images/animal07.png", "images/animal07.png",
            "images/animal08.png", "images/animal08.png",
            "images/animal09.png", "images/animal09.png",
            "images/animal10.png", "images/animal10.png"
        };

        // Etiqueta para controlar si el jugador ha clicado 1º imagen
        Image firstClicked = null;

        // Etiqueta para controlar si el jugador ha clicado 2ª imagen
        Image secondClicked = null;

        //Timer de volteo de parejas
        DispatcherTimer myTimer = new DispatcherTimer();
        //Timer de juego
        DispatcherTimer myGameTimer = new DispatcherTimer();
        //Timer para lentos
        DispatcherTimer myCaracolTimer = new DispatcherTimer();

        

        public MainWindow()
        {
            InitializeComponent();
            AssignIconsToSquares();
            myTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            myTimer.Interval = new TimeSpan(0, 0, 2);

            myGameTimer.Tick += new EventHandler(dispatcherTimer_Tick2);
            myGameTimer.Interval = new TimeSpan(0, 1, 0);
            myGameTimer.Start();

            myCaracolTimer.Tick += new EventHandler(dispatcherTimer_Tick3);
            myCaracolTimer.Interval = new TimeSpan(0, 0, 1);
        }

        private void dispatcherTimer_Tick3(object sender, EventArgs e)
        {
            
            //Cuando ha pasado el tiempo (Tick) se para
            myCaracolTimer.Stop();
            if (secondClicked == null& firstClicked!=null)
            {
                // Se ponen los contenidos de las labels escogidas otra vez con el color de fondo
                firstClicked.Opacity = 0;
            }
        }
    

        private void dispatcherTimer_Tick2(object sender, EventArgs e)
        {
            //Cuando ha pasado el tiempo de juego (Tick) se para
            myGameTimer.Stop();

            //se da notificación al usuario
            MessageBox.Show("Time is up!", "Game over");
            //Se cierra el programa
            Close();
        }

        /// <summary>
        /// Metodo para asignar una imagen a un cuadrado
        /// </summary>
        private void AssignIconsToSquares()
        {
            // En cada uno de los 16 cuadrados del layout
            //Añadir una label 
            foreach (Image control in MyGrid.Children)
            {
                Image iconImage = control;

                if (iconImage != null)
                {
                    //se genera un número aleatorio
                    int randomNumber = random.Next(myImages.Count);
                    //el caracter será una aletatorio del listado que tenemos
                    BitmapImage bi3 = new BitmapImage();
                    bi3.BeginInit();
                    bi3.UriSource = new Uri(myImages[randomNumber], UriKind.RelativeOrAbsolute);
                    bi3.EndInit();
                    iconImage.Source = bi3;
                    //Se pone el mismo color que el fondo
                    iconImage.Opacity = 0;
                    //se quita de la lista el caracter de la lista 
                    myImages.RemoveAt(randomNumber);
                }
            } 
        }


        /// <summary>
        /// Metodo para tener un cronometro que deje a la vista la pareja seleccionada que no 
        /// son la misma imagen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            //Cuando ha pasado el tiempo (Tick) se para
            myTimer.Stop();

            // Se ponen los contenidos de las labels escogidas otra vez con el color de fondo
            firstClicked.Opacity= 0; 
            secondClicked.Opacity=0;

            //Se desasignan las labels a esas flags de 1ª y 2ª label de pareja
            firstClicked = null;
            secondClicked = null;
        }

        /// <summary>
        /// Método para revisar si se ha ganado
        /// </summary>
        private void CheckForWinner()
        {
            // Revisando uno a uno todos los controles del grid (en este caso labels)
            foreach (Image control in MyGrid.Children)
            {
                Image iconLabel = control;
                //Siempre que no sean null...
                if (iconLabel != null)
                {
                    //Si se encuentra alguna cuyo color de contenido es igual al color de fondo
                    if (iconLabel.Opacity == 0)
                        //se sale del método
                        return;
                }
            }

            // Si no se ha salido es porque todas tenían color de contenido diferente del de fondo, 
            //es decir, todas se emparejaron
            //se da notificación al usuario
            MessageBox.Show("You matched all the icons!", "Congratulations");
            //Se cierra el programa
            Close();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Si el dispatcherTimer está corriendo, no hace nada y sale del método
            if (myTimer.IsEnabled == true)
                return;
            //Siendo el sender una label y siempre que no sea null
            Image selectedLabel = sender as Image;
            if (selectedLabel != null)
            {
                // Si el contenido tiene el mismo color que el fondo
                if (selectedLabel.Opacity == 0)
                {
                    // Si no se ha clicado ninguna 1ª label de pareja
                    if (firstClicked == null)
                    {
                        //se asigna esta Label como esa 1ª Label y se pinta el contenido de negro y sale del método
                        firstClicked = selectedLabel;
                        firstClicked.Opacity = 100;
                        myCaracolTimer.Start();
                        return;
                    }
                    //En caso que ya haya una 1ª label de pareja
                    else if (firstClicked!= null & firstClicked!=selectedLabel)
                    {
                        //se asigna esta label como segunda y se pinta el contenido de negro
                        secondClicked = selectedLabel;
                        secondClicked.Opacity = 100;
                    }

                    if (firstClicked != null & secondClicked != null)
                    {
                        // Si el contenido de la pareja de labels es el mismo
                        if (firstClicked.Source.ToString().Equals(secondClicked.Source.ToString()))
                        {
                            //nos aseguramos que los 2 están visibles
                            firstClicked.Opacity=100;
                            secondClicked.Opacity=100;
                            //Se desasignan las labels a esas flags de 1ª y 2ª label de pareja
                            firstClicked = null;
                            secondClicked = null;

                            // Se revisa si ha ganado
                            CheckForWinner();
                            //Si no, se sale del método
                            return;
                        }
                        else
                        {
                            // Si no son los mismos empieza a contar el DispatcherTimer
                            myTimer.Start();
                        }
                    }
                }
            }
        }
    }
}