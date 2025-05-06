using SmartHome.Pages.Events;
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
    /// Логика взаимодействия для EditTypeActionPage.xaml
    /// </summary>
    public partial class EditTypeActionPage : Page
    {
        public EditTypeActionPage()
        {
            InitializeComponent();
            AddData();
            TypeActionPage.TypeCurrent = null;
        }

        private void AddData()
        {
            try
            {
                IdTextBox.Text = Convert.ToString(TypeActionPage.TypeCurrent.type_action_id);
                NameTextBox.Text = TypeActionPage.TypeCurrent.type_name;
            }
            catch (Exception ex)
            {
                SmartHome.Utils.PrintError(ex);
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            string IdStr = IdTextBox.Text;
            string Name = NameTextBox.Text;

            UpdateTypeAction(IdStr, Name);
        }

        public bool UpdateTypeAction(string IdStr, string Name)
        {
            try
            {
                if (string.IsNullOrEmpty(IdStr) ||
                    string.IsNullOrEmpty(Name))
                {
                    MessageBox.Show("Заполните все поля");
                    return false;
                }

                int Id = Convert.ToInt32(IdStr);

                if (Core.DB.TypeAction.Any(u => u.type_name == Name && u.type_action_id != Id))
                {
                    MessageBox.Show("Тип действия с таким именем уже существует");
                    return false;
                }

                var events = Core.DB.TypeAction.FirstOrDefault(h => h.type_action_id == Id);
                if (events == null)
                {
                    MessageBox.Show("Тип действия не найден");
                    return false;
                }

                events.type_name = Name;

                Core.DB.SaveChanges();
                MessageBox.Show("Тип действия успешно обновлен");

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
