using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
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

namespace Lab5
{
    public partial class MainWindow : Window
    {
        string logPath = "logs\\log.json";          // шлях до файлу логів

        University university = new University();   // створення екземпляру класу університету
        int subjectCount = 7;                       // початкова кількість предметів
        int teachersCount = 10;                     // початкова кількість вчителів

        List<Thread> threads = new List<Thread>();  // список потоків

        public MainWindow()
        {
            InitializeComponent();
            university.AddSubjects(subjectCount);
            university.AddTeachers(teachersCount);
        }

        // метод Refresh - оновлює всі 3 текстові поля
        public void Refresh()
        {
            tb_Students.Clear();
            tb_Teachers.Clear();
            tb_Subjects.Clear();
            try
            {
                foreach (var student in university.Students)
                {
                    tb_Students.Text += student.Name + '\n';
                }
                foreach (var teacher in university.Teachers)
                {
                    tb_Teachers.Text += teacher.Name + '\n';
                }
                foreach (var subject in university.Subjects)
                {
                    tb_Subjects.Text += subject.Name + '\n';
                }
            }
            catch 
            {
                Refresh();
            }
        }

        // обробник кнопки Start
        private async void bn_Start_Click(object sender, RoutedEventArgs e)
        {
            int durationTime;   // затримка для потоків (секунди)
            try
            {
                // спроба прочитати з текстового поля число
                durationTime = Convert.ToInt32(tb_DurationTime.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Потрібно ввести ціле число в поле затримки!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            // поток початку вступної компанії
            Thread threadStartAC = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(durationTime));
                    university.StartAdmissionCamping();
                }
                
            });
            threads.Add(threadStartAC);

            // поток початку канікул
            Thread threadStartVC = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(durationTime));
                    university.StartVacationPeriod();
                }
            });
            threads.Add(threadStartVC);

            // поток початку навчання
            Thread threadStartC = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(durationTime));
                    university.StartClasses();             
                }
            });
            threads.Add(threadStartC);

            threadStartAC.Start();
            threadStartVC.Start();
            threadStartC.Start();
            
            // оновлення всіх трьох текстових полів, а також запис у логи кожні n секунд
            while (true)
            {
                await Task.Delay(durationTime * 1000);
                university.SaveStateLog(logPath);
                Refresh();
            }
        }

        // обробник кнопки Stop
        private void bn_Stop_Click(object sender, RoutedEventArgs e)
        {
            // зупинити усі потоки
            foreach (var thread in threads)
            {
                thread.Abort();
            }
        }

        // обробник кнопки Refresh
        private void bn_Refresh_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }
    }
}