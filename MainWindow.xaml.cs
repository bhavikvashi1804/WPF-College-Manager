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
            ShowAllCollege();
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }


        private void ShowAllCollege()
        {
            try
            {
                string query = "select * from College";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable collegeTable = new DataTable();
                    sqlDataAdapter.Fill(collegeTable);

                    ListAllCollege.DisplayMemberPath = "CollegeName";
                    ListAllCollege.SelectedValuePath = "Id";
                    ListAllCollege.ItemsSource = collegeTable.DefaultView;
                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }
        }

        private void DeleteDistrict(object sender, RoutedEventArgs e)
        {

            try
            {
                string query = "delete from District where Id = @districtId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@districtId", ListDistrict.SelectedValue);
                sqlCommand.ExecuteScalar();
                
            }
            catch(Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                sqlConnection.Close();
                ShowDistricts();
            }
            //MessageBox.Show("Delete District was clicked");
           
        }


        private void AddDistrict_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "insert into District values (@Location)";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Location", UserInput.Text);
                sqlCommand.ExecuteScalar();
                UserInput.Text = "";

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                sqlConnection.Close();
                ShowDistricts();
            }
        }


        protected void AddCollegeToDistrict(object sender,EventArgs e)
        {
            //MessageBox.Show("Add College To District");
            try
            {
                string query = "insert into DistrictCollege values (@DistrictId,@CollegeId)";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@DistrictId", ListDistrict.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@CollegeId",ListAllCollege.SelectedValue);
                sqlCommand.ExecuteScalar();
                UserInput.Text = "";

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                sqlConnection.Close();
                //ShowDistricts();
                ShowColleges();
            }
        }
    }
}
