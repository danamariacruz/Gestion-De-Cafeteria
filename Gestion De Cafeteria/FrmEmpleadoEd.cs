﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gestion_De_Cafeteria
{
    public partial class FrmEmpleadoEd : Form
    {
        private Empleado empleado;
        private readonly GestionCafeteriaEntities entities = new GestionCafeteriaEntities();
        readonly ValidacionCedula vc = new ValidacionCedula();
        public FrmEmpleadoEd()
        {
            InitializeComponent();
        }

        public FrmEmpleadoEd(Empleado empleado)
        {
            InitializeComponent();
            this.empleado = empleado;
        }

        

        private void FrmEmpleadoEd_Load(object sender, EventArgs e)
        {
            salaryNumericTB.Maximum = int.MaxValue;
            if(empleado != null)
            {
                txtID.Text = empleado.IdEMpleado.ToString();
                txtNombre.Text = empleado.Nombre.Split()[0];
                txtApellido.Text = empleado.Nombre.Split()[1] ?? "";
                txtDireccion.Text = empleado.Direccion;
                salaryNumericTB.Value = empleado.Salario;
                fechaIngresoTB.Text = empleado.Fecha_Ingreso.ToString();
                estadoComboBox.SelectedItem = empleado.Estado == true ? "Activo" : "Inactivo";
                cedulaTB.Text = empleado.Cedula;
                tandaComboBox.SelectedItem = empleado.Tanda_labor;
                comisionTB.Value = decimal.Parse(empleado.Porciento_Comision == null ? "0" : empleado.Porciento_Comision.Value.ToString());
            }

        }

        private void CmdGuardar_Click(object sender, EventArgs e)
        {
            if (!FieldsAreValid())
                return;
            if (!vc.validaCedula(cedulaTB.Text))
            {
                MessageBox.Show("Cedula incorrecta");
                return;
            }
            else
            {
                empleado = new Empleado()
                {
                    IdEMpleado = string.IsNullOrEmpty(txtID.Text) ? 0 : int.Parse(txtID.Text),
                    Nombre = $"{txtNombre.Text.ToUpperInvariant()} {txtApellido.Text.ToUpperInvariant()}",
                    Direccion = txtDireccion.Text,
                    Salario = salaryNumericTB.Value,
                    Cedula = cedulaTB.Text,
                    Tanda_labor = tandaComboBox.SelectedItem.ToString(),
                    Fecha_Ingreso = string.IsNullOrEmpty(txtID.Text) ? DateTime.Now : DateTime.Parse(fechaIngresoTB.Text),
                    Estado = estadoComboBox.SelectedItem.ToString() == "Activo",
                    Porciento_Comision = int.Parse(comisionTB.Value.ToString())
                };
                if (empleado != null)
                    entities.Empleadoes.AddOrUpdate(empleado);

                entities.SaveChanges();
                this.Dispose();
            }
            
        }

        private bool FieldsAreValid()
        {
            if (string.IsNullOrEmpty(txtNombre.Text) || string.IsNullOrEmpty(txtApellido.Text))
            {
                MessageBox.Show($"Los campos Nombre y Apellido son requeridos", "Campos requeridos");
                return false;
            }
            if (salaryNumericTB.Value == 0)
            {
                MessageBox.Show($"Salario debe ser mayor que 0", "Requerido");
                return false;
            }
            if (string.IsNullOrEmpty(cedulaTB.Text))
            {
                MessageBox.Show($"Debe agregar una cedula", "Requerido");
                return false;
            }
            if (string.IsNullOrEmpty(tandaComboBox.SelectedItem?.ToString()))
            {
                MessageBox.Show($"Debe seleccionar una tanda para el empleado", "Requerido");
                return false;
            }
            if (string.IsNullOrEmpty(estadoComboBox.SelectedItem?.ToString()))
            {
                MessageBox.Show($"Debe seleccionar una tanda para el empleado", "Requerido");
                return false;
            }
            return true;
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void CmdEliminar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Seguro de eliminar?", "Alerta", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            var success = int.TryParse(txtID.Text, out int empleadoId);
            if (!success)
            {
                MessageBox.Show("Id Invalido", "Info");
                return;
            }
                
            Empleado empleado = entities.Empleadoes.Find(empleadoId);
            try
            {
                if (empleado != null)
                {
                    entities.Empleadoes.Remove(empleado);
                    entities.SaveChanges();
                    MessageBox.Show("Empleado eliminado con exito");
                }
                else
                {
                    MessageBox.Show("Marca no existe");
                }
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException)
            {

                MessageBox.Show("Empleado no puede ser eliminado por estar asignado a una cafeteria", "Error");
                return;
            }
            
            this.Close();
        }
    }
}
