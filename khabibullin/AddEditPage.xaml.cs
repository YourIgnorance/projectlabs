﻿using System;
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
    public partial class AddEditPage : Page
    {
        private Service _currentService = new Service();

        public AddEditPage(Service SelectedService)
        {
            InitializeComponent();

            if (SelectedService != null)
                _currentService = SelectedService;

            DataContext = _currentService;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentService.Title))
                errors.AppendLine("Укажите название услуги");
            if(_currentService.Cost <= 0)
                errors.AppendLine("Укажите стоимость услуги");
            if (_currentService.Duration > 240 || _currentService.Duration <= 0)
                errors.AppendLine("Длительность не может быть больше 240 минут или меньше 0");
            if (_currentService.Discount < 0 || _currentService.Discount > 100)
                errors.AppendLine("Укажите скидку от 0 до 100");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            var allService = Khabibullin_autoserviceEntities.GetContext().Service.ToList();
            allService = allService.Where(p => p.Title == _currentService.Title).ToList();

            if (allService.Count == 0)
            {
                if (_currentService.ID == 0)
                {
                    Khabibullin_autoserviceEntities.GetContext().Service.Add(_currentService);
                }
                try
                {
                    Khabibullin_autoserviceEntities.GetContext().SaveChanges();
                    MessageBox.Show("Информация сохранена");
                    Manager.MainFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            else
            {
                MessageBox.Show("Уже существует такая услуга");
            }

            if (_currentService.ID == 0)
            {
                Khabibullin_autoserviceEntities.GetContext().Service.Add(_currentService);
            }
            try
            {
                Khabibullin_autoserviceEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена!");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
