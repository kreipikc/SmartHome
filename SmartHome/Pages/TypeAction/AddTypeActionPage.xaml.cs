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

namespace SmartHome.Pages.TypeAction
{
    /// <summary>
    /// Логика взаимодействия для AddTypeActionPage.xaml
    /// </summary>
    public partial class AddTypeActionPage : Page
    {
        public AddTypeActionPage()
        {
            InitializeComponent();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            string Name = NameTextBox.Text;

            CreateTypeAction(Name);
        }

        public bool CreateTypeAction(string Name)
        {
            try
            {
                if (string.IsNullOrEmpty(Name))
                {
                    MessageBox.Show("Заполните все поля");
                    return false;
                }

                if (Core.DB.TypeAction.Any(u => u.type_name == Name))
                {
                    MessageBox.Show("Тип действия с таким названием уже существует");
                    return false;
                }

                var newType = new Database.TypeAction
                {
                    type_name = Name,
                };

                Core.DB.TypeAction.Add(newType);
                Core.DB.SaveChanges();

                MessageBox.Show("Тип действия успешно создан");
                NavigationService.GoBack();
                return true;
            }
            catch (Exception ex)
            {
                SmartHome.Utils.PrintError(ex);
                return false;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
