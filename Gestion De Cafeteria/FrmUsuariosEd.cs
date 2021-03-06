﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;
using System.Security.Cryptography;

namespace Gestion_De_Cafeteria
{
    public partial class FrmUsuariosEd : Form
    {
        public Usuario Usuario { get; set; }
        private readonly GestionCafeteriaEntities entities = new GestionCafeteriaEntities();
        readonly ValidacionCedula vc = new ValidacionCedula();
        public FrmUsuariosEd()
        {
            InitializeComponent();
        }

        private void FrmUsuariosEd_Load(object sender, EventArgs e)
        {
            CbxTipoUsuario.Items.AddRange(entities.Tipo_Usuario.Select(x => x.Descripcion).ToArray());

            CbxTipoUsuario.DataSource = entities.Tipo_Usuario.ToList();
            CbxTipoUsuario.DropDownStyle = ComboBoxStyle.DropDownList;
            CbxTipoUsuario.DisplayMember = "Descripcion";
            CbxTipoUsuario.ValueMember = "ID";

            TxtLimiteCredito.Maximum = int.MaxValue;

            CbxEstado.SelectedIndex = 0;
            if (Usuario != null)
            {
                TxtIdUsuario.Text = Usuario.IdUsuario.ToString();
                TxtNombre.Text = Usuario.Nombre;
                TxtCedula.Text = Usuario.Cedula.ToString();
                CbxTipoUsuario.SelectedItem = Usuario.TipoUsuario;
                TxtLimiteCredito.Value = (decimal)Usuario.LimiteCredito;
                DtpFechaRegistro.Value = Usuario.FechaRegistro;
                if (!Usuario.Estado)
                {
                    CbxEstado.SelectedIndex = 1;
                }
                txtClave.Text = Usuario.Clave;
            }
        }

        private void CmdGuadar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!vc.validaCedula(TxtCedula.Text))
                {
                    MessageBox.Show("Cedula incorrecta");
                    return;
                }
                if (!string.IsNullOrEmpty(TxtIdUsuario.Text))
                {
                    Usuario usuario = entities.Usuarios.Find(Int32.Parse(TxtIdUsuario.Text));
                    usuario.Nombre = TxtNombre.Text;
                    usuario.Cedula = TxtCedula.Text;
                    usuario.TipoUsuario = (int)CbxTipoUsuario.SelectedValue;
                    usuario.LimiteCredito = decimal.Parse(TxtLimiteCredito.Text);
                    usuario.FechaRegistro = DtpFechaRegistro.Value;
                    usuario.Estado = CbxEstado.Text == "";
                    usuario.Clave = GetMD5Hash(txtClave.Text);
                    entities.Entry(usuario).State = EntityState.Modified;
                    entities.SaveChanges();
                    entities.Entry(usuario).Reload();
                }
                else
                {
                    if (!vc.validaCedula(TxtCedula.Text))
                    {
                        MessageBox.Show("Cedula incorrecta");
                    }
                    else
                    {
                        Nombre = TxtNombre.Text,
                        Cedula = TxtCedula.Text,
                        Clave = GetMD5Hash(txtClave.Text),
                        TipoUsuario = (int)CbxTipoUsuario.SelectedValue,
                        LimiteCredito = decimal.Parse(TxtLimiteCredito.Text),
                        FechaRegistro = DtpFechaRegistro.Value,
                        Estado = CbxEstado.SelectedItem == "Activo"
                    });
                    entities.SaveChanges();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Un campo de los ingresados es invalido " + ex.Message);
            }
        }

        private void CmdEliminar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Seguro de eliminar?", "Alerta", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            bool success = int.TryParse(TxtIdUsuario.Text, out int usuarioId);
            if (!success) 
            {
                MessageBox.Show("Coloque un Id correcto.","Error");
                return;
            }

            var usuario = entities.Usuarios.Find(usuarioId);
            if (usuario != null)
            {
                entities.Usuarios.Remove(usuario);
                entities.SaveChanges();
                MessageBox.Show("Usuario eliminado con exito","Info");
            }
            else
            {
                MessageBox.Show("Usuario no existe","Infoe");
            }
            this.Close();
        }

        public string GetMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

    }
}
