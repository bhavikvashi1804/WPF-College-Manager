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
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace WPF_College_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection sqlConnection ;
        public MainWindow()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["WPF_College_Manager.Properties.Settings.DBDemoConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            ShowDistricts();
        }

        private void ShowDistricts()
        {
            try
            {
                string query = "SELECT * from District";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable districtTable = new DataTable();
                    sqlDataAdapter.Fill(districtTable);

                    ListDistrict.DisplayMemberPath = "Location"; //sane as Column name
                    ListDistrict.SelectedValuePath = "Id";
                    ListDistrict.ItemsSource = districtTable.DefaultView;
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
           
        }
    }
}
