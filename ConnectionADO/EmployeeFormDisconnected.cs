using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Forms;

namespace ConnectionADO
{
    public partial class EmployeeFormDisconnected : Form
    {
        SqlConnection con;
        SqlDataAdapter da;
        DataSet ds;
        SqlCommandBuilder scb;

        public EmployeeFormDisconnected()
        {
            InitializeComponent();
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["defaultConn"].ConnectionString);

        }
        // to fetch the data at app side & load in DataSet

        private DataSet GetEmployees()
        {
            da = new SqlDataAdapter("select * from employee", con);
            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            scb = new SqlCommandBuilder(da);// track DataSet & generate qry & assign to da object
            ds = new DataSet();
            da.Fill(ds, "emp");// emp is a table name given to DataSet table
            return ds;
        }
        private void ClearFormFields()
        {
            txtId.Clear();
            txtName.Clear();
            txtEmail.Clear();
            txtSalary.Clear();
        }

    private void EmployeeFormDisconnected_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ds = GetEmployees();
                // need to add new object in the DataSet
                DataRow row = ds.Tables["emp"].NewRow();
                row["name"] = txtName.Text;
                row["email"] = txtEmail.Text;
                row["salary"] = txtSalary.Text;
                // attach /load the row to the DataSet
                ds.Tables["emp"].Rows.Add(row);
                // reflect the changes to the DB
                int result = da.Update(ds.Tables["emp"]);
                if (result >= 1)
                {
                    MessageBox.Show("Record inserted");
                    ClearFormFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                ds = GetEmployees();
                DataRow row = ds.Tables["emp"].Rows.Find(txtId.Text);
                if (row != null)
                {
                    txtName.Text = row["name"].ToString();
                    txtEmail.Text = row["email"].ToString();
                    txtSalary.Text = row["salary"].ToString();
                }
                else
                {
                    MessageBox.Show("Record not found");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                ds = GetEmployees();
                // find the object in the DataSet
                DataRow row = ds.Tables["emp"].Rows.Find(txtId.Text);
                if (row != null)
                {
                    row["name"] = txtName.Text;
                    row["email"] = txtEmail.Text;
                    row["salary"] = txtSalary.Text;
                    // reflect the changes to the DB
                    int result = da.Update(ds.Tables["emp"]);
                    if (result >= 1)
                    {
                        MessageBox.Show("Record updated");
                        ClearFormFields();
                    }
                }
                else
                {
                    MessageBox.Show("Record not found for Id");
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ds = GetEmployees();
            // find the object in the DataSet
            DataRow row = ds.Tables["emp"].Rows.Find(txtId.Text);
            if (row != null)
            {
                row.Delete();
                int result = da.Update(ds.Tables["emp"]);
                if (result >= 1)
                {
                    MessageBox.Show("Record deleted");
                    ClearFormFields();
                }
            }
            else
            {
                MessageBox.Show("Record not found for Id");
            }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnShowAll_Click(object sender, EventArgs e)
        {
            ds = GetEmployees();
            dataGridView1.DataSource = ds.Tables["emp"];
        }

    }
}

