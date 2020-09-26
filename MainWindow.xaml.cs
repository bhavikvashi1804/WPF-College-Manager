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


        private void ShowColleges()
        {
            try
            {
                string query = "SELECT * from College c inner join DistrictCollege dc on c.Id = dc.CollegeId where dc.DistrictId = @districtID";

                SqlCommand sqlCommand = new SqlCommand(query,sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {

                    //get the selected district and fit to query
                    sqlCommand.Parameters.AddWithValue("@districtID",ListDistrict.SelectedValue);

                    DataTable collegeTable = new DataTable();
                    sqlDataAdapter.Fill(collegeTable);

                    ListCollege.DisplayMemberPath = "CollegeName"; //sane as Column name
                    ListCollege.SelectedValuePath = "Id";
                    ListCollege.ItemsSource = collegeTable.DefaultView;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

        private void ListDistrict_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowColleges();

        }
    }
}
