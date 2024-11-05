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

namespace khabibullin
{
    public partial class SignUpPage : Page
    {
        private Service _currentService = new Service();
        public SignUpPage(Service SelectedService)
        {
            InitializeComponent();
            if (SelectedService != null)
                this._currentService = SelectedService;

            DataContext = _currentService;

            var _currentClient = Khabibullin_autoserviceEntities.GetContext().Client.ToList();

            ComboClient.ItemsSource = _currentClient;
        }

        private ClientService _currentClientService = new ClientService();
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            string s = TBStart.Text;

            if (ComboClient.SelectedItem == null)
            {
                errors.AppendLine("Укажите ФИО клиента");
            }
            if (StartDate.Text == "")
            {
                errors.AppendLine("Укажите дату услуги");
            }
            if (TBStart.Text == "")
            {
                errors.AppendLine("Укажите время начала услуги");
            }
            if (s.Length < 4 || s.Length > 5 || !s.Contains(':'))
            {
                errors.AppendLine($"Неверный формат начала услуги");
            }
            else
            {
                string[] start = s.Split(new char[] { ':' });
                int startHour = Convert.ToInt32(start[0].ToString());
                int startMin = Convert.ToInt32(start[1].ToString());

                if (startHour >= 24 && startMin >= 1)
                {
                    errors.AppendLine($"Время начало услуги указано неверно");
                }
            }
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            _currentClientService.ClientID = ComboClient.SelectedIndex + 1;
            _currentClientService.ServiceID = _currentService.ID;
            _currentClientService.StartTime = Convert.ToDateTime(StartDate.Text + ' ' + TBStart.Text);


            if (_currentClientService.ID == 0)
            {
                Khabibullin_autoserviceEntities.GetContext().ClientService.Add(_currentClientService);
            }

            try
            {
                Khabibullin_autoserviceEntities.GetContext().SaveChanges();
                MessageBox.Show("информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void TBStart_TextChanged(object sender, TextChangedEventArgs e)
        {

            string s = TBStart.Text;

            if (s.Length < 4 || s.Length > 5 || !s.Contains(':'))
            {
                TBEnd.Text = "";
            }
            else
            {
                string[] start = s.Split(new char[] { ':' });
                int startHour = Convert.ToInt32(start[0].ToString()) * 60;
                int startMin = Convert.ToInt32(start[1].ToString());

                int sum = startHour + startMin + _currentService.Duration;

                int EndHour = sum / 60;
                int EndMin = sum % 60;
                if (EndHour > 24)
                    EndHour -= 24;
                if (EndMin % 60 < 10)
                {
                    s = $"{EndHour.ToString()}:0{EndMin.ToString()}";
                }
                else
                {
                    s = $"{EndHour.ToString()}:{EndMin.ToString()}";
                }
                TBEnd.Text = s;
            }
        }
    }
}
