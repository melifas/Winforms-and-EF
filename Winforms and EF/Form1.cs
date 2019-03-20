using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Winforms_and_EF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Customer c = new Customer();

        public void Clear()
        {
            txtFName.Text = txtLName.Text = txtCity.Text = txtAddress.Text = "";
            btnSave.Text = "Save";
            btnDelete.Enabled = false;
            c.CustomerID = 0;
        }

        //private void btnSave_Click(object sender, EventArgs e)
        //{
        //    c.FirstName = txtFName.Text;
        //    c.LastName = txtLName.Text;
        //    c.Address = txtAddress.Text;
        //    c.City = txtCity.Text;
        //    using (var contex = new EFWINFORMDBEntities())
        //    {
        //        if (c.CustomerID == 0)
        //        {
        //            contex.Customer.Add(c);
        //        }
        //        else
        //        {
        //            contex.Entry(c).State = EntityState.Modified;
        //        }
        //        contex.SaveChanges();
        //    }

        //    Clear();
        //    PopulateGrid();
        //    MessageBox.Show("Record Saved Succefully");
        //}

        private void btnSave_Click(object sender, EventArgs e)
        {
            c.FirstName = txtFName.Text;
            c.LastName = txtLName.Text;
            c.Address = txtAddress.Text;
            c.City = txtCity.Text;
            using (var contex = new EFWINFORMDBEntities())
            {
                try
                {
                    contex.Customer.AddOrUpdate(c);
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex);
                }

                contex.SaveChanges();
            }

            Clear();
            PopulateGrid();
            MessageBox.Show("Record Saved Sucefully");
        }

        public void PopulateGrid()
        {
            dataGridView1.AutoGenerateColumns = false;
            using (var contex = new EFWINFORMDBEntities())
            {
                dataGridView1.DataSource = contex.Customer.ToList<Customer>();

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PopulateGrid();
            Clear();
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Index != -1)
            {
                c.CustomerID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["CustomerID"].Value);
                using (var contex = new EFWINFORMDBEntities())
                {
                     var query = contex.Customer.Where(x => x.CustomerID == c.CustomerID).FirstOrDefault();
                    txtFName.Text = query.FirstName;
                    txtLName.Text = query.LastName;
                    txtCity.Text = query.City;
                    txtAddress.Text = query.Address;
                }

                btnSave.Text = "Update";
                btnDelete.Enabled = true;

            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you Sure", "Delete Operation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (var contex = new EFWINFORMDBEntities())
                {
                    c.CustomerID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["CustomerID"].Value);
                    var query = contex.Customer.Where(x => x.CustomerID == c.CustomerID).FirstOrDefault();
                    if (query != null)
                    {
                        contex.Customer.Remove(query);
                    }

                    contex.SaveChanges();
                    PopulateGrid();
                    Clear();
                    MessageBox.Show("Record Saved Sucefully");
                }
            }


        }
    }
}
