﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gestion_De_Cafeteria
{
    public partial class FrmUsuarios : Form
    {
        public Usuario Usuario { get; set; }
        private readonly GestionCafeteriaEntities entities = new GestionCafeteriaEntities();
        public FrmUsuarios()
        {
            InitializeComponent();
        }

        private void FrmUsuarios_Load(object sender, EventArgs e)
        {
            ConsultarPorCriterio();
        }

        private void CmdBuscar_Click(object sender, EventArgs e)
        {
            ConsultarPorCriterio();
        }

        private void CmdAgregar_Click(object sender, EventArgs e)
        {
            FrmUsuariosEd fue = new FrmUsuariosEd();
            fue.ShowDialog();
        }

        private void FrmUsuarios_Activated(object sender, EventArgs e)
        {
            ConsultarPorCriterio();
        }

        private void DgvUsuarios_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = this.DgvUsuarios.SelectedRows[0];
            Usuario usuario = new Usuario
            {
                IdUsuario = Int32.Parse(row.Cells[0].Value.ToString()),
                Nombre = row.Cells[1].Value.ToString(),
                Clave = row.Cells[2].Value.ToString(),
                Cedula = row.Cells[3].Value.ToString(),
                TipoUsuario = (int)row.Cells[8].Value,
                LimiteCredito = decimal.Parse(row.Cells[5].Value.ToString()),
                FechaRegistro = DateTime.Parse(row.Cells[6].Value.ToString()),
                Estado = (bool)row.Cells[7].Value
            };

            FrmUsuariosEd fue = new FrmUsuariosEd
            {
                Usuario = usuario
            };
            fue.ShowDialog();
        }

        private void ConsultarPorCriterio()
        {
            var usuarios = from em in entities.Usuarios
                           join tipo in entities.Tipo_Usuario
                           on em.TipoUsuario equals tipo.ID
                              where (em.IdUsuario.ToString().StartsWith(TxtDatoABuscar.Text) ||
                            em.Nombre.StartsWith(TxtDatoABuscar.Text) ||
                            em.Cedula.ToString().StartsWith(TxtDatoABuscar.Text) ||
                            em.LimiteCredito.ToString().StartsWith(TxtDatoABuscar.Text) ||
                            em.FechaRegistro.ToString().StartsWith(TxtDatoABuscar.Text)
                            )
                              select new 
                              { 
                                em.IdUsuario,
                                em.Nombre,
                                Clave = "*******",
                                em.Cedula,
                                tipo.Descripcion,
                                em.LimiteCredito,
                                em.FechaRegistro,
                                em.Estado,
                                em.TipoUsuario
                              };
            DgvUsuarios.DataSource = usuarios.ToList();
            DgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DgvUsuarios.Columns[8].Visible = false;
            

        }

        private void DgvUsuarios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
